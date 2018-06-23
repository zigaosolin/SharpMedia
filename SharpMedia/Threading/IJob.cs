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
    /// Requirements of a job toward a work unit
    /// </summary>
    public class WorkUnitRequirements
    {
        private int minimumRequiredMemory;

        /// <summary>
        /// Minimum required memory, in bytes
        /// </summary>
        public int MinimumRequiredMemory
        {
            get { return minimumRequiredMemory; }
            set { minimumRequiredMemory = value; }
        }

        private int maximumRequiredMemory;

        /// <summary>
        /// Maximum required memory, in bytes
        /// </summary>
        public int MaximumRequiredMemory
        {
            get { return maximumRequiredMemory; }
            set { maximumRequiredMemory = value; }
        }
    }

    /// <summary>
    /// A job to be executed.
    /// </summary>
    /// <remarks>A job acts as a configurable unit of work that must be executed. Job is configured
    /// by thread control and then asked to execute itself.
    /// </remarks>
    public interface IJob
    {
        /// <summary>
        /// Work unit type support (CPU, GPU, ...).
        /// </summary>
        Type[] WorkUnitTypes { get; }

        /// <summary>
        /// Obtains requirements of the job towards a work unit type
        /// </summary>
        /// <param name="type">type of the work unit (typeof(CPUWorkUnit), ...)</param>
        /// <returns>Requirements, if any, or null</returns>
        WorkUnitRequirements GetWorkUnitRequirements(Type type);

        /// <summary>
        /// Obtains optimizer affinity.
        /// </summary>
        float GetOptimizerAffinity(IOptimizer optimizer);

        /// <summary>
        /// Priority of job [0,1].
        /// </summary>
        float Priority { get; }

        /// <summary>
        /// Executes the job using the allocation.
        /// </summary>
        void Execute();

        /// <summary>
        /// Async check for progress of job.
        /// </summary>
        /// <remarks>
        /// In percent
        /// </remarks>
        float Progress { get; }

        /// <summary>
        /// True if the job is startable now
        /// </summary>
        bool Startable { get; }

        /// <summary>
        /// Binds a working unit to the job
        /// </summary>
        /// <param name="wu">Work Unit</param>
        void Bind(IWorkUnit wu);

        /// <summary>
        /// Blocks the current caller thread until the job is complete
        /// </summary>
        void BlockUntilComplete();
    }

    /// <summary>
    /// An estimatable job
    /// </summary>
    public interface IEstimatableJob
    {
        /// <summary>
        /// Estimates execution time required on the given executor
        /// </summary>
        /// <param name="executor">Executing WorkUnits</param>
        /// <returns>time required</returns>
        TimeSpan Estimate(IWorkUnit[] executors);
    }
}
