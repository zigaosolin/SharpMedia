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
    /// A Bellman-Ford best path searching algorithms.
    /// </summary>
    /// <remarks>Prefer Dijkstra if weights are positive (because it is faster). Bellman-Ford
    /// also identifies negative cycles (where no solution exists).</remarks>
    public sealed class BellmanFordSearch : ISearch
    {
        #region Private Structs

        /// <summary>
        /// A BF Edge.
        /// </summary>
        public class BFEdge
        {
            /// <summary>
            /// The cost of edge.
            /// </summary>
            public float Cost;

            /// <summary>
            /// From node.
            /// </summary>
            public uint NodeFrom;

            /// <summary>
            /// The to node.
            /// </summary>
            public uint NodeTo;

            /// <summary>
            /// Initializes a new instance of the <see cref="BFEdge"/> struct.
            /// </summary>
            /// <param name="from">From.</param>
            /// <param name="to">To.</param>
            /// <param name="cost">The cost.</param>
            public BFEdge(uint from, uint to, float cost)
            {
                NodeFrom = from;
                NodeTo = to;
                Cost = cost;
            }
        }

        #endregion

        #region Private Members
        SearchStatus status = SearchStatus.Running;
        float[] distance;
        uint[] bestPrevious;
        IWeightedGraph graph;
        uint initialNode;
        List<BFEdge> allEdges = new List<BFEdge>();

        uint goal;
        int currentIndex = 0;
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="BellmanFord"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="startNode">The start node.</param>
        public BellmanFordSearch([NotNull] IWeightedGraph graph, uint startNode, uint goal)
        {
            if ((graph.Properties & GraphProperties.Dnyamic) != 0)
            {
                throw InvalidGraphPropertiesException.MustNotHave(GraphProperties.Dnyamic);
            }
            if (startNode >= graph.Count)
            {
                throw new ArgumentException("The starting node not in range.");
            }

            int count = (int)graph.Count;
            this.distance = new float[count];
            this.bestPrevious = new uint[count];
            this.graph = graph;
            this.initialNode = startNode;
            this.goal = goal;

            for (int i = 0; i < count; i++)
            {
                this.distance[i] = float.PositiveInfinity;
                this.bestPrevious[i] = uint.MaxValue;

                // And extract edges.
                List<Edge> edges = graph.NextWeightedNodes((uint)i);
                for (int j = 0; j < edges.Count; j++)
                {
                    allEdges.Add(new BFEdge((uint)i, edges[j].Node, edges[j].Weight));
                }
            }

            distance[startNode] = 0.0f;


        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BellmanFord"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="startNode">The start node.</param>
        public BellmanFordSearch([NotNull] IWeightedGraph graph, uint startNode)
            : this(graph, startNode, startNode)
        {
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <remarks>You can use indexer if you want.</remarks>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public SearchResult GetResult(uint to)
        {
            return this[to];
        }

        public SearchResult this[uint to]
        {
            get
            {
                if (to >= graph.Count) return null;
                if (status != SearchStatus.Complete) return null;


                // We backtrace.
                List<uint> path = new List<uint>();
                uint backtrace = to;
                while (bestPrevious[backtrace] != uint.MaxValue)
                {
                    path.Add(backtrace);
                    backtrace = bestPrevious[backtrace];
                }

                // We have found part of path but 
                if (backtrace != initialNode) return null;
                path.Add(initialNode);

                // We must reverse it.
                path.Reverse();
                return new SearchResult(path.ToArray(), distance[to]);

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
            if (status != SearchStatus.Running) return true;

            // We do next full iteration.
            for (int i = 0; i < allEdges.Count; i++)
            {
                BFEdge edge = allEdges[i];
                if (distance[edge.NodeFrom] + edge.Cost < distance[edge.NodeTo])
                {
                    distance[edge.NodeTo] = distance[edge.NodeFrom] + edge.Cost;
                    bestPrevious[edge.NodeTo] = edge.NodeFrom;
                }
            }

            // We do next iteration.
            if (++currentIndex >= (int)graph.Count)
            {
                // Make sure we do not have negative cycles.
                for (int j = 0; j < allEdges.Count; j++)
                {
                    BFEdge edge = allEdges[j];
                    if (distance[edge.NodeTo] > distance[edge.NodeFrom] + edge.Cost)
                    {
                        status = SearchStatus.NotFound;
                        break;
                    }
                }

                // We have a negative loop.
                if (status == SearchStatus.NotFound)
                {
                    return true;
                }

                // We check if solution was found.
                status = SearchStatus.Complete;
                if (Result == null)
                {
                    status = SearchStatus.NotFound;
                }

                return true;
            }
            return false;
        }

        public void Search()
        {
            while (!NextIteration()) ;
        }

        public SearchResult Result
        {
            get { return this[goal]; }
        }

        public SearchResult[] Results
        {
            get
            {
                SearchResult res = this[goal];
                if (res != null)
                {
                    return new SearchResult[] { res };
                }
                return new SearchResult[0];
            }
        }

        public float DoneRatio
        {
            get { return (float)currentIndex / (float)(graph.Count * 2); }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class BellmanFordTest
    {
        [CorrectnessTest]
        public void SimpleTest()
        {
            DataWeightedGraph<string> graph = new DataWeightedGraph<string>();
            uint a1 = graph.AddNode("1");
            uint a2 = graph.AddNode("2");
            uint a3 = graph.AddNode("3");
            uint a4 = graph.AddNode("4");
            uint a5 = graph.AddNode("5");
            graph.Link(a1, a3, 1.0f);
            graph.Link(a1, a2, 3.0f);
            graph.Link(a3, a2, 1.0f);
            graph.Link(a2, a4, 2.0f);
            graph.Link(a4, a5, 1.0f);
            graph.Link(a1, a5, 7.0f);

            BellmanFordSearch bf = new BellmanFordSearch(graph, a1, a5);
            bf.Search();

            SearchResult result = bf.Result;
            Assert.AreEqual(5.0, result.Cost);
            Assert.AreEqual(5, result.Count);
        }

        [CorrectnessTest]
        public void SimpleTestChangeGoal()
        {
            DataWeightedGraph<string> graph = new DataWeightedGraph<string>();
            uint a1 = graph.AddNode("1");
            uint a2 = graph.AddNode("2");
            uint a3 = graph.AddNode("3");
            uint a4 = graph.AddNode("4");
            uint a5 = graph.AddNode("5");
            graph.Link(a1, a3, 1.0f);
            graph.Link(a1, a2, 3.0f);
            graph.Link(a3, a2, 1.0f);
            graph.Link(a2, a4, 2.0f);
            graph.Link(a4, a5, 1.0f);
            graph.Link(a1, a5, 7.0f);

            BellmanFordSearch bf = new BellmanFordSearch(graph, a1);
            bf.Search();


            SearchResult result = bf[a4];
            Assert.AreEqual(4.0, result.Cost);
            Assert.AreEqual(4, result.Count);
        }
    }
#endif
}