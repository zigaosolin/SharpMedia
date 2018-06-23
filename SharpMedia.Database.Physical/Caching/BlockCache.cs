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

namespace SharpMedia.Database.Physical.Caching
{
    /// <summary>
    /// A block type, used for caching scenarios. Cache is free to ignore
    /// those flags but it is unseful to know the type of block.
    /// </summary>
    public enum BlockType
    {
        /// <summary>
        /// An allocation block, or super block.
        /// </summary>
        AllocationBlock,

        /// <summary>
        /// Either a common or version node header block.
        /// </summary>
        NodeHeaderBlock,

        /// <summary>
        /// A typed stream header block.
        /// </summary>
        TypedStreamHeader,

        /// <summary>
        /// A B+ tree versions block.
        /// </summary>
        BPlusTreeVersionsBlock,

        /// <summary>
        /// A B+ tree children block.
        /// </summary>
        BPlusTreeChildrenBlock,

        /// <summary>
        /// A normal B+ tree typed stream block.
        /// </summary>
        BPlusTreeBlock,

        /// <summary>
        /// Block of a small object.
        /// </summary>
        ObjectData,

        /// <summary>
        /// Block of a big object.
        /// </summary>
        BigObjectData,

        /// <summary>
        /// The journal header block.
        /// </summary>
        JournalSectorBlock,

        /// <summary>
        /// A regular journal block. Must never be cached (no point in caching).
        /// </summary>
        JournalBlock,

        /// <summary>
        /// Unknown, in known by previous write/read.
        /// </summary>
        Unknown,

        /// <summary>
        /// Such blocks are irrelavant and should not be cached.
        /// </summary>
        NoCache
    }

    /// <summary>
    /// A block cache class, abstracting out only cache read and write operations. Cache
    /// then implements only cache relevant operations (add and find from cache).
    /// </summary>
    public abstract class BlockCache
    {
        #region Private Members
        Provider.IProvider provider;
        #endregion

        #region Abstract Methods

        /// <summary>
        /// Writes the block to cache.
        /// </summary>
        /// <param name="type">The block type.</param>
        /// <param name="address">Address of block.</param>
        /// <param name="data">The data contained in block.</param>
        protected abstract void WriteToCache(BlockType type, ulong address, byte[] data);

        /// <summary>
        /// Reads a block from cache.
        /// </summary>
        /// <param name="type">The type of block.</param>
        /// <param name="address">The address of block.</param>
        /// <returns>Null, if cannot locate it in cache.</returns>
        protected abstract byte[] ReadFromCache(BlockType type, ulong address);

        #endregion

        #region Public Methods/Properties


        /// <summary>
        /// Gets the physical location.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public string GetPhysicalLocation(ulong address)
        {
            return provider.GetPhysicalLocation(address);
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
        /// Returns number of cached blocks.
        /// </summary>
        public abstract ulong CachedBlockCount
        {
            get;
        }

        /// <summary>
        /// Writes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="address">The address.</param>
        /// <param name="data">The data.</param>
        public void Write(BlockType type, ulong address, byte[] data)
        {
            if (provider == null || provider.BlockSize != data.Length)
            {
                throw new ArgumentException("data");
            }

            // We optionally write it to cache, if 
            if (type != BlockType.NoCache)
            {
                WriteToCache(type, address, data);
            }

            // We write to provider.
            provider.Write(address, data);
        }

        public byte[] Read(BlockType type, ulong address)
        {
            byte[] data = ReadFromCache(type, address);
            if (data != null)
            {
                return data;
            }

            // We must read it.
            data = provider.Read(address);
            if (data == null)
            {
                return null; //< Out of memory.
            }

            // We optionally write to cache.
            if (type != BlockType.NoCache)
            {
                WriteToCache(type, address, data);
            }

            return data;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with provider.
        /// </summary>
        /// <param name="p">The provider.</param>
        protected BlockCache(Provider.IProvider p)
        {
            provider = p;
        }

        #endregion
    }
}
