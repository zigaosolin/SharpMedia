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

namespace SharpMedia.Math.Matrix
{

    /// <summary>
    /// A matrix helper helps identifying some properties and performing some
    /// operations.
    /// </summary>
    public static class MatrixHelper
    {
        #region Neutral Helpers

        public static void AssertSize(Vectord vector, uint size)
        {
            if (vector.DimensionCount != size)
            {
                throw new ArgumentException(string.Format("Vector invalid size, should be {0} but is {1}",
                    size, vector.DimensionCount));
            }
        }

        public static void AssertSize(Vectorf vector, uint size)
        {
            if (vector.DimensionCount != size)
            {
                throw new ArgumentException(string.Format("Vector invalid size, should be {0} but is {1}",
                    size, vector.DimensionCount));
            }
        }

        #endregion

        #region Double Version

        /// <summary>
        /// Checks if matrix is symetric.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool IsSymetric<T, T2>(IMatrixd<T, T2> matrix)
        {

            return false;
        }

        // TODO: other helpers: skew, diagonal, square, singular, unit, null, ortogonal

        // TODO: norms
        


        #endregion
    }
}
