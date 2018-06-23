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
using SharpMedia.Graphics.Shaders.Operations;
using SharpMedia.Math;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Vector.Fills
{

    /// <summary>
    /// A specific fill type.
    /// </summary>
    /// <remarks>Two types of fill exist - instance dependant and instance independant. The later ones use
    /// only attributes to calculate result and are faster this way.</remarks>
    public interface IFill : IInterface, IEquatable<IFill>
    {

        /// <summary>
        /// The shader specific parameters. They are ordered based on ParameterDesc array.
        /// </summary>
        /// <remarks>Must not change for the same object inside one pass.</remarks>
        object[] ParameterValues { get; }

        /// <summary>
        /// Adds code to shader.
        /// </summary>
        /// <param name="compiler">Shader compile.</param>
        /// <param name="allocator">The allocator that can be used to 
        /// allocate additional parameters (textures, samplers, constants).</param>
        /// <param name="attributes">The custom attributes (float4).</param>
        /// <param name="position">Position in projected space (float2).</param>
        /// <param name="texCoord">Texture coordinate (float2). </param>
        /// <param name="borderDistance">The nearest distance from border, in pixel and relative units (as Floatx2).</param>
        /// <param name="outputColour">The ourput colour, writable (float4).</param>
        void Compile([NotNull] ShaderCompiler compiler, [NotNull] ShaderCompiler.Operand position,  [NotNull] ShaderCompiler.Operand texCoord, 
                     [NotNull] ShaderCompiler.Operand[] attributes, [NotNull] ShaderCompiler.Operand borderDistance,
                     [NotNull] ShaderCompiler.Operand[] constants, [NotNull] ShaderCompiler.Operand outputColour);

        /// <summary>
        /// Does new instance of fill mean new interface operation, or can we reuse operations
        /// on type basis.
        /// </summary>
        /// <remarks>The same type must always return the same value.</remarks>
        bool IsInstanceDependant { [TypeConstantReturn] get; }

        /// <summary>
        /// Does fill require custom attribute (it is computed per vertex).
        /// </summary>
        /// <remarks>The value's maximum depends on canvas but is usually limited to 2.</remarks>
        uint CustomAttributeCount { [TypeConstantReturn] get; }

        /// <summary>
        /// Calculates custom attribute. It is given vertex positions and texcoords.
        /// </summary>
        /// <param name="id">The attribute id.</param>
        /// <param name="shape">The shape object.</param>
        /// <param name="positions">Positions of vertices of shape.</param>
        /// <param name="texCoord">Texture coordinates.</param>
        /// <returns>Custom attributes.</returns>
        /// <remarks>Called only if requires custom attributes returns true.</remarks>
        Vector4f CalcCustomAttribute(uint id, [NotNull] object shape, Vector2f position);
    }


}
