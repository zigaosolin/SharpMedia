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
    /// Contains a job factory and additionals
    /// </summary>
    internal sealed class JobFactory
    {
        private IJobFactory jobFactory;
        /// <summary>
        /// Job factory
        /// </summary>
        public IJobFactory IJobFactory
        {
            get { return jobFactory; }
            set { jobFactory = value; }
        }

        private DateTime lastUpdate;
        /// <summary>
        /// Last update time
        /// </summary>
        public DateTime LastUpdate
        {
            get { return lastUpdate; }
            set { lastUpdate = value; }
        }

        internal JobFactory(IJobFactory inner)
        {
            this.jobFactory = inner;
            this.lastUpdate = DateTime.Now;
        }
    }
}
