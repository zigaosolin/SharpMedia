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
using System.IO;

namespace TemplateEngine.Preprocessor
{

    /// <summary>
    /// A holder node.
    /// </summary>
    public class HolderNode : IDocumentNode
    {
        #region Private Members
        protected IDocumentNode[] nodes;
        #endregion

        #region Constructors

        public HolderNode(IDocumentNode[] nodes)
        {
            this.nodes = nodes;
        }

        #endregion

        #region IDocumentNode Members

        public virtual void Emit(ITemplateSet templateSet, StreamWriter writer)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i].Emit(templateSet, writer);
            }
        }

        #endregion

    }
}
