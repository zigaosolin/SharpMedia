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
using System.Collections.ObjectModel;
using SharpMedia.Math;
using System.Threading;

namespace SharpMedia.Threading.Pooled
{
    internal class CPUCore
    {
        private List<WorkerThread> workerThreads = new List<WorkerThread>();
        public List<WorkerThread> WorkerThreads
        {
            get { return workerThreads; }
            set { workerThreads = value; }
        }

        private WorkUnit workUnit;
        public WorkUnit WorkUnit
        {
            get { return workUnit; }
            set { workUnit = value; }
        }

        internal CPUCore(WorkUnit wu, int concurrencyMultiplier)
        {
            this.workUnit = wu;
            for (int i = 0; i < concurrencyMultiplier; ++i)
            {
                this.workerThreads.Add(
                    new WorkerThread(workUnit as CPUWorkUnit));
            }
        }
    }

    internal class JobFactory
    {
        private IJobFactory factory;
        public IJobFactory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        private List<WorkerThread> lockedWorkerThreads = new List<WorkerThread>();
        public List<WorkerThread> LockedWorkerThreads
        {
            get { return lockedWorkerThreads; }
            set { lockedWorkerThreads = value; }
        }

        private List<WorkerThread> assignedWorkerThreads = new List<WorkerThread>();
        public List<WorkerThread> AssignedWorkerThreads
        {
            get { return assignedWorkerThreads; }
            set { assignedWorkerThreads = value; }
        }

        private DateTime lastOccurance;
        public DateTime LastOccurance
        {
            get { return lastOccurance; }
            set { lastOccurance = value; }
        }

        private ReadOnlyCollection<IJob> jobs;
        public ReadOnlyCollection<IJob> Jobs
        {
            get { return jobs; }
            set { jobs = value; }
        }

        private WorkUnitAllocation allocation;
        public WorkUnitAllocation Allocation
        {
            get { return allocation; }
            set { allocation = value; UpdateAllocations(); }
        }

        internal JobFactory(IJobFactory factory)
        {
            this.factory       = factory;
            this.lastOccurance = DateTime.Now;
            this.jobs          = factory.AvailableJobs;
        }

        private void UpdateAllocations()
        {
            /** first, see if there is any change */
        }
    }

    /// <summary>
    /// Thread Control implementation based on pooling worker threads across the working units
    /// </summary>
    public class ThreadControl : IThreadControl
    {
        List<CPUCore>                    cores           = new List<CPUCore>();
        List<JobFactory>                 factories       = new List<JobFactory>();
        Queue<IJobFactory>               changedQueue    = new Queue<IJobFactory>();
        Dictionary<Type, List<WorkUnit>> units           = new Dictionary<Type, List<WorkUnit>>();
        Thread                           schedulerThread = null;
        Dictionary<IJobFactory, WorkUnitAllocation> 
                                         allocs          = new Dictionary<IJobFactory, WorkUnitAllocation>();
        Timer                            updateTimer     = new Timer(Update);

        private void Scheduler()
        {
            while (changedQueue.Count != 0)
            {
                IJobFactory current = changedQueue.Dequeue();

                bool found = false;
                foreach (JobFactory fact in factories)
                {
                    if (fact.Factory == current)
                    {
                        fact.Allocation = allocs[current];
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    /* new one */
                    factories.Add(new JobFactory(current));
                }
            }
        }

        private void Update(object __unused_State)
        {
            if (schedulerThread == null)
            {
                schedulerThread = new Thread(Scheduler);
            }

            if (changedQueue.Count != 0 && !schedulerThread.IsAlive)
                schedulerThread.Start();
        }

        private TimeSpan calculatedPollInterval = TimeSpan.FromMilliseconds(10);
        private TimeSpan pollInterval           = TimeSpan.FromMilliseconds(10);
        public TimeSpan  PollInterval
        {
            get { return pollInterval; }
            set { pollInterval = value; }
        }

        public ThreadControl(TimeSpan pollInterval)
        {
            this.pollInterval = pollInterval;
            this.updateTimer.Change(TimeSpan.Zero, pollInterval);
        }

        #region IThreadControl Members

        public void Register(WorkUnit workUnit)
        {
            Type wuType = workUnit.GetType();
            if (!units.ContainsKey(wuType))
            {
                units[wuType] = new List<WorkUnit>();
            }

            units[wuType].Add(workUnit);
        }

        public ReadOnlyCollection<T> All<T>() where T : WorkUnit
        {
            return new ReadOnlyCollection<T>(
                units[typeof(T)]);
        }

        public T MostAvailable<T>() where T : WorkUnit
        {
            units[typeof(T)].Sort(delegate(WorkUnit a, WorkUnit b) { return a.Availability - b.Availability; });
            return Default<T>();
        }

        public T Default<T>() where T : WorkUnit
        {
            return units[typeof(T)][0];
        }

        public void Set(IJobFactory factory, WorkUnitAllocation allocation)
        {
            allocs[factory] = allocation;
            Update(null);
        }

        public ReadOnlyCollection<IJob> PendingJobs(WorkUnit unit)
        {
            List<IJob> results = new List<IJob>();

            if (unit.GetType() == typeof(CPUWorkUnit))
            {
                foreach (CPUCore core in cores)
                {
                    if (core.WorkUnit == unit)
                    {
                        foreach (WorkerThread wt in core.WorkerThreads)
                        {
                            foreach (IJob job in wt.JobQueue)
                            {
                                results.Add(job);
                            }
                        }

                        break;
                    }
                }
            }

            /** FIXME: no code for other WU types yet **/

            return new ReadOnlyCollection<IJob>(results);
        }

        public ReadOnlyCollection<IJobFactory> FactoriesUsing(WorkUnit unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void BlockUntilComplete(IJobFactory factory)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void BlockUntilComplete(IJob job)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
