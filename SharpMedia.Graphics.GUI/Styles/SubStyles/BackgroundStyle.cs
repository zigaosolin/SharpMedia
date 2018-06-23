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
using SharpMedia.Graphics.Vector.Fills;
using SharpMedia.Graphics.Vector.Transforms;

namespace SharpMedia.Graphics.GUI.Styles.SubStyles
{

    /// <summary>
    /// Background style.
    /// </summary>
    public sealed class BackgroundStyle : TransformableBase, ISubStyle
    {
        #region Private Members
        BackgroundStyle parent;
        IFill fill;

        protected override TransformableBase TransformableParent
        {
            get { return parent; }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// A background style constructor.
        /// </summary>
        public BackgroundStyle()
        {
        }

        /// <summary>
        /// A background style constructor.
        /// </summary>
        /// <param name="fill"></param>
        public BackgroundStyle(IFill fill)
        {
            this.fill = fill;
        }

        /// <summary>
        /// Gets or sets fill. Null fill no drawing.
        /// </summary>
        public IFill Fill
        {
            get
            {
                if (parent != null && fill == null)
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
        /// Sets or gets colour, overrides the fill.
        /// </summary>
        public Colour Colour
        {
            get
            {
                if (fill is SolidFill)
                {
                    return (fill as SolidFill).Colour;
                }

                throw new InvalidOperationException("Background colour defined only for SolidFill.");
            }
            set
            {
                Fill = new SolidFill(value);
            }
        }

        #endregion

        #region ISubDisplayStyle Members

        public string Name
        {
            get { return "Background";  }
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

                if (value != null && !(value is BackgroundStyle)) throw new ArgumentException("Must assign border style parent.");

                Changed();
                if (parent != null)
                {
                    parent.OnChange -= ChangeDelegate;
                }

                parent = value as BackgroundStyle;

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
            BackgroundStyle style = new BackgroundStyle();
            style.Parent = parent;
            style.fill = fill;

            return style;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Merges background styles.
        /// </summary>
        public static IFill Merge(BackgroundStyle style1, BackgroundStyle style2, float weight,
            out ITransform blendTransform, out IMapper mapper)
        {
            if (style1 == null && style2 == null)
            {
                blendTransform = null;
                mapper = null;
                return null;
            }
            if (style1 == null && style2 != null)
            {
                return Merge(style2, style1, 1.0f - weight, out blendTransform, out mapper);
            }

            if (style2 == null)
            {
                mapper = style1.Mapper;
                blendTransform = style1.MappingTransform;
                return style1.Fill;
            }

            blendTransform = BlendTransform.BlendTransforms(style1.MappingTransform, 
                style2.MappingTransform, weight);
            mapper = style2.Mapper;
            return BlendedFill.BlendFills(style1.Fill, style2.Fill, weight);
            
        }

        #endregion
    }
}
