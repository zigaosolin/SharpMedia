// This file was generated by TemplateEngine from template source 'Interval'
// using template 'Intervalf. Do not modify this file directly, modify it from template source.

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
    /// An interval. Interval is defined as infinite number of real numbers in
    /// some finite space and is thus a set.
    /// </summary>summary>
    /// <remarks>
    /// All intervals here are defined using [a,b] notation.
    /// </remarks>
    [Serializable]
    public struct Intervalf : IComparable, IComparable<Intervalf>, IEquatable<Intervalf>
    {
        #region Public Members

        /// <summary>
        /// The lower boundary.
        /// </summary>
        public float A;

        /// <summary>
        /// The upper boundary.
        /// </summary>
        public float B;

        #endregion

        #region Properties

        /// <summary>
        /// The lower boundary.
        /// </summary>
        [Obsolete]
        public float a
        {
            get { return A; }
            set
            {
                A = value;
            }
        }

        /// <summary>
        /// The upper boundary.
        /// </summary>
        [Obsolete]
        public float b
        {
            get { return B; }
            set
            {
                B = value;
            }
        }

        /// <summary>
        /// The range of interval. If it is set, the lower bound must be set first.
        /// </summary>
        public float Range
        {
            get { return B - A > 0.0f ? B - A : 0.0f; }
            set { B = A + value; }
        }

        #endregion

        #region Static Members

        /// <summary>
        /// A null interval.
        /// </summary>
        public static Intervalf Null
        {
            get
            {
                return new Intervalf(1.0f, 0.0f); 
            }
        }

        //#ifdef NearEqual


        /// <summary>
        /// Are intervals nearly equal.
        /// </summary>
        /// <param name="int1"></param>
        /// <param name="int2"></param>
        /// <returns></returns>
        public static bool NearEqual(Intervalf int1, Intervalf int2)
        {
            if (!MathHelper.NearEqual(int1.A, int2.A)) return false;
            if (!MathHelper.NearEqual(int1.B, int2.B)) return false;
            return true;
        }

        /// <summary>
        /// Are intervals nearly equal.
        /// </summary>
        /// <param name="int1"></param>
        /// <param name="int2"></param>
        /// <returns></returns>
        public static bool NearEqual(Intervalf int1, Intervalf int2, float eps)
        {
            if (!MathHelper.NearEqual(int1.A, int2.A, eps)) return false;
            if (!MathHelper.NearEqual(int1.B, int2.B, eps)) return false;
            return true;
        }

        //#endif

        #endregion

        #region Methods

        /// <summary>
        /// Checks if value is in interval.
        /// </summary>
        /// <param name="x">The value to check.</param>
        /// <returns>True or false.</returns>
        public bool Is(float x)
        {
            return (x >= A) && (x <= B);
        }

        /// <summary>
        /// Performs a linear Sample at interval.
        /// </summary>
        /// <param name="count">Number of samples.</param>
        /// <returns>The sampled data.</returns>
        public float[] Sample([MinUInt(2)] uint count)
        {
            float dcount = (float)(count - 1);
            float range = Range;
            float[] result = new float[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = A + ((float)i / dcount) * range;
            }


            return result;
        }

        //#ifdef FunctionDelegateClassName


        /// <summary>
        /// Samples the specified function.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public float[] Sample(
            [NotNull] Functionf f, 
            [MinUInt(2)] uint count)
        {
            float range = Range;
            float dcount = (float)(count - 1);
            float[] result = new float[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = f(A + (float)i / dcount * Range);
            }

            return result;
        }

        //#endif

        //#ifdef ComplexFunctionDelegateClassName ComplexClassName


        /// <summary>
        /// Samples the specified function.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public Complexf[] Sample(
            [NotNull] ComplexFunctionf f, 
            [MinUInt(2)] uint count)
        {
            float range = Range;
            float dcount = (float)(count - 1);
            Complexf[] result = new Complexf[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = f(A + (float)i / dcount * Range);
            }

            return result;
        }

        //#endif

        /// <summary>
        /// Samples at t.
        /// </summary>
        /// <param name="t">Must be in range [0,1].</param>
        /// <returns>The real at t.</returns>
        public float Sample(float t)
        {
            return A + Range * t;
        }

        #endregion

        #region Intervals

        /// <summary>
        /// Checks if intervals are the same.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Are the same value.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                Intervalf interval = (Intervalf)obj;
                if (interval.A != A) return false;
                if (interval.B != B) return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Obtains hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// The interval with limits.
        /// </summary>
        /// <param name="a">The left limit.</param>
        /// <param name="b">The right limit.</param>
        public Intervalf(float a, float b)
        {
            this.A = a;
            this.B = b;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is Intervalf)
            {
                return this.CompareTo((Intervalf)obj);
            }
            throw new ArgumentException("Invalid argument type.");
        }

        #endregion

        #region IComparable<Intervalf> Members

        public int CompareTo(Intervalf other)
        {
            int cmp = this.A.CompareTo(other.A);
            if (cmp != 0) return cmp;
            return B.CompareTo(other.B);
        }

        #endregion

        #region IEquatable<Intervalf> Members

        public bool Equals(Intervalf other)
        {
            return A == other.A && B == other.B;
        }

        #endregion
    }


#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class Intervalf_Test
    {
        protected Intervalf interval = new Intervalf(0.0f, 1.0f);
        protected Intervalf interval2 = new Intervalf(0.0f, 1.0f);

        [CorrectnessTest]
        public void Is() { Assert.IsTrue(interval.Is((float)0.5)); Assert.IsTrue(interval.Is((float)1.0));  }
        [CorrectnessTest]
        public void Equal() { Assert.IsTrue(interval.Equals(interval2)); }
        [CorrectnessTest]
        public void A() { Assert.AreEqual(interval2.A, 0.0f); }
        [CorrectnessTest]
        public void B() { Assert.AreEqual(interval.B, 1.0f); }
        [CorrectnessTest]
        public void Range() { Assert.AreEqual(interval2.Range, 1.0f); }
    }

#endif

}