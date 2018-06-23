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
using SharpMedia.Math;
using SharpMedia.Math.Shapes;
using SharpMedia.Graphics.GUI.Widgets;
using SharpMedia.Graphics.GUI.Standalone;

namespace SharpMedia.Graphics.GUI.Themes.Default.Renderers
{

    /// <summary>
    /// A pointer renderer.
    /// </summary>
    internal class PointerRenderer : IGuiRenderer
    {
        #region ISkinRenderer Members

        public bool Render(ICanvas canvas, IDisplayObject displayObject)
        {
            IPointer pointer = displayObject as IPointer;
            Vector3f position = pointer.CanvasPosition;

            // A pointer style object.
            GuiPointer.GuiPointerStyle style = 
                displayObject.Style.GetStyle<GuiPointer.GuiPointerStyle>(CommonStyleStates.Normal);
            if (style == null) return false;

            // For now create dot of size in pixels.
            Metrics.GuiVector2 vector = new SharpMedia.Graphics.GUI.Metrics.GuiVector2(new Vector2f(4, 4),
                Vector2f.Zero, Vector2f.Zero);

            Vector2f v = vector.ToConvasSize(canvas, null);
            Rectf shape = new Rectf(position - v, position + new Vector2f(v.X, -v.Y), 
                position + v);

            // We now render pointer
            AreaRenderer.RenderBorderAndArea(canvas, shape, shape, style, null, 0.0f);

            return true;
            
            
        }

        #endregion
    }
}
