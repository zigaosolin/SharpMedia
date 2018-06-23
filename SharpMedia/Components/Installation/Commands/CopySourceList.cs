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

namespace SharpMedia.Components.Installation.Commands
{
    /// <summary>
    /// Copies a list of sources to a destination
    /// </summary>
    [Serializable]
    public class CopySourceList : ICommand
    {
        [NonSerialized]
        private InstallEnvironment environment = null;

        private string sourceListName;
        /// <summary>
        /// Name of the source list
        /// </summary>
        public string SourceListName
        {
            get { return sourceListName; }
            set { sourceListName = value; }
        }

        private string destinationListName;
        /// <summary>
        /// Name of the destination list
        /// </summary>
        public string DestinationListName
        {
            get { return destinationListName; }
            set { destinationListName = value; }
        }

        public CopySourceList() { }
        public CopySourceList(string sourceListName, string destinationListName) 
        {
            this.sourceListName      = sourceListName;
            this.destinationListName = destinationListName;
        }

        #region Command Members

        public void Execute(InstallEnvironment env)
        {
            environment = env;

            List<InstallSource>      sources = env.SourceFileLists[sourceListName];
            List<InstallDestination> dests   = env.DestinationFileLists[destinationListName];

            if (sources == null)
            {
                throw new AbortInstallationException(
                    String.Format("Install Source List {0} not found", sourceListName));
            }

            if (dests == null)
            {
                throw new AbortInstallationException(
                    String.Format("Install Destination List {0} not found", destinationListName));
            }

            /* if the lengths are the same copy name to name */
            if (sources.Count == dests.Count)
            {
                List<InstallDestination>.Enumerator destinationEnumerator = dests.GetEnumerator();
                foreach (InstallSource source in sources)
                {
                    destinationEnumerator.MoveNext();
                    DoCopy(source, destinationEnumerator.Current);
                }
            }
            else
            {
                throw new AbortInstallationException("Cannot copy sources to unequal number of destinations");
            }
        }

        void DoCopy(InstallSource source, InstallDestination destination)
        {
            source.InstallEnvironment      = environment;
            destination.InstallEnvironment = environment;

            /* -- no, we don't do that anymore
            if (destination.Exists)
            {
                destination.Delete();
            }
             */

            foreach (Type t in source.Types)
            {
                // Console.Out.WriteLine("{0}[{1}] -> {1}", source, t.Name, destination);
                TypedStream<object> dstStream = destination.OpenForWriting(t);
                using (TypedStream<object> srcStream = source.OpenForReading(t))
                {
                    dstStream.Array = srcStream.Array;
                    dstStream.Dispose();
                }
            }
        }

        #endregion
    }
}
