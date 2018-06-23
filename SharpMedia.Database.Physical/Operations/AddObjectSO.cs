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
using SharpMedia.Database.Physical.Caching;
using SharpMedia.Database.Physical.Journalling;
using System.Diagnostics;
using SharpMedia.Database.Physical.StorageStructs;

namespace SharpMedia.Database.Physical.Operations
{
    /// <summary>
    /// Adds object to single-object typed stream. Replaces current if it exists.
    /// </summary>
    internal class AddObjectSO : IOperation
    {
        #region Private Members
        ulong typedStreamHeader;
        byte[] objectData;
        #endregion

        #region Constructors

        public AddObjectSO(ulong typedStreamHeader, byte[] data)
        {
            this.typedStreamHeader = typedStreamHeader;
            this.objectData = data;
        }

        #endregion

        #region IOperation Members

        public void Prepare(SharpMedia.Database.Physical.Journalling.IReadService readService, 
            out OperationStartupData data)
        {
            // We need to allocate for our object only.
            data = new OperationStartupData(typedStreamHeader,
                BlockHelper.MaxBlocksForObject(readService.BlockSize, (ulong)objectData.LongLength));
            
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public unsafe void Execute(uint stage, SharpMedia.Database.Physical.Journalling.IService service)
        {
            Debug.Assert(stage == 0);

            // 1) We first write object to stream.
            BlockStream stream = service.AllocationContext.CreateBlockStream((ulong)objectData.LongLength);
            stream.Write(objectData);

            // 2) We may need to delete object from B+ stream.
            Block block = service.Read(BlockType.TypedStreamHeader, typedStreamHeader);
            fixed (byte* p = block.Data)
            {
                TypedStreamHeader* header = (TypedStreamHeader*)p;
                
                // We may need to delete link.
                if (header->ObjectsAddress != 0)
                {
                    BlockStream objToDelete = BlockStream.FromBase(header->ObjectsAddress, service);
                    objToDelete.Deallocate();
                }

                // 3) We must relink it to out block.
                header->ObjectsAddress = stream.BaseAddress;
                header->ObjectSize = (ulong)objectData.LongLength;
            }

            
            service.Write(BlockType.TypedStreamHeader, typedStreamHeader, block);
        }

        #endregion
    }
}
