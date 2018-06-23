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
using SharpMedia.Database.Physical.Operations;
using SharpMedia.Database.Physical.Caching;
using SharpMedia.Database.Physical.StorageStructs;
using System.Collections.Specialized;

namespace SharpMedia.Database.Physical
{
    /// <summary>
    /// The physical node.
    /// </summary>
    public class PhysicalNode : IDriverNode
    {
        #region Private Members
        Journalling.IJournal journal;
        ulong commonAddress;
        ulong versionAddress;
        string name;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalNode"/> class.
        /// </summary>
        /// <param name="commonAddress">The common address.</param>
        /// <param name="versionAddress">The version address.</param>
        /// <param name="journal">The journal.</param>
        public PhysicalNode(string name, ulong commonAddress, ulong versionAddress, Journalling.IJournal journal)
        {
            this.name = name;
            this.journal = journal;
            this.commonAddress = commonAddress;
            this.versionAddress = versionAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalNode"/> class.
        /// </summary>
        /// <param name="commonAddress">The common address.</param>
        /// <param name="journal">The journal.</param>
        public unsafe PhysicalNode(string name, ulong commonAddress, Journalling.IJournal journal)
        {
            this.name = name;
            this.commonAddress = commonAddress;
            this.journal = journal;

            // We must extract most current version.
            Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, commonAddress);
            fixed (byte* p = block.Data)
            {
                NodeCommonHeader* header = (NodeCommonHeader*)p;
                this.versionAddress = header->CurrentVersionAddress;
            }
        }

        #endregion

        #region IDriverNode Members

        public unsafe string Name
        {
            set 
            {
                ulong parentAddress, childrenTS;

                // We have to get root children address.
                Block commonBlock = journal.ReadService.Read(BlockType.NodeHeaderBlock, commonAddress);
                fixed (byte* p = commonBlock.Data)
                {
                    NodeCommonHeader* header = (NodeCommonHeader*)p;
                    parentAddress = header->ParentAddress;
                }

                // We have to extract the parent children B+ tree.
                Block parentCommon = journal.ReadService.Read(BlockType.NodeHeaderBlock, parentAddress);
                fixed (byte* pp = parentCommon.Data)
                {
                    NodeCommonHeader* header = (NodeCommonHeader*)pp;
                    childrenTS = header->ChildrenBTree;
                }

                // We execute the operation.
                journal.Execute(new RenameNode(name, value, childrenTS));
                name = value;
            }
        }

        public unsafe string DefaultType
        {
            get 
            { 
                // We open the version node.
                Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, versionAddress);
                block = journal.ReadService.Read(BlockType.TypedStreamHeader, NodeVersionHelper.GetDefaultTypeBlock(block));

                // We have typed stream header block, extract default type.
                fixed(byte* p = block.Data)
                {
                    TypedStreamHeader* header = (TypedStreamHeader*)p;
                    return new string(header->Type);
                }
            }
        }

        public ulong ByteOverhead
        {
            get { return 0; }
        }

        public IDriverTypedStream GetTypedStream(OpenMode mode, string type)
        {
            Block versionBlock = journal.ReadService.Read(BlockType.NodeHeaderBlock, versionAddress);

            // We extract the typed stream.
            Block tsHeader;
            ulong address;
            NodeVersionHelper.GetTypedStreamIndex(type, versionBlock, journal.ReadService, out tsHeader, out address);
            if (tsHeader == null) return null;

            // We create the typed stream based on header.
            return new PhysicalTypedStream(address, journal);
        }

        public void AddTypedStream(string type, StreamOptions flags)
        {
            journal.Execute(new AddTypedStream(versionAddress, type, flags));
        }

        public void RemoveTypedStream(string type)
        {
            journal.Execute(new DeleteTypedStream(versionAddress, type));
        }

        public void ChangeDefaultStream(string type)
        {
            journal.Execute(new ChangeDefaultTypedStream(versionAddress, type));
        }

        public unsafe ulong Version
        {
            get 
            {
                Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, versionAddress);
                fixed (byte* p = block.Data)
                {
                    NodeVersionHeader* header = (NodeVersionHeader*)p;
                    return header->VersionNumber;
                }
            }
        }

        public unsafe IDriverNode GetVersion(ulong version)
        {
            // 1) Extract version typed stream.
            ulong versionTS;
            Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, commonAddress);
            fixed (byte* p = block.Data)
            {
                NodeCommonHeader* header = (NodeCommonHeader*)p;
                versionTS = header->VersionsBTree;
            }

            // 2) Search for version
            BPlusTree versionTree = new BPlusTree(versionTS);
            ObjectInfo info = versionTree.Find(journal.ReadService, (uint)version.GetHashCode());
            if (info == null) return null;

            // 3) Locate the version.
            BlockStream versionTagStream = BlockStream.FromBase(info.Address, journal.ReadService);
            VersionTag versionTag = Common.DeserializeFromArray(versionTagStream.Read(info.Size)) as VersionTag;

            ulong? rVersionAddress = versionTag.Find(version);
            if (!rVersionAddress.HasValue) return null;

            // 4) Return the node.
            return new PhysicalNode(name, commonAddress, rVersionAddress.Value, journal);
        }

        public IDriverNode CreateNewVersion(string defaultType, StreamOptions flags)
        {
            Operations.CreateNewVersion op = new Operations.CreateNewVersion(commonAddress, defaultType, flags);
            journal.Execute(op);
            return new PhysicalNode(name, commonAddress, op.Version, journal);
        }

        public unsafe IDriverNode Find(string path)
        {
            // 1) Locate children TS first.
            ulong childrenTS;
            Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, commonAddress);
            fixed (byte* p = block.Data)
            {
                NodeCommonHeader* header = (NodeCommonHeader*)p;
                childrenTS = header->ChildrenBTree;
            }

            // 2) Search in B+ tree.
            BPlusTree tree = new BPlusTree(childrenTS);
            ObjectInfo info = tree.Find(journal.ReadService, (uint)path.GetHashCode());
            if (info == null) return null;

            // 3) Find the actual object address.
            BlockStream stream = BlockStream.FromBase(info.Address, journal.ReadService);
            ChildTag childTag = Common.DeserializeFromArray(stream.Read(info.Size)) as ChildTag;
            ulong? address = childTag.Find(path);
            if (!address.HasValue) return null; // May only match hash value.

            // Return the physical node (most current version).
            return new PhysicalNode(path, address.Value, journal);
        }

        public unsafe IDriverNode CreateChild(string name, string defaultType, StreamOptions flags)
        {
            ulong childTS;
            Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, commonAddress);
            fixed(byte* p = block.Data)
            {
                NodeCommonHeader* header = (NodeCommonHeader*)p;
                childTS = header->ChildrenBTree;
            }

            Operations.CreateChild op = new Operations.CreateChild(childTS, commonAddress, name, defaultType, flags);
            journal.Execute(op);

            // We create it now.
            return new PhysicalNode(name, op.CommonAddress, op.VersionAddress, journal);
        }

        public unsafe void DeleteChild(string name)
        {
            // 1) Find children TS.
            ulong childTS = 0;
            Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, commonAddress);
            fixed (byte* p = block.Data)
            {
                NodeCommonHeader* header = (NodeCommonHeader*)p;
                childTS = header->ChildrenBTree;
            }

            // 2) Find child.
            BPlusTree tree = new BPlusTree(childTS);
            ObjectInfo info = tree.Find(journal.ReadService, (uint)name.GetHashCode());
            BlockStream stream = BlockStream.FromBase(info.Address, journal.ReadService);
            ChildTag tag = Common.DeserializeFromArray(stream.Read(info.Size)) as ChildTag;
            ulong? childAddress = tag.Find(name);
            
            // Execute operation
            Operations.DeleteChild deleteChild = new DeleteChild(childAddress.Value, childTS, name);
            journal.Execute(deleteChild);
        }

        public void DeleteVersion(ulong version)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public unsafe DateTime CreationTime
        {
            get 
            {
                Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, versionAddress);
                fixed (byte* p = block.Data)
                {
                    NodeVersionHeader* header = (NodeVersionHeader*)p;
                    return header->CreationTime;
                }
            }
        }

        public unsafe DateTime LastModifiedTime
        {
            get 
            {
                Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, versionAddress);
                fixed (byte* p = block.Data)
                {
                    NodeVersionHeader* header = (NodeVersionHeader*)p;
                    return header->ModifiedTime;
                }
            }
        }

        public DateTime LastReadTime
        {
            get { return DateTime.MinValue; }
        }

        public string PhysicalLocation
        {
            get { return journal.GetPhysicalLocation(versionAddress); }
        }

        public System.Collections.Specialized.StringCollection TypedStreams
        {
            get 
            { 
                Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, versionAddress);
                return NodeVersionHelper.ListAllTypedStreams(block, journal.ReadService);
            }
        }

        public unsafe System.Collections.Specialized.StringCollection Children
        {
            get 
            {
                // We get children TS.
                ulong childTS;
                Block block = journal.ReadService.Read(BlockType.NodeHeaderBlock, commonAddress);
                fixed (byte* p = block.Data)
                {
                    NodeCommonHeader* header = (NodeCommonHeader*)p;
                    childTS = header->ChildrenBTree;
                }

                // We extract all entries.
                BPlusTree tree = new BPlusTree(childTS);
                List<ObjectInfo> all = tree.ListAll(journal.ReadService);
                StringCollection collection = new StringCollection();

                // We go through each.
                foreach (ObjectInfo info in all)
                {
                    BlockStream stream = BlockStream.FromBase(info.Address, journal.ReadService);
                    ChildTag childTag = Common.DeserializeFromArray(stream.Read(info.Size)) as ChildTag;

                    foreach (KeyValuePair<string, ulong> pair in childTag.Children)
                    {
                        collection.Add(pair.Key);
                    }
                }

                // Return collection.
                return collection;
            }
        }

        public ulong[] AvailableVersions
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
