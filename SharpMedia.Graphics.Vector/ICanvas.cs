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
using SharpMedia;
using SharpMedia.Math;
using SharpMedia.Math.Shapes;
using SharpMedia.AspectOriented;
using SharpMedia.Math.Shapes.Compounds;
using SharpMedia.Graphics.Vector.Fills;
using SharpMedia.Graphics.Vector.Mappers;
using SharpMedia.Math.Shapes.Storage;
using SharpMedia.Graphics.Vector.Transforms;

namespace SharpMedia.Graphics.Vector
{
    /// <summary>
    /// Flags when canvas is locked.
    /// </summary>
    [Flags]
    public enum CanvasRenderFlags
    {
        None = 0,

        /// <summary>
        /// Positions are transformed by CPU. 
        /// </summary>
        SoftwarePositionTransform = 1,

        /// <summary>
        /// Mapping coordinates are transformed by CPU.
        /// </summary>
        SoftwareMappingTransform = 2
    }


    /// <summary>
    /// A fictional canvas that can be drawn upon. 
    /// Drawing is in normalized coordinates in range [0,UNIT_X]x[0,UNIT_Y], usually [0,1]x[0,1].
    /// </summary>
    public interface ICanvas : IDisposable
    {
        /// <summary>
        /// Canvas info, bound to this class.
        /// </summary>
        /// <remarks>The canvas info will stay updates to any canvas changes. It is provided
        /// as seperate class so "non-rendering" (preparation) stages are given just the right
        /// amount of information and no control over rendering.</remarks>
        ICanvasInfo CanvasInfo { get; }

        /// <summary>
        /// Maximum number of pixel units the rendering can go wrong (on curves etc.).
        /// </summary>
        /// <remarks>Typical values are around 1.0.</remarks>
        float PixelErrorTolerance { get; set; }

        /// <summary>
        /// The global transform used.
        /// </summary>
        ITransform Transform { get; [param: NotNull] set; }

        /// <summary>
        /// The mapping transform used.
        /// </summary>
        ITransform MappingTransform { get; [param: NotNull] set; }

        /// <summary>
        /// The clipping regions.
        /// </summary>
        Region2f[] ClippingRegions { get; set; }

        /// <summary>
        /// Begins rendering.
        /// </summary>
        /// <remarks>
        /// Calls to begin can be nested as long as there are the same number of following End calls.
        /// </remarks>
        void Begin(CanvasRenderFlags flags);

        /// <summary>
        /// Ends rendering.
        /// </summary>
        void End();

        /// <summary>
        /// Fills a specific shape (interior).
        /// </summary>
        /// <remarks>The shape will be tesselated using the area's tesselate function, given
        /// an adaptive tesselation resolution fitting the pixel error tolerance.</remarks>
        /// <param name="fill">The fill that is used when filling the shape.</param>
        /// <param name="shape">The actual shape.</param>
        /// <param name="mapper">The mapping coordinate generator, if null, PositionMapper is used.</param>
        void FillShape([NotNull] IFill fill, [NotNull] IArea2f shape, IMapper mapper);

        /// <summary>
        /// Fills a specific triangle soup.
        /// </summary>
        /// <remarks>This is available for supporting pre-tesselated shapes, such as fonts, contours etc.</remarks>
        /// <param name="fill">The fill that is used.</param>
        /// <param name="soup">The soup that needs to be drawn.</param>
        /// <param name="mapper">The mapping coordinate generator, if null, PositionMapper is used.</param>
        void FillShape([NotNull] IFill fill, [NotNull] TriangleSoup2f soup, IMapper mapper);

        /// <summary>
        /// Draws a shape, e.g. its outline.
        /// </summary>
        /// <remarks>It is recommended that if you wish to render outlines several times, you can
        /// create triangle soup yourself and than call FillShape. This can be done using OutlineTesselation.Tesselate
        /// method.</remarks>
        /// <param name="pen">The pen to use.</param>
        /// <param name="outline">The outline that is to be drawn.</param>
        /// <param name="mapper">The mapping coordinate generator, if null, PositionMapper is used.</param>
        void DrawShape([NotNull] Pen pen, [NotNull] IOutline2f outline, IMapper mapper);
    }

}
