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
    public sealed class DataGraph<T> : IGraph, IGraphBuilder<T> 
    {
        #region Private Members
        List<T> data = new List<T>();
        List<List<uint>> links = new List<List<uint>>();
        List<uint> order = new List<uint>();
        #endregion

        #region Public Members

        /// <summary>
        /// Checks if data is already contained in graph.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Contains(T node, out uint index)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Equals(node))
                {
                    index = (uint)i;
                    return true;
                }
            }
            index = 0;
            return false;
        }

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

        /// <summary>
        /// Links two nodes both ways.
        /// </summary>
        /// <param name="first">The first node.</param>
        /// <param name="second">The second node.</param>
        public void Link(uint first, uint second)
        {
            if (first < 0 || first >= data.Count) throw new ArgumentException("Invalid link node.");
            if (second < 0 || second >= data.Count) throw new ArgumentException("Invalid link node.");
            links[(int)first].Add(second);
            links[(int)second].Add(first);
            order[(int)first]++;
            order[(int)second]++;
        }

        /// <summary>
        /// Links two nodes directionally.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void LinkDirectional(uint from, uint to)
        {
            if (from < 0 || from >= data.Count) throw new ArgumentException("Invalid link node.");
            if (from < 0 || to >= data.Count) throw new ArgumentException("Invalid link node.");
            links[(int)from].Add(to);
            order[(int)to]++;

        }

        /// <summary>
        /// Does the directional link already exist.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public bool LinkExistsDirectional(uint from, uint to)
        {
            if (from < 0 || from >= data.Count) throw new ArgumentException("Invalid link node.");
            if (from < 0 || to >= data.Count) throw new ArgumentException("Invalid link node.");

            if (links[(int)from].Contains(to)) return true;
            return false;
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
            return order[(int)node];
        }

        public List<uint> NextNodes(uint from)
        {
            if (from <= data.Count) return links[(int)from];
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

        #endregion

        #region IGraphBuilder<T> Members

        public uint AddNode(T d)
        {
            data.Add(d);
            links.Add(new List<uint>());
            order.Add(0);
            return (uint)(data.Count - 1);
        }

        public void Link(uint node1, uint node2, float weight)
        {
            Link(node1, node2);
        }

        public bool UnLink(uint node1, uint node2)
        {
            bool r = false;
            if (links[(int)node1].Remove(node2))
            {
                order[(int)node2]--;
                r = true;
            }

            if (links[(int)node2].Remove(node1))
            {
                order[(int)node1]--;
                r = true;
            }
            return r;
        }

        #endregion
    }
}
