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
using SharpMedia.Loading;
using System.IO;

namespace SharpMedia.Database.Host
{
    /// <summary>
    /// A host database.
    /// </summary>
    public class HostDatabase : IDatabase
    {
        #region Private Members
        string directory;
        IDriverNode root;
        ILoadableFactory defaultFactory = new Loading.RawLoadableFactory();
        Dictionary<string, ILoadableFactory> extensions = new Dictionary<string, ILoadableFactory>();
        #endregion

        #region Internal Members

        /// <summary>
        /// Creates a driver node from a directory path
        /// </summary>
        /// <param name="parent">Parent node to associate</param>
        /// <param name="path">Host directory path to create the node from</param>
        /// <returns>Driver node for the host path</returns>
        internal IDriverNode NodeFromDirectory(IDriverNode parent, string path)
        {
            FileAttributes f = File.GetAttributes(path);

            if ((f & FileAttributes.Directory) != 0)
            {
                return new DirectoryNode(this, path);
            }
            else
            {
                return new FileNode(this, path);
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Extensions registry
        /// </summary>
        public Dictionary<string, ILoadableFactory> Extensions
        {
            get { return extensions; }
        }

        /// <summary>
        /// The default loading factory.
        /// </summary>
        public ILoadableFactory DefaultFactory
        {
            set { defaultFactory = value; }
            get { return defaultFactory; }
        }


        /// <summary>
        /// Gets extension implemented by factory.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public ILoadableFactory GetFactory(string extension)
        {
            if (extension.Length > 1 && extension[0] == '.')
            {
                extension = extension.Substring(1);
            }

            ILoadableFactory factory;
            if (extensions.TryGetValue(extension, out factory))
            {
                return factory;
            }
            return defaultFactory;
        }

        #endregion

        #region IDatabase Members

        public IDriverNode Root
        {
            get { return root; }
        }

        public ulong FreeSpace
        {
            get { return (ulong)new DriveInfo(directory).AvailableFreeSpace; }
        }

        public ulong DeviceStorage
        {
            get { return (ulong)new DriveInfo(directory).TotalSize; }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Creates a host database with only default raw loadable
        /// (always loading data as deserialization or byte[]).
        /// </summary>
        /// <param name="rootDir"></param>
        public HostDatabase(string rootDir)
        {
            this.directory = rootDir;
            this.root      = NodeFromDirectory(null, rootDir);
        }

        /// <summary>
        /// Creates host database with extensions.
        /// </summary>
        /// <param name="rootDir"></param>
        /// <param name="extensions"></param>
        public HostDatabase(string rootDir, params KeyValuePair<string, ILoadableFactory>[] extensions)
        {
            this.directory = rootDir;
            this.root = NodeFromDirectory(null, rootDir);
            foreach (KeyValuePair<string, ILoadableFactory> ex in extensions)
            {
                this.extensions.Add(ex.Key, ex.Value);
            }
        }

        #endregion

        
    }
}
