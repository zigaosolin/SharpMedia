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
using System.Diagnostics;
using SharpMedia.Database.Physical.Caching;
using SharpMedia.Database.Physical.StorageStructs;

namespace SharpMedia.Database.Physical.Operations
{

    /// <summary>
    /// Changes the default typed stream.
    /// </summary>
    internal class ChangeDefaultTypedStream : IOperation
    {
        #region Private Members
        ulong rootAddress;
        string newDefault = string.Empty;
        uint newDefaultIndex = uint.MaxValue;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeDefaultTypedStream"/> class.
        /// </summary>
        /// <param name="rootAddress">The root address.</param>
        /// <param name="newDefaultIndex">New index of the default.</param>
        public ChangeDefaultTypedStream(ulong rootAddress, uint newDefaultIndex)
        {
            this.rootAddress = rootAddress;
            this.newDefaultIndex = newDefaultIndex;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeDefaultTypedStream"/> class.
        /// </summary>
        /// <param name="rootAddress">The root address.</param>
        /// <param name="newDefault">The new default.</param>
        public ChangeDefaultTypedStream(ulong rootAddress, string newDefault)
        {
            this.rootAddress = rootAddress;
            this.newDefault = newDefault;
        }

        #endregion

        #region IOperation Members

        public void Prepare(IReadService readService, out OperationStartupData data)
        {
            // We must extract new default.
            if (newDefaultIndex == uint.MaxValue)
            {
                Block block = readService.Read(BlockType.NodeHeaderBlock, rootAddress);
                Block unused;
                newDefaultIndex = NodeVersionHelper.GetTypedStreamIndex(newDefault, block, readService, out unused);
            }

            data = new OperationStartupData(0, 0);
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public unsafe void Execute(uint stage, IService service)
        {
            Debug.Assert(stage == 0);
            
            // We just change the node header.
            Block block = service.Read(BlockType.NodeHeaderBlock, rootAddress);
            fixed (byte* p = block.Data)
            {
                NodeVersionHeader* header = (NodeVersionHeader*)p;
                header->DefaultTypedStream = newDefaultIndex;
            }

            // We write it.
            service.Write(BlockType.NodeHeaderBlock, rootAddress, block);

        }

        #endregion
    }
}
