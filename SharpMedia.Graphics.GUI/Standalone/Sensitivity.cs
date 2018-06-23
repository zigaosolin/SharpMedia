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
using SharpMedia.Graphics.GUI.Metrics;
using SharpMedia.Math;
using SharpMedia.Graphics.Vector;

namespace SharpMedia.Graphics.GUI.Standalone
{
    /// <summary>
    /// The sensitivity.
    /// </summary>
    public sealed class Sensitivity 
    {
        #region Private Members
        GuiScalar wheelSensitivity = new GuiScalar(1.0f, 0, 0);
        GuiVector2 mouseSensitivity = new GuiVector2(new Vector2f(1.0f, -1.0f), Vector2f.Zero, Vector2f.Zero);
        #endregion

        #region Properties

        /// <summary>
        /// Gets wheel sensitivity.
        /// </summary>
        public GuiScalar WheelSensitivity
        {
            get
            {
                return wheelSensitivity;
            }
        }

        /// <summary>
        /// Gets mouse sensitivity.
        /// </summary>
        public GuiVector2 MouseSensitivity
        {
            get
            {
                return mouseSensitivity;
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Pointer sensitivity constructor.
        /// </summary>
        /// <param name="mouseSensitivity"></param>
        /// <param name="wheelSensitivity"></param>
        /// <param name="repeatKeyPressFirstTime"></param>
        /// <param name="repeatKeyPress"></param>
        public Sensitivity(GuiVector2 mouseSensitivity, GuiScalar wheelSensitivity)
        {
            this.mouseSensitivity = mouseSensitivity;
            this.wheelSensitivity = wheelSensitivity;
        }

        /// <summary>
        /// Default sensitivity, based on pixels.
        /// </summary>
        public Sensitivity()
        {
        }


        /// <summary>
        /// Calculates wheel sensitivity.
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public float CalcWheelSensitivity(ICanvas canvas)
        {
            return wheelSensitivity.CanvasCoordinate +
                wheelSensitivity.PixelCoordinate * canvas.CanvasUnitSize.X / (float)canvas.CanvasPixelSize.Y;
        }

        /// <summary>
        /// Calculates mouse sensitivity.
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public Vector2f CalcMouseSensitivity(ICanvas canvas)
        {
            Vector2i ps = canvas.CanvasPixelSize;
            return mouseSensitivity.CanvasCoordinates +
                Vector2f.ComponentDivision(Vector2f.ComponentMultiply(mouseSensitivity.PixelCoordinates,
                canvas.CanvasUnitSize), new Vector2f(ps.X, ps.Y));
        }

        #endregion
    }
}
