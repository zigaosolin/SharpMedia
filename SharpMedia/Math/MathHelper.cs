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

namespace SharpMedia.Math
{
    /// <summary>
    /// Math Helper implementation.
    /// </summary>
    public static class MathHelper
    {
        #region Half Version

        /// <summary>
        /// The half near epsilon.
        /// </summary>
        public const float HalfNearEpsilon = 1e-3f;

        #endregion

        #region Float Version

        /// <summary>
        /// The PI math constant.
        /// </summary>
        public const float PIf = 3.141592653589793238462643383279502884f;

        /// <summary>
        /// The E constant.
        /// </summary>
        public const float Ef = 2.71828182845904523536028747135266249775724709369995f;

        /// <summary>
        /// Sometimes needed Sqrt(2.0).
        /// </summary>
        public const float Sqrt2f = 1.4142135623730950488016887242097f;

        /// <summary>
        /// A float near equal epsilon constant.
        /// </summary>
        public static float FloatNearEqualEpsilon = 0.0001f;


        /// <summary>
        /// Generates a random number in range [0,1].
        /// </summary>
        /// <returns>The random number.</returns>
        public static float UniformRandomf()
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// Sins the cos.
        /// </summary>
        /// <remarks>May be optimized in future.</remarks>
        /// <param name="x">The x.</param>
        /// <param name="sin">The sin.</param>
        /// <param name="cos">The cos.</param>
        public static void SinCos(float x, out float sin, out float cos)
        {
            sin = (float)global::System.Math.Sin((double)x);
            cos = (float)global::System.Math.Cos((double)x);
        }

        /// <summary>
        /// Arcus sinus.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float ACos(float x)
        {
            return (float)global::System.Math.Acos(x);
        }

        /// <summary>
        /// Arcus sinus.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float ASin(float x)
        {
            return (float)global::System.Math.Asin(x);
        }

        /// <summary>
        /// Arcus tangens.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float ATan(float x)
        {
            return (float)global::System.Math.Atan(x);
        }

        /// <summary>
        /// Arcus tangens.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float ATan(float y, float x)
        {
            return (float)global::System.Math.Atan2(y, x);
        }

        /// <summary>
        /// Computed sin(x).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static float Sin(float x)
        {
            return (float)global::System.Math.Sin((double)x);
        }

        /// <summary>
        /// A logarithem.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Ln(float x)
        {
            return (float)global::System.Math.Log(x);
        }

        /// <summary>
        /// Computes cos(x).
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static float Cos(float x)
        {
            return (float)global::System.Math.Cos((double)x);
        }

        /// <summary>
        /// Computes tan(x).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static float Tan(float x)
        {
            return (float)global::System.Math.Tan((double)x);
        }

        /// <summary>
        /// Computes e^x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static float Expf(int x)
        {
            return (float)global::System.Math.Exp((double)x);
        }

        /// <summary>
        /// Computes e^x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static float Expf(uint x)
        {
            return (float)global::System.Math.Exp((double)x);
        }

        /// <summary>
        /// Computes e^x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static float Exp(float x)
        {
            return (float)global::System.Math.Exp((double)x);
        }

        /// <summary>
        /// Computes square root.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Sqrt(float x)
        {
            return (float)global::System.Math.Sqrt(x);
        }


        /// <summary>
        /// Near equal doubles.
        /// </summary>
        /// <param name="x">The first double.</param>
        /// <param name="y">The second double.</param>
        /// <param name="eps">The epsilon, or maximum difference.</param>
        /// <returns>Indicator.</returns>
        public static bool NearEqual(float x, float y, float eps)
        {
            return ((float)global::System.Math.Abs(x - y) < eps);
        }

        /// <summary>
        /// Near equal, with default epsilon.
        /// </summary>
        /// <param name="x">The first.</param>
        /// <param name="y">The second.</param>
        /// <returns>Are values almost equal.</returns>
        public static bool NearEqual(float x, float y)
        {
            return NearEqual(x, y, FloatNearEqualEpsilon);
        }

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static float Max(float m1, float m2)
        {
            return m1 > m2 ? m1 : m2;
        }

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static float Max(float m1, float m2, float m3)
        {
            return m1 > m2 ? (m1 > m3 ? m1 : m3) : (m2 > m3 ? m2 : m3);
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static float Min(float m1, float m2)
        {
            return m1 < m2 ? m1 : m2;
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static float Min(float m1, float m2, float m3)
        {
            return m1 < m2 ? (m1 < m3 ? m1 : m3) : (m2 < m3 ? m2 : m3);
        }


        /// <summary>
        /// Clamps the specified x in range [min, max].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static float Clamp(float x, float min, float max)
        {
            return x < min ? min : (x > max ? max : x);
        }

        /// <summary>
        /// Clamps the specified x to range [0,1].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static float Clamp(float x)
        {
            return Clamp(x, 0.0f, 1.0f);
        }

        /// <summary>
        /// Conversion to degrees
        /// </summary>
        /// <param name="rad">The radians.</param>
        /// <returns></returns>
        public static float ToDegrees(float rad)
        {
            return rad * 180.0f / (float)PI;
        }

        /// <summary>
        /// Conversion to radians.
        /// </summary>
        /// <param name="deg">Degrees.</param>
        /// <returns></returns>
        public static float ToRadians(float deg)
        {
            return deg * (float)PI / 180.0f;
        }

        /// <summary>
        /// Computes a^b.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static float Pow(float a, float b)
        {
            return (float)global::System.Math.Pow(a, b);
        }

        /// <summary>
        /// Absolute value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Abs(float x)
        {
            return x > 0.0f ? x : -x;
        }

        /// <summary>
        /// Floor of value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Floor(float x)
        {
            return (float)global::System.Math.Floor((double)x);
        }

        #endregion

        #region Double Version

        /// <summary>
        /// The default near equal epsilon.
        /// </summary>
        public static double DoubleNearEqualEpsilon = 0.00001;

        /// <summary>
        /// The PI math constant.
        /// </summary>
        public const double PI = 3.141592653589793238462643383279502884;

        /// <summary>
        /// The E constant.
        /// </summary>
        public const double E = 2.71828182845904523536028747135266249775724709369995;

        /// <summary>
        /// Sometimes needed Sqrt(2.0).
        /// </summary>
        public const double Sqrt2 =  1.4142135623730950488016887242097;

        /// <summary>
        /// Random number generator.
        /// </summary>
        private static global::System.Random random = new global::System.Random();

        /// other constants may be found under scientific constants (those are not scientific).

        /// <summary>
        /// Near equal doubles.
        /// </summary>
        /// <param name="x">The first double.</param>
        /// <param name="y">The second double.</param>
        /// <param name="eps">The epsilon, or maximum difference.</param>
        /// <returns>Indicator.</returns>
        public static bool NearEqual(double x, double y, double eps)
        {
            return (global::System.Math.Abs(x - y) < eps);
        }

        /// <summary>
        /// Near equal, with default epsilon.
        /// </summary>
        /// <param name="x">The first.</param>
        /// <param name="y">The second.</param>
        /// <returns>Are values almost equal.</returns>
        public static bool NearEqual(double x, double y)
        {
            return NearEqual(x, y, DoubleNearEqualEpsilon);
        }


        /// <summary>
        /// Generates a random number in range [0,1].
        /// </summary>
        /// <returns>The random number.</returns>
        public static double UniformRandom()
        {
            return random.NextDouble();
        }

        /// <summary>
        /// Sins the cos.
        /// </summary>
        /// <remarks>May be optimized in future.</remarks>
        /// <param name="x">The x.</param>
        /// <param name="sin">The sin.</param>
        /// <param name="cos">The cos.</param>
        public static void SinCos(double x, out double sin, out double cos)
        {
            sin = global::System.Math.Sin(x);
            cos = global::System.Math.Cos(x);
        }

        /// <summary>
        /// Arcus sinus.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double ACos(double x)
        {
            return global::System.Math.Acos(x);
        }

        /// <summary>
        /// Arcus sinus.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double ASin(double x)
        {
            return global::System.Math.Asin(x);
        }

        /// <summary>
        /// Computed sin(x).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static double Sin(double x)
        {
            return global::System.Math.Sin(x);
        }

        /// <summary>
        /// A logarithem.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Ln(double x)
        {
            return global::System.Math.Log(x);
        }

        /// <summary>
        /// Computes cos(x).
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static double Cos(double x)
        {
            return global::System.Math.Cos(x);
        }

        /// <summary>
        /// Computes tan(x).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static double Tan(double x)
        {
            return global::System.Math.Tan(x);
        }

        /// <summary>
        /// Computes atan(x).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static double ATan(double x)
        {
            return global::System.Math.Atan(x);
        }

        /// <summary>
        /// Computes atan(y, x).
        /// </summary>
        /// <returns></returns>
        public static double ATan(double y, double x)
        {
            return global::System.Math.Atan2(y, x);
        }

        /// <summary>
        /// Computes e^x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static double Exp(int x)
        {
            return global::System.Math.Exp((double)x);
        }

        /// <summary>
        /// Computes e^x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static double Exp(uint x)
        {
            return global::System.Math.Exp((double)x);
        }

        /// <summary>
        /// Computes e^x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static double Exp(double x)
        {
            return global::System.Math.Exp(x);
        }

        /// <summary>
        /// Computes square root.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Sqrt(double x)
        {
            return global::System.Math.Sqrt(x);
        }

        /// <summary>
        /// Returns the binomial coefficient.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        public static double BinomialCoefficient(uint n, uint k)
        {
            return Statistics.Combinatorics.Combinations(n, k);
        }

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static double Max(double m1, double m2)
        {
            return m1 > m2 ? m1 : m2;
        }

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static double Max(double m1, double m2, double m3)
        {
            return m1 > m2 ? (m1 > m3 ? m1 : m3) : (m2 > m3 ? m2 : m3);
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static double Min(double m1, double m2)
        {
            return m1 < m2 ? m1 : m2;
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static double Min(double m1, double m2, double m3)
        {
            return m1 < m2 ? (m1 < m3 ? m1 : m3) : (m2 < m3 ? m2 : m3);
        }

        /// <summary>
        /// Clamps the specified x in range [min, max].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static double Clamp(double x, double min, double max)
        {
            return x < min ? min : (x > max ? max : x);
        }

        /// <summary>
        /// Clamps the specified x to range [0,1].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static double Clamp(double x)
        {
            return Clamp(x, 0.0, 1.0);
        }

        /// <summary>
        /// Conversion to degrees
        /// </summary>
        /// <param name="rad">The radians.</param>
        /// <returns></returns>
        public static double ToDegrees(double rad)
        {
            return rad * 180.0 / PI;
        }

        /// <summary>
        /// Conversion to radians.
        /// </summary>
        /// <param name="deg">Degrees.</param>
        /// <returns></returns>
        public static double ToRadians(double deg)
        {
            return deg * PI / 180.0;
        }

        /// <summary>
        /// Computes a^b.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static double Pow(double a, double b)
        {
            return global::System.Math.Pow(a, b);
        }

        /// <summary>
        /// Absolute value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Abs(double x)
        {
            return x > 0.0 ? x : -x;
        }

        /// <summary>
        /// Floor of value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Floor(double x)
        {
            return global::System.Math.Floor(x);
        }

        #endregion

        #region Complex

        /// <summary>
        /// Computes e^(x*i).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static Complexd ExpI(double x)
        {
            double t1, t2;
            MathHelper.SinCos(x, out t1, out t2);
            return new Complexd(t2, t1);
        }

        /// <summary>
        /// Computes e^(x*i).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static Complexf ExpI(float x)
        {
            float t1, t2;
            MathHelper.SinCos(x, out t1, out t2);
            return new Complexf(t2, t1);
        }

        #endregion

        #region Integer

        /// <summary>
        /// The logarithm of integer 2.
        /// </summary>
        /// <param name="x">The x to check.</param>
        /// <returns>Logarithm.</returns>
        public static uint Log2(uint x)
        {
            return (uint)Log2((int)x);
        }

        /// <summary>
        /// Reverses bits of x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <remarks>The binary 0100100011000011 is revered to 1100001100010010. </remarks>
        /// <returns></returns>
        public static uint ReverseBits(uint x)
        {
            // Reverse it.
            x = ((x & 0xaaaaaaaa) >> 1) | ((x & 0x55555555) << 1);
            x = ((x & 0xcccccccc) >> 2) | ((x & 0x33333333) << 2);
            x = ((x & 0xf0f0f0f0) >> 4) | ((x & 0x0f0f0f0f) << 4);
            x = ((x & 0xff00ff00) >> 8) | ((x & 0x00ff00ff) << 8);
            x = ((x & 0xffff0000) >> 16) | ((x & 0x0000ffff) << 16);

            return x;
        }

        /// <summary>
        /// Reverses the bits.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="bitCount">The bit count.</param>
        /// <returns></returns>
        public static uint ReverseBits(uint x, uint bitCount)
        {
            return ReverseBits(x) >> (int)(32 - bitCount);
        }

        /// <summary>
        /// The logarithm of integer 2.
        /// </summary>
        /// <param name="x">The x to check.</param>
        /// <returns>Logarithm.</returns>
        public static int Log2(int x)
        {
            int minResult = 0;
            int maxResult = 31;

            // We do a binary search.
            while (true)
            {
                int middle = (minResult + maxResult) / 2;
                if (middle == minResult) return (int)minResult;

                if (x >= (1 << middle))
                {
                    minResult = middle;
                }
                else
                {
                    maxResult = middle;
                }
            }
        }

        /// <summary>
        /// Computes a^b.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <returns>Result (possibly cached).</returns>
        /// <remarks>Use this only if result will be in uint range.</remarks>
        public static uint Pow(uint a, uint b)
        {
            // TODO: may cache in future.
            return (uint)(Pow((double)a, (double)b) + 0.5);
        }

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static int Max(int m1, int m2)
        {
            return m1 > m2 ? m1 : m2;
        }

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static int Max(int m1, int m2, int m3)
        {
            return m1 > m2 ? (m1 > m3 ? m1 : m3) : (m2 > m3 ? m2 : m3);
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static int Min(int m1, int m2)
        {
            return m1 < m2 ? m1 : m2;
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static int Min(int m1, int m2, int m3)
        {
            return m1 < m2 ? (m1 < m3 ? m1 : m3) : (m2 < m3 ? m2 : m3);
        }


        /// <summary>
        /// Clamps the specified x in range [min, max].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static int Clamp(int x, int min, int max)
        {
            return x < min ? min : (x > max ? max : x);
        }


        #endregion

        #region Unsigned Integer Version

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static uint Max(uint m1, uint m2)
        {
            return m1 > m2 ? m1 : m2;
        }

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static uint Max(uint m1, uint m2, uint m3)
        {
            return m1 > m2 ? (m1 > m3 ? m1 : m3) : (m2 > m3 ? m2 : m3);
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static uint Min(uint m1, uint m2)
        {
            return m1 < m2 ? m1 : m2;
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static uint Min(uint m1, uint m2, uint m3)
        {
            return m1 < m2 ? (m1 < m3 ? m1 : m3) : (m2 < m3 ? m2 : m3);
        }


        /// <summary>
        /// Clamps the specified x in range [min, max].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static uint Clamp(uint x, uint min, uint max)
        {
            return x < min ? min : (x > max ? max : x);
        }


        #endregion

        #region Long Integer Version

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static long Max(long m1, long m2)
        {
            return m1 > m2 ? m1 : m2;
        }

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static long Max(long m1, long m2, long m3)
        {
            return m1 > m2 ? (m1 > m3 ? m1 : m3) : (m2 > m3 ? m2 : m3);
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static long Min(long m1, long m2)
        {
            return m1 < m2 ? m1 : m2;
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static long Min(long m1, long m2, long m3)
        {
            return m1 < m2 ? (m1 < m3 ? m1 : m3) : (m2 < m3 ? m2 : m3);
        }


        /// <summary>
        /// Clamps the specified x in range [min, max].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static long Clamp(long x, long min, long max)
        {
            return x < min ? min : (x > max ? max : x);
        }


        #endregion

        #region Unsigned Long Integer Version

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static ulong Max(ulong m1, ulong m2)
        {
            return m1 > m2 ? m1 : m2;
        }

        /// <summary>
        /// Maximum of values.
        /// </summary>
        public static ulong Max(ulong m1, ulong m2, uint m3)
        {
            return m1 > m2 ? (m1 > m3 ? m1 : m3) : (m2 > m3 ? m2 : m3);
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static ulong Min(ulong m1, ulong m2)
        {
            return m1 < m2 ? m1 : m2;
        }

        /// <summary>
        /// Manimum of values.
        /// </summary>
        public static ulong Min(ulong m1, ulong m2, ulong m3)
        {
            return m1 < m2 ? (m1 < m3 ? m1 : m3) : (m2 < m3 ? m2 : m3);
        }


        /// <summary>
        /// Clamps the specified x in range [min, max].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static ulong Clamp(ulong x, ulong min, ulong max)
        {
            return x < min ? min : (x > max ? max : x);
        }


        #endregion

        #region Generic Methods

        /// <summary>
        /// Adds two "objects". Objects must be the same type. Floats, doubles, halfs, vectors and
        /// matrices are supported.
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns>Result cast into object.</returns>
        public static object Add([NotNull] object obj1, [NotNull] object obj2)
        {
            if (obj1.GetType() != obj2.GetType()) throw new ArgumentException("Object are not of the same type.");

            if (obj1 is BigNum)
            {
                return (BigNum)obj1 + (BigNum)obj2;
            }

            // Float version:
            if (obj1 is float)
            {
                return (float)obj1 + (float)obj2;
            }
            if (obj1 is Vector2f)
            {
                return (Vector2f)obj1 + (Vector2f)obj2;
            }
            if (obj1 is Vector3f)
            {
                return (Vector3f)obj1 + (Vector3f)obj2;
            }
            if (obj1 is Vector4f)
            {
                return (Vector4f)obj1 + (Vector4f)obj2;
            }
            if (obj1 is Matrix.Matrix2x2f)
            {
                return (Matrix.Matrix2x2f)obj1 + (Matrix.Matrix2x2f)obj2;
            }
            if (obj1 is Matrix.Matrix3x3f)
            {
                return (Matrix.Matrix3x3f)obj1 + (Matrix.Matrix3x3f)obj2;
            }
            if (obj1 is Matrix.Matrix4x4f)
            {
                return (Matrix.Matrix4x4f)obj1 + (Matrix.Matrix4x4f)obj2;
            }
            if (obj1 is Complexf)
            {
                return (Complexf)obj1 + (Complexf)obj2;
            }
            if (obj1 is Quaternionf)
            {
                return (Quaternionf)obj1 + (Quaternionf)obj2;
            }

            // Double version:
            if (obj1 is double)
            {
                return (double)obj1 + (double)obj2;
            }
            if (obj1 is Vector2d)
            {
                return (Vector2d)obj1 + (Vector2d)obj2;
            }
            if (obj1 is Vector3d)
            {
                return (Vector3d)obj1 + (Vector3d)obj2;
            }
            if (obj1 is Vector4d)
            {
                return (Vector4d)obj1 + (Vector4d)obj2;
            }
            if (obj1 is Matrix.Matrix2x2d)
            {
                return (Matrix.Matrix2x2d)obj1 + (Matrix.Matrix2x2d)obj2;
            }
            if (obj1 is Matrix.Matrix3x3d)
            {
                return (Matrix.Matrix3x3d)obj1 + (Matrix.Matrix3x3d)obj2;
            }
            if (obj1 is Matrix.Matrix4x4d)
            {
                return (Matrix.Matrix4x4d)obj1 + (Matrix.Matrix4x4d)obj2;
            }
            if (obj1 is Complexd)
            {
                return (Complexd)obj1 + (Complexd)obj2;
            }
            if (obj1 is Quaterniond)
            {
                return (Quaterniond)obj1 + (Quaterniond)obj2;
            }

            // Integer version:
            if (obj1 is int)
            {
                return (int)obj1 + (int)obj2;
            }
            if (obj1 is Vector2i)
            {
                return (Vector2i)obj1 + (Vector2i)obj2;
            }
            if (obj1 is Vector3i)
            {
                return (Vector3i)obj1 + (Vector3i)obj2;
            }
            if (obj1 is Vector4i)
            {
                return (Vector4i)obj1 + (Vector4i)obj2;
            }

            // Other types.
            if (obj1 is uint)
            {
                return (uint)obj1 + (uint)obj2;
            }
            if (obj1 is short)
            {
                return (short)obj1 + (short)obj2;
            }
            if (obj1 is ushort)
            {
                return (ushort)obj1 + (ushort)obj2;
            }
            if (obj1 is byte)
            {
                return (byte)obj1 + (byte)obj2;
            }
            if (obj1 is ulong)
            {
                return (ulong)obj1 + (ulong)obj2;
            }
            if (obj1 is long)
            {
                return (long)obj1 + (long)obj2;
            }


            throw new NotSupportedException("Unsupported type " + obj1.GetType());
        }

        /// <summary>
        /// Substracts two "objects". Objects must be the same type. Floats, doubles, halfs, vectors and
        /// matrices are supported.
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns>Result cast into object.</returns>
        public static object Sub(object obj1, object obj2)
        {
            if (obj1.GetType() != obj2.GetType()) throw new ArgumentException("Object are not of the same type.");

            if (obj1 is BigNum)
            {
                return (BigNum)obj1 - (BigNum)obj2;
            }

            // Float version:
            if (obj1 is float)
            {
                return (float)obj1 - (float)obj2;
            }
            if (obj1 is Vector2f)
            {
                return (Vector2f)obj1 - (Vector2f)obj2;
            }
            if (obj1 is Vector3f)
            {
                return (Vector3f)obj1 - (Vector3f)obj2;
            }
            if (obj1 is Vector4f)
            {
                return (Vector4f)obj1 - (Vector4f)obj2;
            }
            if (obj1 is Matrix.Matrix2x2f)
            {
                return (Matrix.Matrix2x2f)obj1 - (Matrix.Matrix2x2f)obj2;
            }
            if (obj1 is Matrix.Matrix3x3f)
            {
                return (Matrix.Matrix3x3f)obj1 - (Matrix.Matrix3x3f)obj2;
            }
            if (obj1 is Matrix.Matrix4x4f)
            {
                return (Matrix.Matrix4x4f)obj1 - (Matrix.Matrix4x4f)obj2;
            }
            if (obj1 is Complexf)
            {
                return (Complexf)obj1 - (Complexf)obj2;
            }
            if (obj1 is Quaternionf)
            {
                return (Quaternionf)obj1 - (Quaternionf)obj2;
            }

            // Double version:
            if (obj1 is double)
            {
                return (double)obj1 - (double)obj2;
            }
            if (obj1 is Vector2d)
            {
                return (Vector2d)obj1 - (Vector2d)obj2;
            }
            if (obj1 is Vector3d)
            {
                return (Vector3d)obj1 - (Vector3d)obj2;
            }
            if (obj1 is Vector4d)
            {
                return (Vector4d)obj1 - (Vector4d)obj2;
            }
            if (obj1 is Matrix.Matrix2x2d)
            {
                return (Matrix.Matrix2x2d)obj1 - (Matrix.Matrix2x2d)obj2;
            }
            if (obj1 is Matrix.Matrix3x3d)
            {
                return (Matrix.Matrix3x3d)obj1 - (Matrix.Matrix3x3d)obj2;
            }
            if (obj1 is Matrix.Matrix4x4d)
            {
                return (Matrix.Matrix4x4d)obj1 - (Matrix.Matrix4x4d)obj2;
            }
            if (obj1 is Complexd)
            {
                return (Complexd)obj1 - (Complexd)obj2;
            }
            if (obj1 is Quaterniond)
            {
                return (Quaterniond)obj1 - (Quaterniond)obj2;
            }

            // Integer version:
            if (obj1 is int)
            {
                return (int)obj1 - (int)obj2;
            }
            if (obj1 is Vector2i)
            {
                return (Vector2i)obj1 - (Vector2i)obj2;
            }
            if (obj1 is Vector3i)
            {
                return (Vector3i)obj1 - (Vector3i)obj2;
            }
            if (obj1 is Vector4i)
            {
                return (Vector4i)obj1 - (Vector4i)obj2;
            }

            // Other types.
            if (obj1 is uint)
            {
                return (uint)obj1 - (uint)obj2;
            }
            if (obj1 is short)
            {
                return (short)obj1 - (short)obj2;
            }
            if (obj1 is ushort)
            {
                return (ushort)obj1 - (ushort)obj2;
            }
            if (obj1 is byte)
            {
                return (byte)obj1 - (byte)obj2;
            }
            if (obj1 is ulong)
            {
                return (ulong)obj1 - (ulong)obj2;
            }
            if (obj1 is long)
            {
                return (long)obj1 - (long)obj2;
            }


            throw new NotSupportedException("Unsupported type " + obj1.GetType());
        }

        /// <summary>
        /// Dots two vectors of the same size (scalars as degenerated vectors use normal multiplication).
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns>Result cast into object.</returns>
        public static object Dot(object obj1, object obj2)
        {
            if (obj1.GetType() != obj2.GetType()) throw new ArgumentException("Object are not of the same type.");


            // Float version:
            if (obj1 is float)
            {
                return (float)obj1 * (float)obj2;
            }
            if (obj1 is Vector2f)
            {
                return (Vector2f)obj1 * (Vector2f)obj2;
            }
            if (obj1 is Vector3f)
            {
                return (Vector3f)obj1 * (Vector3f)obj2;
            }
            if (obj1 is Vector4f)
            {
                return (Vector4f)obj1 * (Vector4f)obj2;
            }


            // Double version:
            if (obj1 is double)
            {
                return (double)obj1 * (double)obj2;
            }
            if (obj1 is Vector2d)
            {
                return (Vector2d)obj1 * (Vector2d)obj2;
            }
            if (obj1 is Vector3d)
            {
                return (Vector3d)obj1 * (Vector3d)obj2;
            }
            if (obj1 is Vector4d)
            {
                return (Vector4d)obj1 * (Vector4d)obj2;
            }


            // Integer version:
            if (obj1 is int)
            {
                return (int)obj1 * (int)obj2;
            }
            if (obj1 is Vector2i)
            {
                return (Vector2i)obj1 * (Vector2i)obj2;
            }
            if (obj1 is Vector3i)
            {
                return (Vector3i)obj1 * (Vector3i)obj2;
            }
            if (obj1 is Vector4i)
            {
                return (Vector4i)obj1 * (Vector4i)obj2;
            }

            // Other types.
            if (obj1 is uint)
            {
                return (uint)obj1 * (uint)obj2;
            }
            if (obj1 is short)
            {
                return (short)obj1 * (short)obj2;
            }
            if (obj1 is ushort)
            {
                return (ushort)obj1 * (ushort)obj2;
            }
            if (obj1 is byte)
            {
                return (byte)obj1 * (byte)obj2;
            }
            if (obj1 is ulong)
            {
                return (ulong)obj1 * (ulong)obj2;
            }
            if (obj1 is long)
            {
                return (long)obj1 * (long)obj2;
            }


            throw new NotSupportedException("Unsupported type " + obj1.GetType());
        }

        /// <summary>
        /// Multiplies two objects. Scalar scalar, scalar vector, scalar matrix, vector vector (component),
        /// vector matrix, matrix vector matrix matrix multiplications are supported.
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns>Result cast into object.</returns>
        public static object Mul(object obj1, object obj2)
        {
            if (obj1.GetType() != obj2.GetType()) throw new ArgumentException("Object are not of the same type.");

            if (obj1 is BigNum)
            {
                return (BigNum)obj1 * (BigNum)obj2;
            }

            // Float version:
            if (obj1 is float)
            {
                return (float)obj1 * (float)obj2;
            }
            if (obj1 is Vector2f)
            {
                return Vector2f.ComponentMultiply((Vector2f)obj1, (Vector2f)obj2);
            }
            if (obj1 is Vector3f)
            {
                return Vector3f.ComponentMultiply((Vector3f)obj1, (Vector3f)obj2);
            }
            if (obj1 is Vector4f)
            {
                return Vector4f.ComponentMultiply((Vector4f)obj1, (Vector4f)obj2);
            }
            if (obj1 is Matrix.Matrix2x2f)
            {
                return (Matrix.Matrix2x2f)obj1 * (Matrix.Matrix2x2f)obj2;
            }
            if (obj1 is Matrix.Matrix3x3f)
            {
                return (Matrix.Matrix3x3f)obj1 * (Matrix.Matrix3x3f)obj2;
            }
            if (obj1 is Matrix.Matrix4x4f)
            {
                return (Matrix.Matrix4x4f)obj1 * (Matrix.Matrix4x4f)obj2;
            }
            if (obj1 is Complexf)
            {
                return (Complexf)obj1 * (Complexf)obj2;
            }
            if (obj1 is Quaternionf)
            {
                return (Quaternionf)obj1 * (Quaternionf)obj2;
            }

            // Double version:
            if (obj1 is double)
            {
                return (double)obj1 * (double)obj2;
            }
            if (obj1 is Vector2d)
            {
                return Vector2d.ComponentMultiply((Vector2d)obj1, (Vector2d)obj2);
            }
            if (obj1 is Vector3d)
            {
                return Vector3d.ComponentMultiply((Vector3d)obj1, (Vector3d)obj2);
            }
            if (obj1 is Vector4d)
            {
                return Vector4d.ComponentMultiply((Vector4d)obj1, (Vector4d)obj2);
            }
            if (obj1 is Matrix.Matrix2x2d)
            {
                return (Matrix.Matrix2x2d)obj1 * (Matrix.Matrix2x2d)obj2;
            }
            if (obj1 is Matrix.Matrix3x3d)
            {
                return (Matrix.Matrix3x3d)obj1 * (Matrix.Matrix3x3d)obj2;
            }
            if (obj1 is Matrix.Matrix4x4d)
            {
                return (Matrix.Matrix4x4d)obj1 * (Matrix.Matrix4x4d)obj2;
            }
            if (obj1 is Complexd)
            {
                return (Complexd)obj1 * (Complexd)obj2;
            }
            if (obj1 is Quaterniond)
            {
                return (Quaterniond)obj1 * (Quaterniond)obj2;
            }

            // Integer version:
            if (obj1 is int)
            {
                return (int)obj1 * (int)obj2;
            }
            if (obj1 is Vector2i)
            {
                return Vector2i.ComponentMultiply((Vector2i)obj1, (Vector2i)obj2);
            }
            if (obj1 is Vector3i)
            {
                return Vector3i.ComponentMultiply((Vector3i)obj1, (Vector3i)obj2);
            }
            if (obj1 is Vector4i)
            {
                return Vector4i.ComponentMultiply((Vector4i)obj1, (Vector4i)obj2);
            }

            // Other types.
            if (obj1 is uint)
            {
                return (uint)obj1 * (uint)obj2;
            }
            if (obj1 is short)
            {
                return (short)obj1 * (short)obj2;
            }
            if (obj1 is ushort)
            {
                return (ushort)obj1 * (ushort)obj2;
            }
            if (obj1 is byte)
            {
                return (byte)obj1 * (byte)obj2;
            }
            if (obj1 is ulong)
            {
                return (ulong)obj1 * (ulong)obj2;
            }
            if (obj1 is long)
            {
                return (long)obj1 * (long)obj2;
            }


            throw new NotSupportedException("Unsupported type " + obj1.GetType());
        }

        /// <summary>
        /// Divides two quantities. The same operations as for multiplication are supported.
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns>Result cast into object.</returns>
        public static object Div(object obj1, object obj2)
        {
            if (obj1.GetType() != obj2.GetType()) throw new ArgumentException("Object are not of the same type.");

            if (obj1 is BigNum)
            {
                return (BigNum)obj1 + (BigNum)obj2;
            }

            // Float version:
            if (obj1 is float)
            {
                return (float)obj1 / (float)obj2;
            }
            if (obj1 is Vector2f)
            {
                return Vector2f.ComponentDivision((Vector2f)obj1, (Vector2f)obj2);
            }
            if (obj1 is Vector3f)
            {
                return Vector3f.ComponentDivision((Vector3f)obj1, (Vector3f)obj2);
            }
            if (obj1 is Vector4f)
            {
                return Vector4f.ComponentDivision((Vector4f)obj1, (Vector4f)obj2);
            }
            if (obj1 is Matrix.Matrix2x2f)
            {
                return (Matrix.Matrix2x2f)obj1 / (Matrix.Matrix2x2f)obj2;
            }
            if (obj1 is Matrix.Matrix3x3f)
            {
                return (Matrix.Matrix3x3f)obj1 / (Matrix.Matrix3x3f)obj2;
            }
            if (obj1 is Matrix.Matrix4x4f)
            {
                return (Matrix.Matrix4x4f)obj1 / (Matrix.Matrix4x4f)obj2;
            }
            if (obj1 is Complexf)
            {
                return (Complexf)obj1 / (Complexf)obj2;
            }
            if (obj1 is Quaternionf)
            {
                return (Quaternionf)obj1 / (Quaternionf)obj2;
            }

            // Double version:
            if (obj1 is double)
            {
                return (double)obj1 / (double)obj2;
            }
            if (obj1 is Vector2d)
            {
                return Vector2d.ComponentDivision((Vector2d)obj1, (Vector2d)obj2);
            }
            if (obj1 is Vector3d)
            {
                return Vector3d.ComponentDivision((Vector3d)obj1, (Vector3d)obj2);
            }
            if (obj1 is Vector4d)
            {
                return Vector4d.ComponentDivision((Vector4d)obj1, (Vector4d)obj2);
            }
            if (obj1 is Matrix.Matrix2x2d)
            {
                return (Matrix.Matrix2x2d)obj1 / (Matrix.Matrix2x2d)obj2;
            }
            if (obj1 is Matrix.Matrix3x3d)
            {
                return (Matrix.Matrix3x3d)obj1 / (Matrix.Matrix3x3d)obj2;
            }
            if (obj1 is Matrix.Matrix4x4d)
            {
                return (Matrix.Matrix4x4d)obj1 / (Matrix.Matrix4x4d)obj2;
            }
            if (obj1 is Complexd)
            {
                return (Complexd)obj1 / (Complexd)obj2;
            }
            if (obj1 is Quaterniond)
            {
                return (Quaterniond)obj1 / (Quaterniond)obj2;
            }

            // Integer version:
            if (obj1 is int)
            {
                return (int)obj1 / (int)obj2;
            }

            // Other types.
            if (obj1 is uint)
            {
                return (uint)obj1 / (uint)obj2;
            }
            if (obj1 is short)
            {
                return (short)obj1 / (short)obj2;
            }
            if (obj1 is ushort)
            {
                return (ushort)obj1 / (ushort)obj2;
            }
            if (obj1 is byte)
            {
                return (byte)obj1 / (byte)obj2;
            }
            if (obj1 is ulong)
            {
                return (ulong)obj1 / (ulong)obj2;
            }
            if (obj1 is long)
            {
                return (long)obj1 / (long)obj2;
            }


            throw new NotSupportedException("Unsupported type " + obj1.GetType());
        }

        #endregion

    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class MathHelperTest
    {
        [CorrectnessTest]
        public void ReverseBits()
        {
            // First full reversals.
            uint x = 0xF0F0F0F0;
            Assert.AreEqual(0x0F0F0F0F, MathHelper.ReverseBits(x));

            // Bit reversals.
            Assert.AreEqual(1, MathHelper.ReverseBits(1, 1));
            Assert.AreEqual(1 << 2, MathHelper.ReverseBits(1, 3));
        }
    }
#endif
}
