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

namespace SharpMedia.Graphics.Shaders.Assembler
{
    /// <summary>
    /// Constant attribute can be applied to GPUCompilant method's arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ConstantAttribute : Attribute
    {
        #region Private Members
        bool canOverlap = false;            //< This is a safety guard if two constants can
                                            //< overlap (share the same address).
        uint bufferSlot = uint.MaxValue;    //< Undefined buffer slot.
        uint bufferOffset = uint.MaxValue;  //< Undefined buffer offset.
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantAttribute"/> class.
        /// </summary>
        public ConstantAttribute()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can overlap with another constant.
        /// This flag is only valid if buffer slot and buffer offset are set.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can overlap; otherwise, <c>false</c>.
        /// </value>
        public bool CanOverlap
        {
            get
            {
                return canOverlap;
            }
            set
            {
                canOverlap = value;
            }
        }

        /// <summary>
        /// Gets or sets the buffer slot. This value can be used to group constants. The offset
        /// does not need to be provided (it is chosen automatically).
        /// </summary>
        /// <value>The buffer slot.</value>
        public uint BufferSlot
        {
            get
            {
                return bufferSlot;
            }
            set
            {
                bufferSlot = value;
            }
        }

        /// <summary>
        /// Gets or sets the buffer offset. This can only be set if buffer slot is set.
        /// </summary>
        /// <value>The buffer offset.</value>
        public uint BufferOffset
        {
            get
            {
                return bufferOffset;
            }
            set
            {
                bufferOffset = value;
            }
        }

        #endregion

    }
}
