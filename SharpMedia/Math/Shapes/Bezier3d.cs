// This file was generated by TemplateEngine from template source 'Bezier'
// using template 'Bezier3d. Do not modify this file directly, modify it from template source.

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
using SharpMedia.AspectOriented;
using SharpMedia.Math.Shapes.Volumes;

namespace SharpMedia.Math.Shapes
{


    /// <summary>
    /// A bezier curve, using 3 control points to make the curve.
    /// </summary>
    public sealed class Bezier3d :
        IOutline3d, IControlPoints3d, IContainsPoint3d, ITransformable3d,
        IEquatable<Bezier3d>, IComparable<Bezier3d>,
        IEnumerable<Vector3d>, ICloneable<Bezier3d>
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
        /// The middle control point.
        /// </summary>
        public Vector3d B;

        /// <summary>
        /// The end control point.
        /// </summary>
        public Vector3d C;

        #endregion

        #region Properties

        /// <summary>
        /// Is it closed shape.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return Vector3d.NearEqual(A, C);
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Are two bezier curves connected to each other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsConnected(Bezier3d other)
        {
            return Vector3d.NearEqual(this.A, other.A) ||
                   Vector3d.NearEqual(this.A, other.C) ||
                   Vector3d.NearEqual(this.C, other.A) ||
                   Vector3d.NearEqual(this.C, other.C);
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Creates bezier curve from 3 points (not control points).
        /// </summary>
        /// <param name="p0">Beginning point of bezier curve.</param>
        /// <param name="p1">Middle point </param>
        /// <returns>Bezier curve.</returns>
        public static Bezier3d Approximate(Vector3d p0, Vector3d p1, Vector3d p2)
        {
            return null;
        }

        /// <summary>
        /// Approximates the points by bezier curve. Points must be close to the line,
        /// first and last points are fixed.
        /// </summary>
        /// <param name="ps">The points that represent it.</param>
        /// <param name="maxBeziers">Maximum number of curves returned.</param>
        /// <returns>Bezier curves.</returns>
        public static Bezier3d[] Approximate([NotEmptyArray] Vector3d[] ps, uint maxBeziers)
        {
            return null;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Bezier3d()
        {
        }

        /// <summary>
        /// Constructor with all three points.
        /// </summary>
        /// <param name="p1">First control point.</param>
        /// <param name="p2">Second control point.</param>
        /// <param name="p3">Third control point.</param>
        public Bezier3d(Vector3d p1, Vector3d p2, Vector3d p3)
        {
            A = p1;
            B = p2;
            C = p3;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(100);
            builder.Append("Bezier : {");
            builder.Append(A.ToString());
            builder.Append(", ");
            builder.Append(B.ToString());
            builder.Append(", ");
            builder.Append(C.ToString());
            builder.Append("}");
            return builder.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Bezier3d) return this.Equals((Bezier3d)obj);
            return false;
        }

        #endregion

        #region IContainsPoint3d Members

        public bool ContainsPoint(Vector3d p)
        {
            // We solve quadratic equation for t.
            // p0.x = a.x * (1-t)^2 + b.x * t * (1-t) + c.x * t^2

            double t1, t2;

            // If first is non-linear.
            if (!MathHelper.NearEqual(A.X - (double)2.0 * B.X + C.X, 0.0))
            {
                // If no real solutions, this is not true.
                if (!Functions.Polynomial.Roots(A.X - (double)2.0 * B.X + C.X, (double)2.0 * B.X - (double)2.0 * A.X, A.X - p.X,
                                                out t1, out t2)) return false;
            }
            else if (!MathHelper.NearEqual(A.Y - (double)2.0 * B.Y + C.Y, 0.0))
            {
                // If no real solutions, this is not true.
                if (!Functions.Polynomial.Roots(A.Y - (double)2.0 * B.Y + C.Y, (double)2.0 * B.Y - (double)2.0 * A.Y, A.Y - p.Y,
                                                out t1, out t2)) return false;

            }
            //#ifdef 3D

            else if (!MathHelper.NearEqual(A.Z - (double)2.0 * B.Z + C.Z, 0.0))
            {
                // If no real solutions, this is not true.
                if (!Functions.Polynomial.Roots(A.Z - (double)2.0 * B.Z + C.Z, (double)2.0 * B.Z - (double)2.0 * A.Z, A.Z - p.Z,
                                                out t1, out t2)) return false;
            }
            //#endif
            else
            {
                return false;
            }

            // We check if any of parameters satisfies:
            if (t1 >= 0.0 && t1 <= 1.0)
            {
                if (Vector3d.NearEqual(p, Sample(t1))) return true;
            }
            if (t2 >= 0.0 && t2 <= 1.0)
            {
                if (Vector3d.NearEqual(p, Sample(t2))) return true;
            }
            return false;
        }

        #endregion

        #region IOutline3d Members

        public Vector3d Sample(double t)
        {
            double t1 = 1.0 - t;
            return t1 * t1 * A + (double)2 * t1 * t * B + t * t * C;
        }

        public void Sample(double resolution, Storage.Builders.ILineBuilder3d builder)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IOutlined Members

        public double OutlineLength
        {
            get
            {
                // Those parameters can be changed for "section" length.
                double t1 = 0.0, t2 = 1.0;

                Vector3d spec;
                
                spec = new Vector3d(1, 1, 1);
                

                // We solve the equation. We first compute coefficients for first derivate.
                Vector3d M = (double)2.0 * (-(double)2.0 * Vector3d.ComponentMultiply(A, B) + C);
                Vector3d N = (double)2.0 * (Vector3d.ComponentMultiply(A, B) - A);

                // Now we must compute integral( sqrt((dx/dt)^2 + (dy/dt)^2 + (dz/dt)^2)), we fit in to the
                // formula and compute new coeficients of sqrt(polinomial) to integrate.
                double C0 = M * M;
                double C1 = (double)2.0 * M * N;
                double C2 = N * N;

                // We now compute the actual length by solving the integral:
                return (C1 + (double)2.0 * C0 * t1) / ((double)4.0 * C0) * MathHelper.Sqrt(t1 * (C1 + t1 * C0) + C2) -
                       (C1 + (double)2.0 * C0 * t2) / ((double)4.0 * C0) * MathHelper.Sqrt(t2 * (C1 + t2 * C0) + C2) +
                       ((double)4.0 * C0 * C2 - C1 * C1) / ((double)8.0 * MathHelper.Sqrt(C0 * C0 * C0)) *
                       MathHelper.Ln(((double)2.0 * C0 * t1 + C1 + (double)2.0 * MathHelper.Sqrt(C0 * (t1 * (-C1 + C0 * t1) + C2))) /
                                     ((double)2.0 * C0 * t2 + C1 + (double)2.0 * MathHelper.Sqrt(C0 * (t2 * (-C1 + C0 * t2) + C2))));
            }
        }

        #endregion

        #region IControlPoints3d Members

        public Vector3d[] ControlPoints
        {
            get
            {
                return new Vector3d[] { A, B, C };
            }
            set
            {
                if (value.Length != 3) throw new ArgumentException("Three control points expected for bezier curve.");
                A = value[0];
                B = value[1];
                C = value[2];
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
                default:
                    throw new ArgumentException("Index out of range, must be 0-2 for bezier curve.");
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
                default:
                    throw new ArgumentException("Index out of range, must be 0-2 for bezier curve.");
            }
        }

        #endregion

        #region IControlPointsd Members

        public uint ControlPointCount
        {
            get { return 3; }
        }

        #endregion

        #region ITransformable3d

        public void Transform(Matrix.Matrix4x4d matrix)
        {
            A = matrix * A;
            B = matrix * B;
            C = matrix * C;
        }

        #endregion

        //#ifdef 3D


        #region IAABoxBoundabled Members

        public AABoxd BoundingAABox
        {
            get
            {
                // Possibly not optimal!
                return AABoxd.FromPoints(A, B, C);
            }
        }

        #endregion

        #region ISphereBoundabled Members

        public Sphered BoundingSphere
        {
            get
            {
                // Possibly not optimal!
                return Sphered.FromPoints(A, B, C);
            }
        }

        #endregion

        //#endif

        #region ICloneable<Bezier3d> Members

        public Bezier3d Clone()
        {
            return new Bezier3d(A, B, C);
        }

        #endregion

        #region IEquatable<Bezier3d> Members

        public bool Equals(Bezier3d other)
        {
            if (Vector3d.NearEqual(this.A, other.A) &&
                Vector3d.NearEqual(this.B, other.B) &&
                Vector3d.NearEqual(this.C, other.C)) return true;
            return false;
        }
        #endregion

        #region IComparable<ClassName> Members

        public int CompareTo(Bezier3d other)
        {
            int cmp = A.CompareTo(other.A);
            if (cmp != 0) return cmp;
            cmp = B.CompareTo(other.B);
            if (cmp != 0) return cmp;
            return C.CompareTo(other.C);

        }

        #endregion

        #region IEnumerable<Vector3d> Members

        public IEnumerator<Vector3d> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
        }

        #endregion


    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class Test_Bezier3d
    {
        /*
        [CorrectnessTest]
        public void Properties()
        {
            Bezier3d b = new Bezier3d(new Vector3d(0, 0, 0),
                                    new Vector3d(1, 1, 0),
                                    new Vector3d(2, 0, 0));

            Assert.AreEqual(false, b.IsClosed);
            Assert.AreEqual(new Vector3d(0, 0, 0), b.A);
            Assert.AreEqual(new Vector3d(1, 1, 0), b.B);
            Assert.AreEqual(new Vector3d(2, 0, 0), b.C);
            Assert.AreEqual(new Vector3d(0, 0, 0), b.Sample(0.0));
            Assert.AreEqual(new Vector3d(2, 0, 0), b.Sample(1.0));
        }

        [CorrectnessTest]
        public void Contains()
        {
            Bezier3d b = new Bezier3d(new Vector3d(0, 0, 0),
                                    new Vector3d(1, 1, 0),
                                    new Vector3d(2, 0, 0));

            Vector3d p = b.Sample(0.3);
            Assert.IsTrue(b.ContainsPoint(p));
            Assert.IsFalse(b.ContainsPoint(p + new Vector3d(0, 0, 1)));
        }

        [CorrectnessTest]
        public void Intersections()
        {


        }*/
    }
#endif
}
