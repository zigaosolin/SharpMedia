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
using System.IO;
using SharpMedia.Database.Physical.Journalling;
using SharpMedia.Database.Physical.Caching;
using SharpMedia.Database.Physical.StorageStructs;

namespace SharpMedia.Database.Physical
{
    /// <summary>
    /// Enables writing to block stream.
    /// </summary>
    public class BlockStream 
    {
        #region Private Members
        ulong[] blocks;
        IReadService service;

        /// <summary>
        /// The block link unit.
        /// </summary>
        class LogicalBlockUnit
        {
            /// <summary>
            /// First block.
            /// </summary>
            public ulong First;

            /// <summary>
            /// Links of blocks.
            /// </summary>
            public uint Count = 1;

            /// <summary>
            /// Initializes a new instance of the <see cref="BlockLink"/> class.
            /// </summary>
            /// <param name="first">The first.</param>
            public LogicalBlockUnit(ulong first)
            {
                First = first;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockStream"/> class.
        /// </summary>
        /// <param name="blocks">The blocks.</param>
        public BlockStream(ulong[] blocks, IReadService service)
        {
            this.blocks = blocks;
            this.service = service;
        }

        /// <summary>
        /// Froms the base.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public static BlockStream FromBase(ulong block, IReadService service)
        {
            return new BlockStream(new ulong[] { block }, service);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the base address.
        /// </summary>
        /// <value>The base address.</value>
        public ulong BaseAddress
        {
            get
            {
                return blocks[0];
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        public bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can write.
        /// </summary>
        /// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
        public bool CanWrite
        {
            get
            {
                return service is IService;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deallocates this instance.
        /// </summary>
        public unsafe void Deallocate()
        {
            if (!CanWrite) throw new ArgumentException("Cannot deallocate.");

            IService writeService = service as IService;
            
            // We deallocate link.
            ulong addr = this.blocks[0];
            for(;addr != 0;)
            {
                // We read the link.
                Block block = writeService.Read(BlockType.BigObjectData, addr);
                uint lenght;
                ulong next;

                // We extract header.
                fixed (byte* p = block.Data)
                {
                    BlockLinkHeader* header = (BlockLinkHeader*)p;
                    next = header->NextBlock;
                    lenght = header->Sequential;
                }

                // We free all.
                for (uint i = 0; i < lenght; i++)
                {
                    writeService.DeAllocate(addr + i);
                }

                // We proceed to next.
                addr = next;
                
            }
        }

        /// <summary>
        /// Writes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public unsafe void Write(byte[] data)
        {
            if (!CanWrite) throw new ArgumentException("Cannot write.");

            IService writeService = service as IService;

            // We first organize blocks into logican units.
            ulong prev = 0;
            LogicalBlockUnit processingLink = new LogicalBlockUnit(0);
            List<LogicalBlockUnit> links = new List<LogicalBlockUnit>();
            foreach (ulong block in blocks)
            {
                if (block == prev + 1)
                {
                    processingLink.Count++;
                }
                else
                {
                    links.Add(processingLink);
                    processingLink = new LogicalBlockUnit(block);
                }
                prev = block;
            }
            links.Add(processingLink);
            
            // We have organised units, we can now actually write. We begin
            // at 1 because the first link is always an empty link.
            ulong dataOffset = 0;
            for (uint i = 1; i < (uint)links.Count; i++)
            {
                // We extract logical unit and next.
                LogicalBlockUnit link = links[(int)i];
                ulong next = i == links.Count - 1 ? 0 : links[(int)(i + 1)].First; 

                // We write to it, header first.
                Block block = new Block(service.BlockSize);
                fixed (byte* p = block.Data)
                {
                    BlockLinkHeader* header = (BlockLinkHeader*)p;
                    header->Sequential = link.Count;
                    header->NextBlock = next;
                }

                // TODO: use ulong(s) in future for copying, not bytes.

                // We copy to header block (make sure we do not acces out of it).
                ulong maxWrite = service.BlockSize - (ulong)sizeof(BlockLinkHeader);
                if (maxWrite > (ulong)data.LongLength - dataOffset) maxWrite = (ulong)data.LongLength - dataOffset;
                for (uint k = 0; k < maxWrite; k++)
                {
                    block.Data[k+(uint)sizeof(BlockLinkHeader)] = data[dataOffset++];
                }

                writeService.Write(BlockType.BigObjectData, link.First, block);

                // And the rest of it.
                for (uint j = 1; j < link.Count; j++)
                {
                    maxWrite = service.BlockSize;
                    if (maxWrite > (ulong)data.LongLength - dataOffset) maxWrite = (ulong)data.LongLength - dataOffset;
                    for (uint l = 0; l < maxWrite; l++)
                    {
                        block.Data[l] = data[dataOffset++];
                    }

                    writeService.Write(BlockType.BigObjectData, link.First + j, block);
                }
            }

        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public unsafe byte[] Read(ulong size)
        {
            // We prepare variables.
            byte[] data = new byte[size];
            ulong i = 0;

            // Begin loop.
            for(ulong addr = blocks[0]; addr != 0; )
            {
                uint seq;
                ulong next;

                // Read header first.
                Block headerBlock = service.Read(BlockType.BigObjectData, addr);
                fixed (byte* p = headerBlock.Data)
                {
                    BlockLinkHeader* header = (BlockLinkHeader*)p;
                    next = header->NextBlock;
                    seq = header->Sequential;
                }

                // First block special.
                for (uint j = (uint)sizeof(BlockLinkHeader); j < headerBlock.Data.Length && i < size; i++, j++)
                {
                    data[i] = headerBlock.Data[j];
                }

                // All sequential blocks.
                for (uint k = 1; k < seq; k++)
                {
                    Block dataBlock = service.Read(BlockType.BigObjectData, addr + k);

                    for (uint z = 0; z < dataBlock.Data.Length && i < size; i++, z++)
                    {
                        data[i] = dataBlock.Data[z];
                    }
                }

                // Next iteration.
                addr = next;

            }

            return data;
        }

        #endregion
    }
}
