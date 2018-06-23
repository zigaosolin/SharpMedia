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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Vector
{
    /// <summary>
    /// Implementors of this interface are able to be rendered on a Vector Graphics ICanvas.
    /// </summary>
    public interface ICanvasRenderable
    {
        /// <summary>
        /// Renders to the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas to render to.</param>
        void Render([NotNull] ICanvas canvas);
    }
}
