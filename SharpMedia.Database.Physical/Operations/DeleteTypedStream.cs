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
using SharpMedia.Database.Physical.Caching;
using SharpMedia.Database.Physical.StorageStructs;

namespace SharpMedia.Database.Physical.Operations
{

    /// <summary>
    /// Deletes a typed stream.
    /// </summary>
    internal class DeleteTypedStream : IOperation
    {
        #region Private Members
        ulong nodeVersionAddress;
        string tsToDelete = string.Empty;
        uint tsToDeleteIndex = uint.MaxValue;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTypedStream"/> class.
        /// </summary>
        /// <param name="nodeVersionAddress">The node version address.</param>
        /// <param name="toDelete">To delete.</param>
        public DeleteTypedStream(ulong nodeVersionAddress, string toDelete)
        {
            this.nodeVersionAddress = nodeVersionAddress;
            this.tsToDelete = toDelete;
        }

        

        #endregion

        #region IOperation Members

        public void Prepare(IReadService readService, out OperationStartupData data)
        {
            // We must resolve which TS to delete.
            if (tsToDeleteIndex == uint.MaxValue)
            {
                Block block = readService.Read(BlockType.NodeHeaderBlock, nodeVersionAddress);
                Block unused;
                tsToDeleteIndex = NodeVersionHelper.GetTypedStreamIndex(tsToDelete, block, readService, out unused);
            }

            data = new OperationStartupData(0, 0);
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public unsafe void Execute(uint stage, SharpMedia.Database.Physical.Journalling.IService service)
        {
            // 1) We first delete header node entry.
            Block versionNodeBlock = service.Read(BlockType.NodeHeaderBlock, nodeVersionAddress);
            ulong tsHeaderAddress = NodeVersionHelper.RemoveTypedStream(tsToDeleteIndex, versionNodeBlock);

            // 2) We delete all objects.
            Block tsHeader = service.Read(BlockType.TypedStreamHeader, tsHeaderAddress);
            fixed (byte* p = tsHeader.Data)
            {
                TypedStreamHeader* header = (TypedStreamHeader*)p;

                if ((header->Options & StreamOptions.SingleObject) != 0)
                {
                    BlockStream stream = BlockStream.FromBase(header->ObjectsAddress, service);
                    stream.Deallocate();
                }
                else
                {
                    // We have to delete whole link.
                    BPlusTree tree = new BPlusTree(header->ObjectsAddress);
                    List<ObjectInfo> all = tree.ListAll(service);
                    foreach (ObjectInfo info in all)
                    {
                        BlockStream stream = BlockStream.FromBase(info.Address, service);
                        stream.Deallocate();
                    }

                    // Make sure we get rid of tree.
                    tree.DeallocateTree(service);
                }
            }

            // 3) We delete typed stream.
            service.DeAllocate(tsHeaderAddress);

        }

        #endregion
    }
}
