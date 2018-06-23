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

namespace SharpMedia.Threading
{
    /// <summary>
    /// A CPU executable in single thread.
    /// </summary>
    /// <remarks>Usually added to IJob to </remarks>
    public interface ICPUExecutable
    {
        /// <summary>
        /// Is an execution pausable.
        /// </summary>
        bool IsPausable { get; }

        /// <summary>
        /// Starts execution.
        /// </summary>
        /// <param name="execId"></param>
        void Start();

        /// <summary>
        /// Pauses execution.
        /// </summary>
        /// <returns></returns>
        bool Pause();

        /// <summary>
        /// Aborts execution.
        /// </summary>
        /// <returns></returns>
        bool Abort();

        /// <summary>
        /// Resumes execution.
        /// </summary>
        void Resume();

        /// <summary>
        /// Is the execution done.
        /// </summary>
        bool IsDone { get; }
    }

    /// <summary>
    /// A CPU work unit.
    /// </summary>
    /// <remarks>CPU work unit is accessed by adding 
    /// ICPUExecutable jobs to be processed.</remarks>
    public interface ICPUWorkUnit : IWorkUnit
    {
        /// <summary>
        /// FPU performance metrics.
        /// </summary>
        Performance.FPUPerformance FPU { get; }

        /// <summary>
        /// Common instructions performance.
        /// </summary>
        Performance.InstructionPerformance Instructions { get; }

        /// <summary>
        /// Threads per-CPU unit calibration.
        /// </summary>
        uint ThreadCount { get; }

        /// <summary>
        /// Minimum number of threads per CPU work unit.
        /// </summary>
        uint MinThreadCount { get; set; }

        /// <summary>
        /// Maximum number of threads per CPU work unit.
        /// </summary>
        uint MaxThreadCount { get; set; }

        /// <summary>
        /// Execute the empty delegate
        /// </summary>
        /// <param name="del"></param>
        void Execute(ReturnsVoid del);
    }
}
