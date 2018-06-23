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
using System.Threading;
using SharpMedia.Math.Shapes;
using SharpMedia.Math.Shapes.Compounds;
using SharpMedia.Graphics.Vector.Transforms;

namespace SharpMedia.Graphics.Vector
{
    /// <summary>
    /// A null canvas.
    /// </summary>
    public class NullCanvas : ICanvas, ICanvasInfo
    {
        #region Private Members
        object syncRoot = new object();
        Vector2i pixelSize;
        Vector2f unitSize;
        float pixelErrorTolerance = 0.1f;
        Stack<ITransform> positionTransforms = new Stack<ITransform>();
        Stack<ITransform> textureTransforms = new Stack<ITransform>();
        uint locks = 0;
        #endregion

        #region Private Methods

        void AssertLocked()
        {
            if (locks <= 0)
            {
                throw new InvalidOperationException("Canvas is not locked, operation invalid.");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Must specify the pixel density (the actual target dimensions).
        /// </summary>
        /// <param name="pixelDensity"></param>
        public NullCanvas(Vector2i canvasPixelDimension, Vector2f canvasUnitDimension)
        {
            this.pixelSize = canvasPixelDimension;
            this.unitSize= canvasUnitDimension;
        }

        #endregion

        #region ICanvas Members

        public float TesselationResolution
        {
            get
            {
                return 1.0f;
            }
        }

        public Region2f[] ClippingRegions
        {
            get
            {
                return null;
            }
            set
            {
                return;
            }
        }

        public Vector2i CanvasPixelSize
        {
            get { return pixelSize; }
        }

        public Vector2f CanvasUnitSize
        {
            get { return unitSize; }
        }

        public Vector2i ToPixelPosition(Vector3f position)
        {
            // We first convert to [0,1] space.
            Vector2f uniformSpacePosition = Vector2f.ComponentDivision(position.Vec2, unitSize);

            return new Vector2i((int)((float)pixelSize.X * uniformSpacePosition.X + 0.5f),
                                (int)((float)pixelSize.Y * uniformSpacePosition.Y + 0.5f));
        }

        public ITransform Transform
        {
            get
            {
                return positionTransforms.Peek();
            }
            set
            {
                AssertLocked();
                positionTransforms.Pop();
                if (value != null)
                {
                    positionTransforms.Push(value);
                }
                else
                {
                    positionTransforms.Push(new LinearTransform());
                }
            }
        }

        public ITransform MappingTransform
        {
            get
            {
                return textureTransforms.Peek();
            }
            set
            {
                AssertLocked();
                textureTransforms.Pop();
                if (value != null)
                {
                    textureTransforms.Push(value);
                }
                else
                {
                    textureTransforms.Push(new LinearTransform());
                }
            }
        }

        public void Begin(CanvasRenderFlags flags)
        {
            if (locks == 0)
            {
                Monitor.Enter(syncRoot);

                positionTransforms.Push(new LinearTransform());
                textureTransforms.Push(new LinearTransform());
            }
            else
            {
                positionTransforms.Push(positionTransforms.Peek());
                textureTransforms.Push(textureTransforms.Peek());
            }
            locks++;
        }

        public void End()
        {
            locks--;
            if (locks < 0)
            {
                throw new InvalidOperationException("Too many End() calls (not synced with Begin() calls).");
            }

            positionTransforms.Pop();
            textureTransforms.Pop();

            if (locks == 0)
            {
                Monitor.Exit(syncRoot);
            }
        }

        public ICanvasInfo CanvasInfo
        {
            get { return this; }
        }

        public float PixelErrorTolerance
        {
            get
            {
                return pixelErrorTolerance;
            }
            set
            {
                pixelErrorTolerance = value;
            }
        }


        public void FillShape(SharpMedia.Graphics.Vector.Fills.IFill fill, 
            IArea2f shape, SharpMedia.Graphics.Vector.Mappers.IMapper mapper)
        {
        }

        public void FillShape(SharpMedia.Graphics.Vector.Fills.IFill fill,
            SharpMedia.Math.Shapes.Storage.TriangleSoup2f soup, 
            SharpMedia.Graphics.Vector.Mappers.IMapper mapper)
        {
        }

        public void DrawShape(Pen pen, IOutline2f outline, 
            SharpMedia.Graphics.Vector.Mappers.IMapper mapper)
        {
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region ICanvasInfo Members

        public Vector2i ToPixelPosition(Vector2f canvasPosition)
        {
            return Vector2i.Zero;
        }

        public Vector2f ToCanvasPosition(Vector2i pixelPosition)
        {
            return Vector2f.Zero;
        }

        public float ToCanvasSize(float pixelSize)
        {
            return 0.0f;
        }

        public float ToPixelSize(float canvasSize)
        {
            return 0.0f;
        }

        #endregion
    }
}
