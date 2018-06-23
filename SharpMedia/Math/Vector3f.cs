// This file was generated by TemplateEngine from template source 'Vector3'
// using template 'Vector3f. Do not modify this file directly, modify it from template source.

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
using SharpMedia.Testing;

namespace SharpMedia.Math
{
    /// <summary>
    /// A three dimensional vector type.
    /// </summary>
    [Serializable]
    public struct Vector3f : IComparable, IComparable<Vector3f>, IEquatable<Vector3f>
    {
        #region Public Members

        /// <summary>
        /// A vector processor delegate. Used at many places, like solver etc. Processor
        /// is a kind of Vector3f transformator.
        /// </summary>
        /// <param name="input">Input, as vector.</param>
        /// <returns>Vector output.</returns>
        public delegate Vector3f Processor(Vector3f input);

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public float Z;

        #endregion

        #region Static Members

        /// <summary>
        /// A zero vector.
        /// </summary>
        public static Vector3f Zero
        {
            get { return new Vector3f(0.0f, 0.0f, 0.0f); }
        }

        //#ifdef NaN


        /// <summary>
        /// The NaN vector.
        /// </summary>
        public static Vector3f NaN
        {
            get { return new Vector3f(float.NaN, float.NaN, float.NaN); }
        }

        //#ifdef IsNaN


        /// <summary>
        /// Is the vector NaN. Vector is NaN, if any component is NaN.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsNaN(Vector3f v)
        {
            return float.IsNaN(v.X) || float.IsNaN(v.Y) || float.IsNaN(v.Z);
        }

        //#endif

        //#endif

        /// <summary>
        /// Index access to vector.
        /// </summary>
        /// <param name="index">The index.</param>
        public float this[uint index]
        {
            get
            {
                if (index == 0) return X;
                if (index == 1) return Y;
                if (index == 2) return Z;
                throw new ArgumentException("Invalid index for 3D vector.");
            }
            set
            {
                if (index == 0) X = value;
                else if (index == 1) Y = value;
                else if (index == 2) Z = value;
                else throw new ArgumentException("Invalid index for 3D vector.");
            }
        }

        /// <summary>
        /// Unit vector in axis X.
        /// </summary>
        public static Vector3f AxisX
        {
            get { return new Vector3f(1.0f, 0.0f, 0.0f); }
        }

        /// <summary>
        /// Unit vector in axis y.
        /// </summary>
        public static Vector3f AxisY
        {
            get { return new Vector3f(0.0f, 1.0f, 0.0f); }
        }

        /// <summary>
        /// Unit vector in axis z.
        /// </summary>
        public static Vector3f AxisZ
        {
            get { return new Vector3f(0.0f, 0.0f, 1.0f); }
        }

        //#ifdef UniformRandomSupply


        /// <summary>
        /// Unit random vector property.
        /// </summary>
        public static Vector3f UnitRandom
        {
            get
            {
                Vector3f vec = new Vector3f((float)MathHelper.UniformRandom(), (float)MathHelper.UniformRandom(), (float)MathHelper.UniformRandom());
                return vec.Normal;
            }
        }

        //#endif

        /// <summary>
        /// Multiplication by components.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result of component multiplication.</returns>
        public static Vector3f ComponentMultiply(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        /// <summary>
        /// Division by components.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result of component division.</returns>
        public static Vector3f ComponentDivision(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        //#ifdef NearEqual


        /// <summary>
        /// Checks if vectors are nearly equal.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <param name="eps">The maximum difference in one component.</param>
        /// <returns>Are vectors equal indicator.</returns>
        public static bool NearEqual(Vector3f v1, Vector3f v2, float eps)
        {
            if (!MathHelper.NearEqual(v2.X, v1.X, eps)) return false;
            if (!MathHelper.NearEqual(v2.Y, v1.Y, eps)) return false;
            if (!MathHelper.NearEqual(v2.Z, v1.Z, eps)) return false;
            return true;
        }


        /// <summary>
        /// Near equal using default epsilon.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result indicator.</returns>
        public static bool NearEqual(Vector3f v1, Vector3f v2)
        {
            if (!MathHelper.NearEqual(v1.X, v2.X)) return false;
            if (!MathHelper.NearEqual(v1.Y, v2.Y)) return false;
            if (!MathHelper.NearEqual(v1.Z, v2.Z)) return false;
            return true;
        }

        //#endif

        #endregion

        #region Overrides

        /// <summary>
        /// Outputs vector in (x,y) form.
        /// </summary>
        /// <returns>Vector converted to string.</returns>
        public override string ToString()
        {
            return "(" + X.ToString() + " ," + Y.ToString() + " ," + Z.ToString() + ")";
        }

        /// <summary>
        /// Checks if objects equal.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>Do objects equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType()) return this == (Vector3f)obj;
            return false;
        }

        /// <summary>
        /// Obtain the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Overloadable addition.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result of addition.</returns>
        public static Vector3f operator +(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Overloadable substraction.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result of substraction.</returns>
        public static Vector3f operator -(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Overloadable multiplication, or so called dot product. Result is a scalar.
        /// Dot product can be interpretered as:
        /// <code>this * other == this.Length * other.Length * this.AngleTo(other)</code>
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result of substraction.</returns>
        public static float operator *(Vector3f v1, Vector3f v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        /// <summary>
        /// Vector multiplication with scalar.
        /// </summary>
        /// <param name="v1">The vector.</param>
        /// <param name="f">Scalar.</param>
        /// <returns>Result of operation.</returns>
        public static Vector3f operator *(Vector3f v1, float f)
        {
            return new Vector3f(v1.X * f, v1.Y * f, v1.Z * f);
        }

        /// <summary>
        /// Vector multiplication with scalar.
        /// </summary>
        /// <param name="v1">The vector.</param>
        /// <param name="f">Scalar.</param>
        /// <returns>Result of operation.</returns>
        public static Vector3f operator *(float f, Vector3f v1)
        {
            return new Vector3f(v1.X * f, v1.Y * f, v1.Z * f);
        }

        /// <summary>
        /// Cross product of two vectors. The result is vector with the length
        /// of surface of paralelogram, defined by two vectors, and direction 
        /// is perpendicular to both vectors.
        /// <remarks>
        /// The <c>v1 ^ v2</c> is not the same as <c>v2 ^ v1</c>, it is <c>- v2 ^ v1</c> 
        /// </remarks>
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>The result.</returns>
        public static Vector3f operator ^(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.Y * v2.Z - v1.Z * v2.Y,
                                v1.Z * v2.X - v1.X * v2.Z,
                                v1.X * v2.Y - v1.Y * v2.X);
        }

        /// <summary>
        /// Negation operator.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>Negated vector (direction flipped).</returns>
        public static Vector3f operator -(Vector3f v)
        {
            return v * -1.0f;
        }

        /// <summary>
        /// Vector divided by scalar.
        /// </summary>
        /// <param name="v1">The vector.</param>
        /// <param name="f">Scalar.</param>
        /// <returns>Result of operation.</returns>
        public static Vector3f operator /(Vector3f v1, float f)
        {
            return new Vector3f(v1.X / f, v1.Y / f, v1.Z / f);
        }


        /// <summary>
        /// Checks if vectors are equal. This check is precise and may not give desirable
        /// result if small errors occur due to precission errors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Are vectorrs equal.</returns>
        public static bool operator ==(Vector3f v1, Vector3f v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z);
        }

        /// <summary>
        /// Returns if vectors are different.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Are vectors different.</returns>
        public static bool operator !=(Vector3f v1, Vector3f v2)
        {
            return !(v1 == v2);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The obsolete x-property getter.
        /// </summary>
        [Obsolete]
        public float x
        {
            get { return X; }
            set { X = value; }
        }

        /// <summary>
        /// The obsolete y-property getter.
        /// </summary>
        [Obsolete]
        public float y
        {
            get { return Y; }
            set { Y = value; }
        }

        /// <summary>
        /// The obsolete z-property getter.
        /// </summary>
        [Obsolete]
        public float z
        {
            get { return Z; }
            set { Z = value; }
        }


        //#ifdef Sqrt


        /// <summary>
        /// The length of vector.
        /// </summary>
        public float Length
        {
            get { return MathHelper.Sqrt((this * this)); }
            set { this = this * value / MathHelper.Sqrt((this * this)); }
        }

        /// <summary>
        /// Normalized direction.
        /// </summary>
        public Vector3f Direction
        {
            get { return this / Length; }
            set { this = value * Length; }
        }

        /// <summary>
        /// Normal vector, always length of 1.
        /// </summary>
        public Vector3f Normal
        {
            get { return this / Length; }
        }

        //#endif

        /// <summary>
        /// Length squared.
        /// </summary>
        public float Length2
        {
            get { return this * this; }
            set { this = this * value / (this * this); }
        }

        //#ifdef CylindricalClassName


        /// <summary>
        /// Conversion to cylindrical coordinates.
        /// </summary>
        public Cylindricalf Cylindrical
        {
            get
            {
                Cylindricalf c = new Cylindricalf();
                c.Cartesian = this;
                return c;
            }

            set
            {
                this = value.Cartesian;
            }
        }

        //#endif

        //#ifdef SphericalClassName


        /// <summary>
        /// Conversion to spherical coordinates.
        /// </summary>
        public Sphericalf Spherical
        {
            get
            {
                Sphericalf s = new Sphericalf();
                s.Cartesian = this;
                return s;
            }

            set
            {
                this = value.Cartesian;
            }
        }

        //#endif

        //#ifdef Vector2ClassName


        /// <summary>
        /// Gets or sets the homohenous point 2D.
        /// </summary>
        /// <value>The point2D.</value>
        public Vector2f Point2D
        {
            get
            {
                if (Z == 0.0f)
                {
                    // Point in infinity.
                    throw new ArithmeticException("Cannot convert a homogenous 3D point to 2D point, it is point in infinity.");
                }
                return new Vector2f(X / Z, X / Z);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = 1.0f;
            }
        }

        /// <summary>
        /// A two dimensional vector.
        /// </summary>
        public Vector2f Vec2
        {
            set { X = value.X; Y = value.Y; Z = 0.0f; }
            get { return new Vector2f(X, Y); }
        }

        //#endif


        /// <summary>
        /// Are all components positive.
        /// </summary>
        public bool AllPositive
        {
            get
            {
                if (X >= 0.0f && Y >= 0.0f && Z >= 0.0f) return true;
                return false;
            }
        }

        /// <summary>
        /// Are all components positive, strictly speaking.
        /// </summary>
        public bool AllPositiveStrict
        {
            get
            {
                if (X > 0.0f && Y > 0.0f && Z > 0.0f) return true;
                return false;
            }
        }

        /// <summary>
        /// Are all components negative.
        /// </summary>
        public bool AllNegative
        {
            get
            {
                if (X <= 0.0f && Y <= 0.0f && Z <= 0.0f) return true;
                return false;
            }
        }

        /// <summary>
        /// Are all components negative.
        /// </summary>
        public bool AllNegativeStrict
        {
            get
            {
                if (X < 0.0f && Y < 0.0f && Z < 0.0f) return true;
                return false;
            }
        }

        

        #endregion

        #region Swizzles

        /// <summary>
        /// The XXX swizzle mask.
        /// </summary>
        public Vector3f XXX
        {
            set { X = value.Z; }
            get { return new Vector3f(X, X, X); }
        }

        /// <summary>
        /// The XXY swizzle mask.
        /// </summary>
        public Vector3f XXY
        {
            set { X = value.Y; Y = value.Z; }
            get { return new Vector3f(X, X, Y); }
        }

        /// <summary>
        /// The XXZ swizzle mask.
        /// </summary>
        public Vector3f XXZ
        {
            set { X = value.Y; Z = value.Z; }
            get { return new Vector3f(X, X, Z); }
        }

        /// <summary>
        /// The XYX swizzle mask.
        /// </summary>
        public Vector3f XYX
        {
            set { Y = value.Y; X = value.Z; }
            get { return new Vector3f(X, Y, X); }
        }

        /// <summary>
        /// The XYY swizzle mask.
        /// </summary>
        public Vector3f XYY
        {
            set { X = value.Y; Y = value.Z; }
            get { return new Vector3f(X, Y, Y); }
        }

        /// <summary>
        /// The XYZ swizzle mask.
        /// </summary>
        public Vector3f XYZ
        {
            set { this = value; }
            get { return new Vector3f(X, Y, Z); }
        }

        /// <summary>
        /// The XZX swizzle mask.
        /// </summary>
        public Vector3f XZX
        {
            set { Z = value.Y; X = value.Z; }
            get { return new Vector3f(X, Z, X); }
        }

        /// <summary>
        /// The XZY swizzle mask.
        /// </summary>
        public Vector3f XZY
        {
            set { X = value.X; Y = value.Z; Z = value.Y; }
            get { return new Vector3f(X, Z, Y); }
        }

        /// <summary>
        /// The XZZ swizzle mask.
        /// </summary>
        public Vector3f XZZ
        {
            set { X = value.X; Z = value.Z; }
            get { return new Vector3f(X, Z, Z); }
        }


        /// <summary>
        /// The YXX swizzle mask.
        /// </summary>
        public Vector3f YXX
        {
            set { Y = value.X; X = value.Z; }
            get { return new Vector3f(Y, X, X); }
        }

        /// <summary>
        /// The YXY swizzle mask.
        /// </summary>
        public Vector3f YXY
        {
            set { X = value.Y; Y = value.Z; }
            get { return new Vector3f(Y, X, Y); }
        }

        /// <summary>
        /// The YXZ swizzle mask.
        /// </summary>
        public Vector3f YXZ
        {
            set { Y = value.X; X = value.Y; Z = value.Z; }
            get { return new Vector3f(Y, X, Z); }
        }

        /// <summary>
        /// The YYX swizzle mask.
        /// </summary>
        public Vector3f YYX
        {
            set { Y = value.Y; X = value.Z; }
            get { return new Vector3f(Y, Y, X); }
        }

        /// <summary>
        /// The YYY swizzle mask.
        /// </summary>
        public Vector3f YYY
        {
            set { Y = value.Z; }
            get { return new Vector3f(Y, Y, Y); }
        }

        /// <summary>
        /// The YYZ swizzle mask.
        /// </summary>
        public Vector3f YYZ
        {
            set { Y = value.Y; Z = value.Z; }
            get { return new Vector3f(Y, Y, Z); }
        }

        /// <summary>
        /// The YZX swizzle mask.
        /// </summary>
        public Vector3f YZX
        {
            set { Y = value.X; Z = value.Y; X = value.Z; }
            get { return new Vector3f(Y, Z, X); }
        }

        /// <summary>
        /// The YZY swizzle mask.
        /// </summary>
        public Vector3f YZY
        {
            set { Y = value.Z; Z = value.Y; }
            get { return new Vector3f(Y, Z, Y); }
        }

        /// <summary>
        /// The YZZ swizzle mask.
        /// </summary>
        public Vector3f YZZ
        {
            set { Y = value.X; Z = value.Z; }
            get { return new Vector3f(Y, Z, Z); }
        }

        /// <summary>
        /// The ZXX swizzle mask.
        /// </summary>
        public Vector3f ZXX
        {
            set { Z = value.X; X = value.Z; }
            get { return new Vector3f(Z, X, X); }
        }

        /// <summary>
        /// The ZXY swizzle mask.
        /// </summary>
        public Vector3f ZXY
        {
            set { Z = value.X; X = value.Y; Y = value.Z; }
            get { return new Vector3f(Z, X, Y); }
        }

        /// <summary>
        /// The ZXZ swizzle mask.
        /// </summary>
        public Vector3f ZXZ
        {
            set { X = value.Y; Z = value.Z; }
            get { return new Vector3f(Z, X, Z); }
        }

        /// <summary>
        /// The ZYX swizzle mask.
        /// </summary>
        public Vector3f ZYX
        {
            set { Z = value.X; Y = value.Y; X = value.Z; }
            get { return new Vector3f(Z, Y, X); }
        }

        /// <summary>
        /// The ZYY swizzle mask.
        /// </summary>
        public Vector3f ZYY
        {
            set { Z = value.X; Y = value.Z; }
            get { return new Vector3f(Z, Y, Y); }
        }

        /// <summary>
        /// The ZYZ swizzle mask.
        /// </summary>
        public Vector3f ZYZ
        {
            set { Y = value.Y; Z = value.Z; }
            get { return new Vector3f(Z, Y, Z); }
        }

        /// <summary>
        /// The ZZX swizzle mask.
        /// </summary>
        public Vector3f ZZX
        {
            set { Z = value.Y; X = value.Z; }
            get { return new Vector3f(Z, Z, X); }
        }

        /// <summary>
        /// The ZZY swizzle mask.
        /// </summary>
        public Vector3f ZZY
        {
            set { Y = value.Z; Z = value.Y; }
            get { return new Vector3f(Z, Z, Y); }
        }

        /// <summary>
        /// The ZZZ swizzle mask.
        /// </summary>
        public Vector3f ZZZ
        {
            set { Z = value.Z; }
            get { return new Vector3f(Z, Z, Z); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with arguments.
        /// </summary>
        /// <param name="X">The X component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        public Vector3f(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="r">The vector to copy.</param>
        public Vector3f(Vector3f r)
        {
            X = r.X;
            Y = r.Y;
            Z = r.Z;
        }

        //#ifdef Sqrt


        /// <summary>
        /// Construction with magnitude and direction.
        /// </summary>
        /// <param name="magnitude">The magnitude, or length of vector.</param>
        /// <param name="direction">Direction of vector.</param>
        public Vector3f(float magnitude, Vector3f direction)
        {
            this = magnitude * direction.Normal;
        }

        //#endif

        //#ifdef Vector2ClassName


        /// <summary>
        /// Contructor with 2D vector and additional dimension.
        /// </summary>
        /// <param name="v">The 2D vector.</param>
        /// <param name="add">Additional dimension z.</param>
        public Vector3f(Vector2f v, float z)
        {
            X = v.X;
            Y = v.Y;
            Z = z;
        }

        //#endif

        #endregion

        #region IEquatable<Vector3f> Members

        public bool Equals(Vector3f other)
        {
            return this == other;
        }

        #endregion

        #region IComparable<Vector3f> Members

        public int CompareTo(Vector3f other)
        {
            int cmp = X.CompareTo(other.X);
            if (cmp != 0) return cmp;
            cmp = Y.CompareTo(other.Y);
            if (cmp != 0) return cmp;
            return Z.CompareTo(other.Z);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj.GetType() == this.GetType()) return CompareTo((Vector3f)obj);
            return -1;
        }

        #endregion

        #region Conversions


        //#ifdef Vector2ClassName


        public static explicit operator Vector2f(Vector3f v)
        {
            return new Vector2f(v.X, v.Y);
        }

        //#endif

        //#ifdef Vector4ClassName


        public static implicit operator Vector4f(Vector3f v)
        {
            return new Vector4f(v.X, v.Y, v.Z, 0.0f);
        }

        //#endif

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    
    [TestSuite]
    internal class Test_Vector3f
    {
        protected Vector3f v1 = new Vector3f(1.0f, 2, 3);
        protected Vector3f v2 = new Vector3f(3, -1.0f, -2);
        protected Vector3f v3 = new Vector3f(4, 3, 0.0f);
        protected Vector3f v5 = new Vector3f(1.0f, 0.0f, 0.0f);
        protected Vector3f v6 = new Vector3f(0.0f, 1.0f, 0.0f);

        [CorrectnessTest]
        public void Add() { Assert.AreEqual( v1 + v2, new Vector3f(4, 1.0f, 1.0f)); }
        [CorrectnessTest]
        public void Sub() { Assert.AreEqual( v1 - v2, new Vector3f(-2, 3, 5)); }
        [CorrectnessTest]
        public void Dot() { Assert.AreEqual( v1 * v2, -5); }
        [CorrectnessTest]
        public void Cross() { Assert.AreEqual((v5 ^ v6), new Vector3f(0.0f, 0.0f, 1.0f)); }
        //#ifdef Sqrt

        [CorrectnessTest]
        public void Length() { Assert.AreEqual(v3.Length, 5); }
        [CorrectnessTest]
        public void DirectionNormal() { Assert.AreEqual(v1.Normal, v1.Direction); }
        //#endif
        [CorrectnessTest]
        public void Length2() { Assert.AreEqual(v3.Length2, 25); }
        [CorrectnessTest]
        public void SwizzleXXX() {  Assert.AreEqual(v1.XXX, new Vector3f(1.0f, 1.0f, 1.0f)); }
        [CorrectnessTest]
        public void SwizzleXZY() {  Assert.AreEqual(v1.XZY, new Vector3f(1.0f, 3, 2)); }
        [CorrectnessTest]
        public void WSwizzleXXX()
        {
            Vector3f v4 = v1;
            v4.XXX = new Vector3f(10.0f, 2, 1.0f);
            Assert.AreEqual(v1, v4);
        }
    }
#endif

}
