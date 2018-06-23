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

namespace SharpMedia.Math.Statistics
{
    /// <summary>
    /// Histogram implementation holds T-uint pairs.
    /// </summary>
    /// <typeparam name="T">The type of data.</typeparam>
    public class Histrogram<T> where T : IComparable<T>, IEnumerable<T>
    {
        #region Private Members
        SortedDictionary<T, uint> histogram = new SortedDictionary<T,uint>();
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Histrogram&lt;T&gt;"/> class.
        /// </summary>
        public Histrogram()
        {
        }

        #endregion

        #region Properties

        public uint this[T data]
        {
            get
            {
                // Extract if in it.
                uint d;
                if (histogram.TryGetValue(data, out d)) return d;
                return 0;
            }
            set
            {
                histogram[data] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Add(T data)
        {
            this[data] = this[data] + 1;
        }

        /// <summary>
        /// Removes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Remove(T data)
        {
            uint c = this[data];
            if (c > 0) this[data] = c - 1;
        }



        #endregion
    }
}
