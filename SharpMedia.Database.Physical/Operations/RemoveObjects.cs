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
    /// Removes objects from B+ tree.
    /// </summary>
    internal class RemoveObjects : IOperation
    {
        #region Private Members
        uint index;
        uint count;
        BPlusTree tree;
        bool decreaseCount;
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveObjects"/> class.
        /// </summary>
        /// <param name="btree">The btree.</param>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        /// <param name="decreaseCount">if set to <c>true</c> [decrease count].</param>
        public RemoveObjects(ulong btree, uint index, uint count, bool decreaseCount)
        {
            this.tree = new BPlusTree(btree);
            this.index = index;
            this.count = count;
            this.decreaseCount = decreaseCount;
        }

        #endregion

        #region IOperation Members

        public void Prepare(IReadService readService, out OperationStartupData data)
        {
            data = new OperationStartupData(tree.RootAddress, 
                tree.InspectForRemoving(readService, index, count));
            
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public void Execute(uint stage, IService service)
        {
            // 1) We first delete all objects.
            List<ObjectInfo> all = tree.List(service, index, count);
            foreach (ObjectInfo info in all)
            {
                BlockStream stream = BlockStream.FromBase(info.Address, service);
                stream.Deallocate();
            }

            // 2) We delete them from B+ tree.
            tree.Remove(service, index, count, decreaseCount);
        }

        #endregion
    }
}
