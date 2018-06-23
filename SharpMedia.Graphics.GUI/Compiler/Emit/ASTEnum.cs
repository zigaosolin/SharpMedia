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

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{
    /// <summary>
    /// An enum
    /// </summary>
    internal class ASTEnum : IEmittable
    {
        #region Private Members
        Type enumType;
        string[] split;
        #endregion

        #region Public Members

        /// <summary>
        /// The type is enum.
        /// </summary>
        public ASTEnum(Type enumType, string value)
        {
            this.enumType = enumType;

            split = value.Split('|');

            for (int i = 0; i < split.Length; i++)
            {
                split[i] = split[i].Trim();
                if (enumType.GetField(split[i], BindingFlags.Public | BindingFlags.Static) == null)
                {
                    throw new ParseException(string.Format("Enum does not support this value '{0}'.", split[i].Trim()));
                }
            }
        }

        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            for (int i = 0; i < split.Length; i++)
            {
                writer.Write(string.Format("{0}.{1}", enumType.FullName, split[i]));
                if (i != split.Length - 1)
                {
                    writer.Write(" | ");
                }

            }
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
