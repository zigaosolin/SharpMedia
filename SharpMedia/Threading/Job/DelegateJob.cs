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
using System.Threading;

namespace SharpMedia.Threading.Job
{
    /// <summary>
    /// A job that executes a delegate
    /// </summary>
    public class DelegateJob : IJob
    {
        private Action<Object> argumentDelegate;
        private object runLock = new object();

        /// <summary>
        /// Delegate to call with an argument for a job
        /// </summary>
        public Action<Object> ArgumentDelegate
        {
            get { return argumentDelegate; }
            set { argumentDelegate = value; }
        }

        private Object argument;
        /// <summary>
        /// Argument to pass to the <see cref="ArgumentDelegate"/>
        /// </summary>
        public Object Argument
        {
            get { return argument; }
            set { argument = value; }
        }

        private ReturnsVoid func;
        /// <summary>
        /// No-Arguments delegate to call for a job
        /// </summary>
        public ReturnsVoid Delegate
        {
            get { return func; }
            set { func = value; }
        }

        private WorkUnitRequirements requirements;

        /// <summary>
        /// Work unit requirements
        /// </summary>
        public WorkUnitRequirements Requirements
        {
            get { return requirements; }
            set { requirements = value; }
        }

        // cpu work unit
        static readonly Type[] cpuTypes = { typeof(ICPUWorkUnit) };

        #region IJob Members

        public Type[] WorkUnitTypes
        {
            get { return cpuTypes; }
        }

        public WorkUnitRequirements GetWorkUnitRequirements(Type type)
        {
            if (type == typeof(ICPUWorkUnit))
            {
                return requirements;
            }

            return null;
        }

        public float GetOptimizerAffinity(IOptimizer optimizer)
        {
            /** no affinity */
            return 0.0f;
        }

        protected float priority = 0.0f;
        public float Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        protected bool running = false;
        protected ICPUWorkUnit boundTo = null;

        public void Execute()
        {
            lock (runLock)
            {
                if (running)
                {
                    throw new Exception("Job is already running");
                }

                if (boundTo == null)
                {
                    throw new ArgumentNullException("Bind");
                }

                if (argumentDelegate == null && func == null)
                {
                    throw new ArgumentNullException("Delegate");
                }

                running = true;
                try
                {
                    if (argumentDelegate != null)
                    {
                        boundTo.Execute(
                            delegate()
                            {
                                argumentDelegate(argument);
                            });
                    }
                    else
                    {
                        boundTo.Execute(func);
                    }
                }
                finally
                {
                    boundTo = null;
                    running = false;
                }
            }
        }

        public float Progress
        {
            get { return running ? 50.0f : 0.0f; }
        }

        public bool Startable
        {
            get { return !running && boundTo != null;  }
        }

        public void Bind(IWorkUnit wu)
        {
            if (wu.Type == typeof(ICPUWorkUnit))
            {
                boundTo = wu as ICPUWorkUnit;
            }
        }

        public void BlockUntilComplete()
        {
            while(!Startable) Thread.SpinWait(1000);
            lock (runLock)
            {
                return;
            }
        }


        #endregion

        // only subclasses may call the empty constructor
        protected DelegateJob() { }

        /// <param name="del">Delegate to use as job code</param>
        public DelegateJob(ReturnsVoid del)
        {
            Delegate = del;
        }

        /// <param name="del">Delegate to use as job code</param>
        /// <param name="arg">Argument to pass to the delegate</param>
        public DelegateJob(Action<Object> del, Object arg)
        {
            ArgumentDelegate = del;
            Argument         = arg;
        }
    }
}