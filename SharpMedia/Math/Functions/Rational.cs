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
    /// A rational function, written as r(x) = p(x)/q(x), where p and q are polynomials.
    /// </summary>
    public class Rational : IFunction
    {
        #region Private Members
        Polynomial p;
        Polynomial q;
        #endregion

        #region Properties

        /// <summary>
        /// The asimptote of rational.
        /// </summary>
        public Polynomial Asimptote
        {
            get
            {
                return p / q;
            }
        }

        /// <summary>
        /// The polynomial being divided.
        /// </summary>
        public Polynomial P
        {
            get
            {
                return p;
            }
        }

        /// <summary>
        /// Divisor polynomial.
        /// </summary>
        public Polynomial Q
        {
            get
            {
                return q;
            }
        }

        #endregion

        #region Roots and Poles

        /// <summary>
        /// All poles of rational function at interval.
        /// </summary>
        /// <param name="range">The interval.</param>
        /// <param name="error">Precission of calculation.</param>
        /// <returns>List of poles.</returns>
        public List<double> Poles(Intervald range, double error)
        {
            return q.RealRoots(range);
        }

        /// <summary>
        /// The poles of polynomial, with default precission calculation.
        /// </summary>
        /// <param name="range">The range where poles are to be found.</param>
        /// <returns>List of poles.</returns>
        public List<double> Poles(Intervald range)
        {
            return q.RealRoots(range);
        }

        /// <summary>
        /// All roots of rational function at interval.
        /// </summary>
        /// <param name="range">The interval.</param>
        /// <param name="error">Precission of calculation.</param>
        /// <returns>List of zeros.</returns>
        public List<double> Roots(Intervald range, double error)
        {
            return p.RealRoots(range);
        }

        /// <summary>
        /// The roots of polynomial, with default precission calculation.
        /// </summary>
        /// <param name="range">The range where poles are to be found.</param>
        /// <returns>List of zeros.</returns>
        public List<double> Roots(Intervald range)
        {
            return p.RealRoots(range);
        }

        #endregion

        #region Limit analysis

        /// <summary>
        /// The behavious at positive infinity.
        /// </summary>
        public double PositiveInfinity
        {
            get
            {
                if (p.Order > q.Order) return double.PositiveInfinity * (p[0] / q[0]);
                if (p.Order < q.Order) return 0.0;
                return p[0] / q[0];
            }
        }

        /// <summary>
        /// The behavious at positive infinity.
        /// </summary>
        public double NegativeInfinity
        {
            get
            {
                if (p.Order > q.Order) return double.NegativeInfinity * (p[0] / q[0]);
                if (p.Order < q.Order) return 0.0;
                return p[0] / q[0];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Ration constructed from two polynomials.
        /// </summary>
        /// <param name="polynomial">The upper polynomial.</param>
        /// <param name="divider">The divider polynomial.</param>
        public Rational(Polynomial polynomial, Polynomial divider)
        {
            p = polynomial;
            q = divider;
        }

        #endregion

        #region IFunctiond Members

        public Functiond Functiond
        {
            get 
            {
                Expression ex1 = Polynomial.ToExpression(p.Coefficients);
                Expression ex2 = Polynomial.ToExpression(q.Coefficients);

                Functiond f1 = ex1.GetFunctiond(ex1.Params, null);
                Functiond f2 = ex2.GetFunctiond(ex2.Params, null);

                return delegate(double x)
                {
                    return f1(x) / f2(x);
                };
            }
        }

        public Functionf Functionf
        {
            get 
            {
                Expression ex1 = Polynomial.ToExpression(p.Coefficients);
                Expression ex2 = Polynomial.ToExpression(q.Coefficients);

                Functionf f1 = ex1.GetFunctionf(ex1.Params, null);
                Functionf f2 = ex2.GetFunctionf(ex2.Params, null);

                return delegate(float x)
                {
                    return f1(x) / f2(x);
                };
            }
        }

        public double Eval(double x)
        {
            return Polynomial.Eval(p.Coefficients, x) / Polynomial.Eval(q.Coefficients, x);
            
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class RationaldTest
    {
        [CorrectnessTest]
        public void Construction()
        {

        }

    }
#endif
}
