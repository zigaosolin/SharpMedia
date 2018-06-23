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

namespace SharpMedia.Components.Installation.Commands
{
    /// <summary>
    /// Fails the install process if an application instance is running
    /// </summary>
    [Serializable]
    public class FailIfRunning : ICommand
    {
        #region Command Members

        public void Execute(InstallEnvironment env)
        {
            foreach (Guid app in applicationIds)
            {
                if (appDirectory.FindInstances(app).Length != 0)
                {
                    throw new AbortInstallationException(
                        String.Format("Application {0} is running but shouldn't", app));
                }
            }
        }

        #endregion

        private Guid[] applicationIds;

        /// <summary>
        /// Applications that should not be running at the time of the installation
        /// </summary>
        public Guid[] ApplicationIds
        {
            get { return applicationIds; }
            set { applicationIds = value; }
        }

        private IApplicationInstances appDirectory;
        public IApplicationInstances AppDirectory
        {
            get { return appDirectory; }
            set { appDirectory = value; }
        }

        public FailIfRunning() { }
        public FailIfRunning(Guid appId) { this.applicationIds = new Guid[] { appId }; }
        public FailIfRunning(Guid[] appIds) { this.applicationIds = appIds; }
    }
}
