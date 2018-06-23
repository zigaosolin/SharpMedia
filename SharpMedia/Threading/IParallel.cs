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

namespace SharpMedia.Threading
{
    /// <summary>
    /// Extension of the JobEstimator that works on "indexed" jobs
    /// </summary>
    /// <param name="index">Job index</param>
    /// <param name="instructions">Instruction performance of the CPU</param>
    /// <param name="fpu">FPU performance</param>
    /// <returns>Time required for the job to complete</returns>
    public delegate TimeSpan IndexedJobEstimator(int index, InstructionPerformance instructions, FPUPerformance fpu);

    /// <summary>
    /// Extension of the JobEstimator that works on "indexed" jobs
    /// </summary>
    /// <param name="arg">Custom argument</param>
    /// <param name="instructions">Instruction performance of the CPU</param>
    /// <param name="fpu">FPU performance</param>
    /// <returns>Time required for the job to complete</returns>
    public delegate TimeSpan ArgumentedJobEstimator(object arg, InstructionPerformance instructions, FPUPerformance fpu);
    
    /// <summary>
    /// Interface for simple parallelization wrappers
    /// </summary>
    public interface IParallel
    {
        /// <summary>
        /// Execute a delegate at some point
        /// </summary>
        /// <param name="del">The delegate</param>
        void Do(ReturnsVoid del);

        /// <summary>
        /// Execute a delegate at some point, with an attached estimator of time required for it
        /// to complete
        /// </summary>
        /// <param name="del">The delegate</param>
        /// <param name="estimator">Estimator</param>
        void Do(ReturnsVoid del, Job.JobEstimator estimator);

        /// <summary>
        /// Execute a delegate at some point (with a custom argument)
        /// </summary>
        /// <param name="del">The delegate</param>
        /// <param name="arg">Argument</param>
        void Do(Action<Object> del, Object arg);

        /// <summary>
        /// Execute a delegate at some point (with a custom argument)
        /// </summary>
        /// <param name="del">The delegate</param>
        /// <param name="arg">Argument</param>
        /// <param name="estimator">Estimator</param>
        void Do(Action<Object> del, Object arg, ArgumentedJobEstimator estimator);

        /// <summary>
        /// Execute a delegate in parallel, with indices from minimum inclusive to maximum inclusive
        /// </summary>
        /// <param name="minimum">Minimum index</param>
        /// <param name="maximum">Maximum index</param>
        /// <param name="action">Action to execute</param>
        void For(int minimum, int maximum, Action<int> action);

        /// <summary>
        /// Execute a delegate in parallel, with indices from minimum inclusive to maximum inclusive
        /// </summary>
        /// <param name="minimum">Minimum index</param>
        /// <param name="maximum">Maximum index</param>
        /// <param name="action">Action to execute</param>
        /// <param name="estimator">Estimator</param>
        void For(int minimum, int maximum, Action<int> action, IndexedJobEstimator estimator);

        /// <summary>
        /// Execute a delegate in parallel, with indices from minimum inclusive to maximum (possibly inclusive)
        /// skipping in strides
        /// </summary>
        /// <param name="minimum">Minimum index</param>
        /// <param name="maximum">Maximum index</param>
        /// <param name="skip">Stride to skip when advancing the index</param>
        /// <param name="action">Action to execute</param>
        void For(int minimum, int maximum, int skip, Action<int> action);

        /// <summary>
        /// Execute a delegate in parallel, with indices from minimum inclusive to maximum (possibly inclusive)
        /// skipping in strides
        /// </summary>
        /// <param name="minimum">Minimum index</param>
        /// <param name="maximum">Maximum index</param>
        /// <param name="skip">Stride to skip when advancing the index</param>
        /// <param name="action">Action to execute</param>
        /// <param name="estimator">Estimator</param>
        void For(int minimum, int maximum, int skip, Action<int> action, IndexedJobEstimator estimator);

        /// <summary>
        /// Execute an action in parallel, with strongly typed objects taken from a collection
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="enumerable">Enumerable source of objects</param>
        /// <param name="action">Action to execute</param>
        void ForEach<T>(IEnumerable<T> enumerable, Action<T> action);
    }
}
