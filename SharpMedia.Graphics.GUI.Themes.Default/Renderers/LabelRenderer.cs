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
using SharpMedia.Graphics.GUI.Styles;
using SharpMedia.Graphics.GUI.Widgets;
using SharpMedia.Math.Shapes;
using SharpMedia.Graphics.Vector.Fonts;
using SharpMedia.Math;
using SharpMedia.Graphics.GUI.Styles.SubStyles;
using SharpMedia.Graphics.GUI.Widgets.Helpers;

namespace SharpMedia.Graphics.GUI.Themes.Default.Renderers
{
    internal class LabelRenderer : AreaRenderer
    {
        #region ISkinRenderer Members


        public override bool Render(ICanvas canvas, IDisplayObject genericDisplayObject)
        {
            base.Render(canvas, genericDisplayObject);
            Label displayObject = genericDisplayObject as Label;

            // We render text using parameters.
            Style xstyle = displayObject.Style;
            
            // We get state styles.
            Label.LabelStyle style, otherStyle;
            float alpha = displayObject.Style.GetStyleState<Label.LabelStyle>(
                displayObject.StyleAnimation, out style, out otherStyle);

            // We first merge data.
            float fontSize, lineSpacing;
            TextOptions textOptions; Pen pen; IFill fill;
            Font font = FontStyle.Merge(style != null ? style.TextFont : null, 
                otherStyle != null ? otherStyle.TextFont : null, alpha,
                out fontSize, out lineSpacing, out textOptions, out pen, out fill);

            IFill selFill;
            Pen selPen;
            PartialFontStyle.Merge(style != null ? style.SelectedTextFont : null, 
                otherStyle != null ? otherStyle.SelectedTextFont : null, alpha, out selFill, out selPen);

            // We calculate bounding box.
            Vector2f leftBottom, rightTop;
            displayObject.GetBoundingBox(out leftBottom, out rightTop);


            // Render only if we can.
            if (font != null)
            {
                // We prepare.
                TextRenderInfo info = font.Prepare(canvas, displayObject.Text, 
                    fontSize, textOptions, lineSpacing, leftBottom, rightTop - leftBottom);

                // We set new text information.
                displayObject.SetTextInfo(info);

                Vector2i selection = displayObject.TextSelectedRange;

                // We first draw background.
                if (selection.Y >= selection.X)
                {
                    Vector2f minSel, maxSel;
                    info.GetBoundingRect(selection, out minSel, out maxSel);

                    // We enlarge for better look.
                    float enlarge = (maxSel.Y - minSel.Y) * (lineSpacing - 1.0f);
                    minSel.Y -= enlarge / 1.5f;
                    maxSel.Y += enlarge / 1.5f;

                    ITransform btransform;
                    IMapper bmapper;
                    IFill bfill = BackgroundStyle.Merge(style != null ? style.SelectionBackground : null,
                        otherStyle != null ? otherStyle.SelectionBackground : null, alpha,
                        out btransform, out bmapper);

                    if (bfill != null)
                    {
                        if (btransform != null)
                        {
                            canvas.TextureTransform = btransform;
                        }

                        canvas.FillShape(bfill, new Rectf(minSel, maxSel), bmapper);

                        if (btransform != null)
                        {
                            canvas.TextureTransform = null;
                        }
                    }
                }

                // We draw other text.
                if (fill != null)
                {
                    info.Render(fill, new Vector2i(0, selection.X - 1));
                    info.Render(fill, new Vector2i(selection.Y + 1, (int)info.GlyphCount - 1));
                }
                else if (pen != null)
                {
                    info.Render(pen, new Vector2i(0, selection.X - 1));
                    info.Render(pen, new Vector2i(selection.Y + 1, (int)info.GlyphCount - 1));
                }

                // We draw selected text.
                if (selFill != null)
                {
                    info.Render(selFill, selection);
                }
                else if (selPen != null)
                {
                    info.Render(selPen, selection);
                }

                displayObject.SetTextInfo(info);
            }

            return true;
        }

        #endregion
    }
}
