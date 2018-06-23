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
using System.DirectoryServices;
using System.Collections.Specialized;

namespace SharpMedia.Database.Ldap
{


    /// <summary>
    /// A Ldap node.
    /// </summary>
    internal class LdapNode : IDriverNode
    {


        #region Private Members
        ILdapIdentifier identifier;
        DirectoryEntry directoryEntry;

        // Hierarchy.
        SortedDictionary<string, List<string>> typedStreamHeaders 
            = new SortedDictionary<string, List<string>>();

        #endregion

        #region Private Methods

        void Sync()
        {
            typedStreamHeaders.Clear();

            directoryEntry.RefreshCache();

            // We go for each property.
            foreach(string propertyName in directoryEntry.Properties.PropertyNames)
            {
                string key = identifier.Identify(propertyName);

                if (typedStreamHeaders.ContainsKey(key))
                {
                    typedStreamHeaders[key].Add(propertyName);
                }
                else
                {
                    typedStreamHeaders.Add(key, new List<string>(new string[] { propertyName }));
                }
            }
        }


        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="directoryEntry"></param>
        public LdapNode(DirectoryEntry directoryEntry, ILdapIdentifier identifier)
        {
            this.directoryEntry = directoryEntry;
            this.identifier = identifier;
        }

        public LdapNode(string path, ILdapIdentifier identifier)
        {
            DirectoryEntry entry = new DirectoryEntry(path);
            this.identifier = identifier;
        }

        #region IDriverNode Members

        public string Name
        {
            set 
            {
                directoryEntry.Rename(value);
            }
        }

        public string DefaultType
        {
            get 
            {
                return typeof(LdapEntry).FullName;
            }
        }

        public ulong ByteOverhead
        {
            get { return 0; }
        }

        public IDriverTypedStream GetTypedStream(OpenMode mode, string type)
        {
            return new LdapTypedStream(typedStreamHeaders[type], directoryEntry, identifier);
        }

        public void AddTypedStream(string type, StreamOptions flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveTypedStream(string type)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ChangeDefaultStream(string type)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ulong Version
        {
            get { return 1; }
        }

        public IDriverNode GetVersion(ulong version)
        {
            if (version == 1) return this;
            return null;
        }

        public IDriverNode CreateNewVersion(string defaultType, StreamOptions flags)
        {
            throw new NotSupportedException("Ldap does not support versions.");
        }

        public IDriverNode Find(string path)
        {
            DirectoryEntry child = directoryEntry.Children.Find(path);
            if (child != null)
            {
                return new LdapNode(child, identifier);
            }
            return null;
        }

        public IDriverNode CreateChild(string name, string defaultType, StreamOptions flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DeleteChild(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DeleteVersion(ulong version)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DateTime CreationTime
        {
            get { return DateTime.MinValue; }
        }

        public DateTime LastModifiedTime
        {
            get { return DateTime.MinValue; }
        }

        public DateTime LastReadTime
        {
            get { return DateTime.MinValue; }
        }

        public string PhysicalLocation
        {
            get { return directoryEntry.Path; }
        }

        public System.Collections.Specialized.StringCollection TypedStreams
        {
            get 
            {
                throw new Exception();
            }
        }

        public System.Collections.Specialized.StringCollection Children
        {
            get 
            {
                StringCollection r = new StringCollection();
                foreach (DirectoryEntry entry in directoryEntry.Children)
                {
                    r.Add(entry.Name);
                }

                return r;
            }
        }

        public ulong[] AvailableVersions
        {
            get { return new ulong[] { 1 }; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            directoryEntry.Dispose();
        }

        #endregion
    }
}
