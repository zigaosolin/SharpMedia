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
using SharpMedia.Graphics;

namespace SharpMedia.Scene.Animation
{

    /// <summary>
    /// An animation interface affects how to animate objects. It can consist of preprocessing
    /// and shader code injection through interface.
    /// </summary>
    public interface IAnimation : IInterface
    {
        /// <summary>
        /// Tells whether animation needs CPU processing.
        /// </summary>
        bool RequiresCPUPreprocess { get; }

        /// <summary>
        /// Compiles the animation into shader.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        /// <param name="position">Position operand.</param>
        /// <param name="normal">Normal operand (can be null).</param>
        /// <param name="outPosition">Output position (must be written to).</param>
        /// <param name="outNormal">Output normal (must be written if normal is not-null).</param>
        void Compile(ShaderCompiler compiler, ShaderCompiler.Operand position, ShaderCompiler.Operand normal,
                    ShaderCompiler.Operand outPosition, ShaderCompiler.Operand outNormal);

        /// <summary>
        /// Where to preprocess the data.
        /// </summary>
        /// <param name="from">The constant (no animation).</param>
        /// <param name="to">Where to copy.</param>
        void PreProcess(Geometry from, Geometry to);
    }
}
