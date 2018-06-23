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
    public partial class ShaderCode
    {
        #region Static Members

        /// <summary>
        /// A fallthrough shader, copies all inputs to outputs (the same name).
        /// </summary>
        /// <param name="inputPinComponents">Input components that must be provided.</param>
        /// <param name="outputPinComponents">Output components of shader.</param>
        /// <remarks>
        /// All output compononents must have inputs, if too many inputs are provided, they are simply discarded.
        /// </remarks>
        /// <returns>The ShaderCode matching description.</returns>
        public static ShaderCode FallTroughShader(BindingStage stage, PinComponent inputPinComponents, PinComponent outputPinComponents)
        {
            if ((outputPinComponents & inputPinComponents) != outputPinComponents)
            {
                throw new ArgumentException("Invalid bindings, some output pins do not have input components.");
            }

            // TODO:
            return null;
        }

        #endregion

    }
}
