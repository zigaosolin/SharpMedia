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
using SharpMedia.Scripting.CompilerFacilities;
using System.Reflection;
using System.Globalization;

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{
    internal class ASTColour : ITokenParsable, IEmittable
    {
        #region Private Members
        Colour colour = Colour.Black;
        string value = null;
        #endregion

        #region ITokenParsable Members

        public void Parse(CompileContext context, Stack<Token> stream)
        {
            Token token = stream.Pop();

            if (token.TokenId == TokenId.Number)
            {
                uint value = uint.Parse(token.Identifier);

                colour = Colour.FromRGBA(value);
            }
            else if (token.TokenId == TokenId.Identifier)
            {
                
                PropertyInfo info = typeof(Colour).GetProperty(token.Identifier, BindingFlags.Public|BindingFlags.Static|BindingFlags.IgnoreCase);
                if(info != null)
                {
                    value = info.Name;
                }
                else
                {
                    throw new ParseException(string.Format("'{0}' not recongised as a valid colour.", token.Identifier));
                }
            }
            else
            {
                throw new ParseException("Colour in wrong format: string and RRGGBBAA accepted.");
            }
            
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get { return new List<IElement>(); }
        }

        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            if (value != null)
            {
                writer.Write("SharpMedia.Graphics.Colour." + value);
            }
            else
            {
                writer.Write("new SharpMedia.Graphics.Colour({0}, {1}, {2}, {3})", colour.R.ToString(CultureInfo.InvariantCulture.NumberFormat),
                    colour.G.ToString(CultureInfo.InvariantCulture.NumberFormat), colour.B.ToString(CultureInfo.InvariantCulture.NumberFormat),
                    colour.A.ToString(CultureInfo.InvariantCulture.NumberFormat));
            }
        }

        #endregion
    }
}
