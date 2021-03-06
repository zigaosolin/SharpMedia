// This file was generated by TemplateEngine from template source 'Series'
// using template 'Seriesd. Do not modify this file directly, modify it from template source.

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
using SharpMedia.Math.Functions.Discrete;

namespace SharpMedia.Math.Functions.Series
{
    /// <summary>
    /// The discrete series.
    /// </summary>
    public interface ISeriesd
    {
        /// <summary>
        /// A series as a function.
        /// </summary>
        DiscreteFunctiond Function
        {
            get;
        }

        /// <summary>
        /// The sum function until element (including element).
        /// </summary>
        DiscreteFunctiond SumFunction
        {
            get;
        }

        /// <summary>
        /// Samples the function.
        /// </summary>
        /// <remarks>This is useful if function is recursive because
        /// we can simplify it.
        /// </remarks>
        /// <param name="begin">The beginning value.</param>
        /// <param name="count">Number of elements.</param>
        /// <returns>The samples values.</returns>
        double[] Sample(uint begin, uint count);

        /// <summary>
        /// Samples the function.
        /// </summary>
        /// <remarks>This is useful if function is recursive because
        /// we can simplify it.
        /// </remarks>
        /// <param name="begin">The beginning value.</param>
        /// <param name="count">Number of elements.</param>
        /// <returns>The samples values.</returns>
        double[] SampleSum(uint begin, uint count);

        /// <summary>
        /// Sum of infinite elements, may be infinity (when going in infinity) or NaN if it has more limits.
        /// </summary>
        double InfiniteSum
        {
            get;
        }
    }

    /// <summary>
    /// A generic series, only function is given for each element.
    /// </summary>
    public class GenericSeriesd : ISeriesd
    {
        #region Private Members
        private DiscreteFunctiond f;
        private List<double> sumCache = new List<double>();
        #endregion

        #region Private Methods

        double SumFunctionDelegate(uint x)
        {
            // We check if cached.
            if (x > (uint)sumCache.Count) return sumCache[(int)x];

            // We compute it.
            for (int i = sumCache.Count - 1; i <= (int)x; i++)
            {
                sumCache.Add(f((uint)i) + sumCache[i - 1]);
            }

            return sumCache[(int)x];
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSeriesd"/> class.
        /// </summary>
        /// <param name="f">The f.</param>
        public GenericSeriesd(DiscreteFunctiond f)
        {
            this.f = f;
            this.sumCache.Add(f(0));
        }

        #endregion

        #region ISeriesd Members

        public DiscreteFunctiond Function
        {
            get { return f; }
        }

        public DiscreteFunctiond SumFunction
        {
            get
            {
                return SumFunctionDelegate;
            }
        }

        public double[] Sample(uint begin, uint count)
        {
            double[] result = new double[count];
            for (uint i = 0; i < count; i++)
            {
                result[i] = f(i + begin);
            }
            return result;
        }

        public double[] SampleSum(uint begin, uint count)
        {
            double[] result = new double[count];
            for (uint i = 0; i < count; i++)
            {
                result[i] = SumFunctionDelegate(i + begin);
            }
            return result;
        }

        public double InfiniteSum
        {
            get
            {
                throw new InvalidOperationException("Cannot find infinite sum on generic series.");
            }
        }

        #endregion
    }
}
