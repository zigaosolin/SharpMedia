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
using System.Diagnostics;

namespace SharpMedia.Database.Physical.Operations
{

    /// <summary>
    /// Adds object to node. If object exists at index, it replaces it.
    /// </summary>
    internal class AddObject : IOperation
    {
        #region Private Members
        uint index;
        byte[] objectData;
        BPlusTree tree;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructs operation.
        /// </summary>
        /// <param name="treeAddress"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public AddObject(ulong treeAddress, uint index, byte[] data)
        {
            this.tree = new BPlusTree(treeAddress);
            this.index = index;
            this.objectData = data;
        }

        #endregion

        #region IOperation Members

        public void Prepare(SharpMedia.Database.Physical.Journalling.IReadService readService, out SharpMedia.Database.Physical.Journalling.OperationStartupData data)
        {
            // We fill in the data.
            uint allocations = tree.InspectForAdding(readService, index);
            allocations += BlockHelper.MaxBlocksForObject(readService.BlockSize, (ulong)objectData.LongLength);

            data = new OperationStartupData(tree.RootAddress, allocations);
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public void Execute(uint stage, SharpMedia.Database.Physical.Journalling.IService service)
        {
            Debug.Assert(stage == 0);

            // 1) We first write to allocated block stream.
            BlockStream stream = service.AllocationContext.CreateBlockStream((ulong)objectData.LongLength);
            stream.Write(objectData);

            // 2) We may need to delete object at index.
            ObjectInfo data = tree.Find(service, index);
            if (data != null)
            {
                // Deallocate link.
                BlockStream stream2 = BlockStream.FromBase(data.Address, service);
                stream2.Deallocate();

                // We replace the entry (cheaper than delete and rewrite).
                tree.Replace(service, new ObjectInfo(index, (ulong)objectData.LongLength, stream.BaseAddress));
            }
            else
            {
                // 3) We execute insert operation.
                tree.Add(service, new ObjectInfo(index, (ulong)objectData.LongLength, stream.BaseAddress));
            }
        }

        #endregion
    }
}
