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

namespace SharpMedia.Database.Physical.Journalling
{

    /// <summary>
    /// The operation startup data. It is immutable.
    /// </summary>
    public class OperationStartupData
    {
        #region Private Members
        ulong hintBlock = 0;
        uint allocationsRequired = 0;
        bool needsDynamicAllocation = false;
        #endregion

        #region Static Members

        /// <summary>
        /// Merges all operation startup data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static OperationStartupData Merge(params OperationStartupData[] data)
        {
            ulong hint = data.Length > 0 ? data[0].HintBlock : 0;
            uint allocationsRequired = 0;
            bool needDynamic = false;

            foreach (OperationStartupData d in data)
            {
                allocationsRequired += d.BlockAllocationsRequired;
                if (d.NeedsDynamicAllocation) needDynamic = true;
            }

            return new OperationStartupData(hint, allocationsRequired, needDynamic);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The hint block, 0 means no hint.
        /// </summary>
        public ulong HintBlock
        {
            get
            {
                return hintBlock;
            }
        }

        /// <summary>
        /// Number (overestimate) of blocks required.
        /// </summary>
        public uint BlockAllocationsRequired
        {
            get
            {
                return allocationsRequired;
            }
        }

        /// <summary>
        /// Does operation need dynamic allocation.
        /// </summary>
        public bool NeedsDynamicAllocation
        {
            get
            {
                return needsDynamicAllocation;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs operation startup.
        /// </summary>
        /// <param name="hintBlock">The block hint, where to allocate.</param>
        /// <param name="allocationsRequired">How many blocks are required (can be overestimate).</param>
        public OperationStartupData(ulong hintBlock, uint allocationsRequired)
        {
            this.hintBlock = hintBlock;
            this.allocationsRequired = allocationsRequired;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationStartupData"/> class.
        /// </summary>
        /// <param name="hintBlock">The hint block.</param>
        /// <param name="allocationRequest">The allocation request.</param>
        /// <param name="needsDynamicAllocation">if set to <c>true</c> [needs dynamic allocation].</param>
        public OperationStartupData(ulong hintBlock, uint allocationRequest, bool needsDynamicAllocation)
            : this(hintBlock, allocationRequest)
        {
            this.needsDynamicAllocation = needsDynamicAllocation;
        }

        #endregion
    }
}
