// This file was generated by TemplateEngine from template source 'Triangle'
// using template 'Triangle3f. Do not modify this file directly, modify it from template source.

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
using SharpMedia.Math.Shapes.Volumes;

namespace SharpMedia.Math.Shapes
{


    /// <summary>
    /// Any triangle in 3D space.
    /// </summary>
    [Serializable]
    public sealed class Triangle3f :
        IArea3f, IOutline3f, IControlPoints3f, IContainsPoint3f, ITransformable3f,
        IEquatable<Triangle3f>, IComparable<Triangle3f>,
        IEnumerable<Vector3f>, ICloneable<Triangle3f>
        //#ifdef 3D

        , IAABoxBoundablef, ISphereBoundablef
        //#endif

    {
        #region Public Members

        /// <summary>
        /// The first point.
        /// </summary>
        public Vector3f A;

        /// <summary>
        /// The second point.
        /// </summary>
        public Vector3f B;

        /// <summary>
        /// The third point.
        /// </summary>
        public Vector3f C;

        #endregion

        #region Properties

        //#ifdef 3D


        /// <summary>
        /// The plane normal.
        /// </summary>
        public Vector3f Normal
        {
            get
            {
                return Plane3f.PlaneNormal(A, B, C);
            }
        }

        //#endif

        /// <summary>
        /// The ratio between the circe around the triangle and the biggest circle inside
        /// the triangle.
        /// </summary>
        public float AspectRation
        {
            get
            {
                // Ration between maximum side and minimum atitute.
                return MathHelper.Max(SideA, SideB, SideC) / MathHelper.Min(AltitudeA, AltitudeB, AltitudeC);
            }
        }

        /// <summary>
        /// Angle at A.
        /// </summary>
        public float Alpha
        {
            get
            {
                Vector3f v1 = B - A;
                Vector3f v2 = C - A;

                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// Angle at B.
        /// </summary>
        public float Beta
        {
            get
            {
                Vector3f v1 = A - B;
                Vector3f v2 = C - B;

                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// Angle at C.
        /// </summary>
        public float Gamma
        {
            get
            {
                Vector3f v1 = B - C;
                Vector3f v2 = A - C;

                return MathHelper.ACos((v1 * v2) / (v1.Length * v2.Length));
            }
        }

        /// <summary>
        /// The alpha angle to degrees.
        /// </summary>
        public float AlphaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Alpha);
            }
        }

        /// <summary>
        /// The beta angle to degrees.
        /// </summary>
        public float BetaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Beta);
            }
        }

        /// <summary>
        /// The gamma angle to degrees.
        /// </summary>
        public float GammaDegrees
        {
            get
            {
                return MathHelper.ToDegrees(Gamma);
            }
        }

        /// <summary>
        /// The length of side A.
        /// </summary>
        public float SideA
        {
            get
            {
                return (C - B).Length;
            }
        }

        /// <summary>
        /// Length of side B.
        /// </summary>
        public float SideB
        {
            get
            {
                return (A - C).Length;
            }
        }

        /// <summary>
        /// Length of side C.
        /// </summary>
        public float SideC
        {
            get
            {
                return (B - C).Length;
            }
        }

        /// <summary>
        /// Altitute on A.
        /// </summary>
        public float AltitudeA
        {
            get
            {
                return MathHelper.Sin(Beta) * SideC;
            }
        }

        /// <summary>
        /// Altitute on B.
        /// </summary>
        public float AltitudeB
        {
            get
            {
                return MathHelper.Sin(Alpha) * SideC;
            }
        }

        /// <summary>
        /// Altitute on C.
        /// </summary>
        public float AltitudeC
        {
            get
            {
                return MathHelper.Sin(Alpha) * SideA;
            }
        }

        /// <summary>
        /// Is triangle right.
        /// </summary>
        public bool IsRight
        {
            get
            {
                Vector3f v1 = B - A;
                Vector3f v2 = C - B;
                Vector3f v3 = A - C;

                if (MathHelper.NearEqual(v1 * v2, 0.0f)) return true;
                if (MathHelper.NearEqual(v2 * v3, 0.0f)) return true;
                if (MathHelper.NearEqual(v3 * v1, 0.0f)) return true;

                return false;
            }
        }

        /// <summary>
        /// Is triangle equilateral.
        /// </summary>
        public bool IsEquilateral
        {
            get
            {
                return MathHelper.NearEqual(SideA, SideB) &&
                       MathHelper.NearEqual(SideB, SideC);
            }
        }

        /// <summary>
        /// Is triangle isosceles.
        /// </summary>
        public bool IsIsosceles
        {
            get
            {
                return MathHelper.NearEqual(SideA, SideB) ||
                       MathHelper.NearEqual(SideB, SideC) ||
                       MathHelper.NearEqual(SideC, SideA);
            }
        }


        /// <summary>
        /// Center of triangle.
        /// </summary>
        public Vector3f Center
        {
            get
            {
                return (A + B + C) / (float)3;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Triangle constructor.
        /// </summary>
        public Triangle3f()
        {
        }

        /// <summary>
        /// Triangle construction.
        /// </summary>
        /// <param name="a">The point 1.</param>
        /// <param name="b">The point 2.</param>
        /// <param name="c">The point 3.</param>
        public Triangle3f(Vector3f a, Vector3f b, Vector3f c)
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Obtains barycentric coordinate relative to this triangle.
        /// </summary>
        /// <param name="input">The vector that lies in triangle plane.</param>
        /// <returns></returns>
        public Vector2f GetBarycentric(Vector3f input)
        {
            //#ifdef 3D


            // We need positive components of normal, only magnitude relavant.
            Vector3f normal = this.Normal;
            normal = Vector3f.ComponentMultiply(normal, normal);

            if (normal.X > normal.Y)
            {
                if (normal.X > normal.Y)
                {
                    // We project to x axis, all xs are the same.
                    return LinearSolver.SolveSystem(B.Y - A.Y, C.Y - A.Y,
                                                    B.Z - A.Z, C.Z - A.Z,
                                                    input.Y - A.Y, input.Z - A.Z);
                }
                else
                {
                    // We project to z axis, all zs are the same.
                    return LinearSolver.SolveSystem(B.X - A.X, C.X - A.X,
                                                    B.Y - A.Y, C.Y - A.Y,
                                                    input.X - A.X, input.Y - A.Y);
                }
            }
            else
            {
                if (normal.Y > normal.Z)
                {
                    // We project to y axis, all ys are the same.
                    return LinearSolver.SolveSystem(B.X - A.X, C.X - A.X,
                                                    B.Z - A.Z, C.Z - A.Z,
                                                    input.X - A.X, input.Z - A.Z);
                }
                else
                {
                    // We project to z axis, all zs are the same.
                    return LinearSolver.SolveSystem(B.X - A.X, C.X - A.X,
                                                    B.Y - A.Y, C.Y - A.X,
                                                    input.X - A.X, input.Y - A.Y);
                }
            }

            //#endif
        }

        /// <summary>
        /// Converts point from barycentric.
        /// </summary>
        /// <param name="baryCentric">The barycentric coordinate.</param>
        /// <returns>The point in 3D space on triangle.</returns>
        public Vector3f FromBaryCentric(Vector2f baryCentric)
        {
            return baryCentric.X * A + baryCentric.Y * B + (1.0f - baryCentric.X - baryCentric.Y) * C;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Checks if bary centric component is inside triangle.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsBaryCentricInside(Vector2f v)
        {
            if (v.X < 0.0f || v.X > 1.0f) return false;
            if (v.Y < 0.0f || v.Y > 1.0f) return false;
            if (v.X + v.Y > 1.0f) return false;
            return true;
        }


        #endregion

        #region Overrides

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(100);
            builder.Append("Triangle : {");
            builder.Append(A.ToString());
            builder.Append(", ");
            builder.Append(B.ToString());
            builder.Append(", ");
            builder.Append(C.ToString());
            builder.Append("}");
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Triangle3f)
            {
                return this.Equals((Triangle3f)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IArea3f Members

        public float Area
        {
            get 
            { 
                
                return (float)0.5 * ((B - A) ^ (C - A)).Length;
                
            }
        }

        public void Tesselate(float resolution, Storage.Builders.ITriangleBuilder3f builder)
        {
            if (resolution < 0.0f || 
                (resolution > SideA && resolution > SideB && resolution > SideC))
            {

                // We simply append vertices and proceed.
                uint indexBase = builder.AddControlPoints(A, B, C);

                // We also need indices if indexed.
                if (builder.IsIndexed)
                {
                    builder.AddIndexedTriangles(
                        new uint[] { indexBase, indexBase + 1, indexBase + 2 }
                    );
                }
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        #endregion

        #region IOutline3f Members

        public Vector3f Sample(float t)
        {
            if (t >= (float)0.0 && t <= (1.0f/(float)3))
            {
                float inter = (float)3 * t;
                return A * (1.0f - inter) + B * inter;
            }
            else if (t <= ((float)2 / (float)3))
            {
                float inter = (float)3 * t - 1.0f;
                return B * (1.0f - inter) + C * inter;
            }
            else
            {
                float inter = (float)3 * t - (float)2;
                return C * (1.0f - inter) + A * inter;
            }
        }

        public void Sample(float resolution, Storage.Builders.ILineBuilder3f builder)
        {
            if (resolution < 0.0f)
            {
                builder.AddLineStrip(true, A, B, C);
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
            get { return SideA + SideB + SideC; }
        }

        #endregion

        #region IControlPoints3f Members

        public Vector3f[] ControlPoints
        {
            get
            {
                return new Vector3f[] { A, B, C };
            }
            set
            {
                if (value.Length != 3) throw new ArgumentException("Three control points expected.");
                A = value[0];
                B = value[1];
                C = value[2];
            }
        }

        public void SetControlPoints(uint index, Vector3f cp)
        {
            switch(index)
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
                    throw new ArgumentException("Index out of range, must be 0-2 for triangle.");
            }
        }

        public Vector3f GetControlPoint(uint index)
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
                    throw new ArgumentException("Index out of range, must be 0-2 for triangle.");
            }
        }

        #endregion

        #region IControlPointsf Members

        public uint ControlPointCount
        {
            get { return 3; }
        }

        #endregion

        #region IContainsPoint3f Members

        public bool ContainsPoint(Vector3f point)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITransformable3f

        public void Transform(Matrix.Matrix4x4f matrix)
        {
            A = matrix * A;
            B = matrix * B;
            C = matrix * C;
        }

        #endregion

        //#ifdef 3D


        #region IAABoxBoundablef Members

        public AABoxf BoundingAABox
        {
            get
            {
                return AABoxf.FromPoints(A, B, C);
            }
        }

        #endregion

        #region ISphereBoundablef Members

        public Spheref BoundingSphere
        {
            get
            {
                return Spheref.FromPoints(A, B, C);
            }
        }

        #endregion

        //#endif

        #region IEnumerable<Vector3f> Members

        public IEnumerator<Vector3f> GetEnumerator()
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

        #region IEquatable<Triangle3f> Members

        public bool Equals(Triangle3f other)
        {
            if (Vector3f.NearEqual(A, other.A) &&
                Vector3f.NearEqual(B, other.B) &&
                Vector3f.NearEqual(C, other.C))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ICloneable Members

        public Triangle3f Clone()
        {
            return new Triangle3f(A, B, C);
        }

        #endregion

        #region IComparable<Triangle3f> Members

        public int CompareTo(Triangle3f other)
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
    internal class Test_Triangle3f
    {
        [CorrectnessTest]
        public void Construction()
        {
            
        }

       

    }
#endif
}