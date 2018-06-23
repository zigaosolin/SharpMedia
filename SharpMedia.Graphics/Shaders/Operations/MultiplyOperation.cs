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
    /// A multiply operation. Allows type-type, vector-matrix and matrix-vector products.
    /// </summary>
    /// <remarks>
    /// Class is not part of Arithmetic extenders because it must provide custom descriptors (to support
    /// matrix-vector and vector-matrix).
    /// </remarks>
    [Serializable]
    public class MultiplyOperation : Operation
    {
        #region Private Members
        static readonly PinsDescriptor inputDesc;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        static MultiplyOperation()
        {
            PinDescriptor src1 = new PinDescriptor(BinaryMathOperation.AllFormats, "Operand 1 (Left)");
            PinDescriptor src2 = new PinDescriptor(BinaryMathOperation.AllFormats, "Operand 2 (Right)");
            IPinRelation req = new PinMultipliable(src1, src2);

            inputDesc = new PinsDescriptor(req, src1, src2);
        }

        public MultiplyOperation()
        {
        }

        #endregion

        #region Overrides

        protected override Pin[] CreateOutputs()
        {
            // We search for vector-* and *-vector operations.
            PinFormat scalar;
            if (PinFormatHelper.IsVector(inputs[0].Format, out scalar))
            {
                // The result must be matrix.
                return new Pin[] { inputs[0].Clone(this) };
            }
            if (PinFormatHelper.IsVector(inputs[1].Format, out scalar))
            {
                return new Pin[] { inputs[1].Clone(this) };
            }

            // We search for matrix-* and *-matrix operations.
            if (PinFormatHelper.IsMatrix(inputs[0].Format, out scalar))
            {
                // The result must be matrix.
                return new Pin[] { inputs[0].Clone(this) };
            }
            if (PinFormatHelper.IsMatrix(inputs[1].Format, out scalar))
            {
                // The result must be matrix.
                return new Pin[] { inputs[1].Clone(this) };
            } 

            // Must be equal, we simply output.
            return new Pin[] { inputs[0].Clone(this) };

        }

        public override PinsDescriptor InputDescriptor
        {
            get { return inputDesc; }
        }

        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            return new ShaderCompiler.Operand[] { compiler.Mul(operands[0], operands[1]) };
        }

        #endregion
    }
}
