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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A swap chain is a special render target that consists of RGBA buffer plus
    /// the depth/stencil buffer. It is a chain, meaning that there can be many back-buffers
    /// to avoid sync locks (waiting for the monitor to display the scene while we could have already
    /// been rendering).
    /// Swap chain is used only for render-display features and supports reading of back buffers. It can
    /// be bound to multiple targets (depth/stencil and RGBA). It is asociated with window and resizes
    /// with it.
    /// </summary>
    public sealed class SwapChain : RenderTargetView
    {
        #region Private Members
        GraphicsDevice device;
        Window window;
        Driver.ISwapChain chain;
        PixelFormat format;
        bool fullscreen;
        #endregion

        #region Internal Methods

        internal SwapChain(GraphicsDevice device, Window window, PixelFormat fmt, bool fullscreen, Driver.ISwapChain chain)
            : base(chain)
        {
            this.window = window;
            this.device = device;
            this.format = fmt;
            this.chain = chain;
            this.fullscreen = fullscreen;
        }

        internal void Resized(uint width, uint height)
        {
            // We resize the swap chain.
            chain.Resize(width, height);
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Presents the swap chain. It must not be bound as render target (unbind it first).
        /// </summary>
        /// <remarks>Unbind is automatic, if device is not locked by you anymore.</remarks>
        public void Present()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                chain.Present();
            }
        }

        /// <summary>
        /// Gets fullscreen.
        /// </summary>
        public bool Fullscreen
        {
            get
            {
                return fullscreen;
            }
        }

        /// <summary>
        /// Changes the swap chain's format, width, height or fullscreen. Window is also affected.
        /// </summary>
        /// <param name="width">0 means no change.</param>
        /// <param name="height">0 means no change</param>
        /// <param name="format">Must be supported by hardware, if null it means no change.</param>
        public void Reset(uint width, uint height, PixelFormat format, bool fullscreen)
        {
            lock (syncRoot)
            {
                if (format == null) format = this.format;
                if (width == 0) width = window.Width;
                if (height == 0) height = window.Height;
                if (format.CommonFormatLayout == CommonPixelFormatLayout.NotCommonLayout)
                {
                    throw new ArgumentException("Pixel format must be valid.");
                }

                chain.Reset(width, height, format.CommonFormatLayout, fullscreen);
                window.Update(width, height);
            }
        }

        /// <summary>
        /// Gets the window this swap chain is associated with (and resizes with).
        /// </summary>
        /// <value>The window.</value>
        public Window Window
        {
            get
            {
                return window;
            }
            internal set
            {
                window = value;
            }
        }

        /// <summary>
        /// Gets the device. This is always a master device (e.g. owner).
        /// </summary>
        /// <value>The device.</value>
        public GraphicsDevice Device
        {
            get
            {
                return device;
            }
        }


        /// <summary>
        /// Blocks the thread until present is finished.
        /// </summary>
        public void WaitForPresent()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                chain.Finish();
            }
        }
    
        public override GraphicsLocality Locality
        {
            get
            {
                return GraphicsLocality.DeviceMemoryOnly;
            }
            set
            {
                if (value != GraphicsLocality.DeviceMemoryOnly)
                {
                    throw new ArgumentException("SwapChain supports only DeviceMemoryOnly mode.");
                }
            }
        }

        public override void BindToDevice(GraphicsDevice device)
        {
        }

        public override void UnBindFromDevice()
        {
            throw new InvalidOperationException("Swap chain cannot be unbound from device.");
        }

        public override bool IsBoundToDevice
        {
            get { return true; }
        }

        public override object TypelessResource
        {
            get { return this; }
        }

        public override PixelFormat Format
        {
            get { return this.format; }
        }

        public override uint MipmapCount
        {
            get { return 1; }
        }

        public override void GenerateMipmaps(BuildImageFilter filter)
        {
        }

        public override uint Width
        {
            get { return window.Width; }
        }

        public override uint Height
        {
            get { return window.Height; }
        }

        public override Mipmap Map(MapOptions op, uint mipmap, uint face, bool uncompress)
        {
            throw new NotSupportedException("Cannot read/write to SwapChain directly");
        }

        public override void UnMap(uint mipmap)
        {
            throw new NotSupportedException("Cannot read/write to SwapChain directly");
        }

        public override Image CloneSameType(PixelFormat fmt, uint width, uint height, uint depth, uint faces, uint mipmaps)
        {
            throw new InvalidOperationException("The swap chain cannot be clone, there is only one instance of it.");
        }

        public override SharpMedia.Resources.ResourceAddress Address
        {
            get { return new SharpMedia.Resources.ResourceAddress(Placement.DeviceMemory); }
        }

        #endregion
    }
}
