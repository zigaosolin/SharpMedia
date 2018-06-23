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
using SharpMedia.Database.Physical.StorageStructs;
using SharpMedia.Database.Physical.Caching;

namespace SharpMedia.Database.Physical.Journalling
{
    /// <summary>
    /// The allocation context.
    /// </summary>
    public class AllocationContext : IDisposable
    {
        #region Private Members
        Allocator allocator;
        IOperation operation;
        bool allowDynamic;
        ulong hint;
        List<ulong> availableBlocks;
        List<ulong> allAllocations;
        Caching.BlockCache provider;
        IService service;


        ulong Allocate(ulong prev)
        {
            // Make sure we have at least one block left.
            if (availableBlocks.Count <= 0)
            {
                if (allowDynamic)
                {
                    availableBlocks = allocator.Allocate(operation, hint, AllocationStrategy.DynamicAllocation, 5);
                    
                    // Update also all allocations.
                    allAllocations.AddRange(availableBlocks);
                    allAllocations.Sort();
                }
                else
                {
                    throw new OutOfMemoryException("The operation run out of memory, it did not request enough memory.");
                }
            }


            if (prev == 0)
            {
                // We simply return first block in the row.
                ulong addr = availableBlocks[0];
                availableBlocks.RemoveAt(0);
                return addr;
            }
            else
            {
                // We obtain the index of the next value (following prev).
                int index = ~availableBlocks.BinarySearch(prev);
                if (index == availableBlocks.Count)
                {
                    // We return the first.
                    ulong addr = availableBlocks[0];
                    availableBlocks.RemoveAt(0);
                    return addr;
                    
                }
                else
                {
                    // We return next in sequence.
                    ulong addr = availableBlocks[index];
                    availableBlocks.RemoveAt(index);
                    return addr;
                }

            }
        }
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationContext"/> class.
        /// </summary>
        /// <param name="availableBlocks">The available blocks.</param>
        /// <param name="allocator">The allocator.</param>
        /// <param name="allowDynamic">if set to <c>true</c> [allow dynamic].</param>
        /// <param name="provider">The provider.</param>
        public AllocationContext(List<ulong> availableBlocks, IOperation operation,
            Allocator allocator, bool allowDynamic, Caching.BlockCache provider)
        {
            this.availableBlocks = availableBlocks;
            this.allAllocations = new List<ulong>(availableBlocks);
            this.allocator = allocator;
            this.allowDynamic = allowDynamic;
            this.hint = availableBlocks.Count > 0 ? availableBlocks[availableBlocks.Count - 1] : 0;
            this.provider = provider;
            this.operation = operation;

            // Available blocks must be sorted.
            availableBlocks.Sort();
        }

        /// <summary>
        /// Sets the service.
        /// </summary>
        /// <value>The service.</value>
        public IService Service 
        { 
            set { service = value; }
        }

        /// <summary>
        /// Creates a write block stream for size.
        /// </summary>
        /// <param name="streamSize">The size.</param>
        /// <returns>Reutrns null if not possible.</returns>
        public unsafe BlockStream CreateBlockStream(ulong streamSize)
        {
            List<ulong> blockList = new List<ulong>();

            // Must make sure we can go negative.
            long size = (long)streamSize;

            // We begin with one block.
            ulong addr = Allocate(0);
            blockList.Add(addr);
            size -= provider.BlockSize - sizeof(BlockLinkHeader);

            // We append blocks until long enough.
            while (size > 0)
            {
                ulong newAddr = Allocate(addr);
                size -= provider.BlockSize;
                if (newAddr != addr + 1)
                {
                    size += sizeof(BlockLinkHeader);
                }

                // Make sure we add address.
                blockList.Add(newAddr);
                addr = newAddr;
            }

            return new BlockStream(blockList.ToArray(), service);
        }

        /// <summary>
        /// Creates an empty B+ tree.
        /// </summary>
        /// <returns>The address of B+ tree.</returns>
        public unsafe ulong CreateEmptyBTree()
        {
            // We allocate address.
            ulong address = Allocate(0);

            // We clear out the data and write it.
            byte[] data = new byte[provider.BlockSize];
            fixed (byte* p = data)
            {
                BTreeNode* node = (BTreeNode*)p;
                node->Flags = BTreeNodeOptions.Leaf;
                node->Count = 0;
            }
            provider.Write(BlockType.BPlusTreeBlock, address, data);

            // And return address.
            return address;
        }

        /// <summary>
        /// Checks if this address was allocated.
        /// </summary>
        /// <param name="address">The address in question.</param>
        /// <returns></returns>
        public bool IsAllocated(ulong address)
        {
            int idx = allAllocations.BinarySearch(address);
            return idx >= 0 && idx < allAllocations.Count;
        }

        /// <summary>
        /// Allocates a block.
        /// </summary>
        /// <returns></returns>
        public ulong AllocateBlock()
        {
            return Allocate(0);
        }

        /// <summary>
        /// Gets the hint block.
        /// </summary>
        /// <value>The hint block.</value>
        public ulong HintBlock
        {
            get
            {
                return hint;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            allocator.ReturnAllocations(operation, availableBlocks);
        }

        #endregion
    }
}
