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
using SharpMedia.Graphics.Vector;

namespace SharpMedia.Graphics.GUI.Styles.SubStyles
{
    /// <summary>
    /// A border sub-style.
    /// </summary>
    /// <remarks>Border is usually drawn at element's outline.</remarks>
    [Serializable]
    public sealed class BorderStyle : TransformableBase, ISubStyle
    {
        #region Private Members
        BorderStyle parent;
        Pen pen;

        protected override TransformableBase TransformableParent
        {
            get { return parent; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs border.
        /// </summary>
        public BorderStyle()
        {
        }

        /// <summary>
        /// Constructs border with pen.
        /// </summary>
        /// <param name="pen"></param>
        public BorderStyle(Pen pen)
        {
            this.pen = pen;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a pen.
        /// </summary>
        /// <remarks>If pen is null, no border is drawn.</remarks>
        public Pen Pen
        {
            get
            {
                if (parent != null && pen == null)
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
            get { return "Border"; }
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
                if (parent != null && !(parent is BorderStyle)) throw new ArgumentException("Must assign border style parent.");
                Changed();

                if (parent != null)
                {
                    parent.OnChange -= ChangeDelegate;
                }

                parent = value as BorderStyle;

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
            BorderStyle s = new BorderStyle(pen);
            s.Parent = parent;
            return s;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Blends styles.
        /// </summary>
        public static Pen Merge(BorderStyle style1, BorderStyle style2, float weight,
            out ITransform transform, out IMapper mapper)
        {
            if (style1 == null && style2 == null)
            {
                transform = null;
                mapper = null;
                return null;
            }
            if (style2 != null && style1 == null)
            {
                return Merge(style2, style1, 1.0f - weight, out transform, out mapper);
            }

            if (style2 == null)
            {
                transform = style1.MappingTransform;
                mapper = style1.Mapper;
                return style1.Pen;
            }

            transform = BlendTransform.BlendTransforms(style1.MappingTransform, 
                style2.MappingTransform, weight);
            mapper = style1.Mapper;

            return Pen.BlendPens(style1.Pen, style2.Pen, weight);
            
        }

        #endregion
    }
}
