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
using System.Xml;

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{

    /// <summary>
    /// Similiar to ASTXmlParseable, only that the internal node holds the type and
    /// all the members, this is only a binder.
    /// </summary>
    internal class ASTXmlParsableProperty : IXmlParsable, IPreEmittable
    {
        #region Private Members
        Type resultType;
        ASTXmlParsable inner;
        #endregion

        #region Public Members

        public ASTXmlParsableProperty(Type resultType)
        {
            this.resultType = resultType;
        }

        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            inner.Emit(context, writer);
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get { List<IElement> el = new List<IElement>(); el.Add(inner); return el; }
        }

        #endregion

        #region IPreEmittable Members

        public void PreEmit(CompileContext context, System.IO.TextWriter writer)
        {
            inner.PreEmit(context, writer);
        }

        #endregion

        #region IXmlParsable Members

        public void Parse(CompileContext context, System.Xml.XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;

                if (inner != null)
                {
                    throw new ParseException("Cannot have multiple nodes in property.");
                }

                Type type = context.ResolveType(child.Prefix, child.LocalName);
                if (!Common.IsTypeSameOrDerived(resultType, type))
                {
                    throw new ParseException(string.Format("The types '{0}' and '{1}' are not compatible", 
                        resultType.FullName, type != null ? type.FullName : "null"));
                }

                inner = new ASTXmlParsable(type);
                inner.Parse(context, child);
            }
        }

        #endregion
    }
}
