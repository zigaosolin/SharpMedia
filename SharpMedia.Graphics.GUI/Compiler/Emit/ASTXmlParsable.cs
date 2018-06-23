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
    /// A generic XML parsable.
    /// </summary>
    internal class ASTXmlParsable : IXmlParsable, IPreEmittable
    {
        #region Private Members
        Type type;
        
        string name = "!NOT INITIALIZED!";
        SortedList<string, IEmittable> properties = new SortedList<string, IEmittable>();
        #endregion

        #region Public Members

        public ASTXmlParsable(Type type)
        {
            if (type.GetConstructor(new Type[0]) == null)
            {
                throw new ParseException("A generic parseable must support default contructor.");
            }
            this.type = type;
        }

        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            writer.Write(name);
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get { return new List<IElement>(); }
        }

        #endregion

        #region IPreEmittable Members

        public void PreEmit(CompileContext context, System.IO.TextWriter writer)
        {
            name = context.GetNextTempName();

            writer.WriteLine("        {0} {1} = new {0}();", type.FullName, name);
            for (int i = 0; i < properties.Count; i++)
            {
                // Pre-emit variable.
                if (properties.Values[i] is IPreEmittable)
                {
                    (properties.Values[i] as IPreEmittable).PreEmit(context, writer);
                }

                // We write it and assign.
                writer.Write("        {0}.{1} = ", name, properties.Keys[i]);
                properties.Values[i].Emit(context, writer);
                writer.WriteLine(";");

            }
        }

        #endregion

        #region IXmlParsable Members

        public void Parse(CompileContext context, System.Xml.XmlNode node)
        {
            // We go for each attribute.
            foreach (XmlAttribute att in node.Attributes)
            {
                if (att.NodeType != XmlNodeType.Attribute) continue;

                // We try to parse it.
                properties.Add(att.LocalName, ParsingHelper.ParseValue(context, type, att.Name, att.InnerText));
            }

            // And for each child.
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;

                properties.Add(child.LocalName, ParsingHelper.ParseValue(context, type, child));
            }
        }

        #endregion
    }
}
