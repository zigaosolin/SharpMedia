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
using SharpMedia.Math.Matrix;

namespace SharpMedia.Math.Matrix
{
    /// <summary>
    /// Can solve linear systems. Input is allowed in system form
    /// for small systems and in matrix form for bigger systems.
    /// </summary>
    public static class LinearSolver
    {
        #region Double Version

        /// <summary>
        /// We solve the system of equations, defined as:
        /// M * X = C.
        /// </summary>
        /// <param name="S">The system in matrix form.</param>
        /// <param name="C">constants</param>
        /// <returns></returns>
        public static Vector3d SolveSystem(Matrix3x3d S, Vector3d C)
        {
            return S.Inverse * C;
        }

        /// <summary>
        /// Solves a 2D system.
        /// </summary>
        /// <param name="S">The coefficients.</param>
        /// <param name="C">The constans.</param>
        /// <returns>Vector of solved values.</returns>
        public static Vector2d SolveSystem(Matrix2x2d S, Vector2d C)
        {
            return S.Inverse * C;
        }


        /// <summary>
        /// Solves system using coefficients.
        /// </summary>
        public static Vector2d SolveSystem(double m00, double m01, double m10, double m11, double cx, double cy)
        {
            // Inverse determinant of matrix.
            double d_inv = 1.0 / (m00 * m11 - m01 * m10);
            if (double.IsInfinity(d_inv)) throw new ArgumentException("No solutions of system.");

            // We make calculations as roboust as possible, we create no matrix.
            return new Vector2d(d_inv * (m11 * cx - m10 * cy), d_inv * (-m01 * cx + m00 * cy));
        }

        /// <summary>
        /// Prepares system for solving many times with different data.
        /// </summary>
        /// <param name="S">The system.</param>
        /// <returns>Delegate capable of doing such work.</returns>
        public static Vector3d.Processor GetSolutionProcessorOf(Matrix3x3d S)
        {
            Matrix3x3d inverse = S.Inverse;
            return delegate(Vector3d x)
            {
                return inverse * x;
            };
        }

        /// <summary>
        /// Solves the system.
        /// </summary>
        /// <param name="S">The matrix.</param>
        /// <param name="C">Constants.</param>
        /// <returns></returns>
        public static Vectord SolveSystem(Matrix.DenseMatrixd S, Vectord C)
        {
            return S.Inverse * C;
        }

        #endregion

        #region Float Version

        /// <summary>
        /// We solve the system of equations, defined as:
        /// M * X = C.
        /// </summary>
        /// <param name="S">The system in matrix form.</param>
        /// <param name="C">constants</param>
        /// <returns></returns>
        public static Vector3f SolveSystem(Matrix3x3f S, Vector3f C)
        {
            return S.Inverse * C;
        }

        /// <summary>
        /// Solves a 2D system.
        /// </summary>
        /// <param name="S">The coefficients.</param>
        /// <param name="C">The constans.</param>
        /// <returns>Vector of solved values.</returns>
        public static Vector2f SolveSystem(Matrix2x2f S, Vector2f C)
        {
            return S.Inverse * C;
        }


        /// <summary>
        /// Solves system using coefficients.
        /// </summary>
        public static Vector2f SolveSystem(float m00, float m01, float m10, float m11, float cx, float cy)
        {
            // Inverse determinant of matrix.
            float d_inv = 1.0f / (m00 * m11 - m01 * m10);
            if (float.IsInfinity(d_inv)) throw new ArgumentException("No solutions of system.");

            // We make calculations as roboust as possible, we create no matrix.
            return new Vector2f(d_inv * (m11 * cx - m10 * cy), d_inv * (-m01 * cx + m00 * cy));
        }

        /// <summary>
        /// Prepares system for solving many times with different data.
        /// </summary>
        /// <param name="S">The system.</param>
        /// <returns>Delegate capable of doing such work.</returns>
        public static Vector3f.Processor GetSolutionProcessorOf(Matrix3x3f S)
        {
            Matrix3x3f inverse = S.Inverse;
            return delegate(Vector3f x)
            {
                return inverse * x;
            };
        }

        /// <summary>
        /// Solves the system.
        /// </summary>
        /// <param name="S">The matrix.</param>
        /// <param name="C">Constants.</param>
        /// <returns></returns>
        public static Vectorf SolveSystem(Matrix.DenseMatrixf S, Vectorf C)
        {
            return S.Inverse * C;
        }

        #endregion

    }
}
