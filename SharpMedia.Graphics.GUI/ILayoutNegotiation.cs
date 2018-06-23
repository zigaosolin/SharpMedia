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
using SharpMedia.Graphics.Vector;

namespace SharpMedia.Graphics.GUI
{
    /// <summary>
    /// Layout anchor.
    /// </summary>
    [Flags]
    public enum LayoutAnchor : int
    {
        None = 0,
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8
    }

    /// <summary>
    /// Implemented by components that negotiate layouts with the GUI manager.
    /// </summary>
    public interface ILayoutNegotiation : IPositionable
    {
        /// <summary>
        /// The type of anchor.
        /// </summary>
        LayoutAnchor LayoutAnchor { get; }


        /// <summary>
        /// Obtains preferred rectangle.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="parent"></param>
        /// <param name="leftBottom"></param>
        /// <param name="rightTop"></param>
        /// <remarks>Returns false if no such rectangle exits.</remarks>
        bool GetPreferredRect(ICanvas canvas, IPositionable parent,
            out Vector2f leftBottom, out Vector2f rightTop);


        /// <summary>
        /// Obtains margin as vector coordiantes in that order:
        /// Left, Right, Top, Bottom.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        Vector4f GetMargin(ICanvas canvas, IPositionable parent);

        /// <summary>
        /// Returns preferred size.
        /// </summary>
        Vector2f GetPreferredSize(ICanvas canvas, IPositionable parent);

        /// <summary>
        /// Returns minimum size.
        /// </summary>
        /// <param name="parentSize"></param>
        /// <returns></returns>
        Vector2f GetRequiredMinimumSize(ICanvas canvas, IPositionable parent);

        /// <summary>
        /// Returns maximum size.
        /// </summary>
        /// <param name="parentSize"></param>
        /// <returns></returns>
        Vector2f GetRequiredMaximumSize(ICanvas canvas, IPositionable parent);

        /// <summary>
        /// Is width/height ration preserved.
        /// </summary>
        bool PreserveRatio { get; }
    }
}