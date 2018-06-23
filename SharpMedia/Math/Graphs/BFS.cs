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

namespace SharpMedia.Math.Graphs
{

    /// <summary>
    /// A breadth-first search finds 
    /// </summary>
    public sealed class BFS : ISearch
    {
        #region Private Structures

        /// <summary>
        /// A BFSNode.
        /// </summary>
        class BFSNode
        {
            /// <summary>
            /// The parent node or null.
            /// </summary>
            public BFSNode Parent;

            /// <summary>
            /// The node id.
            /// </summary>
            public uint Node;

            /// <summary>
            /// Initializes a new instance of the <see cref="BFSNode"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="node">The node.</param>
            public BFSNode(BFSNode parent, uint node)
            {
                this.Parent = parent;
                this.Node = node;
            }
        }

        #endregion

        #region Private Members
        int                 maxBranch = int.MaxValue;
        int                 level = 0;
        SearchStatus        status = SearchStatus.Running;
        List<BFSNode>       workingBranch = new List<BFSNode>();
        List<BFSNode>       nextBranch = new List<BFSNode>();
        IGraph              graph;
        int                 workingIndex = 0;
        Predicate<uint>     isGoal;

        List<BFSNode> results = new List<BFSNode>();
        
        #endregion

        #region Public Members


        /// <summary>
        /// Initializes a new instance of the <see cref="BFS"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="goal">The goal.</param>
        public BFS([NotNull] IGraph graph, uint start, uint goal)
            :this(graph, start, delegate(uint x) { return x == goal; }, int.MaxValue)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BFS"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="goal">The goal.</param>
        public BFS([NotNull] IGraph graph, uint start, uint goal, int maxBranch)
            : this(graph, start, delegate(uint x) { return x == goal; }, maxBranch)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BFS"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="isGoal">The is goal.</param>
        public BFS([NotNull] IGraph graph, uint start, [NotNull] Predicate<uint> isGoal)
            : this(graph, start, isGoal, int.MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BFS"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="isGoal">The is goal.</param>
        public BFS([NotNull] IGraph graph, uint start, [NotNull] Predicate<uint> isGoal, int maxBranch)
        {
            this.graph = graph;
            this.isGoal = isGoal;
            this.maxBranch = maxBranch;
            this.workingBranch.Add(new BFSNode(null, start));
            if (isGoal(start))
            {
                // We have result immediatelly.
                this.results.Add(workingBranch[0]);
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
            // Solution was already found.
            if (status == SearchStatus.Complete || status == SearchStatus.NotFound) return true;

            List<uint> nextNodes = graph.NextNodes(workingBranch[workingIndex].Node);
            for (int i = 0; i < nextNodes.Count; i++)
            {
                // We check if it is goal.
                BFSNode nextNode = new BFSNode(workingBranch[workingIndex], nextNodes[i]);
                if (isGoal(nextNode.Node))
                {
                    results.Add(nextNode);
                    status = SearchStatus.RunningWithSolution;
                }

                nextBranch.Add(nextNode);
            }


            // We check if we need to go to next level.
            if (++workingIndex >= workingBranch.Count)
            {
                workingIndex = 0;

                // Check if results were found.
                if (results.Count > 0)
                {
                    status = SearchStatus.Complete;
                    return true;
                }

                if (++level >= maxBranch || nextBranch.Count == 0)
                {
                    status = SearchStatus.NotFound;
                    return false;
                }

                // If results were not found, we go to new level.
                workingBranch = nextBranch;
                nextBranch = new List<BFSNode>(workingBranch.Count);
            }

            return false;
        }

        public void Search()
        {
            while (!NextIteration()) ;
        }

        public float DoneRatio
        {
            get 
            { 
                if (maxBranch != int.MaxValue) return (float)level / (float)maxBranch;
                return 0.0f;
            }
        }

        public SearchResult Result
        {
            get 
            {
                if (results.Count == 0) return null;
                List<uint> path = new List<uint>();

                // We backtrace.
                BFSNode c = results[0];
                while (c != null)
                {
                    path.Add(c.Node);
                    c = c.Parent;
                }
                path.Reverse();

                // And return result.
                return new SearchResult(path.ToArray());
            }
        }

        public SearchResult[] Results
        {
            get 
            {
                SearchResult[] rs = new SearchResult[results.Count];
                for (int i = 0; i < results.Count; i++)
                {
                    List<uint> path = new List<uint>();

                    // We backtrace.
                    BFSNode c = results[i];
                    while (c != null)
                    {
                        path.Add(c.Node);
                        c = c.Parent;
                    }
                    path.Reverse();

                    // And return result.
                    rs[i] = new SearchResult(path.ToArray());
                }
                return rs;
            }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class BFSTest
    {
        [CorrectnessTest]
        public void FindSimple()
        {
            DataGraph<string> g = new DataGraph<string>();
            g.AddNode("A");
            g.AddNode("B");
            g.AddNode("C");
            g.AddNode("D");
            g.AddNode("E");
            g.AddNode("F");
            g.Link(0, 1);
            g.Link(0, 4);
            g.Link(1, 2);
            g.Link(2, 3);
            g.Link(1, 3);
            g.Link(4, 5);

            BFS search = new BFS(g, 0, 3);
            search.Search();

            // We find solutions.
            SearchResult[] results = search.Results;
            Assert.AreEqual(1, results.Length);

            Assert.IsTrue(results[0].Equals(new SearchResult(0, 1, 3)));

        }
    }
#endif
}
