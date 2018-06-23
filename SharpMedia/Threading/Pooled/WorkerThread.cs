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
using System.Threading;

namespace SharpMedia.Threading.Pooled
{
    /// <summary>
    /// Worker thread for the Pooled ThreadControl
    /// </summary>
    internal class WorkerThread
    {
        Queue<IJob> jobQueue   = new Queue<IJob>();
        IJob        currentJob = null;
        CPUWorkUnit workUnit   = null;
        Thread      thread     = null;

        internal WorkerThread(CPUWorkUnit workUnit)
        {
            this.workUnit = workUnit;
            this.thread   = new Thread(WorkingMethod);
        }

        internal void AddJob(IJob job)
        {
            jobQueue.Enqueue(job);
            currentJob = jobQueue.Peek();
        }

        internal Queue<IJob> JobQueue
        {
            get { return jobQueue; }
        }

        internal void BeginProcessing()
        {
            thread.Start();
        }

        protected void WorkingMethod()
        {
            if (currentJob != null)
            {
                /* TODO: set CPU affinity */
                try
                {
                    Thread.BeginThreadAffinity();
                    if (currentJob.Begin() == JobResult.Normal)
                    {
                        jobQueue.Dequeue();
                        currentJob = jobQueue.Count != 0 ? jobQueue.Peek() : null;
                    }
                }
                finally
                {
                    Thread.EndThreadAffinity();
                }
            }
        }
    }
}
