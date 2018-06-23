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
using SharpMedia.Graphics.GUI.Widgets;
using SharpMedia.Graphics.GUI.Styles;
using SharpMedia.Math.Shapes;
using SharpMedia.Graphics.Vector.Fills;
using SharpMedia.Graphics.GUI.Styles.SubStyles;

namespace SharpMedia.Graphics.GUI.Themes.Default.Renderers
{

    /// <summary>
    /// An area renderer.
    /// </summary>
    internal class AreaRenderer : IGuiRenderer
    {
        #region Static members

        public static void RenderBorderAndArea(ICanvas canvas, IShapef shape, IPathf border, 
            Area.AreaStyle style, Area.AreaStyle nextStyle, float alpha)
        {

            // We first fill background.
            {
                // We create background fill.
                ITransform mappingTransform;
                IMapper mapper;
                IFill fill = BackgroundStyle.Merge(style != null ? style.Background : null,
                    nextStyle != null ? nextStyle.Background : null, alpha,
                    out mappingTransform, out mapper);

                if (fill != null)
                {
                    if (mappingTransform != null)
                    {
                        canvas.TextureTransform = mappingTransform;
                    }

                    // We fill shape
                    canvas.FillShape(fill, shape, mapper);

                    // Must reset.
                    if (mappingTransform != null)
                    {
                        canvas.TextureTransform = null;
                    }
                }
            }

            // Now we add border over it.
            
            {
                ITransform transform;
                IMapper mapper;
                Pen pen = BorderStyle.Merge(style != null ? style.Border : null, 
                    nextStyle != null ? nextStyle.Border : null, alpha, out transform, out mapper);

                if (pen != null && pen.Fill != null)
                {
                    if(transform != null)
                    {
                        canvas.TextureTransform = transform;
                    }

                    canvas.DrawShape(pen, border, mapper);

                    if(transform != null)
                    {
                        canvas.TextureTransform = null;
                    }
                }
            }
        }

        #endregion

        #region ISkinRenderer Members

        public virtual bool Render(ICanvas canvas, IDisplayObject displayObject)
        {
            // We extract data.
            Area area = displayObject as Area;
            Style styles = displayObject.Style;

            // We determine state.
            Area.AreaStyle style, nextStyle;
            float alpha = displayObject.Style.GetStyleState<Area.AreaStyle>(
                displayObject.StyleAnimation, out style, out nextStyle);
            

            RenderBorderAndArea(canvas, displayObject.Shape, 
                displayObject.Outline, style, nextStyle, alpha);

            return true;

        }

        #endregion
    }
}
