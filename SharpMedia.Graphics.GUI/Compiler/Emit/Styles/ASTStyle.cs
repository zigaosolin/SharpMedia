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

namespace SharpMedia.Graphics.GUI.Compiler.Emit.Styles
{

    /// <summary>
    /// A style.
    /// </summary>
    internal class ASTStyle : IXmlParsable, IEmittable, IPreEmittable
    {
        #region Private Members
        string name;
        Type type;
        SortedList<string, ASTStateStyle> styles = new SortedList<string,ASTStateStyle>();
        #endregion

        #region IXmlParsable Members

        public void Parse(CompileContext context, System.Xml.XmlNode node)
        {
            if (node.Attributes["Name"] == null)
            {
                throw new ParseException("Style's name must be defined.");
            }

            type = context.ResolveType(node.Prefix, node.LocalName);
            name = node.Attributes["Name"].InnerText;

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;

                ASTStateStyle state = new ASTStateStyle(type);
                state.Parse(context, child);

                styles.Add(child.Name, state);
            }

            // Add to styles list.
            context.Styles.Add(name, this);
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get 
            {
                List<IElement> el = new List<IElement>();
                for (int i = 0; i < styles.Count; i++)
                {
                    el.Add(styles.Values[i]);
                }
                return el;
            }
        }

        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {

            writer.Write("new SharpMedia.Graphics.GUI.Style(");

            for (int i = 0; i < styles.Count; i++)
            {
                writer.Write("new System.Collections.Generic.KeyValuePair<string, SharpMedia.Graphics.GUI.IStateStyle>(\"{0}\", ", styles.Keys[i]);
                
                styles.Values[i].Emit(context, writer);

                if (i != styles.Count - 1)
                {
                    // A bit of formatting
                    writer.Write("),");
                }
                else
                {
                    writer.Write(")");
                }
            }

            writer.Write(")");
        }

        #endregion

        #region IPreEmittable Members

        public void PreEmit(CompileContext context, System.IO.TextWriter writer)
        {
            for (int i = 0; i < styles.Count; i++)
            {
                styles.Values[i].PreEmit(context, writer);
            }
        }

        #endregion
    }
}
