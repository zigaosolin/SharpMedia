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
using SharpMedia.Components.Database;
using SharpMedia.Database;
using SharpMedia.Components.Documents;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Components.Installation.Commands
{
    /// <summary>
    /// Installs an application into the OS Application Directory
    /// </summary>
    [Serializable]
    public class InstallApplication : ICommand
    {
        public InstallApplication() { }

        #region Parameters
        private string sourceListName;
        public string SourceListName
        {
            get { return sourceListName; }
            set { sourceListName = value; }
        }

        private string destListName;
        public string DestinationListName
        {
            get { return destListName; }
            set { destListName = value; }
        }

        InstallEnvironment environment = null;

        [NonSerialized]
        private IDocumentManagementRegistry documentManagement;
        [Required]
        public IDocumentManagementRegistry DocumentManagement
        {
            get { return documentManagement; }
            set { documentManagement = value; }
        }

        #endregion

        #region Command Members

        public void Execute(InstallEnvironment env)
        {
            environment = env;

            if (!env.DestinationFileLists.ContainsKey(destListName))
            {
                throw new AbortInstallationException(
                    String.Format("Destination List {0} not found", destListName));
            }

            if (!env.DestinationFileLists.ContainsKey(sourceListName))
            {
                throw new AbortInstallationException(
                    String.Format("Source List {0} not found", sourceListName));
            }

            List<InstallDestination> dests   = env.DestinationFileLists[destListName];
            List<InstallSource>      sources = env.SourceFileLists[sourceListName];


            /* if the lengths are the same copy name to name */
            if (sources.Count == dests.Count)
            {
                List<InstallDestination>.Enumerator destinationEnumerator = dests.GetEnumerator();
                foreach (InstallSource source in sources)
                {
                    destinationEnumerator.MoveNext();

                    DoInstall(source, destinationEnumerator.Current);
                }
            }
            else
            {
                throw new AbortInstallationException("Cannot install sources to unequal number of destinations");
            }
        }

        #endregion

        private void DoInstall(InstallSource source, InstallDestination installDestination)
        {
            source.InstallEnvironment             = environment;
            installDestination.InstallEnvironment = environment;

            ApplicationDesc app = source.OpenForReading<ApplicationDesc>().Object;

            using (TypedStream<ApplicationDesc> appstream = installDestination.OpenForWriting<ApplicationDesc>())
            {
                appstream.Object = app;
            }

            /* register default bindings */
            foreach (string type in app.DefaultBindings.Keys)
            {
                documentManagement.RegisterDocumentUsage(
                    type, installDestination.ToString(), app.DefaultBindings[type]);
            }
        }
    }
}
