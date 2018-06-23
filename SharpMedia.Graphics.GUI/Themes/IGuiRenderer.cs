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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.GUI.Themes
{
    /// <summary>
    /// A skin rendering interface, specific for each widget (ButtonRenderer).
    /// </summary>
    public interface IGuiRenderer
    {
        /// <summary>
        /// Renders given a canvas.
        /// </summary>
        /// <remarks>It is renderer's job to ensure not to draw out of bounds of object</remarks>
        bool Render([NotNull] ICanvas canvas, [NotNull] IDisplayObject displayObject);
    }
}
