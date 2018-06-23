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
using System.Collections.Specialized;

namespace SharpMedia.Database.Memory
{
    /// <summary>
    /// Holds common (non-version dependant) data.
    /// </summary>
    [Serializable]
    internal class MemoryNodeCommon
    {
        #region Private Members
        string                          name;
        SortedList<ulong, MemoryNode>   versions = new SortedList<ulong,MemoryNode>();
        List<MemoryNodeCommon>          children = new List<MemoryNodeCommon>();
        ulong                           currentVersion = 1;
        #endregion

        #region Constructors

        public MemoryNodeCommon(string name, MemoryNode current)
        {
            this.name = name;
            this.versions[currentVersion] = current;
        }

        #endregion

        #region Methods

        public string Name
        {
            set
            {
                name = value;
            }
        }

        public MemoryNode Find(string name)
        {
            MemoryNodeCommon common = children.Find(delegate(MemoryNodeCommon n) { return n.name == name; });
            
            // Can return null, not child found.
            if (common == null) return null;

            return common.versions[common.currentVersion];
        }

        public void DeleteVersion(ulong version)
        {
            versions.Remove(version);
        }

        public MemoryNode GetVersion(ulong version)
        {
            MemoryNode value;
            if (versions.TryGetValue(version, out value))
            {
                return value;
            }
            return null;
        }

        public MemoryNode CreateNewVersion(string type, StreamOptions options)
        {
            MemoryNode prevVersionNode = versions[currentVersion];
            ++currentVersion;

            // Create new version.
            MemoryNode newVersion = new MemoryNode(currentVersion, type, options, this);
            versions.Add(currentVersion, newVersion);
            return newVersion;
        }

        public MemoryNode CreateChild(string name, string defaultType, StreamOptions flags)
        {
            MemoryNode node = new MemoryNode(name, defaultType, flags);
            children.Add(node.Common);
            return node;
        }

        public void DeleteChild(string name)
        {
            int index = children.FindIndex(delegate(MemoryNodeCommon c) 
            { if (c.name == name) return true; return false; });

            // Just dereference it.
            children.RemoveAt(index);
        }

        public StringCollection Children
        {
            get
            {
                StringCollection collection = new StringCollection();
                foreach (MemoryNodeCommon c in children)
                {
                    collection.Add(c.name);
                }
                return collection;
            }
        }

        public ulong[] AvailableVersions
        {
            get
            {
                ulong[] v = new ulong[versions.Count];
                int i = 0;
                foreach (KeyValuePair<ulong, MemoryNode> n in versions)
                {
                    v[i++] = n.Key;
                }

                return v;
            }
        }

        #endregion
    }
}
