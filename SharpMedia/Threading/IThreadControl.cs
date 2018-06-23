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
    /// A thread control.
    /// </summary>
    public interface IThreadControl
    {
        /// <summary>
        /// Work unit registry
        /// </summary>
        IWorkUnitRegistry WorkUnits
        {
            get;
        }

        /// <summary>
        /// Queues a job to thread control.
        /// </summary>
        /// <param name="job"></param>
        void Queue(IJob job);

        /// <summary>
        /// Adds the job factory.
        /// </summary>
        /// <param name="factory"></param>
        void Add(IJobFactory factory);

        /// <summary>
        /// Removes job factory.
        /// </summary>
        /// <param name="factory"></param>
        void Remove(IJobFactory factory);

        /// <summary>
        /// Registers an optimizer.
        /// </summary>
        /// <param name="optimizer"></param>
        void Register(IOptimizer optimizer);

        /// <summary>
        /// Unregister optimizer.
        /// </summary>
        /// <param name="optimizer"></param>
        void UnRegister(IOptimizer optimizer);

    }
}
