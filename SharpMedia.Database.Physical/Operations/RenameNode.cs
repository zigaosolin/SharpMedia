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
    /// Renames the node.
    /// </summary>
    internal class RenameNode : IOperation
    {
        #region Private Members
        string newName;
        string prevName;
        BPlusTree childrenTree;
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="RenameNode"/> class.
        /// </summary>
        /// <param name="prevName">Name of the prev.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="childrenTree">The children tree.</param>
        public RenameNode(string prevName, string newName, ulong childrenTree)
        {
            this.childrenTree = new BPlusTree(childrenTree);
            this.newName = newName;
            this.prevName = prevName;
        }

        #endregion

        #region IOperation Members

        public void Prepare(IReadService readService, out OperationStartupData data)
        {
            // May need dynamic allocation (we place smallest allocation limit quite high).
            data = new OperationStartupData(childrenTree.RootAddress, 8, true);
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public void Execute(uint stage, SharpMedia.Database.Physical.Journalling.IService service)
        {
            // 1) We read previous object placement and change it.
            ObjectInfo info = childrenTree.Find(service, (uint)prevName.GetHashCode());
            BlockStream stream = BlockStream.FromBase(info.Address, service);
            ChildTag childTag = Common.DeserializeFromArray(stream.Read(info.Size)) as ChildTag;
            childTag.Remove(prevName);

            // Remove it if empty.
            if (childTag.IsEmpty)
            {
                childrenTree.Remove(service, (uint)prevName.GetHashCode(), 1, false);
            }
            else
            {
                // Update the entry (now without this child).
                byte[] childTagData = Common.SerializeToArray(childTag);
                stream = service.AllocationContext.CreateBlockStream((ulong)childTagData.LongLength);
                stream.Write(childTagData);

                childrenTree.Replace(service,
                    new ObjectInfo((uint)prevName.GetHashCode(), (ulong)childTagData.LongLength, stream.BaseAddress));
            }

            // 3) We create new and insert it into tree.
            ObjectInfo info2 = childrenTree.Find(service, (uint)newName.GetHashCode());
            if (info2 == null)
            {
                // We create new tag.
                childTag = new ChildTag();
                childTag.Add(newName, info.Address);
                byte[] childTagData = Common.SerializeToArray(childTag);
                stream = service.AllocationContext.CreateBlockStream((ulong)childTagData.LongLength);
                stream.Write(childTagData);

                // And we add child.
                childrenTree.Add(service,
                    new ObjectInfo((uint)newName.GetHashCode(), (ulong)childTagData.LongLength, stream.BaseAddress));
            }
            else
            {
                // We append it and release previous tag.
                stream = BlockStream.FromBase(info2.Address, service);
                childTag = Common.DeserializeFromArray(stream.Read(info2.Size)) as ChildTag;
                stream.Deallocate();
                
                // We modify and rewrite it.
                childTag.Add(newName, info.Address);
                byte[] childTagData = Common.SerializeToArray(childTag);
                stream = service.AllocationContext.CreateBlockStream((ulong)childTagData.LongLength);
                stream.Write(childTagData);

                // We insert into children tree.
                childrenTree.Replace(service,
                    new ObjectInfo((uint)newName.GetHashCode(), (ulong)childTagData.LongLength, info.Address));
            }

            
        }

        #endregion
    }
}
