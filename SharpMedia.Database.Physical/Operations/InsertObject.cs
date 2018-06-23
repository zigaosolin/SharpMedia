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

namespace SharpMedia.Database.Physical.Operations
{

    /// <summary>
    /// Insert object into B+ tree.
    /// </summary>
    internal class InsertObject : IOperation
    {
        #region Private Members
        uint index;
        byte[] objectData;
        BPlusTree tree;
        bool before;
        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertObject"/> class.
        /// </summary>
        /// <param name="btreeAddress">The btree address.</param>
        /// <param name="index">The index.</param>
        /// <param name="before">if set to <c>true</c> [before].</param>
        /// <param name="objectData">The object data.</param>
        public InsertObject(ulong btreeAddress, uint index, bool before, byte[] objectData)
        {
            this.tree = new BPlusTree(btreeAddress);
            this.index = index;
            this.before = before;
            this.objectData = objectData;
        }

        #endregion

        #region IOperation Members

        public void Prepare(IReadService readService, out OperationStartupData data)
        {
            // We comoute how many blocks are needed.
            uint allocations = tree.InspectForInserting(readService, index, before);
            allocations += BlockHelper.MaxBlocksForObject(readService.BlockSize, (ulong)objectData.LongLength);

            data = new OperationStartupData(tree.RootAddress, allocations);
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public void Execute(uint stage, SharpMedia.Database.Physical.Journalling.IService service)
        {
            // 1) We create the object.
            BlockStream stream = service.AllocationContext.CreateBlockStream((ulong)objectData.LongLength);
            stream.Write(objectData);

            // 2) We insert to B+ tree.
            tree.Insert(service, before, new ObjectInfo(index, (ulong)objectData.LongLength, stream.BaseAddress));
        }

        #endregion
    }
}
