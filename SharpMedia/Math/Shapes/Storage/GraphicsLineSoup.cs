// This file was generated by TemplateEngine from template source 'GraphicsLineSoup'
// using template 'GraphicsLineSoup. Do not modify this file directly, modify it from template source.

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Math.Shapes.Storage
{

    /// <summary>
    /// Represents a collection of lines, in graphics's compatible format.
    /// </summary>
    /// <remarks>This is generic storage oriented format. You can use mesh version's for
    /// processing oriented formats (located in compounds).
    /// </remarks>
    [Serializable]
    public class GraphicsLineSoup : BaseTopologySet, 
        ILineAccess2f, ILineAccess3f, ILineAccess2d, ILineAccess3d
    {
        #region Overrides

        public override uint ControlPointsPerShape
        {
            get
            {
                return 2;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// The line soup constructor.
        /// </summary>
        /// <remarks>Use the builder class to obtain "validated" versions.</remarks>
        /// <param name="query"></param>
        /// <param name="indexBuffer"></param>
        /// <param name="shapeCount"></param>
        public GraphicsLineSoup(ControlPointQuery query, ShapeIndexBufferView indexBuffer, uint shapeCount)
        {
            this.query = query;
            this.indexBuffer = indexBuffer;
            this.shapeCount = shapeCount;
        }

        #endregion

        #region Query Members

        
		//#foreach instanced to 'Double2'


        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        /// <returns></returns>
        public LineSegment2d Get2d(uint index)
        {
            LineSegment2d t = new LineSegment2d();
            Get(CommonComponents.Position, index, t);
            return t;
        }

        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// correct format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public LineSegment2d Get2d(string positionComponent, uint index)
        {
            LineSegment2d t = new LineSegment2d();
            Get(positionComponent, index, t);
            return t;
        }

        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// Vector2f format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public void Get(uint index, LineSegment2d storage)
        {
            Get(CommonComponents.Position, index, storage);
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <remarks>This accessor is not to be used in performance critical parts of applications.</remarks>
        /// <returns></returns>
        public void Get(string positionComponent, uint index, LineSegment2d storage)
        {
            if (index >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(index * 2, 2);

                storage.A = query.Get2d(positionComponent, indices[0]);
                storage.B = query.Get2d(positionComponent, indices[1]);
            }
            else
            {
                storage.A = query.Get2d(positionComponent, index * 2);
                storage.B = query.Get2d(positionComponent, index * 2 + 1);
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The  line index offset.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(uint lineIndex, LineSegment2d[] storage)
        {
            Get(CommonComponents.Position, lineIndex, storage);
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The  line index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(string positionComponent, uint lineIndex, LineSegment2d[] storage)
        {
            if (lineIndex + storage.Length >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(lineIndex * 2, (uint)storage.Length * 2);
                Vector2d[] data = query.Get2d(positionComponent, indices);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new LineSegment2d(data[i * 2], data[i * 2 + 1]);
                }
            }
            else
            {
                Vector2d[] data = query.Get2d(positionComponent, lineIndex * 2, (uint)storage.Length * 2);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new LineSegment2d(data[i * 2], data[i * 2 + 1]);
                }
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The line index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="count">Number of triangles.</param>
        public LineSegment2d[] Get2d(string positionComponent, uint lineIndex, uint count)
        {
            LineSegment2d[] data = new LineSegment2d[count];
            Get(positionComponent, lineIndex, data);
            return data;
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The line index offset.</param>
        /// <param name="count">Number of triangles.</param>
        public LineSegment2d[] Get2d(uint lineIndex, uint count)
        {
            return Get2d(CommonComponents.Position, lineIndex, count);
        }

        //#endfor instanced to 'Double2'

		//#foreach instanced to 'Double3'


        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        /// <returns></returns>
        public LineSegment3d Get3d(uint index)
        {
            LineSegment3d t = new LineSegment3d();
            Get(CommonComponents.Position, index, t);
            return t;
        }

        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// correct format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public LineSegment3d Get3d(string positionComponent, uint index)
        {
            LineSegment3d t = new LineSegment3d();
            Get(positionComponent, index, t);
            return t;
        }

        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// Vector2f format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public void Get(uint index, LineSegment3d storage)
        {
            Get(CommonComponents.Position, index, storage);
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <remarks>This accessor is not to be used in performance critical parts of applications.</remarks>
        /// <returns></returns>
        public void Get(string positionComponent, uint index, LineSegment3d storage)
        {
            if (index >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(index * 2, 2);

                storage.A = query.Get3d(positionComponent, indices[0]);
                storage.B = query.Get3d(positionComponent, indices[1]);
            }
            else
            {
                storage.A = query.Get3d(positionComponent, index * 2);
                storage.B = query.Get3d(positionComponent, index * 2 + 1);
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The  line index offset.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(uint lineIndex, LineSegment3d[] storage)
        {
            Get(CommonComponents.Position, lineIndex, storage);
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The  line index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(string positionComponent, uint lineIndex, LineSegment3d[] storage)
        {
            if (lineIndex + storage.Length >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(lineIndex * 2, (uint)storage.Length * 2);
                Vector3d[] data = query.Get3d(positionComponent, indices);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new LineSegment3d(data[i * 2], data[i * 2 + 1]);
                }
            }
            else
            {
                Vector3d[] data = query.Get3d(positionComponent, lineIndex * 2, (uint)storage.Length * 2);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new LineSegment3d(data[i * 2], data[i * 2 + 1]);
                }
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The line index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="count">Number of triangles.</param>
        public LineSegment3d[] Get3d(string positionComponent, uint lineIndex, uint count)
        {
            LineSegment3d[] data = new LineSegment3d[count];
            Get(positionComponent, lineIndex, data);
            return data;
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The line index offset.</param>
        /// <param name="count">Number of triangles.</param>
        public LineSegment3d[] Get3d(uint lineIndex, uint count)
        {
            return Get3d(CommonComponents.Position, lineIndex, count);
        }

        //#endfor instanced to 'Double3'

		//#foreach instanced to 'Float2'


        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        /// <returns></returns>
        public LineSegment2f Get2f(uint index)
        {
            LineSegment2f t = new LineSegment2f();
            Get(CommonComponents.Position, index, t);
            return t;
        }

        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// correct format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public LineSegment2f Get2f(string positionComponent, uint index)
        {
            LineSegment2f t = new LineSegment2f();
            Get(positionComponent, index, t);
            return t;
        }

        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// Vector2f format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public void Get(uint index, LineSegment2f storage)
        {
            Get(CommonComponents.Position, index, storage);
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <remarks>This accessor is not to be used in performance critical parts of applications.</remarks>
        /// <returns></returns>
        public void Get(string positionComponent, uint index, LineSegment2f storage)
        {
            if (index >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(index * 2, 2);

                storage.A = query.Get2f(positionComponent, indices[0]);
                storage.B = query.Get2f(positionComponent, indices[1]);
            }
            else
            {
                storage.A = query.Get2f(positionComponent, index * 2);
                storage.B = query.Get2f(positionComponent, index * 2 + 1);
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The  line index offset.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(uint lineIndex, LineSegment2f[] storage)
        {
            Get(CommonComponents.Position, lineIndex, storage);
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The  line index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(string positionComponent, uint lineIndex, LineSegment2f[] storage)
        {
            if (lineIndex + storage.Length >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(lineIndex * 2, (uint)storage.Length * 2);
                Vector2f[] data = query.Get2f(positionComponent, indices);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new LineSegment2f(data[i * 2], data[i * 2 + 1]);
                }
            }
            else
            {
                Vector2f[] data = query.Get2f(positionComponent, lineIndex * 2, (uint)storage.Length * 2);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new LineSegment2f(data[i * 2], data[i * 2 + 1]);
                }
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The line index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="count">Number of triangles.</param>
        public LineSegment2f[] Get2f(string positionComponent, uint lineIndex, uint count)
        {
            LineSegment2f[] data = new LineSegment2f[count];
            Get(positionComponent, lineIndex, data);
            return data;
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The line index offset.</param>
        /// <param name="count">Number of triangles.</param>
        public LineSegment2f[] Get2f(uint lineIndex, uint count)
        {
            return Get2f(CommonComponents.Position, lineIndex, count);
        }

        //#endfor instanced to 'Float2'

		//#foreach instanced to 'Float3'


        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        /// <returns></returns>
        public LineSegment3f Get3f(uint index)
        {
            LineSegment3f t = new LineSegment3f();
            Get(CommonComponents.Position, index, t);
            return t;
        }

        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// correct format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public LineSegment3f Get3f(string positionComponent, uint index)
        {
            LineSegment3f t = new LineSegment3f();
            Get(positionComponent, index, t);
            return t;
        }

        /// <summary>
        /// Gets line.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// Vector2f format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public void Get(uint index, LineSegment3f storage)
        {
            Get(CommonComponents.Position, index, storage);
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <remarks>This accessor is not to be used in performance critical parts of applications.</remarks>
        /// <returns></returns>
        public void Get(string positionComponent, uint index, LineSegment3f storage)
        {
            if (index >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(index * 2, 2);

                storage.A = query.Get3f(positionComponent, indices[0]);
                storage.B = query.Get3f(positionComponent, indices[1]);
            }
            else
            {
                storage.A = query.Get3f(positionComponent, index * 2);
                storage.B = query.Get3f(positionComponent, index * 2 + 1);
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The  line index offset.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(uint lineIndex, LineSegment3f[] storage)
        {
            Get(CommonComponents.Position, lineIndex, storage);
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The  line index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(string positionComponent, uint lineIndex, LineSegment3f[] storage)
        {
            if (lineIndex + storage.Length >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(lineIndex * 2, (uint)storage.Length * 2);
                Vector3f[] data = query.Get3f(positionComponent, indices);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new LineSegment3f(data[i * 2], data[i * 2 + 1]);
                }
            }
            else
            {
                Vector3f[] data = query.Get3f(positionComponent, lineIndex * 2, (uint)storage.Length * 2);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new LineSegment3f(data[i * 2], data[i * 2 + 1]);
                }
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The line index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="count">Number of triangles.</param>
        public LineSegment3f[] Get3f(string positionComponent, uint lineIndex, uint count)
        {
            LineSegment3f[] data = new LineSegment3f[count];
            Get(positionComponent, lineIndex, data);
            return data;
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="lineIndex">The line index offset.</param>
        /// <param name="count">Number of triangles.</param>
        public LineSegment3f[] Get3f(uint lineIndex, uint count)
        {
            return Get3f(CommonComponents.Position, lineIndex, count);
        }

        //#endfor instanced to 'Float3'



        #endregion
    }
}