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
using SharpMedia.Math;

namespace SharpMedia.Graphics.Vector
{
    /// <summary>
    /// The most typical canvas type, rendering using device to render target.
    /// </summary>
    public interface IDeviceCanvas : ICanvas
    {
        /// <summary>
        /// The render target. May not be set during rendering, only outside Begin/End.
        /// </summary>
        RenderTargetView Target { get; set; }

        /// <summary>
        /// Viewport of canvas.
        /// </summary>
        Region2i Viewport { get; set; }

        /// <summary>
        /// Gets the device used to draw.
        /// </summary>
        GraphicsDevice Device { get; }
    }
}
