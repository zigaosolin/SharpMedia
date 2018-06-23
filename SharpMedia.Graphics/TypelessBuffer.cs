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
using System.Runtime.Serialization;
using System.Threading;
using SharpMedia.Caching;
using SharpMedia.AspectOriented;
using SharpMedia.Graphics.Shaders;
using SharpMedia.Math.Shapes.Storage;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A graphics buffer is "general purpuse" buffer that can be used as vertex, index,
    /// constant buffer, render target, geometry output or as texture.
    /// </summary>
    [Serializable]
    public sealed class TypelessBuffer : IResource, IGraphicsLocality, 
                ISerializable, IMapable<byte[]>, IGraphicsSignature
    {
        #region Private Members
        // Immutable members:
        object syncRoot = new object();
        Usage usage;
        CPUAccess cpuAccess;
        BufferUsage bufferUsage;
        GraphicsLocality locality;
        ulong byteSize;

        // Custom control:
        bool disposeOnViewDispose = true;
        uint viewCounter = 0;
        uint usedByDeviceCount = 0;
        CacheableState cacheableState = CacheableState.Normal;
        
        // Events.
        Action<ICacheable> onTouch;
        Action<IResource> onWritten;
        Action<IResource> onDisposed;

        // Data (can be outdated due if buffer used as RT or GO):
        bool swOutOfDate = false;
        byte[] swData = null;

        // Driver Part:
        GraphicsDevice device;
        Driver.IBuffer driverPart;

        // Locking Part:
        MapOptions lockOptions;
        ulong lockOffset;
        ulong lockCount;
        byte[] lockedData;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a graphics buffer directly on hardware, with set usage, 
        /// format (e.g. is typed) and element count.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="format"></param>
        /// <param name="elementCount"></param>
        public TypelessBuffer([NotNull] GraphicsDevice device, Usage usage, BufferUsage bufferUsage,
                              CPUAccess access, GraphicsLocality locality, ulong byteSize)
            : this(usage, bufferUsage, access, locality, byteSize)
        {
            BindToDevice(device);
        }

        /// <summary>
        /// Creates a graphics buffer directly on hardware, with set usage, 
        /// format (e.g. is typed) and element count.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="format"></param>
        /// <param name="elementCount"></param>
        public TypelessBuffer([NotNull] GraphicsDevice device, Usage usage, BufferUsage bufferUsage, 
                              CPUAccess access, GraphicsLocality locality, byte[] data)
            : this(usage, bufferUsage, access, locality, 0)
        {
            this.swData = data.Clone() as byte[];
            BindToDevice(device);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsBuffer"/> class.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <param name="bufferUsage">The buffer usage.</param>
        /// <param name="access">The access.</param>
        /// <param name="locality">The locality.</param>
        /// <param name="format">The format.</param>
        /// <param name="elementCount">The element count.</param>
        public TypelessBuffer(Usage usage, BufferUsage bufferUsage, CPUAccess access,
                        GraphicsLocality locality, ulong byteSize)
        {
            this.usage = usage;
            this.bufferUsage = bufferUsage;
            this.cpuAccess = access;
            this.locality = locality;
            this.byteSize = byteSize;
            this.swData = new byte[byteSize];
        }

        #endregion

        #region Internal Methods

        internal Driver.IBuffer DeviceData
        {
            get
            {
                return driverPart;
            }
        }

        internal void UsedByDevice()
        {
            usedByDeviceCount++;
            if (usedByDeviceCount == 1)
            {
                Monitor.Enter(syncRoot);
            }
        }

        internal void UnusedByDevice()
        {
            usedByDeviceCount--;
            if (usedByDeviceCount == 0)
            {
                Monitor.Exit(syncRoot);
            }
        }

        ~TypelessBuffer()
        {
            Dispose(true);
        }

        #endregion

        #region Private Methods

        private void Dispose(bool finalizer)
        {
            if (cacheableState == CacheableState.Disposed) return;
            
           
            cacheableState = CacheableState.Disposed;
            device = null;
            if (driverPart != null)
            {
                driverPart.Dispose();
                driverPart = null;
            }

            // To free asocaited data (if buffer still referenced).
            swData = null;

            if (!finalizer)
            {
                GC.SuppressFinalize(this);
            }

            
            Action<IResource> t = onDisposed;
            if (t != null)
            {
                t(this);
            }
            
        }

        private void FillBuffer()
        {
            if (swData != null && this.swOutOfDate) return;
            swData = GetInternalData();
        }

        private void ReleaseDriverPart()
        {
            if (driverPart != null)
            {
                driverPart.Dispose();
                driverPart = null;
            }
        }

        private void AssertNotDisposed()
        {
            if (cacheableState == CacheableState.Disposed)
            {
                throw new ObjectDisposedException("Typeless buffer was already disposed.");
            }
        }

        private void AssertNotLocked()
        {
            if (lockedData != null)
            {
                throw new InvalidOperationException("Typeless buffer is locked.");
            }
        }

        private void AssertLocked()
        {
            if (lockedData == null)
            {
                throw new InvalidOperationException("Typeless buffer is not locked.");
            }
        }

        private byte[] GetInternalData()
        {
            return driverPart.Read(0, byteSize);
        }

        #endregion

        #region Properties

        /// <summary>
        /// This value indicates whether this buffer is disposed when all views 
        /// pointing to it are disposed. The default value is true.
        /// </summary>
        /// <remarks>
        /// This allows creating views upon this resource and using them.
        /// </remarks>
        public bool DisposeOnViewDispose
        {
            get
            {
                return disposeOnViewDispose;
            }
            set
            {
                lock (syncRoot)
                {
                    disposeOnViewDispose = value;
                }
            }
        }

        /// <summary>
        /// The CPU access to buffer.
        /// </summary>
        public CPUAccess CPUAccess
        {
            get
            {
                return cpuAccess;
            }
        }

        /// <summary>
        /// The usage of buffer.
        /// </summary>
        public Usage Usage
        {
            get
            {
                return usage;
            }
        }

        /// <summary>
        /// Returns device, if bound to it.
        /// </summary>
        public GraphicsDevice Device
        {
            get
            {
                return device;
            }
        }

        /// <summary>
        /// The buffer usage of buffer.
        /// </summary>
        public BufferUsage BufferUsage
        {
            get
            {
                return bufferUsage;
            }
        }

        /// <summary>
        /// Is it vertex buffer.
        /// </summary>
        public bool IsVertexBuffer
        {
            get
            {
                return (bufferUsage & BufferUsage.VertexBuffer) != 0;
            }
        }

        /// <summary>
        /// Is it index buffer.
        /// </summary>
        public bool IsIndexBuffer
        {
            get
            {
                return (bufferUsage & BufferUsage.IndexBuffer) != 0;
            }
        }

        /// <summary>
        /// Is it constant buffer.
        /// </summary>
        public bool IsConstantBuffer
        {
            get
            {
                return (bufferUsage & BufferUsage.ConstantBuffer) != 0;
            }
        }

        /// <summary>
        /// Is it geometry output buffer.
        /// </summary>
        public bool IsGeometryOutput
        {
            get
            {
                return (bufferUsage & BufferUsage.GeometryOutput) != 0;
            }
        }

        /// <summary>
        /// Is it render target buffer.
        /// </summary>
        public bool IsRenderTarget
        {
            get
            {
                return (bufferUsage & BufferUsage.RenderTarget) != 0;
            }
        }

        /// <summary>
        /// The byte count of buffer.
        /// </summary>
        public ulong ByteSize
        {
            get
            {
                return byteSize;
            }
        }

        #endregion

        #region View Creation Methods

        /// <summary>
        /// Creates a control point buffer view.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="offset">Offset in buffer.</param>
        /// <param name="updateFrequency">The update frequency.</param>
        /// <param name="updateFrequencyCount">The frequency count.</param>
        /// <returns></returns>
        public ControlPointBufferView CreateControlPointBuffer([NotNull] ControlPointFormat format,
            ulong offset, UpdateFrequency updateFrequency, uint updateFrequencyCount)
        {
            return new ControlPointBufferView(format, this, offset, updateFrequency, updateFrequencyCount);
        }

        /// <summary>
        /// Creates a control point buffer view.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="offset">Offset in buffer.</param>
        /// <returns></returns>
        public ControlPointBufferView CreateControlPointBuffer([NotNull] ControlPointFormat format, ulong offset)
        {
            return new ControlPointBufferView(format, this, offset);
        }

        /// <summary>
        /// Creates a control point buffer view.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public ControlPointBufferView CreateControlPointBuffer([NotNull] ControlPointFormat format)
        {
            return new ControlPointBufferView(format, this);
        }

        /// <summary>
        /// Creates a shape index buffer view.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The shape index buffer view.</returns>
        public ShapeIndexBufferView CreateShapeIndexBuffer([NotNull] ShapeIndexFormat format, ulong offset)
        {
            return new ShapeIndexBufferView(format, this, offset);
        }

        /// <summary>
        /// Creates a shape index buffer view.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The shape index buffer view.</returns>
        public ShapeIndexBufferView CreateShapeIndexBuffer([NotNull] ShapeIndexFormat format)
        {
            return new ShapeIndexBufferView(format, this);
        }

        /// <summary>
        /// Creates a full buffer render target view.
        /// </summary>
        /// <param name="format">The format, must be simple.</param>
        public RenderTargetView CreateRenderTarget([NotNull] PixelFormat format)
        {
            return CreateRenderTarget(format, 0, format.Size);
        }

        /// <summary>
        /// Creates a full buffer render target view with element offset and stride.
        /// </summary>
        /// <param name="format">The format, must be simple.</param>
        /// <param name="offset">Offset of format.</param>
        /// <param name="stride">The stride (element width).</param>
        public RenderTargetView CreateRenderTarget([NotNull] PixelFormat format, ulong offset, uint stride)
        {
            AssertNotDisposed();
            return new Implementation.TypelessBufferAsRenderTarget(this, format, offset, stride);
        }


        /// <summary>
        /// Creates the texture.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public TextureView CreateTexture([NotNull] PixelFormat format)
        {
            return CreateTexture(format, 0, format.Size);
        }

        /// <summary>
        /// Creates the texture.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="offset">The offset of first element.</param>
        /// <param name="stride">The stride (element width).</param>
        /// <returns></returns>
        public TextureView CreateTexture([NotNull] PixelFormat format, ulong offset, uint stride)
        {
            AssertNotDisposed();
            return new Implementation.TypelessBufferAsTexture(format, offset, stride, this);
        }

        /// <summary>
        /// Creates an index buffer.
        /// </summary>
        /// <param name="format">The format.</param>
        public IndexBufferView CreateIndexBuffer([NotNull] IndexFormat format)
        {
            return CreateIndexBuffer(format, 0);
        }

        /// <summary>
        /// Creates an index buffer.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="offset">The offset.</param>
        public IndexBufferView CreateIndexBuffer([NotNull] IndexFormat format, ulong offset)
        {
            return new IndexBufferView(this, offset, format);
        }


        /// <summary>
        /// Creates the vertex buffer.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="stride">The stride.</param>
        /// <returns></returns>
        public VertexBufferView CreateVertexBuffer(VertexFormat format)
        {
            return CreateVertexBuffer(format, 0, format.ByteSize, UpdateFrequency.PerVertex, 1);
        }

        /// <summary>
        /// Creates the vertex buffer.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="stride">The stride.</param>
        /// <param name="updateFrequency">The update frequency.</param>
        /// <param name="updateFrequencyCount">Number of vertices/instances before element changes.</param>
        /// <returns></returns>
        public VertexBufferView CreateVertexBuffer([NotNull] VertexFormat format, ulong offset, uint stride, 
            UpdateFrequency updateFrequency, uint updateFrequencyCount)
        {
            if (offset >= byteSize) throw new IndexOutOfRangeException("Trying to access index out of range.");
            if (stride < format.ByteSize) throw new ArgumentException("The stride cannot be smaller than the format size.");

            return new VertexBufferView(this, format, offset, stride, updateFrequency, updateFrequencyCount);
        }

        /// <summary>
        /// Creates a constant buffer.
        /// </summary>
        /// <remarks>
        /// Cannot be a property because creation affects the buffer and will not
        /// allow to release it if is created as property.
        /// </remarks>
        public ConstantBufferView CreateConstantBuffer(ConstantBufferLayout layout)
        {
            return new ConstantBufferView(this, layout);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Called when one of views is disposed.
        /// </summary>
        internal void Release()
        {
            viewCounter--;
            if (viewCounter == 0 && disposeOnViewDispose)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Called when view is created.
        /// </summary>
        internal void AddRef()
        {
            viewCounter++;
        }

        /// <summary>
        /// Resource views signal on buffer change made by GPU.
        /// </summary>
        internal void SignalChanged()
        {
            swOutOfDate = true;

            // We raise written event.
            Action<IResource> t = onWritten;
            if (t != null)
            {
                t(this);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// A debug description of buffer.
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Buffer[");
            builder.Append(byteSize);
            builder.Append("]{ Usage = ");
            builder.Append(usage.ToString());
            builder.Append(", BufferUsage = ");
            builder.Append(bufferUsage.ToString());
            builder.Append(", Locality = ");
            builder.Append(locality.ToString());
            builder.Append(", CPUAccess = ");
            builder.Append(locality.ToString());
            return builder.ToString();
        }
        
        #endregion

        #region IResource Members

        public ResourceAddress Address
        {
            get 
            { 
                Placement placement = Placement.NoPlacement;
                if(swData != null && !swOutOfDate) placement |= Placement.Memory;
                if(driverPart != null) placement |= Placement.DeviceMemory;
                return new ResourceAddress(placement);
            }
        }

        public event Action<IResource> Disposed
        {
            add
            {
                lock (syncRoot)
                {
                    onDisposed += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onDisposed -= value;
                }
            }
        }

        public bool IsDisposed
        {
            get { return cacheableState == CacheableState.Disposed; }
        }

        public event Action<IResource> OnWritten
        {
            add
            {
                lock (syncRoot)
                {
                    onWritten += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onWritten -= value;
                }
            }
        }

        #endregion

        #region ICacheable Members

        public void Cached()
        {
            cacheableState = CacheableState.Normal;
        }

        public void Evict()
        {
            cacheableState = CacheableState.Evicted;
        }

        public event Action<SharpMedia.Caching.ICacheable> OnTouch
        {
            add
            {
                lock (syncRoot)
                {
                    onTouch += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onTouch -= value;
                }
            }
        }

        public SharpMedia.Caching.CacheableState State
        {
            get { return cacheableState; }
        }

        public void Touch()
        {

            Action<ICacheable> t = onTouch;
            if (t != null)
            {
                t(this);
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
                return locality;
            }
            set
            {
                // Nothing to do.
                if (locality == value) return;

                lock (syncRoot)
                {
                    switch (value)
                    {
                        case GraphicsLocality.SystemMemoryOnly:
                            
                            break;
                        case GraphicsLocality.DeviceMemoryOnly:
                            break;
                        case GraphicsLocality.DeviceAndSystemMemory:
                            break;
                        case GraphicsLocality.DeviceOrSystemMemory:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void BindToDevice(GraphicsDevice device)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                AssertNotLocked();

                if (locality == GraphicsLocality.SystemMemoryOnly)
                {
                    throw new InvalidOperationException("System memory buffer cannot be bound to device.");
                }

                if (driverPart == null)
                {
                    this.device = device;
                    driverPart = device.DriverDevice.CreateBuffer(bufferUsage,
                        usage, cpuAccess, byteSize, swData);

                    if (locality == GraphicsLocality.DeviceOrSystemMemory ||
                       locality == GraphicsLocality.DeviceMemoryOnly)
                    {
                        // Release RAM copy.
                        swData = null;
                    }
                }
            }
        }

        public void UnBindFromDevice()
        {
            lock (syncRoot)
            {
                if (locality == GraphicsLocality.DeviceMemoryOnly)
                {
                    throw new InvalidOperationException("Cannot unbind device-memory only buffer from device.");
                }

                AssertNotDisposed();
                AssertLocked();

                // We first copy to RAM if approperiate.
                FillBuffer();

                // We release driver.
                ReleaseDriverPart();

            }
        }

        public bool IsBoundToDevice
        {
            get { return driverPart != null; }
        }

        #endregion

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region Mapping Members

        /// <summary>
        /// Maps the data to array.
        /// </summary>
        /// <param name="op">The map options.</param>
        /// <param name="off">Offset of beginning of lock.</param>
        /// <param name="count">The lock range.</param>
        /// <returns>Locked data.</returns>
        public byte[] Map(MapOptions op, ulong off, ulong count)
        {
            
            Monitor.Enter(syncRoot);

            try
            {
                // Validation.
                AssertNotDisposed();
                AssertNotLocked();

                if (off + count > byteSize)
                {
                    throw new IndexOutOfRangeException("Mapping array out of range.");
                }

                // We sync software part if necessary.
                if (swData != null && swOutOfDate == true)
                {
                    swData = GetInternalData();
                    swOutOfDate = false;
                }

                lockOptions = op;
                lockOffset = off;
                lockCount = count;


                // We perform direct if data is accessible.
                if (swData != null)
                {
                    if (off == 0 && count == byteSize)
                    {
                        lockedData = swData;
                    }
                    else
                    {
                        lockedData = new byte[lockCount];

                        // Write only means no need to copy.
                        if(op != MapOptions.Write)
                        {
                            for (ulong i = 0; i < count; i++)
                            {
                                lockedData[i + off] = swData[i];
                            }
                        }
                    }
                }
                else
                {
                    if (op == MapOptions.Write)
                    {
                        lockedData = new byte[lockCount];
                    }
                    else
                    {
                        lockedData = driverPart.Read(off, count);
                    }
                }

                return lockedData;
            }
            catch (Exception)
            {
                Monitor.Exit(syncRoot);
                throw;
            }
        }

        /// <summary>
        /// Is the buffer mapped currently.
        /// </summary>
        /// <remarks>This is not thread safe.</remarks>
        public bool IsMapped
        {
            get
            {
                return lockedData != null;
            }
        }

        /// <summary>
        /// Returns mapped data.
        /// </summary>
        public byte[] MappedData
        {
            get
            {
                return lockedData;
            }
        }

        /// <summary>
        /// Mapping offset, valid only if IsMapped is true.
        /// </summary>
        public ulong MapOffset
        {
            get
            {
                return lockOffset;
            }
        }

        /// <summary>
        /// Mapping byte count, valid only if IsMapped is true.
        /// </summary>
        public ulong MapByteCount
        {
            get
            {
                return lockCount;
            }
        }

        /// <summary>
        /// Mapping options, valid only if IsMapped is true.
        /// </summary>
        public MapOptions MapOptions
        {
            get
            {
                return lockOptions;
            }
        }

        #endregion

        #region IMapable<byte[]> Members

        public byte[] Map(MapOptions op)
        {
            return Map(op, 0, byteSize);
        }

        public void UnMap()
        {
            // Validation.
            AssertNotDisposed();
            AssertLocked();

            try
            {
                // No need to update.
                if (lockOptions == MapOptions.Read) return;

                // We update swData if it exists and lockedData wasn't it already.
                if (swData != null && (lockOffset != 0 || lockCount != byteSize))
                {
                    for (ulong i = 0; i < lockCount; i++)
                    {
                        swData[i] = lockedData[i + lockOffset];
                    }
                }

                // We also update driver part.
                if (driverPart != null)
                {
                    driverPart.Update(lockedData, lockOffset, lockCount);
                }

            }
            finally
            {
                // Reset lock data.
                lockedData = null;
                Monitor.Exit(syncRoot);
            }

            // We raise written event.
            Action<IResource> t = onWritten;
            if (t != null)
            {
                t(this);
            }
        }

        #endregion
    }
}
