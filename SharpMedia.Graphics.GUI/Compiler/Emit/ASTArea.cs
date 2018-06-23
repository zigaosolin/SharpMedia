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
    /// An area node.
    /// </summary>
    public class ASTArea : IEmittable, IResolvable, IXmlParsable
    {
        #region Private Members
        protected string name;
        protected Styles.ASTStyleLink styleLink;
        protected Styles.ASTInlineStyle inlineStyle;

        // Special theme, if available.
        protected ASTThemeLink themeLink;

        // Layouting options.
        protected ASTRect preferredRect;
        protected ASTSize preferredSize;
        protected ASTSize minSize;
        protected ASTSize maxSize;
        protected ASTScalar leftMargin;
        protected ASTScalar rightMargin;
        protected ASTScalar topMargin;
        protected ASTScalar bottomMargin; 
        #endregion

        #region Protected Methods

        // We parse internal data
        protected void ParseAttribute(System.Xml.XmlAttribute attribute)
        {

        }

        #endregion


        #region IEmittable Members

        public virtual void Emit(System.IO.TextWriter writer)
        {
            
        }

        #endregion

        #region IElement Members

        public virtual List<IElement> Children
        {
            get
            {
                return new List<IElement>();
            }
            set
            {
                if (value.Count != 0)
                {
                    throw new ParseException("Area cannot have children.");
                }
            }
        }

        #endregion

        #region IResolvable Members

        public virtual void Resolve(Resolver resolver)
        {
            // FIXME:
        }

        #endregion

        #region IXmlParsable Members

        public virtual void Parse(System.Xml.XmlNode node)
        {
            // We check attributes.
            foreach (System.Xml.XmlAttribute attribute in node.Attributes)
            {
                ParseAttribute(attribute);
            }

            // We do not allow children.
            if (node.ChildNodes.Count != 0)
            {
                throw new ParseException("Area cannot have children.");
            }
            
        }

        #endregion
    }
}
