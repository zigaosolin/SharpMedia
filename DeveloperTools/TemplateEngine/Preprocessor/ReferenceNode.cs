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

namespace TemplateEngine.Preprocessor
{

    /// <summary>
    /// A reference node.
    /// </summary>
    public class ReferenceNode : IDocumentNode
    {
        #region Private Members
        string reference;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="reference"></param>
        public ReferenceNode(string reference)
        {
            this.reference = reference;
        }

        #endregion

        #region IDocumentNode Members

        public void Emit(ITemplateSet templateSet, System.IO.StreamWriter writer)
        {
            object obj = templateSet.Provide(reference);
            if (obj == null)
            {
                throw new Exception(string.Format("Missing reference {0}", reference));
            }

            writer.Write(obj.ToString());
        }

        #endregion
    }
}
