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

namespace SharpMedia.Scene.Lighting
{
    /// <summary>
    /// A light is an interface that can be pluged into lighting operations.
    /// </summary>
    [SceneComponent(QueryType=typeof(ILight))]
    public interface ILight : IInterface
    {

        /// <summary>
        /// Lits the object, returning colour.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        ShaderCompiler.Operand Lit(ShaderCompiler compiler, ShaderCompiler.Operand position, ShaderCompiler.Operand normal,
                            Materials.IBRDFInterface material);

        /// <summary>
        /// Is the light enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// The affecting radius of light, given LOD.
        /// </summary>
        /// <param name="lod"></param>
        /// <returns></returns>
        double GetAffectingRadius(uint lod);

        // TODO: more light helpers, common properties etc.
    }
}
