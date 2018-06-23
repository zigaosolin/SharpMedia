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
using SharpMedia.Graphics.GUI.Widgets;
using System.Reflection;
using SharpMedia.Graphics.GUI.Metrics;
using SharpMedia.Scripting.CompilerFacilities;
using System.Globalization;

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{

    /// <summary>
    /// An generic widget.
    /// </summary>
    internal class ASTWidget : IXmlParsable, IEmittable, IResolvable
    {
        #region Private Members
        Type type;
        Type changeType;
        string name;

        string styleLink;

        // Values that need to be set.
        SortedList<string, IElement> values = new SortedList<string, IElement>();
        SortedList<string, IElement> children = new SortedList<string, IElement>();

        #endregion

        #region Public Members

        /// <summary>
        /// Class name created by widget.
        /// </summary>
        public string ClassName
        {
            get { return name; }
        }
        #endregion

        #region IXmlParsable Members

        void ProcessAttribute(CompileContext context, XmlAttribute node)
        {
            values.Add(node.LocalName, ParsingHelper.ParseValue(context, changeType, node.LocalName, node.InnerText));
        }

        void ProcessChild(CompileContext context, XmlNode node)
        {
            if (node.NodeType != XmlNodeType.Element) return;


            if (node.Prefix == StandardPrefixes.Variable)
            {
                throw new NotSupportedException("Variables not yet supported.");
            }
            else if (node.Prefix == StandardPrefixes.Value || node.Prefix == StandardPrefixes.Property)
            {
                if (node.Name == "Style")
                {
                    styleLink = node["Style"].InnerText;
                }
                else
                {
                    values.Add(node.LocalName, ParsingHelper.ParseValue(context, changeType, node));
                }
            }
            else
            {
                if (type.GetInterface("SharpMedia.Graphics.GUI.Widgets.Containers.IContainer") == null)
                {
                    throw new ParseException(string.Format("Only containers can have child widgets, " +
                    "{0} is not a container.", name));
                }

                Type resolveType = context.ResolveType(node.Prefix, node.LocalName);

                if(resolveType != null && resolveType.GetInterface("SharpMedia.Graphics.GUI.Widgets.IWidget") != null)
                {

                    // We have a child element.
                    ASTWidget widget = new ASTWidget();
                    children.Add(node.LocalName, widget);

                    widget.Parse(context, node);
                } else {
                    throw new ParseException("Only widgets are acceptable as children.");
                }
            }
        }

        public void Parse(CompileContext context, System.Xml.XmlNode node)
        {
            // We first find the type.
            type = context.ResolveType(node.Prefix, node.LocalName);
            if (!Common.IsTypeSameOrDerived(typeof(IWidget), type))
            {
                throw new ParseException(string.Format("The element must be of type IWidget, "+
                    "'{0}' does not implement this interface", type.FullName));
            }

            // Previously, change context was not the same as the type, not they alias.
            changeType = type;

            // Now we try to find name attribute.
            XmlAttribute nameAttr = node.Attributes["Name"];
            if (nameAttr != null)
            {
                name = nameAttr.InnerText;
                if (name.Contains(" "))
                {
                    throw new ParseException("The name contains spaces, invalid.");
                }
            }
            else
            {
                throw new ParseException("Name of widget is required.");
            }

            nameAttr = node.Attributes["Style"];
            if (nameAttr != null)
            {
                styleLink = nameAttr.InnerText;
            }

            // We now go for all other attributes and values.
            foreach (XmlAttribute att in node.Attributes)
            {
                if (att.Name == "Name" || att.Name == "Style") continue;

                ProcessAttribute(context, att);
            }

            foreach (XmlNode node2 in node.ChildNodes)
            {
                ProcessChild(context, node2);
            }

            // Registers a widget.
            context.RegisterWidget(name, this);
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get 
            {
                List<IElement> all = new List<IElement>();
                all.AddRange(values.Values);
                all.AddRange(children.Values);

                return all;
            }
        }

        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("  /// <summary>");
            writer.WriteLine("  /// Partial class {0}, based on {1}.", name, type.Name);
            writer.WriteLine("  /// </summary>");
            writer.WriteLine("  public partial class {0} : {1} {{", name, type.FullName);
            writer.WriteLine();
            writer.WriteLine("    #region Private Members");
            writer.WriteLine("    {0} shareContext;", context.ApplicationContextType);
            writer.WriteLine("    #endregion");
            writer.WriteLine();
            writer.WriteLine("    #region Public Members");
            writer.WriteLine();
            writer.WriteLine("    /// <summary>");
            writer.WriteLine("    /// Default constructor for {0}.", name);
            writer.WriteLine("    /// </summary>");
            writer.WriteLine("    public {0}({1} shareContext) {{", name, context.ApplicationContextType);
            writer.WriteLine("      this.shareContext = shareContext;");
            writer.WriteLine("      this.shareContext.{0} = this;", name);
            writer.WriteLine();
            writer.WriteLine("      using({0} context = CreateChangeContextGeneral() as {0}) {{", changeType.FullName);


            // If style link.
            if (styleLink != null)
            {
                writer.WriteLine("        context.Style = shareContext.{0};", styleLink);
            }

            // We now initialize all values.
            for (int i = 0; i < values.Count; i++)
            {
                // We pre-emit.
                if (values.Values[i] is IPreEmittable)
                {
                    (values.Values[i] as IPreEmittable).PreEmit(context, writer);
                }

                // We actually emit.
                if (!(values.Values[i] is IEmittable)) continue;
                writer.Write("        context.{0} = ", values.Keys[i]);
                (values.Values[i] as IEmittable).Emit(context, writer);
                writer.WriteLine(";");
            }

            // We also add all children.
            for (int i = 0; i < children.Count; i++)
            {
                if (children.Values[i] is ASTWidget)
                {
                    writer.WriteLine("        context.AddChild(new {0}(shareContext));", 
                        (children.Values[i] as ASTWidget).ClassName);
                }
            }

            
            writer.WriteLine("      }");
            writer.WriteLine("    }");
            writer.WriteLine();
            writer.WriteLine("    #endregion");
            writer.WriteLine();
            writer.WriteLine("  }");

            for (int i = 0; i < children.Count; i++)
            {
                if (children.Values[i] is IEmittable)
                {
                    (children.Values[i] as IEmittable).Emit(context, writer);
                }
            }
        }

        #endregion

        #region IResolvable Members

        public void Resolve(CompileContext context, Resolver resolver)
        {
            if (styleLink != null && !context.Styles.ContainsKey(styleLink))
            {
                throw new ResolveException(string.Format("Could not resolve style {0}", styleLink));
            }
        }

        #endregion

    }
}
