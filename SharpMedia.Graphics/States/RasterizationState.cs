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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.States
{
    /// <summary>
    /// The fill mode.
    /// </summary>
    public enum FillMode
    {
        /// <summary>
        /// Wireframe fill mode.
        /// </summary>
        Wireframe,

        /// <summary>
        /// Solid fill mode.
        /// </summary>
        Solid
    }

    /// <summary>
    /// Culling mode for facing objects.
    /// </summary>
    public enum CullMode
    {
        /// <summary>
        /// No culling.
        /// </summary>
        None,

        /// <summary>
        /// Front face culling.
        /// </summary>
        Front,

        /// <summary>
        /// Back face culling.
        /// </summary>
        Back
    }

    /// <summary>
    /// Facing of objects.
    /// </summary>
    public enum Facing
    {
        /// <summary>
        /// Counter clock wise.
        /// </summary>
        CCW,

        /// <summary>
        /// Clock wise.
        /// </summary>
        CW
    }

    /// <summary>
    /// The rasterisation state.
    /// </summary>
    [Serializable]
    public class RasterizationState : IState, IEquatable<RasterizationState>, ICloneable<RasterizationState>
    {
        #region Private Members
        [NonSerialized]
        Driver.IRasterizationState deviceData;
        [NonSerialized]
        bool isInterned = false;
        [NonSerialized]
        internal object SyncRoot = new object();

        FillMode fillMode = FillMode.Solid;
        CullMode cullMode = CullMode.Back;
        Facing frontFacing = Facing.CCW;
        int depthBias = 0;
        float depthBiasClamp = 0.0f;
        float scopeScaledDepthBias = 0.0f;
        bool depthClip = false;
        bool scissorTest = false;
        bool multiSample = false;
        bool lineAntialising = false;

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

        #region Properties

        /// <summary>
        /// Gets the device data.
        /// </summary>
        /// <value>The device data.</value>
        internal Driver.IRasterizationState DeviceData
        {
            get
            {
                return deviceData;
            }
            set
            {
                deviceData = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether line antialising is enabled. If multisampling
        /// is enabled, this property cannot be enabled (this is only for non-samplings).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if line antialising is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool LineAntialisingEnabled
        {
            get
            {
                return lineAntialising;
            }
            set
            {
                Changed();
                if (multiSample && value)
                {
                    throw new InvalidOperationException("Cannot set line intaliasing if samplig enabled.");
                }
                lineAntialising = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether multi sampleing is enabled. Setting this
        /// value to true will force line antialising to false.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if multi sampleing is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool MultiSamplingEnabled
        {
            get
            {
                return multiSample;
            }
            set
            {
                Changed();
                multiSample = value;
                if (multiSample && lineAntialising)
                {
                    lineAntialising = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether scissor test is enabled. If scissor test is
        /// enabled, you should also set scissor rectange <see cref="Device.SetScissorRect"/>.
        /// </summary>
        /// <value><c>true</c> if scissor test is enabled; otherwise, <c>false</c>.</value>
        public bool ScissorTestEnabled
        {
            get
            {
                return scissorTest;
            }
            set
            {
                Changed();
                scissorTest = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether depth clip is enabled.
        /// </summary>
        /// <value><c>true</c> if depth clip is enabled; otherwise, <c>false</c>.</value>
        public bool DepthClipEnabled
        {
            get
            {
                return depthClip;
            }
            set
            {
                Changed();
                depthClip = value;
            }
        }

        /// <summary>
        /// Gets or sets the scope scaled depth bias.
        /// </summary>
        /// <value>The scope scaled depth bias.</value>
        public float ScopeScaledDepthBias
        {
            get
            {
                return scopeScaledDepthBias;
            }
            set
            {
                Changed();
                scopeScaledDepthBias = value;
            }
        }

        /// <summary>
        /// Gets or sets the depth bias clamp.
        /// </summary>
        /// <value>The depth bias clamp.</value>
        public float DepthBiasClamp
        {
            get
            {
                return depthBiasClamp;
            }
            set
            {
                Changed();
                depthBiasClamp = value;
            }
        }

        /// <summary>
        /// Gets or sets the depth bias.
        /// </summary>
        /// <value>The depth bias.</value>
        public int DepthBias
        {
            get
            {
                return depthBias;
            }
            set
            {
                Changed();
                depthBias = value;
            }
        }

        /// <summary>
        /// Gets or sets the front facing.
        /// </summary>
        /// <value>The front facing.</value>
        public Facing FrontFacing
        {
            get
            {
                return frontFacing;
            }
            set
            {
                Changed();
                frontFacing = value;
            }
        }

        /// <summary>
        /// Gets or sets the back facing.
        /// </summary>
        /// <value>The back facing.</value>
        public Facing BackFacing
        {
            get
            {
                return frontFacing == Facing.CCW ? Facing.CW : Facing.CCW;
            }
            set
            {
                Changed();
                frontFacing = value == Facing.CCW ? Facing.CW : Facing.CCW;
            }
        }

        /// <summary>
        /// Gets or sets the fill mode.
        /// </summary>
        /// <value>The fill mode.</value>
        public FillMode FillMode
        {
            get
            {
                return fillMode;
            }
            set
            {
                Changed();
                fillMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the cull mode.
        /// </summary>
        /// <value>The cull mode.</value>
        public CullMode CullMode
        {
            get
            {
                return cullMode;
            }
            set
            {
                Changed();
                cullMode = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copies contents of other into this.
        /// </summary>
        /// <param name="other">The other.</param>
        public void Copy([NotNull] RasterizationState other)
        {
            Changed();
            fillMode = other.fillMode;
            cullMode = other.cullMode;
            frontFacing = other.frontFacing;
            depthBias = other.depthBias;
            depthBiasClamp = other.depthBiasClamp;
            scopeScaledDepthBias = other.scopeScaledDepthBias;
            depthClip = other.depthClip;
            scissorTest = other.scissorTest;
            multiSample = other.multiSample;
            lineAntialising = other.lineAntialising;
        }

        public override int GetHashCode()
        {
            // TODO: write a real hash function.
            return 0;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizationState"/> class.
        /// </summary>
        public RasterizationState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizationState"/> class.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="facing">The facing.</param>
        public RasterizationState(CullMode mode, Facing facing)
        {
            cullMode = mode;
            frontFacing = facing;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizationState"/> class.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="facing">The facing.</param>
        /// <param name="Sample">Ff set to <c>true</c> multisampling is enabled.</param>
        public RasterizationState(CullMode mode, Facing facing, bool sample)
            : this(mode, facing)
        {
            multiSample = sample;
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
                throw new InvalidOperationException("Cannot prepare not interned rasterization state.");
            }
            if (deviceData == null)
            {
                deviceData = device.DriverDevice.CreateState(this);
            }
        }

        #endregion

        #region IEquatable<RasterizationState> Members

        public bool Equals(RasterizationState other)
        {
            if (this.IsInterned && other.IsInterned)
            {
                return object.ReferenceEquals(this, other);
            }


            if (fillMode != other.fillMode) return false;
            if (cullMode != other.cullMode) return false;
            if (frontFacing != other.frontFacing) return false;
            if (depthBias != other.depthBias) return false;
            if (depthBiasClamp != other.depthBiasClamp) return false;
            if (scopeScaledDepthBias != other.scopeScaledDepthBias) return false;
            if (depthClip != other.depthClip) return false;
            if (scissorTest != other.scissorTest) return false;
            if (multiSample != other.multiSample) return false;
            if (lineAntialising != other.lineAntialising) return false;

            return true;
        }

        #endregion

        #region ICloneable<RasterizationState> Members

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public RasterizationState Clone()
        {
            RasterizationState state = new RasterizationState();
            state.Copy(this);
            return state;
        }

        #endregion
    }
}
