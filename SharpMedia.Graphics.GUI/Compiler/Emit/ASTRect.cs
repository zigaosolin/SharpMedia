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
using SharpMedia.Graphics.GUI.Metrics;
using SharpMedia.Math;
using SharpMedia.Scripting.CompilerFacilities;
using SharpMedia.Testing;
using System.Xml;

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{

    /// <summary>
    /// A AST rectanble.
    /// </summary>
    internal sealed class ASTRect : ITokenParsable, IEmittable, IXmlParsable
    {
        #region Private Members
        GuiRect rectangle;
        #endregion

        #region ITokenParsable Members

        public void Parse(CompileContext context, Stack<Token> stream)
        {
            // We have following parsing:
            // [Type:]GuiVector2, GuiVector2
            // where Type = MinMax | LeftBottomAndSize | RightBottomAndSize | LeftTopAndSize | RightTopAndSize
            
            // We first check for type.
            Token token = stream.Pop();
            RectangleMode mode = RectangleMode.MinMax;
            if (token.TokenId == TokenId.Identifier)
            {
                string id = token.Identifier;
                switch (id.ToLower())
                {
                case "minmax":
                    mode = RectangleMode.MinMax;
                    break;
                case "leftbottomandsize":
                    mode = RectangleMode.LeftBottomAndSize;
                    break;
                case "rightbottomandsize":
                    mode = RectangleMode.RightBottomAndSize;
                    break;
                case "lefttopandsize":
                    mode = RectangleMode.LeftTopAndSize;
                    break;
                case "righttopandsize":
                    mode = RectangleMode.RightTopAndSize;
                    break;
                default:
                    stream.Push(token);
                    throw new ParseException("Expected a valid type identifier for rectangle.");
                }

                token = stream.Pop();

                if(token.TokenId != TokenId.Colon)
                {
                    stream.Push(token);
                    throw new ParseException("If specifier for rectangle is used, it must be followed by ':'.");
                }

                token = stream.Pop();
            }

            stream.Push(token);

            ASTVector2 v1 = new ASTVector2(), v2 = new ASTVector2();
            v1.Parse(context, stream);

            token = stream.Pop();
            if (token.TokenId != TokenId.Comma)
            {
                stream.Push(token);
                throw new ParseException("Comma expected to seperate vectors in rectangle.");
            }

            v2.Parse(context, stream);

        
            // We now apply data.
            rectangle = new GuiRect(mode, v1.Vector, v2.Vector);
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get
            {
                return new List<IElement>();
            }
        }

        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            writer.Write(ToString());
        }

        public override string ToString()
        {
            ASTVector2 v1 = new ASTVector2(), v2 = new ASTVector2();
            v1.Vector = rectangle.Argument1;
            v2.Vector = rectangle.Argument2;
            return string.Format("new SharpMedia.Graphics.GUI.Metrics.GuiRect(SharpMedia.Graphics.GUI.Metrics.RectangleMode.{0}, {1}, {2})",
                rectangle.Mode.ToString(), v1, v2);
        }

        #endregion

        #region IXmlParsable Members

        public void Parse(CompileContext context, System.Xml.XmlNode node)
        {
            XmlAttributeCollection attr = node.Attributes;

            ASTScalar left = null, right = null, top = null, bottom = null, width = null, height = null;
            if (attr["Left"] != null)
            {
                left = new ASTScalar();
                left.Parse(context, Token.StackForm(Token.Tokenize(attr["Left"].InnerText)));
            }

            if (attr["Right"] != null)
            {
                right = new ASTScalar();
                right.Parse(context, Token.StackForm(Token.Tokenize(attr["Right"].InnerText)));
            }

            if (attr["Top"] != null)
            {
                top = new ASTScalar();
                top.Parse(context, Token.StackForm(Token.Tokenize(attr["Top"].InnerText)));
            }

            if (attr["Bottom"] != null)
            {
                bottom = new ASTScalar();
                bottom.Parse(context, Token.StackForm(Token.Tokenize(attr["Bottom"].InnerText)));
            }

            if (attr["Width"] != null)
            {
                width = new ASTScalar();
                width.Parse(context, Token.StackForm(Token.Tokenize(attr["Width"].InnerText)));
            }

            if (attr["Height"] != null)
            {
                height = new ASTScalar();
                height.Parse(context, Token.StackForm(Token.Tokenize(attr["Height"].InnerText)));
            }

            // We now try combinations, must be defined somehow. We check four corners.
            if (left != null && right != null && top != null && bottom != null && height ==  null && width == null)
            {
                this.rectangle = new GuiRect(RectangleMode.MinMax, new GuiVector2(left.Scalar, bottom.Scalar),
                    new GuiVector2(right.Scalar, top.Scalar));
                return;
            }

            // We default width/height.
            if (width == null)
            {
                width = new ASTScalar();
                width.Scalar = new GuiScalar(0.0f);
            }
            if (height == null)
            {
                height = new ASTScalar();
                height.Scalar = new GuiScalar(0.0f);
            }

            // Now we check combinations.
            if (left != null && right == null && top == null && bottom != null)
            {
                this.rectangle = new GuiRect(RectangleMode.LeftBottomAndSize,
                    new GuiVector2(left.Scalar, bottom.Scalar), new GuiVector2(width.Scalar, height.Scalar));
                return;
            }

            if (left != null && right == null && top != null && bottom == null)
            {
                this.rectangle = new GuiRect(RectangleMode.LeftTopAndSize,
                    new GuiVector2(left.Scalar, top.Scalar), new GuiVector2(width.Scalar, height.Scalar));
                return;
            }

            if (left == null && right != null && top != null && bottom == null)
            {
                this.rectangle = new GuiRect(RectangleMode.RightTopAndSize,
                    new GuiVector2(right.Scalar, top.Scalar), new GuiVector2(width.Scalar, height.Scalar));
                return;
            }

            if (left == null && right != null && top == null && bottom != null)
            {
                this.rectangle = new GuiRect(RectangleMode.RightBottomAndSize,
                    new GuiVector2(right.Scalar, bottom.Scalar), new GuiVector2(width.Scalar, height.Scalar));
                return;
            }

            throw new ParseException("The rectangle is malformed.");
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class ASTRectTest
    {
        [CorrectnessTest]
        public void Test()
        {
            ASTRect rect = new ASTRect();
            rect.Parse(null, Token.StackForm(Token.Tokenize("MinMax: (10px+10%, -10%-5px), (0%,-5p)")));


        }
    }
#endif
}
