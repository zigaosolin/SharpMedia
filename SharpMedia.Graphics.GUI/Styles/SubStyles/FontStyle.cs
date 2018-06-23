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
    /// A font style.
    /// </summary>
    /// <remarks>Mapping is limited to rotation. No transformation nor scaling is allowed.</remarks>
    public sealed class FontStyle : TransformableBase, ISubStyle
    {
        #region Private Members
        FontStyle parent;

        Font font;
        float? size;
        float? lineSpacing;
        TextOptions? options;
        Pen pen;
        IFill fill;

        protected override TransformableBase TransformableParent
        {
            get { return parent; }
        }

        #endregion

        #region Public Members

        public FontStyle()
        {
        }

        /// <summary>
        /// A constructor for non-outline font.
        /// </summary>
        public FontStyle(Font font, float size, float lineSpacing, TextOptions options, IFill fill)
        {
            this.font = font;
            this.size = size;
            this.lineSpacing = lineSpacing;
            this.options = options;
            this.fill = fill;
        }
        
        /// <summary>
        /// A contructor for outline font.
        /// </summary>
        public FontStyle(Font font, float size, float lineSpacing, TextOptions options, Pen pen)
        {
            this.font = font;
            this.size = size;
            this.lineSpacing = lineSpacing;
            this.options = options;
            this.pen = pen;
        }

        /// <summary>
        /// If font is null, no text is drawn.
        /// </summary>
        public Font Font
        {
            get
            {
                if (parent != null && font == null)
                {
                    return parent.Font;
                }
                return font;
            }
            set
            {
                font = value;
                Changed();
            }
        }


        /// <summary>
        /// Sets the fill used if in normal (not-outline mode).
        /// </summary>
        /// <remarks>Setting pen overrides this choice.</remarks>
        public IFill Fill
        {
            get
            {
                if (fill == null && parent != null)
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

        /// <summary>
        /// The size of font.
        /// </summary>
        public float Size
        {
            get
            {
                if (!size.HasValue && parent != null)
                {
                    return parent.Size;
                }

                return size.GetValueOrDefault(1.0f);
            }
            set
            {
                size = value;
                Changed();
            }
        }

        /// <summary>
        /// Line spacing for multi-line fonts.
        /// </summary>
        public float LineSpacing
        {
            get
            {
                if (!lineSpacing.HasValue && parent != null)
                {
                    return parent.LineSpacing;
                }

                return lineSpacing.GetValueOrDefault(1.3f);
            }
            set
            {
                lineSpacing = value;
                Changed();
            }
        }

        /// <summary>
        /// Text options of font.
        /// </summary>
        public TextOptions TextOptions
        {
            get
            {
                if (!options.HasValue && parent != null)
                {
                    return parent.TextOptions;
                }
                return options.GetValueOrDefault(TextOptions.Left);
            }
            set
            {
                options = value;
                Changed();
            }
        }

        #endregion

        #region ISubDisplayStyle Members

        public string Name
        {
            get { return "Font"; }
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
                if (parent != null && !(parent is FontStyle)) throw new ArgumentException("Must assign font style parent.");
                Changed();

                if (parent != null)
                {
                    parent.OnChange -= ChangeDelegate;
                }

                parent = value as FontStyle;

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
            FontStyle style = new FontStyle();
            style.Parent = parent;
            style.options = options;
            style.size = size;
            style.lineSpacing = lineSpacing;
            style.font = font;
            style.pen = pen;

            return style;
            
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Merges font styles.
        /// </summary>
        /// <returns></returns>
        public static Font Merge(FontStyle style1, FontStyle style2, float weight,
            out float size, out float lineSpacing, out TextOptions options, 
            out Pen pen, out IFill fill)
        {
            // No font.
            if (style1 == null && style2 == null)
            {
                size = 0;
                lineSpacing = 0;
                options = TextOptions.None;
                pen = null;
                fill = null;
                return null;
            }

            // Special case second style only.
            if (style1 == null && style2 != null)
            {
                return Merge(style2, style1, 1.0f - weight, out size, 
                    out lineSpacing, out options, out pen, out fill);
            }

            if (style1 != null && style2 == null)
            {
                size = style1.Size;
                lineSpacing = style1.LineSpacing;
                options = style1.TextOptions;
                pen = style1.Pen;
                fill = style1.Fill;

                return style1.Font;

            }

            float w1 = 1.0f - weight;
            size = style1.Size * w1 + style2.Size * weight;
            lineSpacing = style1.LineSpacing * w1 + style2.LineSpacing * weight;
            options = style1.TextOptions;
            pen = Pen.BlendPens(style1.Pen, style2.Pen, weight);
            fill = BlendedFill.BlendFills(style1.Fill, style2.Fill, weight);

            return style1.Font;
        }

        #endregion

    }
}
