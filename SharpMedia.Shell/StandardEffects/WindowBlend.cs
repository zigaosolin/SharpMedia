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
using System.Collections.ObjectModel;

namespace SharpMedia.Shell.StandardEffects
{

    /// <summary>
    /// A window blend effect.
    /// </summary>
    /// <remarks></remarks>
    public sealed class WindowBlend : IWindowEffect
    {
        #region Private Members
        bool useRTAlpha = false;
        float factor = 1.0f;
        #endregion

        #region Public Members

        /// <summary>
        /// Creates window blend effect with constant alpha factor.
        /// </summary>
        /// <param name="factor"></param>
        public WindowBlend([MinFloat(0.0f), MaxFloat(1.0f)]float factor)
        {
            this.factor = factor;
        }

        /// <summary>
        /// Creates window bleld effect with precomputed factor and possible alpha blending.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="useRTAlpha"></param>
        public WindowBlend([MinFloat(0.0f), MaxFloat(1.0f)]float factor, bool useRTAlpha)
        {
            this.factor = factor;
            this.useRTAlpha = useRTAlpha;
        }

        /// <summary>
        /// Creates window blend effect with 
        /// </summary>
        public WindowBlend()
        {
            useRTAlpha = true;
        }

        /// <summary>
        /// Does effect use render target's alpha.
        /// </summary>
        public bool UseRenderTargetAlpha
        {
            get
            {
                return useRTAlpha;
            }
        }


        /// <summary>
        /// Gets or sets the blend factor.
        /// </summary>
        public float BlendFactor
        {
            get
            {
                return factor;
            }
            set
            {
                factor = value;
            }
        }

        #endregion

        #region IWindowEffect Members

        public ReadOnlyCollection<IWindow> AppliesTo
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
