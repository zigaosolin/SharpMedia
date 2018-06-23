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
    /// A depth first search. It is implemented using internal stack.
    /// </summary>
    public sealed class DFS : ISearch
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

            public SearchData(uint node, int index, List<uint> subNodes)
            {
                Node = node;
                Index = index;
                SubNodes = subNodes;
            }
        }

        // Input parameters.
        IGraph graph;
        uint parent;
        Predicate<uint> isGoal;

        // Status parameters.
        SearchStatus status = SearchStatus.Running;
        Stack<SearchData> stack = new Stack<SearchData>();

        // Results (all equal length).
        List<SearchResult> results = new List<SearchResult>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearch"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="parent">The parent node.</param>
        /// <param name="goal">The goal node.</param>
        public DFS([NotNull] IGraph graph, uint parent, uint goal)
        {
            this.graph = graph;
            this.parent = parent;
            this.isGoal = delegate(uint x) { return x == goal; };

            stack.Push(new SearchData(parent, 0, graph.NextNodes(parent)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearch"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="parent">The parent node.</param>
        /// <param name="goal">The goal node.</param>
        /// <param name="invalid">The invalid node index, usually either 0 or uint.MaxValue.</param>
        public DFS([NotNull] IGraph graph, uint parent, [NotNull] Predicate<uint> goal)
        {
            this.graph = graph;
            this.parent = parent;
            this.isGoal = goal;

            stack.Push(new SearchData(parent, 0, graph.NextNodes(parent)));
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Flushes results.
        /// </summary>
        /// <returns></returns>
        public List<SearchResult> FlushResults()
        {
            List<SearchResult> temp = results;
            results = new List<SearchResult>();
            return temp;
        }

        #endregion

        #region ISearch Members

        public SearchStatus Status
        {
            get { return status; }
        }

        public bool NextIteration()
        {
            // Search ended (early deformed graphs).
            if (stack.Count == 0)
            {
                status = SearchStatus.Complete;
                return true;
            }

            SearchData data = stack.Pop();

            uint next = data.NextNode();
            if (next == uint.MaxValue)
            {
                // No more nodes to process, go up.
                if (stack.Count == 0)
                {
                    status = SearchStatus.Complete;
                    return true;
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
                    data = new SearchData(next, 0, graph.NextNodes(next));
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
    internal class InterativeDeepeningSearchTest
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

            DFS search = new DFS(g, 0, 3);
            search.Search();

            // We find solutions.
            SearchResult[] results = search.Results;
            Assert.AreEqual(2, results.Length);
            string s1 = g.ToString(results[0]), s2 = g.ToString(results[1]);

            Assert.IsTrue(results[0].Equals(new SearchResult(0, 1, 2, 3)));
            Assert.IsTrue(results[1].Equals(new SearchResult(0, 1, 3)));
            
        }

        class Queen : IGraph
        {
            uint dim;
            public Queen(uint dim)
            {
                this.dim = dim;
            }

            public Predicate<uint> Predicate
            {
                get
                {
                    return delegate(uint x) { return x >= dim * dim-1; };
                }
            }

            #region IGraph Members

            public uint NodeCount
            {
                get { return uint.MaxValue; }
            }

            public List<uint> NextNodes(uint from)
            {
                uint x = from == uint.MaxValue - 1 ? 0 : from / dim + 1;
                if (x >= dim) return new List<uint>();
                List<uint> r = new List<uint>((int)dim);
                for (uint i = 0; i < dim; i++)
                {
                    r.Add(x * dim + i);
                }
                return r;
            }

            public GraphProperties Properties
            {
                get { return GraphProperties.Simple | GraphProperties.DirectedAcylic | GraphProperties.Dnyamic; }
            }
      
            public uint Order(uint node)
            {
                throw new Exception("The method or operation is not implemented.");
            }


            public uint Count
            {
                get { return uint.MaxValue; }
            }

            #endregion
        }
         */

        /*
        [CorrectnessTest]
        public void QueensProblem()
        {
            int d = 8;
            Queen q = new Queen(d);
            DFS<int> s = new DFS<int>(q, -1, q.Predicate, int.MinValue);
            s.Search();

            SearchResult<int>[] r = s.Results;
            Assert.AreEqual(4, r.Length);
        }*/

        /*
        [CorrectnessTest]
        public void QueensProblemOneSolution()
        {
            uint d = 8;
            Queen q = new Queen(d);
            DFS s = new DFS(q, uint.MaxValue-1, q.Predicate);
            while (!s.NextIteration())
            {
                if (s.Status == SearchStatus.RunningWithSolution) break;
            }

            SearchResult[] r = s.Results;
            Assert.AreEqual(1, r.Length);
        }
         */
    }
#endif
}
