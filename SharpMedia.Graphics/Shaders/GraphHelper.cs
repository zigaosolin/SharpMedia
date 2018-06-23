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
using SharpMedia.Graphics.Shaders.Operations;
using SharpMedia.Math.Graphs;

namespace SharpMedia.Graphics.Shaders
{
    /// <summary>
    /// A graph helper class.
    /// </summary>
    internal static class GraphHelper
    {
        #region Private Members

        static uint AddOperationToGraph(IOperation op, DataGraph<IOperation> graph, uint parentID)
        {
            if (op == null) return 0;

            // We link this node to parent (and potencially create it if it does not exist).
            uint id;
            if (!graph.Contains(op, out id))
            {
                id = graph.AddNode(op);
            }

            // We add link only if parent exists and the link does not yet exist.
            if (parentID != uint.MaxValue && !graph.LinkExistsDirectional(parentID, id))
            {
                graph.LinkDirectional(parentID, id);
            }

            // We do recursevelly for children.
            foreach (Pin p in op.Inputs)
            {
                AddOperationToGraph(p.Owner, graph, id);
            }

            // We return self id.
            return id;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Obtains sorted operations (order is back to front).
        /// </summary>
        /// <returns>The list of all operations in ShaderCode:</returns>
        public static List<IOperation> GetSortedOperations(IOperation output)
        {
            // We build graph of all operations.
            DataGraph<IOperation> graph = new DataGraph<IOperation>();
            uint outputID = AddOperationToGraph(output, graph, uint.MaxValue);

            // Now we have to list operations in correct order.
            List<uint> ordered = GraphAnalysis.ListNodesInOrder(outputID, graph);
            List<IOperation> result = new List<IOperation>(ordered.Count);

            // We simply "back copy" operations.
            for (int i = 0; i < ordered.Count; i++)
            {
                result.Add(graph[ordered[i]]);
            }

            return result;
        }

        #endregion

    }
}
