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
using SharpMedia.AspectOriented;

namespace SharpMedia.Math.Functions.Distributions
{
    /// <summary>
    /// A Gaussian (normal) distribution function, if form:
    /// g(x) = (1/2pi*sigma) * e^(- (x-ni)^2/(2*sigma^2))
    /// </summary>
    public class GaussianDistribution : IDistributionFunction
    {
        #region Private Members
        static Expression ex = Expression.Parse(
            "1.0 / (2.0*pi*sigma) * e^(-(x-ni)*(x-ni) / (2.0*sigma*sigma))");
        double sigma = 1.0;
        double ni = 0.0;
        #endregion

        #region Properties

        /// <summary>
        /// The sigma value, controlling the shape of function.
        /// </summary>
        public double Sigma
        {
            get { return sigma; }
            set { sigma = value; }
        }

        /// <summary>
        /// The Ni value, controlling the position of highest value.
        /// </summary>
        public double Ni
        {
            get { return ni; }
            set { ni = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Gaussian distribution with center 0.
        /// </summary>
        /// <param name="s"></param>
        public GaussianDistribution(double s)
            : this(s, 0.0)
        {
        }

        /// <summary>
        /// Constructor with gaussian distribution s and n parameters.
        /// </summary>
        /// <param name="s">The sigma, controlling shape.</param>
        /// <param name="n">The center position.</param>
        public GaussianDistribution(double s, double n)
        {
            sigma = s;
            ni = n;
        }

        /// <summary>
        /// Constructs best fitting gaussian disribution through points.
        /// </summary>
        /// <param name="xy">The points.</param>
        /// <returns>Gaussian distribution.</returns>
        public static GaussianDistribution FromPoints([NotEmptyArray] Vector2d[] xy)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IProbabilityFunction Members

        public Functiond Comulative
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public double Mean
        {
            get { return ni; }
        }

        public double Median
        {
            get { return ni; }
        }

        public double Mode
        {
            get { return ni; }
        }

        public double Variance
        {
            get { return sigma * sigma; }
        }

        #endregion

        #region IFunction Members

        public Functiond Functiond
        {
            get 
            {
                // We construct it.
                Expression.FunctionParams p = ex.GetParams(false);
                p["sigma"] = sigma;
                p["ni"] = ni;
                p["pi"] = MathHelper.PI;
                p.SetBinding("x", ex.Variable(0));

                // We construct function.
                return ex.GetFunctiond(p, FunctionSet.Default);
            }
        }

        public Functionf Functionf
        {
            get
            {
                // We construct it.
                Expression.FunctionParams p = ex.GetParams(false);
                p["sigma"] = sigma;
                p["ni"] = ni;
                p["pi"] = MathHelper.PI;
                p.SetBinding("x", ex.Variable(0));

                // We construct function.
                return ex.GetFunctionf(p, FunctionSet.Default);
            }
        }

        public double Eval(double x)
        {
            return 1.0 / (global::System.Math.Sqrt(2.0 * MathHelper.PI) * sigma) *
                          global::System.Math.Exp(-(x - ni) * (x - ni) / (2.0 * sigma * sigma));
                    
        }

        #endregion

    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class GaussianDistribtiondTest
    {
        [CorrectnessTest]
        public void Properties()
        {
            GaussianDistribution d = new GaussianDistribution(1.5, 0.5);
            Assert.AreEqual(0.5, d.Mean);
            Assert.AreEqual(1.5, d.Sigma);
            Assert.AreEqual(0.5, d.Ni);
            Assert.AreEqual(0.5, d.Mode);
            Assert.AreEqual(2.25, d.Variance);
        }

        [CorrectnessTest]
        public void Values()
        {
            GaussianDistribution g = new GaussianDistribution(1.0, 0.5);
            Functiond f1 = g.Functiond;
            Functiond f2 = g.Functiond;
            Assert.AreEqual(f1(2.0), f2(2.0));
        }
    }
#endif
}
