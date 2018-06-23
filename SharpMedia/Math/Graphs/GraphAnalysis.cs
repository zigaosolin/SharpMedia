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
    /// A graph analysis utility class.
    /// </summary>
    public static class GraphAnalysis
    {
        #region Private Members

        /// <summary>
        /// Holds visited nodes that have not yet been processed.
        /// </summary>
        class VisitedNode
        {
            public uint node;
            public uint visitCount;
        }

        /// <summary>
        /// Holds search data stack.
        /// </summary>
        class SearchData
        {
            public uint node;
            public int index;
            public List<uint> subNodes;

            public SearchData(uint node, List<uint> subNodes)
            {
                this.node = node;
                this.index = 0;
                this.subNodes = subNodes;
            }

            public bool Next(out uint data)
            {
                if (this.index >= this.subNodes.Count)
                {
                    data = 0;
                    return false;
                } else {
                    data = subNodes[index++];
                    return true;
                }
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// This method finds all nodes of graph reachable from node.
        /// </summary>
        /// <typeparam name="T">The node type.</typeparam>
        /// <param name="node">The beginning node.</param>
        /// <param name="graph">The graph where node exists.</param>
        /// <param name="comparer">Comparer is needed </param>
        /// <returns>List of nodes.</returns>
        public static List<uint> ListNodes([NotNull] uint node, [NotNull] IGraph graph)
        {
            return null;
        }

        /// <summary>
        /// Computes the graph density index.
        /// </summary>
        /// <remarks>The value of 1.0 means each vertex has connection with every other vertex, 0.0
        /// means that there are no connections. </remarks>
        /// <param name="graph"></param>
        /// <param name="reachableFrom"></param>
        /// <returns></returns>
        public static float GraphDensity([NotNull] IGraph graph, uint reachableFrom)
        {
            return 0.0f;
        }

        /// <summary>
        /// Visits all recheable nodes in correct order.
        /// </summary>
        /// <remarks>
        /// Graph must be directed, acyclic.
        /// </remarks>
        /// <typeparam name="T">The type of graph vertex.</typeparam>
        /// <param name="node">The beginning node.</param>
        /// <param name="graph">The graph structure.</param>
        /// <returns>Listed nodes in correct order.</returns>
        public static List<uint> ListNodesInOrder(uint node, [NotNull] IGraph graph) 
        {

            List<uint> orderedNodes = new List<uint>();

            // We keep track of visited nodes and how many times they were visited.
            // We proceed when count is the same as number of links to that node.
            List<VisitedNode> visitedNode = new List<VisitedNode>(10);
            Stack<SearchData> stack = new Stack<SearchData>();
            stack.Push(new SearchData(node, graph.NextNodes(node)));

            // We also add first.
            orderedNodes.Add(node);

            // Until we finish.
            while (true)
            {
                // We can exit.
                if (stack.Count == 0) break;

                SearchData search = stack.Pop();

                if (search.Next(out node))
                {
                    // It is still useful.
                    stack.Push(search);

                    // We have next in node. We check if we can proceed.
                    uint order = graph.Order(node);
                    if (order == 1)
                    {
                        // We can insert it here, it is ordered.
                        orderedNodes.Add(node);

                        stack.Push(new SearchData(node, graph.NextNodes(node)));
                        continue;
                    }

                    // We have to make sure we add the count to visited. First check
                    // if it exists.
                    bool found = false;
                    for (int i = 0; i < visitedNode.Count; ++i)
                    {
                        // We found it.
                        if (visitedNode[i].node == node)
                        {
                            visitedNode[i].visitCount++;
                            if (visitedNode[i].visitCount >= order)
                            {
                                // We can insert it here, it is ordered.
                                orderedNodes.Add(node);

                                stack.Push(new SearchData(node, graph.NextNodes(node))); 
                            }
                            found = true;
                            break;
                        }
                    }

                    // We may already found it.
                    if (found) continue;

                    // We must add it if not found.
                    VisitedNode vnode = new VisitedNode();
                    vnode.node = node;
                    vnode.visitCount = 1;
                    visitedNode.Add(vnode);
                }
            }

            return orderedNodes;
            
        }

        #endregion

    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class GraphicsAnalysisTest
    {
        [CorrectnessTest]
        public void ListInOrder()
        {
            DataGraph<string> g = new DataGraph<string>();
            uint n0 = g.AddNode("0");
            uint n1 = g.AddNode("1");
            uint n2 = g.AddNode("2");
            uint n3 = g.AddNode("3");
            uint n4 = g.AddNode("4");
            uint n5 = g.AddNode("5");

            g.LinkDirectional(n0, n2);
            g.LinkDirectional(n2, n1);
            g.LinkDirectional(n2, n3);
            g.LinkDirectional(n3, n4);
            g.LinkDirectional(n1, n4);
            g.LinkDirectional(n1, n5);
            g.LinkDirectional(n4, n5);

            // We perform analysis.
            List<uint> r = GraphAnalysis.ListNodesInOrder(n0, g);

            Assert.AreEqual(6, r.Count);

        }

        [CorrectnessTest]
        public void ListInOrder2()
        {
            DataGraph<string> g = new DataGraph<string>();
            uint n0 = g.AddNode("0");
            uint n1 = g.AddNode("1");
            uint n2 = g.AddNode("2");
            uint n3 = g.AddNode("3");
            uint n4 = g.AddNode("4");
            uint n5 = g.AddNode("5");
            uint n6 = g.AddNode("6");
            uint n7 = g.AddNode("7");

            g.LinkDirectional(n0, n1);
            g.LinkDirectional(n0, n5);
            g.LinkDirectional(n0, n3);
            g.LinkDirectional(n1, n2);
            g.LinkDirectional(n1, n4);
            g.LinkDirectional(n2, n3);
            g.LinkDirectional(n5, n6);
            g.LinkDirectional(n5, n7);
            g.LinkDirectional(n6, n3);

            // We perform analysis.
            List<uint> r = GraphAnalysis.ListNodesInOrder(n0, g);


            Assert.AreEqual(8, r.Count);
        }
    }

#endif
}
