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
    /// Estimates job time required for the CPU operations
    /// </summary>
    /// <param name="instructions">Instruction performance of the CPU</param>
    /// <param name="fpu">FPU performance</param>
    /// <returns>Time required for the job to complete</returns>
    public delegate TimeSpan JobEstimator(InstructionPerformance instructions, FPUPerformance fpu);

    /// <summary>
    /// Extension of the DelegateJob that allows for job estimation
    /// </summary>
    public class EstimatableDelegateJob : DelegateJob, IEstimatableJob
    {
        JobEstimator estimateJob = null;

        #region IEstimatableJob Members

        public TimeSpan Estimate(IWorkUnit[] executors)
        {
            foreach (IWorkUnit wu in executors)
            {
                if (wu.Type == typeof(ICPUWorkUnit))
                {
                    return estimateJob(
                        (wu as ICPUWorkUnit).Instructions,
                        (wu as ICPUWorkUnit).FPU);
                }
            }

            return TimeSpan.MinValue;
        }

        #endregion

        public EstimatableDelegateJob(ReturnsVoid del, JobEstimator jobEstimate) 
            : base(del)
        {
            this.estimateJob = jobEstimate;
        }
        public EstimatableDelegateJob(Action<Object> del, Object arg, JobEstimator jobEstimate)
            : base(del, arg) 
        {
            this.estimateJob = jobEstimate;
        }
    }
}
