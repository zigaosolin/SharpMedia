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
using System.Collections.Specialized;

namespace SharpMedia.Database.Host
{
    /// <summary>
    /// A directory node.
    /// </summary>
    internal class DirectoryNode : IDriverNode
    {
        string filePath;
        HostDatabase db;

        #region IDriverNode Members

        public string Name
        {
            set 
            {
                // We compute new name.
                string newFilePath = Path.GetFileName(filePath);
                newFilePath = filePath.Substring(0, filePath.Length - newFilePath.Length - 1) + value;

                // We rename it.
                Directory.Move(filePath, newFilePath);
                filePath = newFilePath;
            }
        }

        public string DefaultType
        {
            get { return typeof(Object).FullName; }
        }

        public ulong ByteOverhead
        {
            get { return 0; }
        }

        public IDriverTypedStream GetTypedStream(OpenMode mode, string type)
        {
            return new NullTypedStream();
        }

        public void AddTypedStream(string type, StreamOptions flags)
        {
            throw new NotSupportedException();
        }

        public void RemoveTypedStream(string type)
        {
            throw new NotSupportedException();
        }

        public void ChangeDefaultStream(string type)
        {
            throw new NotSupportedException();
        }

        public ulong Version
        {
            get { return 0; }
        }

        public IDriverNode GetVersion(ulong version)
        {
            if (version == 0) return this;
            throw new NotSupportedException();
        }

        public IDriverNode CreateNewVersion(string defaultType, StreamOptions flags)
        {
            throw new NotSupportedException();
        }

        public IDriverNode Find(string path)
        {
            return db.NodeFromDirectory(this, this.filePath + "\\" + path);
        }

        public IDriverNode CreateChild(string name, string defaultType, StreamOptions flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DeleteChild(string name)
        {
            string path = filePath + "/" + name;

            FileAttributes att = File.GetAttributes(path);
            if ((att & FileAttributes.Directory) != 0)
            {
                Directory.Delete(path);
            }
            else
            {
                File.Delete(path);
            }
        }

        public void DeleteVersion(ulong version)
        {
            throw new NotSupportedException();
        }

        public DateTime CreationTime
        {
            get { return File.GetCreationTime(filePath); }
        }

        public DateTime LastModifiedTime
        {
            get { return File.GetLastWriteTime(filePath); }
        }

        public DateTime LastReadTime
        {
            get { return File.GetLastAccessTime(filePath); }
        }

        public string PhysicalLocation
        {
            get { return filePath; }
        }

        public StringCollection TypedStreams
        {
            get { StringCollection str = new StringCollection(); str.Add(DefaultType); return str; }
        }

        public System.Collections.Specialized.StringCollection Children
        {
            get 
            {
                string[] files = Directory.GetFiles(filePath);
                StringCollection c = new StringCollection();
                for (int i = 0; i < files.Length; i++)
                {
                    c.Add(files[i].Substring(filePath.Length + 1));
                }

                string[] dir = Directory.GetDirectories(filePath);
                for(int i = 0; i < dir.Length; i++)
                {
                    c.Add(dir[i].Substring(filePath.Length + 1));
                }

                return c;
            }
        }

        public ulong[] AvailableVersions
        {
            get { return new ulong[] { 0 }; }
        }

        #endregion

        public DirectoryNode(HostDatabase db, string filePath) 
        {
            this.db = db;
            this.filePath = filePath;
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
