// This file was generated by TemplateEngine from template source 'Polygon'
// using template 'Polygon3d. Do not modify this file directly, modify it from template source.

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
using SharpMedia.Testing;
using SharpMedia.Math.Matrix;

namespace SharpMedia.Math.Shapes
{


    /// <summary>
    /// Polygon in 3D space.
    /// </summary>
    [Serializable]
    public sealed class Polygon3d :
        IArea3d, IOutline3d, IControlPoints3d, IContainsPoint3d, ITransformable3d,
        IEquatable<Polygon3d>, IComparable<Polygon3d>,
        IEnumerable<Vector3d>, ICloneable<Polygon3d>
    {
        #region Public Members

        /// <summary>
        /// The points of polygon.
        /// </summary>
        public Vector3d[] Points;

        #endregion

        #region Private Helpers

        /// <summary>
        /// Splits line, by adding beginning and possibly middle point, but omitting last point.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="resolution"></param>
        /// <param name="data"></param>
        void SplitLine(Vector3d v1, Vector3d v2, double resolution, List<Vector3d> data)
        {
            if ((v1 - v2).Length2 > resolution * resolution)
            {
                double count = MathHelper.Floor((v1 - v2).Length / resolution);

                for (double i = 1; i < count; i += 1.0)
                {
                    double t = (double)(i - 1) / (double)count;
                    data.Add((1.0 - t) * v1 + t * v2);
                }
            }
            else
            {
                data.Add(v1);
            }

        }

        #endregion

        #region Properties

        /// <summary>
        /// An array accessor.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3d this[uint index]
        {
            get
            {
                return Points[(int)index];
            }
            set
            {
                Points[(int)index] = value;
            }
        }

        /// <summary>
        /// The center point.
        /// </summary>
        public Vector3d Center
        {
            get
            {
                Vector3d center = Vector3d.Zero;
                for (int i = 0; i < Points.Length; i++)
                {
                    center += Points[i];
                }

                center /= (double)Points.Length;
                return center;
            }
        }

        /// <summary>
        /// Is the polygon convex.
        /// </summary>
        public bool IsConvex
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Is the polygon clockwise.
        /// </summary>
        public bool IsClockwise
        {
            get
            {
                // We check winding order.
                double area = 0.0;
                for (int j = 1; j < Points.Length; j++)
                {
                    // FIXME: this is Z projection assumed.
                    area += Points[j - 1].X * Points[j].Y - Points[j - 1].Y * Points[j].X;
                }

                return area <= 0.0;
            }
        }

        /// <summary>
        /// Is the polygon anticlockwise.
        /// </summary>
        public bool IsAntiClockwise
        {
            get
            {
                return !IsClockwise;
            }
        }

        /// <summary>
        /// The bounding region.
        /// </summary>
        public Region3d BoundingRegion
        {
            get
            {
                return Region3d.MinMax(Points);
            }
        }


        #endregion

        #region Public Members

        /// <summary>
        /// Reverses the order of control points, e.g. 
        /// the converts clockwise to anticlockwise and vice versa.
        /// </summary>
        public void Reverse()
        {
            int n = Points.Length / 2;
            for (int i = 0; i < n; i++)
            {
                Vector3d t = Points[i];
                Points[i] = Points[n - i - 1];
                Points[n - i - 1] = t;
            }
        }

        /// <summary>
        /// Converts a polygon to line list, given a resolution.
        /// </summary>
        /// <param name="resolution">The resolution, negative means adaptive.</param>
        /// <returns></returns>
        public Vector3d[] ConvertToList(double resolution)
        {
            return ConvertToList(resolution, false);
        }

        /// <summary>
        /// Converts a polygon to line list, given a resolution.
        /// </summary>
        /// <param name="resolution">The resolution, negative means adaptive.</param>
        /// <param name="duplicateFirst">Is the first point duplicated.</param>
        /// <returns></returns>
        public Vector3d[] ConvertToList(double resolution, bool duplicateFirst)
        {
            Vector3d[] points = Points;

            if (resolution > 0.0)
            {
                List<Vector3d> list = new List<Vector3d>(Points.Length);

                for (int i = 1; i < Points.Length; i++)
                {
                    SplitLine(Points[i - 1], Points[i], resolution, list);
                }
                SplitLine(Points[Points.Length - 1], Points[0], resolution, list);

                if (duplicateFirst)
                {
                    list.Add(Points[0]);
                }

                points = list.ToArray();
            }
            else if(duplicateFirst)
            {
                points = new Vector3d[Points.Length + 1];
                Points.CopyTo(points, 0);
                points[points.Length - 1] = points[0];
            }

            return points;
        }

        /// <summary>
        /// Obtains a line length.
        /// </summary>
        /// <param name="index">The line index, from control point 'index' to 'index+1'.</param>
        /// <returns></returns>
        public double GetLineLength(uint index)
        {
            if (index >= Points.Length) throw new ArgumentException("Index out of range.");
            uint nextIndex = index + 1 >= Points.Length ? 0 : index + 1;
            return (Points[nextIndex] - Points[index]).Length;
        }

        

        #endregion

        #region Constructors

        /// <summary>
        /// Polygon constructor.
        /// </summary>
        /// <param name="points">Points of polygon.</param>
        public Polygon3d(params Vector3d[] points)
        {
            this.Points = points;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(100);
            builder.Append("Polygon : {");
            for (int i = 0; i < Points.Length; i++)
            {
                builder.AppendLine(Points[i].ToString());
            }
            builder.Append("}");
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Polygon3d)
            {
                return this.Equals((Polygon3d)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IArea3d Members

        public double Area
        {
            get
            {
                return double.NaN;
            }
        }

        public void Tesselate(double resolution, Storage.Builders.ITriangleBuilder3d builder)
        {
            Vector3d[] points = this.ConvertToList(resolution);

            // We now tesselate points.
            List<Vector3d[]> polygons = new List<Vector3d[]>();
            polygons.Add(points);

            Algorithms.PolygonTesselation.Tesselate(polygons, builder);
        }

        #endregion

        #region IOutline3d Members

        public Vector3d Sample(double t)
        {
            uint point = (uint)(t * (double)Points.Length);

            // We calculate interpolation ratio.
            double intRatio = t * Points.Length - t * (double)point;

            uint nextPoint = point + 1 >= Points.Length ? 0 : point + 1;

            // We return linear interpolation.
            return (1.0 - intRatio) * Points[point] + intRatio * Points[nextPoint];

        }

        public void Sample(double resolution, Storage.Builders.ILineBuilder3d builder)
        {
            Vector3d[] points = this.ConvertToList(resolution);

            if (points.Length == 0) return;

            builder.AddLineStrip(true, points);
        }

        #endregion

        #region IOutlined Members

        public double OutlineLength
        {
            get
            {
                double length = 0.0;
                for (int i = 1; i < Points.Length; i++)
                {
                    length += (Points[i - 1] - Points[i]).Length;
                }
                length += (Points[Points.Length - 1] - Points[0]).Length;
                return length;
            }

        }

        #endregion

        #region IControlPoints3d Members

        public Vector3d[] ControlPoints
        {
            get
            {
                return Points;
            }
            set
            {
                Points = value;
            }
        }

        public void SetControlPoints(uint index, Vector3d cp)
        {
            Points[(int)index] = cp;
        }

        public Vector3d GetControlPoint(uint index)
        {
            return Points[(int)index];
        }

        #endregion

        #region IControlPointsd Members

        public uint ControlPointCount
        {
            get { return (uint)Points.Length; }
        }

        #endregion

        #region IContainsPoint3d Members

        public bool ContainsPoint(Vector3d point)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITransformable3d

        public void Transform(Matrix.Matrix4x4d matrix)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = matrix * Points[i];
            }
        }

        #endregion

        #region IEnumerable<Vector3d> Members

        public IEnumerator<Vector3d> GetEnumerator()
        {
            for (int i = 0; i < Points.Length; i++)
            {
                yield return Points[i];
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Points.Length; i++)
            {
                yield return Points[i];
            }
        }

        #endregion

        #region IEquatable<Polygon3d> Members

        public bool Equals(Polygon3d other)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                if (!Vector3d.NearEqual(this.Points[i], other.Points[i])) return false;
            }
            return true;
        }

        #endregion

        #region ICloneable Members

        public Polygon3d Clone()
        {
            return new Polygon3d(Points.Clone() as Vector3d[]);
        }

        #endregion

        #region IComparable<Triangled> Members

        public int CompareTo(Polygon3d other)
        {
            int cmp = Points.Length.CompareTo(other.Points.Length);
            if (cmp != 0) return cmp;

            for (int i = 0; i < Points.Length; i++)
            {
                cmp = Points[i].CompareTo(other.Points[i]);
                if (cmp != 0) return cmp;
            }
            return 0;
        }

        #endregion
    }


#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class Test_Polygon3d
    {
        [CorrectnessTest]
        public void Construction()
        {
            
        }

       

    }
#endif
}