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
    /// The decision tree iterator, at each step, a decision must be made to proceed.
    /// </summary>
    public sealed class DecisionTreeIterator
    {
        #region Public Members

        uint CurrentNode
        {
            get
            {
                return 0;
            }
        }

        List<uint> PossibleMoves
        {
            get
            {
                return null;
            }
        }

        void Decide(int index)
        {

        }

        #endregion
    }

    /// <summary>
    /// A decision tree.
    /// </summary>
    public sealed class DecisionTree : IGraph
    {
        #region Static Members

        public static DecisionTree FromResults(List<SearchResult> results)
        {
            return null;
        }

        #endregion

        #region Public Members

        public DecisionTreeIterator Iterator
        {
            get
            {
                return null;
            }
        }

        public DecisionTree()
        {

        }

        #endregion

        #region IGraph Members

        public GraphProperties Properties
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public List<uint> NextNodes(uint from)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public uint Order(uint node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public uint Count
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
