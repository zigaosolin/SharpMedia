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
using System.Collections.ObjectModel;

namespace SharpMedia.Threading
{
    /// <summary>
    /// An optimizer.
    /// </summary>
    public interface IOptimizer
    {
        /// <summary>
        /// Performs optimization for a given job
        /// </summary>
        /// <param name="job">Job</param>
        /// <param name="tcOptimization">Optimization API</param>
        /// <param name="tcParent">WU Query and job assign API</param>
        void PerformOptimization(IJob job, IThreadControlOptimization tcOptimization, IThreadControl tcParent);
    }

    /// <summary>
    /// Interface exposed to the optimizers
    /// </summary>
    public interface IThreadControlOptimization
    {
        /// <summary>
        /// Assign a job to the work unit
        /// </summary>
        /// <param name="workUnit">Work unit</param>
        /// <param name="job">Job</param>
        /// <param name="sureness">Sureness 1..0 inclusive</param>
        void Assign(IWorkUnit workUnit, IJob job, float sureness);
    }
}
