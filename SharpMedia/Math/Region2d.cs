// This file was generated by TemplateEngine from template source 'Region2'
// using template 'Region2d. Do not modify this file directly, modify it from template source.

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Math
{
    /// <summary>
    /// A 2D region.
    /// </summary>
    [Serializable]
    public struct Region2d
    {
        #region Public Members
        /// <summary>
        /// Left bottom coordinate of region.
        /// </summary>
        public Vector2d LeftBottom;

        /// <summary>
        /// Right top coordinate of region.
        /// </summary>
        public Vector2d RightTop;
        #endregion

        #region Properties

        /// <summary>
        /// Left top coordinate of region.
        /// </summary>
        public Vector2d LeftTop
        {
            get
            {
                return new Vector2d(LeftBottom.X, RightTop.Y);
            }
            set
            {
                LeftBottom.X = value.X;
                RightTop.Y = value.Y;
            }
        }

        /// <summary>
        /// Left top coordinate of region.
        /// </summary>
        public Vector2d RightBottom
        {
            get
            {
                return new Vector2d(RightTop.X, LeftBottom.Y);
            }
            set
            {
                RightTop.X = value.X;
                LeftBottom.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        public double X
        {
            get
            {
                return LeftBottom.X;
            }
            set
            {
                LeftBottom.X = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        public double Y
        {
            get
            {
                return LeftBottom.Y;
            }
            set
            {
                LeftBottom.Y = value;
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public double Width
        {
            get
            {
                return RightTop.X - LeftBottom.X;
            }
            set
            {
                RightTop.X = LeftBottom.X + value;
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public double Height
        {
            get
            {
                return RightTop.Y - LeftBottom.Y;
            }
            set
            {
                RightTop.Y = LeftBottom.X + value;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get
            {
                return Width >= 0 && Height >= 0;
            }
        }

        /// <summary>
        /// Gets the dimensions of region.
        /// </summary>
        public Vector2d Dimensions
        {
            get 
            { 
                return RightTop - LeftBottom;
            }
            set 
            { 
                RightTop = LeftBottom + value;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// A null region.
        /// </summary>
        public static Region2d Null
        {
            get
            {
                return new Region2d(0.0, 0.0, 0.0, 0.0);
            }
        }

        /// <summary>
        /// Obtains a min-max range.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Region2d MinMax(params Vector2d[] points)
        {
            if (points.Length == 0) return Null;

            Vector2d min = points[0];
            Vector2d max = points[0];

            for (int i = 1; i < points.Length; i++)
            {
                Vector2d v = points[i];

                min.X = min.X < v.X ? min.X : v.X;
                min.Y = min.Y < v.Y ? min.Y : v.Y;

                max.X = max.X > v.X ? max.X : v.X;
                max.Y = max.Y > v.Y ? max.Y : v.Y;
            }

            return new Region2d(min, max);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Region2i"/> class.
        /// </summary>
        /// <param name="leftBottom">The left botttom coordinate.</param>
        /// <param name="rightTop">The right top coordiante.</param>
        public Region2d(Vector2d leftBottom, Vector2d rightTop)
        {
            this.LeftBottom = leftBottom;
            this.RightTop = rightTop;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Region2i"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Region2d(double x, double y, double width, double height)
        {
            this.LeftBottom = new Vector2d(x, y);
            this.RightTop = new Vector2d(width, height);
        }

        #endregion
    }
}
