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

namespace SharpMedia.Database.Physical.Journalling
{

    /// <summary>
    /// A Read-only service to access blocks.
    /// </summary>
    public interface IReadService
    {

        /// <summary>
        /// Returns the size of one block.
        /// </summary>
        uint BlockSize
        {
            get;
        }

        /// <summary>
        /// Reads a block from storage.
        /// </summary>
        /// <param name="type">The block type, must be specified and correct.</param>
        /// <param name="address">The actual address of block.</param>
        /// <returns>Returned block.</returns>
        Block Read(Caching.BlockType type, ulong address);

    }

    /// <summary>
    /// A full journalling service, with state of operation, commits, allocations etc.
    /// </summary>
    public interface IService : IReadService
    {
        /// <summary>
        /// The free blocks smart context.
        /// </summary>
        AllocationContext AllocationContext { get; }

        /// <summary>
        /// Allocates the address. The actual allocations happens at commit (new stage) and is atomic 
        /// (all or nothing). Many times, we are able to allocate many node as one commit.
        /// </summary>
        /// <param name="address"></param>
        void Allocate(ulong address);

        /// <summary>
        /// Address can only be deallocated if locked.
        /// </summary>
        /// <param name="address">The address.</param>
        void DeAllocate(ulong address);

        /// <summary>
        /// Writes a block of data.
        /// </summary>
        /// <remarks>
        /// Writing to allocated memory (the allocation is commited after this
        /// operations/in this stage) is safe, while actual writing to other blocks
        /// (rewriting) is journalled.
        /// </remarks>
        /// <param name="type">The type of block.</param>
        /// <param name="address">The address of block.</param>
        /// <param name="block">The block data.</param>
        void Write(Caching.BlockType type, ulong address, Block block);
        
    }
}
