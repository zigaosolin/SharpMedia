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
    /// Creates a child.
    /// </summary>
    internal class CreateChild : IOperation
    {
        #region Private Data
        BPlusTree childrenTree;
        ulong parentAddress;
        string childName;
        string defaultTSType;
        StreamOptions defaultTSOptions;

        ulong versionAddress;
        ulong commonAddress;
        #endregion

        #region Public members


        /// <summary>
        /// Gets the version address.
        /// </summary>
        /// <value>The version address.</value>
        public ulong VersionAddress
        {
            get
            {
                return versionAddress;
            }
        }

        /// <summary>
        /// Gets the common address.
        /// </summary>
        /// <value>The common address.</value>
        public ulong CommonAddress
        {
            get
            {
                return commonAddress;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateChild"/> class.
        /// </summary>
        /// <param name="childrenTree">The children tree.</param>
        /// <param name="parentAddress">The parent address.</param>
        /// <param name="childName">Name of the child.</param>
        /// <param name="defaultTSType">Type of the default TS.</param>
        /// <param name="ops">The ops.</param>
        public CreateChild(ulong childrenTree, ulong parentAddress,
            string childName, string defaultTSType, StreamOptions ops)
        {
            if (defaultTSType.Length > TypedStreamHeader.MaxNameLength)
            {
                throw new InvalidOperationException("Too long type name");
            }

            this.childrenTree = new BPlusTree(childrenTree);
            this.parentAddress = parentAddress;
            this.childName = childName;
            this.defaultTSOptions = ops;
            this.defaultTSType = defaultTSType;
        }

        #endregion

        #region IOperation Members

        public void Prepare(IReadService readService, out OperationStartupData data)
        {
            // We need to eximine common node address.
            uint allocations = childrenTree.InspectForAdding(readService, (uint)childName.GetHashCode());
            allocations += 15; // rought estimate (we use dynamic allocation in extreme cases).

            data = new OperationStartupData(parentAddress, allocations, true);
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public unsafe void Execute(uint stage, IService service)
        {

            // 1) We create the node common block.
            versionAddress = service.AllocationContext.AllocateBlock();
            commonAddress = service.AllocationContext.AllocateBlock();
            BPlusTree versionTS = new BPlusTree(service.AllocationContext.CreateEmptyBTree());

            // Fill common block and write.
            Block commonBlock = new Block(service.BlockSize);
            fixed (byte* p = commonBlock.Data)
            {
                NodeCommonHeader* header = (NodeCommonHeader*)p;
                header->ChildrenBTree = service.AllocationContext.CreateEmptyBTree();
                header->CurrentVersionAddress = versionAddress;
                header->CurrentVersionNumber = 0;
                header->HeaderMagic = NodeCommonHeader.Magic;
                header->ParentAddress = parentAddress;
                header->VersionsBTree = versionTS.RootAddress;
            }
            service.Write(BlockType.NodeHeaderBlock, commonAddress, commonBlock);

            // 2) We create the node default typed stream.
            Block tsblock = new Block(service.BlockSize);

            ulong tsBlock = service.AllocationContext.AllocateBlock();
            fixed (byte* p = tsblock.Data)
            {
                TypedStreamHeader* header = (TypedStreamHeader*)p;
                header->ObjectsAddress = (defaultTSOptions & StreamOptions.SingleObject) == 0 ?
                    service.AllocationContext.CreateEmptyBTree() : 0;
                header->Options = defaultTSOptions;
                header->ObjectSize = 0;

                int i;
                for (i = 0; i < defaultTSType.Length; i++)
                {
                    header->Type[i] = defaultTSType[i];
                }

                // Null terminate it.
                header->Type[i] = '\0';
            }
            
            service.Write(BlockType.TypedStreamHeader, tsBlock, tsblock);

            // 3) We create the node version block.
            // Fill version block and write.
            Block versionBlock = new Block(service.BlockSize);
            fixed (byte* pp = versionBlock.Data)
            {
                NodeVersionHeader* header = (NodeVersionHeader*)pp;
                header->CreationTime = DateTime.Now;
                header->DefaultTypedStream = 0;
                header->HeaderMagic = NodeVersionHeader.Magic;
                header->ModifiedTime = DateTime.Now;
                header->NodeCommonAddress = commonAddress;
                header->StreamCount = 0; //< Is actually one but NodeHelper changes this.
                header->VersionNumber = 0;
                
            }

            // We must add default typed stream to block.
            NodeVersionHelper.AddTypedStream(tsBlock, defaultTSType, versionBlock);
            service.Write(BlockType.NodeHeaderBlock, versionAddress, versionBlock);

            // 4) We add to children tree.
            ObjectInfo info = childrenTree.Find(service, (uint)childName.GetHashCode());
            if (info == null)
            {
                // We simply create a new object at address.
                ChildTag childTag = new ChildTag();
                childTag.HashCode = childName.GetHashCode();
                childTag.Add(childName, commonAddress);
                byte[] childData = Common.SerializeToArray(childTag);

                // We write it to block stream.
                BlockStream stream = service.AllocationContext.CreateBlockStream((ulong)childData.LongLength);
                stream.Write(childData);

                // We now add it to B+ tree.
                childrenTree.Add(service,new ObjectInfo((uint)childName.GetHashCode(), (ulong)childData.LongLength, stream.BaseAddress));
            }
            else
            {
                // The index at hash already exists, we must add it, first read it.
                BlockStream stream = BlockStream.FromBase(info.Address, service);
                ChildTag childTag = Common.DeserializeFromArray(stream.Read(info.Size)) as ChildTag;

                // Steam not used anymore.
                stream.Deallocate();

                // We modify child tag.
                childTag.Add(childName, commonAddress);
                byte[] childData = Common.SerializeToArray(childTag);

                // We must write it to new address.
                stream = service.AllocationContext.CreateBlockStream((ulong)childData.LongLength);
                stream.Write(childData);

                // Now we replace the data in tree.
                childrenTree.Replace(service, 
                    new ObjectInfo((uint)childName.GetHashCode(), (ulong)childData.LongLength, stream.BaseAddress));

            }

            // 5) Add version to typed stream.
            VersionTag versionTag = new VersionTag((0).GetHashCode());
            versionTag.Add(0, versionAddress);
            byte[] versionTagData = Common.SerializeToArray(versionTag);

            // Write to stream.
            BlockStream versionTagStream = service.AllocationContext.CreateBlockStream((ulong)versionTagData.LongLength);
            versionTagStream.Write(versionTagData);

            versionTS.Add(service, new ObjectInfo((uint)(0).GetHashCode(), 
                (ulong)versionTagData.LongLength, versionTagStream.BaseAddress));
        }

        #endregion
    }
}
