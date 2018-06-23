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
using SharpMedia.Resources;

namespace SharpMedia.Graphics.Implementation
{
    /// <summary>
    /// The typeless texture 2D bound as Texture2D.
    /// </summary>
    [Serializable]
    internal sealed class TypelessTexture2DAsTexture2D : TextureView
    {
        #region Private Members
        TypelessTexture2D texture2D;
        PixelFormat format;
        uint mostDetailedMipmap;
        uint mipmapCount;

        // Access methods
        Mipmap mipmap;
        uint usedByDevice = 0;
        #endregion

        #region Internal Methods

        internal TypelessTexture2DAsTexture2D(PixelFormat format,
            uint mostDetailed, uint count, TypelessTexture2D texture2D)
        {
            this.format = format;
            this.mostDetailedMipmap = mostDetailed;
            this.mipmapCount = count;
            this.texture2D = texture2D;

            // We try to bind to device.
            if (this.texture2D.Device != null)
            {
                // We bind to device.
                BindToDevice(texture2D.Device);
            }
        }


        internal override void UsedByDevice()
        {
            if (++usedByDevice == 1)
            {
                Monitor.Enter(syncRoot);
            }
        }

        internal override void UnusedByDevice()
        {
            if (--usedByDevice == 0)
            {
                Monitor.Exit(syncRoot);
            }
        }

        #endregion

        #region Private Methods

        new void AssertNotDisposed()
        {
            if (cacheableState == SharpMedia.Caching.CacheableState.Disposed)
            {
                throw new ObjectDisposedException("Image already disposed, cannot use it.");
            }
        }

        void AssertNotLocked()
        {
            if (mipmap != null)
            {
                throw new InvalidOperationException("Image view is given CPU access.");
            }
        }

        void AssertLocked()
        {
            if (mipmap == null)
            {
                throw new InvalidOperationException("Image view is not given CPU access.");
            }
        }

        void Dispose(bool fromFinalizer)
        {
            if (cacheableState == SharpMedia.Caching.CacheableState.Disposed) return;
            
            cacheableState = SharpMedia.Caching.CacheableState.Disposed;
            texture2D.Release();

            if (!fromFinalizer)
            {
                GC.SuppressFinalize(this);
                texture2D = null;
            }
            

            // We fire events.
            Action<IResource> r = onDisposed;
            if (r != null)
            {
                r(this);
            }
        }

        #endregion

        #region Public Methods

        ~TypelessTexture2DAsTexture2D()
        {
            Dispose(true);
        }

        public override PinFormat ViewType
        {
            get { return PinFormat.Texture2D; }
        }

        public override PixelFormat Format
        {
            get { return format; }
        }

        public override uint MipmapCount
        {
            get { return mipmapCount; }
        }

        public override void GenerateMipmaps(BuildImageFilter filter)
        {
            // Generates all mipmaps of TypelessTexture2D.
            texture2D.GenerateMipmaps(filter);
        }

        public override uint Width
        {
            get 
            {

                uint width = texture2D.Width >> (int)mostDetailedMipmap;
                if (width == 0) return 1;
                return width;
            }
        }

        public override uint Height
        {
            get 
            {
                uint height = texture2D.Height >> (int)mostDetailedMipmap;
                if (height == 0) return 1;
                return height;
            }
        }

        public override Mipmap Map(MapOptions op, uint mipmap, uint face, bool uncompress)
        {
            Monitor.Enter(syncRoot);
            try
            {
                AssertNotDisposed();

                // We map the mipmap (no need to lock).
                return texture2D.Map(op, mostDetailedMipmap + mipmap, face, uncompress);
            }
            catch (Exception)
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
                texture2D.UnMap(mipmap);
            }
            finally
            {
                Monitor.Exit(syncRoot);
            }
        }

        public override Image CloneSameType(PixelFormat fmt, uint width, uint height, 
            uint depth, uint faces, uint mipmaps)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                // We clone the same image and the view.
                TypelessTexture2D baseTexture = texture2D.CloneSameType(fmt, width, 
                    height, depth, faces, mipmaps) as TypelessTexture2D;

                // And create the view (mostDetailed and mipmapCount may be clamped).
                return baseTexture.CreateTexture(format, mostDetailedMipmap, mipmapCount);
            }
        }

        public override SharpMedia.Resources.ResourceAddress Address
        {
            get { return texture2D.Address; }
        }


        public override void Dispose()
        {
            lock (syncRoot)
            {
                Dispose(false);
            }
        }

        public override object TypelessResource
        {
            get { return texture2D; }
        }

        public override GraphicsLocality Locality
        {
            get
            {
                return texture2D.Locality;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    texture2D.Locality = value;
                }
            }
        }

        public override void BindToDevice(GraphicsDevice device)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                // We first bind underlaying (forced).
                texture2D.BindToDevice(device);

                // Bind only if not bound.
                if (view == null)
                {
                    // We bind ourself.
                    view = device.DriverDevice.CreateTextureView(texture2D.DeviceData,
                        Driver.UsageDimensionType.Texture2D, format.CommonFormatLayout,
                        mostDetailedMipmap, mipmapCount, 0);
                }
                
            }
        }

        public override void UnBindFromDevice()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                // Free view.
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

        #region Access Methods

        public override void BeginCPUAccess()
        {
            Monitor.Enter(syncRoot);
            try
            {
                AssertNotDisposed();
                AssertNotLocked();

                // We extract mipmap.
                mipmap = Map(MapOptions.Read, mostDetailedMipmap);
            }
            catch (Exception)
            {
                Monitor.Exit(syncRoot);
                throw;
            }

        }

        public override void EndCPUAccess()
        {
            AssertNotDisposed();
            AssertLocked();
            mipmap.Dispose();

            mipmap = null;
            Monitor.Exit(syncRoot);
        }

        public override SharpMedia.Math.Vector4f Load(SharpMedia.Math.Vector2i x)
        {
            return mipmap.Load(x);
        }

        public override SharpMedia.Math.Vector4f Load(SharpMedia.Math.Vector3i x)
        {
            return mipmap.Load(x);
        }

        public override SharpMedia.Math.Vector4f Load(SharpMedia.Math.Vector4i x)
        {
            return mipmap.Load(x);
        }

        public override SharpMedia.Math.Vector4f Sample(SharpMedia.Graphics.States.SamplerState state, SharpMedia.Math.Vector2f x)
        {
            return Sampler.Sample(state, this, x);
        }

        public override SharpMedia.Math.Vector4f Sample(SharpMedia.Graphics.States.SamplerState state, SharpMedia.Math.Vector3f x)
        {
            return Sampler.Sample(state, this, x);
        }

        public override SharpMedia.Math.Vector4f Sample(SharpMedia.Graphics.States.SamplerState state, SharpMedia.Math.Vector4f x)
        {
            return Sampler.Sample(state, this, x);
        }

        #endregion


    }
}
