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

namespace SharpMedia.Math.Graphs
{
    /// <summary>
    /// A graph builder interface.
    /// </summary>
    public interface IGraphBuilder<T>
    {
        /// <summary>
        /// Creates a new node (unlinked).
        /// </summary>
        /// <returns></returns>
        uint AddNode(T data);

        /// <summary>
        /// Links the nodes.
        /// </summary>
        /// <param name="node1">The node1.</param>
        /// <param name="node2">The node2.</param>
        /// <param name="weight">The weight of link.</param>
        /// <remarks>If nodes are already linked, the result is undefined.</remarks>
        void Link(uint node1, uint node2, float weight);

        /// <summary>
        /// Unlinks nodes.
        /// </summary>
        /// <param name="node1">The node1.</param>
        /// <param name="node2">The node2.</param>
        /// <returns>Were the nodes unlinked (they are not if they were not linked).</returns>
        bool UnLink(uint node1, uint node2);
    }
}
