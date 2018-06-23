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
using SharpMedia.AspectOriented;
using SharpMedia.Math;
using SharpMedia.Math.Shapes.Algorithms;
using SharpMedia.Graphics.Vector.Fills;

namespace SharpMedia.Graphics.Vector
{

    /// <summary>
    /// The pen information holder.
    /// </summary>
    public class Pen : ICloneable<Pen>
    {
        #region Private Members
        float widthInCanvas = 0.0f;
        float widthInPixels = 0.0f;
        IFill fill = null;
        OutlineEnd lineEnd = OutlineEnd.Square;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a null-fill pen.
        /// </summary>
        public Pen()
        {
        }

        /// <summary>
        /// A pen construction from fill.
        /// </summary>
        /// <param name="fill">The fill.</param>
        public Pen([NotNull] IFill fill)
        {
            this.fill = fill;
        }

        /// <summary>
        /// A pen from fill and width.
        /// </summary>
        /// <param name="fill">The fill.</param>
        /// <param name="width">The width.</param>
        public Pen([NotNull] IFill fill, [MinFloat(0.0f)] float canvasWidth, 
            [MinFloat(0.0f)] float pixelWidth, OutlineEnd endMode)
        {
            this.fill = fill;
            this.widthInCanvas = canvasWidth;
            this.widthInPixels = pixelWidth;
            this.lineEnd = endMode;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates width of border.
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public float CalculateWidth(ICanvasInfo canvas)
        {
            Vector2i ps = canvas.CanvasPixelSize;
            Vector2f t = Vector2f.ComponentDivision(canvas.CanvasUnitSize,  new Vector2f(ps.X, ps.Y));
            return widthInCanvas + widthInPixels * (t.X > t.Y ? t.X : t.Y);
        }

        /// <summary>
        /// Converts pen to algorithm's representation.
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public OutlineTesselation.TesselationOptionsf ToOutlineTesselationOptions(ICanvasInfo canvas)
        {
            OutlineTesselation.TesselationOptionsf t = new OutlineTesselation.TesselationOptionsf();
            t.OutlineType = OutlineType.Line;
            t.OutlineEnd = OutlineEnd.Square;
            t.LineThickness = widthInCanvas + canvas.ToCanvasSize(widthInPixels);

            return t;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Blends pens.
        /// </summary>
        /// <param name="first">The first pen.</param>
        /// <param name="second">The second pen.</param>
        /// <param name="weight">The weight between pends.</param>
        /// <returns></returns>
        public static Pen BlendPens(Pen first, Pen second, float weight)
        {
            if (first == null)
            {
                first = new Pen(null, 0.0f, 0.0f, OutlineEnd.Square);
            }
            if (second == null)
            {
                second = new Pen(null, 0.0f, 0.0f, OutlineEnd.Square);
            }

            float weight1 = 1.0f  - weight;

            // TODO: how to combine line mode, more logic, for now not needed.

            // We now blend them.
            return new Pen(Fills.BlendedFill.BlendFills(first.fill, second.fill, weight), 
                first.WidthInCanvas * weight1 + second.WidthInCanvas * weight,
                first.WidthInPixels * weight1 + second.WidthInPixels * weight,
                first.EndMode);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Pen width in canvas units.
        /// </summary>
        public float WidthInCanvas
        {
            get
            {
                return widthInCanvas;
            }
            [param: MinFloat(0.0f)]
            set
            {
                widthInCanvas = value;
            }
        }

        /// <summary>
        /// Pen width in pixel units.
        /// </summary>
        public float WidthInPixels
        {
            get
            {
                return widthInPixels;
            }
            [param: MinFloat(0.0f)]
            set
            {
                widthInPixels = value;
            }
        }

        /// <summary>
        /// The fill used.
        /// </summary>
        public IFill Fill
        {
            get
            {
                return fill;
            }
            [param: NotNull]
            set
            {
                fill = value;
            }
        }

        /// <summary>
        /// The line ending mode.
        /// </summary>
        public OutlineEnd EndMode
        {
            get
            {
                return lineEnd;
            }
            [param: NotNull]
            set
            {
                lineEnd = value;
            }
        }

        #endregion

        #region ICloneable<Pen> Members

        public Pen Clone()
        {
            Pen p = new Pen(fill);
            p.WidthInPixels = WidthInPixels;
            p.WidthInCanvas = WidthInCanvas;
            p.EndMode = EndMode;
            return p;
        }

        #endregion
    }
}
