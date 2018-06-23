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
using SharpMedia.Math.Shapes;
using SharpMedia.Math.Shapes.Compounds;
using SharpMedia.Graphics.Driver;
using System.Threading;
using SharpMedia.AspectOriented;
using System.Runtime.InteropServices;
using SharpMedia.Math;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A geometry consists of vertex buffers and possibly index buffer with information
    /// how to bind them.
    /// </summary>
    [Serializable]
    public class Geometry : IEnumerable<VertexBufferView>, IGraphicsLocality, IDisposable
    {
        #region Private Members
        protected object syncRoot = new object();

        // Data.
        protected Topology topology = Topology.Triangle;
        protected IndexBufferView indexBuffer;
        protected List<VertexBufferView> vertexBuffers = new List<VertexBufferView>();

        // Control.
        protected bool isDisposed = false;
        protected bool isLocked = false;
        protected uint usedByDevice = 0;
        protected bool associateBuffers = false;

        // Events.

        // Layout.
        protected Driver.IVerticesBindingLayout layout;
        protected Driver.IVerticesOutBindingLayout outLayout;

        #endregion

        #region Private Methods


        protected void AssertNotDisposed()
        {
            if (isDisposed) throw new ObjectDisposedException("Geometry already disposed.");
        }

        protected void AssertNotLocked()
        {
            if (isLocked) throw new InvalidOperationException("The geometry is already locked.");
        }

        protected void AssertBatched()
        {
            if (!isLocked) throw new InvalidOperationException("The geometry must be batched in order to allow this call.");
        }

        void ClearCache()
        {
            if (layout != null)
            {
                layout.Dispose();
                layout = null;
            }

            if (outLayout != null)
            {
                outLayout.Dispose();
                outLayout = null;
            }
        }

        void Dispose(bool fin)
        {
            
            AssertNotLocked();

            ClearCache();

            if (!isDisposed && associateBuffers)
            {
                foreach (VertexBufferView view in vertexBuffers)
                {
                    view.Dispose();
                }
                if (indexBuffer != null) indexBuffer.Dispose();
            }

            if (!isDisposed)
            {
                if (!fin)
                {
                    GC.SuppressFinalize(this);
                }
                else
                {
                    // Make sure we don't hold references.
                    indexBuffer = null;
                    vertexBuffers.Clear();
                }
            }

            isDisposed = true;


            
        }

        ~Geometry()
        {
            Dispose(true);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the layout.
        /// </summary>
        /// <value>The layout.</value>
        internal Driver.IVerticesBindingLayout Layout
        {
            get { return layout; }
        }

        /// <summary>
        /// Gets the out layout.
        /// </summary>
        /// <value>The out layout.</value>
        internal Driver.IVerticesOutBindingLayout OutLayout
        {
            get { return outLayout; }
        }

        internal void UsedByDevice()
        {
            usedByDevice++;
            if (usedByDevice == 1)
            {
                Monitor.Enter(syncRoot);
            }

            if (indexBuffer != null) indexBuffer.UsedByDevice();
            for (int i = 0; i < vertexBuffers.Count; i++)
            {
                vertexBuffers[i].UsedByDevice();
            }
        }

        internal void UnusedByDevice()
        {
            if (--usedByDevice == 0)
            {
                Monitor.Exit(syncRoot);
            }

            if (indexBuffer != null) indexBuffer.UnusedByDevice();
            for (int i = 0; i < vertexBuffers.Count; i++)
            {
                vertexBuffers[i].UnusedByDevice();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// A custom geometry construction.
        /// </summary>
        public Geometry()
        {
        }

        /// <summary>
        /// A custom geometry construction.
        /// </summary>
        public Geometry(GraphicsDevice device)
        {
        }

        /// <summary>
        /// Creates dynamic buffers that are are update (usually each frame).
        /// </summary>
        /// <param name="device"></param>
        /// <param name="vertexFormat"></param>
        /// <param name="indexFormat"></param>
        /// <param name="maxVertices"></param>
        /// <param name="maxIndices"></param>
        /// <returns></returns>
        public static GeometryBatch CreateBatch(VertexFormat vertexFormat, IndexFormat indexFormat,
                                                ulong maxVertices, ulong maxIndices, uint maxCyclicBuffers)
        {
            if (maxCyclicBuffers == 0) maxCyclicBuffers = 1;

            VertexBufferView[] vbufferView = new VertexBufferView[maxCyclicBuffers];
            IndexBufferView[] ibufferView = new IndexBufferView[maxCyclicBuffers];

            for (uint i = 0; i < maxCyclicBuffers; i++)
            {
                // We create vertex buffer.
                TypelessBuffer vbuffer = new TypelessBuffer(Usage.Dynamic, BufferUsage.VertexBuffer, CPUAccess.Write,
                GraphicsLocality.DeviceOrSystemMemory, vertexFormat.ByteSize * maxVertices);
                vbuffer.DisposeOnViewDispose = true;

                // We create view.
                vbufferView[i] = vbuffer.CreateVertexBuffer(vertexFormat);

                // We may also create index buffer.
                if (maxIndices > 0)
                {
                    TypelessBuffer ibuffer = new TypelessBuffer(Usage.Dynamic, BufferUsage.IndexBuffer, CPUAccess.Write,
                        GraphicsLocality.DeviceOrSystemMemory, indexFormat.ByteSize * maxIndices);
                    ibuffer.DisposeOnViewDispose = true;

                    // We create view.
                    ibufferView[i] = ibuffer.CreateIndexBuffer(indexFormat);
                }
            }

            // We create geometry.
            GeometryBatch geometry = new GeometryBatch(vbufferView, ibufferView);

            return geometry;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// A topology of geometry.
        /// </summary>
        public Topology Topology
        {
            get
            {
                return topology;
            }
            set
            {
                topology = value;
            }
        }

        /// <summary>
        /// On Dispose, buffer views are also disposed.
        /// </summary>
        public bool AssociateBuffers
        {
            get
            {
                return associateBuffers;
            }
            set
            {
                associateBuffers = value;
            }
        }

        /// <summary>
        /// An index buffer property.
        /// </summary>
        public IndexBufferView IndexBuffer
        {
            get
            {
                return indexBuffer;
            }
            set
            {
                indexBuffer = value;
            }
        }

        /// <summary>
        /// Binding of typed vertex buffers.
        /// </summary>
        /// <param name="index">The index of buffer.</param>
        public VertexBufferView this[uint index]
        {
            get
            {
                lock(syncRoot)
                {
                    return vertexBuffers.Count >= index ? vertexBuffers[(int)index] : null;
                }
            }
            set
            {
                lock(syncRoot)
                {
                    AssertNotDisposed();
                    AssertNotLocked();

                    if (value == null) throw new ArgumentException("Cannot set a null buffer.");

                    if ((uint)vertexBuffers.Count == index)
                    {
                        vertexBuffers.Add(value);
                        ClearCache();
                    }
                    else if ((uint)vertexBuffers.Count > index)
                    {
                        vertexBuffers[(int)index] = value;
                    }
                    else
                    {
                        throw new InvalidOperationException("Vertex buffers must be added sequentially.");
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is output compatible (can be bound as output).
        /// </summary>
        public bool IsOutputCompatible
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the vertex buffer count.
        /// </summary>
        /// <value>The vertex buffer count.</value>
        public uint VertexBufferCount
        {
            get
            {
                return (uint)vertexBuffers.Count;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether referencing in range.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public bool IsInRange(ulong offset, ulong count)
        {
            return true;
        }

        /// <summary>
        /// Determines whether referencing in range.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The count.</param>
        /// <param name="baseIndex">The base index, usually 0.</param>
        public bool IsInRange(ulong offset, ulong length, long baseIndex)
        {
            return true;
        }

        /// <summary>
        /// Determines whether referencing in range.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <param name="instanceOffset">The instance offset.</param>
        /// <param name="instanceCount">The instance count.</param>
        public bool IsInRange(ulong offset, ulong count,
                  uint instanceOffset, uint instanceCount)
        {
            return true;
        }

        /// <summary>
        /// Determines whether referencing in range.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <param name="baseIndex">Index of the base.</param>
        /// <param name="instanceOffset">The instance offset.</param>
        /// <param name="instanceCount">The instance count.</param>
        public bool IsInRange(ulong offset, ulong count, long baseIndex,
                  uint instanceOffset, uint instanceCount)
        {
            return true;
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
                return GraphicsLocality.DeviceAndSystemMemory;
            }
            set
            {
                if (value != GraphicsLocality.DeviceAndSystemMemory)
                {
                    throw new InvalidOperationException("The locality of Geometry resource is always DeviceAndSystemMemory.");
                }
            }
        }

        public void BindToDevice(GraphicsDevice device)
        {
            lock (syncRoot)
            {
                for (int i = 0; i < vertexBuffers.Count; i++)
                {
                    vertexBuffers[i].BindToDevice(device);
                }

                if (indexBuffer != null)
                {
                    indexBuffer.BindToDevice(device);
                }
            }
        }

        /// <summary>
        /// Binds input layout to device.
        /// </summary>
        /// <param name="device"></param>
        public void BindInputLayout([NotNull] GraphicsDevice device)
        {
            lock (syncRoot)
            {
                if (layout == null)
                {
                    VertexBindingElement[] elements = new VertexBindingElement[vertexBuffers.Count];

                    for (int i = 0; i < vertexBuffers.Count; i++)
                    {
                        VertexBindingElement element = new VertexBindingElement();
                        element.Format = vertexBuffers[i].Format;
                        element.UpdateFrequency = vertexBuffers[i].UpdateFrequency;
                        element.UpdateFrequencyCount = vertexBuffers[i].UpdateFrequencyCount;

                        // Copy the element to array.
                        elements[i] = element;
                    }

                    layout = device.DriverDevice.CreateVertexBinding(elements);

                }
            }
        }


        /// <summary>
        /// Binds output layout to device.
        /// </summary>
        /// <param name="device"></param>
        public void BindOutputLayout([NotNull] GraphicsDevice device)
        {
            lock (syncRoot)
            {
                if (outLayout == null)
                {
                    throw new NotImplementedException();

                }
            }
        }

        public void UnBindFromDevice()
        {
            lock (syncRoot)
            {
                // We just clear cache.
                ClearCache();
            }
        }

        public bool IsBoundToDevice
        {
            get { return layout != null || outLayout != null; }
        }

        #endregion

        #region IEnumerable<VertexBuffer> Members

        public IEnumerator<VertexBufferView> GetEnumerator()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                AssertNotLocked();

                for (int i = 0; i < vertexBuffers.Count; i++)
                {
                    yield return vertexBuffers[i];
                }
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                AssertNotLocked();

                for (int i = 0; i < vertexBuffers.Count; i++)
                {
                    yield return vertexBuffers[i];
                }
            }
        }

        #endregion

    }
}
