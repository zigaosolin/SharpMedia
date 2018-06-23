// This file was generated by TemplateEngine from template source 'Vector3'
// using template 'Vector3i. Do not modify this file directly, modify it from template source.

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
    public struct Vector3i : IComparable, IComparable<Vector3i>, IEquatable<Vector3i>
    {
        #region Public Members

        /// <summary>
        /// A vector processor delegate. Used at many places, like solver etc. Processor
        /// is a kind of Vector3i transformator.
        /// </summary>
        /// <param name="input">Input, as vector.</param>
        /// <returns>Vector output.</returns>
        public delegate Vector3i Processor(Vector3i input);

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public int X;

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public int Y;

        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public int Z;

        #endregion

        #region Static Members

        /// <summary>
        /// A zero vector.
        /// </summary>
        public static Vector3i Zero
        {
            get { return new Vector3i(0, 0, 0); }
        }

        

        /// <summary>
        /// Index access to vector.
        /// </summary>
        /// <param name="index">The index.</param>
        public int this[uint index]
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
        public static Vector3i AxisX
        {
            get { return new Vector3i(1, 0, 0); }
        }

        /// <summary>
        /// Unit vector in axis y.
        /// </summary>
        public static Vector3i AxisY
        {
            get { return new Vector3i(0, 1, 0); }
        }

        /// <summary>
        /// Unit vector in axis z.
        /// </summary>
        public static Vector3i AxisZ
        {
            get { return new Vector3i(0, 0, 1); }
        }

        

        /// <summary>
        /// Multiplication by components.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result of component multiplication.</returns>
        public static Vector3i ComponentMultiply(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        /// <summary>
        /// Division by components.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result of component division.</returns>
        public static Vector3i ComponentDivision(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        

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
            if (obj.GetType() == this.GetType()) return this == (Vector3i)obj;
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
        public static Vector3i operator +(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Overloadable substraction.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result of substraction.</returns>
        public static Vector3i operator -(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Overloadable multiplication, or so called dot product. Result is a scalar.
        /// Dot product can be interpretered as:
        /// <code>this * other == this.Length * other.Length * this.AngleTo(other)</code>
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result of substraction.</returns>
        public static int operator *(Vector3i v1, Vector3i v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        /// <summary>
        /// Vector multiplication with scalar.
        /// </summary>
        /// <param name="v1">The vector.</param>
        /// <param name="f">Scalar.</param>
        /// <returns>Result of operation.</returns>
        public static Vector3i operator *(Vector3i v1, int f)
        {
            return new Vector3i(v1.X * f, v1.Y * f, v1.Z * f);
        }

        /// <summary>
        /// Vector multiplication with scalar.
        /// </summary>
        /// <param name="v1">The vector.</param>
        /// <param name="f">Scalar.</param>
        /// <returns>Result of operation.</returns>
        public static Vector3i operator *(int f, Vector3i v1)
        {
            return new Vector3i(v1.X * f, v1.Y * f, v1.Z * f);
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
        public static Vector3i operator ^(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(v1.Y * v2.Z - v1.Z * v2.Y,
                                v1.Z * v2.X - v1.X * v2.Z,
                                v1.X * v2.Y - v1.Y * v2.X);
        }

        /// <summary>
        /// Negation operator.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>Negated vector (direction flipped).</returns>
        public static Vector3i operator -(Vector3i v)
        {
            return v * -1;
        }

        /// <summary>
        /// Vector divided by scalar.
        /// </summary>
        /// <param name="v1">The vector.</param>
        /// <param name="f">Scalar.</param>
        /// <returns>Result of operation.</returns>
        public static Vector3i operator /(Vector3i v1, int f)
        {
            return new Vector3i(v1.X / f, v1.Y / f, v1.Z / f);
        }


        /// <summary>
        /// Checks if vectors are equal. This check is precise and may not give desirable
        /// result if small errors occur due to precission errors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Are vectorrs equal.</returns>
        public static bool operator ==(Vector3i v1, Vector3i v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z);
        }

        /// <summary>
        /// Returns if vectors are different.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Are vectors different.</returns>
        public static bool operator !=(Vector3i v1, Vector3i v2)
        {
            return !(v1 == v2);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The obsolete x-property getter.
        /// </summary>
        [Obsolete]
        public int x
        {
            get { return X; }
            set { X = value; }
        }

        /// <summary>
        /// The obsolete y-property getter.
        /// </summary>
        [Obsolete]
        public int y
        {
            get { return Y; }
            set { Y = value; }
        }

        /// <summary>
        /// The obsolete z-property getter.
        /// </summary>
        [Obsolete]
        public int z
        {
            get { return Z; }
            set { Z = value; }
        }


        

        /// <summary>
        /// Length squared.
        /// </summary>
        public int Length2
        {
            get { return this * this; }
            set { this = this * value / (this * this); }
        }

        

        

        //#ifdef Vector2ClassName


        /// <summary>
        /// Gets or sets the homohenous point 2D.
        /// </summary>
        /// <value>The point2D.</value>
        public Vector2i Point2D
        {
            get
            {
                if (Z == 0)
                {
                    // Point in infinity.
                    throw new ArithmeticException("Cannot convert a homogenous 3D point to 2D point, it is point in infinity.");
                }
                return new Vector2i(X / Z, X / Z);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = 1;
            }
        }

        /// <summary>
        /// A two dimensional vector.
        /// </summary>
        public Vector2i Vec2
        {
            set { X = value.X; Y = value.Y; Z = 0; }
            get { return new Vector2i(X, Y); }
        }

        //#endif


        /// <summary>
        /// Are all components positive.
        /// </summary>
        public bool AllPositive
        {
            get
            {
                if (X >= 0 && Y >= 0 && Z >= 0) return true;
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
                if (X > 0 && Y > 0 && Z > 0) return true;
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
                if (X <= 0 && Y <= 0 && Z <= 0) return true;
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
                if (X < 0 && Y < 0 && Z < 0) return true;
                return false;
            }
        }

        

        #endregion

        #region Swizzles

        /// <summary>
        /// The XXX swizzle mask.
        /// </summary>
        public Vector3i XXX
        {
            set { X = value.Z; }
            get { return new Vector3i(X, X, X); }
        }

        /// <summary>
        /// The XXY swizzle mask.
        /// </summary>
        public Vector3i XXY
        {
            set { X = value.Y; Y = value.Z; }
            get { return new Vector3i(X, X, Y); }
        }

        /// <summary>
        /// The XXZ swizzle mask.
        /// </summary>
        public Vector3i XXZ
        {
            set { X = value.Y; Z = value.Z; }
            get { return new Vector3i(X, X, Z); }
        }

        /// <summary>
        /// The XYX swizzle mask.
        /// </summary>
        public Vector3i XYX
        {
            set { Y = value.Y; X = value.Z; }
            get { return new Vector3i(X, Y, X); }
        }

        /// <summary>
        /// The XYY swizzle mask.
        /// </summary>
        public Vector3i XYY
        {
            set { X = value.Y; Y = value.Z; }
            get { return new Vector3i(X, Y, Y); }
        }

        /// <summary>
        /// The XYZ swizzle mask.
        /// </summary>
        public Vector3i XYZ
        {
            set { this = value; }
            get { return new Vector3i(X, Y, Z); }
        }

        /// <summary>
        /// The XZX swizzle mask.
        /// </summary>
        public Vector3i XZX
        {
            set { Z = value.Y; X = value.Z; }
            get { return new Vector3i(X, Z, X); }
        }

        /// <summary>
        /// The XZY swizzle mask.
        /// </summary>
        public Vector3i XZY
        {
            set { X = value.X; Y = value.Z; Z = value.Y; }
            get { return new Vector3i(X, Z, Y); }
        }

        /// <summary>
        /// The XZZ swizzle mask.
        /// </summary>
        public Vector3i XZZ
        {
            set { X = value.X; Z = value.Z; }
            get { return new Vector3i(X, Z, Z); }
        }


        /// <summary>
        /// The YXX swizzle mask.
        /// </summary>
        public Vector3i YXX
        {
            set { Y = value.X; X = value.Z; }
            get { return new Vector3i(Y, X, X); }
        }

        /// <summary>
        /// The YXY swizzle mask.
        /// </summary>
        public Vector3i YXY
        {
            set { X = value.Y; Y = value.Z; }
            get { return new Vector3i(Y, X, Y); }
        }

        /// <summary>
        /// The YXZ swizzle mask.
        /// </summary>
        public Vector3i YXZ
        {
            set { Y = value.X; X = value.Y; Z = value.Z; }
            get { return new Vector3i(Y, X, Z); }
        }

        /// <summary>
        /// The YYX swizzle mask.
        /// </summary>
        public Vector3i YYX
        {
            set { Y = value.Y; X = value.Z; }
            get { return new Vector3i(Y, Y, X); }
        }

        /// <summary>
        /// The YYY swizzle mask.
        /// </summary>
        public Vector3i YYY
        {
            set { Y = value.Z; }
            get { return new Vector3i(Y, Y, Y); }
        }

        /// <summary>
        /// The YYZ swizzle mask.
        /// </summary>
        public Vector3i YYZ
        {
            set { Y = value.Y; Z = value.Z; }
            get { return new Vector3i(Y, Y, Z); }
        }

        /// <summary>
        /// The YZX swizzle mask.
        /// </summary>
        public Vector3i YZX
        {
            set { Y = value.X; Z = value.Y; X = value.Z; }
            get { return new Vector3i(Y, Z, X); }
        }

        /// <summary>
        /// The YZY swizzle mask.
        /// </summary>
        public Vector3i YZY
        {
            set { Y = value.Z; Z = value.Y; }
            get { return new Vector3i(Y, Z, Y); }
        }

        /// <summary>
        /// The YZZ swizzle mask.
        /// </summary>
        public Vector3i YZZ
        {
            set { Y = value.X; Z = value.Z; }
            get { return new Vector3i(Y, Z, Z); }
        }

        /// <summary>
        /// The ZXX swizzle mask.
        /// </summary>
        public Vector3i ZXX
        {
            set { Z = value.X; X = value.Z; }
            get { return new Vector3i(Z, X, X); }
        }

        /// <summary>
        /// The ZXY swizzle mask.
        /// </summary>
        public Vector3i ZXY
        {
            set { Z = value.X; X = value.Y; Y = value.Z; }
            get { return new Vector3i(Z, X, Y); }
        }

        /// <summary>
        /// The ZXZ swizzle mask.
        /// </summary>
        public Vector3i ZXZ
        {
            set { X = value.Y; Z = value.Z; }
            get { return new Vector3i(Z, X, Z); }
        }

        /// <summary>
        /// The ZYX swizzle mask.
        /// </summary>
        public Vector3i ZYX
        {
            set { Z = value.X; Y = value.Y; X = value.Z; }
            get { return new Vector3i(Z, Y, X); }
        }

        /// <summary>
        /// The ZYY swizzle mask.
        /// </summary>
        public Vector3i ZYY
        {
            set { Z = value.X; Y = value.Z; }
            get { return new Vector3i(Z, Y, Y); }
        }

        /// <summary>
        /// The ZYZ swizzle mask.
        /// </summary>
        public Vector3i ZYZ
        {
            set { Y = value.Y; Z = value.Z; }
            get { return new Vector3i(Z, Y, Z); }
        }

        /// <summary>
        /// The ZZX swizzle mask.
        /// </summary>
        public Vector3i ZZX
        {
            set { Z = value.Y; X = value.Z; }
            get { return new Vector3i(Z, Z, X); }
        }

        /// <summary>
        /// The ZZY swizzle mask.
        /// </summary>
        public Vector3i ZZY
        {
            set { Y = value.Z; Z = value.Y; }
            get { return new Vector3i(Z, Z, Y); }
        }

        /// <summary>
        /// The ZZZ swizzle mask.
        /// </summary>
        public Vector3i ZZZ
        {
            set { Z = value.Z; }
            get { return new Vector3i(Z, Z, Z); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with arguments.
        /// </summary>
        /// <param name="X">The X component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        public Vector3i(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="r">The vector to copy.</param>
        public Vector3i(Vector3i r)
        {
            X = r.X;
            Y = r.Y;
            Z = r.Z;
        }

        

        //#ifdef Vector2ClassName


        /// <summary>
        /// Contructor with 2D vector and additional dimension.
        /// </summary>
        /// <param name="v">The 2D vector.</param>
        /// <param name="add">Additional dimension z.</param>
        public Vector3i(Vector2i v, int z)
        {
            X = v.X;
            Y = v.Y;
            Z = z;
        }

        //#endif

        #endregion

        #region IEquatable<Vector3i> Members

        public bool Equals(Vector3i other)
        {
            return this == other;
        }

        #endregion

        #region IComparable<Vector3i> Members

        public int CompareTo(Vector3i other)
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
            if (obj.GetType() == this.GetType()) return CompareTo((Vector3i)obj);
            return -1;
        }

        #endregion

        #region Conversions


        //#ifdef Vector2ClassName


        public static explicit operator Vector2i(Vector3i v)
        {
            return new Vector2i(v.X, v.Y);
        }

        //#endif

        //#ifdef Vector4ClassName


        public static implicit operator Vector4i(Vector3i v)
        {
            return new Vector4i(v.X, v.Y, v.Z, 0);
        }

        //#endif

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    
    [TestSuite]
    internal class Test_Vector3i
    {
        protected Vector3i v1 = new Vector3i(1, 2, 3);
        protected Vector3i v2 = new Vector3i(3, -1, -2);
        protected Vector3i v3 = new Vector3i(4, 3, 0);
        protected Vector3i v5 = new Vector3i(1, 0, 0);
        protected Vector3i v6 = new Vector3i(0, 1, 0);

        [CorrectnessTest]
        public void Add() { Assert.AreEqual( v1 + v2, new Vector3i(4, 1, 1)); }
        [CorrectnessTest]
        public void Sub() { Assert.AreEqual( v1 - v2, new Vector3i(-2, 3, 5)); }
        [CorrectnessTest]
        public void Dot() { Assert.AreEqual( v1 * v2, -5); }
        [CorrectnessTest]
        public void Cross() { Assert.AreEqual((v5 ^ v6), new Vector3i(0, 0, 1)); }
        
        [CorrectnessTest]
        public void Length2() { Assert.AreEqual(v3.Length2, 25); }
        [CorrectnessTest]
        public void SwizzleXXX() {  Assert.AreEqual(v1.XXX, new Vector3i(1, 1, 1)); }
        [CorrectnessTest]
        public void SwizzleXZY() {  Assert.AreEqual(v1.XZY, new Vector3i(1, 3, 2)); }
        [CorrectnessTest]
        public void WSwizzleXXX()
        {
            Vector3i v4 = v1;
            v4.XXX = new Vector3i(10, 2, 1);
            Assert.AreEqual(v1, v4);
        }
    }
#endif

}