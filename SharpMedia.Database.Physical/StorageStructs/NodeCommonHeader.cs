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
    /// A child tag, that contains information about children.
    /// </summary>
    [Serializable]
    public class ChildTag
    {
        /// <summary>
        /// Name of child, must be valid.
        /// </summary>
        public string Name;

        /// <summary>
        /// Common node header of this child.
        /// </summary>
        public ulong CommonNodeHeader;
    }

    /// <summary>
    /// A common node header is a structure that represent non-version related information
    /// about certain node and is shared between many versions. It is at most one block in length.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct NodeCommonHeader
    {
        /// <summary>
        /// A magic number that only validates header.
        /// </summary>
        public const ulong Magic = 0xABCDEF0123456789;

        /// <summary>
        /// The magic, must have value of magic.
        /// </summary>
        public ulong HeaderMagic;

        /// <summary>
        /// Address of parent's common node header.
        /// </summary>
        public ulong ParentAddress;

        /// <summary>
        /// The current's (modifiable) version's address.
        /// </summary>
        public ulong CurrentVersionAddress;

        /// <summary>
        /// Current version's number, all previous versions one less.
        /// </summary>
        public ulong CurrentVersionNumber;

        /// <summary>
        /// A version typed stream, it exists if more than one version is present
        /// and finds versions swiftly.
        /// </summary>
        public ulong VersionsBTree;

        /// <summary>
        /// The children typed stream, containing ChildTag type.
        /// </summary>
        public ulong ChildrenBTree;


        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCommonHeader"/> class.
        /// </summary>
        /// <param name="parentAddress">The parent address.</param>
        /// <param name="nodeAddress">The node address.</param>
        /// <param name="childrenTS">The children TS.</param>
        public NodeCommonHeader(ulong parentAddress, ulong nodeAddress, ulong childrenTS)
        {
            HeaderMagic = Magic;
            ParentAddress = parentAddress;
            CurrentVersionAddress = nodeAddress;
            CurrentVersionNumber = 1;
            ChildrenBTree = childrenTS;
            VersionsBTree = 0;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get
            {
                return HeaderMagic == Magic;
            }
        }
    }
}
