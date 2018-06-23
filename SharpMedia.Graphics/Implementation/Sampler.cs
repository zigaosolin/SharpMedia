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

namespace SharpMedia.Graphics.Implementation
{
    /// <summary>
    /// The class can perform filtering based on sampler state object.
    /// </summary>
    /// <remarks>Does not do equal mipmapped results as it should but should provide good approximate.</remarks>
    internal static class Sampler
    {
        /// <summary>
        /// Samples the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public static Vector4f Sample(SamplerState state, TextureView texture, Vector2f p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Samples the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public static Vector4f Sample(SamplerState state, TextureView texture, Vector3f p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Samples the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public static Vector4f Sample(SamplerState state, TextureView texture, Vector4f p)
        {
            throw new NotImplementedException();
        }

    }
}
