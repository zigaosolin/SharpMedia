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
using SharpMedia.Graphics.Vector.Fonts;
using SharpMedia.Graphics.Vector;
using SharpMedia.Graphics.Vector.Fills;

namespace SharpMedia.Graphics.GUI.Styles.SubStyles
{
    /// <summary>
    /// A partial font style.
    /// </summary>
    /// <remarks>Mapping is limited to rotation. No transformation nor scaling is allowed.</remarks>
    public sealed class PartialFontStyle : TransformableBase, ISubStyle
    {
        #region Private Members
        PartialFontStyle parent;

        Pen pen;
        IFill fill;

        protected override TransformableBase TransformableParent
        {
            get { return parent; }
        }

        #endregion

        #region Public Members

        public PartialFontStyle()
        {
        }

        /// <summary>
        /// A constructor for non-outline font.
        /// </summary>
        public PartialFontStyle(IFill fill)
        {
            this.fill = fill;
        }
        
        /// <summary>
        /// A contructor for outline font.
        /// </summary>
        public PartialFontStyle(Pen pen)
        {
            this.pen = pen;
        }

        /// <summary>
        /// Sets the fill used if in normal (not-outline mode).
        /// </summary>
        /// <remarks>Setting pen overrides this choice.</remarks>
        public IFill Fill
        {
            get
            {
                if(fill == null && parent != null)
                {
                    return parent.Fill;
                }
                return fill;
            }
            set
            {
                Changed();
                fill = value;
            }
        }

        /// <summary>
        /// Sets the pen that is used if in outline mode.
        /// </summary>
        /// <remarks>Setting fill overrides pen.</remarks>
        public Pen Pen
        {
            get
            {
                if (pen == null && parent != null)
                {
                    return parent.Pen;
                }
                return pen;
            }
            set
            {
                Changed();
                pen = value;
            }
        }

        #endregion

        #region ISubDisplayStyle Members

        public string Name
        {
            get { return "PartialFont"; }
        }

        #endregion

        #region ISubDisplayStyle Members

        public ISubStyle Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent == value) return;
                if (parent != null && !(parent is PartialFontStyle)) throw new ArgumentException("Must assign font style parent.");
                Changed();

                if (parent != null)
                {
                    parent.OnChange -= ChangeDelegate;
                }

                parent = value as PartialFontStyle;

                if (parent != null)
                {
                    parent.OnChange += ChangeDelegate;
                }
            }
        }

        #endregion

        #region ICloneable<ISubDisplayStyle> Members

        public ISubStyle Clone()
        {
            PartialFontStyle style = new PartialFontStyle();
            style.Parent = parent;
            style.Pen = pen;
            style.Fill = fill;

            return style;
            
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Merges partial styles.
        /// </summary>
        public static void Merge(PartialFontStyle style1, PartialFontStyle style2, float weight,
            out IFill fill, out Pen pen)
        {
            if (style1 == null && style2 == null)
            {
                fill = null;
                pen = null;
                return;
            }

            if (style2 != null && style1 == null)
            {
                Merge(style2, style1, 1.0f - weight, out fill, out pen);
                return;
            }

            if (style2 == null)
            {
                fill = style1.Fill;
                pen = style1.Pen;
                return;
            }

            fill = BlendedFill.BlendFills(style1.Fill, style2.Fill, weight);
            pen = Pen.BlendPens(style1.Pen, style2.Pen, weight);

            return;
        }
        
        #endregion

    }
}
