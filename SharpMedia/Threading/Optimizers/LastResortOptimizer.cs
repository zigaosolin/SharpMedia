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

namespace SharpMedia.Threading.Optimizers
{
    public class LastResortOptimizer : IOptimizer
    {
        #region IOptimizer Members

        public void PerformOptimization(IJob job, IThreadControlOptimization tcOptimization, IThreadControl tcParent)
        {
            foreach (Type wuType in job.WorkUnitTypes)
            {
                WorkUnitRequirements requirements = job.GetWorkUnitRequirements(wuType);

                if (wuType == typeof(ICPUWorkUnit))
                {
                    /* TODO: perform *real* CPU-level load balancing */
                    IWorkUnit selectedThread = tcParent.WorkUnits.FreeMost(wuType);
                    float sureness = 0.75f;

                    tcOptimization.Assign(selectedThread, job, sureness);
                }
                else
                {
                    /* assign to "default" with 0 sureness level.
                     * if a better optimizer will be smarter, it will override
                     * our "default" assign with a higher sureness number
                     */
                    tcOptimization.Assign(tcParent.WorkUnits.Default(wuType), job, 0);
                }
            }
        }

        #endregion
    }
}
