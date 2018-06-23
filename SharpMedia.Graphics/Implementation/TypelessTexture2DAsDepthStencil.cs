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

namespace SharpMedia.Graphics.Implementation
{
    internal class TypelessTexture2DAsDepthStencil : DepthStencilTargetView
    {
        #region Private Members
        object syncRoot = new object();
        uint usedByDevice = 0;
        TypelessTexture2D texture;
        PixelFormat format;
        uint mipmap = 0;
        bool multisample = false;
        bool disposed = false;
        #endregion

        #region Internal Methods

        void Dispose(bool fin)
        {
            if (disposed) return;

            disposed = true;

            if (view != null)
            {
                view.Dispose();
                view = null;
            }

            texture.Release();

            if (!fin)
            {
                GC.SuppressFinalize(this);
                texture = null;
            }
        }

        ~TypelessTexture2DAsDepthStencil()
        {
            Dispose(true);
        }

        internal TypelessTexture2DAsDepthStencil(TypelessTexture2D texture, PixelFormat fmt, uint mipmap)
        {
            this.texture = texture;
            this.format = fmt;
            this.mipmap = mipmap;

            texture.AddRef();
        }

        internal TypelessTexture2DAsDepthStencil(TypelessTexture2D texture, PixelFormat fmt)
        {
            this.texture = texture;
            this.format = fmt;
            this.multisample = true;

            texture.AddRef();
        }

        #endregion

        #region Public Members

        internal override void UsedByDevice()
        {
            if (usedByDevice == 0) Monitor.Enter(syncRoot);
            usedByDevice++;
        }

        internal override void UnusedByDevice()
        {
            if (--usedByDevice == 0) Monitor.Exit(syncRoot);
        }

        public override void Dispose()
        {
            lock (syncRoot)
            {
                Dispose(true);
            }
        }

        public override object TypelessResource
        {
            get { return texture; }
        }

        public override GraphicsLocality Locality
        {
            get
            {
                return texture.Locality;
            }
            set
            {
                texture.Locality = value;
            }
        }

        public override void BindToDevice(GraphicsDevice device)
        {
            lock (syncRoot)
            {
                texture.BindToDevice(device);

                if (view == null)
                {
                    view = device.DriverDevice.CreateDepthStencilTargetView(texture.DeviceData,
                        multisample ? SharpMedia.Graphics.Driver.UsageDimensionType.Texture2D :
                        SharpMedia.Graphics.Driver.UsageDimensionType.Texture2DMS, format.CommonFormatLayout,
                        mipmap, 0, 0);       
                }
            }
        }

        public override void UnBindFromDevice()
        {
            lock (syncRoot)
            {
                texture.UnBindFromDevice();

                if (view != null)
                {
                    view.Dispose();
                    view = null;
                }
            }
        }

        public override bool IsBoundToDevice
        {
            get { return view != null; }
        }

        #endregion
    }
}
