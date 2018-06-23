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
using SharpMedia.Math.Shapes;
using SharpMedia.Math;
using SharpMedia.Graphics.Vector;

namespace SharpMedia.Graphics.GUI
{
    /// <summary>
    /// A positionable element.
    /// </summary>
    public interface IPositionable
    {
        /// <summary>
        /// Applies canvas global bounding rectangle (shapes must be adjusted to fit the rectangle).
        /// </summary>
        /// <remarks>This can only be called by layouting engine, or parent elements.</remarks>
        /// <param name="minPosition"></param>
        /// <param name="maxPosition"></param>
        [Linkable(LinkMask.SecurityLevel1)]
        void ApplyBoundingRect(ICanvas canvas, Vector2f minPosition, Vector2f maxPosition);

        /// <summary>
        /// Obtains bounding box.
        /// </summary>
        /// <param name="rightBottom"></param>
        /// <param name="leftTop"></param>
        void GetBoundingBox(out Vector2f leftBottom, out Vector2f rightTop); 

        /// <summary>
        /// Outline of the object (border).
        /// </summary>
        IPathf Outline { get; }

        /// <summary>
        /// The shape, must be boundable (can be transformed to bounding rect).
        /// </summary>
        IShapef Shape { get; }

    }
}
