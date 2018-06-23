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
using System.Threading;
using SharpMedia.Caching;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.States
{
    /// <summary>
    /// Blend source for operation.
    /// </summary>
    public enum BlendOperand
    {
        /// <summary>
        /// A zero (0,0,0) source.
        /// </summary>
        Zero,

        /// <summary>
        /// A one (1,1,1) source.
        /// </summary>
        One,

        /// <summary>
        /// Source is the colour.
        /// </summary>
        SrcColour,

        /// <summary>
        /// Source is the colour inversed.
        /// </summary>
        SrcColourInverse,

        /// <summary>
        /// Source is the alpha.
        /// </summary>
        SrcAlpha,

        /// <summary>
        /// Source is the alpha inversed.
        /// </summary>
        SrcAlphaInverse,

        /// <summary>
        /// Source is destination alpha.
        /// </summary>
        DstAlpha,

        /// <summary>
        /// Source is the destination alpha inverse.
        /// </summary>
        DstAlphaInverse,

        /// <summary>
        /// Source is destination colour.
        /// </summary>
        DstColour,

        /// <summary>
        /// Source is destination colour inversed.
        /// </summary>
        DstColourInverse,

        /// <summary>
        /// Source is blend factor.
        /// </summary>
        BlendFactor,

        /// <summary>
        /// Source is blend factor inversed.
        /// </summary>
        BlendFactorInverse
    }

    /// <summary>
    /// The bledning operation.
    /// </summary>
    public enum BlendOperation
    {
        /// <summary>
        /// Adds together the sources.
        /// </summary>
        Add,

        /// <summary>
        /// Substract the sources.
        /// </summary>
        Subtract,

        /// <summary>
        /// Reverse substracts the sources.
        /// </summary>
        ReverseSubtract,

        /// <summary>
        /// Minimum of the two sources.
        /// </summary>
        Min,

        /// <summary>
        /// Maximum of the two sources.
        /// </summary>
        Max
    }

    /// <summary>
    /// The buffer write mask.
    /// </summary>
    [Flags]
    public enum WriteMask
    {
        /// <summary>
        /// No write mask.
        /// </summary>
        None = 0,

        /// <summary>
        /// Red colour can be written.
        /// </summary>
        Red = 1,

        /// <summary>
        /// Green colour can be written.
        /// </summary>
        Green = 2,

        /// <summary>
        /// Blue colour can be written.
        /// </summary>
        Blue = 4,

        /// <summary>
        /// Alpha channel can be written.
        /// </summary>
        Alpha = 8,

        /// <summary>
        /// All can be written (most common option).
        /// </summary>
        All = Red|Green|Blue|Alpha
    }

    /// <summary>
    /// A blend state; has all control for blending on targets.
    /// </summary>
    [Serializable]
    public class BlendState : IState, IEquatable<BlendState>, ICloneable<BlendState>
    {

        #region Private Members
        // Device data is valid until a modification is made to blending state.
        // Try to reuse the same state for as many drawing as possible.
        [NonSerialized]
        private Driver.IBlendState deviceData;
        [NonSerialized]
        private bool isInterned = false;
        [NonSerialized]
        internal object SyncRoot = new object();

        bool alphaToCoverage = false;
        bool[] blendEnable = new bool[GraphicsDevice.MaxRenderTargets];
        WriteMask[] writeMasks = new WriteMask[GraphicsDevice.MaxRenderTargets];
        BlendOperand srcBlend = BlendOperand.One;
        BlendOperand dstBlend = BlendOperand.Zero;
        BlendOperation opBlend = BlendOperation.Add;
        BlendOperand srcAlphaBlend = BlendOperand.One;
        BlendOperand dstAlphaBlend = BlendOperand.Zero;
        BlendOperation opAlphaBlend = BlendOperation.Add;
        uint sampleMask = 1;


        private void Changed()
        {
            if (isInterned)
            {
                throw new InvalidOperationException("Cannot mutate state, it is interned.");
            }
        }

        internal void Intern()
        {
            isInterned = true;         
        }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            // We conststruct good hash (use all bits).
            int hashCode = 0;
            if (alphaToCoverage) hashCode |= 1;

            // We take care of blend enables.
            for (int i = 0; i < GraphicsDevice.MaxRenderTargets; i++)
            {
                if (blendEnable[i])
                {
                    hashCode |= 1 << (i+1);

                    // Write mask only if enabled.
                    hashCode |= (int)(writeMasks[i]) << (10 + 2*i);
                }
            }

            // We add data.
            hashCode += (int)srcBlend;
            hashCode += (int)dstBlend << 1;
            hashCode += (int)opBlend << 2;
            hashCode += (int)srcAlphaBlend << 3;
            hashCode += (int)dstAlphaBlend << 4;
            hashCode += (int)opAlphaBlend << 5;
            hashCode ^= ((int)sampleMask<<6);

            return hashCode;
        }

        /// <summary>
        /// Sets the write mask.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="mask">The mask.</param>
        public void SetWriteMask(uint index, WriteMask mask)
        {

            Changed();
            writeMasks[index] = mask;
            
        }

        /// <summary>
        /// Gets the write mask.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Write mask.</returns>
        public WriteMask GetWriteMask(uint index)
        {
            return writeMasks[index];
        }

        /// <summary>
        /// Makes a copy of the state.
        /// </summary>
        /// <param name="other">The other state.</param>
        public void Copy([NotNull] BlendState other)
        {

            // We can copy device data because it is immutable.
            Changed();
            alphaToCoverage = other.alphaToCoverage;
            for (uint i = 0; i < GraphicsDevice.MaxRenderTargets; i++)
            {
                writeMasks[i] = other.writeMasks[i];
                blendEnable[i] = other.blendEnable[i];
            }
            srcBlend = other.srcBlend;
            dstBlend = other.dstBlend;
            opBlend = other.opBlend;
            srcAlphaBlend = other.srcAlphaBlend;
            dstAlphaBlend = other.dstAlphaBlend;
            opAlphaBlend = other.opAlphaBlend;
            sampleMask = other.sampleMask;
            

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the device data.
        /// </summary>
        /// <value>The device data.</value>
        internal Driver.IBlendState DeviceData
        {
            get { return deviceData; }
            set { deviceData = value; }
        }

        /// <summary>
        /// Gets or sets the Sample mask.
        /// </summary>
        /// <value>The Sample mask.</value>
        public uint SampleMask
        {
            get
            {
                return sampleMask;
            }
            set
            {
                Changed();
                sampleMask = value;
            
            }
        }

        /// <summary>
        /// Gets or sets the alpha blend operation.
        /// </summary>
        /// <value>The alpha blend operation.</value>
        public BlendOperation AlphaBlendOperation
        {
            get 
            {
                return opAlphaBlend;
            }
            set 
            {
                Changed();
                opAlphaBlend = value;
            }
        }

        /// <summary>
        /// Gets or sets the alpha blend destination.
        /// </summary>
        /// <value>The alpha blend destination.</value>
        public BlendOperand AlphaBlendDestination
        {
            get
            {
                return dstAlphaBlend;
            }
            set
            {
                Changed();
                dstAlphaBlend = value;  
            }
        }

        /// <summary>
        /// Gets or sets the alpha blend source.
        /// </summary>
        /// <value>The alpha blend source.</value>
        public BlendOperand AlphaBlendSource
        {
            get
            {
                return srcAlphaBlend;
            }
            set
            {
                Changed();
                srcAlphaBlend = value;   
            }
        }

        /// <summary>
        /// Gets or sets the blend operation.
        /// </summary>
        /// <value>The blend operation.</value>
        public BlendOperation BlendOperation
        {
            get
            {
                return opBlend;
            }
            set
            {
                Changed();
                opBlend = value;
            }
        }

        /// <summary>
        /// Gets or sets the destination blend.
        /// </summary>
        /// <value>The destination blend.</value>
        public BlendOperand BlendDestination
        {
            get
            {
                return dstBlend;
            }
            set
            {
                Changed();
                dstBlend = value;
            }
        }

        /// <summary>
        /// Gets or sets the source blend.
        /// </summary>
        /// <value>The source blend.</value>
        public BlendOperand BlendSource
        {
            get
            {
                return srcBlend;
            }
            set
            {
                Changed();
                srcBlend = value;
            }
        }

        /// <summary>
        /// Alpha to coverage blend state.
        /// </summary>
        public bool AlphaToCoverage
        {
            get 
            { 
                return alphaToCoverage;
            }
            set 
            {
                Changed();
                alphaToCoverage = value;
            }
        }



        /// <summary>
        /// Gets or sets the <see cref="System.Boolean"/> at the specified index.
        /// </summary>
        /// <value>Blending enabled for index.</value>
        public bool this[uint index]
        {
            get
            {
                return blendEnable[index];
            }
            set
            {
                Changed();
                blendEnable[index] = value;
            }
        }
       
        #endregion

        #region Constructors

        /// <summary>
        /// Default contructor.
        /// </summary>
        public BlendState()
        {
            for (uint i = 0; i < GraphicsDevice.MaxRenderTargets; i++)
            {
                blendEnable[i] = false;
                writeMasks[i] = WriteMask.All;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendState"/> class.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="dst">The DST.</param>
        /// <param name="op">The operation.</param>
        public BlendState(BlendOperand src, BlendOperand dst, BlendOperation op)
            : this()
        {
            blendEnable[0] = true;

            srcBlend = src;
            dstBlend = dst;
            opBlend = op;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendState"/> class.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="dst">The DST.</param>
        /// <param name="op">The op.</param>
        /// <param name="srcAlpha">The SRC alpha.</param>
        /// <param name="dstAlpha">The DST alpha.</param>
        /// <param name="opAlpha">The op alpha.</param>
        public BlendState(BlendOperand src, BlendOperand dst, BlendOperation op,
                          BlendOperand srcAlpha, BlendOperand dstAlpha, BlendOperation opAlpha)
            : this(src, dst, op)
        {

            srcAlphaBlend = srcAlpha;
            dstAlphaBlend = dstAlpha;
            opAlphaBlend = opAlpha;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendState"/> class.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="dst">The DST.</param>
        /// <param name="op">The op.</param>
        /// <param name="srcAlpha">The SRC alpha.</param>
        /// <param name="dstAlpha">The DST alpha.</param>
        /// <param name="opAlpha">The op alpha.</param>
        /// <param name="alphaToCov">if set to <c>true</c> alpha to coverage is used.</param>
        /// <param name="write">The write.</param>
        /// <param name="Sample">The Sample.</param>
        public BlendState(BlendOperand src, BlendOperand dst, BlendOperation op,
                          BlendOperand srcAlpha, BlendOperand dstAlpha, BlendOperation opAlpha,
                          bool alphaToCov, WriteMask write, byte sample)
            : this(src, dst, op, srcAlpha, dstAlpha, opAlpha)
        {
            alphaToCoverage = alphaToCov;
            writeMasks[0] = write;
            sampleMask = sample; 
        }

        #endregion

        #region IState Members

        public bool IsInterned
        {
            get { return isInterned; }
        }

        public void Prepare([NotNull] GraphicsDevice device)
        {
            if (!IsInterned)
            {
                throw new InvalidOperationException("Cannot prepare un-interned state.");
            }
            if (deviceData == null)
            {
                deviceData = device.DriverDevice.CreateState(this);
            }
            
        }

        #endregion

        #region IEquatable<BlendState> Members

        public bool Equals([NotNull] BlendState other)
        {
            if (this.IsInterned && other.IsInterned)
            {
                return object.ReferenceEquals(this, other);
            }

            // We do full match.
            if (this.alphaToCoverage != other.alphaToCoverage) return false;
            for (int i = 0; i < GraphicsDevice.MaxRenderTargets; i++)
            {
                if (this.blendEnable[i] != other.blendEnable[i]) return false;

                if (this.blendEnable[i])
                {
                    if (this.writeMasks[i] != other.writeMasks[i]) return false;
                }
            }
            if (srcBlend != other.srcBlend) return false;
            if (dstBlend != other.dstBlend) return false;
            if(opBlend != other.opBlend) return false;
            if(srcAlphaBlend != other.srcAlphaBlend) return false;
            if(dstAlphaBlend != other.dstAlphaBlend) return false;
            if(opAlphaBlend != other.opAlphaBlend) return false;
            if(sampleMask != other.sampleMask) return false;

            return true;
        }

        #endregion

        #region ICloneable<BlendState> Members

        public BlendState Clone()
        {
            BlendState state = new BlendState();
            state.Copy(this);
            return state;
        }

        #endregion
    }
}
