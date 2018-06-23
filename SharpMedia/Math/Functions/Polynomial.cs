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
using SharpMedia.Math.Matrix;
using SharpMedia.AspectOriented;

namespace SharpMedia.Math.Functions
{

    /// <summary>
    /// A polynomal represents a polynomal of n-th degree. It is sometimes useful
    /// because we can find complex and real zeros of polynomals numerically within
    /// a given error range. Furthermore, we support polynomal division, integration,
    /// derivation and lots more.
    /// </summary>
    /// <remarks>
    /// We support polynomal definition in coeficient array manner or using polynomial
    /// class. In array notation, the first coeficient is the one belonging to the
    /// polynomal's highest degree, for example: p(x) = coef[0]*x^2 + coef[1]*x + coef[2]. Last
    /// coefficient is always constant factor.
    /// </remarks>
    public class Polynomial : IFunction
    {
        #region Private Data
        static double defaultError = 1e-5;
        #endregion

        #region Static Properties

        /// <summary>
        /// The default error tolerance, for root finding algorithms.
        /// </summary>
        public static double DefaultError
        {
            get { return defaultError; }
            set { defaultError = value; }
        }

        #endregion

        #region Specialized Root finders

        /// <summary>
        /// A root of linear polinom; e.g. 0 = k*x + x0 
        /// </summary>
        /// <param name="k">The linear factor.</param>
        /// <param name="x0">The constant factor.</param>
        /// <returns>The x at intersection.</returns>
        public static double Root(double k, double x0)
        {
            if (k != 0.0)
            {
                return -x0 / k;
            }
            else
            {
                // No intersection.
                if (x0 == 0.0) return 0.0;
                else
                {
                    return double.PositiveInfinity;
                }
            }
        }

        /// <summary>
        /// Quadratic equation root; e.g. 0 = a*x^2 + b*x + c. There may be either none or
        /// 2 results. This is because there can be only complex results. Use RootComplex to
        /// find complex results.
        /// </summary>
        /// <param name="x0">The first result, bigger.</param>
        /// <param name="x1">The second result.</param>
        /// <returns>True if roots were found.</returns>
        public static bool Roots(double a, double b, double c, out double x0, out double x1)
        {
            double D = b * b - 4.0 * a * c;
            if (D < 0.0) { x0 = x1 = double.NaN; return false; }
            double sD = global::System.Math.Sqrt(D);
            x0 = (-b + sD) / (2.0 * a);
            x1 = (-b - sD) / (2.0 * a);
            return true;
        }

        /// <summary>
        /// Quadratic equation root; e.g. 0 = a*x^2 + b*x + c. The solution is either both with
        /// real numbers (imagionary = 0) or both with complex numbers.
        /// </summary>
        /// <param name="x0">The first complex result.</param>
        /// <param name="x1">The second complex result.</param>
        public static void Roots(double a, double b, double c, out Complexd x0, out Complexd x1)
        {
            double D = b * b - 4.0 * a * c;
            if (D < 0.0)
            {
                double sD = global::System.Math.Sqrt(-D);
                x0 = new Complexd((-b) / (2.0 * a), sD / (2.0 * a));
                x1 = new Complexd((-b) / (2.0 * a), -sD / (2.0 * a));

            }
            else
            {
                double sD = global::System.Math.Sqrt(D);
                x0 = new Complexd((-b + sD) / (2.0 * a), 0.0);
                x1 = new Complexd((-b - sD) / (2.0 * a), 0.0);
            }
        }

        /// <summary>
        /// A root of linear polinom; e.g. 0 = k*x + x0 
        /// </summary>
        /// <param name="k">The linear factor.</param>
        /// <param name="x0">The constant factor.</param>
        /// <returns>The x at intersection.</returns>
        public static float Root(float k, float x0)
        {
            if (k != 0.0)
            {
                return -x0 / k;
            }
            else
            {
                // No intersection.
                if (x0 == 0.0f) return 0.0f;
                else
                {
                    return float.PositiveInfinity;
                }
            }
        }

        /// <summary>
        /// Quadratic equation root; e.g. 0 = a*x^2 + b*x + c. There may be either none or
        /// 2 results. This is because there can be only complex results. Use RootComplex to
        /// find complex results.
        /// </summary>
        /// <param name="x0">The first result, bigger.</param>
        /// <param name="x1">The second result.</param>
        /// <returns>True if roots were found.</returns>
        public static bool Roots(float a, float b, float c, out float x0, out float x1)
        {
            float D = b * b - 4.0f * a * c;
            if (D < 0.0f) { x0 = x1 = float.NaN; return false; }
            float sD = (float)global::System.Math.Sqrt(D);
            x0 = (-b + sD) / (2.0f * a);
            x1 = (-b - sD) / (2.0f * a);
            return true;
        }

        /// <summary>
        /// Quadratic equation root; e.g. 0 = a*x^2 + b*x + c. The solution is either both with
        /// real numbers (imagionary = 0) or both with complex numbers.
        /// </summary>
        /// <param name="x0">The first complex result.</param>
        /// <param name="x1">The second complex result.</param>
        public static void Roots(float a, float b, float c, out Complexf x0, out Complexf x1)
        {
            double D = b * b - 4.0 * a * c;
            if (D < 0.0f)
            {
                float sD = (float)global::System.Math.Sqrt(-D);
                x0 = new Complexf((-b) / (2.0f * a), sD / (2.0f * a));
                x1 = new Complexf((-b) / (2.0f * a), -sD / (2.0f * a));

            }
            else
            {
                float sD = (float)global::System.Math.Sqrt(D);
                x0 = new Complexf((-b + sD) / (2.0f * a), 0.0f);
                x1 = new Complexf((-b - sD) / (2.0f * a), 0.0f);
            }
        }

        #endregion

        #region Generic Root Finders

        /// <summary>
        /// Finds all xomplex roots of polinomial.
        /// </summary>
        /// <param name="coefficients">Coefficients of polynomail.</param>
        /// <returns></returns>
        public static Complexd[] Roots([NotNull] double[] coefficients)
        {
            return Roots(coefficients, defaultError);
        }

        /// <summary>
        /// Finds all complex roots of polinomial within certain error range.
        /// </summary>
        /// <param name="coefficients">Coefficients of polynomial.</param>
        /// <returns></returns>
        public static Complexd[] Roots([NotNull] double[] coeficients, double error)
        {
            throw new NotImplementedException();

        }

        public static List<double> RealRoots([NotNull] double[] coeficients)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all real roots of polynomial.
        /// </summary>
        /// <param name="coeficents">The coefficients od polynomial.</param>
        /// <returns></returns>
        public static List<double> RealRoots([NotNull] double[] coeficents, Intervald range)
        {
            return RealRoots(coeficents, range, defaultError);
        }

        /// <summary>
        /// Finds all real roots of polynomial.
        /// </summary>
        /// <param name="coeficents">The coefficients of polynomial.</param>
        /// <param name="error">The error tollerance, actual roots are further polished to give
        /// much better approximation (around 1/100 of unpolished).</param>
        /// <returns>List of zeros.</returns>
        public static List<double> RealRoots([NotNull] double[] coeficents, Intervald range, double error)
        {
            // For now, replace by Laguerre when implemented.
            return RootFinder.MultiBisectionAndPolish(
                  Polynomial.CreateFunctiond(coeficents),
                  Polynomial.CreateFunctiond(Polynomial.Differentiate(coeficents)),
                  range, error, error * 0.01);
            
        }

        #endregion

        #region Fitting

        /// <summary>
        /// Fits polynomial to points.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="degree"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static Polynomial Fit(Vector2d[] points, uint degree, out double sigma)
        {
            sigma = 0;
            return null;
        }

        /// <summary>
        /// Fits polynomial to points.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="degree"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static Polynomial Fit(Vector2f[] points, uint degree, out float sigma)
        {
            sigma = 0;
            return null;
        }

        #endregion

        #region Integration/Differentiation

        /// <summary>
        /// Integrates polynomial.
        /// </summary>
        /// <param name="coef">The coefficients of polinomial.</param>
        /// <returns>Coeficients of integrated polynomial.</returns>
        public static double[] Integrate([NotNull] double[] coef)
        {
            double[] p = new double[coef.Length + 1];
            int i;
            // We add all coeficients.
            for (i = 0; i < coef.Length; i++)
            {
                double factor = (double)(coef.Length - i);
                p[i] = coef[i] / factor;
            }

            // Nullify all other elements.
            for (; i < p.Length; i++) p[i] = 0.0;

            return p;
        }

        /// <summary>
        /// Differentiates the polynomial.
        /// </summary>
        /// <param name="coef">Coefficients of polynomial.</param>
        /// <returns>Array representing polynomial.</returns>
        public static double[] Differentiate([NotNull] double[] coef)
        {
            if(coef.Length <= 1) return new double[]{0.0};

            // Create result polynomial.
            double[] p = new double[coef.Length-1];

            // We differentiate.
            for(int i = 0; i < (coef.Length-1);i++)
            {
                p[i] = coef[i] * (double)(coef.Length-i-1);
            }

            // Return result.
            return p;
        }

        #endregion

        #region Evaluation

        /// <summary>
        /// Simple evaluation of polynomial at x. We use horner's formula for
        /// performance and stability. Note that if you are evaluating the polynomial
        /// more then once, use the "optimised" function form, because loop is
        /// unrolled and zero-based values are put away.
        /// </summary>
        /// <param name="coef">The coefficients.</param>
        /// <param name="x">The x, value to be evaluated.</param>
        /// <returns>Value of polynomial at x.</returns>
        public static double Eval([NotNull] double[] coef, double x)
        {
            double v = 0.0;
            for (int i = 0; i < coef.Length; i++)
            {
                v *= x;
                v += coef[i];
            }
            return v;
        }

        #endregion

        #region Operations

        /// <summary>
        /// Adds two polynomials.
        /// </summary>
        /// <param name="p1">First polynomial.</param>
        /// <param name="p2">Second polynomial.</param>
        /// <returns>Resulting polynomial.</returns>
        public static double[] Add([NotNull] double[] p1, [NotNull] double[] p2)
        {
            // We ensure p2 is bigger then p1.
            if(p1.Length > p2.Length) return Add(p2,p1);

            // We have polynomials, first polynomial 2 part only.
            double[] p = new double[p2.Length];

            // Polynomial part 2 only.
            int i, ld = p2.Length-p1.Length;
            for (i = 0; i < ld; i++)
            {
                p[i] = p2[i];
            }

            // Common part.
            for (; i < p2.Length; i++)
            {
                p[i] = p1[i - ld] + p2[i];
            }

            // Return polynomial.
            return p;

        }

        /// <summary>
        /// Substracts two polynomials.
        /// </summary>
        /// <param name="p1">First polynomial.</param>
        /// <param name="p2">Second polynomial.</param>
        /// <returns>Resulting polynomial.</returns>
        public static double[] Substract([NotNull] double[] p1, [NotNull] double[] p2)
        {
            // We ensure p2 is bigger then p1.
            if (p1.Length > p2.Length)
            {
                // We have polynomials, first polynomial 2 part only.
                double[] p = new double[p1.Length];

                // Polynomial part 2 only.
                int i, ld = p1.Length - p2.Length;
                for (i = 0; i < ld; i++)
                {
                    p[i] = p1[i];
                }

                // Common part.
                for (; i < p1.Length; i++)
                {
                    p[i] = p1[i] - p2[i - ld];
                }

                // Return polynomial.
                return p;
            }
            else
            {

                // We have polynomials, first polynomial 2 part only.
                double[] p = new double[p2.Length];

                // Polynomial part 2 only.
                int i, ld = p2.Length - p1.Length;
                for (i = 0; i < ld; i++)
                {
                    p[i] = -p2[i];
                }

                // Common part.
                for (; i < p2.Length; i++)
                {
                    p[i] = p1[i - ld] - p2[i];
                }

                // Return polynomial.
                return p;
            }
        }

        /// <summary>
        /// Multiplies two polynomials.
        /// </summary>
        /// <param name="p1">First polynomial.</param>
        /// <param name="p2">Second polynomial.</param>
        /// <returns></returns>
        public static double[] Multiply([NotNull] double[] p1, [NotNull] double[] p2)
        {
            // We create polynomial.
            uint order = (uint)(p1.Length + p2.Length - 1);

            // We initialize result.
            double[] result = new double[order];
            result.Initialize();

            for (int i = 0; i < p1.Length; i++)
            {
                for (int j = 0; j < p2.Length; j++)
                {
                    result[i + j] += p1[i] * p2[j];
                }

            }

            return result;
        }

        /// <summary>
        /// Composition of tow polynomials, p2 is inserted as x in p1.
        /// </summary>
        /// <param name="p1">First polynomial.</param>
        /// <param name="p2">Second polynomial.</param>
        /// <returns></returns>
        public static double[] Composition([NotNull] double[] p1, [NotNull] double[] p2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Division of two polynomals, such that:
        /// p1(x) = q(x)*p2(x) + r(x).
        /// </summary>
        /// <param name="p1">The base polynomial.</param>
        /// <param name="p2">The other polynomial.</param>
        /// <param name="remainder">Remainder of division.</param>
        /// <returns>The whole part of division.</returns>
        public static double[] Divide([NotNull] double[] p1, [NotNull] double[] p2, out double[] remainder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Division by linear polynomial (x - c), such that:
        /// p(x) = (x-c) * q(x) + p(c), where p(c) is remainder and q(x) is divided polynomial.
        /// </summary>
        /// <param name="p">Input polynomial.</param>
        /// <param name="c">The constant factor for division.</param>
        /// <param name="remainder">Output remainder.</param>
        /// <returns></returns>
        public static double[] HornerDivision([NotNull] double[] p, double c, out double remainder)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Optimisations

        internal static Expression ToExpression(double[] coef)
        {
            // A constant function.
            if (coef.Length == 0) return new Expression(new Expression.IdElement(0.0));

            // We use Horner's formula, to ensure stability and speed.
            Expression.IElement root = new Expression.IdElement(coef[0]);

            for (int i = 1; i < coef.Length; i++)
            {
                // We multiply by x.
                root = new Expression.Multiply(root, new Expression.IdElement("x"));

                // We add the next constant factor.
                root = new Expression.Add(root, new Expression.IdElement(coef[i]));
            }

            return new Expression(root);
        }

        /// <summary>
        /// Optimized function evaluation support.
        /// </summary>
        /// <param name="coef"></param>
        /// <returns></returns>
        public static Functiond CreateFunctiond(double[] coef)
        {
            Expression ex = ToExpression(coef);
            return ex.GetFunctiond(ex.Params, null);
        }

        /// <summary>
        /// Dynamic conversion, if you need less precission.
        /// </summary>
        /// <param name="coef"></param>
        /// <returns></returns>
        public static Functionf CreateFunctionf(double[] coef)
        {
            Expression ex = ToExpression(coef);
            return ex.GetFunctionf(ex.Params, null);
        }

        #endregion

        #region Private Members
        double[] coefficients;
        #endregion

        #region Properties

        /// <summary>
        /// The order of polynomial.
        /// </summary>
        public uint Order
        {
            get { return (uint)coefficients.Length - 1; }
        }

        /// <summary>
        /// The coefficients of polynomial.
        /// </summary>
        public double[] Coefficients
        {
            get { return coefficients; }
        }

        /// <summary>
        /// Polynomial index access.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public double this[uint index]
        {
            get
            {
                if (index >= coefficients.Length)
                {
                    throw new ArgumentException("Index out of polynomial degree range..");
                }
                return coefficients[index];
            }
            set
            {
                if (index >= coefficients.Length)
                {
                    // We allow out of range if it is zero element.
                    if (value == 0.0) return;
                    throw new ArgumentException("Index out of polynomial degree range..");
                }
                coefficients[index] = value;
            }
        }

        /// <summary>
        /// Integral of polynomial.
        /// </summary>
        public Polynomial Integral
        {
            get { return new Polynomial(Integrate(coefficients)); }
        }

        /// <summary>
        /// Differential of polynomial.
        /// </summary>
        public Polynomial Differential
        {
            get { return new Polynomial(Differentiate(coefficients)); }
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Real roots of polynomial.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="precission">Precission.</param>
        /// <returns>The list of real zeros.</returns>
        public List<double> RealRoots(Intervald interval, double error)
        {
            return Polynomial.RealRoots(coefficients, interval, error);
        }

        /// <summary>
        /// Real roots of polynomial.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="precission">Precission.</param>
        /// <returns>The list of real zeros.</returns>
        public List<double> RealRoots(Intervald interval)
        {
            return Polynomial.RealRoots(coefficients, interval);
        }

        /// <summary>
        /// Polynomial, defined with coefficients.
        /// </summary>
        /// <param name="coef">Collection of coefficients, in descending x power
        /// order.</param>
        public Polynomial([NotNull] params double[] coef)
        {
            coefficients = coef;
        }

        /// <summary>
        /// Construction of polynomial with degree.
        /// </summary>
        /// <param name="degree">The size of polynomial with degree.</param>
        public Polynomial(uint degree)
        {
            coefficients = new double[degree+1];
        }


        public override string ToString()
        {
            StringBuilder b = new StringBuilder(Coefficients.Length * 10);

            for (int i = 0; ; )
            {
                b.Append(coefficients[i]);
                b.Append("*x^");
                b.Append(Coefficients.Length-i-1);
                ++i;
                if (i < Coefficients.Length) b.Append(" + ");
                else break;
            }

            return b.ToString();
        }

        #endregion

        #region Member Operators

        /// <summary>
        /// Horner division of polynomial.
        /// </summary>
        /// <param name="c">The constant factor.</param>
        /// <param name="rem">Remainder of division, also f(c).</param>
        /// <returns>Polynomial.</returns>
        public Polynomial HornerDivide(double c, out double rem)
        {
            return new Polynomial(Polynomial.HornerDivision(coefficients, c, out rem));
        }

        /// <summary>
        /// Multiplication of polynomials.
        /// </summary>
        /// <param name="p1">The first polynomial.</param>
        /// <param name="p2">The second polynomial.</param>
        /// <returns>Resulting polynomial.</returns>
        public static Polynomial operator *([NotNull] Polynomial p1, [NotNull] Polynomial p2)
        {
            return new Polynomial(Multiply(p1.Coefficients, p2.Coefficients));
        }

        /// <summary>
        /// Substraction of polynomials.
        /// </summary>
        /// <param name="p1">The first polynomial.</param>
        /// <param name="p2">The second polynomial.</param>
        /// <returns>Resulting polynomial.</returns>
        public static Polynomial operator -([NotNull] Polynomial p1, [NotNull] Polynomial p2)
        {
            return new Polynomial(Substract(p1.Coefficients, p2.Coefficients));
        }

        /// <summary>
        /// Addition of polynomials.
        /// </summary>
        /// <param name="p1">The first polynomial.</param>
        /// <param name="p2">The second polynomial.</param>
        /// <returns>Resulting polynomial.</returns>
        public static Polynomial operator +([NotNull] Polynomial p1, [NotNull] Polynomial p2)
        {
            return new Polynomial(Add(p1.Coefficients, p2.Coefficients));
        }

        /// <summary>
        /// Division of polynomials. Remainder is ignored.
        /// </summary>
        /// <param name="p1">The first polynomial.</param>
        /// <param name="p2">The second polynomial.</param>
        /// <returns>Resulting polynomial.</returns>
        public static Polynomial operator /([NotNull] Polynomial p1, [NotNull] Polynomial p2)
        {
            double[] o;
            return new Polynomial(Divide(p1.Coefficients, p2.Coefficients, out o));
        }

        /// <summary>
        /// Equals operator.
        /// </summary>
        public static bool operator ==([NotNull] Polynomial p1, [NotNull] Polynomial p2)
        {
            if (p1.Coefficients.Length != p2.Coefficients.Length) return false;

            for (int i = 0; i < p1.Coefficients.Length; i++)
            {
                if (p1.Coefficients[i] != p2.Coefficients[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Defferent operator.
        /// </summary>
        public static bool operator !=([NotNull] Polynomial p1, [NotNull] Polynomial p2)
        {
            return !(p1 == p2);
        }

        /// <summary>
        /// Are polynomials nearly equal.
        /// </summary>
        /// <param name="p1">First polynomial.</param>
        /// <param name="p2">Second polynomial.</param>
        /// <returns>Are they nearly equal.</returns>
        public static bool NearEqual([NotNull] Polynomial p1, [NotNull] Polynomial p2)
        {
            if (p1.Coefficients.Length != p2.Coefficients.Length) return false;

            for (int i = 0; i < p1.Coefficients.Length; i++)
            {
                if(!MathHelper.NearEqual(p1.Coefficients[i], p2.Coefficients[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Are polynomials nearly equal, given maximum error.
        /// </summary>
        /// <param name="p1">First polynomial.</param>
        /// <param name="p2">Second polynomial.</param>
        /// <param name="eps">Maximum difference of component.</param>
        /// <returns>Are they nearly equal.</returns>
        public static bool NearEqual([NotNull] Polynomial p1, [NotNull] Polynomial p2, double eps)
        {
            if (p1.Coefficients.Length != p2.Coefficients.Length) return false;

            for (int i = 0; i < p1.Coefficients.Length; i++)
            {
                if (!MathHelper.NearEqual(p1.Coefficients[i], p2.Coefficients[i], eps)) return false;
            }
            return true;
        }

        public override bool Equals([NotNull] object obj)
        {
            if (obj is Polynomial)
            {
                return this == (Polynomial)obj;
            }
            throw new ArgumentException("Invalid argument.");
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Full division of polynomals.
        /// </summary>
        /// <param name="p">The other polynomal.</param>
        /// <param name="remainder">Remainder, must be of degree p-1.</param>
        /// <returns></returns>
        public Polynomial Divide([NotNull] Polynomial p, [NotNull] Polynomial remainder)
        {
            return new Polynomial(Divide(Coefficients, p.Coefficients, out remainder.coefficients));
        }

        #endregion

        #region Construction from points

        /// <summary>
        /// Constructs polynomiald from (x,y) pairs.
        /// </summary>
        /// <param name="xy">The points, no x must be duplicated.</param>
        /// <returns>Polynomial through all points.</returns>
        /// <remarks>This method should be used only if a point set is big enough. Use
        /// specialised linear/quadratic/cubic interpolations if not many control
        /// points are used.</remarks>
        public static Polynomial FromPoints([NotEmptyArray] Vector2d[] xy)
        {

            // The theorem is that for n points, there is a polynomial of n-th degree
            // through those points. We can write p(x) = y and we must thus solve
            // a linear system. 
            // | x0^n .... x0 1 | * | a0 | = | y0 |
            // | x1^n .... x1 1 | * | a1 | = | y1 |
            // ...
            // | xn^n .... xn 1 | * | an | = | yn |
            // If needed in future, other faster algorithms may be used for construction.
            // using VandermondeMatrix class.
            uint degree = (uint)xy.Length;
            DenseMatrixd matrix = new DenseMatrixd(degree, degree);

            // We fill the matrix.
            for (uint i = 0; i < degree; i++)
            {
                double c = 1.0;
                for (int j = (int)degree-1; j >= 0; j--)
                {
                    matrix[i, (uint)j] = c;

                    // We must multiply the matrix
                    c *= xy[i].X;
                }
            }

            // We fill constants.
            Vectord constants = new Vectord(degree);
            for (uint j = 0; j < degree; j++)
            {
                constants[j] = xy[j].Y;
            }

            // We solve the system.
            // FIXME: If this method will be used many times, we can provide optimisations since
            // this is Vandermonde matrix.
            Vectord solution = LinearSolver.SolveSystem(matrix, constants);

            // We now pack it to vector ( we reuse array).
            return new Polynomial(solution.Components);
        }

        #endregion

        #region IFunctiond Members

        public Functiond Functiond
        {
            get { return CreateFunctiond(coefficients); }
        }

        public Functionf Functionf
        {
            get { return CreateFunctionf(coefficients); }
        }

        public double Eval(double x)
        {
            return Eval(coefficients, x);
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class PolinomialdTest
    {
        Polynomial p1 = new Polynomial(1, 5, 7);
        Polynomial p2 = new Polynomial(3, 4, 1, 7);
        Polynomial p3 = new Polynomial(1, 0);

        [CorrectnessTest]
        public void LinearRoot() { Assert.AreEqual(Polynomial.Root(1.0, 1.0), -1.0); }
        [CorrectnessTest]
        public void QuadraticRoot()
        {
            double x1, x2;
            Assert.IsTrue(Polynomial.Roots(1.0, 0.0, 0.0, out x1, out x2));
            Assert.AreEqual(x1, 0.0);
            Assert.AreEqual(x2, 0.0);
        }
        [CorrectnessTest]
        public void CQuadraticRoot()
        {
            Complexd c1, c2; Polynomial.Roots(1.0, 0.0, 1.0, out c1, out c2);
            Assert.AreEqual(c1, new Complexd(0.0, 1.0));
            Assert.AreEqual(c2.Conjugate, c1);
        }


        [CorrectnessTest]
        public void Add()
        {
            Assert.IsTrue(Polynomial.NearEqual(p1 + p2, new Polynomial(3, 5, 6, 14)));
        }

        [CorrectnessTest]
        public void Mul()
        {
            Assert.IsTrue(Polynomial.NearEqual(p1 * p3, new Polynomial(1, 5, 7, 0)));
        }

        [CorrectnessTest]
        public void Sub()
        {
            Assert.IsTrue(Polynomial.NearEqual(p1 - p2, new Polynomial(-3, -3, 4, 0)));
        }

        [CorrectnessTest]
        public void FromPoints()
        {
            Polynomial p = Polynomial.FromPoints(new Vector2d[]
                                { new Vector2d(-1, 0), new Vector2d(0, 1), new Vector2d(1, 0) });

            Assert.IsTrue(Polynomial.NearEqual(p, new Polynomial(-1, 0, 1)));
        }

        [CorrectnessTest]
        public void RootN()
        {
            // Roots are 1,-1 and -2.
            List<double> r = Polynomial.RealRoots(new double[] { 1, 2, -1, -2 }, new Intervald(-10, 10));
            r.Sort();
            Assert.IsTrue(r.Count == 3);
            
        }

        [CorrectnessTest]
        public void ToExpression()
        {
            Polynomial p = new Polynomial(1.0, -2.0, 1.0);
            Functiond f = p.Functiond;
            Assert.AreEqual(1.0, f(2.0));
        }
    }
#endif
}
