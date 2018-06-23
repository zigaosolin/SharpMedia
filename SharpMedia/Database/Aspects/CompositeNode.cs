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
using SharpMedia.Database.Managed;
using SharpMedia.Testing;

namespace SharpMedia.Database.Aspects
{

    /// <summary>
    /// Compositing rules.
    /// </summary>
    [Flags]
    public enum CompositingRules
    {
        None = 0,

        /// <summary>
        /// Should we merge children of all.
        /// </summary>
        MergeChildren = 1,

        /// <summary>
        /// Should we merge typed stream.
        /// </summary>
        MergeTypedStream = 2
    }

    /// <summary>
    /// A composite node aspect. It merges nodes in certain order and provide data as if
    /// those nodes were one node. On conflicts, the order is used to resolve them.
    /// </summary>
    [Serializable]
    public class CompositeNode<T>
    {
        #region Private Members
        Node<object>[] children;
        #endregion




    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class CompositeNodeTest
    {
        DatabaseManager manager;

        /// <summary>
        /// A composite node layer.
        /// </summary>
        public void Composite()
        {
            
        }

    }

#endif
}
