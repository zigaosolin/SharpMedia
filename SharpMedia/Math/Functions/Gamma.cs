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

namespace SharpMedia.Math.Functions
{

    /// <summary>
    /// A gamma function implementation.
    /// </summary>
    public sealed class Gamma : IFunction
    {
        #region Private Members
        static readonly float[] coefF = {76.18009172947148f, -86.50532032941667f,
            14.01409824083091f, -1.231739572450155f, 0.1208650973866179e-2f,
            -0.5395239384953e-5f };
        static readonly double[] coefD = {76.18009172947148, -86.50532032941667,
            14.01409824083091, -1.231739572450155, 0.1208650973866179e-2,
            -0.5395239384953e-5 };
        #endregion

        #region Static Members

        /// <summary>
        /// Calculates gama of x.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Eval(float x)
        {
            if (x < 0.0f)
            {
                throw new NotImplementedException();
            }
            else
            {
                return MathHelper.Exp(EvalLn(x));
            }
        }

        /// <summary>
        /// Calculates ln(gama(x)) for positive x.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float EvalLn(float x)
        {
            float t = x + 5.5f;
            t = t - (x + 0.5f) * MathHelper.Ln(t);

            float s = 1.000000000190015f;
            float t2 = x;
            for (int i = 0; i < 6; i++) s += coefF[i] / (++t2);
            return t - MathHelper.Ln(2.50662827463100005f * s / x);
        }

        /// <summary>
        /// Calculates gama of x.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Eval(double x)
        {
            if (x < 0.0f)
            {
                throw new NotImplementedException();
            }
            else
            {
                return MathHelper.Exp(EvalLn(x));
            }
        }

        /// <summary>
        /// Calculates ln(gama(x)).
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double EvalLn(double x)
        {
            double t = x + 5.5;
            t = t - (x + 0.5) * MathHelper.Ln(t);

            double s = 1.000000000190015;
            double t2 = x;
            for (int i = 0; i < 6; i++) s += coefD[i] / (++t2);
            return t - MathHelper.Ln(2.50662827463100005 * s / x);
        }

        #endregion

        #region IFunction Members

        public Functiond Functiond
        {
            get { return delegate(double d) { return Gamma.Eval(d); }; }
        }

        public Functionf Functionf
        {
            get { return delegate(float f) { return Gamma.Eval(f); }; }
        }

        double IFunction.Eval(double x)
        {
            return Gamma.Eval(x);
        }

        #endregion

    }
}
