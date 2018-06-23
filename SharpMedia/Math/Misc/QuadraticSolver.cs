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

namespace SharpMedia.Math.Misc
{

    /// <summary>
    /// Can solve some systems of quadratic equations. Any system of quadratic equations
    /// solver should be placed in this class.
    /// </summary>
    public static class QuadraticSolver
    {
        /// <summary>
        /// Solves a system of two quadratic equations in such form:
        /// a1*x^2 + b1*x + c1*y^2 + d1*y = e1
        /// a2*x^2 + b2*x + c2*y^2 + d2*y = e2
        /// 
        /// Returned values are x/y pairs (as 2D points). All complex solutions are discarded.
        /// </summary>
        /// <returns>All real solutions to the problem.</returns>
        public static List<Vector2d> Solve(double a1, double b1, double c1, double d1, double e1,
                                       double a2, double b2, double c2, double d2, double e2)
        {
            if (!MathHelper.NearEqual(c1, 0.0))
            {
                // We invert the order because we want in such form!
                if (MathHelper.NearEqual(c2, 0.0))
                {
                    return Solve(a2, b2, c2, d2, e2,
                                 a1, b1, c1, d1, e1);
                }

                // We need to zero out c1, we substract equations.
                double f = c2 / c1;
                a1 -= f * a2;
                b1 -= f * b2;
                c1 = 0.0;
                d1 -= f * d2;
                e1 -= f * e2;
            }

            // We have to solve equation in form for y.
            // a1*x^2 + b1*x + d1*y = e1;
            // y = (e1 - a1*x^2 - b1*x) / (d1)
            // Plugging into second, we obtain:
            // a2*x^2 + b2*x + c2*( (e1- a1*x^2 - b1*x) / d1)^2 + d2*(e1 - a1*x^2 + b1*x)/d1 = e2.
            // After solving, we get polynomial of forth degree.
            List<double> roots = Functions.Polynomial.RealRoots(new double[]{
                c2*a1*a1/d1,
                2.0*c2*a1*b1/d1,
                a2-c2*(b1*b1-2.0*e1*a1)/d1 - d2*a1/d1,
                b2-2.0*c2*b1*e1/d1-d2*b1/d1,
                -e2+e1*e1*c2/d1+d2*e1/d1});

            // We construct list with maximum number of elements (max is 8).
            List<Vector2d> results = new List<Vector2d>(roots.Count * 2);

            // We find all y's and out of that solutions.
            for (int i = 0; i < roots.Count; i++)
            {
                double x0 = roots[i];
                double y0, y1;

                // We solve equation plugging x in, we solve it for y.
                // a2*x0^2 + b2*x0 + c2*y^2 + d2*y = e2
                if (Functions.Polynomial.Roots(c2, d2, a2 * x0 * x0 + b2 * x0 - e2, out y0, out y1))
                {
                    results.Add(new Vector2d(x0, y0));
                    results.Add(new Vector2d(x0, y1));
                }
            }

            return results;
        }

    }
}
