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

namespace SharpMedia.Database.Query
{
    /// <summary>
    /// The query result, allows multi-threaded/asyc processing.
    /// </summary>
    public sealed class QueryResults<T> : IAsyncResult
    {
        #region Private Members
        ManualResetEvent manualEvent;
        Action<List<T>> action;
        volatile bool isComplete;
        List<T> result = new List<T>();

        void DoWork(object state)
        {
            action(result);
            isComplete = true;
            manualEvent.Set();
        }

        #endregion

        #region Constructors

        internal QueryResults(Action<List<T>> action)
        {
            // We set events.
            this.action = action;
            this.manualEvent = new ManualResetEvent(false);

            // We just quene work.
            ThreadPool.QueueUserWorkItem(DoWork);
        }

        #endregion

        #region IAsyncResult Members

        public object AsyncState
        {
            get { return isComplete; }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { return manualEvent; }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool IsCompleted
        {
            get { return isComplete; }
        }

        #endregion

        #region Result Members

        /// <summary>
        /// Results, if query is complete.
        /// </summary>
        public List<T> Results
        {
            get
            {
                if (isComplete)
                {
                    return result;
                }
                return null;
            }
        }

        #endregion
    }
}
