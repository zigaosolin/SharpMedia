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
    /// A foreach node.
    /// </summary>
    public class ForEachNode : HolderNode
    {
        #region Private Members
        string templateTarget;
        #endregion

        #region Public Members

        public ForEachNode(string templateTarget, IDocumentNode[] nodes)
            : base(nodes)
        {
            this.templateTarget = templateTarget;
        }

        #endregion

        #region IDocumentNode Members

        public override void Emit(ITemplateSet templateSet, System.IO.StreamWriter writer)
        {
            object ttarget = templateSet.Provide(templateTarget);
            if (ttarget is ITemplateSet)
            {
                ITemplateSet target = ttarget as ITemplateSet;


                foreach (string s in target.Available)
                {
                    ITemplateSet set = target.Provide(s) as ITemplateSet;

                    writer.WriteLine();
                    writer.WriteLine("\t\t//#foreach instanced to '{0}'", set.Name);
                    base.Emit(set, writer);
                    writer.WriteLine("//#endfor instanced to '{0}'", set.Name);
                }

            }
            else
            {
                Console.WriteLine("Error: could not resolve target '{0}' as ITemplateSet", templateTarget);
            }
        }

        #endregion
    }
}
