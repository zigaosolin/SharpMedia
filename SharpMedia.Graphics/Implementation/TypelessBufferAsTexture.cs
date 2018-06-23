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
    /// Typeless buffer as texture view.
    /// </summary>
    internal sealed class TypelessBufferAsTexture : TextureView
    {
        #region Private Members
        TypelessBuffer buffer;
        PixelFormat format;
        ulong offset;
        uint width;

        // Access methods
        Mipmap mipmap;
        uint usedByDevice = 0;
        #endregion

        #region Internal Methods

        internal TypelessBufferAsTexture(PixelFormat format, ulong offset, uint width, TypelessBuffer buffer)
        {
            this.format = format;
            this.offset = offset;
            this.buffer = buffer;
            this.width = width;

            buffer.AddRef();

            // We try to bind to device.
            if (this.buffer.Device != null)
            {
                // We bind to device.
                BindToDevice(buffer.Device);
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
                throw new ObjectDisposedException("Buffer already disposed, cannot use it.");
            }
        }

        void AssertNotLocked()
        {
            if (mipmap != null)
            {
                throw new InvalidOperationException("Buffer view is given CPU access.");
            }
        }

        void AssertLocked()
        {
            if (mipmap == null)
            {
                throw new InvalidOperationException("Buffer view is not given CPU access.");
            }
        }

        void Dispose(bool fromFinalizer)
        {

            if (cacheableState == SharpMedia.Caching.CacheableState.Disposed) return;
            cacheableState = SharpMedia.Caching.CacheableState.Disposed;
            buffer.Release();

            if (!fromFinalizer)
            {
                GC.SuppressFinalize(this);
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

        ~TypelessBufferAsTexture()
        {
            Dispose(true);
        }

        public override PinFormat ViewType
        {
            get { return PinFormat.BufferTexture; }
        }

        public override PixelFormat Format
        {
            get { return format; }
        }

        public override uint MipmapCount
        {
            get { return 1; }
        }

        public override void GenerateMipmaps(BuildImageFilter filter)
        {
            // Silently allow, since mipmap count is 1.
        }

        public override uint Width
        {
            get 
            {
                return (uint)((buffer.ByteSize - offset) / (ulong)width);
            }
        }

        public override uint Height
        {
            get 
            {
                return 1;
            }
        }

        public override Mipmap Map(MapOptions op, uint mipmap, uint face, bool uncompress)
        {
            Monitor.Enter(syncRoot);
            try
            {
                AssertNotDisposed();

                throw new NotSupportedException("Mapping a buffer view is not supported.");
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
                buffer.UnMap();
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

                throw new NotSupportedException("Cloning typeless buffer is not supported.");
            }
        }

        public override SharpMedia.Resources.ResourceAddress Address
        {
            get { return buffer.Address; }
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
            get { return buffer; }
        }

        public override GraphicsLocality Locality
        {
            get
            {
                return buffer.Locality;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    buffer.Locality = value;
                }
            }
        }

        public override void BindToDevice(GraphicsDevice device)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                // We first bind underlaying (forced).
                buffer.BindToDevice(device);

                // Bind only if not bound.
                if (view == null)
                {
                    // We bind ourself.
                    view = device.DriverDevice.CreateTextureView(buffer.DeviceData,
                        Driver.UsageDimensionType.Buffer, format.CommonFormatLayout,
                        offset, width, 0);
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
                mipmap = Map(MapOptions.Read, 0);
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
