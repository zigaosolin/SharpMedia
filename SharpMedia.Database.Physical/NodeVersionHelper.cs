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
using SharpMedia.Database.Physical.Journalling;
using System.Runtime.InteropServices;
using SharpMedia.Database.Physical.StorageStructs;
using SharpMedia.Database.Physical.Caching;
using System.Collections.Specialized;

namespace SharpMedia.Database.Physical
{
    /// <summary>
    /// The Node version helper.
    /// </summary>
    internal static class NodeVersionHelper
    {
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        private struct TSData
        {
            /// <summary>
            /// The hash of name.
            /// </summary>
            public int Hash;

            /// <summary>
            /// The address of typed stream.
            /// </summary>
            public ulong Address;
        }

        /// <summary>
        /// Adds the typed stream to node version.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        public static unsafe void AddTypedStream(ulong address, string type, Block block)
        {
            fixed (byte* pp = block.Data)
            {
                // We obtain the data.
                NodeVersionHeader* header = (NodeVersionHeader*)pp;
                TSData* data = (TSData*)&header->TypedStreamData[0];

                // We write the data.
                data[header->StreamCount].Address = address;
                data[header->StreamCount++].Hash = type.GetHashCode();
            }
        }

        /// <summary>
        /// Lists all typed streams.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <returns></returns>
        public static unsafe StringCollection ListAllTypedStreams(Block block, Journalling.IReadService service)
        {
            StringCollection collection = new StringCollection();
            fixed (byte* p = block.Data)
            {
                NodeVersionHeader* header = (NodeVersionHeader*)p;
                TSData* data = (TSData*)&header->TypedStreamData[0];

                // We go through all.
                for (uint i = 0; i < header->StreamCount; i++)
                {
                    Block tsHeader = service.Read(BlockType.TypedStreamHeader, data[i].Address);
                    fixed (byte* pp = tsHeader.Data)
                    {
                        TypedStreamHeader* h = (TypedStreamHeader*)pp;
                        collection.Add(new string(h->Type));
                    }
                }
            }

            return collection;
        }

        /// <summary>
        /// Lists all typed streams.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <returns></returns>
        public static unsafe List<ulong> ListAllTypedStreamsAsAddresses(Block block, Journalling.IReadService service)
        {
            List<ulong> collection = new List<ulong>();
            fixed (byte* p = block.Data)
            {
                NodeVersionHeader* header = (NodeVersionHeader*)p;
                TSData* data = (TSData*)&header->TypedStreamData[0];

                // We go through all.
                for (uint i = 0; i < header->StreamCount; i++)
                {
                    collection.Add(data[i].Address);
                }
            }

            return collection;
        }


        /// <summary>
        /// Gets the default type.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <returns></returns>
        public static unsafe ulong GetDefaultTypeBlock(Block block)
        {
            fixed (byte* p = block.Data)
            {
                NodeVersionHeader* header = (NodeVersionHeader*)p;
                TSData* data = (TSData*)&header->TypedStreamData[0];
                return data[header->DefaultTypedStream].Address;
            }
        }

        /// <summary>
        /// Removes the typed stream index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="block">The block.</param>
        /// <returns></returns>
        public static unsafe ulong RemoveTypedStream(uint index, Block block)
        {
            ulong toReturn = 0;
            fixed (byte* p = block.Data)
            {
                NodeVersionHeader* header = (NodeVersionHeader*)p;
                TSData* data = (TSData*)&header->TypedStreamData[0];

                toReturn = data[index].Address;

                // We check if last, we can simply remove it.
                if (header->StreamCount == index + 1)
                {
                    header->StreamCount--;
                }
                // We replace the last with this.
                else
                {
                    data[index].Address = data[header->StreamCount - 1].Address;
                    data[index].Hash = data[header->StreamCount - 1].Hash;

                    // We check if default TS was last that was moved.
                    if (header->DefaultTypedStream == header->StreamCount - 1)
                    {
                        header->DefaultTypedStream = index;
                    }
                    header->StreamCount--;
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Determines whether typed stream with type is the specified block.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="type">The type.</param>
        public static unsafe bool IsTypedStreamWithType(Block block, string type)
        {
            fixed (byte* p = block.Data)
            {
                TypedStreamHeader* header = (TypedStreamHeader*)p;
                return new string(header->Type) == type;
            }
        }

        /// <summary>
        /// Gets the index of the typed stream.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="block">The block.</param>
        /// <param name="service">The service.</param>
        /// <param name="typedStreamBlock">The typed stream block.</param>
        /// <returns></returns>
        public static unsafe uint GetTypedStreamIndex(string name, Block block,
            IReadService service, out Block typedStreamBlock)
        {
            ulong unused;
            return GetTypedStreamIndex(name, block, service, out typedStreamBlock, out unused);
        }

        /// <summary>
        /// Gets the index of the typed stream.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="block">The block.</param>
        /// <param name="service">The service.</param>
        /// <param name="typedStreamBlock">The typed stream block.</param>
        /// <returns></returns>
        public static unsafe uint GetTypedStreamIndex(string name, Block block, 
            IReadService service, out Block typedStreamBlock, out ulong address)
        {

            int nameHash = name.GetHashCode();
            
            fixed (byte* p = block.Data)
            {
                NodeVersionHeader* header = (NodeVersionHeader*)p;
                TSData* data = (TSData*)&header->TypedStreamData[0];


                for (uint i = 0; i < header->StreamCount; i++)
                {
                    
                    if (nameHash == data[i].Hash)
                    {
                        // We check it if it is the correct one.
                        typedStreamBlock = service.Read(BlockType.TypedStreamHeader, data[i].Address);

                        if (IsTypedStreamWithType(typedStreamBlock, name))
                        {
                            address = data[i].Address;
                            return i;
                        }
                    }

                }
            }

            // Noone found.
            typedStreamBlock = null;
            address = 0;
            return uint.MaxValue;
        }

        /// <summary>
        /// Gets the typed stream address.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="block">The block.</param>
        /// <returns></returns>
        public static ulong GetTypedStreamAddress(uint index, Block block)
        {
            int hash;
            return GetTypedStreamAddress(index, block, out hash);
        }

        /// <summary>
        /// Gets the typed stream address and hash.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="block">The block.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        public static unsafe ulong GetTypedStreamAddress(uint index, Block block, out int hash)
        {
            fixed (byte* p = block.Data)
            {
                NodeVersionHeader* header = (NodeVersionHeader*)p;
                TSData* data = (TSData*)&header->TypedStreamData[0];

                // Must watch for overflows.
                hash = data[index].Hash;
                return data[index].Address;
            }
        }
    }
}
