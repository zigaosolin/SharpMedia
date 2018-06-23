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
using SharpMedia.Testing;

namespace SharpMedia.Math.Graphs
{
    

    /// <summary>
    /// A hill climbing search is used to improve the current solution. Current implementation
    /// does not support random mutators or any other ways of dealing with local extrema.
    /// </summary>
    /// <remarks>It works with perfectly dynamic graphs.</remarks>
    public sealed class HillClimbingSearch : ISearch
    {
        #region Public SubMembers

        /// <summary>
        /// The evaluate node delegate.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The evaluation; bigger means better.</returns>
        public delegate float NodeEval(uint node);

        #endregion

        #region Private Members
        SearchStatus status = SearchStatus.Running;
        IGraph graph;
        uint currentNode;
        float currentResult;
        NodeEval eval;

        List<uint> backtrace = new List<uint>();
        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="HillClimbingSearch"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="start">The start.</param>
        /// <param name="eval">The eval.</param>
        public HillClimbingSearch([NotNull] IGraph graph, uint start, [NotNull] NodeEval eval)
        {
            this.graph = graph;
            this.currentNode = start;
            this.currentResult = eval(start);
            this.eval = eval;
            this.backtrace.Add(currentNode);
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

            List<uint> nextNodes = graph.NextNodes(currentNode);

            // Select the best next node.
            bool wasFound = false;
            for (int i = 0; i < nextNodes.Count; i++)
            {
                float res = eval(nextNodes[i]);
                if (res > currentResult)
                {
                    currentResult = res;
                    currentNode = nextNodes[i];
                    wasFound = true;
                }
            }

            // Check if no better found.
            if (!wasFound)
            {
                status = SearchStatus.Complete;
                return true;
            }

            backtrace.Add(currentNode);

            return false;
        }

        public void Search()
        {
            while (!NextIteration()) ;
        }

        public float DoneRatio
        {
            get { return 0.0f; }
        }

        public SearchResult Result
        {
            get 
            {
                if (status != SearchStatus.Complete) return null;

                return new SearchResult(backtrace.ToArray());
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

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class HillClimbingTest
    {
        public class Quad : IGraph
        {

            #region IGraph Members

            public GraphProperties Properties
            {
                get { return GraphProperties.Simple; }
            }

            public List<uint> NextNodes(uint from)
            {
                // We return 2 nodes, one left and one right.
                return new List<uint>(new uint[] { from - 1, from + 1 });
            }

            public uint Order(uint node)
            {
                return 2;
            }

            public uint Count
            {
                get { return uint.MaxValue; }
            }

            #endregion
        }

        [CorrectnessTest]
        public void Quadratic()
        {
            HillClimbingSearch search = new HillClimbingSearch(new Quad(), 5,
                delegate(uint _x)
                {
                    float x = _x;
                    return - (x - 5.0f) * (x - 9.0f);
                });

            search.Search();
            Assert.AreEqual(3, search.Result.Count);
            Assert.AreEqual(7, search.Result[2]);
        }
    }
#endif
}
