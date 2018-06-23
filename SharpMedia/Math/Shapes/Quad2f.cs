// This file was generated by TemplateEngine from template source 'Quad'
// using template 'Quad2f. Do not modify this file directly, modify it from template source.

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
    /// A quad in 2D space.
    /// </summary>
    [Serializable]
    public class Quad2f :
        IArea2f, IOutline2f, IControlPoints2f, IContainsPoint2f, ITransformable2f,
        IEquatable<Quad2f>, IComparable<Quad2f>,
        IEnumerable<Vector2f>, ICloneable<Quad2f>
        
    {
        #region Public Members

        /// <summary>
        /// The first control point.
        /// </summary>
        public Vector2f A;

        /// <summary>
        /// The second control point.
        /// </summary>
        public Vector2f B;

        /// <summary>
        /// The third control point.
        /// </summary>
        public Vector2f C;

        /// <summary>
        /// The forth control point.
        /// </summary>
        public Vector2f D;

        #endregion

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Quad2f()
        {
        }

        /// <summary>
        /// Constructor with all four points.
        /// </summary>
        /// <param name="p0">The first point.</param>
        /// <param name="p1">The second point.</param>
        /// <param name="p2">The third point.</param>
        /// <param name="p3">The forth point.</param>
        public Quad2f(Vector2f p0, Vector2f p1,
            Vector2f p2, Vector2f p3)
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
        public float Alpha
        {
            get
            {
                Vector2f v1 = B - A;
                Vector2f v2 = D - A;
                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// Alpha in degrees.
        /// </summary>
        public float AlphaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Alpha);
            }
        }

        /// <summary>
        /// Beta angle, angle at B.
        /// </summary>
        public float Beta
        {
            get
            {
                Vector2f v1 = A - B;
                Vector2f v2 = C - B;
                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// Beta angle in degrees.
        /// </summary>
        public float BetaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Beta);
            }
        }

        /// <summary>
        /// Gamma angle, angle at C.
        /// </summary>
        public float Gamma
        {
            get
            {
                Vector2f v1 = B - C;
                Vector2f v2 = D - C;
                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// Gamma angle in degrees.
        /// </summary>
        public float GammaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Gamma);
            }
        }

        /// <summary>
        /// Delta angle, the angle at D.
        /// </summary>
        public float Delta
        {
            get
            {
                Vector2f v1 = A - D;
                Vector2f v2 = C - D;
                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// Delta angle in degrees.
        /// </summary>
        public float DeltaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Delta);
            }
        }

        /// <summary>
        /// The side a, between vertices A and B.
        /// </summary>
        public float SideA
        {
            get
            {
                return (B - A).Length;
            }
        }

        /// <summary>
        /// The side a, between vertices C and B.
        /// </summary>
        public float SideB
        {
            get
            {
                return (B - C).Length;
            }
        }

        /// <summary>
        /// The side a, between vertices C and D.
        /// </summary>
        public float SideC
        {
            get
            {
                return (D - C).Length;
            }
        }

        /// <summary>
        /// The side a, between vertices A and D.
        /// </summary>
        public float SideD
        {
            get
            {
                return (D - A).Length;
            }
        }

        /// <summary>
        /// The center of quad.
        /// </summary>
        public Vector2f Center
        {
            get
            {
                return (A + B + C + D) / (float)4.0;
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
                if (!MathHelper.NearEqual((B - A) * (D - A), 0.0f)) return false;
                if (!MathHelper.NearEqual((A - B) * (C - B), 0.0f)) return false;
                if (!MathHelper.NearEqual((D - C) * (B - C), 0.0f)) return false;
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
            if (obj is Quad2f) return this.Equals((Quad2f)obj);
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
        public void Split(out Triangle2f t1, out Triangle2f t2)
        {
            t1 = new Triangle2f(A, B, C);
            t2 = new Triangle2f(A, C, D);
        }

        #endregion

        #region IArea2f Members

        public float Area
        {
            get
            {
                // We sum two triangles.
                return (float)0.5 * (A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y)) +
                       (float)0.5 * (A.X * (C.Y - D.Y) + C.X * (D.Y - A.Y) + D.X * (A.Y - C.Y));
            }
        }

        public void Tesselate(float resolution, Storage.Builders.ITriangleBuilder2f builder)
        {
            if (resolution < 0.0f ||
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

        #region IOutline2f Members

        public Vector2f Sample(float t)
        {
            if (t >= (float)0.0 && t <= (1.0f / (float).25))
            {
                float inter = (float)4 * t;
                return A * (1.0f - inter) + B * inter;
            }
            else if (t <= ((float)2 / (float)4))
            {
                float inter = (float)4 * t - 1.0f;
                return B * (1.0f - inter) + C * inter;
            }
            else if (t <= ((float)3 / (float)4))
            {
                float inter = (float)4 * t - (float)2;
                return C * (1.0f - inter) + D * inter;
            }
            else
            {
                float inter = (float)4 * t - (float)3;
                return D * (1.0f - inter) + A * inter;
            }
        }

        public void Sample(float resolution, Storage.Builders.ILineBuilder2f builder)
        {
            if (resolution < 0.0f)
            {
                builder.AddLineStrip(true, A, B, C, D);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IOutlinef Members

        public float OutlineLength
        {
            get { return SideA + SideB + SideC + SideD; }
        }

        #endregion

        #region IControlPoints2f Members

        public Vector2f[] ControlPoints
        {
            get
            {
                return new Vector2f[] { A, B, C, D };
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

        public void SetControlPoints(uint index, Vector2f cp)
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

        public Vector2f GetControlPoint(uint index)
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

        #region IControlPointsf Members

        public uint ControlPointCount
        {
            get { return 4; }
        }

        #endregion

        #region IContainsPoint2f Members

        public bool ContainsPoint(Vector2f point)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITransformable2f

        public void Transform(Matrix.Matrix3x3f matrix)
        {
            A = matrix * A;
            B = matrix * B;
            C = matrix * C;
            D = matrix * D;
        }

        #endregion

        

        #region IEquatable<Quad2f> Members

        public bool Equals(Quad2f other)
        {
            return Vector2f.NearEqual(A, other.A) &&
                   Vector2f.NearEqual(B, other.B) &&
                   Vector2f.NearEqual(C, other.C) &&
                   Vector2f.NearEqual(D, other.D);

        }

        #endregion

        #region IEnumerable<Vector2f> Members

        public IEnumerator<Vector2f> GetEnumerator()
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

        #region ICloneable<Quad2f> Members

        public Quad2f Clone()
        {
            return new Quad2f(A, B, C, D);
        }

        #endregion

        #region IComparable<Triangled> Members

        public int CompareTo(Quad2f other)
        {
            float a1 = Area, a2 = other.Area;
            if (a1 < a2) return -1;
            else if (a1 == a2) return 0;
            return 1;
        }

        #endregion


    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class Quad2f_Test
    {
        /*
        [CorrectnessTest]
        public void Construction()
        {
            Quad2f q = new Quad2f(new Vector2f(0, 0, 0),
                                new Vector2f(1, 0, 0),
                                new Vector2f(1, 1, 0),
                                new Vector2f(0, 1, 0));

            Assert.AreEqual(q.A, new Vector2f(0, 0, 0));
            Assert.AreEqual(q.B, new Vector2f(1, 0, 0));
            Assert.AreEqual(q.C, new Vector2f(1, 1, 0));
            Assert.AreEqual(q.D, new Vector2f(0, 1, 0));
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
