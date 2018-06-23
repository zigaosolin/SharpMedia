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
using System.Threading;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A render target view.
    /// </summary>
    public abstract class RenderTargetView : Image, IResourceView, IGraphicsLocality
    {
        #region Private Members
        protected Driver.IRenderTargetView view;
        protected uint usedByDevice = 0;
        #endregion

        #region Internal Methods

        internal RenderTargetView()
        {
        }

        internal RenderTargetView(Driver.IRenderTargetView view)
        {
            this.view = view;
        }

        /// <summary>
        /// Gets the device data.
        /// </summary>
        /// <value>The device data.</value>
        internal Driver.IRenderTargetView DeviceData
        {
            get
            {
                return view;
            }
        }

        /// <summary>
        /// Useds the by device.
        /// </summary>
        internal virtual void UsedByDevice()
        {
            usedByDevice++;
            if (usedByDevice == 1)
            {
                Monitor.Enter(syncRoot);
            }
        }

        /// <summary>
        /// Unuseds the by device.
        /// </summary>
        internal virtual void UnusedByDevice()
        {
            usedByDevice--;
            if (usedByDevice == 0)
            {
                Monitor.Exit(syncRoot);
            }
        }

        #endregion

        #region IGraphicsLocality Members

        public abstract GraphicsLocality Locality { get; set; }
        public abstract void BindToDevice(GraphicsDevice device);
        public abstract void UnBindFromDevice();
        public abstract bool IsBoundToDevice { get; }

        #endregion

        #region IResourceView Members

        public abstract object TypelessResource { get; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a 2D render target.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="multiSampleType"></param>
        /// <param name="multiSampleQuality"></param>
        /// <returns></returns>
        public static RenderTargetView Create(GraphicsDevice graphicsDevice, 
                                PixelFormat format, uint width, uint height,
                                uint multiSampleType, uint multiSampleQuality)
        {
            TypelessTexture2D t = new TypelessTexture2D(graphicsDevice, Usage.Default, TextureUsage.RenderTarget, CPUAccess.None,
                format, width, height, 1, multiSampleType, multiSampleQuality, GraphicsLocality.DeviceOrSystemMemory, null);

            t.DisposeOnViewDispose = true;

            return t.CreateRenderTargetMS(format);
        }

        #endregion
    }
}
