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
    /// Finds the next move given the current position.
    /// </summary>
    public sealed class MinMaxSearch : ISearch
    {
        #region Public SubMembers

        /// <summary>
        /// The node's state.
        /// </summary>
        public enum NodeState
        {
            /// <summary>
            /// Unknown state - if search must be ended, heuristics can be employed.
            /// </summary>
            Unknown,

            /// <summary>
            /// The state represents a draw.
            /// </summary>
            Draw,

            /// <summary>
            /// The white wins.
            /// </summary>
            WhiteWins,

            /// <summary>
            /// The black losses.
            /// </summary>
            BlackWins
        }

        /// <summary>
        /// Checks if the node is final.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public delegate NodeState NodeStateDelegate(uint node);

        #endregion

        #region Private Members
        SearchStatus status = SearchStatus.Running;
        IGraph graph;
        uint start;
        bool whiteOnMove;
        NodeStateDelegate isFinal;

        // Running data.
        List<DFS> searches;
        int inspectionIndex = 0;

        uint currentBest = uint.MaxValue;

        /// <summary>
        /// A delegate helper.
        /// </summary>
        bool IsGoalState(uint x)
        {
            NodeState state = isFinal(x);
            return state != NodeState.Unknown;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxSearch"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="start">The start.</param>
        /// <param name="whiteOnMove">if set to <c>true</c> [white on move].</param>
        /// <param name="isFinal">The is final.</param>
        public MinMaxSearch([NotNull] IGraph graph, uint start, bool whiteOnMove, NodeStateDelegate isFinal)
        {
            this.graph = graph;
            this.start = start;
            this.whiteOnMove = whiteOnMove;
            this.isFinal = isFinal;

            List<uint> nodesToInspect = graph.NextNodes(start);
            searches = new List<DFS>(nodesToInspect.Count);
            for (int i = 0; i < nodesToInspect.Count; i++)
            {
                searches[i] = new DFS(graph, nodesToInspect[i], IsGoalState);
            }
        }


        #endregion

        #region ISearch Members

        public SearchStatus Status
        {
            get { return status; }
        }

        public bool NextIteration()
        {
            if (status == SearchStatus.Complete) return true;

            if (searches[inspectionIndex].NextIteration())
            {
                // We check all results (we flush to free DFS).
                List<SearchResult> results = searches[inspectionIndex].FlushResults();

                // We have to reconstruct common path.
                DecisionTree tree = DecisionTree.FromResults(results);
                
                // We do the search from terminal states.
                DecisionTreeIterator iterator = tree.Iterator;

                if (whiteOnMove)
                {

                }
                else
                {

                }
                
            }

            return false;
        }

        public void Search()
        {
            while (!NextIteration()) ;
        }

        public float DoneRatio
        {
            get { return (float)inspectionIndex / (float)(searches.Count-1); }
        }

        public SearchResult Result
        {
            get 
            { 
                if (status != SearchStatus.Complete) return null;
                if (currentBest == uint.MaxValue) return null;

                return new SearchResult(currentBest);
            }
        }

        public SearchResult[] Results
        {
            get 
            {
                SearchResult result = Result;
                if (result != null) return new SearchResult[] { result };
                return new SearchResult[0];
            }
        }

        #endregion
    }
}
