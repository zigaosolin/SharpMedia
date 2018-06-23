// This file was generated by TemplateEngine from template source 'ArithmeticSeries'
// using template 'ArithmeticSeriesf. Do not modify this file directly, modify it from template source.

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
using SharpMedia.Math.Functions.Discrete;

namespace SharpMedia.Math.Functions.Series
{

    /// <summary>
    /// Arithmetic series, defined as:
    /// a, a+d, a+2d, ... a+n*d
    /// </summary>
    public class ArithmeticSeriesf : ISeriesf
    {
        #region Private Members
        private float a;
        private float d;
        #endregion

        #region Properties

        /// <summary>
        /// The difference between two adjected elements.
        /// </summary>
        public float Difference
        {
            get { return d; }
            set { d = value; }
        }

        /// <summary>
        /// An initial element of arithmetic series.
        /// </summary>
        public float Initial
        {
            get { return a; }
            set { a = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construction of arithmetic series.
        /// </summary>
        /// <param name="initial">The initial element.</param>
        /// <param name="dif">The difference between two elements in row.</param>
        public ArithmeticSeriesf(float initial, float dif)
        {
            a = initial;
            d = dif;
        }

        #endregion

        #region ISeriesf Members

        public DiscreteFunctionf Function
        {
            get
            {
                return delegate(uint x)
                {
                    return a + x * d;
                };
            }
        }

        public DiscreteFunctionf SumFunction
        {
            get
            {
                return delegate(uint x)
                {
                    return (float)0.5 * (float)(x + 1) * ((float)2.0 * a + (float)x * d);
                };
            }
        }

        public float[] Sample(uint begin, uint count)
        {
            float[] r = new float[count];
            for (int i = 0; i < count; i++)
            {
                r[i] = a + (begin + i) * d;
            }

            return r;
        }

        public float[] SampleSum(uint begin, uint count)
        {
            float[] r = new float[count];
            for (int i = 0; i < count; i++)
            {
                r[i] = (float)0.5 * (float)(begin + i + 1)
                       * ((float)2.0 * a + (begin + i) * d);
            }

            return r;
        }

        public float InfiniteSum
        {
            get { return float.PositiveInfinity * d; }
        }

        #endregion
    }


#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class Test_ArithmeticSeriesf
    {
        [CorrectnessTest]
        public void Properties()
        {
            ArithmeticSeriesf s = new ArithmeticSeriesf((float)1.0, (float)0.2);
            Assert.AreEqual((float)1.2, s.Function(1));
            Assert.AreEqual((float)2.2, s.SumFunction(1));
            Assert.AreEqual((float)1.0, s.Initial);
            Assert.AreEqual(float.PositiveInfinity, s.InfiniteSum);
            Assert.AreEqual((float)0.2, s.Difference);
        }
    }
#endif
}
