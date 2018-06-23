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

namespace SharpMedia.Graphics.Shaders
{


    /// <summary>
    /// An interface element. Interfaces are "dynamic" operations that effect the shader
    /// compiling process. Interface is part of <see cref="ShaderParameters" /> and not part
    /// of ShaderCode.
    /// </summary>
    /// <remarks>
    /// Interface type, not interface itself, fixes the shader generation. This means that class
    /// that extends IInterface must always have the same additional parameters.
    /// </remarks>
    public interface IInterface
    {
        /// <summary>
        /// The type of operation that can accept such argument.
        /// </summary>
        Type TargetOperationType
        {
            get;
        }

        /// <summary>
        /// Additional parameters that must be set in order to use this interface.
        /// </summary>
        /// <remarks>Parameters must be fixed per-type. All parameters are put to scope, meaning
        /// that returning "Texture" name actually means "InterfaceScope.Texture", for example,
        /// "Lights[1].CubeMapMin".</remarks>
        ParameterDescription[] AdditionalParameters
        {
            get;
        }
    }
}
