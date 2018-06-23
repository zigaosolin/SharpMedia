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
using SharpMedia.Math;

namespace SharpMedia.Graphics.GUI.Metrics
{

    /// <summary>
    /// A GUI scalar.
    /// </summary>
    public sealed class GuiScalar
    {
        #region Private Members
        float pixelCoordinate;
        float canvasCoordinate;
        float parentCoordinate;
        #endregion

        #region Public Members

        /// <summary>
        /// Default constructors.
        /// </summary>
        public GuiScalar()
            : this(0.0f)
        {
        }

        public GuiScalar(float pixelCoordinate,
                         float canvasCoordinate,
                         float parentCoordinate)
        {
            this.pixelCoordinate = pixelCoordinate;
            this.canvasCoordinate = canvasCoordinate;
            this.parentCoordinate = parentCoordinate;
        }

        public GuiScalar(float parentCoordinate)
        {
            this.parentCoordinate = parentCoordinate;
            this.canvasCoordinate = this.pixelCoordinate = 0.0f;
        }

        /// <summary>
        /// Pixel coordinate.
        /// </summary>
        public float PixelCoordinate
        {
            get
            {
                return pixelCoordinate;
            }
        }

        /// <summary>
        /// Canvas coordinate.
        /// </summary>
        public float CanvasCoordinate
        {
            get
            {
                return canvasCoordinate;
            }
        }

        /// <summary>
        /// Parent coonrdinates.
        /// </summary>
        public float ParentCoordinate
        {
            get
            {
                return parentCoordinate;
            }
        }


        public float ToParentPosition(ICanvas canvas, IPositionable parentElement)
        {
            throw new ArgumentException();   
        }

        public float ToCanvasPosition(ICanvas canvas, IPositionable parentElement)
        {
            throw new ArgumentException();   
        }

        public float ToPixelPosition(ICanvas canvas, IPositionable parentElement)
        {
            throw new ArgumentException();
        }

        public float ToParentSize(ICanvas canvas, IPositionable parentElement)
        {
            throw new ArgumentException();
        }

        public float ToCanvasSize(ICanvas canvas, IPositionable parentElement)
        {
            // FIXME: for now as x

            Vector2f leftBottom, rightTop;
            parentElement.GetBoundingBox(out leftBottom, out rightTop);

            Vector2f pixel = GuiVector2.PixelToCanvas(canvas, 
                new Vector2f(this.pixelCoordinate, this.pixelCoordinate));

            // We now construct canvas position.
            return this.parentCoordinate * (rightTop - leftBottom).X +
                this.canvasCoordinate + pixel.X;
        }

        public float ToPixelSize(ICanvas canvas, IPositionable parentElement)
        {
            throw new ArgumentException();
        }

        #endregion
    }
}
