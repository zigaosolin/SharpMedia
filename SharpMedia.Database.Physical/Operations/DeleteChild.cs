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
    /// Delete a child and the whole sequent link.
    /// </summary>
    internal class DeleteChild : IOperation
    {
        #region Private Members
        ulong commonAddress;
        string childName;
        BPlusTree parentChildTree;
        List<DeleteChild> subOperationsToExecute = null;
        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteChild"/> class.
        /// </summary>
        /// <param name="commonAddress">The common address.</param>
        public DeleteChild(ulong commonAddress, ulong parentChildTree, string childName)
        {
            this.commonAddress = commonAddress;
            this.parentChildTree = new BPlusTree(parentChildTree);
            this.childName = childName;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Internal prepare
        /// </summary>
        /// <param name="readService"></param>
        /// <param name="data"></param>
        /// <param name="subOps"></param>
        unsafe void PrepareInternal(IReadService readService, List<DeleteChild> subOps)
        {

            // We inspect children.
            ulong childrenTS;
            Block block = readService.Read(BlockType.NodeHeaderBlock, commonAddress);
            fixed (byte* p = block.Data)
            {
                NodeCommonHeader* header = (NodeCommonHeader*)p;
                childrenTS = header->ChildrenBTree;
            }

            // We check children count.
            BPlusTree tree = new BPlusTree(childrenTS);
            List<ObjectInfo> objects = tree.ListAll(readService);

            // We update stage index/count.
            for (int j = 0; j < objects.Count; j++)
            {
                BlockStream childTagStream = BlockStream.FromBase(objects[j].Address, readService);
                ChildTag childTag = Common.DeserializeFromArray(childTagStream.Read(objects[j].Size)) as ChildTag;

                foreach (KeyValuePair<string, ulong> child in childTag.Children)
                {
                    DeleteChild subOp = new DeleteChild(child.Value, childrenTS, child.Key);
                    subOps.Add(subOp);
                }
            }

            subOps.Add(this);

        }

        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <param name="service">Service for operation.</param>
        unsafe void ExecuteInternal(IService service)
        {
            ulong childrenTS;
            ulong versionTS;

            // 1) We extract child TS and version TS
            Block block = service.Read(BlockType.NodeHeaderBlock, commonAddress);
            fixed (byte* p = block.Data)
            {
                NodeCommonHeader* header = (NodeCommonHeader*)p;
                childrenTS = header->ChildrenBTree;
                versionTS = header->VersionsBTree;
            }

            // 2) Can get rid of node.
            service.DeAllocate(commonAddress);

            // 3) We delete children TS, it should be empty (since deletes were already called on nodes).
            service.DeAllocate(childrenTS);

            // 4) We go through versions.
            BPlusTree versionsTree = new BPlusTree(versionTS);
            List<ObjectInfo> versions = versionsTree.ListAll(service);

            // 5) We delete each version.
            foreach (ObjectInfo info in versions)
            {
                // a) Read version tag.
                BlockStream versionTagStream = BlockStream.FromBase(info.Address, service);
                VersionTag versionTag = Common.DeserializeFromArray(versionTagStream.Read(info.Size)) as VersionTag;
                versionTagStream.Deallocate();

                foreach(KeyValuePair<ulong,ulong> versionToNode in versionTag.VersionAddress)
                {
                    block = service.Read(BlockType.NodeHeaderBlock, versionToNode.Value);

                    List<ulong> typedStreams = NodeVersionHelper.ListAllTypedStreamsAsAddresses(block, service);
                    
                    // b) Delete all typed streams.
                    for (int i = 0; i < typedStreams.Count; i++)
                    {
                        // 1) We delete the typed stream object.
                        block = service.Read(BlockType.TypedStreamHeader, typedStreams[i]);
                        fixed (byte* p = block.Data)
                        {
                            TypedStreamHeader* header = (TypedStreamHeader*)p;

                            // We delete single object.
                            if ((header->Options & StreamOptions.SingleObject) != 0)
                            {
                                if (header->ObjectsAddress != 0)
                                {
                                    BlockStream stream = BlockStream.FromBase(header->ObjectsAddress, service);
                                    stream.Deallocate();
                                }
                            }
                            else
                            {
                                // We delete all children.
                                BPlusTree tree = new BPlusTree(header->ObjectsAddress);
                                foreach (ObjectInfo info2 in tree.ListAll(service))
                                {
                                    BlockStream stream = BlockStream.FromBase(info2.Address, service);
                                    stream.Deallocate();
                                }
                            }
                        }

                        // 2) We also delete the header itself.
                        service.DeAllocate(typedStreams[i]);
                    }

                    // c) We deallocate version block.
                    service.DeAllocate(versionToNode.Value);
                }
                
            }

            // 6) We delete the tree.
            versionsTree.DeallocateTree(service);

            // 7) We must erase the node from root.
            ObjectInfo childInfo = parentChildTree.Find(service, (uint)childName.GetHashCode());
            BlockStream childTagStream = BlockStream.FromBase(childInfo.Address, service);
            byte[] childTagData = childTagStream.Read(childInfo.Size);
            ChildTag childTag = Common.DeserializeFromArray(childTagData) as ChildTag;
            childTagStream.Deallocate();

            if (childTag.Children.Count == 1)
            {
                // Simply delete it.
                parentChildTree.Remove(service, (uint)childName.GetHashCode(), 1, false);
            }
            else
            {
                // We have to replace it.
                childTag.Remove(childName);
                childTagData = Common.SerializeToArray(childTag);
                childTagStream = service.AllocationContext.CreateBlockStream((ulong)childTagData.LongLength);
                childTagStream.Write(childTagData);

                parentChildTree.Replace(service, new ObjectInfo((uint)childName.GetHashCode(),
                    (ulong)childTagData.LongLength, childTagStream.BaseAddress));
            }
            
        }

        #endregion

        #region IOperation Members

        public void Prepare(IReadService readService, out OperationStartupData data)
        {
            subOperationsToExecute = new List<DeleteChild>();
            PrepareInternal(readService, subOperationsToExecute);

            data = new OperationStartupData(commonAddress, 0, true);
        }

        public uint StageCount
        {
            get { return (uint)subOperationsToExecute.Count; }
        }

        public void Execute(uint stage, SharpMedia.Database.Physical.Journalling.IService service)
        {
            subOperationsToExecute[(int)stage].ExecuteInternal(service);
        }

        #endregion
    }
}
