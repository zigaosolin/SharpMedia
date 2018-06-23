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
    /// Creates directory nodes from a list
    /// </summary>
    [Serializable]
    public class DirNodeCreate : ICommand
    {
        [NonSerialized]
        private InstallEnvironment environment = null;

        private string destinationListName;
        /// <summary>
        /// Name of the destination list
        /// </summary>
        public string DestinationListName
        {
            get { return destinationListName; }
            set { destinationListName = value; }
        }

        #region ICommand Members

        public void Execute(InstallEnvironment env)
        {
            environment = env;

            foreach (InstallDestination dest in env.DestinationFileLists[destinationListName])
            {
                dest.InstallEnvironment = environment;
                if (!dest.Exists) { using (dest.OpenForWriting<Object>()) { /* do nothing just create and close */ } }
            }
        }

        #endregion

        public DirNodeCreate() { }
        public DirNodeCreate(string destListName) { this.destinationListName = destListName; }
    }
}
