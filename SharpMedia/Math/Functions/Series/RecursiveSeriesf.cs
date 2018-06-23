// This file was generated by TemplateEngine from template source 'RecursiveSeries'
// using template 'RecursiveSeriesf. Do not modify this file directly, modify it from template source.

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
using SharpMedia.Math.Functions.Discrete;

namespace SharpMedia.Math.Functions.Series
{

    /// <summary>
    /// A recursive series. Next value is computed from previous.
    /// </summary>
    public class RecursiveSeriesf : ISeriesf
    {
        #region Private Members
        float initial;
        Functionf f;
        #endregion

        #region Constructors

        public RecursiveSeriesf(float initial, [NotNull] Functionf f)
        {
            this.initial = initial;
            this.f = f;
        }

        #endregion

        #region ISeriesd Members

        public DiscreteFunctionf Function
        {
            get
            {
                return delegate(uint n)
                {
                    float v = initial;
                    for (int i = 1; i <= n; i++)
                    {
                        v = f(v);
                    }
                    return v;
                };

            }
        }

        public DiscreteFunctionf SumFunction
        {
            get
            {
                return delegate(uint n)
                {
                    float sum = initial;
                    float v = initial;
                    for (int i = 1; i <= n; i++)
                    {
                        v = f(v);
                        sum += v;
                    }
                    return sum;
                };
            }
        }

        public float[] Sample(uint begin, uint count)
        {
            float v = initial;
            for (int i = 1; i < begin; i++)
            {
                v = f(v);
            }

            // We go through all values.
            float[] results = new float[count];
            for (int j = 0; j < count; j++)
            {
                v = f(v);
                results[j] = v;
            }
            return results;
        }

        public float[] SampleSum(uint begin, uint count)
        {
            float v = initial, sum = 0; ;
            for (int i = 1; i < begin; i++)
            {
                v = f(v);
                sum += v;
            }

            // We go through all values.
            float[] results = new float[count];
            for (int j = 0; j < count; j++)
            {
                v = f(v);
                sum += v;
                results[j] = sum;
            }

            return results;
        }

        public float InfiniteSum
        {
            get { return float.NaN; }
        }

        #endregion
    }


}