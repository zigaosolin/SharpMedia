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
using System.Reflection;

namespace SharpMedia.Graphics.GUI.Compiler.Emit.Code
{
    
    /// <summary>
    /// A delegate class.
    /// </summary>
    internal class ASTDelegate : IEmittable, IResolvable, IXmlParsable
    {
        #region Private Members
        Type delegateType;
        ParameterInfo[] parameters;
        string[] parameterNames;

        // The code of delegate.
        string code;
        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            // We generate arguments.
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < parameters.Length; i++)
            {
                builder.AppendFormat("{0} {1}", parameters[i].ParameterType.FullName, parameterNames[i]);
                if (i != parameters.Length - 1) builder.Append(", ");
            }

            // We now name out arguments.

            // We create anonymouse delegate.
            writer.Write("delegate({0}) { {1} }", builder.ToString(), code);
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get { return new List<IElement>(); }
        }

        #endregion

        #region IResolvable Members

        public void Resolve(CompileContext context, Resolver resolver)
        {
            // We split code and search for ids.

        }

        #endregion

        #region IXmlParsable Members

        public void Parse(CompileContext context, System.Xml.XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
