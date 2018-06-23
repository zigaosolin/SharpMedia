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

namespace SharpMedia.Math.Graphs
{
    /// <summary>
    /// A minimum cost flow search. Caclulates the biggest amount of "quantity"
    /// the network can transport.
    /// </summary>
    /// <remarks>You can implement multiple sources/sinks by binding all sources
    /// to supersink and all sinks to supersink.</remarks>
    public class MinimumCostFlowSearch : ISearch
    {
        #region ISearch Members

        public SearchStatus Status
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool NextIteration()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Search()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float DoneRatio
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public SearchResult Result
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public SearchResult[] Results
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
