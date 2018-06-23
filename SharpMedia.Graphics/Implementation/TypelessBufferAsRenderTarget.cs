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
    internal class TypelessBufferAsRenderTarget : RenderTargetView
    {
        #region Private Members
        ulong offset;
        uint width;
        PixelFormat format;
        TypelessBuffer buffer;
        #endregion

        #region Internal Methods

        internal TypelessBufferAsRenderTarget(TypelessBuffer buffer, PixelFormat fmt, ulong offset, uint width)
        {
            this.buffer = buffer;
            this.format = fmt;
            this.offset = offset;
            this.width = width;

            buffer.AddRef();
        }


        internal override void UnusedByDevice()
        {
            buffer.UnusedByDevice();

            // It was used as RT, potencially changed.
            buffer.SignalChanged();
            base.UnusedByDevice();
        }

        internal override void UsedByDevice()
        {
            base.UsedByDevice();
            buffer.UsedByDevice();

        }

        void Dispose(bool fin)
        {
            // Already disposed.
            if (cacheableState == SharpMedia.Caching.CacheableState.Disposed) return;

            buffer.Release();

            if (this.view != null)
            {
                view.Dispose();
            }

            if (!fin)
            {
                GC.SuppressFinalize(this);
                buffer = null;
            }

            cacheableState = SharpMedia.Caching.CacheableState.Disposed;
        }

        ~TypelessBufferAsRenderTarget()
        {
            Dispose(true);
        }

        #endregion

        #region Overrides


        public override void Dispose()
        {
            lock (syncRoot)
            {
                Dispose(false);
            }
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

                buffer.BindToDevice(device);

                // We now create view.
                if (view == null)
                {
                    view = device.DriverDevice.CreateRenderTargetView(buffer.DeviceData,
                        UsageDimensionType.Buffer,
                        format.CommonFormatLayout, offset, width, 0);

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
            get { return buffer; }
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
            // ignore, no mimpaps
        }

        public override uint Width
        {
            get { return (uint)((buffer.ByteSize - offset) / (ulong)width); }
        }

        public override uint Height
        {
            get { return 1; }
        }

        public override Mipmap Map(MapOptions op, uint mipmap, uint face, bool uncompress)
        {
            Monitor.Enter(syncRoot);
            try
            {
                AssertNotDisposed();

                throw new NotSupportedException();
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

        public override Image CloneSameType(PixelFormat fmt, uint width, uint height, uint depth,
            uint faces, uint mipmaps)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                throw new NotSupportedException();

            }
        }

        public override SharpMedia.Resources.ResourceAddress Address
        {
            get { return buffer.Address; }
        }

        #endregion
    }
}
