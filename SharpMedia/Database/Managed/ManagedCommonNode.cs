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

namespace SharpMedia.Database.Managed
{

    /// <summary>
    /// This class is used by ManagedNode(s) and implements version unspecific data sharing.
    /// </summary>
    internal class ManagedCommonNode : IDisposable
    {
        #region Private Members
        bool                                mountPoint = false;
        object                              syncRoot = new object();
        ManagedCommonNode                   parent;
        SortedList<ulong, WeakReference>    versions = new SortedList<ulong,WeakReference>();
        ManagedNode                         currentVersion;
        SortedList<string, WeakReference>   children = new SortedList<string,WeakReference>();
        ManagedDatabaseManager              manager;
        string                              name = string.Empty;
        List<ManagedNode>                   transparentChildren; //< We have to have hard references to them

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public ManagedCommonNode Parent
        {
            get
            {
                return parent;
            }
        }

        /// <summary>
        /// Gets the current version.
        /// </summary>
        /// <value>The current version.</value>
        public ManagedNode CurrentVersion
        {
            get
            {
                return currentVersion;
            }
        }

        /// <summary>
        /// Gets the sync root.
        /// </summary>
        /// <value>The sync root.</value>
        public object SyncRoot
        {
            get
            {
                return syncRoot;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                lock(syncRoot)
                {
                    return name;
                }
            }
            set
            {
                name = value;
            }
        }

        public bool IsMountPoint
        {
            get
            {
                lock (syncRoot)
                {
                    return mountPoint;
                }
            }
            set
            {
                lock (syncRoot)
                {
                    mountPoint = value;
                }
            }
        }

        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <value>The manager.</value>
        public ManagedDatabaseManager Manager
        {
            get
            {
                return manager;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Previouses the version.
        /// </summary>
        /// <param name="thisVersion">The this version.</param>
        /// <returns></returns>
        public ManagedNode PreviousVersion(ulong thisVersion)
        {
            if (thisVersion == 0) return null;

            // We search for nearest previous version.
            for (ulong v = thisVersion - 1; v >= 0;)
            {
                WeakReference versionRef;
                if (versions.TryGetValue(v, out versionRef))
                {
                    ManagedNode prevVersion = versionRef.Target as ManagedNode;
                    if (prevVersion != null)
                    {
                        return prevVersion;
                    }
                }

                if (v == 0) break;
            }

            // No previous version.
            return null;
        }

        /// <summary>
        /// Finds a version.
        /// </summary>
        /// <param name="id">The version id.</param>
        /// <returns>Managed node around the version.</returns>
        public ManagedNode GetVersion(IDriverNode n, ulong id)
        {
            // We search in cachd first:
            WeakReference versionRef;
            if(versions.TryGetValue(id, out versionRef))
            {
                ManagedNode version = versionRef.Target as ManagedNode;
                
                // Can be dereferenced in between.
                if (version != null)
                {
                    return version;
                }
            }

            // We cannot create it if node does not exist.
            if (n == null)
            {
                return null;
            }

            // We read it from driver.
            IDriverNode vn = n.GetVersion(id);
            if (vn == null) return null;

            // We create it, add to cache and return it.
            ManagedNode versionNode = new ManagedNode(vn, id, this);
            versions[id] = new WeakReference(versionNode);
            return versionNode;
            
        }

        /// <summary>
        /// Creates the new version.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public ManagedNode CreateNewVersion(IDriverNode node, string type, StreamOptions options)
        {
            IDriverNode newVersionNode = node.CreateNewVersion(type, options);

            // We now add it to current.
            ulong ver = currentVersion.Version + 1;
            currentVersion = new ManagedNode(newVersionNode, ver, this);
            versions.Add(ver, new WeakReference(currentVersion));

            // And return it.
            return currentVersion;
        }

        /// <summary>
        /// We delete a specific version.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="version">The version.</param>
        public void DeleteVersion(IDriverNode node, ulong version)
        {
            if (currentVersion.Version == version)
            {
                throw new InvalidOperationException("Cannot delete current version.");
            }

            // We make sure we lose references.
            WeakReference versionRef;
            if (versions.TryGetValue(version, out versionRef))
            {
                ManagedNode v = versionRef.Target as ManagedNode;
                if (v != null)
                {
                    // Cannot be used anymore!
                    v.Dispose();
                }

                versions.Remove(version);
            }

            // And issue delete.
            node.DeleteVersion(version);
        }

        /// <summary>
        /// Adds the transparent and mount point.
        /// </summary>
        /// <param name="s">The s.</param>
        public void AddTransparentAndMountPoint(StringCollection s)
        {
            foreach (KeyValuePair<string, WeakReference> m in children)
            {
                ManagedCommonNode node = m.Value.Target as ManagedCommonNode;
                if (node != null)
                {
                    if (node.CurrentVersion.IsMountPoint || node.CurrentVersion.IsTransparentNode)
                    {
                        s.Add(m.Key);
                    }
                }
            }
        }

        /// <summary>
        /// Create a transparent node.
        /// </summary>
        /// <param name="name">The name of node.</param>
        /// <returns>The managed node.</returns>
        public ManagedNode CreateTransparent(string name)
        {
            ManagedCommonNode c = new ManagedCommonNode(this, name, null, manager);
            if (transparentChildren == null)
            {
                transparentChildren = new List<ManagedNode>();
            }
            transparentChildren.Add(c.CurrentVersion);
            children.Add(name, new WeakReference(c));
            return c.CurrentVersion;
        }

        /// <summary>
        /// Obtains child of this node.
        /// </summary>
        /// <param name="n">Internal node if not in cache, can be null for some nodes.</param>
        /// <param name="name">The name of child.</param>
        /// <returns></returns>
        public ManagedNode GetChild(IDriverNode n, string name)
        {

            ManagedCommonNode child;

            // We first search in children.
            WeakReference childRef;
            if (children.TryGetValue(name, out childRef))
            {
                // We extract child.
                child = childRef.Target as ManagedCommonNode;

                // It may still be null, if discared meanwhile.
                if (child != null)
                {
                    return child.currentVersion;
                }
            }

            if (n == null) return null;

            // We have to do the search in node.
            IDriverNode childNode = n.Find(name);

            // We cannot return it.
            if (childNode == null) return null;

            // Create the child, with the same name reference.
            child = new ManagedCommonNode(this, name, childNode, manager);

            // Add it to cache.
            children[name] = new WeakReference(child);

            // Return most current version.
            return child.currentVersion;
            
        }

        /// <summary>
        /// Renames the specified child.
        /// </summary>
        public void RenameChildNoLock(string name, ManagedNode child, string newName)
        {
            children.Remove(name);
            children.Add(newName, new WeakReference(child));
        }

        /// <summary>
        /// Existses the child.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool ExistsChild(string name)
        {
            return children.ContainsKey(name);
        }

        /// <summary>
        /// Creates the child.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultType">Type of the default.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public ManagedNode CreateChild(IDriverNode node, string name, 
            string defaultType, StreamOptions options)
        {
            // We first check if it exists already.
            WeakReference childRef;
            if (children.TryGetValue(name, out childRef))
            {
                if (childRef.IsAlive)
                {
                    throw new InvalidNameException("The child with name " + name + " already exists.");
                }
            }

            // We check node for such child.
            IDriverNode child = node.Find(name);
            if (child != null)
            {
                // Will not use child anymore.
                throw new InvalidNameException("The child with name " + name + " already exists.");
            }

            // We can create new child.
            IDriverNode driverChild = node.CreateChild(name, defaultType, options);
            ManagedCommonNode commonChildNode = new ManagedCommonNode(this, name, driverChild, manager);
            children.Add(name, new WeakReference(commonChildNode));

            // We return current version
            return commonChildNode.CurrentVersion;
        }

        /// <summary>
        /// Deletes the child.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        public void DeleteChild(IDriverNode node, string name)
        {
            // We first make sure node exists.
            WeakReference childRef;
            if (children.TryGetValue(name, out childRef))
            {
                // We must make sure it is not managed or transparent.
                ManagedCommonNode n = childRef.Target as ManagedCommonNode;
                if (n != null)
                {
                    if (n.CurrentVersion.IsMountPoint || n.CurrentVersion.IsTransparentNode)
                    {
                        throw new InvalidOperationException("Cannot delete a managed node " + name + 
                            ", it is needed for mounting.");
                    }

                    // Dispose all versions.
                    n.Dispose();
                }

                // Remove name.
                children.Remove(name);

            }
            // We check if not cached.
            else if (node.Find(name) == null)
            {
                throw new InvalidOperationException("The child with name " + name + " does not exist.");
            }

            // We can now delete it.
            node.DeleteChild(name);

        }

        #endregion

        #region Constructors

        public ManagedCommonNode(ManagedCommonNode parent, string name, 
            IDriverNode driverNode, ManagedDatabaseManager manager)
        {
            ulong version = 1;
            if(driverNode != null) version = driverNode.Version;

            this.parent = parent;
            this.name = name;
            this.manager = manager;
            this.currentVersion = new ManagedNode(driverNode, version, this);
            this.versions[version] = new WeakReference(this.currentVersion);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (SyncRoot)
            {
                foreach (KeyValuePair<ulong, WeakReference> v in versions)
                {
                    ManagedNode node = v.Value.Target as ManagedNode;
                    if (node != null)
                    {
                        node.Dispose();
                    }
                }
            }
        }

        #endregion
    }

}
