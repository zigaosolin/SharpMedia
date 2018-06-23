// This file was generated by TemplateEngine from template source 'GeometricSeries'
// using template 'GeometricSeriesf. Do not modify this file directly, modify it from template source.

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
    /// A geometric series, defined as:
    /// a, ak, ak^2, ak^3, ...
    /// </summary>
    public class GeometricSeriesf : ISeriesf
    {
        #region Private Members
        private float k;
        private float a;
        #endregion

        #region Properties

        /// <summary>
        /// The koefficient, next element versus previous is this element.
        /// </summary>
        public float K
        {
            get
            {
                return k;
            }
            set
            {
                k = value;
            }
        }

        /// <summary>
        /// The initial value.
        /// </summary>
        public float Initial
        {
            get
            {
                return a;
            }
            set
            {
                a = value;
            }
        }

        #endregion

        #region ISeriesd Members

        public DiscreteFunctionf Function
        {
            get
            {
                return delegate(uint x)
                {
                    return a * MathHelper.Pow(k, (float)x);
                };
            }
        }

        public DiscreteFunctionf SumFunction
        {
            get
            {
                return delegate(uint x)
                {
                    return a * ((float)1.0 - MathHelper.Pow(k, (float)(x + 1))) / ((float)1.0 - k);
                };
            }

        }

        public float[] Sample(uint begin, uint count)
        {
            // Must be nonzero.
            if (count == 0) return null;

            float[] r = new float[count];

            // We calculate the begin element.
            float x = Function(begin);
            r[0] = x;

            for (uint i = 1; i < count; i++)
            {
                x *= k;
                r[i] = x;
            }

            // Returns next element.
            return r;
        }

        public float[] SampleSum(uint begin, uint count)
        {
            if (count == 0) return null;

            float[] r = new float[count];

            // We caclulate first element.
            float x = Function(begin);
            r[0] = SumFunction(begin);

            for (uint i = 1; i < count; i++)
            {
                x *= k;
                r[i] = r[i - 1] + x;
            }

            // Returns next element.
            return r;

        }

        public float InfiniteSum
        {
            get
            {
                if (MathHelper.Abs(k) < 1.0f) return a / (1.0f - k);
                if (k < 0.0f) return float.NaN;

                // Otherwise, result depends on a.'s sign.
                return a * float.PositiveInfinity;

            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor given needed parameters.
        /// </summary>
        /// <param name="initial">Initial, first values in series.</param>
        /// <param name="q">The queficient with adjecting elements.</param>
        public GeometricSeriesf(float initial, float q)
        {
            a = initial;
            k = q;
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class Test_GeometricSeriesf
    {
        [CorrectnessTest]
        public void Properties()
        {
            GeometricSeriesf s = new GeometricSeriesf((float)1.0, (float)0.5);

            Assert.AreEqual((float)1.0, s.Function(0));
            Assert.AreEqual((float)0.5, s.Function(1));
            Assert.AreEqual((float)1.0, s.SumFunction(0));
            Assert.AreEqual((float)1.5, s.SumFunction(1));
            Assert.AreEqual((float)1.0, s.Initial);
            Assert.AreEqual((float)0.5, s.K);
            Assert.AreEqual((float)2.0, s.InfiniteSum);
        }
    }
#endif
}
