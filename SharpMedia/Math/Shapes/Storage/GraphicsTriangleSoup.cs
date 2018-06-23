// This file was generated by TemplateEngine from template source 'GraphicsTriangleSoup'
// using template 'GraphicsTriangleSoup. Do not modify this file directly, modify it from template source.

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Math.Shapes.Storage
{

    /// <summary>
    /// Represents a collection of triangles, in graphics's compatible format.
    /// </summary>
    /// <remarks>This is generic storage oriented format. You can use mesh version's for
    /// processing oriented formats (located in compounds).
    /// </remarks>
    [Serializable]
    public class GraphicsTriangleSoup : BaseTopologySet,
        ITriangleAccess2f, ITriangleAccess3f, ITriangleAccess2d, ITriangleAccess3d
    {
        #region ITopologySet Members

        public override uint ControlPointsPerShape
        {
            get { return 2; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// The triangle soup constructor.
        /// </summary>
        /// <remarks>Use the builder class to obtain "validated" versions.</remarks>
        /// <param name="query"></param>
        /// <param name="indexBuffer"></param>
        /// <param name="shapeCount"></param>
        public GraphicsTriangleSoup(ControlPointQuery query, ShapeIndexBufferView indexBuffer, uint shapeCount)
        {
            this.query = query;
            this.indexBuffer = indexBuffer;
            this.shapeCount = shapeCount;
        }

        #endregion

        #region Query Members

        
		//#foreach instanced to 'Double2'


        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle triangle index.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        /// <returns></returns>
        public Triangle2d Get2d(uint index)
        {
            Triangle2d t = new Triangle2d();
            Get(CommonComponents.Position, index, t);
            return t;
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// correct format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public Triangle2d Get2d(string positionComponent, uint index)
        {
            Triangle2d t = new Triangle2d();
            Get(positionComponent, index, t);
            return t;
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// Vector2f format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public void Get(uint index, Triangle2d storage)
        {
            Get(CommonComponents.Position, index, storage);
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle triangle index.</param>
        /// <remarks>This accessor is not to be used in performance critical parts of applications.</remarks>
        /// <returns></returns>
        public void Get(string positionComponent, uint index, Triangle2d storage)
        {
            if (index >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(index * 3, 3);

                storage.A = query.Get2d(positionComponent, indices[0]);
                storage.B = query.Get2d(positionComponent, indices[1]);
                storage.C = query.Get2d(positionComponent, indices[2]);
            }
            else
            {
                storage.A = query.Get2d(positionComponent, index * 3);
                storage.B = query.Get2d(positionComponent, index * 3 + 1);
                storage.C = query.Get2d(positionComponent, index * 3 + 2);
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The  triangle index offset.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(uint triangleIndex, Triangle2d[] storage)
        {
            Get(CommonComponents.Position, triangleIndex, storage);
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The  triangle index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(string positionComponent, uint triangleIndex, Triangle2d[] storage)
        {
            if (triangleIndex + storage.Length >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(triangleIndex * 3, (uint)storage.Length * 3);
                Vector2d[] data = query.Get2d(positionComponent, indices);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new Triangle2d(data[i * 3], data[i * 3 + 1], data[i * 3 + 2]);
                }
            }
            else
            {
                Vector2d[] data = query.Get2d(positionComponent, triangleIndex * 3, (uint)storage.Length * 3);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new Triangle2d(data[i * 3], data[i * 3 + 1], data[i * 3 + 2]);
                }
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The triangle index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="count">Number of triangles.</param>
        public Triangle2d[] Get2d(string positionComponent, uint triangleIndex, uint count)
        {
            Triangle2d[] data = new Triangle2d[count];
            Get(positionComponent, triangleIndex, data);
            return data;
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The triangle index offset.</param>
        /// <param name="count">Number of triangles.</param>
        public Triangle2d[] Get2d(uint triangleIndex, uint count)
        {
            return Get2d(CommonComponents.Position, triangleIndex, count);
        }

        //#endfor instanced to 'Double2'

		//#foreach instanced to 'Double3'


        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle triangle index.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        /// <returns></returns>
        public Triangle3d Get3d(uint index)
        {
            Triangle3d t = new Triangle3d();
            Get(CommonComponents.Position, index, t);
            return t;
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// correct format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public Triangle3d Get3d(string positionComponent, uint index)
        {
            Triangle3d t = new Triangle3d();
            Get(positionComponent, index, t);
            return t;
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// Vector2f format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public void Get(uint index, Triangle3d storage)
        {
            Get(CommonComponents.Position, index, storage);
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle triangle index.</param>
        /// <remarks>This accessor is not to be used in performance critical parts of applications.</remarks>
        /// <returns></returns>
        public void Get(string positionComponent, uint index, Triangle3d storage)
        {
            if (index >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(index * 3, 3);

                storage.A = query.Get3d(positionComponent, indices[0]);
                storage.B = query.Get3d(positionComponent, indices[1]);
                storage.C = query.Get3d(positionComponent, indices[2]);
            }
            else
            {
                storage.A = query.Get3d(positionComponent, index * 3);
                storage.B = query.Get3d(positionComponent, index * 3 + 1);
                storage.C = query.Get3d(positionComponent, index * 3 + 2);
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The  triangle index offset.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(uint triangleIndex, Triangle3d[] storage)
        {
            Get(CommonComponents.Position, triangleIndex, storage);
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The  triangle index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(string positionComponent, uint triangleIndex, Triangle3d[] storage)
        {
            if (triangleIndex + storage.Length >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(triangleIndex * 3, (uint)storage.Length * 3);
                Vector3d[] data = query.Get3d(positionComponent, indices);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new Triangle3d(data[i * 3], data[i * 3 + 1], data[i * 3 + 2]);
                }
            }
            else
            {
                Vector3d[] data = query.Get3d(positionComponent, triangleIndex * 3, (uint)storage.Length * 3);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new Triangle3d(data[i * 3], data[i * 3 + 1], data[i * 3 + 2]);
                }
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The triangle index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="count">Number of triangles.</param>
        public Triangle3d[] Get3d(string positionComponent, uint triangleIndex, uint count)
        {
            Triangle3d[] data = new Triangle3d[count];
            Get(positionComponent, triangleIndex, data);
            return data;
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The triangle index offset.</param>
        /// <param name="count">Number of triangles.</param>
        public Triangle3d[] Get3d(uint triangleIndex, uint count)
        {
            return Get3d(CommonComponents.Position, triangleIndex, count);
        }

        //#endfor instanced to 'Double3'

		//#foreach instanced to 'Float2'


        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle triangle index.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        /// <returns></returns>
        public Triangle2f Get2f(uint index)
        {
            Triangle2f t = new Triangle2f();
            Get(CommonComponents.Position, index, t);
            return t;
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// correct format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public Triangle2f Get2f(string positionComponent, uint index)
        {
            Triangle2f t = new Triangle2f();
            Get(positionComponent, index, t);
            return t;
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// Vector2f format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public void Get(uint index, Triangle2f storage)
        {
            Get(CommonComponents.Position, index, storage);
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle triangle index.</param>
        /// <remarks>This accessor is not to be used in performance critical parts of applications.</remarks>
        /// <returns></returns>
        public void Get(string positionComponent, uint index, Triangle2f storage)
        {
            if (index >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(index * 3, 3);

                storage.A = query.Get2f(positionComponent, indices[0]);
                storage.B = query.Get2f(positionComponent, indices[1]);
                storage.C = query.Get2f(positionComponent, indices[2]);
            }
            else
            {
                storage.A = query.Get2f(positionComponent, index * 3);
                storage.B = query.Get2f(positionComponent, index * 3 + 1);
                storage.C = query.Get2f(positionComponent, index * 3 + 2);
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The  triangle index offset.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(uint triangleIndex, Triangle2f[] storage)
        {
            Get(CommonComponents.Position, triangleIndex, storage);
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The  triangle index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(string positionComponent, uint triangleIndex, Triangle2f[] storage)
        {
            if (triangleIndex + storage.Length >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(triangleIndex * 3, (uint)storage.Length * 3);
                Vector2f[] data = query.Get2f(positionComponent, indices);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new Triangle2f(data[i * 3], data[i * 3 + 1], data[i * 3 + 2]);
                }
            }
            else
            {
                Vector2f[] data = query.Get2f(positionComponent, triangleIndex * 3, (uint)storage.Length * 3);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new Triangle2f(data[i * 3], data[i * 3 + 1], data[i * 3 + 2]);
                }
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The triangle index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="count">Number of triangles.</param>
        public Triangle2f[] Get2f(string positionComponent, uint triangleIndex, uint count)
        {
            Triangle2f[] data = new Triangle2f[count];
            Get(positionComponent, triangleIndex, data);
            return data;
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The triangle index offset.</param>
        /// <param name="count">Number of triangles.</param>
        public Triangle2f[] Get2f(uint triangleIndex, uint count)
        {
            return Get2f(CommonComponents.Position, triangleIndex, count);
        }

        //#endfor instanced to 'Float2'

		//#foreach instanced to 'Float3'


        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle triangle index.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        /// <returns></returns>
        public Triangle3f Get3f(uint index)
        {
            Triangle3f t = new Triangle3f();
            Get(CommonComponents.Position, index, t);
            return t;
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// correct format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public Triangle3f Get3f(string positionComponent, uint index)
        {
            Triangle3f t = new Triangle3f();
            Get(positionComponent, index, t);
            return t;
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle index.</param>
        /// <param name="positionComponent">The component where we look for position, must be
        /// Vector2f format.</param>
        /// <remarks>This is not performance wise getter.</remarks>
        public void Get(uint index, Triangle3f storage)
        {
            Get(CommonComponents.Position, index, storage);
        }

        /// <summary>
        /// Gets triangle.
        /// </summary>
        /// <param name="index">The triangle triangle index.</param>
        /// <remarks>This accessor is not to be used in performance critical parts of applications.</remarks>
        /// <returns></returns>
        public void Get(string positionComponent, uint index, Triangle3f storage)
        {
            if (index >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(index * 3, 3);

                storage.A = query.Get3f(positionComponent, indices[0]);
                storage.B = query.Get3f(positionComponent, indices[1]);
                storage.C = query.Get3f(positionComponent, indices[2]);
            }
            else
            {
                storage.A = query.Get3f(positionComponent, index * 3);
                storage.B = query.Get3f(positionComponent, index * 3 + 1);
                storage.C = query.Get3f(positionComponent, index * 3 + 2);
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The  triangle index offset.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(uint triangleIndex, Triangle3f[] storage)
        {
            Get(CommonComponents.Position, triangleIndex, storage);
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The  triangle index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="storage">Where to store result.</param>
        public void Get(string positionComponent, uint triangleIndex, Triangle3f[] storage)
        {
            if (triangleIndex + storage.Length >= shapeCount)
                throw new ArgumentException("Index out of range, not that many triangles.");

            if (indexBuffer != null)
            {
                uint[] indices = indexBuffer.Getui(triangleIndex * 3, (uint)storage.Length * 3);
                Vector3f[] data = query.Get3f(positionComponent, indices);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new Triangle3f(data[i * 3], data[i * 3 + 1], data[i * 3 + 2]);
                }
            }
            else
            {
                Vector3f[] data = query.Get3f(positionComponent, triangleIndex * 3, (uint)storage.Length * 3);

                for (int i = 0; i < storage.Length; i++)
                {
                    storage[i] = new Triangle3f(data[i * 3], data[i * 3 + 1], data[i * 3 + 2]);
                }
            }
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The triangle index offset.</param>
        /// <param name="positionComponent">The position component.</param>
        /// <param name="count">Number of triangles.</param>
        public Triangle3f[] Get3f(string positionComponent, uint triangleIndex, uint count)
        {
            Triangle3f[] data = new Triangle3f[count];
            Get(positionComponent, triangleIndex, data);
            return data;
        }

        /// <summary>
        /// An array getter, optimized by reusing mappings.
        /// </summary>
        /// <param name="triangleIndex">The triangle index offset.</param>
        /// <param name="count">Number of triangles.</param>
        public Triangle3f[] Get3f(uint triangleIndex, uint count)
        {
            return Get3f(CommonComponents.Position, triangleIndex, count);
        }

        //#endfor instanced to 'Float3'



        #endregion
    }
}
