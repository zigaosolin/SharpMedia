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
    /// Possible depth test comparison functions.
    /// </summary>
    public enum CompareFunction
    {
        /// <summary>
        /// Never succeeds.
        /// </summary>
        Never,

        /// <summary>
        /// Always succeeds.
        /// </summary>
        Always,

        /// <summary>
        /// Succeeds if equal.
        /// </summary>
        Equal,

        /// <summary>
        /// Succeeds if less, one of the most common.
        /// </summary>
        Less,

        /// <summary>
        /// Succeeds if less or equal, one of the most common.
        /// </summary>
        LessEqual,

        /// <summary>
        /// Succeeds if depth value is greater.
        /// </summary>
        Greater,

        /// <summary>
        /// Succeeds if value is greater or equal.
        /// </summary>
        GreaterEqual,

        /// <summary>
        /// Succeeds if values are not equal.
        /// </summary>
        NotEqual
    }

    /// <summary>
    /// Stencil operation.
    /// </summary>
    public enum StencilOperation
    {
        /// <summary>
        /// Keeps the stencil value.
        /// </summary>
        Keep,

        /// <summary>
        /// Zeros out stencil value.
        /// </summary>
        Zero,

        /// <summary>
        /// Replaces the value with stencil reference value.
        /// </summary>
        Replace,

        /// <summary>
        /// Increases by one with no limits (255+1 = 0)
        /// </summary>
        Increase,

        /// <summary>
        /// Decreases by one with no limits (0 - 1 = 255).
        /// </summary>
        Decrease,

        /// <summary>
        /// Increases by one with borders (255+1 = 255).
        /// </summary>
        IncreaseSAT,

        /// <summary>
        /// Decreases by one with borders (0-1 = 0).
        /// </summary>
        DecreaseSAT,

        /// <summary>
        /// Inverts the value (bitwise).
        /// </summary>
        Invert
    }

    /// <summary>
    /// The depth stencil state. 
    /// </summary>
    [Serializable]
    public class DepthStencilState : IState, IEquatable<DepthStencilState>, ICloneable<DepthStencilState>
    {
        #region Private Members
        [NonSerialized]
        Driver.IDepthStencilState deviceData;
        [NonSerialized]
        bool isInterned = false;
        [NonSerialized]
        internal object SyncRoot = new object();

        bool depthTest = true;
        bool depthWrite = true;
        CompareFunction depthCompare = CompareFunction.LessEqual;
        bool stencilTest = false;
        uint stencilReadMask = uint.MaxValue;
        uint stencilWriteMask = uint.MaxValue;

        StencilOperation frontStencilFail = StencilOperation.Keep;
        StencilOperation frontDepthFail = StencilOperation.Keep;
        StencilOperation frontDepthPass = StencilOperation.Keep;
        CompareFunction frontStencilCompare = CompareFunction.LessEqual;

        StencilOperation backStencilFail = StencilOperation.Keep;
        StencilOperation backDepthFail = StencilOperation.Keep;
        StencilOperation backDepthPass = StencilOperation.Keep;
        CompareFunction backStencilCompare = CompareFunction.LessEqual;

        private void Changed()
        {
            if (isInterned) throw new InvalidOperationException("Cannot mutate state, it is interned.");
        }

        internal void Intern()
        {
            isInterned = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the device data.
        /// </summary>
        /// <value>The device data.</value>
        internal Driver.IDepthStencilState DeviceData
        {
            get { return deviceData; }
            set { deviceData = value; }
        }

        /// <summary>
        /// Gets or sets the stencil compare.
        /// </summary>
        /// <value>The stencil compare.</value>
        public CompareFunction BackStencilCompare
        {
            get
            {
                return backStencilCompare;
            }
            set
            {
                Changed();
                backStencilCompare = value;
            }
        }

        /// <summary>
        /// Gets or sets the depth fail, for both sides. Getter returns result for
        /// front facing primitives.
        /// </summary>
        /// <value>The stencil fail.</value>
        public StencilOperation BackDepthPass
        {
            get
            {
                return backDepthPass;
            }
            set
            {
                Changed();
                backDepthPass = value;
            }
        }

        /// <summary>
        /// Gets or sets the depth fail, for both sides. Getter returns result for
        /// front facing primitives.
        /// </summary>
        /// <value>The stencil fail.</value>
        public StencilOperation BackDepthFail
        {
            get
            {
                return backDepthFail;
            }
            set
            {
                Changed();
                backDepthFail = value; 
            }
        }

        /// <summary>
        /// Gets or sets the stencil fail, for both sides. Getter returns result for
        /// front facing primitives.
        /// </summary>
        /// <value>The stencil fail.</value>
        public StencilOperation BackStencilFail
        {
            get
            {
                return backStencilFail;
            }
            set
            {
                Changed();
                backStencilFail = value;        
            }
        }


        /// <summary>
        /// Gets or sets the stencil compare.
        /// </summary>
        /// <value>The stencil compare.</value>
        public CompareFunction FrontStencilCompare
        {
            get
            {
                return frontStencilCompare;
            }
            set
            {
                Changed();
                frontStencilCompare = value;
            }
        }

        /// <summary>
        /// Gets or sets the depth fail, for both sides. Getter returns result for
        /// front facing primitives.
        /// </summary>
        /// <value>The stencil fail.</value>
        public StencilOperation FrontDepthPass
        {
            get
            {
                return frontDepthPass;
            }
            set
            {
                Changed();
                frontDepthPass = value;           
            }
        }

        /// <summary>
        /// Gets or sets the depth fail, for both sides. Getter returns result for
        /// front facing primitives.
        /// </summary>
        /// <value>The stencil fail.</value>
        public StencilOperation FrontDepthFail
        {
            get
            {
                return frontDepthFail;
            }
            set
            {
                Changed();
                frontDepthFail = value;        
            }
        }

        /// <summary>
        /// Gets or sets the stencil fail, for both sides. Getter returns result for
        /// front facing primitives.
        /// </summary>
        /// <value>The stencil fail.</value>
        public StencilOperation FrontStencilFail
        {
            get
            {
                return frontStencilFail;
            }
            set
            {
                Changed();
                frontStencilFail = value;         
            }
        }

        /// <summary>
        /// Gets or sets the stencil compare.
        /// </summary>
        /// <value>The stencil compare.</value>
        public CompareFunction StencilCompare
        {
            get
            {
                return frontStencilCompare;
            }
            set
            {
                Changed();
                frontStencilCompare = backStencilCompare = value;
            }
        }

        /// <summary>
        /// Gets or sets the depth fail, for both sides. Getter returns result for
        /// front facing primitives.
        /// </summary>
        /// <value>The stencil fail.</value>
        public StencilOperation DepthPass
        {
            get
            {
                return frontDepthPass;
            }
            set
            {
                Changed();
                frontDepthPass = backDepthPass = value;
                
            }
        }

        /// <summary>
        /// Gets or sets the depth fail, for both sides. Getter returns result for
        /// front facing primitives.
        /// </summary>
        /// <value>The stencil fail.</value>
        public StencilOperation DepthFail
        {
            get
            {
                return frontDepthFail;
            }
            set
            {
                Changed();
                frontDepthFail = backDepthFail = value;
            }
        }

        /// <summary>
        /// Gets or sets the stencil fail, for both sides. Getter returns result for
        /// front facing primitives.
        /// </summary>
        /// <value>The stencil fail.</value>
        public StencilOperation StencilFail
        {
            get
            {
                return frontStencilFail;
            }
            set
            {
                Changed();
                frontStencilFail = backStencilFail = value;
            }
        }

        /// <summary>
        /// Gets or sets the stencil write mask.
        /// </summary>
        /// <value>The stencil write mask.</value>
        public uint StencilWriteMask
        {
            get
            {
                return stencilWriteMask;
            }
            set
            {
                Changed();
                stencilWriteMask = value;
            }
        }

        /// <summary>
        /// Gets or sets the stencil read mask.
        /// </summary>
        /// <value>The stencil read mask.</value>
        public uint StencilReadMask
        {
            get
            {
                return stencilReadMask;
            }
            set
            {
                Changed();
                stencilReadMask = value;
            }
        }
        

        /// <summary>
        /// Gets or sets a value indicating whether stencil test is enabled.
        /// </summary>
        public bool StencilTestEnabled
        {
            get
            {
                return stencilTest;
            }
            set
            {
                Changed();
                stencilTest = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether depth write is enabled.
        /// </summary>
        /// <value><c>true</c> if depth write is enabled; otherwise, <c>false</c>.</value>
        public bool DepthWriteEnabled
        {
            get
            {
                return depthWrite;
            }
            set
            {
                Changed();
                depthWrite = value;
            }
        }

        /// <summary>
        /// Gets or sets the depth compare.
        /// </summary>
        /// <value>The depth compare.</value>
        public CompareFunction DepthCompare
        {
            get
            {
                return depthCompare;
            }
            set
            {
                Changed();
                depthCompare = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether depth test is enabled.
        /// </summary>
        /// <value><c>true</c> if depth test enables; otherwise, <c>false</c>.</value>
        public bool DepthTestEnabled
        {
            get
            {
                return depthTest;
            }
            set
            {
                Changed();
                depthTest = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilState"/> class.
        /// </summary>
        public DepthStencilState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilState"/> class.
        /// </summary>
        /// <param name="depthCompareFunc">The depth compare function.</param>
        public DepthStencilState(CompareFunction depthCompareFunc)
        {
            depthCompare = depthCompareFunc;
        }

        #endregion

        #region Methods

        /// <summary>
        /// A suitable hashing function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // TODO: write a real hash.
            return 0;
        }

        /// <summary>
        /// Copies state to this object.
        /// </summary>
        /// <param name="other"></param>
        public void Copy([NotNull] DepthStencilState other)
        {

            Changed();
            depthTest = other.depthTest;
            depthWrite = other.depthWrite;
            depthCompare = other.depthCompare;
            stencilTest = other.stencilTest;
            stencilReadMask = other.stencilReadMask;
            stencilWriteMask = other.stencilWriteMask;
            frontStencilFail = other.frontStencilFail;
            frontDepthFail = other.frontDepthFail;
            frontDepthPass = other.frontDepthPass;
            frontStencilCompare = other.frontStencilCompare;
            backStencilFail = other.backStencilFail;
            backDepthFail = other.backDepthFail;
            backDepthPass = other.backDepthPass;
            backStencilCompare = other.backStencilCompare;

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
                throw new InvalidOperationException("DepthStencil state must be interned before prepared.");
            }

            if (deviceData == null)
            {
                deviceData = device.DriverDevice.CreateState(this);
            }
        }

        #endregion

        #region IEquatable<DepthStencilState> Members

        public bool Equals([NotNull] DepthStencilState other)
        {
            if (this.IsInterned && other.IsInterned)
            {
                return object.ReferenceEquals(this, other);
            }

            if (depthTest != other.depthTest) return false;
            if (depthWrite != other.depthWrite) return false;
            if (depthCompare != other.depthCompare) return false;
            if (stencilTest != other.stencilTest) return false;
            if (stencilReadMask != other.stencilReadMask) return false;
            if (stencilWriteMask != other.stencilWriteMask) return false;
            if (frontStencilFail != other.frontStencilFail) return false;
            if (frontDepthFail != other.frontDepthFail) return false;
            if (frontDepthPass != other.frontDepthPass) return false;
            if (frontStencilCompare != other.frontStencilCompare) return false;
            if (backStencilFail != other.backStencilFail) return false;
            if (backDepthFail != other.backDepthFail) return false;
            if (backDepthPass != other.backDepthPass) return false;
            if (backStencilCompare != other.backStencilCompare) return false;
            return true;
        }

        #endregion

        #region ICloneable<DepthStencilState> Members

        public DepthStencilState Clone()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

}
