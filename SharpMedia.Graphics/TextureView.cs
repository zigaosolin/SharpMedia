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
using SharpMedia.Resources;
using SharpMedia.Math;

namespace SharpMedia.Graphics
{
    /// <summary>
    /// A texture view of either buffer of typeless texture.
    /// </summary>
    public abstract class TextureView : Image, IResourceView, IGraphicsLocality
    {
        #region Protected Members
        [NonSerialized]
        protected Driver.ITextureView view = null;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected TextureView()
        {
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Called when used by device.
        /// </summary>
        /// <param name="device">The device.</param>
        internal abstract void UsedByDevice();

        /// <summary>
        /// Not used by device anymore.
        /// </summary>
        internal abstract void UnusedByDevice();

        /// <summary>
        /// Gets the device data.
        /// </summary>
        /// <value>The device data.</value>
        internal Driver.ITextureView DeviceData
        {
            get
            {
                return view;
            }
        }

        #endregion

        #region Properties

        public abstract PinFormat ViewType { get; }

        #endregion

        #region IResourceView Members

        public abstract object TypelessResource { get; }

        #endregion

        #region IGraphicsLocality Members

        public abstract GraphicsLocality Locality { get; set; }
        public abstract void BindToDevice(GraphicsDevice device);
        public abstract void UnBindFromDevice();
        public abstract bool IsBoundToDevice { get; }

        #endregion

        #region Access/Sample Methods

        /// <summary>
        /// Begins using the texture. Data is mapped to view.
        /// </summary>
        public abstract void BeginCPUAccess();

        /// <summary>
        /// Ends using the texture. Data is unmapped.
        /// </summary>
        public abstract void EndCPUAccess();

        /// <summary>
        /// Loads the specified x.
        /// </summary>
        /// <remarks>Loading x may fail in CPU (if no read access) or may be very slow.
        /// If GPU Assembling is used, this method is replaced with GPU eqiuvaled.</remarks>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public abstract Vector4f Load(Vector2i x);

        /// <summary>
        /// Loads the specified x.
        /// </summary>
        /// <remarks>Loading x may fail in CPU (if no read access) or may be very slow.
        /// If GPU Assembling is used, this method is replaced with GPU eqiuvaled.</remarks>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public abstract Vector4f Load(Vector3i x);

        /// <summary>
        /// Loads the specified x.
        /// </summary>
        /// <remarks>Loading x may fail in CPU (if no read access) or may be very slow.
        /// If GPU Assembling is used, this method is replaced with GPU eqiuvaled.</remarks>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public abstract Vector4f Load(Vector4i x);

        /// <summary>
        /// Samples the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="x">The x.</param>
        /// <remarks>Sampling at x may fail on CPU (if no read access) or may be very slow. If
        /// GPU assembling is used, this method is replaced with GPU eqiuvalent.
        /// </remarks>
        /// <returns></returns>
        public abstract Vector4f Sample(States.SamplerState state, Vector2f x);

        /// <summary>
        /// Samples the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="x">The x.</param>
        /// <remarks>Sampling at x may fail on CPU (if no read access) or may be very slow. If
        /// GPU assembling is used, this method is replaced with GPU eqiuvalent.
        /// </remarks>
        /// <returns></returns>
        public abstract Vector4f Sample(States.SamplerState state, Vector3f x);

        /// <summary>
        /// Samples the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="x">The x.</param>
        /// <remarks>Sampling at x may fail on CPU (if no read access) or may be very slow. If
        /// GPU assembling is used, this method is replaced with GPU eqiuvalent.
        /// </remarks>
        /// <returns></returns>
        public abstract Vector4f Sample(States.SamplerState state, Vector4f x);

        #endregion

        #region Static Constructions

        // Provide simple (hidden typeless) contructs.
        

        #endregion


    }

    
    
}
