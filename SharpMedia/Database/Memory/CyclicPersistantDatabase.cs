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
using System.IO;
using System.Xml;
using SharpMedia.AspectOriented;

namespace SharpMedia.Database.Memory
{

    /// <summary>
    /// Cyclic persistant database is based on memory database. It is persisted
    /// into 2 files (along with XML configuration) to ensure data safety.
    /// </summary>
    /// <remarks>
    /// The filename is used to generate all three names:
    /// filename1.image
    /// filename2.image
    /// filename.xml
    /// </remarks>
    public class CyclicPersistantDatabase : IDatabase
    {
        #region Private Members
        string filename;
        uint current = 0;
        MemoryDatabase database;

        private void ReadXml()
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename + ".xml");
            XmlElement element = document.GetElementById("Database");
            current = uint.Parse(element.Value);
        }

        private void WriteXml()
        {
            XmlDocument document = new XmlDocument();
            XmlElement element = document.CreateElement("Database");
            element.Value = current.ToString();
            document.Save(filename + ".xml");
        }

        private CyclicPersistantDatabase(string filename, string name, Type type, StreamOptions ops)
        {
            this.filename = filename;
            this.database = MemoryDatabase.CreateNewDatabase(name, type, ops);
        }

        private CyclicPersistantDatabase(string filename)
        {
            if (File.Exists(filename + ".xml"))
            {
                throw new FileNotFoundException("Xml with name " + filename + ".xml must " +
                    "exist for database (if previously persisted).");
            }

            // We read it.
            database = MemoryDatabase.OpenDatabase(filename + current.ToString() + ".image");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the new database.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="ops">The ops.</param>
        /// <returns></returns>
        public static CyclicPersistantDatabase CreateNew([NotEmpty] string filename, 
            [NotEmpty] string name, [NotNull] Type type, StreamOptions ops)
        {
            return new CyclicPersistantDatabase(filename, name, type, ops);
        }

        /// <summary>
        /// Creates the new database.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static CyclicPersistantDatabase CreateNew([NotEmpty] string filename, [NotEmpty] string name)
        {
            return CreateNew(filename, name, typeof(object), StreamOptions.None);
        }

        /// <summary>
        /// Opens the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static CyclicPersistantDatabase Open([NotEmpty] string filename)
        {
            return new CyclicPersistantDatabase(filename);
        }

        /// <summary>
        /// Persists this instance.
        /// </summary>
        public void Persist()
        {
            // We first change current.
            if (current == 1) current = 0;
            else current = 1;

            // We write database first.
            MemoryDatabase.PersistToStream(filename + current.ToString() + ".image", database);

            // And update XML.
            WriteXml();
        }

        #endregion

        #region IDatabase Members

        public IDriverNode Root
        {
            get { return database.Root; }
        }

        public ulong FreeSpace
        {
            get { return database.FreeSpace; }
        }

        public ulong DeviceStorage
        {
            get { return database.DeviceStorage; }
        }

        #endregion
    }
}
