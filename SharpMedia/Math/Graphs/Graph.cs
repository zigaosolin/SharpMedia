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
using System.Collections.ObjectModel;

namespace SharpMedia.Math.Graphs
{
    /// <summary>
    /// Properties of graphs.
    /// </summary>
    [Flags]
    public enum GraphProperties
    {
        /// <summary>
        /// No graph properties.
        /// </summary>
        None = 0,

        /// <summary>
        /// Graph is simple; has no loops and no same element referencing.
        /// </summary>
        Simple = 1,

        /// <summary>
        /// Graph is directed.
        /// </summary>
        Directed = 2 | Simple,

        /// <summary>
        /// Graph is acyclic.
        /// </summary>
        ACyclic = 4 | Simple, 

        /// <summary>
        /// Directed acyclic graph.
        /// </summary>
        DirectedAcylic = Directed | ACyclic,

        /// <summary>
        /// A graph is binary tree.
        /// </summary>
        Binary = 8 | Tree, 

        /// <summary>
        /// Graph is a tree.
        /// </summary>
        Tree = 16 | Simple,

        /// <summary>
        /// Dynamic graph, it's size is not known or is infinite.
        /// </summary>
        Dnyamic = 32,

        /// <summary>
        /// This is a hint flag - otherwise, graph is considered sparse. The
        /// actual density can be obtained through graph analysis.
        /// </summary>
        Dense = 64
    }


    /// <summary>
    /// A graph is an interface that allows searching. The T is the node to search for. The
    /// first node begins with index 0 and goes
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// Graph properties to ease search.
        /// </summary>
        GraphProperties Properties { get; }

        /// <summary>
        /// Returns next nodes from current node.
        /// </summary>
        /// <param name="from">The current node.</param>
        /// <returns>A list of next nodes.</returns>
        List<uint> NextNodes(uint from);

        /// <summary>
        /// The order of node; e.g. number of links to node.
        /// </summary>
        /// <param name="node">The node whose order we seek.</param>
        /// <returns>Order of node.</returns>
        uint Order(uint node);

        /// <summary>
        /// Number of nodes, indices go from 0 to Count-1.
        /// </summary>
        /// <remarks>
        /// Algorithm can return uint.MaxValue if it is Dynamic - it's size is not known.
        /// </remarks>
        uint Count { get; }
    }

    /// <summary>
    /// An edge.
    /// </summary>
    public struct Edge
    {
        /// <summary>
        ///  A weight of edge.
        /// </summary>
        public float Weight;

        /// <summary>
        /// The node that it links to.
        /// </summary>
        public uint Node;

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> struct.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="weight">The weight.</param>
        public Edge(uint node, float weight)
        {
            Node = node;
            Weight = weight;
        }
    }

    /// <summary>
    /// A weighted graph, allows weights.
    /// </summary>
    public interface IWeightedGraph : IGraph
    {

        /// <summary>
        /// A next weighted node from here.
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        List<Edge> NextWeightedNodes(uint from);
    }

}
