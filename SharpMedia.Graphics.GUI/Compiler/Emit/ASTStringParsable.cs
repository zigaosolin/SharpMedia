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

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{
    /// <summary>
    /// A generic string parsable object, not built-in.
    /// </summary>
    internal class ASTStringParsable : IEmittable
    {
        #region Private Members
        string data;
        Type type;
        #endregion

        #region Public Members

        public ASTStringParsable(string data, Type type)
        {
            this.data = data;
            this.type = type;
        }

        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            writer.WriteLine("{0}.Parse({1})", type, data);
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get { return new List<IElement>(); }
        }

        #endregion
    }
}
