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

namespace SharpMedia.Graphics.Images.Compositing
{

    /// <summary>
    /// A pixel provider interface, used by compositing operation.
    /// </summary>
    public interface ICompositeInterface : IInterface
    {

        /// <summary>
        /// Parameter values, as defined by parameter description.
        /// </summary>
        object[] ParameterValues { get; }

        /// <summary>
        /// Provides a pixel based on position.
        /// </summary>
        /// <param name="compiler"></param>
        /// <param name="absolutePosition"></param>
        /// <param name="parentProviders"></param>
        /// <returns></returns>
        ShaderCompiler.Operand GetPixel(ShaderCompiler compiler,
            ShaderCompiler.Operand absolutePosition, 
            Dictionary<ICompositeInterface, ShaderCompiler.Operand[]> constants);

    }
}
