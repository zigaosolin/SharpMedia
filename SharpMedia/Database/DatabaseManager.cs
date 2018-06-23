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
using SharpMedia.Database;
using SharpMedia.Database.Managed;
using SharpMedia.Database.Aspects;

namespace SharpMedia.Database
{


    /// <summary>
    /// A component aware Strongly Typed Database Manager implementation
    /// </summary>
    public sealed class DatabaseManager : MarshalByRefObject
    {
        #region Private Members
        IDatabaseManager manager;
        #endregion

        #region IDatabaseManager Members

        public Node<T> Create<T>(string path, StreamOptions flags)
        {
            return Root.Create<T>(path, flags);
        }

        public Node<Object> Create(string path, Type t, StreamOptions flags)
        {
            return Root.Create(path, t, flags);
        }

        public Node<T> Create<T>(string path)
        {
            return Create<T>(path, StreamOptions.None);
        }

        public Node<Object> Find(string path)
        {
            return Root.Find(path);
        }

        public Node<T> Find<T>(string path)
        {
            return Root.Find<T>(path);
        }

        public string FullPath<T>(Node<T> node)
        {
            return manager.FullPath(node.AsINode());
        }

        public void Mount(string path, IDatabase db)
        {
            manager.Mount(path, db);
        }

        public void UnMount(string path)
        {
            manager.UnMount(path);
        }

        /// <summary>
        /// Searches using a pattern, with T isolation.
        /// </summary>
        /// <param name="pattern">Can include wildcards.</param>
        /// <returns></returns>
        public Node<T>[] Search<T>(string pattern)
        {
            return Root.Search<T>(pattern);
        }

        /// <summary>
        /// Searches using the pattern.
        /// </summary>
        /// <param name="pattern">Can include wildcards.</param>
        /// <returns></returns>
        public Node<object>[] Search(string pattern)
        {
            return Root.Search(pattern);
        }

        #endregion

        #region IDatabase Members

        public ulong DeviceStorage
        {
            get { return manager.DeviceStorage; }
        }

        public ulong FreeSpace
        {
            get { return manager.FreeSpace; }
        }

        public Node<Object> Root
        {
            get { return new Node<Object>(manager.Root as INode); }
        }

        #endregion

        #region Constructors

        internal DatabaseManager(IDatabaseManager manager)
        {
            this.manager = manager;
        }

        public DatabaseManager()
        {
            this.manager = new ManagedDatabaseManager();
        }

        #endregion
    }
}
