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
    /// A iterative deepening search - combines BFS and DFS. The search is optimal for non-weighted graph.
    /// It has DFS storage requirements. The cost for this is that it revisits root nodes more than once.
    /// </summary>
    public sealed class InterativeDeepeningSearch : ISearch
    {
        #region Private Data

        /// <summary>
        /// A search data class.
        /// </summary>
        class SearchData
        {
            public uint Node;
            public int Index;
            public List<uint> SubNodes;
            public uint Depth;


            /// <summary>
            /// Finds the next node.
            /// </summary>
            /// <returns></returns>
            public uint NextNode()
            {
                if (Index >= SubNodes.Count) return uint.MaxValue;
                uint next = SubNodes[Index];
                Index++;
                return next;
            }

            public SearchData(uint node, int index, uint depth, List<uint> subNodes)
            {
                Node = node;
                Index = index;
                Depth = depth;
                SubNodes = subNodes;
            }
        }

        // Input parameters.
        IGraph graph;
        uint parent;
        Predicate<uint> isGoal;
        uint currentMaxDepth = 2;
        bool limitReached = false;

        // Status parameters.
        SearchStatus status = SearchStatus.Running;
        Stack<SearchData> stack = new Stack<SearchData>();

        // Results (all equal length).
        List<SearchResult> results = new List<SearchResult>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InterativeDeepeningSearch"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="start">The start.</param>
        /// <param name="goal">The goal.</param>
        public InterativeDeepeningSearch([NotNull] IGraph graph, uint start, uint goal)
            : this(graph, start, delegate(uint g) { return goal == g; })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterativeDeepeningSearch"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="start">The start.</param>
        /// <param name="goal">The goal.</param>
        public InterativeDeepeningSearch([NotNull] IGraph graph, uint start, [NotNull] Predicate<uint> isGoal)
        {
            this.graph = graph;
            this.parent = start;
            this.isGoal = isGoal;
            this.stack.Push(new SearchData(start, 0, 0, graph.NextNodes(start)));
        }

        #endregion

        #region ISearch Members

        public SearchStatus Status
        {
            get { return status; }
        }

        public bool NextIteration()
        {

            // Search ended.
            if (stack.Count == 0)
            {

                if (limitReached && results.Count == 0)
                {
                    limitReached = false;
                    currentMaxDepth++;
                    this.stack.Push(new SearchData(parent, 0, 0, graph.NextNodes(parent)));
                }
                else
                {
                    status = SearchStatus.Complete;
                    return true;
                }
            }

            SearchData data = stack.Pop(); 

            uint next = data.NextNode();

            // We check if limit was reached.
            if (data.Depth + 1 >= currentMaxDepth)
            {
                // We discard such nodes.
                limitReached = true;
                return false;
            }

            if (next == uint.MaxValue)
            {
                // No more nodes to process, go up.
                if (stack.Count == 0)
                {
                    return false;
                }
                data = stack.Pop();
            }
            else
            {
                // We process next node.
                if (isGoal(next))
                {
                    // We create the result.
                    uint[] resultData = new uint[stack.Count + 2];
                    resultData[0] = parent;

                    // Copy the path.
                    SearchData[] d = stack.ToArray();
                    for (int i = 0; i < stack.Count; i++)
                    {
                        int wIndex = stack.Count - i - 1;
                        resultData[i + 1] = d[wIndex].SubNodes[d[wIndex].Index - 1];
                    }

                    // The last (goal) node.
                    resultData[stack.Count + 1] = next;

                    // We have found the goal, do not proceed.
                    SearchResult result = new SearchResult(resultData);
                    results.Add(result);
                    status = SearchStatus.RunningWithSolution;
                    return false;
                }
                else
                {
                    // We add our node.
                    stack.Push(data);
                    data = new SearchData(next, 0, data.Depth+1, graph.NextNodes(next));
                }
                
            }



            // We need to pus current node.
            stack.Push(data);
            return false;
        }

        public void Search()
        {
            while (!NextIteration()) ;
        }

        public SearchResult Result
        {
            get
            {
                if (results.Count > 0) return results[0];
                return null;
            }
        }

        public SearchResult[] Results
        {
            get { return results.ToArray(); }
        }

        public float DoneRatio
        {
            get { return 0.0f; }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    public class DepthFirstSearchTest
    {
        /*
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

            InterativeDeepeningSearch search = new InterativeDeepeningSearch(g, 0, 3);
            search.Search();

            // We find solutions.
            SearchResult[] results = search.Results;
            Assert.AreEqual(1, results.Length);

            Assert.IsTrue(results[0].Equals(new SearchResult(0, 1, 3)));

        }
         */
    }
#endif
}
