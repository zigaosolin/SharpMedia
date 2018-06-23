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
using System.Globalization;

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{

    /// <summary>
    /// A GUI scalar.
    /// </summary>
    /// <remarks>Specified as combination of "number[specifier]+", example "20%+2px-3px+0.001c"</remarks>
    internal class ASTScalar : IEmittable, ITokenParsable, IXmlParsable
    {
        #region Private Members
        Metrics.GuiScalar data;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets scalar.
        /// </summary>
        public Metrics.GuiScalar Scalar
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
            return string.Format("new SharpMedia.Graphics.GUI.Metrics.GuiScalar({0}f, {1}f, {2}f)",
                data.PixelCoordinate.ToString(CultureInfo.InvariantCulture),
                data.CanvasCoordinate.ToString(CultureInfo.InvariantCulture),
                data.ParentCoordinate.ToString((CultureInfo.InvariantCulture)));
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
            float pixel = 0.0f, parent = 0.0f, canvas = 0.0f;

            // We now parse until unknown token is found.
            Token token;
            while (true)
            {
                token = stream.Pop();
                bool? sign = true;

                // May be '+' or '-'.
                if (token.TokenId == TokenId.Plus)
                {
                    sign = true;
                    token = stream.Pop();
                }
                else if (token.TokenId == TokenId.Minus)
                {
                    sign = false;
                    token = stream.Pop();
                }
                
                // We parse number.
                if(token.TokenId != TokenId.Number)
                {
                    stream.Push(token);
                    if (sign.HasValue)
                    {
                        throw new ParseException("Number expected after +/-");
                    }
                    break;
                }

                float number = float.Parse(token.Identifier);
                string specifier = "p";

                // We parse specifier
                token = stream.Pop();

                // We also allow '%' elements, id specifier is empty.
                if (token.TokenId == TokenId.Percent)
                {
                    if (token.TokenId == TokenId.Percent)
                    {
                        number *= 0.01f;
                    }

                    token = stream.Pop();
                }
                else if (token.TokenId == TokenId.Identifier)
                {
                    specifier = token.Identifier;

                    token = stream.Pop();
                }

                if (sign.HasValue)
                {
                    number *= sign.Value ? 1.0f : -1.0f;
                }

                // We apply values.
                specifier = specifier.ToLower();
                if (specifier == "p")
                {
                    parent += number;
                }
                else if (specifier == "px")
                {
                    pixel += number;
                }
                else if (specifier == "c")
                {
                    canvas += number;
                }
                else
                {
                    throw new ParseException("Scalar parsing failed: expected '', 'p', 'px' or 'c' specifier");
                }

                stream.Push(token);
                // We now need '+', or we break it.
                if (token.TokenId != TokenId.Plus && token.TokenId != TokenId.Minus)
                {
                    break;
                }

                // We do not pop it, part of stream.
            }

            data = new GuiScalar(pixel, canvas, parent);

        }

        #endregion

        #region IXmlParsable Members

        public void Parse(CompileContext context, System.Xml.XmlNode node)
        {
            if (node.Attributes["Value"] != null)
            {
                ASTScalar scalar = new ASTScalar();
                scalar.Parse(context, Token.StackForm(Token.Tokenize(node.Attributes["Value"].InnerText)));

                data = scalar.Scalar;
            }
            else
            {
                throw new ParseException("Value of scalar not present in 'Value' attribute.");
            }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class ASTScalarTest
    {
        [CorrectnessTest]
        public void Test()
        {
            ASTScalar scalar = new ASTScalar();
            scalar.Parse(null, Token.StackForm(Token.Tokenize("10.0c+5px+2%")));

            Assert.AreEqual(5.0f, scalar.Scalar.PixelCoordinate);
            Assert.AreEqual(10.0f, scalar.Scalar.CanvasCoordinate);
            Assert.AreEqual(0.02f, scalar.Scalar.ParentCoordinate);
        }
    }
#endif
}
