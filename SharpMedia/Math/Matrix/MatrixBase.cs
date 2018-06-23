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


namespace SharpMedia.Math.Matrix
{
    /// <summary>
    /// A matrix interface, exposing all matrix properties.
    /// </summary>
    public interface IMatrixBase<T> : ICloneable<T>, IEquatable<T>
    {
        /// <summary>
        /// Inverse of matrix, or null if not applicable.
        /// </summary>
        T Inverse
        {
            get;
        }

        /// <summary>
        /// Rank od matrix, using near equality to zero as a zero test.
        /// </summary>
        uint Rank
        {
            get;
        }

        /// <summary>
        /// Number of rows of matrix.
        /// </summary>
        uint RowCount
        {
            get;
        }

        /// <summary>
        /// Numbers of colums of matrix.
        /// </summary>
        uint ColumnCount
        {
            get;
        }
    }
}
