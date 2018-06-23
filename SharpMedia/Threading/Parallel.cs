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
using SharpMedia.Threading.Job;
using SharpMedia.Threading.Performance;

namespace SharpMedia.Threading
{
    /// <summary>
    /// A simple wrapper class around ThreadControl and job delegates
    /// </summary>
    public class Parallel : IParallel, IJobFactory
    {
        protected List<IJob> jobs = new List<IJob>();

        private IThreadControl threadControl;
        /// <summary>
        /// Thread Control with which to execute jobs
        /// </summary>
        [Required]
        public IThreadControl ThreadControl
        {
            get { return threadControl; }
            set
            {
                if (threadControl != null)
                {
                    if (autoThreadControlRegister)
                    {
                        threadControl.Remove(this);
                    }
                }

                threadControl = value;

                if (autoThreadControlRegister)
                {
                    threadControl.Add(this);
                }
            }
        }

        public event Action<IJob> OnJobCreated;

        private bool autoThreadControlRegister = true;

        /// <summary>
        /// Whether to register to the thread control automatically
        /// </summary>
        public bool AutoThreadControlRegister
        {
            get { return autoThreadControlRegister; }
            set { autoThreadControlRegister = value; }
        }

        #region IParallel Members

        public void Do(ReturnsVoid del)
        {
            jobs.Add(new DelegateJob(del));
        }

        public void Do(Action<object> del, object arg)
        {
            jobs.Add(new DelegateJob(del, arg));
        }

        public void For(int minimum, int maximum, Action<int> action)
        {
            if (maximum < minimum) { int t = maximum; maximum = minimum; minimum = t; }

            for (int i = minimum; i < maximum; i++)
            {
                Int32 temp = i;
                // Console.Out.WriteLine("Enqueue: {0}", i);
                jobs.Add(new DelegateJob(delegate() { action(temp); }));
            }
        }

        public void For(int minimum, int maximum, int skip, Action<int> action)
        {
            if (maximum < minimum) { int t = maximum; maximum = minimum; minimum = t; }

            if (skip < 0)
            {
                skip = -skip;
            }
            else if (skip == 0)
            {
                throw new ArgumentNullException("skip");
            }

            for (int i = minimum; i < maximum; i+=skip)
            {
                Int32 temp = i;
                jobs.Add(new DelegateJob(delegate() { action(temp); }));
            }
        }

        public void Do(ReturnsVoid del, JobEstimator estimator)
        {
            jobs.Add(new EstimatableDelegateJob(del, estimator));
        }

        public void Do(Action<object> del, object arg, ArgumentedJobEstimator estimator)
        {
            jobs.Add(new EstimatableDelegateJob(
                del, arg, 
                delegate(InstructionPerformance perf, FPUPerformance fpu) 
                {
                    return estimator(arg, perf, fpu); 
                }));
        }

        public void For(int minimum, int maximum, Action<int> action, IndexedJobEstimator estimator)
        {
            if (maximum < minimum) { int t = maximum; maximum = minimum; minimum = t; }

            for (int i = minimum; i < maximum; i++)
            {
                Int32 temp = i;
                // Console.Out.WriteLine("Enqueue: {0}", i);
                jobs.Add(new EstimatableDelegateJob(
                    delegate() { action(temp); },
                    delegate(InstructionPerformance perf, FPUPerformance fpu)
                    {
                        return estimator(temp, perf, fpu);
                    }));
            }
        }

        public void For(int minimum, int maximum, int skip, Action<int> action, IndexedJobEstimator estimator)
        {
            if (maximum < minimum) { int t = maximum; maximum = minimum; minimum = t; }

            if (skip < 0)
            {
                skip = -skip;
            }
            else if (skip == 0)
            {
                throw new ArgumentNullException("skip");
            }

            for (int i = minimum; i < maximum; i += skip)
            {
                Int32 temp = i;
                jobs.Add(new EstimatableDelegateJob(
                    delegate() { action(temp); },
                    delegate(InstructionPerformance perf, FPUPerformance fpu)
                    {
                        return estimator(temp, perf, fpu);
                    }));
            }
        }

        public void ForEach<T>(IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T obj in enumerable)
            {
                T temp = obj;
                jobs.Add(new DelegateJob(
                    delegate()
                    {
                        action(temp);
                    }));
            }
        }

        #endregion

        #region IJobFactory Members

        public FactoryOptions Options
        {
            get { return FactoryOptions.None; }
        }

        public IJob[] CreateJobs(uint allowedConcurrency)
        {
            IJob[] returns = jobs.ToArray();
            jobs.Clear();

            foreach (IJob job in jobs)
            {
                if (OnJobCreated != null)
                {
                    OnJobCreated(job);
                }
            }

            return returns;
        }

        public TimeSpan PollTime
        {
            get { return TimeSpan.FromMilliseconds(10); }
        }

        protected float priority = 0.0f;
        public float CurrentPriority
        {
            get { return priority; }
            set { priority = value; }
        }

        public float Progress
        {
            get { return System.Math.Max(100.0f - jobs.Count, 0.0f); }
        }

        #endregion
    }
}
