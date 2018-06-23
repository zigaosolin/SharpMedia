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
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;
using SharpMedia.Database.Managed;
using SharpMedia.Database.Indexing;
using SharpMedia.Database.Aspects;

namespace SharpMedia.Database
{


    /// <summary>
    /// Generic typed node in a database
    /// </summary>
    /// <typeparam name="T">The type of the default typed stream</typeparam>
    [Serializable]
    public class Node<T>
    {
        #region Private Members
        INode underNode;
        #endregion

        #region Helpers

        public override string ToString()
        {
            return this.Path;
        }

        private static bool ContainsType(INode iNode, Type tx)
        {
            if (tx == typeof(Object)) return true;

            foreach (Type t in iNode.TypedStreams)
            {
                if (t == null)
                {
                    throw new Exception("Internal Database Error");
                }

                if (Common.IsTypeSameOrDerived(t, tx))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ContainsDefaultType(INode iNode, Type tx)
        {
            if (tx == typeof(Object)) return true;

            return Common.IsTypeSameOrDerived(tx, iNode.DefaultType);
        }

        

        internal INode AsINode() {  return underNode; }

        /// <summary>
        /// Constructs a strongly typed node from an untyped node interface
        /// </summary>
        /// <param name="underNode">untyped node interfaceto wrap</param>
        internal Node(INode underNode)
        {
            if (ContainsDefaultType(underNode, typeof(T)))
            {
                this.underNode = underNode;
                return;
            }

            throw new Exception("Node does not contain type " + typeof(T));
        }


        void SearchNode<Other>(INode node, string pattern, List<Node<Other>> results)
        {
            // We first extract pattern.
            int idx = pattern.IndexOf('/');

            string matchValue;
            if (idx != -1)
            {
                matchValue = pattern.Substring(0, idx);
            }
            else
            {
                matchValue = pattern;
            }


            StringCollection children = node.Children;

            // We check if it is final.
            if (idx == -1)
            {

                for (int i = 0; i < children.Count; i++)
                {
                    if (Common.WildcardsMatch(matchValue, children[i]))
                    {
                        INode child = node.Find(children[i]);

                        // Type filtering.
                        if (!Common.IsTypeSameOrDerived(typeof(Other), child.DefaultType)) continue;

                        results.Add(new Node<Other>(node.Find(children[i])));
                    }
                }
            }
            else
            {
                string rest = pattern.Substring(idx + 1);

                for (int i = 0; i < children.Count; i++)
                {
                    if (Common.WildcardsMatch(matchValue, children[i]))
                    {
                        INode child = node.Find(children[i]);

                        SearchNode<Other>(child, rest, results);
                    }
                }
            }

            
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Automatic conversion to object.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static implicit operator Node<object>(Node<T> node)
        {
            return node.As<object>();
        }

        /// <summary>
        /// Converts to another Node type
        /// </summary>
        /// <typeparam name="TOther">Node type to convert to</typeparam>
        /// <returns>Converted node</returns>
        public Node<TOther> As<TOther>()
        {
            return new Node<TOther>(this.underNode);
        }

        /// <summary>
        /// Is it castable to Node type.
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <returns></returns>
        public bool Is<TOther>()
        {
            return ContainsDefaultType(underNode, typeof(TOther));
        }

        /// <summary>
        /// Is it castable to Node type.
        /// </summary>
        public bool Is(Type t)
        {
            return ContainsDefaultType(underNode, t);
        }

        #endregion

        #region INode-like Members

        /// <summary>
        /// Adds a new typed stream to this node
        /// </summary>
        /// <typeparam name="TData">The type of the new typed stream</typeparam>
        /// <param name="flags">Flags of the new typed stream</param>
        public void AddTypedStream<TData>(StreamOptions flags)
        {
            underNode.AddTypedStream(typeof(TData), flags);
        }

        /// <summary>
        /// Adds a typed stream (non-generic version).
        /// </summary>
        /// <param name="type"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public void AddTypedStream(Type type, StreamOptions flags)
        {
            underNode.AddTypedStream(type, flags);
        }

        /// <summary>
        /// Does typed stream exist.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public bool TypedStreamExists<TData>()
        {
            return TypedStreamExists(typeof(TData));
        }

        /// <summary>
        /// Does the typed stream exist.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool TypedStreamExists(Type type)
        {
            foreach (Type t in underNode.TypedStreams)
            {
                if (t == type) return true;
            }
            return false;
        }

        /// <summary>
        /// Clears the typed stream.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        public void ClearTypedStream<TData>()
        {
            ClearTypedStream(typeof(TData));
        }

        /// <summary>
        /// Clears the typed stream.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        public void ClearTypedStream(Type type)
        {
            using (TypedStream<object> ts = Open(type, OpenMode.Write))
            {
                uint[] locations = ts.ObjectLocations;
                for (int i = 0; i < locations.Length; i++)
                {
                    ts.Erase(locations[i], 1, true);
                }
            }
        }

        /// <summary>
        /// Obtains an array of available versions of this node
        /// </summary>
        public ulong[] AvailableVersions
        {
            get { return underNode.AvailableVersions; }
        }

        /// <summary>
        /// Obtains or sets the Base node
        /// </summary>
        public Node<Object> Base
        {
            get
            {
                return new Node<Object>(underNode.Base);
            }
            set
            {
                underNode.Base = value.AsINode();
            }
        }

        /// <summary>
        /// Sets the versioned base node
        /// </summary>
        public Node<T> BaseWithVersion
        {
            set { underNode.BaseWithVersion = value.AsINode(); }
        }

        /// <summary>
        /// Number of bytes spent in overhead
        /// </summary>
        public ulong ByteOverhead
        {
            get { return underNode.ByteOverhead; }
        }

        /// <summary>
        /// Changes the default typed stream
        /// </summary>
        /// <typeparam name="TNew">Existing typed stream type that will become default</typeparam>
        /// <returns></returns>
        public Node<TNew> ChangeDefaultStream<TNew>()
        {
            underNode.ChangeDefaultStream(typeof(TNew));
            return new Node<TNew>(underNode);
        }

        /// <summary>
        /// Changes the default typed stream
        /// </summary>
        /// <typeparam name="TNew">Existing typed stream type that will become default</typeparam>
        /// <returns></returns>
        public Node<object> ChangeDefaultTypedStream(Type newType)
        {
            underNode.ChangeDefaultStream(newType);
            return new Node<object>(underNode);
        }

        /// <summary>
        /// Returns names of child nodes
        /// </summary>
        public StringCollection ChildNames
        {
            get { return underNode.Children; }
        }

        /// <summary>
        /// Returns child nodes as an array
        /// </summary>
        public Node<Object>[] Children
        {
            get
            {
                Node<Object>[] objects = new Node<object>[underNode.Children.Count];
                int i = 0;
                foreach(string name in underNode.Children) 
                {
                    objects[i++] = Find(name);
                }

                return objects;
            }
        }

        /// <summary>
        /// Children of specific type.
        /// </summary>
        /// <typeparam name="TSomething"></typeparam>
        /// <returns></returns>
        public Node<TSomething>[] ChildrenOfType<TSomething>()
        {
            List<Node<TSomething>> results = new List<Node<TSomething>>();

            foreach (Node<Object> n in Children)
            {
                if (ContainsDefaultType(n.AsINode(), typeof(TSomething)))
                {
                    results.Add(n.As<TSomething>());
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// Children with specific type (not necessarily their default).
        /// </summary>
        /// <typeparam name="TSomething"></typeparam>
        /// <returns></returns>
        public Node<object>[] ChildrenWithType<TSomething>()
        {
            List<Node<object>> results = new List<Node<object>>();

            foreach (Node<Object> n in Children)
            {
                if (ContainsType(n.AsINode(), typeof(TSomething)))
                {
                    results.Add(n);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// Creates a child node in this node
        /// </summary>
        /// <typeparam name="TNew">The default type of the new node</typeparam>
        /// <param name="name">The name of the new node</param>
        /// <param name="flags">Stream flags of the default stream of the new node</param>
        /// <returns>The new created node</returns>
        public Node<TNew> Create<TNew>(string name, StreamOptions flags)
        {
            return new Node<TNew>(underNode.CreateChild(name, typeof(TNew), flags));
        }

        /// <summary>
        /// Creates a child node.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Node<object> Create(string name, Type type, StreamOptions flags)
        {
            return new Node<object>(underNode.CreateChild(name, type, flags));
        }

        /// <summary>
        /// Creates a new version in this node
        /// </summary>
        /// <typeparam name="TNew">The new version default stream type</typeparam>
        /// <param name="options">Stream flags of the default stream of the new version</param>
        /// <returns>The new node version</returns>
        public Node<TNew> CreateNewVersion<TNew>(StreamOptions options)
        {
            return new Node<TNew>(underNode.CreateNewVersion(typeof(TNew), options));
        }

        /// <summary>
        /// Creates a new vesion in this node.
        /// </summary>
        /// <param name="defaultType"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Node<object> CreateNewVersion(Type defaultType, StreamOptions options)
        {
            return new Node<object>(underNode.CreateNewVersion(defaultType, options));
        }

        /// <summary>
        /// Point in time when this node was created
        /// </summary>
        public DateTime CreationTime
        {
            get { return underNode.CreationTime; }
        }

        /// <summary>
        /// Age of the node (time elapsed since creation)
        /// </summary>
        public TimeSpan Age
        {
            get { return DateTime.Now - CreationTime; }
        }

        /// <summary>
        /// The default type of this node
        /// </summary>
        public Type DefaultType
        {
            get { return underNode.DefaultType; }
        }

        /// <summary>
        /// Deletes a child by reference
        /// </summary>
        /// <typeparam name="TSomething">The type of the child</typeparam>
        /// <param name="node">Child node</param>
        public void DeleteChild<TSomething>(Node<TSomething> node)
        {
            underNode.DeleteChild(node.Name);
        }

        /// <summary>
        /// Deletes a child node by path
        /// </summary>
        /// <param name="name">Path to the child node to delete</param>
        public void Delete(string path)
        {
            underNode.DeleteChild(path);
        }

        /// <summary>
        /// Deletes a version by index
        /// </summary>
        /// <param name="version">Verison index to delete</param>
        public void DeleteVersion(ulong version)
        {
            underNode.DeleteVersion(version);
        }

        /// <summary>
        /// Finds a child node by name or path
        /// </summary>
        /// <param name="path">The name or path of the child node to find</param>
        /// <returns>The found node or null if it is not found</returns>
        public Node<Object> Find(string path)
        {
            INode node = underNode.Find(path);
            if (node == null) return null;
            return new Node<Object>(node);
        }

        /// <summary>
        /// Finds a child by name or path.
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public Node<TChild> Find<TChild>(string path)
        {
            Node<object> result = Find(path);
            if (result == null) return null;

            return result.As<TChild>();
        }

        /// <summary>
        /// Opens a specific typed stream in this node
        /// </summary>
        /// <typeparam name="TSomething">The type of the stream to open</typeparam>
        /// <param name="mode">Opening mode to use</param>
        /// <returns>The opened typed stream</returns>
        public TypedStream<TSomething> Open<TSomething>(OpenMode mode)
        {
            ITypedStream s = underNode.GetTypedStream(mode, typeof(TSomething));
            if (s == null) return null;
            return new TypedStream<TSomething>(s);
        }

        /// <summary>
        /// Opens a specific typed stream cast to TSomething.
        /// </summary>
        /// <typeparam name="TSomething"></typeparam>
        /// <param name="type"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public TypedStream<TSomething> Open<TSomething>(Type type, OpenMode mode)
        {
            TypedStream<object> t = Open(type, mode);
            if(t == null) return null;
            return t.As<TSomething>();
        }

        /// <summary>
        /// Opens a specific typed stream as generic.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public TypedStream<object> Open(Type type, OpenMode mode)
        {
            ITypedStream s = underNode.GetTypedStream(mode, type);
            if (s == null) return null;
            return new TypedStream<object>(s);
        }

        /// <summary>
        /// Opens a stream for reading
        /// </summary>
        /// <typeparam name="TSomething">Type of the stream to open</typeparam>
        /// <returns>The opened typed stream</returns>
        public TypedStream<TSomething> OpenForReading<TSomething>()
        {
            return Open<TSomething>(OpenMode.Read);
        }

        /// <summary>
        /// Opens a stream for writing
        /// </summary>
        /// <typeparam name="TSomething">Type of the stream to open</typeparam>
        /// <returns>The opened typed stream</returns>
        public TypedStream<TSomething> OpenForWriting<TSomething>()
        {
            return Open<TSomething>(OpenMode.Write);
        }

        /// <summary>
        /// Opens a stream for reading and writing
        /// </summary>
        /// <typeparam name="TSomething">Type of the stream to open</typeparam>
        /// <returns>The opened typed stream</returns>
        public TypedStream<TSomething> OpenForReadingAndWriting<TSomething>()
        {
            return Open<TSomething>(OpenMode.ReadWrite);
        }

        /// <summary>
        /// Point in time when this node was last modified
        /// </summary>
        public DateTime LastModifiedTime
        {
            get { return underNode.LastModifiedTime; }
        }

        /// <summary>
        /// Time elapsed since last modification
        /// </summary>
        public TimeSpan SinceLastModification
        {
            get { return DateTime.Now - LastModifiedTime; }
        }

        /// <summary>
        /// Point in time when this node was last read from
        /// </summary>
        public DateTime LastReadTime
        {
            get { return underNode.LastReadTime; }
        }

        /// <summary>
        /// Time elapsed since the last read on this node
        /// </summary>
        public TimeSpan SinceLastRead
        {
            get { return DateTime.Now - LastReadTime; }
        }

        /// <summary>
        /// Gets or sets the name of this node. May not contain path element seperation characters.
        /// </summary>
        public string Name
        {
            get
            {
                return underNode.Name;
            }
            set
            {
                underNode.Name = value;
            }
        }

        /// <summary>
        /// Opens the default typed stream
        /// </summary>
        /// <param name="mode">The mode to use</param>
        /// <returns>The opened typed stream</returns>
        public TypedStream<T> Open(OpenMode mode)
        {
            return new TypedStream<T>(underNode.OpenDefaultStream(mode));
        }

        /// <summary>
        /// Opens the default typed stream for reading
        /// </summary>
        /// <param name="mode">The mode to use</param>
        /// <returns>The opened typed stream</returns>
        public TypedStream<T> OpenForReading()
        {
            return new TypedStream<T>(underNode.OpenDefaultStream(OpenMode.Read));
        }

        /// <summary>
        /// Opens the default typed stream for writing
        /// </summary>
        /// <param name="mode">The mode to use</param>
        /// <returns>The opened typed stream</returns>
        public TypedStream<T> OpenForWriting()
        {
            return new TypedStream<T>(underNode.OpenDefaultStream(OpenMode.Write));
        }

        /// <summary>
        /// Opens the default typed stream for reading and writing
        /// </summary>
        /// <returns>The opened typed stream</returns>
        public TypedStream<T> OpenForReadingAndWriting()
        {
            return new TypedStream<T>(underNode.OpenDefaultStream(OpenMode.ReadWrite));
        }

        /// <summary>
        /// Obtains the parent node
        /// </summary>
        public Node<Object> Parent
        {
            get { return new Node<Object>(underNode.Parent); }
        }

        /// <summary>
        /// Obtains the path 
        /// </summary>
        public string Path
        {
            get { return underNode.Path; }
        }

        public string PhysicalLocation
        {
            get { return underNode.PhysicalLocation; }
        }

        public Node<Object> PreviousVersion
        {
            get 
            { 
                INode prev = underNode.PreviousVersion;
                if (prev == null) return null;
                return new Node<object>(prev);
            }
        }

        public void RemoveTypedStream<TSomething>()
        {
            underNode.RemoveTypedStream(typeof(TSomething));
        }

        public void RemoveTypedStream(Type type)
        {
            underNode.RemoveTypedStream(type);
        }

        public Type[] TypedStreams
        {
            get { return underNode.TypedStreams; }
        }

        public ulong Version
        {
            get { return underNode.Version; }
        }

        public Node<Object> this[ulong version]
        {
            get { return new Node<Object>(underNode[version]); }
        }

        public T Object
        {
            get
            {
                using (ITypedStream ts = underNode.OpenDefaultStream(OpenMode.Read))
                {
                    T obj = (T)ts.Read(0);
                    return obj;
                }
            }
            set
            {
                using (ITypedStream ts = underNode.OpenDefaultStream(OpenMode.Write))
                {
                    ts.Write(0, value);
                }
            }
        }

        public T[] Array
        {
            get
            {
                using (ITypedStream ts = underNode.OpenDefaultStream(OpenMode.Read))
                {
                    T[] ret = new T[ts.Count];
                    object[] ret2 = ts.Read(0, (uint)ret.Length);
                    for (int i = 0; i < ret.Length; i++)
                    {
                        ret[i] = (T)ret2[i];
                    }

                    return ret;
                }
            }
            set
            {
                using (ITypedStream ts = underNode.OpenDefaultStream(OpenMode.Write))
                {
                    object[] packed = new object[value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                        packed[i] = value[i];
                    }

                    ts.WriteObjects(0, packed);
                }
            }
        }

        public ICollection<T> Collection
        {
            get
            {
                return new List<T>(Array);
            }
            set
            {
                T[] temp = new T[value.Count];
                value.CopyTo(temp, 0);
                Array = temp;
            }
        }

        #endregion

        #region Indexing

        /// <summary>
        /// Obtains index table associated with this node, if it exists.
        /// </summary>
        public IndexTable IndexTable
        {
            get
            {
                if (underNode is IIndexAccess)
                {
                    return (underNode as IIndexAccess).IndexTable;
                }
                return null;
            }
        }

        /// <summary>
        /// Enabling indexing may fail. You can check this by setting it first and then check if
        /// indexing was actually enabled.
        /// </summary>
        public bool EnableIndexing
        {
            get 
            {
                if (underNode is IIndexAccess)
                {
                    return (underNode as IIndexAccess).IndexTable != null;
                }
                return false;
            }
            set 
            {
                if (underNode is IIndexAccess)
                {
                    (underNode as IIndexAccess).EnableIndexing = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Repairs index - this may happen if node was closed incorrectly.
        /// </summary>
        public void RepairIndex()
        {
            if (underNode is IIndexAccess)
            {
                (underNode as IIndexAccess).Sync(true);
            }
        }

        /// <summary>
        /// Flushes the index.
        /// </summary>
        public void FlushIndex()
        {
            if (underNode is IIndexAccess)
            {
                (underNode as IIndexAccess).Sync(false);
            }
        }


        #endregion

        #region Additional Helpers

        /// <summary>
        /// Obtains the object of an alternate type than the default
        /// </summary>
        /// <typeparam name="OtherType">Other type</typeparam>
        public OtherType ObjectOfType<OtherType>()
        {
            using (TypedStream<OtherType> t = OpenForReading<OtherType>())
            {
                return t.Object;
            }
        }

        /// <summary>
        /// Obtains an array of an alternate type than the default
        /// </summary>
        /// <typeparam name="OtherType">Other type</typeparam>
        public OtherType[] ArrayOfType<OtherType>()
        {
            using (TypedStream<OtherType> t = OpenForReading<OtherType>())
            {
                if (t == null) return new OtherType[0];

                return t.Array;
            }
        }
        #endregion

        #region Searching

        /// <summary>
        /// Searches using a pattern, with T isolation.
        /// </summary>
        /// <param name="pattern">Can include wildcards.</param>
        /// <returns></returns>
        public Node<Other>[] Search<Other>(string pattern)
        {
            // We first extract split until '/'
            int idx = pattern.IndexOf('/');

            // If root redirect is used.
            if(idx == 0) {
                return Find("/").Search<Other>(pattern.Substring(1));
            }

            // We now extract what we must match against.
            List<Node<Other>> results = new List<Node<Other>>();
            SearchNode<Other>(this.underNode, pattern, results);

            return results.ToArray();
        }

        /// <summary>
        /// Searches using the pattern.
        /// </summary>
        /// <param name="pattern">Can include wildcards.</param>
        /// <returns></returns>
        public Node<object>[] Search(string pattern)
        {
            return Search<object>(pattern);
        }

        #endregion
    }

#if SHARPMEDIA_APIUSAGE

    class NodeAPITest
    {
        // tests implicit conversion from <object> to <T>
        void ApiUsage1()
        {
            Node<string> n1 = null;
            n1 = n1.Parent.As<string>();
        }

        // tests implicit conversion from <T1> to <T>
        void ApiUsage2()
        {
            Node<string> n1 = null;
            Node<float>  n2 = n1.As<float>();
            
        }

        void FictionalCharacterCounter()
        {
            Node<Object> parent = null;
            int chars = 0;
            foreach (Node<string> n in parent.ChildrenOfType<string>())
            {
                foreach (String s in n.OpenForReading().Array)
                {
                    chars += s.Length;
                }
            }
        }
    }
    
    
#endif
}
