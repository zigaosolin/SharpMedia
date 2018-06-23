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

namespace SharpMedia.Graphics.GUI.Metrics
{
    /// <summary>
    /// How is the rectangle defined.
    /// </summary>
    public enum RectangleMode
    {
        MinMax,
        LeftBottomAndSize,
        RightBottomAndSize,
        LeftTopAndSize,
        RightTopAndSize
    }

    /// <summary>
    /// A Gui rectangle holds rectangle.
    /// </summary>
    public sealed class GuiRect
    {
        #region Private Members
        RectangleMode mode;
        GuiVector2 arg1;
        GuiVector2 arg2;
        #endregion

        #region Constructors

        /// <summary>
        /// Default contructor.
        /// </summary>
        public GuiRect()
        {
        }

        /// <summary>
        /// Creates a rectangle.
        /// </summary>
        public GuiRect(RectangleMode mode, GuiVector2 arg1, GuiVector2 arg2)
        {
            this.mode = mode;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        /// <summary>
        /// The most common constructor.
        /// </summary>
        /// <param name="leftBottom"></param>
        /// <param name="rightTop"></param>
        /// <returns></returns>
        public static GuiRect CreateRectangle(GuiVector2 leftBottom, GuiVector2 rightTop)
        {
            return new GuiRect(RectangleMode.MinMax, leftBottom, rightTop);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The rectangle mode associated with rectangle.
        /// </summary>
        public RectangleMode Mode
        {
            get
            {
                return mode;
            }
        }

        /// <summary>
        /// First argument, represents initial point.
        /// </summary>
        public GuiVector2 Argument1
        {
            get
            {
                return arg1;
            }
        }

        /// <summary>
        /// Second argument, represents either second point or dimension.
        /// </summary>
        public GuiVector2 Argument2
        {
            get
            {
                return arg2;
            }
        }

        #endregion

        #region Methods


        public void ToCanvasRect(ICanvas canvas, IPositionable parentElement,
            out Vector2f leftBottom, out Vector2f rightTop)
        {
            if (mode == RectangleMode.MinMax)
            {
                leftBottom = arg1.ToConvasPosition(canvas, parentElement);
                rightTop = arg2.ToConvasPosition(canvas, parentElement);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void ToParentRect(ICanvas canvas, IPositionable parentElement,
            out Vector2f leftBottom, out Vector2f rightTop)
        {
            throw new NotImplementedException();
        }

        public void ToPixelRect(ICanvas canvas, IPositionable parentElement,
            out Vector2f leftBottom, out Vector2f rightTop)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
