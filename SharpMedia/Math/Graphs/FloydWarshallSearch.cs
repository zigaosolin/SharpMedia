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
    /// Finds the best part from any pair of vertices.
    /// </summary>
    /// <remarks>
    /// If you know the starting position, use Bellman-Ford search and if you know both start and
    /// end position, use Dijkstra.
    /// </remarks>
    public sealed class FloydWarshallSearch : ISearch
    {
        #region Private Members
        SearchStatus    status = SearchStatus.Running;
        int             currentIndex = 0;
        IGraph          graph;
        float[,]        path;
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="FloydWarshallSearch"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        public FloydWarshallSearch([NotNull] IWeightedGraph graph)
        {
            if ((graph.Properties & GraphProperties.Dnyamic) != 0)
            {
                throw InvalidGraphPropertiesException.MustNotHave(GraphProperties.Dnyamic);
            }

            // Extract count.
            int count = (int)graph.Count;

            this.graph = graph;
            this.path = new float[count, count];

            for (int i = 0; i < count; i++)
            {
                // Initialize all to infinite.
                for (int j = 0; j < count; j++)
                {
                    path[i, j] = float.PositiveInfinity;
                }

                // Correct for those that are not infinite.
                List<Edge> edges = graph.NextWeightedNodes((uint)i);
                for (int k = 0; k < edges.Count; k++)
                {
                    path[i, edges[k].Node] = edges[k].Weight;
                }
            }
            
        }

        /// <summary>
        /// Gets the <see cref="SharpMedia.Math.Graphs.SearchResult"/> with the specified from.
        /// </summary>
        /// <value></value>
        public float this[uint from, uint to]
        {
            get
            {
                return GetResult(from, to);
            }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public float GetResult(uint from, uint to)
        {
            if (status != SearchStatus.Complete) return float.PositiveInfinity;

            // We return only the result with cost.
            return path[from, to];
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

            int count = (int)graph.Count;
            int k = currentIndex;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    path[i, j] = MathHelper.Min(path[i, j], path[i, k] + path[k, j]);
                }
            }


            // We may need to finish.
            if (++currentIndex >= (int)graph.Count)
            {
                status = SearchStatus.Complete;
            }

            return false;
        }

        public void Search()
        {
            while (!NextIteration()) ;
        }

        public float DoneRatio
        {
            get { return (float)currentIndex / (float)graph.Count; }
        }

        public SearchResult Result
        {
            get { return null; }
        }

        public SearchResult[] Results
        {
            get { return new SearchResult[0]; }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class FloydWarshallTest
    {
        [CorrectnessTest]
        public void AllPaths()
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

            FloydWarshallSearch floydMarshall = new FloydWarshallSearch(graph);
            floydMarshall.Search();

            Assert.AreEqual(5.0f, floydMarshall[a1,a5]);
        }
    }
#endif

}
