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
using SharpMedia.Math.Matrix;
using SharpMedia.Graphics;
using SharpMedia.Math;
using SharpMedia.Math.Shapes;
using System.Collections.ObjectModel;
using SharpMedia.AspectOriented;

namespace SharpMedia.Scene
{
    /// <summary>
    /// A scene camera component.
    /// </summary>
    [SceneComponent]
    public class Camera : ISCNamed
    {
        #region Protected Members
        string cameraName = string.Empty;
        RenderTargetView renderTarget = null;
        Region2i targetViewport = new Region2i(0, 0, 0, 0);
        double fieldOfView = 120.0;
        double minimumDistance = 0.0;
        double maximumDistance = 1000.0;
        List<PostEffects.IPostEffectPass> postEffects 
            = new List<PostEffects.IPostEffectPass>();
        Colour backgroundColour = Colour.Black;
        bool clearBeforeRender = false;
        bool isEnabled = false;
        #endregion

        #region ISCNamed Members

        public string Name
        {
            get { return cameraName; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Is the camera enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        /// <summary>
        /// Camera field of view, in degrees. 
        /// </summary>
        /// <remarks>You can specify 0.0 for orto camera projection.</remarks>
        public double FieldOfView 
        { 
            get { return fieldOfView; } 
            set { fieldOfView = value; } 
        }

        /// <summary>
        /// Minimum distance that the camera is able to capture
        /// </summary>
        public double MinimumDistance 
        { 
            get { return minimumDistance; } 
            set { minimumDistance = value; } 
        }

        /// <summary>
        /// Maximum distance that the camera is able to capture
        /// </summary>
        public double MaximumDistance 
        { 
            get { return maximumDistance; } 
            set { maximumDistance = value; } 
        }

        /// <summary>
        /// <summary>
        /// A View of a Render Target to render to (with)
        /// </summary>
        public RenderTargetView RenderTarget 
        { 
            get { return renderTarget; } 
            set { renderTarget = value; } 
        }

        /// <summary>
        /// Render target viewport - target rectangle to use to render to. Set to (0,0,0,0) to cover the whole screen
        /// </summary>
        public Region2i TargetViewport 
        { 
            get { return targetViewport; } 
            set { targetViewport = value; } 
        }

        /// <summary>
        /// A readonly collection of post effects that are applied to the renderings of the camera
        /// </summary>
        public ReadOnlyCollection<PostEffects.IPostEffectPass> PostEffects 
        { 
            get { return new ReadOnlyCollection<PostEffects.IPostEffectPass>(postEffects); } 
        }


        /// <summary>
        /// The background colour to use to clear the render target with before rendering
        /// </summary>
        public Colour BackgroundColour 
        { 
            get { return backgroundColour; } 
            set { backgroundColour = value; } 
        }

        /// <summary>
        /// Whether to clear the render target with the background colour before rendering commences
        /// </summary>
        public bool ClearBeforeRender 
        { 
            get { return clearBeforeRender; } 
            set { clearBeforeRender = value; } 
        }
 

        #endregion

        #region Methods

        /// <summary>
        /// Adds a post effect to the camera
        /// </summary>
        /// <param name="fx">Post effect to add</param>
        public void AddPostEffect(PostEffects.IPostEffectPass fx)
        {
            postEffects.Add(fx);
        }

        /// <summary>
        /// Removes a post effect from the camera
        /// </summary>
        /// <param name="fx">Post effect to remove</param>
        public void RemovePostEffect(PostEffects.IPostEffectPass fx)
        {
            postEffects.Remove(fx);
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a camera with field of view.
        /// </summary>
        /// <param name="renderTarget">Render target to render to</param>
        /// <param name="fieldOfView">Field of view, in degrees</param>
        public Camera([NotNull] RenderTargetView renderTarget, double fieldOfView)
        {
            this.renderTarget = renderTarget;
            this.fieldOfView = fieldOfView;
        }

        #endregion


    }
}
