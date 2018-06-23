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

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A depth stencil target view allows binding to depth stencil target.
    /// </summary>
    public abstract class DepthStencilTargetView : IResourceView, IGraphicsLocality
    {
        #region Private Members
        [NonSerialized]
        protected Driver.IDepthStencilTargetView view = null;
        #endregion

        #region Internal Methodws

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected DepthStencilTargetView()
        {
        }

        /// <summary>
        /// Gets the device data.
        /// </summary>
        /// <value>The device data.</value>
        internal Driver.IDepthStencilTargetView DeviceData
        {
            get
            {
                return view;
            }
        }

        /// <summary>
        /// Useds the by device.
        /// </summary>
        internal abstract void UsedByDevice();

        /// <summary>
        /// Unuseds the by device.
        /// </summary>
        internal abstract void UnusedByDevice();

        #endregion

        #region IDisposable Members

        public abstract void Dispose();

        #endregion

        #region IResourceView Members

        public abstract object TypelessResource { get; }

        #endregion

        #region IGraphicsLocality Members

        public abstract GraphicsLocality Locality
        {
            get;
            set;
        }
        public abstract void BindToDevice(GraphicsDevice device);
        public abstract void UnBindFromDevice();
        public abstract bool IsBoundToDevice { get; }

        #endregion
    }

}
