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
using System.Runtime.InteropServices;
using SharpMedia.Math;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// Geometry batch, allows adding data to to buffer dynamically.
    /// </summary>
    /// <remarks>Use Geometry.CreateBatch to construct batch.</remarks>
    public sealed class GeometryBatch : Geometry
    {
        #region Private Members


        // Batching.
        uint bufferIndex = 0;
        ulong vertexIndex = 0;
        ulong indexIndex = 0;
        byte[] batchIndexData;
        byte[] batchVertexData;

        // Buffers.
        VertexBufferView[] vbuffers;
        IndexBufferView[] ibuffers;
        #endregion

        #region Constructors

        internal GeometryBatch(VertexBufferView[] vbuffers, IndexBufferView[] ibuffers)
        {
            this.vbuffers = vbuffers;
            this.ibuffers = ibuffers;
            this.bufferIndex = (uint)vbuffers.Length;
        }

        #endregion


        #region Batch Methods


        /// <summary>
        /// Begins writing to batch.
        /// </summary>
        /// <remarks>The vertex buffer can change when this is called (cyclic buffers optimization).</remarks>
        public void BeginBatch()
        {
            AssertNotDisposed();
            AssertNotLocked();

            // We advance index.
            bufferIndex = bufferIndex + 1 >= vbuffers.Length ? 0 : bufferIndex + 1;

            // We set current buffer.
            this[0] = vbuffers[bufferIndex];
            this.IndexBuffer = ibuffers[bufferIndex];
            

            try
            {
                Monitor.Enter(syncRoot);

                isLocked = true;

                vertexIndex = 0;

                // We map the buffer.
                batchVertexData = vertexBuffers[0].TypelessBuffer.Map(MapOptions.Write);

                indexIndex = 0;
                if (indexBuffer != null)
                {
                    batchIndexData = indexBuffer.TypelessBuffer.Map(MapOptions.Write);
                }

            }
            catch (Exception)
            {
                isLocked = false;
                Monitor.Exit(syncRoot);
                throw;
            }


        }

        /// <summary>
        /// Ends writing to batch.
        /// </summary>
        public void EndBatch()
        {
            AssertBatched();

            batchIndexData = batchVertexData = null;

            vertexBuffers[0].TypelessBuffer.UnMap();
            if (indexBuffer != null)
            {
                indexBuffer.TypelessBuffer.UnMap();
            }

            isLocked = false;
            Monitor.Exit(syncRoot);
        }

        /// <summary>
        /// Adds user defined struct with the same layout as specified with VertexFormat.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <remarks>Must be called between begin/end batch. It will also work for non-batch
        /// generated geometry (constains other vertex buffers),
        /// but will only write to primary (first) buffer.</remarks>
        public unsafe ulong AddVertices<T>([NotNull] T[] data) where T : struct
        {
            AssertBatched();

            // We check the number of vertices we can copy.
            ulong structSize = (ulong)Marshal.SizeOf(typeof(T));

            // Validate T against VertexFormat.
            if (structSize != vertexBuffers[0].Format.ByteSize)
            {
                throw new InvalidOperationException("The vertex format does not match structure, cannot add.");
            }

            // We compute number of vertices to add.
            ulong dataToAdd = MathHelper.Min((ulong)batchVertexData.LongLength / structSize - vertexIndex,
                                             (ulong)data.LongLength);

            fixed (byte* dst = this.batchVertexData)
            {
                Common.Memcpy(data, dst + vertexIndex * structSize, structSize * dataToAdd);
            }

            vertexIndex += dataToAdd;

            return dataToAdd;
        }

        /// <summary>
        /// Adds user defined struct with the same layout as specified with VertexFormat.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <remarks>Must be called between begin/end batch. It will also work for non-batch
        /// generated geometry (constains other vertex buffers),
        /// but will only write to primary (first) buffer.</remarks>
        public ulong AddVertices<T>([NotNull] IEnumerable<T> data)
        {
            AssertBatched();

            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds ushort indices.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public unsafe ulong AddIndices([NotNull] ushort[] data)
        {
            AssertBatched();

            // Checks 
            if (batchIndexData == null)
            {
                throw new InvalidOperationException("Cannot add indices to non-indeaxable geometry.");
            }

            if (!indexBuffer.Format.IsShort)
            {
                throw new ArgumentException("Invalid indices format.");
            }

            // We compute number of vertices to add.
            ulong dataToAdd = MathHelper.Min((ulong)batchIndexData.LongLength / 2 - indexIndex,
                                             (ulong)data.LongLength);


            fixed (byte* dst = batchIndexData)
            {
                Common.Memcpy(data, dst + indexIndex * 2, dataToAdd * 2);
            }

            indexIndex += dataToAdd;

            return dataToAdd;
        }

        /// <summary>
        /// Adds uint indices.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public unsafe ulong AddIndices([NotNull] uint[] data)
        {
            AssertBatched();

            // Checks 
            if (batchIndexData == null)
            {
                throw new InvalidOperationException("Cannot add indices to non-indeaxable geometry.");
            }

            if (!indexBuffer.Format.IsWide)
            {
                throw new ArgumentException("Invalid indices format.");
            }

            // We compute number of vertices to add.
            ulong dataToAdd = MathHelper.Min((ulong)batchIndexData.LongLength / 4 - indexIndex,
                                             (ulong)data.LongLength);


            fixed (byte* dst = batchIndexData)
            {
                Common.Memcpy(data, dst + indexIndex * 4, dataToAdd * 4);
            }

            indexIndex += dataToAdd;

            return dataToAdd;
        }



        #endregion

        #region Properties

        /// <summary>
        /// Number of vertices.
        /// </summary>
        /// <remarks>The result is valid until batch.BeginBatch() is called (count is reset to 0 at that moment).</remarks>
        public ulong VertexCount
        {
            get
            {
                return vertexIndex;
            }
        }

        /// <summary>
        /// Number of indices.
        /// </summary>
        /// <remarks>The result is valid until batch.BeginBatch() is called (count is reset to 0 at that moment).</remarks>
        public ulong IndexCount
        {
            get
            {
                return indexIndex;
            }
        }

        #endregion

    }
}
