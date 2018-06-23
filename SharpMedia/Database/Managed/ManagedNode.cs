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
using SharpMedia.Caching;
using System.Collections.Specialized;
using System.Threading;
using SharpMedia.AspectOriented;
using SharpMedia.Database.Indexing;
using SharpMedia.Database.Aspects;

namespace SharpMedia.Database.Managed
{

    /// <summary>
    /// A base node tag class that is used to represent base class.
    /// </summary>
    [Serializable]
    internal class BaseNodeTag
    {
        /// <summary>
        /// A link as string to base node. Link is always a full name.
        /// </summary>
        public string Link;

        /// <summary>
        /// The invalid version.
        /// </summary>
        public ulong Version = ulong.MaxValue;

        public override string ToString()
        {
            return Link + "@" + Version.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseNodeTag"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public BaseNodeTag(string s)
        {
            Link = s;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseNodeTag"/> class.
        /// </summary>
        /// <param name="s">The name of link.</param>
        /// <param name="version">The version.</param>
        public BaseNodeTag(string s, ulong version)
        {
            Link = s;
            Version = version;
        }
    }

    /// <summary>
    /// Object that holds index table.
    /// </summary>
    [Serializable]
    internal sealed class IndexTableTag
    {
        public IndexTable Table;

        public IndexTableTag(IndexTable table) { this.Table = table; }
    }

    /// <summary>
    /// Managed node is the only kind of node that can be accessed by user directly. We
    /// do this to provide default safety checks (database can imply better checks) and put
    /// most of the burden on the managed version of node rather then on multiple, specific
    /// versions of driver nodes. This means that managed node takes care of caching, checking,
    /// finding (deaper paths), thread safety ...
    /// </summary>
    /// <remarks>
    /// The speed considerations because we use multiple layers should be minimum because we
    /// only issue one more call. This, however, makes code more readable on both sides. Memory
    /// access using database (or even remotely) is considered preaty slow anyway, so this should
    /// not bring too much overhead.
    /// </remarks>
    internal class ManagedNode : MarshalByRefObject, INode, IDriverNode, IDisposable, IIndexAccess
    {
        #region Private Fields
        IDriverNode                     node;
        List<TypedStreamContainer>      typedStreams = new List<TypedStreamContainer>();
        ManagedCommonNode               common;
        ulong                           version = 0;
        bool                            isDisposed = false;
        IndexTable                      cachedTable;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the sync root.
        /// </summary>
        /// <value>The sync root.</value>
        private object SyncRoot
        {
            get
            {
                return common.SyncRoot;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is mount node.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is mount node; otherwise, <c>false</c>.
        /// </value>
        internal bool IsMountPoint
        {
            get
            {
                return common.IsMountPoint;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is transparent node, e.g. is
        /// managed (no internal node).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is transparent node; otherwise, <c>false</c>.
        /// </value>
        internal bool IsTransparentNode
        {
            get
            {
                return node == null;
            }
        }

        #endregion

        #region Managed Methods

        /// <summary>
        /// Sets as mount node.
        /// </summary>
        /// <param name="root">The root, can be null if reseting to non-mount node.</param>
        internal void SetAsMountNode(IDriverNode root)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                common.IsMountPoint = root != null;
                node = root;
            }
        }

        /// <summary>
        /// Finds the node and if it does not exist, it creates it.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Managed node at path (constructed).</returns>
        internal ManagedNode FindOrCreate(int i, string[] path)
        {
            // We resolved the whole path.
            if (path.Length == i) return this;

            lock (SyncRoot)
            {
                AssertNotDisposed();
                ManagedNode redirect = common.GetChild(node, path[i]);
                
                // We must create transparent node.
                if (redirect == null)
                {
                    redirect = common.CreateTransparent(path[i]);
                }

                // We remove processed string.
                return redirect.FindOrCreate(i+1, path);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new node.
        /// </summary>
        /// <returns></returns>
        internal static ManagedNode CreateNew(ManagedDatabaseManager manager)
        {
            ManagedCommonNode node = new ManagedCommonNode(null, "", null, manager);
            return node.CurrentVersion;
        }


        /// <summary>
        /// A version constructor.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="version">The version.</param>
        /// <param name="common">The common.</param>
        internal ManagedNode(IDriverNode node, ulong version, ManagedCommonNode common)
        {
            this.node = node;
            this.version = version;
            this.common = common;
        }


        #endregion

        #region Private Methods

        private void CacheIndexTable()
        {
            if (cachedTable != null) return;

            if (node.TypedStreams.Contains(typeof(IndexTableTag).FullName))
            {
                using (IDriverTypedStream stream =
                    node.GetTypedStream(OpenMode.Read, typeof(IndexTableTag).FullName))
                {

                    cachedTable = ((IndexTableTag) (stream.UsesRaw ? 
                        Common.DeserializeFromArray(stream.Read(0) as byte[]) : stream.Read(0))).Table;
                }
            }
        }

        private void FlushIndexTable()
        {
            if (cachedTable == null) return;

            using (IDriverTypedStream stream = node.GetTypedStream(OpenMode.Write, typeof(IndexTableTag).FullName))
            {
                IndexTableTag tag = new IndexTableTag(cachedTable);

                stream.Write(0, typeof(IndexTableTag).FullName, 
                    stream.UsesRaw ? (object)Common.SerializeToArray(tag) : tag);
            }
        }

        private void AddIndexedObject(Type type, object obj)
        {

        }

        private void RemoveIndexedObject(Type type, object obj)
        {

        }

        private void AssertNotDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("The INode was already disposed, e.g. it was release by force (delete operation).");
            }
        }

        private void AssertCurrentVersion()
        {
            lock (common.SyncRoot)
            {
                if (common.CurrentVersion != this)
                {
                    throw new InvalidOperationException("Cannot use method on non-current version.");
                }
            }
        }

        /// <summary>
        /// Caches base node, if not already cached.
        /// </summary>
        private ManagedNode CacheBase()
        {
            

            // We must quickly open default tag stream, if it exists.
            using (IDriverTypedStream stream = node.GetTypedStream(OpenMode.ReadWrite, typeof(BaseNodeTag).FullName))
            {

                // Make sure we proceed this read only once.
                if (stream == null)
                {
                    return null;
                }

                // Now we find the type.
                BaseNodeTag tag = stream.UsesRaw ? 
                    Common.DeserializeObject(stream.Read(0) as byte[]) as BaseNodeTag : stream.Read(0) as BaseNodeTag;

                // We find the link.
                ManagedNode baseCached = common.Manager.Find(tag.Link) as ManagedNode;

                // Avoid processing once more.
                if (baseCached == null)
                {
                    // We do not reset it because it can be mounted in future.
                    throw new NodeNotFoundException("The base node " + tag.ToString() + " could not be found.");
                }

                // Must also take version into account.
                if (tag.Version != ulong.MaxValue)
                {
                    baseCached = baseCached[tag.Version] as ManagedNode;
                    if (baseCached == null)
                    {
                        throw new NodeNotFoundException("The base node " + tag.ToString() + " could not be found.");
                    }
                }

                return baseCached;
            }
        }

        /// <summary>
        /// Helper; creates cached TS.
        /// </summary>
        private ManagedTypedStream GetCachedTypedStream(string t, OpenMode mode)
        {

            // First update all unused typed streams containers.
            typedStreams.RemoveAll(delegate(TypedStreamContainer c) {
                if (!c.IsAlive) return true;
                return false; 
            });

            // Foreach stream.
            foreach (TypedStreamContainer c in typedStreams)
            {
                if (c.Type == t)
                {
                    ManagedTypedStream stream = c.Create(mode);
                    if (stream != null) return stream;
                    break;
                }
            }

            // We have to create it.


            // We ask node to provide TS.
            IDriverTypedStream s = node.GetTypedStream(OpenMode.ReadWrite, t);
            if (s == null) return null;

            TypedStreamContainer cont = new TypedStreamContainer(t, s);
            
            // We add it in front, to make sure we first check this new type.
            typedStreams.Insert(0, cont);
            return cont.Create(mode);
        }


        #endregion

        #region INode Members

        public IDriverNode DriverAspect
        {
            get { return this; }
        }

        public string Name
        {
            get
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    return common.Name;
                }
            }
            set
            {
                // We make sure that it is valid.
                if (!PathHelper.ValidateName(value))
                {
                    throw new InvalidNameException("The name for node is not valid, cannot be renamed.");
                }

                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    // A no-op.
                    if (value == common.Name) return;

                    // We issue a rename.
                    lock (common.Parent.SyncRoot)
                    {
                        // We make sure no child with such name exists.
                        if (common.Parent.ExistsChild(value))
                        {
                            throw new InvalidNameException("Cannot be renamed to name " 
                                + value + "; it already exists.");
                        }

                        common.Parent.RenameChildNoLock(common.Name, this, value);
                        common.Name = value;
                        
                    }
                }
            }
        }

        public Type DefaultType
        {
            get 
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    if (node == null) return typeof(object);
                    return Type.GetType(node.DefaultType);
                }
            }
        }

        public ulong ByteOverhead
        {
            get 
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    if (node == null) return 0;
                    return node.ByteOverhead;
                }
            }
        }

        public ITypedStream OpenDefaultStream(OpenMode mode)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                if (mode != OpenMode.Read)
                {
                    AssertCurrentVersion();
                }

                if (node == null)
                {
                    throw new TypedStreamNotFoundException("Default typed stream does " + 
                        " not exist on transparent nodes.");
                }

                // We obtain the stream.
                ManagedTypedStream stream = GetCachedTypedStream(node.DefaultType, mode);
                return stream;
            }
        }

        public ITypedStream GetTypedStream(OpenMode mode, Type t)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                if (mode != OpenMode.Read) AssertCurrentVersion();

                // We first check streams in cache, then try to load.
                ManagedTypedStream stream = GetCachedTypedStream(t.FullName, mode);
                if (stream != null) return stream;

                // We can also check in base node.
                ManagedNode b = CacheBase();
                if (b != null)
                {
                    return b.GetTypedStream(mode, t);
                }
                return null;
            }
        }

        public void AddTypedStream(Type t, StreamOptions flags)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                AssertCurrentVersion();

                // We first check that it does not exist.
                ManagedTypedStream stream = GetCachedTypedStream(t.FullName, OpenMode.Read);
                if (stream != null)
                {
                    stream.Dispose();
                    throw new TypedStreamAlreadyExistsException("Typed stream " + 
                        t.FullName + " already exists.");
                }

                // We can add it.
                node.AddTypedStream(t.FullName, flags);

            }
        }

        public void RemoveTypedStream(Type t)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                AssertCurrentVersion();

                // We first make sure it exists.
                ManagedTypedStream stream = GetCachedTypedStream(t.FullName, OpenMode.Read);
                if (stream == null)
                {
                    throw new TypedStreamNotFoundException("Cannot remove unexisting typed stream.");
                }

                // We check it's usage, there must only one read stream opened.
                TypedStreamContainer container = typedStreams.Find(
                    delegate(TypedStreamContainer c)
                    {
                        if (c.Type == t.FullName) return true;
                        return false;
                    });

                // We obtained the container, make sure only one stream is using it.
                if (container.ReadCount != 1)
                {
                    stream.Dispose();
                    throw new InvalidOperationException("Cannot delete typed stream in usage.");
                }

                // We must dispose it, invalidating the container.
                stream.Dispose();

                // We can delete it.
                node.RemoveTypedStream(t.FullName);
            }
        }

        public void ChangeDefaultStream(Type t)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                AssertCurrentVersion();

                // We make sure the typed stream is valid.
                ManagedTypedStream ts = GetCachedTypedStream(t.FullName, OpenMode.Read);
                if (ts == null)
                {
                    throw new TypedStreamNotFoundException("The typed stream of type " + 
                        t.FullName + " does not exist in this node.");
                }

                // We are not using it anymore.
                ts.Dispose();

                // Change it.
                node.ChangeDefaultStream(t.FullName);
            }
        }

        public ulong Version
        {
            get 
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    return version;
                }
            }
        }

        public INode PreviousVersion
        {
            get 
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    return common.PreviousVersion(version);
                }
            }
        }

        public INode this[ulong version]
        {
            get 
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    return common.GetVersion(node, version);
                }
            }
        }

        public INode CreateNewVersion(Type defaultType, StreamOptions options)
        {
            if (defaultType == null) throw new ArgumentNullException("Default type must be non-null.");
            lock (SyncRoot)
            {
                AssertNotDisposed();

                // New version can be created from any version.
                return common.CreateNewVersion(node, defaultType.FullName, options);
            }

        }

        public INode Parent
        {
            get 
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    if (common.Parent == null) return null;
                    return common.Parent.CurrentVersion;
                }
            }
        }

        public INode Find(string inPath)
        {

            // First validate path.
            if (!PathHelper.ValidatePath(ref inPath))
            {
                throw new InvalidPathException("The path " + inPath + " is invalid.");
            }

            // A special case of root linking.
            if (inPath == PathHelper.Slash)
            {
                return common.Manager.Root;
            }

            // We do processing out of lock if possible.
            string part;
            string path = PathHelper.SeperatePath(inPath, out part);

            // We process root links.
            if (path == string.Empty)
            {
                if (part == null) return common.Manager.Root;
                return common.Manager.Find(part);
            }

            // We process back links.
            if (path == PathHelper.ParentLink)
            {

                // Cannot find it.
                if (common.Parent == null) return null;

                if (part == null)
                {
                    return common.Parent.CurrentVersion;
                }
                else
                {
                    return common.Parent.CurrentVersion.Find(part);
                }
            }

            lock (SyncRoot)
            {
                AssertNotDisposed();
                ManagedNode child = common.GetChild(node, path);

                // We check if child cannot be found.
                if (child == null)
                {
                    ManagedNode b = CacheBase();
                    if (b != null)
                    {
                        return b.Find(inPath);
                    }

                    // Cannot be found.
                    return null;
                }
                else
                {
                    // We return child if no more to search.
                    if (part == null) return child;
                    return child.Find(part);
                }
            }
        }

        public INode CreateChild(string name, Type defaultType, StreamOptions flags)
        {
            // Perform all argument checks.
            if (defaultType == null || name == null)
            {
                throw new ArgumentNullException("Name or default stream null.");
            }
            if (!PathHelper.ValidateName(name))
            {
                throw new ArgumentException("Invalid name " + name + " for child.");
            }

            lock (SyncRoot)
            {
                AssertNotDisposed();

                // We resolve child.
                int idx = name.LastIndexOf('/');
                if (idx > 0)
                {
                    // We split, resolve path and add child to it.
                    INode node = Find(name.Substring(0, idx));
                    if (node == null)
                    {
                        throw new InvalidPathException(string.Format("Path {0} is invalid", name));
                    }

                    return node.CreateChild(name.Substring(idx + 1), defaultType, flags);
                }
                else if (idx == 0)
                {
                    // We must handle this seperatelly, an abolute path.
                    return common.Manager.Create(name.Substring(1), defaultType, flags);
                }
                else
                {
                    // No relative name.
                    return common.CreateChild(node, name, defaultType.FullName, flags);
                }
            }
        }

        public void DeleteChild([NotEmpty] string name)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();

                // We resolve child.
                int idx = name.LastIndexOf('/');
                if (idx > 0)
                {
                    // We split, resolve path and add child to it.
                    INode node = Find(name.Substring(0, idx));
                    if (node == null)
                    {
                        throw new InvalidPathException(string.Format("Path {0} is invalid", name));
                    }

                    node.DeleteChild(name.Substring(idx + 1));
                }
                else if (idx == 0)
                {
                    // We must handle this seperatelly, an abolute path.
                    common.Manager.Root.DeleteChild(name.Substring(1));
                }
                else
                {
                    // No relative name.
                    common.DeleteChild(node, name);
                }
            }
        }

        public INode MoveTo([NotNull] INode n)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();

                // Implementation notes: can make it more efficient using direct in database
                // copy support.

                // For now dumb implementation.
                INode ret = CopyTo(n);

                // Deletes self.
                Parent.DeleteChild(Name);

                return ret;

            }
        }

        public INode CopyTo([NotNull] INode n)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();

                // Must ensure from the same db and other checks.
                ManagedNode dest = n as ManagedNode;


                // We check if we are not moving to child of our node.
                string pathTo = dest.Path;
                string pathFrom = Path;

                if (pathTo.Length > pathFrom.Length && pathTo.Substring(0, pathFrom.Length) == pathFrom)
                {
                    throw new InvalidOperationException(string.Format("Moving from path {0} to " +
                        "path {1} which is a child of path {0}", pathFrom, pathTo));
                }

                // We create it with default type.
                INode myCopy;
                using (ITypedStream defaultTS = OpenDefaultStream(OpenMode.Read))
                {
                    myCopy = dest.CreateChild(Name, defaultTS.StreamType, defaultTS.Flags);

                    // Copy data.
                    using (ITypedStream defaultCopyTS = myCopy.OpenDefaultStream(OpenMode.Write))
                    {
                        defaultTS.CopyTo(defaultCopyTS);
                    }
                }

                // All typed streams.
                foreach (Type t in TypedStreams)
                {
                    // Default type already handled.
                    if (t == DefaultType) continue;

                    using (ITypedStream ts = GetTypedStream(OpenMode.Read, t))
                    {
                        // Copy data.
                        myCopy.AddTypedStream(ts.StreamType, ts.Flags);

                        using (ITypedStream tsCopy = myCopy.GetTypedStream(OpenMode.Write, ts.StreamType))
                        {
                            ts.CopyTo(tsCopy);
                        }
                    }
                }
                

                // And all children at last.
                foreach (string s in Children)
                {
                    INode child = this.Find(s);
                    child.CopyTo(myCopy);
                }

                return myCopy;
            }
        }

        public void DeleteVersion(ulong version)
        {
            if (version == this.version)
            {
                throw new InvalidOperationException("Cannot delete referenced version.");
            }

            lock (SyncRoot)
            {
                AssertNotDisposed();
                common.DeleteVersion(node, version);
            }
        }

        public DateTime CreationTime
        {
            get 
            {
                lock (SyncRoot)
                {
                    if (node == null) return DateTime.MinValue;
                    return node.CreationTime;
                }
            }
        }

        public DateTime LastModifiedTime
        {
            get 
            {
                lock (SyncRoot)
                {
                    if (node == null) return DateTime.MinValue;
                    return node.LastModifiedTime;
                }
            }
        }

        public DateTime LastReadTime
        {
            get 
            {
                lock (SyncRoot)
                {
                    if (node == null) return DateTime.MinValue;
                    return node.LastReadTime;
                }
            }
        }

        public string PhysicalLocation
        {
            get 
            {
                lock (SyncRoot)
                {
                    if (node == null) return "RAM";
                    return node.PhysicalLocation;
                }
            }
        }

        public Type[] TypedStreams
        {
            get 
            { 
                lock(SyncRoot)
                {
                    if (node == null) return new Type[0];
                    StringCollection typeStrings = node.TypedStreams;

                    // Convert to types.
                    Type[] types = new Type[typeStrings.Count];
                    for (int i = 0; i < typeStrings.Count; i++)
                    {
                        Type t = Type.GetType(typeStrings[i]);
                        types[i] = t;

                        if (t == null)
                        {
                            throw new InvalidOperationException(
                                "Could not resolve type " + typeStrings[i] 
                                + ". Make sure it is loaded.");
                        }
                    }

                    return types;
                }
            }
        }

        public StringCollection Children
        {
            get 
            {
                lock (SyncRoot)
                {
                    StringCollection strings;
                    if (node != null)
                    {
                        strings = node.Children;
                    }
                    else
                    {
                        strings = new StringCollection();
                    }

                    // We add transparent and mount points.
                    common.AddTransparentAndMountPoint(strings);

                    return strings;
                }
            }
        }

        public ulong[] AvailableVersions
        {
            get 
            {
                lock (SyncRoot)
                {
                    if (node == null)
                    {
                        return new ulong[] { 1 };
                    }
                    else
                    {
                        return node.AvailableVersions;
                    }
                }
            }
        }

        public INode Base
        {
            get
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    return CacheBase();
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    AssertCurrentVersion();

                    

                    ManagedNode b = CacheBase();

                    // We now check what must we do.
                    if (value == null)
                    {
                        // We have to delete base.
                        if (b != null)
                        {
                            node.RemoveTypedStream(typeof(BaseNodeTag).FullName);
                        }
                    }
                    else
                    {
                        // We first make sure this link is valid and not cyclic.
                        INode n = value;
                        while (n != null)
                        {
                            if (((ManagedNode)n).common == this.common)
                            {
                                throw new InvalidOperationException("Cyclic link construction.");
                            }

                            n = n.Base;
                        }



                        // We check if we must create it.
                        if (b == null)
                        {
                            node.AddTypedStream(typeof(BaseNodeTag).FullName, StreamOptions.SingleObject);
                        }
                            
                        // We write in the stream.
                        using (IDriverTypedStream ts = node.GetTypedStream(OpenMode.ReadWrite, typeof(BaseNodeTag).FullName))
                        {
                            BaseNodeTag tag = new BaseNodeTag(common.Manager.FullPath(value));
                            ts.Write(0, typeof(BaseNodeTag).FullName, Common.SerializeToArray(tag));
                        }

                        
                    }

                }
            }
        }

        public INode BaseWithVersion
        {
            set 
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    AssertCurrentVersion();

                    ManagedNode b = CacheBase();

                    // We now check what must we do.
                    if (value == null)
                    {
                        // We have to delete base.
                        if (b != null)
                        {
                            node.RemoveTypedStream(typeof(BaseNodeTag).FullName);
                        }
                    }
                    else
                    {
                        // We first make sure this link is valid and not cyclic.
                        INode n = value;
                        while (n != null)
                        {
                            if (((ManagedNode)n).common == this.common)
                            {
                                throw new InvalidOperationException("Cyclic link construction.");
                            }

                            n = n.Base;
                        }



                        // We check if we must create it.
                        if (b == null)
                        {
                            node.AddTypedStream(typeof(BaseNodeTag).FullName, StreamOptions.SingleObject);
                        }

                        // We write in the stream.
                        using (IDriverTypedStream ts = node.GetTypedStream(OpenMode.ReadWrite, typeof(BaseNodeTag).FullName))
                        {
                            BaseNodeTag tag = new BaseNodeTag(common.Manager.FullPath(value), value.Version);
                            ts.Write(0, typeof(BaseNodeTag).FullName, Common.SerializeToArray(tag));
                        }

                    }
                }
            }
        }

        public string Path
        {
            get 
            {
                lock (SyncRoot)
                {
                    return common.Manager.FullPath(this);
                }
            }
        }

        #endregion

        #region IDriverNode Members

        string IDriverNode.DefaultType
        {
            get 
            {
                lock (SyncRoot)
                {
                    AssertNotDisposed();
                    if (node == null) return string.Empty;
                    return node.DefaultType;
                }
            }
        }

        public IDriverTypedStream GetTypedStream(OpenMode mode, string t)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                if (mode != OpenMode.Read) AssertCurrentVersion();

                // We first check streams in cache, then try to load.
                ManagedTypedStream stream = GetCachedTypedStream(t, mode);
                if (stream != null) return stream;

                // We can also check in base node.
                ManagedNode b = CacheBase();
                if (b != null)
                {
                    return b.GetTypedStream(mode, t);
                }
                return null;
            }
        }

        public void AddTypedStream(string t, StreamOptions flags)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                AssertCurrentVersion();

                // We first check that it does not exist.
                ManagedTypedStream stream = GetCachedTypedStream(t, OpenMode.Read);
                if (stream != null)
                {
                    stream.Dispose();
                    throw new TypedStreamAlreadyExistsException("Typed stream " +
                        t + " already exists.");
                }

                // We can add it.
                node.AddTypedStream(t, flags);

            }
        }

        public void RemoveTypedStream(string t)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                AssertCurrentVersion();

                // We first make sure it exists.
                ManagedTypedStream stream = GetCachedTypedStream(t, OpenMode.Read);
                if (stream == null)
                {
                    throw new TypedStreamNotFoundException("Cannot remove unexisting typed stream.");
                }

                // We check it's usage, there must only one read stream opened.
                TypedStreamContainer container = typedStreams.Find(
                    delegate(TypedStreamContainer c)
                    {
                        if (c.Type == t) return true;
                        return false;
                    });

                // We obtained the container, make sure only one stream is using it.
                if (container.ReadCount != 1)
                {
                    stream.Dispose();
                    throw new InvalidOperationException("Cannot delete typed stream in usage.");
                }

                // We must dispose it, invalidating the container.
                stream.Dispose();

                // We can delete it.
                node.RemoveTypedStream(t);
            }
        }

        public void ChangeDefaultStream(string t)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                AssertCurrentVersion();

                // We make sure the typed stream is valid.
                ManagedTypedStream ts = GetCachedTypedStream(t, OpenMode.Read);
                if (ts == null)
                {
                    throw new TypedStreamNotFoundException("The typed stream of type " +
                        t + " does not exist in this node.");
                }

                // We are not using it anymore.
                ts.Dispose();

                // Change it.
                node.ChangeDefaultStream(t);
            }
        }
            

        public IDriverNode GetVersion(ulong version)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();
                return common.GetVersion(node, version);
            }
        }

        public IDriverNode CreateNewVersion(string defaultType, StreamOptions flags)
        {
            lock (SyncRoot)
            {
                AssertNotDisposed();

                // New version can be created from any version.
                return common.CreateNewVersion(node, defaultType, flags);
            }
        }

        IDriverNode IDriverNode.Find(string path)
        {
            return Find(path) as IDriverNode;
        }

        public IDriverNode CreateChild(string name, string defaultType, StreamOptions flags)
        {
            // Perform all argument checks.
            if (defaultType == null || defaultType == string.Empty || name == null)
            {
                throw new ArgumentNullException("Name or default stream null.");
            }

            if (!PathHelper.ValidateName(name))
            {
                throw new ArgumentException("Invalid name " + name + " for child.");
            }

            lock (SyncRoot)
            {
                AssertNotDisposed();
                return common.CreateChild(node, name, defaultType, flags);
            }
        }

        StringCollection IDriverNode.TypedStreams
        {
            get 
            {
                lock (SyncRoot)
                {
                    if (node == null) return new StringCollection();
                    return node.TypedStreams;
                }
                 
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (SyncRoot)
            {
                isDisposed = true;
                node.Dispose();
                node = null;
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            lock(SyncRoot)
            {
                return Path + " (DefaultType: " + DefaultType + ")";
            }
        }

        #endregion

        #region IIndexAccess Members

        void IIndexAccess.Sync(bool repair)
        {
            lock (SyncRoot)
            {
                if (repair)
                {
                    throw new NotImplementedException();
                }

                FlushIndexTable();
            }
        }

        bool IIndexAccess.EnableIndexing
        {
            set 
            {
                lock (SyncRoot)
                {
                    if (value == false)
                    {
                        // We disable it.
                        cachedTable = null;

                        if (node.TypedStreams.Contains(typeof(IndexTableTag).FullName))
                        {
                            node.RemoveTypedStream(typeof(IndexTableTag).FullName);
                        }
                    }
                    else
                    {
                        CacheIndexTable();

                        if (!node.TypedStreams.Contains(typeof(IndexTableTag).FullName))
                        {
                            node.AddTypedStream(typeof(IndexTableTag).FullName, StreamOptions.SingleObject);
                        }
                    }
                }
            }
        }

        IndexTable IIndexAccess.IndexTable
        {
            get 
            {
                lock (SyncRoot)
                {
                    CacheIndexTable();
                    return cachedTable;
                }
            }
        }

        #endregion
    }

}
