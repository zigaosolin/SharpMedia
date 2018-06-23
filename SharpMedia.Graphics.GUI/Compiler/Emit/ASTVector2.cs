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
using SharpMedia.Scripting.CompilerFacilities;
using SharpMedia.Testing;

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{

    /// <summary>
    /// A GUI vector.
    /// </summary>
    internal class ASTVector2 : IEmittable, ITokenParsable, IXmlParsable
    {
        #region Private Members
        GuiVector2 data;
        #endregion

        #region Properties

        /// <summary>
        /// The vector itself.
        /// </summary>
        public GuiVector2 Vector
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
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
            ASTScalar s1 = new ASTScalar(), s2 = new ASTScalar();
            s1.Scalar = data.X;
            s2.Scalar = data.Y;

            return string.Format("new SharpMedia.Graphics.GUI.Metrics.GuiVector2({0}, {1})", s1, s2);
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

        #region ITokenParsable Members

        public void Parse(CompileContext context, Stack<Token> stream)
        {
            Token token = stream.Pop();
            if (token.TokenId != TokenId.LeftBracket)
            {
                stream.Push(token);
                throw new ParseException("Expected '(' to begin vector.");
            }

            // We parse first scalar.
            ASTScalar scalar1 = new ASTScalar(), scalar2 = new ASTScalar();
            scalar1.Parse(context, stream);

            // We expect comma.
            token = stream.Pop();
            if (token.TokenId != TokenId.Comma)
            {
                stream.Push(token);
                throw new ParseException("Expected ',' seperating two scalars.");
            }

            // We parse the second.
            scalar2.Parse(context, stream);


            // We need right bracket.
            token = stream.Pop();
            if (token.TokenId != TokenId.RightBracket)
            {
                stream.Push(token);
                throw new ParseException("Expecting ')' to end vector.");
            }

            // We apply them.
            data = new GuiVector2(scalar1.Scalar, scalar2.Scalar);
        }

        #endregion

        #region IXmlParsable Members

        public void Parse(CompileContext context, System.Xml.XmlNode node)
        {
            ASTScalar width = null, height = null;

            if (node.Attributes["Width"] != null)
            {
                width = new ASTScalar();
                width.Parse(context, Token.StackForm(Token.Tokenize(node.Attributes["Width"].InnerText)));
            }

            if (node.Attributes["X"] != null)
            {
                if (width != null)
                {
                    throw new ParseException("Could not combine X and Width, same meaning in terms of vector.");
                }
                width = new ASTScalar();
                width.Parse(context, Token.StackForm(Token.Tokenize(node.Attributes["X"].InnerText)));
            }

            if (node.Attributes["Height"] != null)
            {
                height = new ASTScalar();
                height.Parse(context, Token.StackForm(Token.Tokenize(node.Attributes["Height"].InnerText)));
            }

            if (node.Attributes["Y"] != null)
            {
                if (height != null)
                {
                    throw new ParseException("Could not combine Y and Height, same meaning in terms of vector.");
                }
                height = new ASTScalar();
                height.Parse(context, Token.StackForm(Token.Tokenize(node.Attributes["Y"].InnerText)));
            }

            // Default to zero
            if (width == null) { width = new ASTScalar(); width.Scalar = new GuiScalar(0.0f); }
            if (height == null) { height = new ASTScalar(); height.Scalar = new GuiScalar(0.0f); }

            data = new GuiVector2(width.Scalar, height.Scalar);
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class ASTVector2Test
    {
        [CorrectnessTest]
        public void Test()
        {
            ASTVector2 vector = new ASTVector2();
            vector.Parse(null, Token.StackForm(Token.Tokenize("(10px+10%, -10%-5px)")));


        }
    }
#endif
}
