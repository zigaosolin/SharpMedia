// This file was generated by TemplateEngine from template source 'QuadraticIntegrator'
// using template 'QuadraticIntegratord. Do not modify this file directly, modify it from template source.

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Math.Integration
{

    /// <summary>
    /// A quadratic integrator (1D functions only).
    /// </summary>
    public class QuadraticIntegratord : IIntegratord
    {
        #region Private Members
        Functiond func;
        uint refCount = 0;
        double integral = 0.0f;
        double prevIntegral = double.PositiveInfinity;
        Intervald range;
        #endregion

        #region Constructor

        /// <summary>
        /// Quadratic integrator.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="range"></param>
        public QuadraticIntegratord(Functiond func, Intervald range)
        {
            this.func = func;
            this.range = range;
        }

        #endregion

        #region IIntegrator Members

        public uint RefinementCount
        {
            get { return refCount; }
        }

        public bool Refine(uint maxSamples)
        {
            // We perform refinement.
            refCount++;
            double t = integral;

            // Cannot continue with so few samples.
            if (maxSamples / 3 < (1 << (int)refCount)) return false;

            // Special case first step.
            if (refCount == 1)
            {
                integral = (double)0.5 * range.Range * (func(range.A) + func(range.B));
            }
            else
            {
                integral = 0.0;
                double h = (range.Range / (1 << (int)refCount));
                double b, c, d;
                for (double a = range.A; a < range.B; a += h)
                {
                    // We get the c.
                    c = a + h > range.B ? range.B : a + h;
                    d = (c - a);
                    b = a + d * (double)0.5;

                    // We approximate function on interval by trepezoid.
                    integral += (double)1.0 / (double)3.0 * func(a)
                        + (double)4.0 / (double)3.0 * func(b) + (double)1.0 / (double)3.0 * func(c) * d;
                }
            }

            prevIntegral = t;
            return true;
        }

        public double IntegralApproximation
        {
            get { return integral; }
        }

        public double ErrorEstimate
        {
            get { return prevIntegral - integral; }
        }

        #endregion
    }
}