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
using SharpMedia.AspectOriented;

namespace SharpMedia.Math.Graphs
{
    /// <summary>
    /// One of the possible (optimal) result search paths. It is immutable.
    /// </summary>
    public sealed class SearchResult : IEquatable<SearchResult>, IEnumerable<uint> 
    {
        #region Private Members
        uint[] path;
        float cost;
        #endregion

        #region Properties

        /// <summary>
        /// A node index accessor.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The id at index.</returns>
        public uint this[uint index]
        {
            get
            {
                return path[index];
            }
        }

        /// <summary>
        /// The length of result.
        /// </summary>
        public uint Count
        {
            get
            {
                return (uint)path.Length;
            }
        }

        /// <summary>
        /// The cost of path; if not weighted, this is the same as count.
        /// </summary>
        public float Cost
        {
            get
            {
                return cost;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public SearchResult([NotNull] params uint[] path)
        {
            if (path == null) throw new ArgumentException("Path must be non-null.");
            this.path = path;
            this.cost = path.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="cost">The cost.</param>
        public SearchResult([NotNull] uint[] path, float cost)
        {
            if (path == null) throw new ArgumentException("Path must be non-null.");
            this.path = path;
            this.cost = cost;
        }

        #endregion

        #region IEquatable<SearchResult> Members

        public bool Equals(SearchResult other)
        {
            if (other == null) return false;
            if (this.Count != other.Count) return false;
            for (uint i = 0; i < this.Count; i++)
            {
                if (!this[i].Equals(other[i])) return false;
            }
            return true;
        }

        #endregion

        #region IEnumerable<uint> Members

        public IEnumerator<uint> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this.path[i];
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this.path[i];
            }
        }

        #endregion
    }
}
