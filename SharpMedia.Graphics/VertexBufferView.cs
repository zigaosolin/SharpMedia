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
using SharpMedia.AspectOriented;
using System.Runtime.InteropServices;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A vertex buffer view.
    /// </summary>
    [Serializable]
    public class VertexBufferView : IResourceView, IGraphicsLocality
    {
        #region Private Members
        object syncRoot = new object();
        UpdateFrequency updateFrequency = UpdateFrequency.PerVertex;
        uint updateFrequencyCount = 0;
        TypelessBuffer typelessBuffer;
        ulong offset;
        uint stride;
        VertexFormat format;
        Driver.IVBufferView view;
        uint usedByDevice = 0;
        bool isDisposed = false;
        #endregion

        #region Private Methods

        void AssertNotDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("Vertex buffer was already disposed.");
            }
        }

        ~VertexBufferView()
        {
            Dispose(true);
        }

        public void Dispose(bool finalizer)
        {
            
            if (!isDisposed)
            {
                if (view != null)
                {
                    view.Dispose();
                    view = null;
                    isDisposed = true;
                }

                typelessBuffer.Release();

                if (!finalizer)
                {
                    GC.SuppressFinalize(this);
                    typelessBuffer = null;
                }
            }
            
        }

        #endregion

        #region Internal Methods

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

        internal Driver.IVBufferView DeviceData
        {
            get
            {
                return view;
            }
        }

        internal VertexBufferView(TypelessBuffer buffer, VertexFormat format, ulong offset, uint stride,
                              UpdateFrequency updateFrequency, uint updateFrequencyCount)
        {
            this.offset = offset;
            this.format = format;
            this.typelessBuffer = buffer;
            this.updateFrequencyCount = updateFrequencyCount;
            this.updateFrequency = updateFrequency;
            this.stride = stride;

            // Add reference to buffer.
            buffer.AddRef();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the typeless buffer.
        /// </summary>
        public TypelessBuffer TypelessBuffer
        {
            get
            {
                return typelessBuffer;
            }
        }

        /// <summary>
        /// Returns the update frequency of view.
        /// </summary>
        public UpdateFrequency UpdateFrequency
        {
            get
            {
                return updateFrequency;
            }
        }

        /// <summary>
        /// How many objects (vertices, instances) must pass in order to iterate to next element.
        /// </summary>
        public uint UpdateFrequencyCount
        {
            get
            {
                return updateFrequencyCount;
            }
        }

        /// <summary>
        /// The stride of buffer.
        /// </summary>
        public uint Stride
        {
            get
            {
                return stride;
            }
        }

        /// <summary>
        /// Returns the vertex format asociated with this typed buffer.
        /// </summary>
        public VertexFormat Format
        {
            get
            {
                return format;
            }
        }

        /// <summary>
        /// Returns the offset.
        /// </summary>
        public ulong Offset
        {
            get
            {
                return offset;
            }
        }

        #endregion

        #region Data Manipulation

        /// <summary>
        /// Maps buffers, sets data and unmaps it.
        /// </summary>
        /// <param name="data">Data.</param>
        public void SetData<T>(T[] data) where T : struct
        {
            SetData(data, 0);

        }

        /// <summary>
        /// Maps buffers, sets data and unmaps it.
        /// </summary>
        /// <param name="offset">The element offset.</param>
        public unsafe void SetData<T>([NotNull] T[] data, ulong offset) where T : struct
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                // Validate T.
                ulong structSize = (ulong)Marshal.SizeOf(typeof(T));

                if (structSize != format.ByteSize)
                {
                    throw new ArgumentException("The T is not compatible with vertex format.");
                }

                // We first map it, it may throw (if it throws, it is automatically unlocked).
                byte[] dstBuffer = TypelessBuffer.Map(MapOptions.Write,
                    this.offset + offset * format.ByteSize, (ulong)data.LongLength * format.ByteSize);

                

                try
                {

                    fixed (byte* p = dstBuffer)
                    {
                        Common.Memcpy(data, p, (ulong)dstBuffer.LongLength);
                    }
                }
                finally
                {
                    // Must unmap.
                    TypelessBuffer.UnMap();
                }

            }

        }

        /// <summary>
        /// Reads data to buffer.
        /// </summary>
        public unsafe T[] GetData<T>(ulong offset, ulong count) where T : struct
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                // Validate T.
                ulong structSize = (ulong)Marshal.SizeOf(typeof(T));

                if (structSize != format.ByteSize)
                {
                    throw new ArgumentException("The T is not compatible with vertex format.");
                }

                T[] data = new T[count];

                // We first map it, it may throw (if it throws, it is automatically unlocked).
                byte[] srcBuffer = TypelessBuffer.Map(MapOptions.Read,
                    this.offset + offset * format.ByteSize, count * format.ByteSize);


                
                try
                {
                    fixed(byte* p = srcBuffer)
                    {
                        Common.Memcpy(p, data, (ulong)srcBuffer.LongLength);
                    }
                }
                finally
                {
                    // Must unmap.
                    TypelessBuffer.UnMap();
                }

                return data;
            }


        }
        

        #endregion

        #region IResourceView Members

        public object TypelessResource
        {
            get { return typelessBuffer; }
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
                    view = device.DriverDevice.CreateVBufferView(typelessBuffer.DeviceData, stride, offset); 
                }
            }
        }

        public void UnBindFromDevice()
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

        public bool IsBoundToDevice
        {
            get { return view != null; }
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

        #region Static Members

        /// <summary>
        /// A helper to create a vertex buffer.
        /// </summary>
        /// <returns></returns>
        public static VertexBufferView Create(GraphicsDevice device, VertexFormat format, Usage usage, 
            CPUAccess access, GraphicsLocality locality, ulong elementCount)
        {
            TypelessBuffer buffer = new TypelessBuffer(usage, BufferUsage.VertexBuffer, access, locality,
                format.ByteSize * elementCount);

            buffer.DisposeOnViewDispose = true;

            return buffer.CreateVertexBuffer(format);
        }

        /// <summary>
        /// Creates and fills vertex buffer.
        /// </summary>
        /// <returns></returns>
        public static VertexBufferView Create<T>(GraphicsDevice device, VertexFormat format, Usage usage,
            CPUAccess access, GraphicsLocality locality, params T[] elements) where T : struct
        {
            VertexBufferView buffer = Create(device, format, usage, access,
                locality, (ulong)elements.LongLength);

            try
            {
                buffer.SetData<T>(elements);
            }
            catch (Exception)
            {
                buffer.Dispose();
                throw;
            }

            return buffer;

        }

        // TODO: more helpers

        #endregion
    }
}
