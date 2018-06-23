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
using SharpMedia.Math.Functions;
using SharpMedia.Testing;

namespace SharpMedia.Math.Statistics
{
    /// <summary>
    /// Combinatorics class.
    /// </summary>
    public static class Combinatorics
    {
        #region T Versions

        internal static void Permutate<T>(T[] data, int idx, List<T[]> result)
        {
            if (idx == data.Length)
            {
                // We have permutation.
                result.Add(data.Clone() as T[]);
                return;
            }

            for (int i = idx; i < data.Length; i++)
            {
                // We put i-th data to idx place.
                T tmp = data[idx];
                data[idx] = data[i];
                data[i] = tmp;

                // We permutate
                Permutate(data, idx + 1, result);

                // we correct back.
                tmp = data[idx];
                data[idx] = data[i];
                data[i] = tmp;
            }
        }

        /// <summary>
        /// We perform all permutations of data.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns></returns>
        public static List<T[]> Permutate<T>(T[] data)
        {
            // We find all permutations.
            List<T[]> res = new List<T[]>((int)Factorial.Eval((uint)data.Length));

            Permutate(data.Clone() as T[], 0, res);

            return res;
        }


        #endregion

        #region Base Operators

        /// <summary>
        /// Variationses the specified n.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        public static uint Variations(uint n, uint k)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Computes number of variations with repetion.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        public static uint VariationsWithRepetions(uint n, uint k)
        {
            return MathHelper.Pow(n, k);
        }

        /// <summary>
        /// Computes number of combinations:
        /// ( n )
        /// ( k )
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        public static uint Combinations(uint n, uint k)
        {
            return (uint)(0.5f * MathHelper.Exp(Gamma.EvalLn(n) - Gamma.EvalLn(k) - Gamma.EvalLn(n - k)));
        }

        /// <summary>
        /// Computes number of combinations:
        /// ( n )
        /// ( k )
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        public static float Combinations(float n, float k)
        {
            return (0.5f * MathHelper.Exp(Gamma.EvalLn(n) - Gamma.EvalLn(k) - Gamma.EvalLn(n - k)));
        }

        /// <summary>
        /// Computes number of combinations:
        /// ( n )
        /// ( k )
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        public static double Combinations(double n, double k)
        {
            return (0.5 * MathHelper.Exp(Gamma.EvalLn(n) - Gamma.EvalLn(k) - Gamma.EvalLn(n - k)));
        }

        /// <summary>
        /// Computes number of permutations of n elements.
        /// </summary>
        /// <param name="n">The element count.</param>
        /// <returns></returns>
        public static uint Permutations(uint n)
        {
            return (uint)Functions.Factorial.Evald(n);
        }

        #endregion
    }



#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class CombTest
    {
        [CorrectnessTest]
        public void Permutations()
        {
            uint[] data = new uint[] { 1, 2, 3, 4 };

            List<uint[]> perm = Combinatorics.Permutate(data);

            Assert.AreEqual(4 * 3 * 2 * 1, perm.Count);
        }
    }

#endif
}
