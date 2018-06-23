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

namespace SharpMedia.Math.Statistics
{
    /// <summary>
    /// Accumulator can accumulate data and compute statistical properties.
    /// </summary>
    public class Accumulator
    {
        #region Private Members
        double sum = 0.0;
        double sumSquared = 0.0;
        uint count;
        double min = double.PositiveInfinity;
        double max = double.NegativeInfinity;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the min.
        /// </summary>
        /// <value>The min.</value>
        public double Min
        {
            get { return min; }
        }

        /// <summary>
        /// Gets the max.
        /// </summary>
        /// <value>The max.</value>
        public double Max
        {
            get { return max; }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public uint Count
        {
            get { return count; }
        }

        /// <summary>
        /// Gets the sum.
        /// </summary>
        /// <value>The sum.</value>
        public double Sum
        {
            get { return sum; }
        }

        /// <summary>
        /// Gets the squared sum.
        /// </summary>
        /// <value>The squared sum.</value>
        public double SquaredSum
        {
            get { return sumSquared; }
        }

        /// <summary>
        /// Gets the mean.
        /// </summary>
        /// <value>The mean.</value>
        public double Mean
        {
            get { return sum / count; }
        }

        /// <summary>
        /// Gets the mean squared.
        /// </summary>
        /// <value>The mean squared.</value>
        public double MeanSquared
        {
            get { return sumSquared / count; }
        }

        /// <summary>
        /// Gets the variance.
        /// </summary>
        /// <value>The variance.</value>
        public double Variance
        {
            get
            {
                double mean = this.Mean;
				return (sumSquared / (double)count - mean * mean);
            }
        }

        /// <summary>
        /// Gets the sigma.
        /// </summary>
        /// <value>The sigma.</value>
        public double Sigma
        {
            get
            {
                return MathHelper.Sqrt(Variance);
            }
        }

        /// <summary>
        /// Gets the error estimate.
        /// </summary>
        /// <value>The error estimate.</value>
        public double ErrorEstimate
        {
            get
            {
                return Sigma / (double)MathHelper.Sqrt(count);
            }
        }

        #endregion

        #region Member Functions

        /// <summary>
        /// Initializes a new instance of the <see cref="Accumulator"/> class.
        /// </summary>
        public Accumulator() 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Accumulator"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Accumulator(IEnumerable<double> data)
        {
            Add(data);
        }

        /// <summary>
        /// Adds new data to accumulator.
        /// </summary>
        /// <param name="x">The x.</param>
        public void Add(double x)
        {
            sum += x;
            sumSquared += x * x;
            count++;
            min = x < min ? x : min;
            max = x > max ? x : max;
        }

        /// <summary>
        /// Adds the specified xs.
        /// </summary>
        /// <param name="xs">The xs.</param>
        public void Add(params double[] xs)
        {
            for (int i = 0; i < xs.Length; i++)
            {
                Add(xs[i]);
            }
        }

        /// <summary>
        /// Adds the specified xs.
        /// </summary>
        /// <param name="xs">The xs.</param>
        public void Add(IEnumerable<double> xs)
        {
            
            foreach(double x in xs)
            {
                Add(x);
            }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class AccumulatorTest
    {
        [CorrectnessTest]
        public void Accum()
        {
            Accumulator a = new Accumulator(new double[]{0,0,1,-1,2,-2});
            Assert.AreEqual(6, a.Count);
            Assert.AreEqual(2, a.Max);
            Assert.AreEqual(-2, a.Min);
            
        }
    }
#endif
}
