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
using SharpMedia.Database.Physical.Journalling;

namespace SharpMedia.Database.Physical
{
    /// <summary>
    /// An object info structure.
    /// </summary>
    public class ObjectInfo
    {
        /// <summary>
        /// The index.
        /// </summary>
        public uint Index;

        /// <summary>
        /// Size of object.
        /// </summary>
        public ulong Size;

        /// <summary>
        /// The offset of object.
        /// </summary>
        public ulong Address;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfo"/> class.
        /// </summary>
        public ObjectInfo()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfo"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="address">The address.</param>
        public ObjectInfo(uint index, ulong size, ulong address)
        {
            this.Size = size;
            this.Address = address;
            this.Index = index;
        }

    }

    /// <summary>
    /// A BTree implementation. It is actually a helper class that allows
    /// (fast) searches in B+ tree. It also allows removing and inserting
    /// new objects atomically.
    /// </summary>
    internal class BPlusTree
    {
        #region Private Members
        ulong rootAddress;
        #endregion

        #region Private Methods

        /// <summary>
        /// Finds the macimum object we can fit in.
        /// </summary>
        /// <param name="blockSize">Size of the block.</param>
        /// <returns></returns>
        unsafe uint MaxObjectFit(uint blockSize)
        {
            blockSize -= (uint)sizeof(BTreeNode);
            return blockSize / (uint)sizeof(BTreeNodePart);
        }

        /// <summary>
        /// Extracts object data from leaf.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        unsafe List<ObjectInfo> ExtractObjectsFromLeaf(Block block)
        {
            fixed (byte* p = block.Data)
            {
                // We extract header and finish if not leaf.
                BTreeNode* node = (BTreeNode*)p;
                if (node->Flags != BTreeNodeOptions.Leaf) return null;

                // We iterate through objects.
                List<ObjectInfo> all = new List<ObjectInfo>(node->Count);
                BTreeNodePart* data = (BTreeNodePart*)&node->Data;

                for (uint i = 0; i < node->Count; i++)
                {
                    all.Add(new ObjectInfo(data[i].ObjectIndex, data[i].ObjectSize, data[i].BlockAddress));
                }

                // We are finished.
                return all;
            }


        }


        /// <summary>
        /// Writes the objects to leaf.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="maxObjects">The max objects.</param>
        /// <param name="info">The info.</param>
        /// <returns></returns>
        unsafe bool WriteObjectsToLeaf(Block block, uint maxObjects, List<ObjectInfo> info)
        {
            if ((uint)info.Count > maxObjects) return false;

            fixed (byte* p = block.Data)
            {
                // Initialize header.
                BTreeNode* node = (BTreeNode*)p;
                node->Count = (ushort)info.Count;
                node->Flags = BTreeNodeOptions.Leaf;

                // And write data.
                BTreeNodePart* data = (BTreeNodePart*)&node->Data;
                for (int i = 0; i < info.Count; i++)
                {
                    data[i].ObjectSize = info[i].Size;
                    data[i].ObjectIndex = info[i].Index;
                    data[i].BlockAddress = info[i].Address;
                }
            }

            // Sucessfully written it.
            return true;
        }

        /// <summary>
        /// Inserts to ordered collection.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="newEntry">The new entry.</param>
        void InsertToOrderedCollection(List<ObjectInfo> info, ObjectInfo newEntry)
        {
            // First find a position to insert.
            int i;
            for (i = 0; i < info.Count; i++)
            {
                if (info[i].Index > newEntry.Index)
                {
                    break;
                }
            }

            // We insert it now.
            info.Insert(i, newEntry);
        }


        #endregion


        #region Public Members

        /// <summary>
        /// Constructs B-plus tree.
        /// </summary>
        /// <param name="address"></param>
        public BPlusTree(ulong address)
        {
            rootAddress = address;
        }

        /// <summary>
        /// The root address of node.
        /// </summary>
        public ulong RootAddress
        {
            get
            {
                return rootAddress;
            }
        }

        /// <summary>
        /// Adds a new entry into B+ tree. If it already exists, exception is thrown.
        /// </summary>
        /// <param name="services">The services node.</param>
        /// <param name="info"></param>
        public unsafe void Add(Journalling.IService services, ObjectInfo info)
        {
            Block block = services.Read(BlockType.BPlusTreeBlock, rootAddress);
            List<ObjectInfo> objects = ExtractObjectsFromLeaf(block);

            // We check if already exists (may remove this in fitire for performance reasons).
            if (objects.Exists(delegate(ObjectInfo other) { return other.Index == info.Index; }))
            {
                throw new DatabaseException("Adding to existing location.");
            }

            // We add the new object and write.
            InsertToOrderedCollection(objects, info);
            if (!WriteObjectsToLeaf(block, MaxObjectFit(services.BlockSize), objects))
            {
                throw new NotSupportedException();
            }

            services.Write(BlockType.BPlusTreeBlock, rootAddress, block);

        }

        /// <summary>
        /// Inserts a new entry into B+ tree, increasing count of all sequent indices.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="index">Index where to insert.</param>
        /// <param name="info">The info of object.</param>
        public void Insert(Journalling.IService services,
                            bool before, ObjectInfo info)
        {
            Block block = services.Read(BlockType.BPlusTreeBlock, rootAddress);
            List<ObjectInfo> objects = ExtractObjectsFromLeaf(block);

            // We convert all to before.
            if (!before) info.Index--;

            // Increment all sequent blocks.
            objects.ForEach(delegate(ObjectInfo other)
            {
                if (other.Index >= info.Index)
                {
                    other.Index++;
                }
            });

            // We now insert our object.
            InsertToOrderedCollection(objects, info);

            // We write the header.
            if (!WriteObjectsToLeaf(block, MaxObjectFit(services.BlockSize), objects))
            {
                throw new NotSupportedException();
            }
            services.Write(BlockType.BPlusTreeBlock, rootAddress, block);
        }

        /// <summary>
        /// Inspects for inserting.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="index">The index.</param>
        /// <param name="before">if set to <c>true</c> [before].</param>
        /// <returns></returns>
        public uint InspectForInserting(Journalling.IReadService services, uint index, bool before)
        {
            return 0;
        }

        /// <summary>
        /// Inspects the BPlus tree for adding. If entry already exists, 0 is returned.
        /// </summary>
        /// <param name="services">Reading service.</param>
        /// <param name="index"></param>
        /// <param name="explicitLocks"></param>
        /// <returns></returns>
        public uint InspectForAdding(Journalling.IReadService services, uint index)
        {
            return 0;
        }

        /// <summary>
        /// Finds the object info.
        /// </summary>
        /// <param name="services">Reading services.</param>
        /// <param name="index">The index to locate.</param>
        /// <returns>Object info, or null if it does not exist.</returns>
        public ObjectInfo Find(Journalling.IReadService services, uint index)
        {
            Block block = services.Read(BlockType.BPlusTreeBlock, rootAddress);
            List<ObjectInfo> objects = ExtractObjectsFromLeaf(block);

            // We find it fast (may use binary search in future)
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Index == index) return objects[i];
            }

            // Not found.
            return null;
        }

        /// <summary>
        /// Removes an index. Does not remove what it references.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="index">The index to remove.</param>
        /// <param name="count">Number of object to delete.</param>
        /// <param name="decreaseCount">Should we decrease all sequent indices by one.</param>
        /// <returns></returns>
        public void Remove(Journalling.IService service, uint index, 
                        uint count, bool decreaseCount)
        {
            // Read it first.
            Block block = service.Read(BlockType.BPlusTreeBlock, rootAddress);
            List<ObjectInfo> objects = ExtractObjectsFromLeaf(block);

            // We remove the object.
            objects.RemoveAll(delegate(ObjectInfo info)
            {
                return info.Index >= index && info.Index < index + count;
            });

            // We may decrease count.
            if (decreaseCount)
            {
                objects.ForEach(delegate(ObjectInfo other)
                {
                    if (other.Index > index) other.Index -= count;
                });
            }

            // We write it back.
            if (!WriteObjectsToLeaf(block, MaxObjectFit(service.BlockSize), objects))
            {
                throw new NotSupportedException();
            }
            service.Write(BlockType.BPlusTreeBlock, rootAddress, block);

        }

        /// <summary>
        /// Lists all entries in B+ tree.
        /// </summary>
        /// <param name="service">The service to read.</param>
        /// <returns></returns>
        public List<ObjectInfo> ListAll(Journalling.IReadService service)
        {
            ulong overhead;
            return ListAll(service, out overhead);
        }

        /// <summary>
        /// Lists all entries in B+ tree.
        /// </summary>
        /// <param name="service">The service to read.</param>
        /// <param name="overhead">The byte overhead.</param>
        /// <returns></returns>
        public List<ObjectInfo> ListAll(Journalling.IReadService service, out ulong overhead)
        {
            ulong over = 0;

            // We read them first.
            Block block = service.Read(BlockType.BPlusTreeBlock, rootAddress);
            List<ObjectInfo> objects = ExtractObjectsFromLeaf(block);

            // Compute overhead.
            objects.ForEach(delegate(ObjectInfo info) { over += info.Size; });

            overhead = over;
            return objects;
        }

        /// <summary>
        /// Lists entries in B+ tree.
        /// </summary>
        /// <param name="service">The service to read.</param>
        /// <param name="index">The index of object.</param>
        /// <param name="count">Number of objects.</param>
        /// <returns></returns>
        public List<ObjectInfo> List(Journalling.IReadService service, uint index, uint count)
        {
            List<ObjectInfo> all = ListAll(service);
            all.RemoveAll(delegate(ObjectInfo info)
            {
                return info.Index < index || info.Index >= index + count;
            });

            // We may return it.
            return all;
        }


        /// <summary>
        /// Deallocates the tree.
        /// </summary>
        /// <param name="service">The service.</param>
        public void DeallocateTree(Journalling.IService service)
        {
            service.DeAllocate(rootAddress);
        }

        /// <summary>
        /// Inspects for removing.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="index">The index.</param>
        /// <param name="count">Number of object to remove.</param>
        /// <returns></returns>
        public uint InspectForRemoving(Journalling.IReadService service, uint index, uint count)
        {
            return 0;
        }

        /// <summary>
        /// Replaces the specified index data. Fails if it does not exist.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="index">The index.</param>
        /// <param name="info">The info.</param>
        public void Replace(Journalling.IService service, ObjectInfo info)
        {
            Block block = service.Read(BlockType.BPlusTreeBlock, rootAddress);
            List<ObjectInfo> objects = ExtractObjectsFromLeaf(block);

            // We remove it first.
            objects.RemoveAll(delegate(ObjectInfo info2) { return info.Index == info2.Index; });

            // We add the new object and write.
            InsertToOrderedCollection(objects, info);
            if (!WriteObjectsToLeaf(block, MaxObjectFit(service.BlockSize), objects))
            {
                throw new NotSupportedException();
            }

            service.Write(BlockType.BPlusTreeBlock, rootAddress, block);
        }

        #endregion
    }
}
