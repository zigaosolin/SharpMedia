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
    /// Creates a new version of node.
    /// </summary>
    internal class CreateNewVersion : IOperation
    {
        #region Private Members
        ulong commonAddress;
        ulong version;
        BPlusTree versionTree;
        string defaultTSType;
        StreamOptions defaultTSOptions;
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNewVersion"/> class.
        /// </summary>
        /// <param name="commonAddress">The common address.</param>
        /// <param name="defaultTSType">Type of the default TS.</param>
        /// <param name="defaultTSOptions">The default TS options.</param>
        public CreateNewVersion(ulong commonAddress, string defaultTSType, StreamOptions defaultTSOptions)
        {
            if (defaultTSType.Length > TypedStreamHeader.MaxNameLength)
            {
                throw new InvalidOperationException("Too long type name");
            }
            this.commonAddress = commonAddress;
            this.defaultTSOptions = defaultTSOptions;
            this.defaultTSType = defaultTSType;
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public ulong Version
        {
            get
            {
                return version;
            }
        }

        #endregion

        #region IOperation Members

        public unsafe void Prepare(IReadService readService, out OperationStartupData data)
        {

            Block block = readService.Read(BlockType.NodeHeaderBlock, commonAddress);
            fixed (byte* p = block.Data)
            {
                NodeCommonHeader* header = (NodeCommonHeader*)p;
                version = header->CurrentVersionNumber;
                versionTree = new BPlusTree(header->VersionsBTree);
            }

            // We estimate how must we need.
            uint allocations = 15; //< Approximate estimate.

            data = new OperationStartupData(commonAddress, allocations, true);
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public unsafe void Execute(uint stage, IService service)
        {
            ulong versionAddress = service.AllocationContext.AllocateBlock();

            // 1) We first create new version node.
            Block versionBlock = new Block(service.BlockSize);
            fixed (byte* p = versionBlock.Data)
            {
                NodeVersionHeader* header = (NodeVersionHeader*)p;
                header->CreationTime = DateTime.Now;
                header->DefaultTypedStream = 0;
                header->HeaderMagic = NodeVersionHeader.Magic;
                header->ModifiedTime = DateTime.Now;
                header->NodeCommonAddress = commonAddress;
                header->StreamCount = 0; //< For now 0.
                header->VersionNumber = version;
            }

            // 2) We create default TS.
            ulong defaultTSAddress = service.AllocationContext.AllocateBlock();
            Block block = new Block(service.BlockSize);
            fixed (byte* p = block.Data)
            {
                TypedStreamHeader* header = (TypedStreamHeader*)p;
                header->ObjectsAddress = (defaultTSOptions & StreamOptions.SingleObject) != 0 
                    ? 0 : service.AllocationContext.CreateEmptyBTree();
                header->ObjectSize = 0;
                header->Options = defaultTSOptions;

                // Copy the name.
                int i;
                for (i = 0; i < defaultTSType.Length; i++)
                {
                    header->Type[i] = defaultTSType[i];
                }
                header->Type[i] = '\0';
            }

            service.Write(BlockType.TypedStreamHeader, defaultTSAddress, block);

            // 3) We add the default typed stream and write it.
            NodeVersionHelper.AddTypedStream(defaultTSAddress, defaultTSType, versionBlock);
            service.Write(BlockType.NodeHeaderBlock, versionAddress, versionBlock);

            // 4) And finnaly add to versions.
            ObjectInfo info = versionTree.Find(service, (uint)version.GetHashCode());
            if (info == null)
            {
                // Create block stream.
                VersionTag versionTag = new VersionTag(version.GetHashCode());
                versionTag.Add(version, versionAddress);
                byte[] versionTagData = Common.SerializeToArray(versionTag);
                BlockStream stream = service.AllocationContext.CreateBlockStream((ulong)versionTagData.LongLength);

                // Write the stream.
                stream.Write(versionTagData);

                // And to B+ tree.
                versionTree.Add(service, new ObjectInfo((uint)version.GetHashCode(), 
                    (ulong)versionTagData.LongLength, stream.BaseAddress)); ;
            }
            else
            {
                // We first create appropriate VersionTag and kill the stream.
                BlockStream stream = BlockStream.FromBase(info.Address, service);
                VersionTag versionTag = Common.DeserializeFromArray(stream.Read(info.Size)) as VersionTag;
                versionTag.Add(version, versionAddress);
                stream.Deallocate();

                // We write a new version tag.
                byte[] versionTagData = Common.SerializeToArray(versionTag);
                stream = service.AllocationContext.CreateBlockStream((ulong)versionTagData.LongLength);
                stream.Write(versionTagData);

                // We replace in B+ tree.
                versionTree.Replace(service, new ObjectInfo((uint)version.GetHashCode(),
                    (ulong)versionTagData.LongLength, versionAddress));

            }

            // 5) The new version is replaced in common.
            block = service.Read(BlockType.NodeHeaderBlock, commonAddress);
            fixed(byte* p = block.Data)
            {
                NodeCommonHeader* header = (NodeCommonHeader*)p;
                header->CurrentVersionNumber = version;
                header->CurrentVersionAddress = versionAddress;
            }

            service.Write(BlockType.NodeHeaderBlock, commonAddress, block);

        }

        #endregion
    }
}
