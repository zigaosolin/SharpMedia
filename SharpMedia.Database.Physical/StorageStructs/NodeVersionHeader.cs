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
using System.Runtime.InteropServices;

namespace SharpMedia.Database.Physical.StorageStructs
{

    /// <summary>
    /// Node version header contains information about certain node's version. Only
    /// most recent version can be written to. NodeVersionHeader can span through many
    /// blocks and is thus held in linked block.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct NodeVersionHeader
    {
        /// <summary>
        /// A magic number.
        /// </summary>
        public const ulong Magic = 0xFBA9C241E357D680;

        /// <summary>
        /// Must be magic or this header is invalid.
        /// </summary>
        public ulong HeaderMagic;

        /// <summary>
        /// Version number of this version, counting from 0 on.
        /// </summary>
        public ulong VersionNumber;

        /// <summary>
        /// Backlink to common node.
        /// </summary>
        public ulong NodeCommonAddress;

        /// <summary>
        /// Time of creation for this version.
        /// </summary>
        public DateTime CreationTime;

        /// <summary>
        /// Last modified time, ignored for now.
        /// </summary>
        public DateTime ModifiedTime;

        /// <summary>
        /// Default typed stream identifier.
        /// </summary>
        public uint DefaultTypedStream;

        /// <summary>
        /// Number of typed streams.
        /// </summary>
        public uint StreamCount;

        /// <summary>
        /// Typed stream indirections. 3-uint define one typed stream. The first
        /// one is the hash (converted to int), the second two can be constructed to
        /// address.
        /// </summary>
        /// <remarks>
        /// Cannot use struct because fixed is not allowed on structs.
        /// </remarks>
        public fixed uint TypedStreamData[1];

    }
}
