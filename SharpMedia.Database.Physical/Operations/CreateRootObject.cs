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
using SharpMedia.Database.Physical.StorageStructs;
using SharpMedia.Database.Physical.Journalling;
using SharpMedia.Database.Physical.Caching;

namespace SharpMedia.Database.Physical.Operations
{

    /// <summary>
    /// Creates a root object.
    /// </summary>
    internal class CreateRootObject : IOperation
    {
        #region IOperation Members

        public void Prepare(SharpMedia.Database.Physical.Journalling.IReadService readService, 
            out SharpMedia.Database.Physical.Journalling.OperationStartupData data)
        {
            data = new SharpMedia.Database.Physical.Journalling.OperationStartupData(0, 10);
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public unsafe void Execute(uint stage, IService service)
        {
            string defaultTSType = typeof(object).FullName;

            // 1) We create the node common block.
            ulong versionAddress = service.AllocationContext.AllocateBlock();
            ulong commonAddress = service.AllocationContext.AllocateBlock();
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
                header->ParentAddress = 0;
                header->VersionsBTree = versionTS.RootAddress;
            }
            service.Write(BlockType.NodeHeaderBlock, commonAddress, commonBlock);

            // 2) We create the node default typed stream.
            Block tsblock = new Block(service.BlockSize);

            fixed (byte* p = tsblock.Data)
            {
                TypedStreamHeader* header = (TypedStreamHeader*)p;
                header->ObjectsAddress = service.AllocationContext.AllocateBlock();
                header->Options = StreamOptions.None;
                header->ObjectSize = 0;

                int i;
                for (i = 0; i < defaultTSType.Length; i++)
                {
                    header->Type[i] = defaultTSType[i];
                }

                // Null terminate it.
                header->Type[i] = '\0';
            }
            ulong tsBlock = service.AllocationContext.AllocateBlock();
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

            // 4) We must create versions
            VersionTag versionTag = new VersionTag((0).GetHashCode());
            versionTag.Add(0, versionAddress);
            byte[] versionTagData = Common.SerializeToArray(versionTag);

            // Write to stream.
            BlockStream versionTagStream = service.AllocationContext.CreateBlockStream((ulong)versionTagData.LongLength);
            versionTagStream.Write(versionTagData);

            versionTS.Add(service, new ObjectInfo((uint)(0).GetHashCode(),
                (ulong)versionTagData.LongLength, versionTagStream.BaseAddress));

            // 5) We must write to database header.
            Block block = service.Read(BlockType.NoCache, 0);
            fixed (byte* ppp = block.Data)
            {
                DatabaseHeader* header = (DatabaseHeader*)ppp;
                header->RootObjectAddress = commonAddress;
            }

            // Write the header.
            service.Write(BlockType.NoCache, 0, block);
        }

        #endregion
    }
}
