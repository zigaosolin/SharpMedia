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

namespace SharpMedia.Math.Functions
{
    /// <summary>
    /// Factorial implementation.
    /// </summary>
    /// <remarks>
    /// Before factorial is used, the value is rounded (e.g. this is not implementation of gamma function).
    /// Factorials only exist until value 170 for double precission, after that, they go out of the range of
    /// double precission. Try conerting to logarithmic basis.
    /// </remarks>
    public class Factorial : IFunction
    {
        #region Private Members
        public const int TableSize = 170;
        static double[] table = new double[TableSize];
        #endregion

        #region Static Members

        /// <summary>
        /// Static constructor, creating table.
        /// </summary>
        static Factorial()
        {
            table[0] = 1.0;

            // We precompute.
            for (int i = 1; i < TableSize; i++)
            {
                table[i] = table[i - 1] * (double)i; 
            }
        }

        /// <summary>
        /// Integer version of factorial.
        /// </summary>
        /// <param name="x">The input.</param>
        /// <returns>The output value, always double because it can be so big.</returns>
        public static double Evald(uint x)
        {
            if (x >= TableSize) return double.PositiveInfinity;
            return table[x];
        }

        /// <summary>
        /// Integer version of factorial.
        /// </summary>
        /// <param name="x">The input.</param>
        /// <returns>The output value, always double because it can be so big.</returns>
        public static float Evalf(uint x)
        {
            if (x >= TableSize) return float.PositiveInfinity;
            return (float)table[x];
        }

        /// <summary>
        /// Integer version of factorial.
        /// </summary>
        /// <param name="x">The input.</param>
        /// <returns>The output value.</returns>
        public static uint Eval(uint x)
        {
            return (uint)Evald(x);
        }

        /// <summary>
        /// Double version of factorial. Doubles are rounded to uint taking the following rule:
        /// - if decimal part >= 0.5, we round up
        /// - else round down
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Eval(double x)
        {
            // We round the value.
            uint r = (uint)global::System.Math.Floor(x + 0.5);
            return Evald(r);
        }

        /// <summary>
        /// Float version of factorial.
        /// - if decimal part >= 0.5, we round up
        /// - else round down
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Eval(float x)
        {
            uint r = (uint)global::System.Math.Floor(x + 0.5);
            return Evalf(r);
        }

        #endregion

        #region IFunctiond Members

        public Functiond Functiond
        {
            get 
            {
                return delegate(double x)
                {
                     return Eval(x);
                };
            }
        }

        public Functionf Functionf
        {
            get 
            {
                return delegate(float x)
                {
                    return (float)Eval(x);
                };
            }
        }

        double IFunction.Eval(double x)
        {
            return Eval(x);
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class FactorialTest
    {
        [CorrectnessTest]
        public void F()
        {
            Assert.AreEqual(Factorial.Evald(4), 24.0);
            Assert.AreEqual(Factorial.Evald(15), Factorial.Evald(14) * 15.0);
        }
    }
#endif
}
