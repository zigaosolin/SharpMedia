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
    /// A data graph, holding per-index data associated with user defined data.
    /// </summary>
    /// <typeparam name="T">The typename, user defined data.</typeparam>
    public sealed class DataWeightedGraph<T> : IWeightedGraph, IGraphBuilder<T>
    {
        #region Private Members
        List<T> data = new List<T>();
        List<List<Edge>> links = new List<List<Edge>>();
        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <value></value>
        public T this[uint index]
        {
            get
            {
                return data[(int)index];
            }
            set
            {
                data[(int)index] = value;
            }
        }

        /// <summary>
        /// Converts search result to string based on graph.
        /// </summary>
        /// <param name="result">The result.</param>
        public string ToString(SearchResult result)
        {
            StringBuilder b = new StringBuilder();
            bool init = true;
            foreach (int i in result)
            {
                if (init)
                {
                    init = false;
                }
                else
                {
                    b.Append(" -> ");
                }
                if (i >= data.Count) throw new InvalidCastException("Search results invalid.");
                b.Append(data[i].ToString());
            }
            return b.ToString();
        }

        #endregion

        #region IGraph Members

        public uint NodeCount
        {
            get { return (uint)data.Count; }
        }

        public uint Order(uint node)
        {
            if (node >= (uint)links.Count) return 0;
            return (uint)links[(int)node].Count;
        }

        public List<uint> NextNodes(uint from)
        {
            if (from <= data.Count)
            {
                List<Edge> l = links[(int)from];
                List<uint> r = new List<uint>(l.Count);
                for (int i = 0; i < l.Count; i++)
                {
                    r.Add(l[i].Node);
                }
                return r;
            }
            return new List<uint>();
        }

        public GraphProperties Properties
        {
            get { return GraphProperties.None; }
        }

        public uint Count
        {
            get { return (uint)data.Count; }
        }

        public List<Edge> NextWeightedNodes(uint from)
        {
            if (from <= data.Count) return links[(int)from];
            return new List<Edge>();
        }

        #endregion

        #region IGraphBuilder<T> Members

        public bool UnLink(uint node1, uint node2)
        {
            links[(int)node1].RemoveAll(delegate(Edge e1) { return e1.Node == node2; });
            links[(int)node2].RemoveAll(delegate(Edge e2) { return e2.Node == node1; });
            return true;
        }

        public uint AddNode(T d)
        {
            data.Add(d);
            links.Add(new List<Edge>());
            return (uint)(data.Count - 1);
        }

        public void Link(uint first, uint second, float cost)
        {
            if (first < 0 || first >= data.Count) throw new ArgumentException("Invalid link node.");
            if (second < 0 || second >= data.Count) throw new ArgumentException("Invalid link node.");
            links[(int)first].Add(new Edge(second, cost));
            links[(int)second].Add(new Edge(first, cost));
        }

        #endregion
    }
}
