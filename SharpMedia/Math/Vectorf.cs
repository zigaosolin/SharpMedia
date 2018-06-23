// This file was generated by TemplateEngine from template source 'Vector'
// using template 'Vectorf. Do not modify this file directly, modify it from template source.

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
using SharpMedia.AspectOriented;
using SharpMedia.Testing;

namespace SharpMedia.Math
{

    /// <summary>
    /// A general implementation of n-dimensional vector.
    /// </summary>
    [Serializable]
    public sealed class Vectorf : IEquatable<Vectorf>, ICloneable<Vectorf>
    {
        #region Private Members
        private float[] components;
        #endregion

        #region Properties

        //#ifdef Sqrt


        /// <summary>
        /// The length of vector.
        /// </summary>
        public float Length
        {
            get { return MathHelper.Sqrt(this * this); }
        }

        //#endif

        /// <summary>
        /// The length of vector squared. It is prefered if you can use Length2 instead od Length.
        /// </summary>
        public float Length2
        {
            get { return this * this; }
        }

        /// <summary>
        /// Obtains components.
        /// </summary>
        public float[] Components
        {
            get { return components; }
        }

        /// <summary>
        /// The number of dimensions in vector.
        /// </summary>
        public uint DimensionCount
        {
            get { return (uint)components.Length; }
        }

        /// <summary>
        /// Indexer on vector.
        /// </summary>
        /// <param name="index">The index, must be smaller then Length.</param>
        /// <returns>The element at that</returns>
        public float this[uint index]
        {
            get { return components[index]; }
            set { components[index] = value; }
        }

        //#ifdef Vector2ClassName


        /// <summary>
        /// Cast to 2D vector, taking all furher dimensions away.
        /// </summary>
        public Vector2f Vector2
        {
            get
            {
                if (components.Length < 2) throw new ArgumentException("Not castable to vector 2D.");
                return new Vector2f(components[0], components[1]);
            }
        }

        //#endif
        //#ifdef Vector3ClassName


        /// <summary>
        /// Cast to 3D vector, taking all furher dimensions away.
        /// </summary>
        public Vector3f Vector3
        {
            get
            {
                if (components.Length < 3) throw new ArgumentException("Not castable to vector 3D.");
                return new Vector3f(components[0], components[1], components[2]);
            }
        }

        //#endif
        //#ifdef Vector4ClassName


        /// <summary>
        /// Cast to 4D vector, taking all furher dimensions away.
        /// </summary>
        public Vector4f Vector4
        {
            get
            {
                if (components.Length < 4) throw new ArgumentException("Not castable to vector 4D.");
                return new Vector4f(components[0], components[1], components[2], components[3]);
            }
        }

        //#endif

        #endregion

        #region Operators

        /// <summary>
        /// The generic vector adition. Vectors must be the same dimension.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Vectors added.</returns>
        public static Vectorf operator +([NotNull] Vectorf v1, [NotNull] Vectorf v2)
        {
            // Precheck.
            if (v1.DimensionCount != v2.DimensionCount)
            {
                throw new ArithmeticException("The vectors are not compatible in size, one is " +
                    v1.ToString() + " and the other is " + v2.ToString());
            }

            // Create result.
            uint size = v1.DimensionCount;
            Vectorf result = new Vectorf(size);

            for (uint i = 0; i < size; ++i)
            {
                result.components[i] = v1.components[i] + v2.components[i];
            }

            // Return the result.
            return result;
        }

        /// <summary>
        /// The generic vector substraction. Vectors must be the same dimension.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Vectors added.</returns>
        public static Vectorf operator -([NotNull] Vectorf v1, [NotNull] Vectorf v2)
        {
            // Precheck.
            if (v1.DimensionCount != v2.DimensionCount)
            {
                throw new ArithmeticException("The vectors are not compatible in size, one is " +
                    v1.ToString() + " and the other is " + v2.ToString());
            }

            // Create result.
            uint size = v1.DimensionCount;
            Vectorf result = new Vectorf(size);

            for (uint i = 0; i < size; ++i)
            {
                result.components[i] = v1.components[i] - v2.components[i];
            }

            // Return the result.
            return result;
        }

        /// <summary>
        /// Dot product, defined as a * b = |a| * |b| * cos(phi), where phi is the
        /// angle between vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Scalar result.</returns>
        public static float operator *([NotNull] Vectorf v1, [NotNull] Vectorf v2)
        {
            // Precheck.
            if (v1.DimensionCount != v2.DimensionCount)
            {
                throw new ArithmeticException("The vectors are not compatible in size, one is " +
                    v1.ToString() + " and the other is " + v2.ToString());
            }

            // Create result.
            uint size = v1.DimensionCount;
            float result = 0.0f;

            for (uint i = 0; i < size; ++i)
            {
                result += v1.components[i] * v2.components[i];
            }

            // Return the result.
            return result;
        }


        /// <summary>
        /// Swizzle operation on vector.
        /// </summary>
        /// <param name="mask">The mask may be any length, but indices must not exceed range.
        /// Each element represents the new offset of the element from old vector. The { 0, 2, 1 }
        /// means first element is the offset 0 in this vector, second is the offset 2 and last is
        /// the offset 1.</param>
        /// <returns>Swizzled vector.</returns>
        public Vectorf Swizzle([NotNull] uint[] mask)
        {
            uint size = (uint)components.Length;
            float[] array = new float[size];

            for (uint x = 0; x < size; x++)
            {
                uint data = mask[x];
                if (data >= size)
                {
                    throw new ArithmeticException("The swizzle offset of mask out of range, vector is "
                        + ToString() + " and the mask is " + mask.ToString());
                }

                array[x] = components[data];
            }

            return new Vectorf(array);
        }

        /// <summary>
        /// Compares two vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Are vectors equal.</returns>
        public static bool operator ==([NotNull] Vectorf v1, [NotNull] Vectorf v2)
        {
            uint count = v1.DimensionCount;
            if (count != v2.DimensionCount) return false;

            for (uint i = 0; i < count; i++)
            {
                if (v1[i] != v2[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Compares two vectors.
        /// </summary>
        /// <param name="v1">The first  vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Result of operation.</returns>
        public static bool operator !=([NotNull] Vectorf v1, [NotNull] Vectorf v2)
        {
            return !(v1 == v2);
        }

        #endregion

        #region Overrides

        public override bool Equals([NotNull] object obj)
        {
            if (obj.GetType() == this.GetType()) return this == (Vectorf)obj;
            return false;
        }

        public override string ToString()
        {
            uint dimensionsCount = DimensionCount;
            StringBuilder builder = new StringBuilder("(", (int)dimensionsCount * 2);
            for (uint i = 0; i < (dimensionsCount - 1); i++)
            {
                builder.Append(components[i]);
                builder.Append(",");
            }

            builder.Append(components[dimensionsCount - 1]);
            builder.Append(")");
            return builder.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Static Members

        //#ifdef NearEqual


        /// <summary>
        /// Near equal.
        /// </summary>
        public static bool NearEqual([NotNull] Vectorf v1, [NotNull] Vectorf v2)
        {
            if (v1.DimensionCount != v2.DimensionCount) return false;

            for (uint i = 0; i < v1.DimensionCount; i++)
            {
                if (!MathHelper.NearEqual(v1[i], v2[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Near equal test.
        /// </summary>
        public static bool NearEqual([NotNull] Vectorf v1, [NotNull] Vectorf v2, float eps)
        {
            if (v1.DimensionCount != v2.DimensionCount) return false;

            for (uint i = 0; i < v1.DimensionCount; i++)
            {
                if (!MathHelper.NearEqual(v1[i], v2[i], eps)) return false;
            }
            return true;
        }

        //#endif

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with the vector's dimension. All data is left undefined (set to zero).
        /// </summary>
        /// <param name="n">The dimension of vector.</param>
        public Vectorf(uint n)
        {
            components = new float[n];
        }

        /// <summary>
        /// Initialisation with actual array.
        /// </summary>
        /// <param name="array">The array of coefficients. The components are not cloned for
        /// performance reasons, so it is possible to change components from outside.</param>
        public Vectorf([NotNull] params float[] coef)
        {
            components = coef;
        }

        #endregion

        #region IEquatable<Vectorf> Members

        public bool Equals(Vectorf other)
        {
            return this == other;
        }

        #endregion

        #region ICloneable<Vectorf> Members

        public Vectorf Clone()
        {
            return new Vectorf(components.Clone() as float[]);
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    /// <summary>
    /// A vectord test.
    /// </summary>
    [TestSuite]
    internal class Test_Vectorf
    {
        protected Vectorf v1 = new Vectorf((float)0, (float)1, (float)2);
        protected Vectorf v2 = new Vectorf((float)1, (float)2, (float)3);

        [CorrectnessTest]
        public void Index() 
        {
            Assert.AreEqual(v1[0], (float)0);
            Assert.AreEqual(v1[1], (float)1);
            Assert.AreEqual(v1[2], (float)2); 
        }
        [CorrectnessTest]
        public void Add() { Assert.AreEqual(v1 + v2, new Vectorf(new float[] { (float)1, (float)3, (float)5 })); }
        [CorrectnessTest]
        public void Sub() { Assert.AreEqual(v1 - v2, new Vectorf(new float[] { (float)-1, (float)-1, (float)-1 })); }
        [CorrectnessTest]
        public void Dot() { Assert.AreEqual(v1 * v2, (float)8); }
    }
#endif
}