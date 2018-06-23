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
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct TypedStreamHeader
    {
        public const uint MaxNameLength = 190;

        /// <summary>
        /// Stream options specifier.
        /// </summary>
        public StreamOptions Options;

        /// <summary>
        /// The size of object, if single object.
        /// </summary>
        public ulong ObjectSize;

        /// <summary>
        /// Address to objects; it this is a "single object" typed stream, this
        /// is a link to block link that contains the bytes of object, otherwise
        /// a link to B+ tree.
        /// </summary>
        public ulong ObjectsAddress;

        /// <summary>
        /// A full type name.
        /// </summary>
        public fixed char Type[(int)MaxNameLength+1];
    }
}
