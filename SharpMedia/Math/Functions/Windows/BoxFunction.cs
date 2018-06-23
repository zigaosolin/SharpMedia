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

namespace SharpMedia.Math.Functions.Windows
{
    /// <summary>
    /// A box function.
    /// </summary>
    public class BoxFunction : IWindowFunction
    {
        #region Private Members
        double min;
        double max;
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxFunction"/> class.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        public BoxFunction(double min, double max)
        {
            this.max = max;
            this.min = min;
        }

        #endregion

        #region IWindowFunctiond Members

        public Intervald NonZeroRange
        {
            get { return new Intervald(min, max); }
        }

        #endregion

        #region IFunctiond Members

        public Functiond Functiond
        {
            get { return delegate(double x) { return x >= min && x <= max ? 1.0 : 0.0; }; }
        }

        public Functionf Functionf
        {
            get 
            {
                float fmin = (float)min;
                float fmax = (float)max;
                return delegate(float x) { return x >= fmin && fmin <= fmax ? 1.0f : 0.0f; };
            }
        }

        public double Eval(double x)
        {
            return x >= min && x <= max ? 1.0 : 0.0;
        }

        #endregion
    }
}
