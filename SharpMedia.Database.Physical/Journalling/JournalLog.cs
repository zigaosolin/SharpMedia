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

namespace SharpMedia.Database.Physical.Journalling
{

    /// <summary>
    /// A journal log is a thread-safe journal allocator.
    /// </summary>
    public class JournalLog
    {
        #region Private Members
        object          syncRoot = new object();
        Allocator       allocator;
        uint            frequency;


        private class Logger
        {
            int currentBlockIndex = 0;
            int currentOffset = 0;
            int backwardBlock;
            List<ulong> blocks;
            byte[] prevBlock = null;
            byte[] currentBlockData;
            Caching.BlockCache provider;

            #region Public Members

            public Logger(List<ulong> blocks, Caching.BlockCache provider)
            {
                this.backwardBlock = blocks.Count - 1;
                this.blocks = blocks;
                this.currentBlockData = new byte[provider.BlockSize];
                this.provider = provider;
            }

            public unsafe void Log(JournalLogData input)
            {
                fixed (byte* p = currentBlockData)
                {
                    JournalLogData* data = (JournalLogData*)p;

                    if (currentOffset > 0)
                    {
                        data[currentOffset - 1].NextLogEntryBlock = blocks[currentBlockIndex];
                        data[currentOffset - 1].NextLogEntryOffset = (uint)currentOffset;
                    }
                    data[currentOffset++] = input;

                    
                }

                // Update previous block and write.
                if (currentOffset == 1 && currentBlockIndex > 0)
                {
                    fixed (byte* p = prevBlock)
                    {
                        JournalLogData* data = (JournalLogData*)p;
                        int lastIndex = (int)(provider.BlockSize / sizeof(JournalLogData) - 1);
                        data[lastIndex].NextLogEntryBlock = blocks[currentBlockIndex];
                        data[lastIndex].NextLogEntryOffset = 0;
                    }

                    provider.Write(BlockType.JournalBlock, blocks[currentOffset-1], prevBlock);
                }

                if (currentOffset >= provider.BlockSize / sizeof(JournalLogData))
                {
                    // Write is issued on previous block only.
                    prevBlock = currentBlockData;
                    currentBlockData = new byte[provider.BlockSize];

                    // Go to next (can reuse block).
                    currentOffset = 0;
                    currentBlockIndex++;
                }
            }

            public ulong Log(byte[] data)
            {
                ulong addr = blocks[backwardBlock--];
                provider.Write(BlockType.JournalBlock, addr, data);
                return addr;
            }

            public void Flush()
            {
                if (currentOffset == 0)
                {
                    provider.Write(BlockType.JournalBlock, blocks[currentOffset-1], prevBlock);
                }

                // Also flush current.
                provider.Write(BlockType.JournalBlock, blocks[currentBlockIndex], currentBlockData);
            }

            #endregion

        }

        /// <summary>
        /// Converts to bigger units.
        /// </summary>
        List<KeyValuePair<ulong, uint>> ToUnits(List<ulong> blocks)
        {
            List<KeyValuePair<ulong, uint>> retVal = new List<KeyValuePair<ulong, uint>>();
            if(blocks.Count == 0) return retVal;

            // We create first value.
            KeyValuePair<ulong, uint> keyValue = new KeyValuePair<ulong, uint>(blocks[0], 1);
            for (int i = 1; i < blocks.Count; i++)
            {
                if (blocks[i] == keyValue.Key + keyValue.Value)
                {
                    keyValue = new KeyValuePair<ulong,uint>(keyValue.Key, keyValue.Value+1);
                }
                else
                {
                    retVal.Add(keyValue);
                    keyValue = new KeyValuePair<ulong, uint>(blocks[i], 1);
                }
            }

            retVal.Add(keyValue);

            return retVal;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Only recovers blocks that reside in logs. It returns object serialized by
        /// journal for further processing.
        /// </summary>
        /// <param name="provider">The block provider.</param>
        /// <param name="journal">The journal.</param>
        public static bool StartupRecovery(IJournal journal)
        {
            // We go through all journal logs.


            return true;   
        }

        /// <summary>
        /// Is the address a journal log.
        /// </summary>
        public static bool IsJournalLog(ulong address, uint frequency, uint blockSize)
        {
            return GetNearestJournal(address, frequency, blockSize, ulong.MaxValue) == address;
        }

        /// <summary>
        /// Gets the nearest journal.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="blockSize">Size of the block.</param>
        /// <param name="blockCount">The block count.</param>
        /// <returns></returns>
        public static ulong GetNearestJournal(ulong address, uint frequency, uint blockSize, ulong blockCount)
        {
            // Makes sure 0 and 1 are still valid hints.
            if (address <= BlockHelper.FirstSuperBlockAddress) address = BlockHelper.FirstAllocationBlockAddress;

            // Find nearest allocation block.
            ulong allocBlockIndex = BlockHelper.GetAllocationBlockIndex(blockSize, address);

            // Find closest.
            ulong difference = allocBlockIndex % (ulong)frequency;

            if ((ulong)frequency / 2 > difference)
            {
                // We round up.
                allocBlockIndex += difference;
            }
            else
            {
                // We round down.
                allocBlockIndex -= difference;
            }

            return BlockHelper.GetAllocationBlockAddress(blockSize, allocBlockIndex) + 1;
        }

        /// <summary>
        /// Writes the allocations deallocs.
        /// </summary>
        /// <param name="allocs">The allocs.</param>
        public static void WriteAllocationsDeallocs(Caching.BlockCache provider, List<ulong> allocs)
        {
            // We can now group them now for each block.
            for (int i = 0; i < allocs.Count; )
            {
                // We obtain the allocation block.
                uint offset;
                ulong allocAddress = BlockHelper.GetAllocationBlock(provider.BlockSize, allocs[i], out offset);

                // We read it and gain access to it.
                BoolArray array = new BoolArray(provider.Read(BlockType.AllocationBlock, allocAddress));

                // We write first data.
                array.Change(offset);

                // Continue while we can in this block.
                i++;
                for (; i < allocs.Count; i++)
                {
                    // We quite if not the same block.
                    if (allocAddress != BlockHelper.GetAllocationBlock(provider.BlockSize, allocs[i], out offset)) break;

                    // Otherwise write.
                    array.Change(offset);
                }

                // We need to write it now.
                provider.Write(BlockType.AllocationBlock, allocAddress, array.Data);

            }
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="JournalLog"/> class.
        /// </summary>
        /// <param name="journal">The journal.</param>
        /// <param name="allocator">The allocator.</param>
        /// <param name="frequency">The frequency.</param>
        public unsafe JournalLog(Allocator allocator)
        {
            this.allocator = allocator;

            byte[] data = allocator.Provider.Read(BlockType.NoCache, 0);
            fixed (byte* p = data)
            {
                DatabaseHeader* header = (DatabaseHeader*)p;
                this.frequency = header->JournalFrequency;
            }
        }


        /// <summary>
        /// End the operation - called on operation log dispose.
        /// </summary>
        /// <param name="baseAddress">The key.</param>
        public unsafe void RunTransaction(ulong hint, List<ulong> allocations, 
            List<ulong> deallocations, SortedList<ulong,KeyValuePair<BlockType,byte[]>> safeWrites)
        {
            int j;
            object key = new object();

            // We compress them.
            List<KeyValuePair<ulong, uint>> allocationUnits = ToUnits(allocations);
            List<KeyValuePair<ulong, uint>> deallocationUnits = ToUnits(deallocations);

            // 1) Prepares the transaction.
            
            // a) Calculate number of blocks needed.
            uint externalBlocks = (uint)safeWrites.Count;
            uint logSize = (uint)(((allocationUnits.Count + deallocationUnits.Count + safeWrites.Count)
                *(long)sizeof(JournalLogData)+(long)allocator.BlockSize-1)/(long)allocator.BlockSize);

            // No transaction needed.
            if (logSize == 0) return;

            // b) Allocate blocks.
            List<ulong> blocks = allocator.Allocate(key, hint, AllocationStrategy.JournalAllocation, externalBlocks + logSize);

            Logger logger = new Logger(blocks, allocator.Provider);

            try
            {
                // c) Write data to blocks.
                for (j = 0; j < allocationUnits.Count; j++)
                {
                    // We write entry for each allocation.
                    logger.Log(new JournalLogData(JournalLogKind.AllocateBlock,
                        allocationUnits[j].Key, allocationUnits[j].Value));
                }

                for (j = 0; j < deallocationUnits.Count; j++)
                {
                    logger.Log(new JournalLogData(JournalLogKind.DeallocateBlock,
                        deallocationUnits[j].Key, deallocationUnits[j].Value));
                }

                for (j = 0; j < safeWrites.Count; j++)
                {
                    ulong backupAddress = safeWrites.Keys[j];
                    byte[] data = allocator.Provider.Read(safeWrites.Values[j].Key, backupAddress);
                    logger.Log(new JournalLogData(JournalLogKind.UpdateBlock, logger.Log(data), backupAddress));
                }

                // Make sure we flush it.
                logger.Flush();

                ulong journalAddress = GetNearestJournal(hint, frequency,
                        allocator.BlockSize, allocator.BlockCount);
                uint journalAddressOffset = uint.MaxValue;

                // 2) Allocates the journal sector.
                lock (syncRoot)
                {
                    
                    // Find free space and write.
                    byte[] block = allocator.Provider.Read(BlockType.JournalSectorBlock, journalAddress);
                    fixed (byte* p = block)
                    {
                        ulong* data = (ulong*)p;

                        for (uint z = 0; z < allocator.BlockSize/8; z++)
                        {
                            if (z == 0)
                            {
                                data[z] = blocks[0];
                                journalAddressOffset = z;
                                break;
                            }
                        }

                        if (journalAddressOffset == uint.MaxValue)
                        {
                            throw new InvalidOperationException();
                        }
                    }

                    // We are in transaction now.
                    allocator.Provider.Write(BlockType.JournalSectorBlock, journalAddress, block);
                }

                // Must be able to recover from those states.

                // 3) Execute operations (writes).
                List<ulong> all = new List<ulong>(allocations.Count + deallocations.Count);
                all.AddRange(allocations);
                all.AddRange(deallocations);

                // a) Write allocs and deallocs
                JournalLog.WriteAllocationsDeallocs(allocator.Provider, all);


                // b) Write to buffers.
                for (int z = 0; z < safeWrites.Count; z++)
                {
                    allocator.Provider.Write(safeWrites.Values[z].Key,
                        safeWrites.Keys[z], safeWrites.Values[z].Value);
                }


                // 4) Deallocate the journal sector (finished operation).
                lock (syncRoot)
                {
                    byte[] block = allocator.Provider.Read(BlockType.JournalSectorBlock, journalAddress);
                    fixed (byte* p = block)
                    {
                        ulong* data = (ulong*)p;
                        data[journalAddressOffset] = 0;
                    }
                    allocator.Provider.Write(BlockType.JournalSectorBlock, journalAddress, block);
                }

            }
            finally
            {
                allocator.ReturnAllocations(key, blocks);
            }

        }

        #endregion
    }
}
