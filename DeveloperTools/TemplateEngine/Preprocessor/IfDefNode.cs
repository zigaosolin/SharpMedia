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
    /// If defined node.
    /// </summary>
    public class IfDefNode : HolderNode
    {
        #region Private Members
        string[] conditions;
        #endregion

        #region Constructors

        public IfDefNode(IDocumentNode[] nodes, string[] conditions)
            : base(nodes)
        {
            this.conditions = conditions;
        }

        #endregion


        public override void Emit(ITemplateSet templateSet, System.IO.StreamWriter writer)
        {

            string comp = string.Empty;
            for(int i = 0; i < conditions.Length; i++)
            {
                if(comp != string.Empty) comp += " ";
                comp += conditions[i];
            }

            bool satisfied = true;
            for (int i = 0; i < conditions.Length; i++)
            {
                if (templateSet.Provide(conditions[i]) == null)
                {
                    
                    satisfied = false;
                    break;
                }
            }

            if (satisfied)
            {
                writer.WriteLine("//#ifdef {0}", comp);

                for (int i = 0; i < nodes.Length; i++)
                {
                    if (nodes[i] is ElseNode)
                    {
                        break;
                    }
                    nodes[i].Emit(templateSet, writer);
                }

                writer.Write("//#endif");
            }
            else
            {

                int i;
                for (i = 0; i < nodes.Length; i++)
                {
                    if (nodes[i] is ElseNode)
                    {
                        break;
                    }
                }

                for (; i < nodes.Length; i++)
                {
                    nodes[i].Emit(templateSet, writer);
                }
            }
        }
    }
}
