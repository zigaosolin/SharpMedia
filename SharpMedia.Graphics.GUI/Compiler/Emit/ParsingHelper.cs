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
using System.Reflection;
using SharpMedia.Graphics.GUI.Metrics;
using System.Globalization;
using SharpMedia.Scripting.CompilerFacilities;

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{
    internal static class ParsingHelper
    {

        #region Public Members


        /// <summary>
        /// We parse the value as XmlNode.
        /// </summary>
        public static IEmittable ParseValue(CompileContext context, Type changeType, XmlNode node)
        {
            // We find the name in changeType.
            PropertyInfo info = changeType.GetProperty(node.LocalName);
            if (info == null || !info.CanWrite)
            {
                throw new ParseException(string.Format("The property '{0}' for class '{1}' either does not exist" +
                    " or it is not writable.", node.Name, changeType));
            }

            

            Type type = info.PropertyType;


            // A property defines a bucket, where internal is of different type.
            if (node.Prefix == StandardPrefixes.Property)
            {
                ASTXmlParsableProperty prop = new ASTXmlParsableProperty(type);
                prop.Parse(context, node);

                return prop;
            }

            // A normal, direct initialization.
            if (type == typeof(GuiScalar))
            {
                ASTScalar scalar = new ASTScalar();
                scalar.Parse(context, node);
                return scalar;
            }
            else if (type == typeof(GuiRect))
            {
                ASTRect rect = new ASTRect();
                rect.Parse(context, node);
                return rect;
            }
            else if (type == typeof(GuiVector2))
            {
                ASTVector2 v = new ASTVector2();
                v.Parse(context, node);
                return v;
            }
            else if (type == typeof(string))
            {
                ASTString s = new ASTString();
                s.Value = node.InnerText;
                return s;
            }
            else
            {
                // If there is only a 'Value' tag with no children, we consider using normal parsing.
                if (node.Attributes.Count == 1 && node.Attributes["Value"] != null && node.ChildNodes.Count == 0)
                {
                    return ParseValue(context, changeType, node.LocalName, node.Value);
                }

                // If no alternative, we use generic XMLParseable.
                ASTXmlParsable parseable = new ASTXmlParsable(type);
                parseable.Parse(context, node);
                return parseable;

            }
        }

        /// <summary>
        /// We parse value as text.
        /// </summary>
        public static IEmittable ParseValue(CompileContext context, Type changeType, string property, string value)
        {
            // We find the name in changeType.
            PropertyInfo info = changeType.GetProperty(property);
            if (info == null || !info.CanWrite)
            {
                throw new ParseException(string.Format("The property '{0}' for class '{1}' either does not exist" +
                    " or it is not writable.", property, changeType.Name));
            }


            Type type = info.PropertyType;
            if (type == typeof(GuiScalar))
            {
                ASTScalar scalar = new ASTScalar();
                scalar.Parse(context, Token.StackForm(Token.Tokenize(value)));
                return scalar;
            }
            else if (type == typeof(GuiRect))
            {
                ASTRect rect = new ASTRect();

                rect.Parse(context, Token.StackForm(Token.Tokenize(value)));
                return rect;
            }
            else if (type == typeof(GuiVector2))
            {
                ASTVector2 v = new ASTVector2();

                v.Parse(context, Token.StackForm(Token.Tokenize(value)));
                return v;
            }
            else if (type == typeof(string))
            {
                ASTString s = new ASTString();

                s.Value = value;
                return s;
            }
            else if (type == typeof(int))
            {
                ASTInt s = new ASTInt();

                s.Value = int.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
                return s;
            }
            else if (type == typeof(uint))
            {
                ASTUInt s = new ASTUInt();

                s.Value = uint.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
                return s;
            }
            else if (type == typeof(bool))
            {
                ASTBool s = new ASTBool();

                s.Value = bool.Parse(value);
                return s;
            }
            else if (type.BaseType == typeof(Enum))
            {
                ASTEnum e = new ASTEnum(type, value);
                return e;
            }
            else if (type == typeof(Colour))
            {
                ASTColour c = new ASTColour();
                c.Parse(context, Token.StackForm(Token.Tokenize(value)));

                return c;
            }
            else if (type.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null,
           new Type[] { typeof(string) }, null) != null)
            {
                // We try with "Parse" method.
                ASTStringParsable p = new ASTStringParsable(value, type);

                return p;
            }
            else
            {
                throw new ParseException(string.Format("The property {0} is not supported because it is not built-in or does not" +
                    " implement a static Parse(string) method.", property));
            }
        }


        #endregion

    }
}
