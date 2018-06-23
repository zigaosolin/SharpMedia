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

namespace SharpMedia.Database.Physical.Journalling
{
    /// <summary>
    /// The safe journal service.
    /// </summary>
    internal class SafeService : IService
    {
        #region Private Members
        Caching.BlockCache provider;
        AllocationContext context;

        List<ulong> freedData = new List<ulong>();
        List<ulong> allocData = new List<ulong>();
        SortedList<ulong, KeyValuePair<BlockType, byte[]>> blocksWritten = new SortedList<ulong, KeyValuePair<BlockType, byte[]>>();

        #endregion

        #region Public Members

        /// <summary>
        /// Nons the safe journal.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="context">The context.</param>
        public SafeService(Caching.BlockCache provider, AllocationContext context)
        {
            this.provider = provider;
            this.context = context;
            this.context.Service = this;
        }

        /// <summary>
        /// Flushes this service.
        /// </summary>
        public unsafe void Commit(Allocator allocator, JournalLog log)
        {
            freedData.Sort();
            allocData.Sort();

            // Upon dispose, we execute.
            log.RunTransaction(context.HintBlock, allocData, freedData, blocksWritten);

            // We can also return data to allocator.
            context.Dispose();

            // Signal memory freed.
            allocator.MemoryFreed(freedData);
        }

        #endregion

        #region IService Members

        public AllocationContext AllocationContext
        {
            get { return context; }
        }

        public void Allocate(ulong address)
        {
            allocData.Add(address);
        }

        public void DeAllocate(ulong address)
        {
            if (address == 103)
            {
                address = 103;
            }
            if (freedData.Contains(address))
            {
                return;
            }
            freedData.Add(address);
        }

        public void Write(BlockType type, ulong address, Block block)
        {
            if (context.IsAllocated(address))
            {
                // We can immediatelly write since it is not journalled.
                provider.Write(type, address, block.Data);
            }
            else
            {
                blocksWritten[address] = new KeyValuePair
                    <BlockType,byte[]>(type, (byte[])block.Data.Clone());
            }
        }

        #endregion

        #region IReadService Members

        public uint BlockSize
        {
            get { return provider.BlockSize; }
        }

        public Block Read(SharpMedia.Database.Physical.Caching.BlockType type, ulong address)
        {
            // First check if already written to.
            KeyValuePair<BlockType, byte[]> value;
            if (blocksWritten.TryGetValue(address, out value))
            {
                return new Block((byte[])value.Value.Clone());
            }

            return new Block(provider.Read(type, address));
        }

        #endregion
    }
}
