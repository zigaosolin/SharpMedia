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
using SharpMedia.Threading.Performance;

namespace SharpMedia.Threading.Job
{
    /// <summary>
    /// Abstract base class for jobs that execute once
    /// </summary>
    public abstract class OneTimeJob : DelegateJob
    {
        /// <summary>
        /// Job code resides in this method
        /// </summary>
        protected abstract void ExecuteJob();

        public OneTimeJob()
        {
            base.Delegate = Execute;
        }
    }

    /// <summary>
    /// Extension of OneTimeJob that estimates its execution duration
    /// </summary>
    public abstract class EstimatableOneTimeJob : OneTimeJob, IEstimatableJob
    {
        /// <summary>
        /// Estimate time required for the job to complete, given arguments
        /// </summary>
        /// <param name="instructions">Performance of the ALU</param>
        /// <param name="fpu">Performance of the FPU</param>
        /// <returns>Time required for job completition</returns>
        protected abstract TimeSpan EstimateCPUTime(InstructionPerformance instructions, FPUPerformance fpu);

        #region IEstimatableJob Members

        public TimeSpan Estimate(IWorkUnit[] executors)
        {
            foreach (IWorkUnit wu in executors)
            {
                if (wu.Type == typeof(ICPUWorkUnit))
                {
                    return EstimateCPUTime(
                        (wu as ICPUWorkUnit).Instructions,
                        (wu as ICPUWorkUnit).FPU);
                }
            }

            return TimeSpan.MinValue;
        }

        #endregion
    }
}
