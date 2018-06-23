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
using SharpMedia.Loading;

namespace SharpMedia.Database.Host
{

    /// <summary>
    /// A file node.
    /// </summary>
    internal class FileNode : IDriverNode, IDriverTypedStream
    {
        #region Private Members

        string filePath;
        ILoadableFactory factory;

        #endregion

        public FileNode(HostDatabase db, string filePath) 
        {
            this.filePath = filePath;
            this.factory = db.GetFactory(Path.GetExtension(filePath));
            
        }

        #region IDriverNode Members

        public string Name
        {
            set
            { 
                // We compute new name.
                string newFilePath = Path.GetFileName(filePath);
                newFilePath = filePath.Substring(0, filePath.Length - newFilePath.Length - 1) + value;

                // We rename it.
                File.Move(filePath, newFilePath);
                filePath = newFilePath;
            }
        }

        public string DefaultType
        {
            get { return factory.LoadableType.FullName; }
        }

        public ulong ByteOverhead
        {
            get { return 0; }
        }

        public IDriverTypedStream GetTypedStream(OpenMode mode, string type)
        {
            return this;
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
            return null;
        }

        public IDriverNode CreateChild(string name, string defaultType, StreamOptions flags)
        {
            throw new InvalidOperationException("Host file does not support children.");
        }

        public void DeleteChild(string name)
        {
        }

        public void DeleteVersion(ulong version)
        {
        }

        public DateTime CreationTime
        {
            get { return File.GetCreationTime(this.filePath); }
        }

        public DateTime LastModifiedTime
        {
            get { return File.GetLastWriteTime(this.filePath); }
        }

        public DateTime LastReadTime
        {
            get { return File.GetLastAccessTime(this.filePath); }
        }

        public string PhysicalLocation
        {
            get { return filePath; }
        }

        public StringCollection TypedStreams
        {
            get { StringCollection str = new StringCollection(); str.Add(factory.LoadableType.FullName); return str; }
        }

        public StringCollection Children
        {
            get { StringCollection str = new StringCollection(); return str; }
        }

        public ulong[] AvailableVersions
        {
            get { return new ulong[] { 0 }; }
        }

        #endregion

        #region IDriverTypedStream Members

        public StreamOptions Flags
        {
            get { return StreamOptions.SingleObject | StreamOptions.AllowDerivedTypes; }
        }

        public string StreamType
        {
            get { return factory.LoadableType.FullName; }
        }

        public ulong GetByteSize(uint index)
        {
            return 0;
        }

        public ulong ByteSize
        {
            get { return 0; }
        }

        public ulong ByteOverheadSize
        {
            get { return 0; }
        }


        public uint[] ObjectLocations
        {
            get { return new uint[] { 0 }; }
        }

        public string GetObjectType(uint index)
        {
            return factory.LoadableType.FullName;
        }

     

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IDriverTypedStream Members

        public bool UsesRaw
        {
            get { return false; }
        }

        public object Read(uint index)
        {
            if (index != 0) return null;

            using (FileStream stream = File.OpenRead(filePath))
            {
                if (stream.Length == 0) return null;
                return factory.Load(stream);
            }
        }

        public object[] Read(uint startIndex, uint numObjects)
        {
            object[] ret = new object[numObjects];
            if (startIndex != 0) return ret;

            ret[0] = Read(0);
            
            return ret;
        }

        public void Write(uint index, string type, object obj)
        {
            if (index != 0) throw new InvalidOperationException("Host database provider does not allow write " +
                 "at arbitary location, only index 0 available.");

            using (FileStream stream = File.Create(filePath))
            {
                factory.Save(obj, stream);
            }
        }

        public void WriteObjects(uint index, string[] types, object[] objects)
        {
            if (index != 0 || objects.Length != 1) throw new InvalidOperationException("Host database provider does not allow multiple " +
                 "objects in stream.");

            Write(0, types[0], objects[0]);
        }

        public void InsertBefore(uint index, string type, object obj)
        {
            throw new InvalidOperationException("Host database provider does not allow 'insert before'");
        }

        public void InsertAfter(uint index, string type, object obj)
        {
            throw new InvalidOperationException("Host database provider does not allow 'insert after'");
        }

        public void Erase(uint index, uint numObjects, bool makeEmpty)
        {
            if (index != 0)
            {
                throw new InvalidOperationException("Only index 0 can be deleted in host database.");
            }

            File.WriteAllBytes(filePath, new byte[] { });
        }

        public uint Count
        {
            get 
            {
                using (FileStream s = File.OpenRead(filePath))
                {
                    if (s.Length > 0) return 1;
                }
                return 0;
            }
        }

        #endregion
    }
}
