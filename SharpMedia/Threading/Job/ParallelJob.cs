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

namespace SharpMedia.Threading.Job
{
    /// <summary>
    /// A Parallel job base class
    /// </summary>
    /// <remarks>
    /// When you implement methods that access this resource, be sure to 
    /// </remarks>
    public abstract class ParallelJob : Parallel
    {
        List<IJob> outstandingJobs = new List<IJob>();

        protected ParallelJob()
        {
            this.AutoThreadControlRegister = false;
            this.OnJobCreated += new Action<IJob>(ParallelJob_OnJobCreated);
        }

        void ParallelJob_OnJobCreated(IJob obj)
        {
            outstandingJobs.Add(obj);
        }

        protected ParallelJob(IThreadControl tcParent)
            : this()
        {
            this.ThreadControl = tcParent;
        }

        protected void Begin()
        {
            if (ThreadControl == null)
            {
                throw new InvalidOperationException("Not bound to ThreadControl");
            }

            ThreadControl.Add(this);
        }

        protected void End()
        {
            if (ThreadControl == null)
            {
                throw new InvalidOperationException("Not bound to ThreadControl");
            }

            ThreadControl.Remove(this);
        }

        public void BlockUntilComplete()
        {
            outstandingJobs.RemoveAll(
                delegate(IJob job)
                {
                    job.BlockUntilComplete();
                    return true;
                });
        }
    }
}
