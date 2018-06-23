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

namespace SharpMedia.Graphics
{
    /// <summary>
    /// An index buffer view.
    /// </summary>
    public class IndexBufferView : IResourceView, IGraphicsLocality
    {
        #region Private Members
        object syncRoot = new object();
        TypelessBuffer typelessBuffer;
        ulong offset;
        IndexFormat format;
        Driver.IIBufferView view;
        uint usedByDevice = 0;
        bool isDisposed = false;
        #endregion

        #region Internal Methods

        internal Driver.IIBufferView DeviceData
        {
            get
            {
                return view;
            }
        }

        internal void UsedByDevice()
        {
            typelessBuffer.UsedByDevice();

            usedByDevice++;

            if (usedByDevice == 1)
            {
                Monitor.Enter(syncRoot);
            }
        }

        internal void UnusedByDevice()
        {
            usedByDevice--;

            if (usedByDevice == 0)
            {
                Monitor.Exit(syncRoot);
            }

            typelessBuffer.UnusedByDevice();
        }

        internal IndexBufferView(TypelessBuffer buffer, ulong offset, IndexFormat format)
        {
            this.typelessBuffer = buffer;
            this.offset = offset;
            this.format = format;

            buffer.AddRef();
        }

        #endregion

        #region Private Members

        void AssertNotDisposed()
        {
            if (isDisposed) throw new ObjectDisposedException("Index buffer was disposed, call invalid.");
        }

        void Dispose(bool fin)
        {
            
            if (isDisposed) return;

            if (view != null)
            {
                view.Dispose();
                view = null;
            }

            isDisposed = true;

            if (!fin)
            {
                typelessBuffer.Release();
            }

            // We leave buffer to be garbage collected.
            if (!fin)
            {
                typelessBuffer = null;
                GC.SuppressFinalize(this);
            }
            
        }

        ~IndexBufferView()
        {
            Dispose(true);
        }

        #endregion

        #region IResourceView Members

        public object TypelessResource
        {
            get { return typelessBuffer; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Typeless buffer.
        /// </summary>
        public TypelessBuffer TypelessBuffer
        {
            get
            {
                return typelessBuffer;
            }
        }

        /// <summary>
        /// Gets the format.
        /// </summary>
        public IndexFormat Format
        {
            get
            {
                return format;
            }
        }

        #endregion

        #region Data Manipulation

        /// <summary>
        /// Maps buffers, sets data and unmaps it.
        /// </summary>
        public void SetData(uint[] data)
        {
            SetData(data, 0);
        }

        /// <summary>
        /// Maps buffers, sets data and unmaps it.
        /// </summary>
        public unsafe void SetData(uint[] data, ulong offset)
        {
            lock (syncRoot)
            {
                if (!format.IsWide) throw new InvalidOperationException("Format and data incompatible.");

                ulong dataLength = (ulong)data.LongLength;

                // May throw iundex out of range.
                byte[] dstBuffer = typelessBuffer.Map(MapOptions.Write, this.offset + offset * 3,
                    dataLength * format.ByteSize);

                try
                {
                    // We set all data.
                    fixed (byte* dst = dstBuffer)
                    {
                        Common.Memcpy(data, dst, dataLength);
                    }
                }
                finally
                {
                    typelessBuffer.UnMap();
                }
            }
        }

        /// <summary>
        /// Maps buffers, sets data and unmaps it.
        /// </summary>
        public void SetData(ushort[] data)
        {
            SetData(data, 0);
        }

        /// <summary>
        /// Maps buffers, sets data and unmaps it.
        /// </summary>
        public unsafe void SetData(ushort[] data, ulong offset)
        {
            lock (syncRoot)
            {
                if (!format.IsWide) throw new InvalidOperationException("Format and data incompatible.");

                ulong dataLength = (ulong)data.LongLength;

                // May throw iundex out of range.
                byte[] dstBuffer = typelessBuffer.Map(MapOptions.Write, this.offset + offset * 2,
                    dataLength * format.ByteSize);

                try
                {
                    // We set all data.
                    fixed (byte* dst = dstBuffer)
                    {
                        Common.Memcpy(data, dst, dataLength);
                    }
                }
                finally
                {
                    typelessBuffer.UnMap();
                }
            }
        }

        /// <summary>
        /// Obtains wide data.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe uint[] GetDataWide(ulong offset, ulong count)
        {
            lock (syncRoot)
            {
                if (!format.IsWide)
                {
                    throw new InvalidOperationException("Data is not wide, use GetDataShort");
                }

                // We create buffer.
                uint[] buffer = new uint[count];

                // We map buffer.
                byte[] data = typelessBuffer.Map(MapOptions.Read, this.offset + offset * 4, count * 4);

                try
                {
                    fixed (byte* src = data)
                    {
                        Common.Memcpy(src, buffer, count * 4);
                    }
                }
                finally
                {
                    typelessBuffer.UnMap();
                }

                return buffer;

            }
        }

        /// <summary>
        /// Obtains short data.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe ushort[] GetDataShort(ulong offset, ulong count)
        {
            lock (syncRoot)
            {
                if (!format.IsShort)
                {
                    throw new InvalidOperationException("Data is not wide, use GetDataWide");
                }

                // We create buffer.
                ushort[] buffer = new ushort[count];

                // We map buffer.
                byte[] data = typelessBuffer.Map(MapOptions.Read, this.offset + offset * 2, count * 2);

                try
                {
                    fixed (byte* src = data)
                    {
                        Common.Memcpy(src, buffer, count * 2);
                    }
                }
                finally
                {
                    typelessBuffer.UnMap();
                }

                return buffer;

            }
        }
        
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (syncRoot)
            {
                Dispose(false);
            }
        }

        #endregion

        #region IGraphicsLocality Members

        public GraphicsLocality Locality
        {
            get
            {
                return typelessBuffer.Locality;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    typelessBuffer.Locality = value;
                }
            }
        }

        public void BindToDevice(GraphicsDevice device)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                typelessBuffer.BindToDevice(device);
                if (view == null)
                {
                    view = device.DriverDevice.CreateIBufferView(typelessBuffer.DeviceData, format.IsWide, offset);
                }
            }
        }

        public void UnBindFromDevice()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                // we only unbind self, not also buffer.
                if (view != null)
                {
                    view.Dispose();
                    view = null;
                }
            }
        }

        public bool IsBoundToDevice
        {
            get { return view != null; }
        }

        #endregion
    }
}
