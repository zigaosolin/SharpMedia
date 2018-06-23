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
    /// Options of job factory.
    /// </summary>
    [Flags]
    public enum FactoryOptions
    {
        None,

        /// <summary>
        /// A factory that never gets out of work.
        /// </summary>
        Continious = 1,

        /// <summary>
        /// Jobs are executed in time-spaced manner (pooling stuff).
        /// </summary>
        /// <remarks>This is a hint that will pool factory more frequently.</remarks>
        TimeSpaced = 2
    }

    /// <summary>
    /// A job factory.
    /// </summary>
    public interface IJobFactory
    {
        /// <summary>
        /// Factory options.
        /// </summary>
        FactoryOptions Options { get; }

        /// <summary>
        /// Creates jobs that must be executed. They will be executed in parallel.
        /// </summary>
        IJob[] CreateJobs(uint allowedConcurrency);

        /// <summary>
        /// Pool time for this factory.
        /// </summary>
        TimeSpan PollTime { get; }

        /// <summary>
        /// Current priority of job factory (may change over time).
        /// </summary>
        float CurrentPriority { get; }

        /// <summary>
        /// Progress of job factory.
        /// </summary>
        float Progress { get; }
    }
}
