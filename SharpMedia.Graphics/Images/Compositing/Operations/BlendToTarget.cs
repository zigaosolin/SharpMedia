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
using SharpMedia.Graphics.States;

namespace SharpMedia.Graphics.Images.Compositing.Operations
{
    /// <summary>
    /// A blend to target operation.
    /// </summary>
    public sealed class BlendToTarget : CopyToTarget
    {
        #region Blend State
        BlendState blendState;
        Colour blendColour;
        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="colour"></param>
        public BlendToTarget(Vector2i offset, BlendState state, Colour colour)
            : base(offset)
        {
            this.blendState = StateManager.Intern(state);
            this.blendColour = colour;
        }

        #endregion

        #region Overrides

        public override void CompositeTo(CompositingResources resources, RenderTargetView view)
        {
            // We compute target position.
            Region2i viewport = new Region2i((Vector2i)offset, new Vector2i((int)view.Width, (int)view.Height));

            resources.CompositeToSource(this, blendState, blendColour, viewport, view);
        }

        #endregion
    }
}
