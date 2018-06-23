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
using SharpMedia.Math;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.States
{
    /// <summary>
    /// The filter used for sampler.
    /// </summary>
    public enum Filter
    {
        /// <summary>
        /// Point filter, taking nearest point into account.
        /// </summary>
        Point,

        /// <summary>
        /// Linear filter, using linear interpolation between 2 (1D texture), 4 (2D texture) or 8 (3D texture)
        /// values.
        /// </summary>
        Linear,

        /// <summary>
        /// Anisotropic filter.
        /// </summary>
        Anisotropic
    }

    /// <summary>
    /// Addressing modes when taking samples out of range [0,1].
    /// </summary>
    public enum AddressMode
    {
        /// <summary>
        /// Texture coordinates are wrapped, 1.3 becomes 0.3, 1.7 becomes 0.7.
        /// </summary>
        Wrap,

        /// <summary>
        /// Texture is repeated but every time by mirror image: 1.3 is mirrored to 0.7.
        /// </summary>
        Mirror,

        /// <summary>
        /// The values are clamped, 1.3 becomes 1.0.
        /// </summary>
        Clamp,

        /// <summary>
        /// Border colour is used out of range.
        /// </summary>
        Border,

        /// <summary>
        /// Only mirrors once.
        /// </summary>
        MirrorOnce
    }

    

    /// <summary>
    /// The sampler state, can be applied for each sampler.
    /// </summary>
    [Serializable]
    public class SamplerState : IState, IEquatable<SamplerState>, ICloneable<SamplerState>
    {
        #region Private Members
        [NonSerialized]
        Driver.ISamplerState deviceData;
        [NonSerialized]
        bool isInterned = false;
        [NonSerialized]
        internal object SyncRoot = new object();

        Colour borderColour = Colour.Black;
        Filter filter = Filter.Linear;
        Filter mipmapFilter = Filter.Linear;
        AddressMode addressU = AddressMode.Clamp;
        AddressMode addressV = AddressMode.Clamp;
        AddressMode addressW = AddressMode.Clamp;
        float mipLODBias = 0.0f;
        uint maxAnisotropy = 1;
        uint minMipmap = 0;
        uint maxMipmap = uint.MaxValue;

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
        internal Driver.ISamplerState DeviceData
        {
            get { return deviceData; }
            set { deviceData = value; }
        }

        /// <summary>
        /// Gets or sets the border colour.
        /// </summary>
        /// <value>The border colour.</value>
        public Colour BorderColour
        {
            get
            {
                return borderColour;
            }
            set
            {
                Changed();
                borderColour = value;
            }
        }

        /// <summary>
        /// Gets or sets the max mipmap.
        /// </summary>
        /// <value>The max mipmap.</value>
        public uint MaxMipmap
        {
            get
            {
                return maxMipmap;
            }
            set
            {
                Changed();
                maxMipmap = value;
            }
        }

        /// <summary>
        /// Gets or sets the min mipmap.
        /// </summary>
        /// <value>The min mipmap.</value>
        public uint MinMipmap
        {
            get
            {
                return minMipmap;
            }
            set
            {
                Changed();
                maxMipmap = value;
            }
        }

        /// <summary>
        /// Gets or sets the max anisotropy.
        /// </summary>
        /// <value>The max anisotropy.</value>
        public uint MaxAnisotropy
        {
            get
            {
                return maxAnisotropy;
            }
            set
            {
                Changed();
                maxAnisotropy = value;
            }
        }

        /// <summary>
        /// Gets or sets the mipmap LOD bias.
        /// </summary>
        /// <value>The mipmap LOD bias.</value>
        public float MipmapLODBias
        {
            get
            {
                return mipLODBias;
            }
            set
            {
                Changed();
                mipLODBias = value;
            }
        }

        /// <summary>
        /// Gets or sets the address W.
        /// </summary>
        /// <value>The address W.</value>
        public AddressMode AddressW
        {
            get
            {
                return addressW;
            }
            set
            {
                Changed();
                addressW = value;
            }
        }

        /// <summary>
        /// Gets or sets the address V.
        /// </summary>
        /// <value>The address V.</value>
        public AddressMode AddressV
        {
            get
            {
                return addressV;
            }
            set
            {
                Changed();
                addressV = value;
            }
        }

        /// <summary>
        /// Gets or sets the address U.
        /// </summary>
        /// <value>The address U.</value>
        public AddressMode AddressU
        {
            get
            {
                return addressU;
            }
            set
            {
                Changed();
                addressU = value;
            }
        }

        /// <summary>
        /// Gets or sets the mipmap filter.
        /// </summary>
        /// <value>The mipmap filter.</value>
        public Filter MipmapFilter
        {
            get
            {
                return mipmapFilter;
            }
            set
            {
                Changed();
                mipmapFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public Filter Filter
        {
            get
            {
                return filter;
            }
            set
            {
                Changed();
                filter = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerState"/> class.
        /// </summary>
        public SamplerState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerState"/> class.
        /// </summary>
        /// <param name="texFilter">The texture filter.</param>
        /// <param name="mipFilter">The mipmap filter.</param>
        public SamplerState(Filter texFilter, Filter mipFilter)
        {
            filter = texFilter;
            mipmapFilter = mipFilter;
        }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            // TODO: write a real hash function.
            return 0;
        }

        /// <summary>
        /// Copies sampler state from other.
        /// </summary>
        /// <param name="other">The other sampler state.</param>
        public void Copy([NotNull] SamplerState other)
        {

            Changed();
            filter = other.filter;
            mipmapFilter = other.mipmapFilter;
            addressU = other.addressU;
            addressV = other.addressV;
            addressW = other.addressW;
            mipLODBias = other.mipLODBias;
            maxAnisotropy = other.maxAnisotropy;
            minMipmap = other.minMipmap;
            maxMipmap = other.maxMipmap;


        }


        #endregion

        #region IState Members

        public void Prepare(GraphicsDevice device)
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

        public bool IsInterned
        {
            get { return isInterned; }
        }

        #endregion

        #region IEquatable<SamplerState> Members

        public bool Equals([NotNull] SamplerState other)
        {
            if (IsInterned && other.IsInterned)
            {
                return object.ReferenceEquals(this, other);
            }

            if (filter != other.filter) return false;
            if (mipmapFilter != other.mipmapFilter) return false;
            if (addressU != other.addressU) return false;
            if (addressV != other.addressV) return false;
            if (addressW != other.addressW) return false;
            if (mipLODBias != other.mipLODBias) return false;
            if (maxAnisotropy != other.maxAnisotropy) return false;
            if (minMipmap != other.minMipmap) return false;
            if (maxMipmap != other.maxMipmap) return false;
            return true;
        }

        #endregion

        #region ICloneable<SamplerState> Members

        public SamplerState Clone()
        {
            SamplerState state = new SamplerState();
            state.Copy(this);
            return state;
        }

        #endregion
    }
}
