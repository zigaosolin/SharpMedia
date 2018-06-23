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
using SharpMedia.Math.Matrix;

namespace SharpMedia.Threading.Job
{
    /// <summary>
    /// A Job that may branch into sub jobs on request
    /// </summary>
    public abstract class BranchedJob : OneTimeJob, IJobFactory
    {
        TimeSpan       pollTime      = TimeSpan.FromMilliseconds(10);
        IThreadControl threadControl = null;
        bool           hasMore       = true;

        #region IJobFactory Members

        public FactoryOptions Options
        {
            get { return FactoryOptions.None; }
        }

        /// <summary>
        /// Call when processing is done
        /// </summary>
        protected void End()
        {
            threadControl.Remove(this);
        }

        /// <summary>
        /// Call when internal state is ready to receive more work
        /// </summary>
        protected void HasMoreWork()
        {
            hasMore = true;
        }

        public IJob[] CreateJobs(uint allowedConcurrency)
        {
            if (hasMore) return new IJob[] { this };
            return new IJob[0];
        }

        public TimeSpan PollTime
        {
            get { return pollTime; }
        }

        public float CurrentPriority
        {
            get { return priority; }
        }

        #endregion

        protected BranchedJob(IThreadControl parent)
        {
            this.threadControl = parent;
            this.threadControl.Add(this);
        }
    }

    /// <summary>
    /// A Job that may branch into sub jobs on request, and is estimatable
    /// </summary>
    public abstract class EstimatableBranchedJob : EstimatableOneTimeJob, IJobFactory
    {
        TimeSpan pollTime = TimeSpan.FromMilliseconds(10);
        IThreadControl threadControl = null;
        bool hasMore = true;

        #region IJobFactory Members

        public FactoryOptions Options
        {
            get { return FactoryOptions.None; }
        }

        /// <summary>
        /// Call when processing is done
        /// </summary>
        protected void End()
        {
            threadControl.Remove(this);
        }

        /// <summary>
        /// Call when internal state is ready to receive more work
        /// </summary>
        protected void HasMoreWork()
        {
            hasMore = true;
        }

        public IJob[] CreateJobs(uint allowedConcurrency)
        {
            if (hasMore) return new IJob[] { this };
            return new IJob[0];
        }

        public TimeSpan PollTime
        {
            get { return pollTime; }
        }

        public float CurrentPriority
        {
            get { return priority; }
        }

        #endregion

        protected EstimatableBranchedJob(IThreadControl parent)
        {
            this.threadControl = parent;
            this.threadControl.Add(this);
        }
    }

    class Demo : EstimatableBranchedJob
    {
        int low = 0, high = 0;
        int existingLow = 0, existingHigh = 0;
        Matrix4x4f[] array = new Matrix4x4f[0];

        protected override TimeSpan EstimateCPUTime(SharpMedia.Threading.Performance.InstructionPerformance instructions, SharpMedia.Threading.Performance.FPUPerformance fpu)
        {
            return TimeSpan.FromSeconds(
                1.0 * (low - existingLow + high - existingHigh) /
                fpu.MatrixMultiplies4D);
        }

        protected override void ExecuteJob()
        {
            for (int i = 0; i < (low - existingLow); ++i)
            {
                array[i] = /* calculate */ Matrix4x4f.Identity;
            }

            /** high left as an excercise to the reader */
        }

        public void RefineResults(int newLow, int newHigh)
        {
            if (low < newLow) newLow = low;
            if (high > newHigh) newHigh = high;

            Matrix4x4f[] temp = new Matrix4x4f[newHigh - newLow];
            array.CopyTo(temp, newLow - low);

            array = temp;

            existingLow = low; existingHigh = high;
            low = newLow; high = newHigh;

            this.HasMoreWork();
        }

        public Matrix4x4f[] Results
        {
            get { return array.Clone() as Matrix4x4f[]; }
        }

        protected Demo() : base(/* FIXME: obviously this has to be non-null to work */ null) { }

        public static void Test()
        {
            Demo demo = new Demo();
            demo.RefineResults(-20, 200);
            demo.BlockUntilComplete();

            Matrix4x4f someResult = demo.Results[43];
            
            demo.RefineResults(-500, 5000);
            demo.BlockUntilComplete();

            someResult = demo.Results[42];

            /* etc */
        }
    }
}
