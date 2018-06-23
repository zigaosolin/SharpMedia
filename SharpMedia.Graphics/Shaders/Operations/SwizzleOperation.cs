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

namespace SharpMedia.Graphics.Shaders.Operations
{

    /// <summary>
    /// A swizzle operation.
    /// </summary>
    [Serializable]
    public class SwizzleOperation : Operation
    {
        #region Private Members
        SwizzleMask mask;
        PinsDescriptor inputDesc;
        #endregion

        #region Public Members

        public SwizzleOperation(SwizzleMask mask)
        {
            this.mask = mask;

            inputDesc = new PinsDescriptor(
                new PinDescriptor(PinFormatHelper.ContainsComponents(mask.MinimumSwizzleInFormat), "Data to be swizzled."));
        }

        #endregion

        #region Overrides

        protected override Pin[] CreateOutputs()
        {
            return new Pin[] { new Pin(mask.SwizzleOutFormat, Pin.NotArray, this) };
        }

        public override PinsDescriptor InputDescriptor
        {
            get { return inputDesc; }
        }

        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            return new ShaderCompiler.Operand[]
            {
                compiler.Swizzle(operands[0], mask)
            };
        }

        #endregion
    }
}
