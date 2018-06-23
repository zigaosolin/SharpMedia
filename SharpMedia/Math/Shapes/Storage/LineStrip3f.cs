// This file was generated by TemplateEngine from template source 'LineStrip'
// using template 'LineStrip3f. Do not modify this file directly, modify it from template source.

using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.AspectOriented;
using SharpMedia.Math.Shapes.Storage.Builders;

namespace SharpMedia.Math.Shapes.Storage
{

    /// <summary>
    /// Represents a collection of lines in strip format, in native format.
    /// </summary>
    /// <remarks>This is processing oriented format with 'builder and accessor' in one.
    /// </remarks>
    [Serializable]
    public class LineStrip3f : ILineAccess3f, ILineBuilder3f
    {
        #region Private Members
        List<uint> startIndices = new List<uint>();
        List<Vector3f[]> strips = new List<Vector3f[]>();
        #endregion

        #region Private Methods

        private Vector3f[] Find(uint index, out uint inIndex)
        {
            // We perform manual binary search.
            int min = 0, max = startIndices.Count - 1;

            // Special kind last.
            if(startIndices[max] < index) 
            {
                inIndex = index - startIndices[max];
                return strips[max];
            }

            while(true)
            {
                int middle = (min + max) / 2;
                if(middle == min) break;

                if (startIndices[middle] > index)
                {
                    max = middle;
                }
                else
                {
                    min = middle;
                }
            }

            inIndex = index - startIndices[min];
            return strips[min];
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LineStrip3f()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// A list of vertices.
        /// </summary>
        /// <remarks>Appending/removing vertices is dangerous.</remarks>
        public List<Vector3f[]> LineStrips
        {
            get { return strips; }
        }


        /// <summary>
        /// Start indices.
        /// </summary>
        public List<uint> StartIndices
        {
            get { return startIndices; }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Number of shapes.
        /// </summary>
        public uint ShapeCount
        {
            get
            {
                if (startIndices.Count == 0) return 0;
                return startIndices[startIndices.Count - 1] + (uint)strips[strips.Count - 1].Length;
            }
        }

        #endregion

        #region Line Access

        /// <summary>
        /// Obtains a line at index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A trinagle at index.</returns>
        public LineSegment3f Get(uint index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills a line at index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="line">The triangle to be filled.</param>
        public void Get(uint index, [NotNull] LineSegment3f line)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains a collection of lines.
        /// </summary>
        /// <param name="index">The base index.</param>
        /// <param name="count">Number of triangles.</param>
        /// <returns>The triangle collection.</returns>
        public LineSegment3f[] Get(uint index, uint count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains lines.
        /// </summary>
        /// <param name="index">The triangle index.</param>
        /// <param name="lines">Data to be filled.</param>
        public void Get(uint index, LineSegment3f[] lines)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ILineBuilder3f Members

        public bool IsIndexed 
        { 
            get 
            {
                return false;
            } 
        }

        public uint AddControlPoints(params Vector3f[] data)
        {
            if (data.Length % 2 != 0) throw new ArgumentException("Expected data length that is multiple of two (two vertices per line).");

            // This is not colled preferrable.
            for (int i = 0; i < data.Length; i+=2)
            {
                Vector3f[] line = new Vector3f[2];
                line[0] = data[i];
                line[1] = data[i + 1];

                strips.Add(line);
            }

            // Cannot be used anyways, but fixme later.
            return 0;
        }


        public void AddIndexedLines(params uint[] indices)
        {
            throw new InvalidOperationException();
        }


        public void AddLineStrip(bool linkFirstToLast, params Vector3f[] data)
        {
            if (data.Length == 0) throw new ArgumentException("The data must be non-null.");

            Vector3f[] list = new Vector3f[linkFirstToLast ? data.Length + 1 : data.Length];

            // We copy data.
            data.CopyTo(list, 0);

            if (linkFirstToLast)
            {
                list[list.Length - 1] = data[0];
            }

            strips.Add(list);
        }

        #endregion

    }
}