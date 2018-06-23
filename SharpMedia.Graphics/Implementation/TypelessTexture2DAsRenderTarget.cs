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
using SharpMedia.Graphics.Driver;
using System.Threading;

namespace SharpMedia.Graphics.Implementation
{
    internal class TypelessTexture2DAsRenderTarget : RenderTargetView
    {
        #region Private Members
        bool multisample = false;
        uint mipmapSlice;
        PixelFormat format;
        TypelessTexture2D texture;
        #endregion

        #region Internal Methods

        internal TypelessTexture2DAsRenderTarget(TypelessTexture2D texture, PixelFormat fmt)
        {
            this.texture = texture;
            this.format = fmt;
            this.multisample = true;

            texture.AddRef();
        }

        internal TypelessTexture2DAsRenderTarget(TypelessTexture2D texture, PixelFormat fmt, uint mipmapSlice)
        {
            this.texture = texture;
            this.mipmapSlice = mipmapSlice;
            this.format = fmt;

            texture.AddRef();
        }

        internal override void UnusedByDevice()
        {
            texture.UnusedByDevice();

            // It was used as RT, potencially changed.
            texture.SignalChanged();
            base.UnusedByDevice();
        }

        internal override void UsedByDevice()
        {
            base.UsedByDevice();
            texture.UsedByDevice();
            
        }

        void Dispose(bool fin)
        {
            // Already disposed.
            if (cacheableState == SharpMedia.Caching.CacheableState.Disposed) return;

            texture.Release();

            if (this.view != null)
            {
                view.Dispose();
            }

            if (!fin)
            {
                GC.SuppressFinalize(this); 
                texture = null;
            }

            cacheableState = SharpMedia.Caching.CacheableState.Disposed;
        }

        ~TypelessTexture2DAsRenderTarget()
        {
            Dispose(true);
        }

        #endregion

        #region Overrides


        public override void Dispose()
        {
            lock(syncRoot)
            {
                Dispose(false);
            }
        }

        public override GraphicsLocality Locality
        {
            get
            {
                return texture.Locality;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();

                    texture.Locality = value;
                }
            }
        }

        public override void BindToDevice(GraphicsDevice device)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                texture.BindToDevice(device);

                // We now create view.
                if (view == null)
                {
                    view = device.DriverDevice.CreateRenderTargetView(texture.DeviceData,
                        multisample ? UsageDimensionType.Texture2DMS : UsageDimensionType.Texture2D,
                        format.CommonFormatLayout, mipmapSlice, 0, 0);

                }
            }
        }

        public override void UnBindFromDevice()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

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

        public override object TypelessResource
        {
            get { return texture; }
        }

        public override PixelFormat Format
        {
            get { return format; }
        }

        public override uint MipmapCount
        {
            get { return texture.MipmapCount; }
        }

        public override void GenerateMipmaps(BuildImageFilter filter)
        {
            texture.GenerateMipmaps(filter);
        }

        public override uint Width
        {
            get { return texture.Width; }
        }

        public override uint Height
        {
            get { return texture.Height; }
        }

        public override Mipmap Map(MapOptions op, uint mipmap, uint face, bool uncompress)
        {
            Monitor.Enter(syncRoot);
            try
            {
                AssertNotDisposed();

                return texture.Map(op, mipmap, face, uncompress);
            }
            catch (Exception )
            {
                Monitor.Exit(syncRoot);
                throw;
            }
        }

        public override void UnMap(uint mipmap)
        {
            try
            {
                AssertNotDisposed();
                texture.UnMap(mipmap);
            }
            finally
            {
                Monitor.Exit(syncRoot);
            }
        }

        public override Image CloneSameType(PixelFormat fmt, uint width, uint height, uint depth, 
            uint faces, uint mipmaps)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                TypelessTexture2D b = texture.CloneSameType(fmt, width, height, depth, faces, mipmaps) as TypelessTexture2D;

                if (multisample)
                {
                    return new TypelessTexture2DAsRenderTarget(b, format);
                }
                else
                {
                    return new TypelessTexture2DAsRenderTarget(b, format, mipmapSlice);
                }

            }
        }

        public override SharpMedia.Resources.ResourceAddress Address
        {
            get { return texture.Address; }
        }

        #endregion
    }
}
