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
using System.IO;
using SharpMedia.AspectOriented;

namespace SharpMedia.Database.Memory
{
    /// <summary>
    /// A memory database implementation.
    /// </summary>
    [Serializable]
    public class MemoryDatabase : IDatabase
    {
        #region Private Members
        MemoryNode rootNode;

        internal MemoryDatabase(string name, string type, StreamOptions ops)
        {
            rootNode = new MemoryNode(name, type, ops);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates the new database, using object type and no stream options.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static MemoryDatabase CreateNewDatabase(string name)
        {
            return new MemoryDatabase(name, typeof(object).FullName, StreamOptions.None);
        }

        /// <summary>
        /// Creates the new database.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type of default stream.</param>
        /// <param name="ops">The options of default stream.</param>
        /// <returns></returns>
        public static MemoryDatabase CreateNewDatabase(string name, Type type, StreamOptions ops)
        {
            return new MemoryDatabase(name, type.FullName, ops);
        }

        /// <summary>
        /// Opends a database previously persisted to stream.
        /// </summary>
        /// <param name="stream">The stream to serialize from.</param>
        /// <returns></returns>
        public static MemoryDatabase OpenDatabase(Stream stream)
        {
            return Common.DeserializeFromStream(stream) as MemoryDatabase;
        }

        /// <summary>
        /// Opens the database.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static MemoryDatabase OpenDatabase(string filename)
        {
            Stream stream = File.OpenRead(filename);
            try
            {
                return OpenDatabase(stream);
            }
            finally
            {
                stream.Dispose();
            }
        }

        /// <summary>
        /// Persists to stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="database">The database.</param>
        public static void PersistToStream(Stream stream, MemoryDatabase database)
        {
            Common.SerializeToStream(stream, database);
        }

        /// <summary>
        /// Persists to stream.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="database">The database.</param>
        public static void PersistToStream([NotEmpty] string filename, [NotNull] MemoryDatabase database)
        {
            Stream stream = File.Create(filename);
            try
            {
                PersistToStream(stream, database);
            }
            finally
            {
                stream.Dispose();
            }
        }

        /// <summary>
        /// Creates a temporary, inline typed stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TypedStream<T> CreateInlineTypedStream<T>(T value)
        {
            DatabaseManager mgr = new DatabaseManager();
            mgr.Mount("/", new MemoryDatabase("", typeof(T).FullName, StreamOptions.SingleObject|StreamOptions.AllowDerivedTypes));
            mgr.Root.As<T>().Object = value;

            return mgr.Root.OpenForReadingAndWriting<T>();
        }

        #endregion

        #region IDatabase Members

        public IDriverNode Root
        {
            get { return rootNode; }
        }

        public ulong FreeSpace
        {
            get { return 0; }
        }

        public ulong DeviceStorage
        {
            get { return 0; }
        }

        #endregion
    }
}
