// This file was generated by TemplateEngine from template source 'GaussJordan'
// using template 'GaussJordan. Do not modify this file directly, modify it from template source.

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

namespace SharpMedia.Math.Matrix.Algorithms
{
    /// <summary>
    /// Can invert square matrix.
    /// </summary>
    public static class GaussJordan
    {
        
		//#foreach instanced to 'Double'


        #region Double Version

        #region Private Members

        /// <summary>
        /// Changes elements of two rows.
        /// </summary>
        static void InterchangeRows(double[,] m, int row1, int row2)
        {
            int size = m.GetLength(1), i;
            double tmp;

            // We perform row2 <-> row1 copy.
            for (i = 0; i < size; i++)
            {
                tmp = m[row1, i];
                m[row1, i] = m[row2, i];
                m[row2, i] = tmp;
            }
        }

        /// <summary>
        /// Divides row by value, starting at some offset of matrix since it must be
        /// null there.
        /// </summary>
        static void DivideByValue(double[,] m, int row, int divideOffset, double v)
        {
            int size = m.GetLength(1), i;
            double v_inv = 1.0 / v;

            for (i = divideOffset; i < size; i++)
            {
                m[row, i] *= v_inv;
            }
        }

        /// <summary>
        /// Adds scaled row, to make element 0.0.
        /// </summary>
        static void AddScaledRow(double[,] m, int rowTo, int rowFrom, int offset, double scale)
        {
            int size = m.GetLength(1), i;

            for (i = offset; i < size; i++)
            {
                m[rowTo, i] += m[rowFrom, i] * scale;
            }

        }

        /// <summary>
        /// Performs partial pivoting.
        /// </summary>
        /// <param name="m">The matrix.</param>
        /// <param name="column">Column.</param>
        static void PartialPivotMatrix(double[,] m, int column)
        {
            double absMax = 0.0;
            int index = -1, size = m.GetLength(0);

            // We calculate the biggest element.
            for (int i = column; i < size; i++)
            {
                double v = MathHelper.Abs(m[i, column]);
                if (v >= absMax)
                {
                    index = i;
                    absMax = v;
                }
            }

            // Perform pivoting.
            if (index != column)
            {
                InterchangeRows(m, column, index);
            }

        }

        #endregion

        #region Public Members


        /// <summary>
        /// Inverts the matrix using Gauss-Jordan method. We use row exchange everytime to ensure
        /// numeric stability. 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double[,] Invert(double[,] m)
        {
            if (m == null) throw new ArgumentNullException("Array must be non-null.");
            if (m.GetLength(0) != m.GetLength(1)) throw new ArgumentException("Cannot invert non square matrix.");

            int size = m.GetLength(0);
            int size2 = 2 * size;

            // 1) We first construct our matrix as ( M | I )
            double[,] matrix = new double[size, size * 2];
            for (int r = 0; r < size; r++)
            {
                for (int x = 0; x < size; x++)
                {
                    matrix[x, r] = m[x, r];
                }
            }

            for (int r = size; r < size2; r++)
            {
                int x;
                for (x = size; x < r; x++)
                {
                    matrix[r - 3, x] = 0.0;
                }

                matrix[r - 3, r] = 1.0;

                for (x = r + 1; x < size2; x++)
                {
                    matrix[r - 3, x] = 0.0;
                }

            }

            // 2) We go column by column, converting matrix to solution. We
            // use pivoting to avoid big numeric errors.
            for (int column = 0; column < size; column++)
            {
                // We exchange rows, with the one which has biggest element in this column.
                GaussJordan.PartialPivotMatrix(matrix, column);

                double pivot = matrix[column, column];
                if (MathHelper.NearEqual(pivot, 0))
                {
                    throw new DivideByZeroException("Matrix cannot be inverted.");
                }

                // We divide row by value.
                GaussJordan.DivideByValue(matrix, column, column, pivot);

                // We zero out all elements.
                int i;
                for (i = 0; i < column; i++)
                {
                    AddScaledRow(matrix, i, column, column, -matrix[i, column]);
                }

                for (i = column + 1; i < size; i++)
                {
                    AddScaledRow(matrix, i, column, column, -matrix[i, column]);
                }
            }


            // 3) We extract solution.
            double[,] solution = new double[size, size];
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    solution[j, k] = matrix[j, k + size];
                }
            }

            return solution;
        }

        /// <summary>
        /// Calculates determinant of matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double Determinant(double[,] matrix)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains the rank of system.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="NearZeroEps"></param>
        /// <returns></returns>
        public static uint GetRank(double[,] matrix, double NearZeroEps)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains the rank of matrix with default epsilon.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static uint GetRank(double[,] matrix)
        {
            return GetRank(matrix, 0.0);
        }

        // TODO: system solving facilities (that is with Gauss-Jordan). Should solve
        // a non-square system by providing solution with parameters or sign that no solution
        // to the system is possible.




        #endregion


        #endregion

        //#endfor instanced to 'Double'

		//#foreach instanced to 'Float'


        #region Float Version

        #region Private Members

        /// <summary>
        /// Changes elements of two rows.
        /// </summary>
        static void InterchangeRows(float[,] m, int row1, int row2)
        {
            int size = m.GetLength(1), i;
            float tmp;

            // We perform row2 <-> row1 copy.
            for (i = 0; i < size; i++)
            {
                tmp = m[row1, i];
                m[row1, i] = m[row2, i];
                m[row2, i] = tmp;
            }
        }

        /// <summary>
        /// Divides row by value, starting at some offset of matrix since it must be
        /// null there.
        /// </summary>
        static void DivideByValue(float[,] m, int row, int divideOffset, float v)
        {
            int size = m.GetLength(1), i;
            float v_inv = 1.0f / v;

            for (i = divideOffset; i < size; i++)
            {
                m[row, i] *= v_inv;
            }
        }

        /// <summary>
        /// Adds scaled row, to make element 0.0f.
        /// </summary>
        static void AddScaledRow(float[,] m, int rowTo, int rowFrom, int offset, float scale)
        {
            int size = m.GetLength(1), i;

            for (i = offset; i < size; i++)
            {
                m[rowTo, i] += m[rowFrom, i] * scale;
            }

        }

        /// <summary>
        /// Performs partial pivoting.
        /// </summary>
        /// <param name="m">The matrix.</param>
        /// <param name="column">Column.</param>
        static void PartialPivotMatrix(float[,] m, int column)
        {
            float absMax = 0.0f;
            int index = -1, size = m.GetLength(0);

            // We calculate the biggest element.
            for (int i = column; i < size; i++)
            {
                float v = MathHelper.Abs(m[i, column]);
                if (v >= absMax)
                {
                    index = i;
                    absMax = v;
                }
            }

            // Perform pivoting.
            if (index != column)
            {
                InterchangeRows(m, column, index);
            }

        }

        #endregion

        #region Public Members


        /// <summary>
        /// Inverts the matrix using Gauss-Jordan method. We use row exchange everytime to ensure
        /// numeric stability. 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static float[,] Invert(float[,] m)
        {
            if (m == null) throw new ArgumentNullException("Array must be non-null.");
            if (m.GetLength(0) != m.GetLength(1)) throw new ArgumentException("Cannot invert non square matrix.");

            int size = m.GetLength(0);
            int size2 = 2 * size;

            // 1) We first construct our matrix as ( M | I )
            float[,] matrix = new float[size, size * 2];
            for (int r = 0; r < size; r++)
            {
                for (int x = 0; x < size; x++)
                {
                    matrix[x, r] = m[x, r];
                }
            }

            for (int r = size; r < size2; r++)
            {
                int x;
                for (x = size; x < r; x++)
                {
                    matrix[r - 3, x] = 0.0f;
                }

                matrix[r - 3, r] = 1.0f;

                for (x = r + 1; x < size2; x++)
                {
                    matrix[r - 3, x] = 0.0f;
                }

            }

            // 2) We go column by column, converting matrix to solution. We
            // use pivoting to avoid big numeric errors.
            for (int column = 0; column < size; column++)
            {
                // We exchange rows, with the one which has biggest element in this column.
                GaussJordan.PartialPivotMatrix(matrix, column);

                float pivot = matrix[column, column];
                if (MathHelper.NearEqual(pivot, 0))
                {
                    throw new DivideByZeroException("Matrix cannot be inverted.");
                }

                // We divide row by value.
                GaussJordan.DivideByValue(matrix, column, column, pivot);

                // We zero out all elements.
                int i;
                for (i = 0; i < column; i++)
                {
                    AddScaledRow(matrix, i, column, column, -matrix[i, column]);
                }

                for (i = column + 1; i < size; i++)
                {
                    AddScaledRow(matrix, i, column, column, -matrix[i, column]);
                }
            }


            // 3) We extract solution.
            float[,] solution = new float[size, size];
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    solution[j, k] = matrix[j, k + size];
                }
            }

            return solution;
        }

        /// <summary>
        /// Calculates determinant of matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static float Determinant(float[,] matrix)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains the rank of system.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="NearZeroEps"></param>
        /// <returns></returns>
        public static uint GetRank(float[,] matrix, float NearZeroEps)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains the rank of matrix with default epsilon.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static uint GetRank(float[,] matrix)
        {
            return GetRank(matrix, 0.0f);
        }

        // TODO: system solving facilities (that is with Gauss-Jordan). Should solve
        // a non-square system by providing solution with parameters or sign that no solution
        // to the system is possible.




        #endregion


        #endregion

        //#endfor instanced to 'Float'


    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class GaussJordanTest
    {
        double[,] I;
        double[,] M, M_inv;

        public GaussJordanTest()
        {
            I = new double[3, 3]{ { 1, 0, 0 },
                                  { 0, 1, 0 },
                                  { 0, 0, 1 } };

            M = new double[3, 3]{{1, 0, 0},
                                {2, 2, 3},
                                {0, 5, 3}};

            M_inv = new double[3, 3]{{1, 0, 0},
                        {2.0 / 3.0, -1.0 / 3.0, 1.0 / 3.0},
                        {-10.0 / 9.0, 5.0 / 9.0, -2.0 / 9.0}};
        }

        [CorrectnessTest]
        public void Invert1()
        {
            double[,] m = I;

            double[,] inv = GaussJordan.Invert(m);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.IsTrue(MathHelper.NearEqual(inv[i,j], m[i,j]));
                }
            }
        }

        [CorrectnessTest]
        public void Invert2()
        {

            double[,] m_inv = GaussJordan.Invert(M);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.IsTrue(MathHelper.NearEqual(M_inv[i, j], m_inv[i, j]));
                }
            }
        }
    }
#endif
}
