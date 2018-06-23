// This file constitutes a part of the SharpMedia project, (c) 2007 by the SharpMedia team
// and is licensed for your use under the conditions of the NDA or other legally binding contract
// that you or a legal entity you represent has signed with the SharpMedia team.
// In an event that you have received or obtained this file without such legally binding contract
// in place, you MUST destroy all files and other content to which this lincese applies and
// contact the SharpMedia team for further instructions at the internet mail address:
//
//    legal@sharpmedia.com
//
using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Components.Configuration;
using System.ComponentModel;
using System.Threading;
using SharpMedia.AspectOriented;
using SharpMedia.Components.Configuration.ComponentProviders;
using SharpMedia.Components.Configuration.ConfigurationValues;

namespace SharpMedia.Threading.Default
{
    /// <summary>
    /// A Thread Control class
    /// </summary>
    public sealed class ThreadControl : IThreadControl, IComponentPostInitialization, IDisposable
    {
        #region Internals

        // default: 10 job reschedules/second
        static readonly TimeSpan DefaultTimeSlice = TimeSpan.FromSeconds(1.0f/10);

        // number of worker threads per CPU core
        uint workerThreadsPerCore = 0;

        // list of jobs to schedule on next turn
        List<IJob> jobsToSchedule = new List<IJob>();

        // optimization API
        ThreadControlOptimization optimizing = null;

        // work unit registry
        IWorkUnitRegistry registry = null;

        // registered job factories
        List<JobFactory> factories = new List<JobFactory>();

        // control thread
        Thread controlThread = null;

        // current frequency the job scheduler is running at
        TimeSpan currentTimeSlice = DefaultTimeSlice;

        // timer that check for control thread running
        Timer controlThreadTimer = null;

        // timer locking object
        object timerLock = new object();

        // number of concurrent threads
        uint concurrencyCount = 1;

        // should job scheduling follow job factory poll level aggresively?
        bool aggresiveJobScheduling = true;

        // timer started method, goes to Controller thread on demand
        private void Schedule(object __unused)
        {
            if (jobsToSchedule.Count + factories.Count > 0)
            {
                Controller();
            }
        }

        #endregion

        #region Component Requirements
        private IComponentDirectory components;
        [Required]
        public IComponentDirectory Components
        {
            get { return components; }
            set { components = value; }
        }

        #endregion

        #region IThreadControl Members

        public IWorkUnitRegistry WorkUnits
        {
            get { return registry; }
        }

        public void Queue(IJob job)
        {
            lock (jobsToSchedule)
            {
                jobsToSchedule.Add(job);
            }
        }

        public void Add(IJobFactory factory)
        {
            lock (factories)
            {
                if (aggresiveJobScheduling)
                {
                    if (factory.PollTime < currentTimeSlice)
                    {
                        lock (timerLock)
                        {
                            currentTimeSlice   = factory.PollTime;
                            controlThreadTimer.Dispose();
                            controlThreadTimer = new Timer(Schedule, null, TimeSpan.Zero, currentTimeSlice);
                        }
                    }
                }

                factories.Add(new JobFactory(factory));
            }
        }

        public void Remove(IJobFactory factory)
        {
            lock (factories)
            {
                factories.RemoveAll(delegate(JobFactory f) { return f.IJobFactory == factory; });
            }
        }

        public void Register(IOptimizer optimizer)
        {
            optimizing.Optimizers.Add(optimizer);
        }

        public void UnRegister(IOptimizer optimizer)
        {
            optimizing.Optimizers.RemoveAll(delegate(IOptimizer xopt) { return xopt == optimizer; });
        }

        #endregion

        #region TC API

        /// <summary>
        /// Assigns a job to a working unit
        /// </summary>
        /// <param name="job">Job</param>
        /// <param name="wu">Work Unit</param>
        internal void Assign(IJob job, IWorkUnit wu)
        {
            /** we may be doing some additional hocus.pocus, but otherwise from that... */
            /** as soon as all the neccessary binds are completed, the job should start */
            job.Bind(wu);
        }

        /// <summary>
        /// Controller method, running in a thread
        /// </summary>
        internal void Controller()
        {
            /** process factories */
            lock (factories)
            {
                DateTime now = DateTime.Now;

                foreach (JobFactory fact in factories)
                {
                    /* Console.Out.WriteLine("Factory: {0}, LastUpdate: {1}, PollTime: {2}",
                        fact, fact.LastUpdate, fact.IJobFactory.PollTime); */

                    if (fact.LastUpdate + fact.IJobFactory.PollTime <= now)
                    {
                        fact.LastUpdate = now;

                        foreach (IJob job in fact.IJobFactory.CreateJobs(concurrencyCount))
                        {
                            Queue(job);
                        }
                    }
                }
            }

            /** optimize and start jobs */
            lock (jobsToSchedule)
            {
                optimizing.RunOptimizers();

                List<IJob> remove = new List<IJob>();
                foreach (IJob job in jobsToSchedule)
                {
                    // Console.Out.WriteLine("Job: {0}, Startable: {1}", job, job.Startable);
                    if (job.Startable)
                    {
                        job.Execute();

                        remove.Add(job);
                    }
                }

                foreach (IJob job in remove) { jobsToSchedule.Remove(job); }
            }
        }

        /// <summary>
        /// Jobs to be (re)scheduled
        /// </summary>
        internal List<IJob> JobsToSchedule
        {
            get { return jobsToSchedule; }
        }

        #endregion

        public TimeSpan PollInterval
        {
            get { return currentTimeSlice; }
            set
            {
                if (value != currentTimeSlice)
                {
                    lock (timerLock)
                    {
                        currentTimeSlice = value;
                        controlThreadTimer.Dispose();
                        controlThreadTimer = new Timer(Schedule, null, TimeSpan.Zero, currentTimeSlice);
                    }
                }
            }
        }

        public ThreadControl()
            : this(2)
        {
        }

        /// <summary>
        /// ThreadControl constructor
        /// </summary>
        /// <param name="workerThreadsPerCore">Number of worker threads per core</param>
        public ThreadControl([Required, DefaultValue(2), MinUInt(1)] uint workerThreadsPerCore)
        {
            this.optimizing = new ThreadControlOptimization(this);
            this.registry = new WorkUnitRegistry();
            this.workerThreadsPerCore = workerThreadsPerCore;
        }

        #region IComponentPostInitialization Members

        public void PostComponentInit(IComponentDirectory directory)
        {
            concurrencyCount = (uint)(System.Environment.ProcessorCount * workerThreadsPerCore);

            for (int i = 0; i < System.Environment.ProcessorCount; ++i)
            {
                CPUWorkUnit masterWorkUnit = null;

                for (int j = 0; j < workerThreadsPerCore; ++j)
                {
                    IComponentConfiguration config = new StandardConfiguration();
                    config.Values["CoreNumber"] = new Simple(i);
                    config.Values["SliceNumber"] = new Simple(j);
                    if (masterWorkUnit != null)
                    {
                        config.Values["Previous"] = new Simple(masterWorkUnit);
                    }
                    config.Values["SlicesPerCore"] = new Simple((int)workerThreadsPerCore);

                    ConfiguredComponent component = new ConfiguredComponent(
                        typeof(CPUWorkUnit).FullName,
                        config);

                    CPUWorkUnit wu =
                        Components.ConfigureInlineComponent(component) as CPUWorkUnit;

                    this.registry.Register(wu);

                    if (j == 0) masterWorkUnit = wu;
                }
            }

            controlThread = new Thread(Controller);

            controlThreadTimer =
                new Timer(Schedule, null, TimeSpan.Zero, DefaultTimeSlice);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            controlThreadTimer.Dispose();
            foreach(IWorkUnit wu in this.registry.All<ICPUWorkUnit>()) 
            {
                (wu as IDisposable).Dispose();
            }
        }

        #endregion
    }
}
