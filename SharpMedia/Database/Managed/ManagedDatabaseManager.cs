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
using SharpMedia.Testing;
using System.Runtime.Serialization;
using SharpMedia.AspectOriented;
using SharpMedia.Database.Aspects;

namespace SharpMedia.Database.Managed
{


    /// <summary>
    /// A database manager interface.
    /// </summary>
    internal interface IDatabaseManager : IDatabase
    {
        /// <summary>
        /// Mounts a database on specific path.
        /// </summary>
        /// <param name="path">The path. Path must be different from any already mounted path nodes.</param>
        /// <param name="db">IDatabase to mount on that node.</param>
        void Mount([NotNull] string path, [NotNull] IDatabase db);

        /// <summary>
        /// Unmounts from path. After this succeeds, the path will be unmounted if
        /// it was preiously mounted. 
        /// </summary>
        /// <param name="path">The path that should be unmounted.</param>
        void UnMount([NotNull] string path);

        /// <summary>
        /// Find a node at specific path.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <returns>Node, or null if not found.</returns>
        INode Find([NotNull] string path);

        /// <summary>
        /// Creates a node on the specific path. Path can be anything valid, with
        /// the last entry a unique child.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="defaultType">Default type of the node, cannot be null.</param>
        /// <param name="flags">Flags of the node to create.</param>
        /// <returns>Created node.</returns>
        INode Create([NotNull] string path, [NotNull] Type defaultType, StreamOptions flags);


        /// <summary>
        /// Returns full name of the node.
        /// </summary>
        /// <param name="node">The node, must be valid.</param>
        /// <returns>Full path name, not including version.</returns>
        string FullPath([NotNull] INode node);

    }

    /// <summary>
    /// IDatabase manager is a mounting point and is a managed implementation.
    /// </summary>
    internal class ManagedDatabaseManager : MarshalByRefObject, IDatabaseManager
    {
        #region Private Members

        /// <summary>
        /// The root node, or the "." entry point. Always managed, can also be a mount position.
        /// </summary>
        ManagedNode root;

        /// <summary>
        /// Mounts on this database manager.
        /// </summary>
        SortedList<string, IDatabase> mounts = new SortedList<string,IDatabase>();

        #endregion

        #region IDatabaseManager

        public INode Root
        {
            get { return root; }
        }

        public ulong FreeSpace
        {
            get 
            {
                ulong space = 0;
                foreach (KeyValuePair<string, IDatabase> data in mounts)
                {
                    space += data.Value.FreeSpace;
                }
                return space;
            }
        }

        public ulong DeviceStorage
        {
            get
            {
                ulong space = 0;
                foreach (KeyValuePair<string, IDatabase> data in mounts)
                {
                    space += data.Value.DeviceStorage;
                }
                return space;
            }
        }

        public void Mount(string path, IDatabase db)
        {
            lock (mounts)
            {
                ManagedNode node; 
                if(path == PathHelper.Slash) 
                {
                    node = root;
                }
                else
                {
                    path = path.Trim('/');
                    node = root.FindOrCreate(0, path.Split('/'));
                    if(node == null)
                    {
                        throw new MountException("Could not mount on path " + path 
                            + " because it is invalid.");
                    }
                }

                if(node.IsMountPoint)
                {
                    throw new MountException("Could not mount on path " + path 
                        + " because there is already a mount point.");
                }
                node.SetAsMountNode(db.Root);
                mounts.Add(path, db);
            }
        }

        public void UnMount(string path)
        {
            lock (mounts)
            {
                if (mounts.Remove(path))
                {
                    ManagedNode node = root.Find(path) as ManagedNode;
                    node.SetAsMountNode(null);
                }
                else
                {
                    throw new UnMountException("The path " + path + 
                        " is not a mount point, cannot be unmounted.");
                }
            }
        }

        public INode Find(string path)
        {
            // Sefety checks.
            if (path == null || path == string.Empty) return null;

            // We trim out path, 
            path = path.Trim('/');

            // Special case "" only
            if (path == string.Empty) return root;

            return root.Find(path);
        }

        public INode Create(string path, Type defaultType, StreamOptions flags)
        {
            return root.CreateChild(path, defaultType, flags);  
        }

        /// <summary>
        /// Computes full path of node.
        /// </summary>
        /// <param name="node">The node in question.</param>
        /// <returns>The full path, not including version.</returns>
        public string FullPath(INode node)
        {
            if (node == null) throw new ArgumentNullException("Cannot locate full path of null argument.");

            // We have special case root because it makes our loop better.
            if (node == root) return PathHelper.Slash;

            StringBuilder builder = new StringBuilder(80);
            while (node != root)
            {
                builder.Insert(0, PathHelper.Slash + node.Name);
                node = node.Parent;
            }
            return builder.ToString();
        }

        #endregion

        /// <summary>
        /// Database manager construction.
        /// </summary>
        public ManagedDatabaseManager()
        {
            root = ManagedNode.CreateNew(this);
        }

        #region IDatabase Members

        IDriverNode IDatabase.Root
        {
            get { return root; }
        }

        #endregion
    }
}
