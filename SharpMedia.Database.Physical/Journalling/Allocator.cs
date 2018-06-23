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
using System.Collections;

namespace SharpMedia.Database.Physical.Journalling
{
    /// <summary>
    /// Allocation strategy can be used to hint what kind of allocation we want.
    /// </summary>
    public enum AllocationStrategy
    {
        /// <summary>
        /// Normal allocation.
        /// </summary>
        Normal,

        /// <summary>
        /// We extend previous allocation - this allocation will always be forced.
        /// </summary>
        DynamicAllocation,

        /// <summary>
        /// The journal allocation.
        /// </summary>
        JournalAllocation
    }

    /// <summary>
    /// An allocator class. 
    /// TODO: allocation block info must be cached, not permanent.
    /// </summary>
    public class Allocator
    {
        #region Private Members
        
        /// <summary>
        /// The allocation block data.
        /// </summary>
        class AllocationBlockInfo
        {
            /// <summary>
            /// Address of allocation block.
            /// </summary>
            public ulong Address;

            /// <summary>
            /// The free blocks in data.
            /// </summary>
            public List<uint> FreeBlocks;

            /// <summary>
            /// The operation locking.
            /// </summary>
            public object LockingOperation;

            /// <summary>
            /// Initializes a new instance of the <see cref="AllocationBlockInfo"/> class.
            /// </summary>
            /// <param name="addr">The addr.</param>
            /// <param name="freeBlocks">The free blocks.</param>
            public AllocationBlockInfo(ulong addr, List<uint> freeBlocks)
            {
                Address = addr;
                FreeBlocks = freeBlocks;
            }
        }

        /// <summary>
        /// The operation lock data.
        /// </summary>
        class OperationLockData
        {
            /// <summary>
            /// The unique key.
            /// </summary>
            public object Key;

            /// <summary>
            /// The information.
            /// </summary>
            public List<AllocationBlockInfo> Info = new List<AllocationBlockInfo>();

            /// <summary>
            /// Initializes a new instance of the <see cref="OperationLockData"/> class.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="info">The info.</param>
            public OperationLockData(object key, List<AllocationBlockInfo> info)
            {
                this.Key = key;
                this.Info = info;
            }

        }


        object syncRoot = new object();
        Caching.BlockCache provider;
        ulong blockCount;
        SortedDictionary<ulong, AllocationBlockInfo> inspectedFreeBlocks = new SortedDictionary<ulong, AllocationBlockInfo>();
        List<OperationLockData> lockingList = new List<OperationLockData>();
        #endregion

        #region Private Methods


        /// <summary>
        /// Scans one allocation block.
        /// </summary>
        /// <param name="address">The allocation block address.</param>
        /// <returns></returns>
        AllocationBlockInfo ScanBlock(ulong address)
        {
            AllocationBlockInfo value;
            if (inspectedFreeBlocks.TryGetValue(address, out value))
            {
                return value;
            }

            BoolArray array = new BoolArray(provider.Read(BlockType.AllocationBlock, address));

            // We first check for all written (fast).
            if (array.IsAllTrue)
            {
                value = new AllocationBlockInfo(address, new List<uint>());
                inspectedFreeBlocks.Add(address, value);
                return value;
            }

            List<uint> freeBlocks = new List<uint>();
            
            // We search through all (optimized search, may optimize even better in future.
            // If this will be a bottle neck, we can fix the array outside and than search
            // through it using ulong* pointer (no need to repeat this every time)).
            for (uint i = 0; i < provider.BlockSize/8; i++)
            {
                // We chaeck 8-bytes at one for all full.
                if (array.IsULongFull(i))
                {
                    continue;
                }
                
                // For all free.
                if (array.IsULongFree(i))
                {
                    for (uint j = i*64; j < (i+1)*64; j++)
                    {
                        freeBlocks.Add(j+1);
                    }
                    continue;
                }

                // We now check by byte.
                for (uint j = i * 64; j < (i + 1) * 64; j++)
                {
                    if (!array[j])
                    {
                        freeBlocks.Add(j+1);
                    }
                }
                
            }

            value = new AllocationBlockInfo(address, freeBlocks);
            inspectedFreeBlocks.Add(address, value);
            return value;
        }

        /// <summary>
        /// Adds blocks and all data.
        /// </summary>
        void AddBlock(object op, ulong address, 
            List<ulong> freeBlocks, List<AllocationBlockInfo> blocksLocked,
            ref uint count)
        {
            // We could add any.
            AllocationBlockInfo info = ScanBlock(address);
            if ((info.LockingOperation != null && info.LockingOperation != op) ||
                info.FreeBlocks.Count == 0)
            {
                return;
            }

            // We add all we can (max count),
            info.LockingOperation = op;
            blocksLocked.Add(info);

            // We remove those used.
            int c = info.FreeBlocks.Count > (int)count ? (int)count : info.FreeBlocks.Count;
            for (int i = 0; i < c; i++)
            {
                ulong addrToAdd = address + (ulong)info.FreeBlocks[i];
                if (addrToAdd == 103)
                {
                    addrToAdd = 103;
                }
                freeBlocks.Add(addrToAdd);
            }

            // Remove used indices.
            count -= (uint)c;
            info.FreeBlocks.RemoveRange(0, c);
        }

        /// <summary>
        /// Finds the free blocks.
        /// </summary>
        /// <param name="hint">The hint.</param>
        /// <param name="strategy">The strategy.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        List<ulong> FindFreeBlocks(object op, ulong hint, AllocationStrategy strategy,
            uint count, List<AllocationBlockInfo> lockedBlocks)
        {
            // We load blocks in vicinity. If there is no hint, we chose one.
            if (hint == 0)
            {
                // We choose the "default" key.
                uint unused;

                // We must extract first allocation block, the only way is to use the enumerator.
                IEnumerator<KeyValuePair<ulong,AllocationBlockInfo>> en = inspectedFreeBlocks.GetEnumerator();
                en.MoveNext();

                hint = inspectedFreeBlocks.Count > 0 ? en.Current.Key
                    : BlockHelper.GetAllocationBlock(provider.BlockSize, 1, out unused);
            }

            // TODO Super blocks.
            List<ulong> freeBlocks = new List<ulong>();

            // We begin allocation (forward from hint)
            uint unused2;
            ulong block = BlockHelper.GetAllocationBlock(provider.BlockSize, hint, out unused2);
            for (; count > 0; block = BlockHelper.GetNextAllocationBlock(provider.BlockSize, block, blockCount))
            {
                AddBlock(op, block, freeBlocks, lockedBlocks, ref count);
            }

            // And backwards if none found.
            for (block = BlockHelper.GetPrevAllocationBlock(provider.BlockSize, block); count > 0;
                block = BlockHelper.GetPrevAllocationBlock(provider.BlockSize, block))
            {
                AddBlock(op, block, freeBlocks, lockedBlocks, ref count);
            }

            return freeBlocks;
        }

        #endregion


        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="Allocator"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public unsafe Allocator(Caching.BlockCache provider)
        {
            this.provider = provider;
            byte[] data = provider.Read(BlockType.NoCache, 0);
            fixed (byte* p = data)
            {
                DatabaseHeader* header = (DatabaseHeader*)p;
                if (!header->IsValid)
                {
                    throw new DatabaseException("The database header is not valid, not a valid physical database. Must reformat it.");
                }

                blockCount = header->BlockCount;
            }

        }

        /// <summary>
        /// Clears the cache. This is sometimes forced.
        /// </summary>
        public void ClearCache()
        {
            lock (syncRoot)
            {
                inspectedFreeBlocks.Clear();
            }
        }

        /// <summary>
        /// Gets the block count.
        /// </summary>
        /// <value>The block count.</value>
        public ulong BlockCount
        {
            get
            {
                return blockCount;
            }
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public Caching.BlockCache Provider
        {
            get
            {
                return provider;
            }
        }

        /// <summary>
        /// Gets the size of the block.
        /// </summary>
        /// <value>The size of the block.</value>
        public uint BlockSize
        {
            get
            {
                return provider.BlockSize;
            }
        }

        /// <summary>
        /// The free space.
        /// </summary>
        public ulong FreeSpace
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Allocates the blocks. Must still be commited (this is journal responsibility).
        /// </summary>
        /// <param name="who">Who is the allocator.</param>
        /// <param name="hint">The hint block, or 0.</param>
        /// <param name="strategy">The allocation strategy.</param>
        /// <param name="count">Number of blocks to return.</param>
        /// <returns></returns>
        public List<ulong> Allocate(object who, ulong hint, AllocationStrategy strategy, uint count)
        {
            lock (syncRoot)
            {
                

                // Finds free blocks.
                List<AllocationBlockInfo> lockedBlocks = new List<AllocationBlockInfo>();
                List<ulong> returnData = FindFreeBlocks(who, hint, strategy, count, lockedBlocks);

                // We add operation data.
                lockingList.Add(new OperationLockData(who, lockedBlocks));

                Console.WriteLine("{0} allocated {1} : {2} blocks", who, count, Common.ArrayToString(returnData));

                return returnData;
            }
        }

        /// <summary>
        /// Signals the memory freed.
        /// </summary>
        /// <param name="blocks">The blocks, must be sorted.</param>
        public void MemoryFreed(List<ulong> blocks)
        {
            lock (syncRoot)
            {

                Console.WriteLine("{0} : {1} blocks freed.", blocks.Count,
                    Common.ArrayToString<ulong>((IList<ulong>)blocks)); 

                // We go through sorted list.
                for (int i = 0; i < blocks.Count; )
                {
                    ulong addr = blocks[i];

                    // We check the allocation block.
                    uint offset;
                    ulong allocBlock = BlockHelper.GetAllocationBlock(provider.BlockSize, addr, out offset);

                    AllocationBlockInfo value;
                    if (inspectedFreeBlocks.TryGetValue(allocBlock, out value))
                    {
                        // We have the block.
                        value.FreeBlocks.Add(offset);

                        i++;
                        for (; i < blocks.Count; i++)
                        {
                            if (allocBlock != BlockHelper.GetAllocationBlock(provider.BlockSize, blocks[i], out offset))
                            {
                                break;
                            }

                            // We add it too.
                            value.FreeBlocks.Add(offset);
                        }

                        // Must ensure sorted.
                        value.FreeBlocks.Sort();
                    }
                    else
                    {
                        // Skip all belonging to the same allocation block.
                        i++;
                        for (; i < blocks.Count; i++)
                        {
                            if (allocBlock != BlockHelper.GetAllocationBlock(provider.BlockSize, blocks[i], out offset))
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns allocations (not commited). Must return even if there is nothing
        /// to return because operation is still blocking the allocation block.
        /// </summary>
        /// <param name="returned"></param>
        public void ReturnAllocations(object returning, List<ulong> returned)
        {
            lock (syncRoot)
            {
                Console.WriteLine("{0} operation returned {1} : {2} blocks returned.", 
                    returning, returned.Count, Common.ArrayToString<ulong>(returned));
                Console.Out.Flush();

                // We first return blocks.
                for (int i = 0; i < returned.Count; )
                {
                    ulong block = returned[i];

                    // We find the block.
                    uint offset;
                    ulong allocBlock = BlockHelper.GetAllocationBlock(provider.BlockSize, block, out offset);
                    AllocationBlockInfo blockInfo = inspectedFreeBlocks[allocBlock];

                    // We return the address.
                    blockInfo.FreeBlocks.Add(offset);

                    // We check all further (they are sorted so there is a big chance it is in the same block).
                    i++;
                    for (; i < returned.Count; i++)
                    {
                        block = returned[i];

                        // We exit if not in the same block.
                        if (BlockHelper.GetAllocationBlock(provider.BlockSize, block, out offset) != allocBlock) break;
                        blockInfo.FreeBlocks.Add(offset);
                    }

                    // Must be sorted.
                    blockInfo.FreeBlocks.Sort();
                }

                // We free all allocation blocks locks.
                List<AllocationBlockInfo> all = lockingList.Find(
                    delegate(OperationLockData data2) { return data2.Key == returning; }).Info;
                all.ForEach(delegate(AllocationBlockInfo info) { info.LockingOperation = null; });

                // We remove the operation.
                lockingList.RemoveAll(delegate(OperationLockData data3) { return data3.Key == returning; });
            }
        }

        #endregion
    }
}
