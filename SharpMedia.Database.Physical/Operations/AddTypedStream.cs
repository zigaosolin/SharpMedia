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
using System.Diagnostics;

namespace SharpMedia.Database.Physical.Operations
{

    /// <summary>
    /// Adds typed stream.
    /// </summary>
    internal class AddTypedStream : IOperation
    {
        #region Private Members
        ulong versionNode;
        string type;
        StreamOptions options;
        #endregion

        #region Constructor

        public AddTypedStream(ulong versionNode, string type, StreamOptions ops)
        {
            if (type.Length > TypedStreamHeader.MaxNameLength)
            {
                throw new InvalidOperationException("Too long type name");
            }
            this.versionNode = versionNode;
            this.type = type;
            this.options = ops;
        }

        #endregion

        #region IOperation Members

        public unsafe void Prepare(IReadService readService,
            out OperationStartupData data)
        {
            // We must make sure we can terminate if too many streams.
            Block block = readService.Read(BlockType.NodeHeaderBlock, versionNode);
            fixed (byte* p = block.Data)
            {
                NodeVersionHeader* header = (NodeVersionHeader*)p;
                if (header->StreamCount + 1 > BlockHelper.MaxTypedStreamsInNode(readService.BlockSize))
                {
                    throw new InvalidOperationException("Number of typed stream in node is more that "
                        + BlockHelper.MaxTypedStreamsInNode(readService.BlockSize) + " which is the limit"
                        + " at block size " + readService.BlockSize.ToString());
                }
            }

            // We need one allocation if typed stream is single object, otherwise two.
            if ((options & StreamOptions.SingleObject) != 0)
            {
                data = new OperationStartupData(versionNode, 1);
            }
            else
            {
                data = new OperationStartupData(versionNode, 2);
            }
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public unsafe void Execute(uint stage, IService service)
        {
            Debug.Assert(stage == 0);

            // 1) We create B+ tree (if not single object).
            ulong objectAddress = 0;
            if ((options & StreamOptions.SingleObject) == 0)
            {
                objectAddress = service.AllocationContext.CreateEmptyBTree();
            }


            // 2) We create TS header and write it.
            Block block = new Block(service.BlockSize);

            fixed (byte* p = block.Data)
            {
                TypedStreamHeader* header = (TypedStreamHeader*)p;
                header->ObjectsAddress = objectAddress;
                header->Options = options;
                header->ObjectSize = 0;

                int i;
                for (i = 0; i < type.Length; i++)
                {
                    header->Type[i] = type[i];
                }

                // Null terminate it.
                header->Type[i] = '\0';
            }
            ulong tsBlock = service.AllocationContext.AllocateBlock();
            service.Write(BlockType.TypedStreamHeader, tsBlock, block);

            // 3) We link it in version node.
            block = service.Read(BlockType.NodeHeaderBlock, versionNode);
            NodeVersionHelper.AddTypedStream(tsBlock, type, block);

            service.Write(BlockType.NodeHeaderBlock, versionNode, block);
        }

        #endregion
    }
}
