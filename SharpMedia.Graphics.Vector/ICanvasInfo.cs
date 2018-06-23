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
    /// A canvas information interface. It cannot issue rendering commands but contains all
    /// information about the size, pixel density etc.
    /// </summary>
    public interface ICanvasInfo
    {
        /// <summary>
        /// Obtains the size of canvas in pixels.
        /// </summary>
        Vector2i CanvasPixelSize { get; }

        /// <summary>
        /// Obtains the canvas size in canvas units.
        /// </summary>
        Vector2f CanvasUnitSize { get; }

        /// <summary>
        /// Converts a virtual unit position to a pixel position in the 2D projected space.
        /// </summary>
        /// <param name="canvasPosition">The virtual unit position.</param>
        /// <remarks>All transformations are ignored.</remarks>
        /// <returns>The pixel position.</returns>
        Vector2i ToPixelPosition(Vector2f canvasPosition);

        /// <summary>
        /// Converts a pixel position to virtual unit position.
        /// </summary>
        /// <remarks>All transformations are ignored.</remarks>
        /// <param name="pixelPosition">The pixel position</param>
        /// <returns>The canvas position.</returns>
        Vector2f ToCanvasPosition(Vector2i pixelPosition);

        /// <summary>
        /// Converts pixel size to canvas size. It always takes Y axis into account.
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        float ToCanvasSize(float pixelSize);

        /// <summary>
        /// Converts canvas to pixel size. It always takes Y axis into account.
        /// </summary>
        /// <param name="canvasSize"></param>
        /// <returns></returns>
        float ToPixelSize(float canvasSize);

        /// <summary>
        /// Maximum number of pixel units the rendering can go wrong (on curves etc.).
        /// </summary>
        /// <remarks>Typical values are around 0.1 - 2.0.</remarks>
        float PixelErrorTolerance { get; }

        /// <summary>
        /// The tesselation resolution, calculated using pixel error tolerance and canvas dimensions.
        /// </summary>
        /// <remarks>The resolution is provided in canvas coordinates.</remarks>
        float TesselationResolution { get; }
    }

}
