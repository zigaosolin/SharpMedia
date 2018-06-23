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
    /// An A* serching algorithms.
    /// </summary>
    /// <typeparam name="T">The type of node.</typeparam>
    /// <remarks>Only non-negative weights are possible.</remarks>
    public sealed class AStarSearch : ISearch
    {
        #region Private Structs

        /// <summary>
        /// The dijkstra node.
        /// </summary>
        struct AStarNode : IComparable<AStarNode>
        {
            /// <summary>
            /// The distance from origin.
            /// </summary>
            public float Distance;

            /// <summary>
            /// The node we are refering to.
            /// </summary>
            public uint Node;


            /// <summary>
            /// Initializes a new instance of the <see cref="DijkstraNode"/> struct.
            /// </summary>
            /// <param name="distance">The distance.</param>
            /// <param name="node">The node.</param>
            public AStarNode(float distance, uint node)
            {
                Distance = distance;
                Node = node;
            }

            #region IComparable<AStarNode> Members

            public int CompareTo(AStarNode other)
            {
                return this.Distance.CompareTo(other.Distance);
            }

            #endregion
        }

        #endregion

        #region Private Members
        uint goal;
        SearchStatus status = SearchStatus.Running;
        uint[] bestPrevious;
        float[] distance;
        bool[] wasProcessed;
        MinHeap<AStarNode> minHeap = new MinHeap<AStarNode>();
        IWeightedGraph graph;
        uint nodesVisited = 0;
        Heuristic<uint> heuristics;
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="Dijkstra"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="start">The start.</param>
        /// <param name="goal">The goal.</param>
        public AStarSearch([NotNull] IWeightedGraph graph, uint start, uint goal, [NotNull] Heuristic<uint> heuristics)
        {
            if ((graph.Properties & GraphProperties.Dnyamic) != 0)
            {
                throw InvalidGraphPropertiesException.MustNotHave(GraphProperties.Dnyamic);
            }

            int count = (int)graph.Count;
            if (start >= (uint)count || goal >= (uint)count)
            {
                throw new ArgumentException("Goal or start out of graph nodes.");
            }

            this.graph = graph;
            this.goal = goal;
            this.heuristics = heuristics;

            // We initialize distance and best previous.

            bestPrevious = new uint[count];
            wasProcessed = new bool[count];
            distance = new float[count];

            for (int i = 0; i < count; i++)
            {
                distance[i] = float.PositiveInfinity;
                bestPrevious[i] = uint.MaxValue;
                wasProcessed[i] = false;
            }

            distance[start] = 0.0f;
            minHeap.Push(new AStarNode(heuristics(start, goal), start));
        }

        /// <summary>
        /// Allows changing goal, and reuses computation results.
        /// </summary>
        /// <param name="node">The node.</param>
        public void ChangeGoal(uint node)
        {
            if (node >= graph.Count)
            {
                throw new ArgumentException("Invalid node: " + node);
            }

            goal = node;

            if (!wasProcessed[node])
            {
                status = SearchStatus.Running;
            }
            else
            {
                // Check the status.
                status = Result != null ? SearchStatus.Complete : SearchStatus.NotFound;
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
            if (status == SearchStatus.Complete ||
               status == SearchStatus.NotFound) return true;

            uint searchNode = uint.MaxValue;
            float bestDistance = float.PositiveInfinity;

            while (minHeap.Count > 0)
            {
                AStarNode dnode = minHeap.Pop();
                if (!wasProcessed[dnode.Node])
                {
                    searchNode = dnode.Node;
                    break;

                }
            }

            // We check if not found.
            if (searchNode == uint.MaxValue)
            {
                status = SearchStatus.NotFound;
                return true;
            }

            // We search if goal.
            if (searchNode == goal)
            {
                // We must push back because it was not processed. This is
                // needed if we change the goal node.
                minHeap.Push(new AStarNode(bestDistance, searchNode));

                status = SearchStatus.Complete;
                return true;
            }

            // Update node visited count.
            nodesVisited++;

            // We tag that it was processed.
            wasProcessed[searchNode] = true;

            // We have found it, we inspect all links.
            List<Edge> edges = graph.NextWeightedNodes(searchNode);
            for (int j = 0; j < edges.Count; j++)
            {
                Edge edge = edges[j];
                float newDist = distance[searchNode] + edge.Weight;
                if (newDist < distance[edge.Node])
                {
                    // Update the distance.
                    distance[edge.Node] = newDist;
                    bestPrevious[edge.Node] = searchNode;

                    // We also update the min-heap.
                    minHeap.Push(new AStarNode(newDist + heuristics(edge.Node, goal), edge.Node));
                }
            }

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
                if (status != SearchStatus.Complete) return null;

                // We construct result.
                if (bestPrevious[goal] == uint.MaxValue) return null;

                List<uint> result = new List<uint>();

                uint backtrace = goal;
                while (bestPrevious[backtrace] != uint.MaxValue)
                {
                    result.Add(backtrace);
                    backtrace = bestPrevious[backtrace];
                }

                // Add beginning.
                result.Add(backtrace);

                // We have full backtrace.
                result.Reverse();

                return new SearchResult(result.ToArray(), distance[goal]);
            }
        }

        public SearchResult[] Results
        {
            get
            {
                SearchResult res = Result;
                if (res != null) return new SearchResult[] { res };
                return new SearchResult[0];
            }
        }


        public float DoneRatio
        {
            get
            {
                // This is overestimate, worst-case scenario.
                return (float)nodesVisited / (float)(graph.Count - 1);
            }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class AStartTest
    {
        [CorrectnessTest]
        public void SimpleTest()
        {
            DataWeightedGraph<Vector2f> graph = new DataWeightedGraph<Vector2f>();
            uint a1 = graph.AddNode(Vector2f.Zero);
            uint a2 = graph.AddNode(new Vector2f(1.0f, 0.0f));
            uint a3 = graph.AddNode(new Vector2f(0.5f, 0.5f));
            uint a4 = graph.AddNode(new Vector2f(2.0f, 0.5f));
            uint a5 = graph.AddNode(new Vector2f(3.0f, 0.0f));
            graph.Link(a1, a2, 1.0f);
            graph.Link(a2, a3, (new Vector2f(0.5f, 0.5f)).Length);
            graph.Link(a2, a4, (new Vector2f(1.5f, 0.0f)).Length);
            graph.Link(a4, a5, (new Vector2f(1.0f, 0.5f)).Length);

            AStarSearch aStar = new AStarSearch(graph, a1, a5,
                delegate(uint from, uint to)
                {
                    return (graph[from] - graph[to]).Length;
                });
            aStar.Search();

            SearchResult result = aStar.Result;
            Assert.AreEqual(4, result.Count);
        }
    }
#endif
}
