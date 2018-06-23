// This file was generated by TemplateEngine from template source 'Quad'
// using template 'Quad3d. Do not modify this file directly, modify it from template source.

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
using SharpMedia.Math.Shapes.Volumes;

namespace SharpMedia.Math.Shapes
{


    /// <summary>
    /// A quad in 3D space.
    /// </summary>
    [Serializable]
    public class Quad3d :
        IArea3d, IOutline3d, IControlPoints3d, IContainsPoint3d, ITransformable3d,
        IEquatable<Quad3d>, IComparable<Quad3d>,
        IEnumerable<Vector3d>, ICloneable<Quad3d>
        //#ifdef 3D

        , IAABoxBoundabled, ISphereBoundabled
        //#endif
    {
        #region Public Members

        /// <summary>
        /// The first control point.
        /// </summary>
        public Vector3d A;

        /// <summary>
        /// The second control point.
        /// </summary>
        public Vector3d B;

        /// <summary>
        /// The third control point.
        /// </summary>
        public Vector3d C;

        /// <summary>
        /// The forth control point.
        /// </summary>
        public Vector3d D;

        #endregion

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Quad3d()
        {
        }

        /// <summary>
        /// Constructor with all four points.
        /// </summary>
        /// <param name="p0">The first point.</param>
        /// <param name="p1">The second point.</param>
        /// <param name="p2">The third point.</param>
        /// <param name="p3">The forth point.</param>
        public Quad3d(Vector3d p0, Vector3d p1,
            Vector3d p2, Vector3d p3)
        {
            A = p0;
            B = p1;
            C = p2;
            D = p3;
        }

        #endregion

        #region Properties


        /// <summary>
        /// The alpha, angle at A.
        /// </summary>
        public double Alpha
        {
            get
            {
                Vector3d v1 = B - A;
                Vector3d v2 = D - A;
                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// Alpha in degrees.
        /// </summary>
        public double AlphaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Alpha);
            }
        }

        /// <summary>
        /// Beta angle, angle at B.
        /// </summary>
        public double Beta
        {
            get
            {
                Vector3d v1 = A - B;
                Vector3d v2 = C - B;
                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// Beta angle in degrees.
        /// </summary>
        public double BetaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Beta);
            }
        }

        /// <summary>
        /// Gamma angle, angle at C.
        /// </summary>
        public double Gamma
        {
            get
            {
                Vector3d v1 = B - C;
                Vector3d v2 = D - C;
                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// Gamma angle in degrees.
        /// </summary>
        public double GammaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Gamma);
            }
        }

        /// <summary>
        /// Delta angle, the angle at D.
        /// </summary>
        public double Delta
        {
            get
            {
                Vector3d v1 = A - D;
                Vector3d v2 = C - D;
                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// Delta angle in degrees.
        /// </summary>
        public double DeltaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Delta);
            }
        }

        /// <summary>
        /// The side a, between vertices A and B.
        /// </summary>
        public double SideA
        {
            get
            {
                return (B - A).Length;
            }
        }

        /// <summary>
        /// The side a, between vertices C and B.
        /// </summary>
        public double SideB
        {
            get
            {
                return (B - C).Length;
            }
        }

        /// <summary>
        /// The side a, between vertices C and D.
        /// </summary>
        public double SideC
        {
            get
            {
                return (D - C).Length;
            }
        }

        /// <summary>
        /// The side a, between vertices A and D.
        /// </summary>
        public double SideD
        {
            get
            {
                return (D - A).Length;
            }
        }

        /// <summary>
        /// The center of quad.
        /// </summary>
        public Vector3d Center
        {
            get
            {
                return (A + B + C + D) / (double)4.0;
            }
        }

        /// <summary>
        /// Is the quad square.
        /// </summary>
        public bool IsSquare
        {
            get
            {
                if (!IsRectangle) return false;

                // We check that all sides are the same length.
                return MathHelper.NearEqual((D - A).Length2, (B - A).Length2);
            }
        }

        /// <summary>
        /// Is the shape a rectangle.
        /// </summary>
        public bool IsRectangle
        {
            get
            {
                // We require all angles to be 90 degress.
                if (!MathHelper.NearEqual((B - A) * (D - A), 0.0)) return false;
                if (!MathHelper.NearEqual((A - B) * (C - B), 0.0)) return false;
                if (!MathHelper.NearEqual((D - C) * (B - C), 0.0)) return false;
                // The other is automatically.

                return true;
            }
        }


        #endregion

        #region Overrides

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(100);
            builder.Append("Quad : {");
            builder.Append(A.ToString());
            builder.Append(", ");
            builder.Append(B.ToString());
            builder.Append(", ");
            builder.Append(C.ToString());
            builder.Append(", ");
            builder.Append(D.ToString());
            builder.Append("}");
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Quad3d) return this.Equals((Quad3d)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Always prefer this split over the tesellation because it is faster and there is
        /// no need to allocate collection. This only works for quads/rectanges.
        /// </summary>
        /// <param name="t1">The first triangle.</param>
        /// <param name="t2">The second triangle.</param>
        public void Split(out Triangle3d t1, out Triangle3d t2)
        {
            t1 = new Triangle3d(A, B, C);
            t2 = new Triangle3d(A, C, D);
        }

        #endregion

        #region IArea3d Members

        public double Area
        {
            get
            {
                // We sum two triangles.
                return (double)0.5 * (A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y)) +
                       (double)0.5 * (A.X * (C.Y - D.Y) + C.X * (D.Y - A.Y) + D.X * (A.Y - C.Y));
            }
        }

        public void Tesselate(double resolution, Storage.Builders.ITriangleBuilder3d builder)
        {
            if (resolution < 0.0 ||
                (resolution > SideA && resolution > SideB && resolution > SideC && resolution > SideD))
            {
                if (builder.IsIndexed)
                {
                    // We simply append vertices and proceed.
                    uint indexBase = builder.AddControlPoints(A, B, C, D);

                    // Add vertices if indexed

                    builder.AddIndexedTriangles(
                        new uint[] { indexBase, indexBase + 1, indexBase + 2,
                                 indexBase, indexBase + 2, indexBase + 3 }
                    );
                }
                else
                {
                    // We need to pust 2 triangles.
                    builder.AddControlPoints(A, B, C, A, C, D);
                }
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        #endregion

        #region IOutline3d Members

        public Vector3d Sample(double t)
        {
            if (t >= (double)0.0 && t <= (1.0 / (double).25))
            {
                double inter = (double)4 * t;
                return A * (1.0 - inter) + B * inter;
            }
            else if (t <= ((double)2 / (double)4))
            {
                double inter = (double)4 * t - 1.0;
                return B * (1.0 - inter) + C * inter;
            }
            else if (t <= ((double)3 / (double)4))
            {
                double inter = (double)4 * t - (double)2;
                return C * (1.0 - inter) + D * inter;
            }
            else
            {
                double inter = (double)4 * t - (double)3;
                return D * (1.0 - inter) + A * inter;
            }
        }

        public void Sample(double resolution, Storage.Builders.ILineBuilder3d builder)
        {
            if (resolution < 0.0)
            {
                builder.AddLineStrip(true, A, B, C, D);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IOutlined Members

        public double OutlineLength
        {
            get { return SideA + SideB + SideC + SideD; }
        }

        #endregion

        #region IControlPoints3d Members

        public Vector3d[] ControlPoints
        {
            get
            {
                return new Vector3d[] { A, B, C, D };
            }
            set
            {
                if (value.Length != 4) throw new ArgumentException("Three control points expected.");
                A = value[0];
                B = value[1];
                C = value[2];
                D = value[3];
            }
        }

        public void SetControlPoints(uint index, Vector3d cp)
        {
            switch (index)
            {
                case 0:
                    A = cp;
                    break;
                case 1:
                    B = cp;
                    break;
                case 2:
                    C = cp;
                    break;
                case 3:
                    D = cp;
                    break;
                default:
                    throw new ArgumentException("Index out of range, must be 0-3 for quad.");
            }
        }

        public Vector3d GetControlPoint(uint index)
        {
            switch (index)
            {
                case 0:
                    return A;
                case 1:
                    return B;
                case 2:
                    return C;
                case 3:
                    return D;
                default:
                    throw new ArgumentException("Index out of range, must be 0-3 for quad.");
            }
        }

        #endregion

        #region IControlPointsd Members

        public uint ControlPointCount
        {
            get { return 4; }
        }

        #endregion

        #region IContainsPoint3d Members

        public bool ContainsPoint(Vector3d point)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITransformable3d

        public void Transform(Matrix.Matrix4x4d matrix)
        {
            A = matrix * A;
            B = matrix * B;
            C = matrix * C;
            D = matrix * D;
        }

        #endregion

        //#ifdef 3D


        #region IAABoxBoundabled Members

        public AABoxd BoundingAABox
        {
            get
            {
                return AABoxd.FromPoints(A, B, C, D);
            }
        }

        #endregion

        #region ISphereBoundabled Members

        public Sphered BoundingSphere
        {
            get
            {
                return Sphered.FromPoints(A, B, C, D);
            }
        }

        #endregion

        //#endif

        #region IEquatable<Quad3d> Members

        public bool Equals(Quad3d other)
        {
            return Vector3d.NearEqual(A, other.A) &&
                   Vector3d.NearEqual(B, other.B) &&
                   Vector3d.NearEqual(C, other.C) &&
                   Vector3d.NearEqual(D, other.D);

        }

        #endregion

        #region IEnumerable<Vector3d> Members

        public IEnumerator<Vector3d> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }

        #endregion

        #region ICloneable<Quad3d> Members

        public Quad3d Clone()
        {
            return new Quad3d(A, B, C, D);
        }

        #endregion

        #region IComparable<Triangled> Members

        public int CompareTo(Quad3d other)
        {
            double a1 = Area, a2 = other.Area;
            if (a1 < a2) return -1;
            else if (a1 == a2) return 0;
            return 1;
        }

        #endregion


    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class Quad3d_Test
    {
        /*
        [CorrectnessTest]
        public void Construction()
        {
            Quad3d q = new Quad3d(new Vector3d(0, 0, 0),
                                new Vector3d(1, 0, 0),
                                new Vector3d(1, 1, 0),
                                new Vector3d(0, 1, 0));

            Assert.AreEqual(q.A, new Vector3d(0, 0, 0));
            Assert.AreEqual(q.B, new Vector3d(1, 0, 0));
            Assert.AreEqual(q.C, new Vector3d(1, 1, 0));
            Assert.AreEqual(q.D, new Vector3d(0, 1, 0));
            Assert.AreEqual(q.AlphaDegrees, 90.0);
        }

        [CorrectnessTest]
        public void Intersection()
        {

        }

        [CorrectnessTest]
        public void Tesselation()
        {

        }*/
    }
#endif

}
