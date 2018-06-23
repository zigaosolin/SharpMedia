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

namespace SharpMedia.Math.Functions
{
    /// <summary>
    /// Root finder can find roots of unknown functions.
    /// </summary>
    public static class RootFinder
    {
        #region Private Members
        const uint MaxTangentIterations = 5000;
        #endregion


        #region Double version of methods

        /// <summary>
        /// A multi-bisection algorithm, usually finds all roots of function if not too close together.
        /// </summary>
        /// <param name="f">The function.</param>
        /// <param name="interval">Interval where to search for roots.</param>
        /// <param name="samples">Number of samples.</param>
        /// <param name="maxError">Maximum error tolerance.</param>
        /// <returns>List of all roots.</returns>
        public static List<double> MultiBisection([NotNull] Functiond f, Intervald interval, double maxError)
        {
            // We do progressivelly for all values.
            List<double> roots = new List<double>();

            // We first create iteration values.
            double a = interval.A, fa = f(interval.A);
            double b = interval.A + maxError;

            // We iterate through all values.
            for (double fb; b <= interval.B; b += maxError)
            {
                // Calculate fb first.
                fb = f(b);

                // We check it there is a root between a and b.
                if (fa * fb < 0.0)
                {
                    // We have a root or singularity. We do not check for
                    // singularity, we add it to the list.
                    roots.Add(0.5 * (a + b));
                }

                // We iterate to next.
                a = b;
                fa = fb;
            }

            return roots;
        }

        /// <summary>
        /// A combination of bisection and tangent (Newton) method. Bisection is used to determine all
        /// possible zeros and then additional steps are made to convengance to those solutions using Newton's
        /// method.
        /// </summary>
        /// <param name="f">The actual function.</param>
        /// <param name="df">The derivate of function.</param>
        /// <param name="interval">The interval of method.</param>
        /// <param name="maxBError">Maximum bisection error.</param>
        /// <param name="maxPError">Maximum polishing error.</param>
        /// <returns></returns>
        public static List<double> MultiBisectionAndPolish([NotNull] Functiond f, [NotNull] Functiond df, 
            Intervald interval, double maxBError, double maxPError)
        {
            // We do progressivelly for all values.
            List<double> roots = new List<double>();

            // We first create iteration values.
            double a = interval.A, fa = f(interval.A);
            double b = interval.A + maxBError;

            // We iterate through all values.
            for (double fb; b <= interval.B; b += maxBError)
            {
                // Calculate fb first.
                fb = f(b);

                // We check it there is a root between a and b.
                if (fa * fb < 0.0)
                {
                    // We have a root or singularity. We do not check for
                    // singularity, we add it to the list.
                    roots.Add(TangentRoot(f, df, 0.5 * (a + b), maxPError));
                }

                // We iterate to next.
                a = b;
                fa = fb;
            }

            return roots;
        }



        /// <summary>
        /// A normal bisection method. The finding needs quite a few interations because
        /// the convengence is guaranteed but slow.
        /// </summary>
        /// <param name="f">The function.</param>
        /// <param name="resolution">Number of interations.</param>
        /// <param name="maxError">The maximum error.</param>
        /// <returns>Whe x where the approcimate root exists.</returns>
        public static double Bisection([NotNull] Functiond f, Intervald interval, double maxError)
        {
            // Actual error is multiplied by 2.0, because we use A+b average.
            maxError *= 2.0;
            double A = interval.A, B = interval.B;
            double fA = f(A);
            double fB = f(B);
            if (fA * fB > 0.0)
            {
                throw new ArgumentException("The bisection can only be performed on interval, where one side " +
                                            "is more then 0.0 and one is less the 0.0.");
            }

            double C = 0.0, fC = 0.0;

            // We interate.
            for (; ; )
            {
                C = (A + B) * 0.5;
                fC = f(C);

                // We check what kind we have.
                if (fC > 0.0)
                {
                    if (fA > 0.0)
                    {
                        fA = fC;
                        A = C;
                    }
                    else
                    {
                        fB = fC;
                        B = C;
                    }
                }
                else
                {
                    if (fA > 0.0)
                    {
                        fB = fC;
                        B = C;
                    }
                    else
                    {
                        fA = fC;
                        A = C;
                    }
                }

                // Check if condition is met.
                if (B - A < maxError) break;
            }

            // Return best approximation.
            return (A + B) * 0.5;
        }

        /// <summary>
        /// The tangent method. It requires the derivate and is not effective in some situations;
        /// e.g. may run out of maximum iterations.
        /// </summary>
        /// <param name="f">The function.</param>
        /// <param name="df">The derivate of function.</param>
        /// <param name="guess">First guess.</param>
        /// <param name="eps">The maximum difference between 2 approximates.</param>
        /// <returns>The approximate root.</returns>
        public static double TangentRoot([NotNull] Functiond f, [NotNull] Functiond df, 
                                        double guess, double eps)
        {
            double fy, dfy, x;
            

            for (int i = 0; i < MaxTangentIterations; i++)
            {
                // We calculate f(x) and f'(x)
                fy = f(guess);
                dfy = df(guess);

                // New guess.
                x = (dfy * guess - fy) / dfy;

                // Check if ended.
                if (MathHelper.NearEqual(x, guess, eps))
                {
                    return x;
                }

                guess = x;
            }

            throw new InvalidOperationException("Tangent root could not be located " +
                "because the Newtonian method did not convergence.");
        }


        /// <summary>
        /// Finds roots using Brent's method.
        /// </summary>
        /// <param name="f">The function.</param>
        /// <param name="interval">Interval where to search.</param>
        /// <param name="eps">The maximum allowed error.</param>
        /// <returns>The root.</returns>
        public static double BrentRoot([NotNull] Functiond f, Intervald interval, double eps)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class RootTester
    {

        [CorrectnessTest]
        public void Bisection()
        {
            Assert.IsTrue(MathHelper.NearEqual(RootFinder.Bisection(delegate(double x)
                   { return x * x - 1.0; }, new Intervald(0.0, 10.0), 0.019), 1.0, 0.02));
        }
        [CorrectnessTest]
        public void TangentRoot()
        {
            Assert.IsTrue(MathHelper.NearEqual(RootFinder.TangentRoot(delegate(double x) { return x * x - 1.0; },
                delegate(double x) { return 2 * x; }, 2.0, 0.001), 1.0, 0.0011));
        }

        [CorrectnessTest]
        public void MultiBisection()
        {
            List<double> roots = RootFinder.MultiBisection(delegate(double x) 
            { return x * x * x + 2.0 * x * x - x - 2.0; }, new Intervald(-4.0, 2.0), 0.01);
            roots.Sort();

            Assert.AreEqual(3, roots.Count);
            Assert.IsTrue(MathHelper.NearEqual(roots[0], -2.0, 0.01));
            Assert.IsTrue(MathHelper.NearEqual(roots[1], -1.0, 0.01));
            Assert.IsTrue(MathHelper.NearEqual(roots[2], 1.0, 0.01));
        }
    }

#endif
}
