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

namespace SharpMedia.Graphics.GUI.Widgets.Containers
{

    /// <summary>
    /// The scroller mode.
    /// </summary>
    [Flags]
    public enum ScrollerMode
    {
        None = 0,

        // Mode types.
        AllowMouseWheel = 1,
        AllowKeyUpDown = 2,
        AllowKeyLeftRight = 4,
        AllowHMouseAtBorder = 8,
        AllowVMouseAtBorder = 16,
        AllowHMouseAtBorderClick = 32,
        AllowVMouseAtBorderClick = 64,

    }

    /// <summary>
    /// A scrollable container.
    /// </summary>
    public interface IScrollable : IContainer
    {
        /// <summary>
        /// Vertical scrolling bounds.
        /// </summary>
        Vector2f VScrollBounds { get; }

        /// <summary>
        /// Horizontal scrolling bounds.
        /// </summary>
        Vector2f HScrollBounds { get; }

        /// <summary>
        /// Obtains object's bounds.
        /// </summary>
        void GetObjectBounds(object child, out Vector2f vbounds, out Vector2f hbounds);

        /// <summary>
        /// The scroller mode.
        /// </summary>
        ScrollerMode ScrollerMode { get; set; }

        /// <summary>
        /// Vertical scroller position at the top of the screen (bounds of .X).
        /// </summary>
        float VScrollPosition { get; set; }

        /// <summary>
        /// Horizontal scroller position at the top of the screen (bounds of .X).
        /// </summary>
        float HScrollPosition { get; set; }

        /// <summary>
        /// Makes the children visible in the priority order they are passed.
        /// </summary>
        /// <param name="children"></param>
        /// <returns>Number of children visible after this call.</returns>
        uint MakeVisible(object[] children, bool allowRearange);

    }
}
