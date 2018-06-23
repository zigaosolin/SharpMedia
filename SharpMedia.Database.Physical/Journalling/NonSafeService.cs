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
using SharpMedia.Database.Physical.Provider;
using SharpMedia.Database.Physical.Caching;

namespace SharpMedia.Database.Physical.Journalling
{


    /// <summary>
    /// The nonsafe journal service.
    /// </summary>
    internal class NonSafeService : IService
    {
        #region Private Members
        Caching.BlockCache provider;
        AllocationContext context;

        List<ulong> freedData = new List<ulong>();
        List<ulong> allocData = new List<ulong>();
        #endregion

        #region Public Members

        /// <summary>
        /// Nons the safe journal.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="context">The context.</param>
        public NonSafeService(Caching.BlockCache provider, AllocationContext context)
        {
            this.provider = provider;
            this.context = context;
            this.context.Service = this;
        }

        /// <summary>
        /// Flushes this service.
        /// </summary>
        public unsafe void Dispose(Allocator allocator)
        {
            // Must sort it anyway.
            freedData.Sort();

            List<ulong> allocs = new List<ulong>(freedData.Count + allocData.Count);
            allocs.AddRange(freedData);
            allocs.AddRange(allocData);

            // We sort allocations and frees.
            allocs.Sort();

            JournalLog.WriteAllocationsDeallocs(provider, allocs);

            // We also end the context.
            context.Dispose();

            // Return freed blocks.
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
            if (freedData.Contains(address))
            {
                return;
            }
            freedData.Add(address);
        }

        public void Write(BlockType type, ulong address, Block block)
        {
    
            // We can immediatelly write since it is not journalled.
            provider.Write(type, address, block.Data);
        }

        #endregion

        #region IReadService Members

        public uint BlockSize
        {
            get { return provider.BlockSize; }
        }

        public Block Read(SharpMedia.Database.Physical.Caching.BlockType type, ulong address)
        {
            return new Block(provider.Read(type, address));
        }

        #endregion
    }
}
