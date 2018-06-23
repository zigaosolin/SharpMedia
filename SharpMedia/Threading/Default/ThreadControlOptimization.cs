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

namespace SharpMedia.Threading.Default
{
    /// <summary>
    /// Controls the optimization process
    /// </summary>
    internal sealed class ThreadControlOptimization : IThreadControlOptimization
    {
        ThreadControl parent = null;

        Dictionary<IJob, List<KeyValuePair<IWorkUnit, float>>> assigns 
            = new Dictionary<IJob, List<KeyValuePair<IWorkUnit, float>>>();

        List<IOptimizer> optimizers = new List<IOptimizer>();

        #region IThreadControlOptimization Members

        public void Assign(IWorkUnit workUnit, IJob job, float sureness)
        {
            if (!assigns.ContainsKey(job))
            {
                assigns[job] = new List<KeyValuePair<IWorkUnit, float>>();
            }

            assigns[job].Add(new KeyValuePair<IWorkUnit, float>(workUnit, sureness));
        }

        #endregion

        #region TC API

        /// <summary>
        /// List of IOptimizer's used
        /// </summary>
        internal List<IOptimizer> Optimizers { get { return optimizers; } }

        /// <summary>
        /// Run the optimization process and decide on 
        /// </summary>
        internal void RunOptimizers() 
        {
            assigns.Clear();

            foreach (IOptimizer opt in optimizers)
            {
                foreach (IJob job in parent.JobsToSchedule)
                {
                    opt.PerformOptimization(job, this, parent);
                }
            }

            /** ok, assign! **/
            Dictionary<Type, KeyValuePair<IWorkUnit, float>> weightedAssigns = 
                new Dictionary<Type,KeyValuePair<IWorkUnit,float>>();

            foreach (IJob job in assigns.Keys)
            {
                weightedAssigns.Clear();

                assigns[job].ForEach(delegate(KeyValuePair<IWorkUnit, float> arg) 
                    {
                        if (!(weightedAssigns.ContainsKey(arg.Key.Type)) ||
                            weightedAssigns[arg.Key.Type].Value < arg.Value)
                        {
                            weightedAssigns[arg.Key.Type] = new KeyValuePair<IWorkUnit, float>(arg.Key, arg.Value);
                        }
                    });

                foreach (KeyValuePair<Type, KeyValuePair<IWorkUnit, float>> result in weightedAssigns)
                {
                    parent.Assign(job, result.Value.Key);
                }
            }
        }
        #endregion

        /// <summary>
        /// Thread control optimization constructor
        /// </summary>
        /// <param name="parent">Thread control parent</param>
        internal ThreadControlOptimization(ThreadControl parent)
        {
            this.parent = parent;
        }
    }
}
