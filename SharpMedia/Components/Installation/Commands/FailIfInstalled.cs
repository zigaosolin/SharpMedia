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
using SharpMedia.Components.Configuration;

namespace SharpMedia.Components.Installation.Commands
{
    /// <summary>
    /// Fails the installation if a specified set of applications and/or packages is installed
    /// </summary>
    [Serializable]
    public class FailIfInstalled : ICommand
    {
        private Guid[] installedApplications = new Guid[0];

        /// <summary>
        /// Applications that must not be installed, or the installation fails
        /// </summary>
        public String[] InstalledApplications
        {
            get { return Array.ConvertAll<Guid, String>(installedApplications, delegate(Guid g) { return g.ToString(); }); }
            set 
            {
                Guid[] tmp = new Guid[value.Length];
                int i = 0;
                foreach (String s in value)
                {
                    tmp[i] = new Guid(s);
                }

                installedApplications = tmp;
            }
        }

        private Guid[] installedPackages = new Guid[0];

        /// <summary>
        /// Packages that must not be installed, or the installation fails
        /// </summary>
        public String[] InstalledPackages
        {
            get { return Array.ConvertAll<Guid, String>(installedPackages, delegate(Guid g) { return g.ToString(); }); }
            set
            {
                Guid[] tmp = new Guid[value.Length];
                int i = 0;
                foreach (String s in value)
                {
                    tmp[i] = new Guid(s);
                }

                installedPackages = tmp;
            }
        }

        public FailIfInstalled() { }
        public FailIfInstalled(Guid[] installedPackages) { this.installedPackages = installedPackages; }
        public FailIfInstalled(Guid[] installedPackages, Guid[] installedApplications)
        {
            this.installedPackages = installedPackages; this.installedApplications = installedApplications;
        }

        [NonSerialized]
        private IApplicationInstances appDirectory;
        [Required]
        public IApplicationInstances AppDirectory
        {
            get { return appDirectory; }
            set { appDirectory = value; }
        }

        [NonSerialized]
        private InstallationService installService;
        [Required]
        public InstallationService InstallationService
        {
            get { return installService; }
            set { installService = value; }
        }

        #region Command Members

        public void Execute(InstallEnvironment env)
        {
            foreach (Guid g in installedPackages)
            {
                if (installService.IsPackageInstalled(g))
                {
                    throw new Exception(
                        String.Format("Package {0} is installed", g));
                }
            }
        }

        #endregion
    }
}
