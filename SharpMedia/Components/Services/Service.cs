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
using SharpMedia.Components.Applications;

namespace SharpMedia.Components.Services
{
    /// <summary>
    /// A Service descriptor
    /// </summary>
    [Serializable]
    public class Service : IServiceControl
    {
        [NonSerialized]
        IApplicationInstance instance = null;

        [NonSerialized]
        private IApplicationInstances appInstances;
        /// <summary>
        /// Application Instances
        /// </summary>
        public IApplicationInstances AppInstances
        {
            get { return appInstances; }
            set { appInstances = value; }
        }

        private string serviceName;
        /// <summary>
        /// The name of the service
        /// </summary>
        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        }

        private string applicationPath;
        /// <summary>
        /// The path to the application
        /// </summary>
        public string ApplicationPath
        {
            get { return applicationPath; }
            set { applicationPath = value; }
        }

        /// <param name="name">Name of the service</param>
        public Service(string name, bool autoStart) { this.serviceName = name; this.automaticStartup = autoStart; }

        #region IServiceControl Members

        public bool Stop()
        {
            if (instance != null && instance.IsRunning)
            {
                appInstances.Kill(instance);
                instance = null;
            }

            return !Started;
        }

        public bool Start()
        {
            if (instance == null || !instance.IsRunning)
            {
                instance = appInstances.Run(ApplicationPath, string.Empty, new string[0]);
            }

            return Started;
        }

        public bool Started
        {
            get { return instance != null && instance.IsRunning; }
        }

        bool automaticStartup;
        public bool AutomaticStartup
        {
            get
            {
                return automaticStartup;
            }
            set
            {
                automaticStartup = value;
            }
        }

        #endregion
    }
}
