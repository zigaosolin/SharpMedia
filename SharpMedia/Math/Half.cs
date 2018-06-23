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

namespace SharpMedia.Math
{

    /// <summary>
    /// A "half" precission floating point number.
    /// </summary>
    /// <remarks>Implemented based on http://en.wikipedia.org/wiki/Half_precision.</remarks>
    [Serializable]
    public struct Half : IComparable, IComparable<Half>, IEquatable<Half>
    {
        #region Private Members
        ushort data;
        #endregion

        #region Static Members

        /// <summary>
        /// Gets the na N.
        /// </summary>
        /// <value>The na N.</value>
        public static Half NaN
        {
            get
            {
                return new Half((ushort)((0x1fu << 10) | 0x1u));
            }
        }

        /// <summary>
        /// Gets the positive infinity.
        /// </summary>
        /// <value>The positive infinity.</value>
        public static Half PositiveInfinity
        {
            get
            {
                return new Half((ushort)0x7c00u); 
            }
        }

        /// <summary>
        /// Gets the negative infinity.
        /// </summary>
        /// <value>The negative infinity.</value>
        public static Half NegativeInfinity
        {
            get
            {
                return new Half((ushort)0xfc00u); 
            }
        }

        /// <summary>
        /// Gets the max value.
        /// </summary>
        /// <value>The max value.</value>
        public static Half MaxValue
        {
            get
            {
                return new Half((ushort)0x7bffu);
            }
        }

        /// <summary>
        /// Gets the min value.
        /// </summary>
        /// <value>The min value.</value>
        public static Half MinValue
        {
            get
            {
                return new Half((ushort)0xfbffu);
            }
        }

        #endregion

        #region Private Members

        static unsafe ushort ConvToShort(float data)
        {
            uint sign, exp, mantisa;
            float* _p = &data;
            {
                uint* p = (uint*)_p;

                // We extract values.
                sign = *p & 0x80000000;
                exp = (*p & ~0x80000000) >> 23;
                mantisa = *p & 0x7fffff;
            }

            // Check for special values.
            if (exp == 0)
            {
                if (mantisa == 0)
                {
                    // Zero.
                    return 0;
                }
                else
                {
                    // We return the same non-normalized. Conversion is always to zero.
                    return 0;
                }
            }
            else if (exp == 0xff)
            {
                if (mantisa == 0)
                {
                    return sign != 0 ? Half.NegativeInfinity.Data : Half.PositiveInfinity.Data;
                }
                else
                {
                    return Half.NaN.Data;
                }
            }

            // Real exponent and mantisa.
            int realExp = (int)exp - 127;

            // We check if it can be fitted to half range. We first check if exponent fits.
            if (realExp > 15)
            {
                return sign != 0 ? Half.NegativeInfinity.Data : Half.PositiveInfinity.Data;
            }

            if (realExp < -14)
            {
                return 0;
            }

            // We strip mantisa.
            mantisa >>= 13;

            uint result2 = 0;
            result2 |= sign != 0 ? 0x8000u : 0u;
            result2 |= (uint)(realExp + 15) << 10;
            result2 |= mantisa;

            return (ushort)result2;
        }

        static unsafe float ConvToFloat(ushort data)
        {
            // We extract sign first.
            uint sign = (uint)(data & 0x8000u);

            // We extract exponent (mask sign and shift).
            uint exp = (data & ~0x8000u) >> 10;

            // And extract mantisa.
            uint mantisa = data & 0x03FFu;

            // We check if exponent is special (zero or denormalized).
            if (exp == 0x00)
            {
                if (mantisa == 0)
                {
                    return 0.0f;
                }
                else
                {
                    return 0;
                    /* This should be thrown but we rather cast to 0.
                    throw new NotSupportedException("Denormalized floats not yet supported.");
                    */
                }
            }

            // NaN or infinity.
            if (exp == 0x01)
            {
                if (mantisa == 0)
                {
                    return sign != 0 ? float.NegativeInfinity : float.PositiveInfinity;
                }
                else
                {
                    return float.NaN;
                }
            }

            // We compute real exponent.
            exp = exp - 15;

            float result = 0;

            uint* p = (uint*)&result;

            // We write sign.
            *p |= sign != 0 ? 0x80000000 : 0;

            // We write exponent.
            exp = exp + 127;
            *p |= exp << 23;

            // At last, we also write mantissa.
            *p |= mantisa << 13;
            

            // We return the result.
            return result;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="Half"/> struct.
        /// </summary>
        /// <param name="data">The data.</param>
        private Half(ushort data)
        {
            this.data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Half"/> struct.
        /// </summary>
        /// <param name="data">The data.</param>
        public Half(float data)
        {
            this.data = ConvToShort(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Half"/> struct.
        /// </summary>
        /// <param name="data">The data.</param>
        public Half(double data)
        {
            this.data = ConvToShort((float)data);
        }


        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public ushort Data
        {
            get
            {
                return data;
            }
        }

        #endregion

        #region Conversions


        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpMedia.Math.Half"/> to <see cref="System.Single"/>.
        /// </summary>
        /// <param name="half">The half.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Single(Half half)
        {
            return ConvToFloat(half.data);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Single"/> to <see cref="SharpMedia.Math.Half"/>.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Half(float f)
        {
            return new Half(ConvToShort(f));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Single"/> to <see cref="SharpMedia.Math.Half"/>.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Half(double f)
        {
            return new Half(ConvToShort((float)f));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpMedia.Math.Half"/> to <see cref="System.Double"/>.
        /// </summary>
        /// <param name="half">The half.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Double(Half half)
        {
            return (double)ConvToFloat(half.data);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpMedia.Math.Half"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="half">The half.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Int32(Half half)
        {
            return (int)ConvToFloat(half.data);
        }

        #endregion

        #region Operations

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="h1">The h1.</param>
        /// <param name="h2">The h2.</param>
        /// <returns>The result of the operator.</returns>
        public static Half operator +(Half h1, Half h2)
        {
            return new Half(ConvToShort(ConvToFloat(h1.data) + ConvToFloat(h2.data)));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="h1">The h1.</param>
        /// <param name="h2">The h2.</param>
        /// <returns>The result of the operator.</returns>
        public static Half operator *(Half h1, Half h2)
        {
            return new Half(ConvToShort(ConvToFloat(h1.data) * ConvToFloat(h2.data)));
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="h1">The h1.</param>
        /// <param name="h2">The h2.</param>
        /// <returns>The result of the operator.</returns>
        public static Half operator -(Half h1, Half h2)
        {
            return new Half(ConvToShort(ConvToFloat(h1.data) - ConvToFloat(h2.data)));
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="h1">The h1.</param>
        /// <param name="h2">The h2.</param>
        /// <returns>The result of the operator.</returns>
        public static Half operator /(Half h1, Half h2)
        {
            return new Half(ConvToShort(ConvToFloat(h1.data) / ConvToFloat(h2.data)));
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="h1">The h1.</param>
        /// <param name="h2">The h2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Half h1, Half h2)
        {
            return (float)h1 == (float)h2;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="h1">The h1.</param>
        /// <param name="h2">The h2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Half h1, Half h2)
        {
            return !(h1 == h2);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if obj and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return this == (Half)obj;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return data.GetHashCode();
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return ((float)this).ToString();
        }

        /// <summary>
        /// Nears the equal.
        /// </summary>
        /// <param name="half1">The half1.</param>
        /// <param name="half2">The half2.</param>
        /// <returns></returns>
        public static bool NearEqual(Half half1, Half half2)
        {
            return NearEqual(half1, half1, MathHelper.HalfNearEpsilon);
        }

        /// <summary>
        /// Nears the equal.
        /// </summary>
        /// <param name="half1">The half1.</param>
        /// <param name="half2">The half2.</param>
        /// <param name="eps">The eps.</param>
        /// <returns></returns>
        public static bool NearEqual(Half half1, Half half2, float eps)
        {
            float dif = global::System.Math.Abs(half1 - half2);
            return dif < eps;

        }

        #endregion

        #region IEquatable<Half> Members

        public bool Equals(Half other)
        {
            return ConvToFloat(data).Equals(ConvToFloat(other.data));
        }

        #endregion

        #region IComparable<Half> Members

        public int CompareTo(Half other)
        {
            return ConvToFloat(data).CompareTo(ConvToFloat(other.data));
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is Half)
            {
                return ConvToFloat(data).CompareTo(ConvToFloat(((Half)obj).data));
            }
            throw new ArgumentException("Cannot convert half to non half.");
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class HalfTest
    {
        [CorrectnessTest]
        public void Conv()
        {
            // Simple exponent test.
            Half half = new Half(2.0f);
            Assert.AreEqual(2.0f, (float)half);
            
              
            // Simple mantisa test.
            half = new Half(1.1f);
            Assert.IsTrue(global::System.Math.Abs(1.1f - (float)half) < 0.1f);
        }

        [CorrectnessTest]
        public void Operators()
        {
            Half h1 = (Half)2.0f, h2 = (Half)3.0f;
            Assert.IsTrue(Half.NearEqual(h1 * h2, new Half(6.0f)));
        }

        [CorrectnessTest]
        public void OutOfRange()
        {
            Half h1 = (Half)1e30f, h2 = (Half) (-1e30f), h3 = (Half)1.0e-30f;
            Assert.AreEqual(Half.PositiveInfinity, h1);
            Assert.AreEqual(Half.NegativeInfinity, h2);
            Assert.AreEqual((Half)0.0f, h3);
        }
    }

#endif
}
