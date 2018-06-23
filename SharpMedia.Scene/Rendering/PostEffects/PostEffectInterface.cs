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
using SharpMedia.Graphics.Shaders;

namespace SharpMedia.Scene.PostEffects
{

    /// <summary>
    /// This is a post-effect interface.
    /// </summary>
    public interface IPostEffectInterface : IInterface
    {

        /// <summary>
        /// A post effect.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        /// <param name="colour">The input colour.</param>
        /// <param name="position">Pixel world space position.</param>
        /// <param name="normal">Pixel normal in world space.</param>
        /// <returns>Output colour.</returns>
        ShaderCompiler.Operand PostEffect(ShaderCompiler compiler, ShaderCompiler.Operand colour,
                                          ShaderCompiler.Operand position, ShaderCompiler.Operand normal);

    }
}
