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
using SharpMedia.Math.Shapes;

namespace SharpMedia.Graphics.GUI.Metrics
{
    /// <summary>
    /// A GUI vector allows combination of coordinates.
    /// </summary>
    public sealed class GuiVector2
    {
        #region Private Members
        Vector2f pixelCoordinates;
        Vector2f canvasCoordinates;
        Vector2f parentCoordinates;
        #endregion

        #region Public Members

        /// <summary>
        /// Default constructor
        /// </summary>
        public GuiVector2()
        {
            this.pixelCoordinates = Vector2f.Zero;
            this.canvasCoordinates = Vector2f.Zero;
            this.parentCoordinates = Vector2f.Zero;
        }

        /// <summary>
        /// Constructs a vector from three vector of different spaces.
        /// </summary>
        /// <param name="pixelCoordinates"></param>
        /// <param name="canvasCoordinates"></param>
        /// <param name="parentCoordinates"></param>
        public GuiVector2(Vector2f pixelCoordinates,
                           Vector2f canvasCoordinates,
                           Vector2f parentCoordinates)
        {
            this.pixelCoordinates = pixelCoordinates;
            this.canvasCoordinates = canvasCoordinates;
            this.parentCoordinates = parentCoordinates;
        }

        /// <summary>
        /// Constructs vector of two scalars.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public GuiVector2(GuiScalar x, GuiScalar y)
        {
            this.pixelCoordinates = new Vector2f(x.PixelCoordinate, y.PixelCoordinate);
            this.canvasCoordinates = new Vector2f(x.CanvasCoordinate, y.CanvasCoordinate);
            this.parentCoordinates = new Vector2f(x.ParentCoordinate, y.ParentCoordinate);
        }

        /// <summary>
        /// Constructs vector from most common parent coordinates only.
        /// </summary>
        /// <param name="parentCoordinates"></param>
        public GuiVector2(Vector2f parentCoordinates)
        {
            this.parentCoordinates = parentCoordinates;
            this.canvasCoordinates = this.pixelCoordinates = Vector2f.Zero;
        }

        /// <summary>
        /// Converts pixel coordinates to canvas coordinates.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public static Vector2f PixelToCanvas(ICanvas canvas, Vector2f pixel)
        {
            Vector2i ps = canvas.CanvasPixelSize;
            return Vector2f.ComponentDivision(
                Vector2f.ComponentMultiply(pixel, canvas.CanvasUnitSize),
                 new Vector2f(ps.X, ps.Y));
        }

        /// <summary>
        /// Pixel coordinates.
        /// </summary>
        public Vector2f PixelCoordinates
        {
            get
            {
                return pixelCoordinates;
            }
        }

        /// <summary>
        /// Canvas coordinates.
        /// </summary>
        public Vector2f CanvasCoordinates
        {
            get
            {
                return canvasCoordinates;
            }
        }

        /// <summary>
        /// Parent coonrdinates.
        /// </summary>
        public Vector2f ParentCoordinates
        {
            get
            {
                return parentCoordinates;
            }
        }

        /// <summary>
        /// The Gui scalar.
        /// </summary>
        public GuiScalar X
        {
            get
            {
                return new GuiScalar(pixelCoordinates.X, canvasCoordinates.X, parentCoordinates.X);
            }
        }

        /// <summary>
        /// The Gui scalar.
        /// </summary>
        public GuiScalar Y
        {
            get
            {
                return new GuiScalar(pixelCoordinates.Y, canvasCoordinates.Y, parentCoordinates.Y);
            }
        }


        public Vector2f ToParentPosition(ICanvas canvas, IPositionable parentElement)
        {
            throw new ArgumentException();   
        }

        public Vector2f ToConvasPosition(ICanvas canvas, IPositionable parentElement)
        {
            Vector2f leftBottom, rightTop;
            parentElement.GetBoundingBox(out leftBottom, out rightTop);

            // We now construct canvas position.
            return leftBottom + Vector2f.ComponentMultiply(parentCoordinates, (rightTop - leftBottom)) +
                canvasCoordinates + PixelToCanvas(canvas, pixelCoordinates);
        }

        public Vector2f ToPixelPosition(ICanvas canvas, IPositionable parentElement)
        {
            throw new ArgumentException();
        }

        public Vector2f ToParentSize(ICanvas canvas, IPositionable parentElement)
        {
            throw new ArgumentException();
        }

        public Vector2f ToConvasSize(ICanvas canvas, IPositionable parentElement)
        {
            if (parentElement == null)
            {
                // We now construct canvas position.
                return canvasCoordinates + PixelToCanvas(canvas, pixelCoordinates);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public Vector2f ToPixelSize(ICanvas canvas, IPositionable parentElement)
        {
            throw new ArgumentException();
        }

        #endregion
    }
}
