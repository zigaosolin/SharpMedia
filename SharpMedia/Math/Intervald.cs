// This file was generated by TemplateEngine from template source 'Interval'
// using template 'Intervald. Do not modify this file directly, modify it from template source.

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
    public struct Intervald : IComparable, IComparable<Intervald>, IEquatable<Intervald>
    {
        #region Public Members

        /// <summary>
        /// The lower boundary.
        /// </summary>
        public double A;

        /// <summary>
        /// The upper boundary.
        /// </summary>
        public double B;

        #endregion

        #region Properties

        /// <summary>
        /// The lower boundary.
        /// </summary>
        [Obsolete]
        public double a
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
        public double b
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
        public double Range
        {
            get { return B - A > 0.0 ? B - A : 0.0; }
            set { B = A + value; }
        }

        #endregion

        #region Static Members

        /// <summary>
        /// A null interval.
        /// </summary>
        public static Intervald Null
        {
            get
            {
                return new Intervald(1.0, 0.0); 
            }
        }

        //#ifdef NearEqual


        /// <summary>
        /// Are intervals nearly equal.
        /// </summary>
        /// <param name="int1"></param>
        /// <param name="int2"></param>
        /// <returns></returns>
        public static bool NearEqual(Intervald int1, Intervald int2)
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
        public static bool NearEqual(Intervald int1, Intervald int2, double eps)
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
        public bool Is(double x)
        {
            return (x >= A) && (x <= B);
        }

        /// <summary>
        /// Performs a linear Sample at interval.
        /// </summary>
        /// <param name="count">Number of samples.</param>
        /// <returns>The sampled data.</returns>
        public double[] Sample([MinUInt(2)] uint count)
        {
            double dcount = (double)(count - 1);
            double range = Range;
            double[] result = new double[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = A + ((double)i / dcount) * range;
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
        public double[] Sample(
            [NotNull] Functiond f, 
            [MinUInt(2)] uint count)
        {
            double range = Range;
            double dcount = (double)(count - 1);
            double[] result = new double[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = f(A + (double)i / dcount * Range);
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
        public Complexd[] Sample(
            [NotNull] ComplexFunctiond f, 
            [MinUInt(2)] uint count)
        {
            double range = Range;
            double dcount = (double)(count - 1);
            Complexd[] result = new Complexd[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = f(A + (double)i / dcount * Range);
            }

            return result;
        }

        //#endif

        /// <summary>
        /// Samples at t.
        /// </summary>
        /// <param name="t">Must be in range [0,1].</param>
        /// <returns>The real at t.</returns>
        public double Sample(double t)
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
                Intervald interval = (Intervald)obj;
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
        public Intervald(double a, double b)
        {
            this.A = a;
            this.B = b;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is Intervald)
            {
                return this.CompareTo((Intervald)obj);
            }
            throw new ArgumentException("Invalid argument type.");
        }

        #endregion

        #region IComparable<Intervald> Members

        public int CompareTo(Intervald other)
        {
            int cmp = this.A.CompareTo(other.A);
            if (cmp != 0) return cmp;
            return B.CompareTo(other.B);
        }

        #endregion

        #region IEquatable<Intervald> Members

        public bool Equals(Intervald other)
        {
            return A == other.A && B == other.B;
        }

        #endregion
    }


#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class Intervald_Test
    {
        protected Intervald interval = new Intervald(0.0, 1.0);
        protected Intervald interval2 = new Intervald(0.0, 1.0);

        [CorrectnessTest]
        public void Is() { Assert.IsTrue(interval.Is((double)0.5)); Assert.IsTrue(interval.Is((double)1.0));  }
        [CorrectnessTest]
        public void Equal() { Assert.IsTrue(interval.Equals(interval2)); }
        [CorrectnessTest]
        public void A() { Assert.AreEqual(interval2.A, 0.0); }
        [CorrectnessTest]
        public void B() { Assert.AreEqual(interval.B, 1.0); }
        [CorrectnessTest]
        public void Range() { Assert.AreEqual(interval2.Range, 1.0); }
    }

#endif

}