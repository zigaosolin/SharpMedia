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
using SharpMedia.Testing;
using SharpMedia.Database.Managed;

namespace SharpMedia.Database.Memory
{
    [Serializable]
    internal class MemoryNode : IDriverNode
    {
        #region Private members
        ulong                           version = 0;
        MemoryNodeCommon                common = null;
        MemoryTypedStream               defaultTypedStream = null;
        List<MemoryTypedStream>         typedStreams = new List<MemoryTypedStream>();
        DateTime readTime = DateTime.Now;
        DateTime createTime = DateTime.Now;
        DateTime modTime = DateTime.Now;

        void Touched()
        {
            readTime = DateTime.Now;
        }

        void Modified()
        {
            modTime = DateTime.Now;
        }

        #endregion

        #region Constructors

        public MemoryNode(string name, string defaultType, StreamOptions defaultStreamOptions)
        {
            common = new MemoryNodeCommon(name, this);
            defaultTypedStream = new MemoryTypedStream(defaultType, defaultStreamOptions);
            typedStreams.Add(defaultTypedStream);
        }

        public MemoryNode(ulong version, string defaultTyped, StreamOptions streamOptions, MemoryNodeCommon common)
        {
            this.common = common;
            this.version = version;
        }

        public MemoryNodeCommon Common
        {
            get { return common; }
        }

        #endregion

        #region IDriverNode Members

        public string Name
        {
            set
            {
                common.Name = value;
            }
        }

        public string DefaultType
        {
            get { return defaultTypedStream.StreamType; }
        }

        public ulong ByteOverhead
        {
            get { return 0; }
        }

        public IDriverTypedStream GetTypedStream(OpenMode mode, string type)
        {
            Touched();
            foreach (MemoryTypedStream s in typedStreams)
            {
                if (s.StreamType == type) return s;
            }
            return null;
        }

        public void AddTypedStream(string type, StreamOptions flags)
        {
            Modified();
            typedStreams.Add(new MemoryTypedStream(type, flags));
        }

        public void RemoveTypedStream(string type)
        {
            Modified();
            // Find it.
            int index = typedStreams.FindIndex(delegate(MemoryTypedStream s) 
            { if (s.StreamType == type) return true; return false; });

            // We remove it.
            typedStreams.RemoveAt(index);
        }

        public void ChangeDefaultStream(string type)
        {
            Modified();
            foreach (MemoryTypedStream t in typedStreams)
            {
                if (t.StreamType == type)
                {
                    defaultTypedStream = t;
                    return;
                }
            }
        }

        public ulong Version
        {
            get { return version; }
        }

        public IDriverNode GetVersion(ulong version)
        {
            return common.GetVersion(version);
        }

        public IDriverNode CreateNewVersion(string type, StreamOptions options)
        {
            return common.CreateNewVersion(type, options);
        }

        public IDriverNode Find(string path)
        {
            return common.Find(path);
        }

        public IDriverNode CreateChild(string name, string defaultType, StreamOptions flags)
        {
            return common.CreateChild(name, defaultType, flags);
        }

        public void DeleteChild(string name)
        {
            common.DeleteChild(name);
        }

        public void DeleteVersion(ulong version)
        {
            common.DeleteVersion(version);
        }

        public DateTime CreationTime
        {
            get { return createTime; }
        }

        public DateTime LastModifiedTime
        {
            get { return modTime; }
        }

        public DateTime LastReadTime
        {
            get { return readTime; }
        }

        public string PhysicalLocation
        {
            get { return "RAM"; }
        }

        public System.Collections.Specialized.StringCollection TypedStreams
        {
            get 
            { 
                StringCollection c = new StringCollection();
                foreach (MemoryTypedStream t in typedStreams)
                {
                    c.Add(t.StreamType);
                }
                return c;
            }
        }

        public System.Collections.Specialized.StringCollection Children
        {
            get { return common.Children; }
        }

        public ulong[] AvailableVersions
        {
            get { return common.AvailableVersions; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class MemoryNodeTest : NodeTest
    {
        DatabaseManager manager;

        protected override Node<object>  RootNode
        {
	        get 
            {
                if (manager != null) return manager.Find("/MountPoint");
                manager = new DatabaseManager();
                manager.Mount("/MountPoint", MemoryDatabase.CreateNewDatabase("MyDatabase"));
                return RootNode;
            }
        }

    }
#endif
}
