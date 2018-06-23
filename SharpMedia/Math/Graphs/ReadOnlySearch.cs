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
    /// A read only search proxy.
    /// </summary>
    public class ReadOnlySearch : ISearch
    {
        #region Private Methods
        ISearch internalSearch;
        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySearch"/> class.
        /// </summary>
        /// <param name="internalSearch">The internal search.</param>
        public ReadOnlySearch([NotNull] ISearch internalSearch)
        {
            this.internalSearch = internalSearch;
        }

        #endregion

        #region ISearch Members

        public SearchStatus Status
        {
            get { return internalSearch.Status; }
        }

        public bool NextIteration()
        {
            throw new InvalidOperationException("Cannot performn next iteration on read-only search.");
        }

        public void Search()
        {
            throw new InvalidOperationException("Cannot performn search on read-only search.");
        }

        public float DoneRatio
        {
            get { return internalSearch.DoneRatio; }
        }

        public SearchResult Result
        {
            get { return internalSearch.Result; }
        }

        public SearchResult[] Results
        {
            get { return internalSearch.Results; }
        }

        #endregion
    }
}
