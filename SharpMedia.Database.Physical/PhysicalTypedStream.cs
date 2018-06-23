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
using SharpMedia.Database.Physical.Caching;
using SharpMedia.Database.Physical.StorageStructs;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SharpMedia.Database.Physical.Operations;

namespace SharpMedia.Database.Physical
{
    /// <summary>
    /// A physical implementation of typed stream.
    /// </summary>
    public class PhysicalTypedStream : IDriverTypedStream
    {
        #region Cached Data
        Journalling.IJournal journal;
        ulong headerAddress;

        // Cached data.
        string type;
        StreamOptions options;
        ulong objectAddress;
        ulong objectSize;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalTypedStream"/> class.
        /// </summary>
        /// <param name="headerBlock">The header block.</param>
        /// <param name="journal">The journal.</param>
        public unsafe PhysicalTypedStream(ulong headerBlock, Journalling.IJournal journal)
        {
            this.headerAddress = headerBlock;
            this.journal = journal;

            // Cache data.
            Block block = journal.ReadService.Read(BlockType.TypedStreamHeader, headerBlock);
            fixed (byte* p = block.Data)
            {
                TypedStreamHeader* header = (TypedStreamHeader*)p;
                type = new string(header->Type);
                options = header->Options;
                objectAddress = header->ObjectsAddress;
                objectSize = header->ObjectSize;
            }
        }

        #endregion

        #region IDriverTypedStream Members

        public uint[] ObjectLocations
        {
            get
            {
                if ((options & StreamOptions.SingleObject) != 0)
                {
                    return new uint[] { 0 };
                } 

                BPlusTree tree = new BPlusTree(objectAddress);
                List<ObjectInfo> all = tree.ListAll(journal.ReadService);

                // We construct result.
                uint[] retVal = new uint[all.Count];
                for (int i = 0; i < all.Count; i++)
                {
                    retVal[i] = all[i].Index;
                }

                return retVal;
            }
        }

        public StreamOptions Flags
        {
            get { return options; }
        }

        public string StreamType
        {
            get { return type; }
        }

        public object Read(uint index)
        {
            if ((options & StreamOptions.SingleObject) != 0)
            {
                if (index != 0) return null;
                if (objectAddress == 0) return null;

                // We read it.
                BlockStream stream = BlockStream.FromBase(objectAddress, journal.ReadService);
                return stream.Read(objectSize);
            }
            else
            {
                // We have to find it in B+ tree first.
                BPlusTree tree = new BPlusTree(objectAddress);
                ObjectInfo info = tree.Find(journal.ReadService, index);
                if (info == null) return null;

                // We read it.
                BlockStream stream = BlockStream.FromBase(info.Address, journal.ReadService);
                return stream.Read(info.Size);
            }

        }

        public ulong GetByteSize(uint index)
        {
            if ((options & StreamOptions.SingleObject) != 0)
            {
                // We have single object stream.
                if (index != 0) return 0;
                return objectSize;
            }
            else
            {
                // We have to locate the object.
                BPlusTree tree = new BPlusTree(this.objectAddress);
                ObjectInfo info = tree.Find(journal.ReadService, index);
                if (info == null) return 0;
                return info.Size;
            }
        }

        public ulong ByteSize
        {
            get 
            {
                if ((options & StreamOptions.SingleObject) != 0)
                {
                    return objectSize;
                }
                else
                {
                    // We have to locate the object.
                    BPlusTree tree = new BPlusTree(this.objectAddress);
                    ulong size = 0;
                    List<ObjectInfo> all = tree.ListAll(journal.ReadService);
                    foreach (ObjectInfo info in all)
                    {
                        size += info.Size;
                    }
                    return size;
                }
            }
        }

        public ulong ByteOverheadSize
        {
            get 
            {
                if ((options & StreamOptions.SingleObject) != 0)
                {
                    // We compute the approximate.
                    return journal.ReadService.BlockSize + 
                        BlockHelper.ApproximateObjectOverhead(journal.ReadService.BlockSize, objectSize);
                }
                else
                {
                    // We have to locate the object.
                    BPlusTree tree = new BPlusTree(this.objectAddress);
                    
                    // 1) We take header.
                    ulong size = journal.ReadService.BlockSize;

                    // 2) We need B+ size.
                    ulong tsSize;
                    List<ObjectInfo> all = tree.ListAll(journal.ReadService, out tsSize);
                    size += tsSize;

                    // 3) We need all objects.
                    foreach (ObjectInfo info in all)
                    {
                        size += BlockHelper.ApproximateObjectOverhead(journal.ReadService.BlockSize, info.Size);
                    }
                    return size;
                }
            }
        }

        public object[] Read(uint startIndex, uint numObjects)
        {
            object[] data = new object[numObjects];
            for (uint i = 0; i < numObjects; i++)
            {
                data[i] = Read(startIndex + i);
            }
            return data;
        }

        public string GetObjectType(uint index)
        {
            // The only possible type for non derived types.
            if ((options & StreamOptions.AllowDerivedTypes) == 0)
            {
                return type;
            }

            // Not type specific attributes, we must load object for now.
            return null;
        }

        public void Write(uint index, string type, object obj)
        {
            if ((options & StreamOptions.SingleObject) != 0)
            {
                journal.Execute(new AddObjectSO(headerAddress, obj as byte[]));
            }
            else
            {
                journal.Execute(new AddObject(objectAddress, index, obj as byte[]));
            }
        }

        public void WriteObjects(uint index, string[] types, object[] objects)
        {
            journal.Execute(new AddObjects(objectAddress, index, objects));
        }

        public void InsertBefore(uint index, string type, object obj)
        {
            journal.Execute(new InsertObject(objectAddress, index, true, obj as byte[]));
        }

        public void InsertAfter(uint index, string type, object obj)
        {
            journal.Execute(new InsertObject(objectAddress, index, false, obj as byte[]));
        }

        public void Erase(uint index, uint numObjects, bool makeEmpty)
        {
            journal.Execute(new RemoveObjects(objectAddress, index, numObjects, !makeEmpty));
        }

        public unsafe uint Count
        {
            get 
            {
                if ((options & StreamOptions.SingleObject) != 0)
                {
                    return objectAddress != 0 ? 1u : 0u;
                }
                else
                {
                    BPlusTree tree = new BPlusTree(objectAddress);
                    return (uint)tree.ListAll(journal.ReadService).Count;
                }
            }
        }

        public bool UsesRaw
        {
            get { return true; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
