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

namespace SharpMedia.Database.Physical
{
    /// <summary>
    /// A block helper class.
    /// </summary>
    internal static class BlockHelper
    {
        public const ulong FirstSuperBlockAddress = 1;
        public const ulong FirstAllocationBlockAddress = 2;

        /// <summary>
        /// We allow sizes from 512 to 4096.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static bool IsValidSize(uint size)
        {
            if (size % 512 == 0 && size <= 4096 && size >= 512) return true;
            return false;
        }

        /// <summary>
        /// Maximum number of typed stream in one node.
        /// </summary>
        /// <param name="blockSize">The block size.</param>
        /// <returns></returns>
        public static unsafe uint MaxTypedStreamsInNode(uint blockSize)
        {
            blockSize -= (uint)(sizeof(NodeVersionHeader) - sizeof(ulong));
            return blockSize / 12; //sizeof ulong + sizeof uint
        }

        /// <summary>
        /// Maximum number of required blocks for one object.
        /// </summary>
        /// <param name="objectSize">The size of object.</param>
        /// <param name="blockSize">The size of one block.</param>
        /// <returns>Number of blocks.</returns>
        public unsafe static uint MaxBlocksForObject(uint blockSize, ulong objectSize)
        {
            blockSize -= (uint)sizeof(BlockLinkHeader);
            return (uint)(objectSize / (ulong)blockSize + 1);
        }

        /// <summary>
        /// Approximates the object overhead.
        /// </summary>
        /// <param name="blockSize">Size of the block.</param>
        /// <param name="objectSize">Size of the object.</param>
        /// <returns></returns>
        public static ulong ApproximateObjectOverhead(uint blockSize, ulong objectSize)
        {
            if (objectSize == 0) return 0;

            // The useful block size, approximate (because of header).
            ulong usefulBlockSize = blockSize - 4;

            // We assume linked block (with a little header).
            ulong howMany = objectSize / (ulong)usefulBlockSize;
            ulong remainingInBlock = objectSize - howMany * (ulong)usefulBlockSize;

            // We compute the approximate.
            return remainingInBlock + 4 * howMany;
        }

        /// <summary>
        /// Obtains allocation block address and offset.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static ulong GetAllocationBlock(uint blockSize, ulong address, out uint offset)
        {
            if (address <= FirstSuperBlockAddress)
            {
                offset = 0; //< This must be unused.
                return FirstAllocationBlockAddress;
            }

            // We prepare.
            uint blockSizeBits = blockSize * 8;
            ulong allocBlockSize = (blockSizeBits + 1);
            ulong superBlockSize = (blockSizeBits * allocBlockSize + 1);
           
            address -= FirstSuperBlockAddress;

            // We first obtain super block.
            ulong superBlock = address / superBlockSize;

            // We obtain the allocation block.
            address -= superBlock * superBlockSize + 1;
            ulong allocIndex = address / allocBlockSize;

            // We need the offset.
            offset = (uint)(address - allocIndex * allocBlockSize);

            return FirstAllocationBlockAddress + 
                superBlock * superBlockSize +
                allocIndex * allocBlockSize;
        }

        /// <summary>
        /// Gets the index of the allocation block.
        /// </summary>
        /// <param name="blockSize">Size of the block.</param>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static ulong GetAllocationBlockIndex(uint blockSize, ulong address)
        {
            if (address <= FirstSuperBlockAddress)
            {
                throw new InvalidOperationException();
            }

            // We prepare.
            uint blockSizeBits = blockSize * 8;
            ulong allocBlockSize = (blockSizeBits + 1);
            ulong superBlockSize = (blockSizeBits * allocBlockSize + 1);

            address -= FirstSuperBlockAddress;

            // We first obtain super block.
            ulong superBlock = address / superBlockSize;

            // We obtain the allocation block.
            address -= superBlock * superBlockSize + 1;
            ulong allocIndex = address / allocBlockSize;

            return allocIndex + superBlock * blockSizeBits;
        }

        /// <summary>
        /// Gets the allocation block address.
        /// </summary>
        /// <param name="blockSize">Size of the block.</param>
        /// <param name="superBlock">The super block.</param>
        /// <param name="allocationIndex">Index of the allocation.</param>
        /// <returns></returns>
        public static ulong GetAllocationBlockAddress(uint blockSize, ulong allocationIndex)
        {
            // We prepare.
            uint blockSizeBits = blockSize * 8;
            ulong allocBlockSize = (blockSizeBits + 1);
            ulong superBlockSize = (blockSizeBits * allocBlockSize + 1);

            ulong superBlockIndex = allocationIndex / (ulong)blockSizeBits;

            // And return the address.
            return FirstAllocationBlockAddress +
                superBlockIndex * superBlockSize +
                allocationIndex * allocBlockSize;
        }

        /// <summary>
        /// Gets the next allocation block.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <returns></returns>
        public static ulong GetNextAllocationBlock(uint blockSize, ulong current, ulong blockCount)
        {
            // First check for overflow.
            ulong newAddress = current + blockSize*8 + 1;
            if (newAddress >= blockCount) return 0;


            uint unused1, unused2;
            if (GetSuperBlock(blockSize, current, out unused1) == GetSuperBlock(blockSize, newAddress, out unused2))
            {
                return newAddress;
            }
            else
            {
                // There is a superblock in between.
                return newAddress + 1;
            }
        }

        /// <summary>
        /// Gets the next super block.
        /// </summary>
        /// <param name="blockSize">Size of the block.</param>
        /// <param name="current">The current.</param>
        /// <param name="blockCount">The block count.</param>
        /// <returns></returns>
        public static ulong GetNextSuperBlock(uint blockSize, ulong current, ulong blockCount)
        {
            uint blockSizeBits = blockSize * 8;
            ulong allocBlockSize = (blockSizeBits + 1);
            ulong superBlockSize = (blockSizeBits * allocBlockSize + 1);

            // Compute next.
            current += superBlockSize;
            if (current > blockCount) return 0;
            return current;
        }

        /// <summary>
        /// Gets the next allocation block.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <returns></returns>
        public static ulong GetPrevAllocationBlock(uint blockSize, ulong current)
        {
            // First check for overflow.
            if (blockSize * 8 + 1 > current) return 0;
            ulong newAddress = current - blockSize * 8 + 1;


            uint unused1, unused2;
            if (GetSuperBlock(blockSize, current, out unused1) == 
                GetSuperBlock(blockSize, newAddress, out unused2))
            {
                return newAddress;
            }
            else
            {
                // There is a superblock in between.
                return newAddress - 1;
            }
        }

        /// <summary>
        /// Gets the super block.
        /// </summary>
        /// <param name="blockSize">The block size.</param>
        /// <param name="allocationBlock">The allocation block.</param>
        /// <param name="offset">The address.</param>
        /// <returns></returns>
        public static ulong GetSuperBlock(uint blockSize, ulong address, out uint offset)
        {
            // We compute some data.
            uint blockSizeBits = blockSize * 8;
            ulong allocBlockSize = (blockSizeBits + 1);
            ulong superBlockSize = (blockSizeBits * allocBlockSize + 1);

            // Make sure we offset right.
            address -= FirstSuperBlockAddress;

            // We first obtain super block.
            ulong superBlock = address / superBlockSize;

            // We obtain the allocation block.
            address -= superBlock * superBlockSize + 1;
            offset = (uint)(address / allocBlockSize);

            return superBlock * superBlockSize + FirstSuperBlockAddress;
        }

    }
}
