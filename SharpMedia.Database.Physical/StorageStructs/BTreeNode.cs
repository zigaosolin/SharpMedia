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
    /// Flags of B+ tree node.
    /// </summary>
    internal enum BTreeNodeOptions : short
    {
        /// <summary>
        /// The node is a non-data node.
        /// </summary>
        NonLeaf,

        /// <summary>
        /// Node is leaf and contains data.
        /// </summary>
        Leaf
    }

    /// <summary>
    /// A B-tree node.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct BTreeNode
    {
        /// <summary>
        /// Flags 
        /// </summary>
        public BTreeNodeOptions Flags;

        /// <summary>
        /// Number of elements following, maximum count is limited by block size.
        /// </summary>
        public ushort Count;

        /// <summary>
        /// The data (must be cast to BTreeNodePart*)
        /// </summary>
        public ulong Data;

    }

    /// <summary>
    /// A B+ tree node part, 20 bytes each.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct BTreeNodePart
    {
        /// <summary>
        /// The index data.
        /// </summary>
        public uint ObjectIndex;

        /// <summary>
        /// The size of object.
        /// </summary>
        public ulong ObjectSize;

        /// <summary>
        /// The address of the block, either next B+ node or
        /// object link.
        /// </summary>
        public ulong BlockAddress;

    }
}
