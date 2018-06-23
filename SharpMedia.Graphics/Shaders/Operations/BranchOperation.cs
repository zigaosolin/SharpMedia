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
    /// A simple branch operation.
    /// </summary>
    public sealed class BranchOperation : Operation
    {

        #region Static Members

        static readonly PinFormat[] AllFormats = new PinFormat[] { 
            PinFormat.Float, PinFormat.Floatx2, PinFormat.Floatx3, PinFormat.Floatx4,
            PinFormat.Integer, PinFormat.Integerx2, PinFormat.Integerx3, PinFormat.Integerx4,
            PinFormat.UInteger, PinFormat.UIntegerx2, PinFormat.UIntegerx3, PinFormat.UIntegerx4,
            PinFormat.UNorm, PinFormat.UNormx2, PinFormat.UNormx3, PinFormat.UNormx4,
            PinFormat.SNorm, PinFormat.SNormx2, PinFormat.SNormx3, PinFormat.SNormx4,
            PinFormat.Float2x2, PinFormat.Float3x3, PinFormat.Float4x4
        };

        static PinsDescriptor inputDesc;

        static BranchOperation()
        {
            PinDescriptor b = new PinDescriptor(PinFormat.Bool, "Branch value.");
            PinDescriptor in1 = new PinDescriptor(AllFormats, "True branch value.");
            PinDescriptor in2 = new PinDescriptor(AllFormats, "False branch value.");

            inputDesc = new PinsDescriptor(
                new PinEqual(in1, in2), in1, in2);
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Creates a branching operation.
        /// </summary>
        public BranchOperation()
        {
        }

        protected override Pin[] CreateOutputs()
        {
            return new Pin[] { inputs[1].Clone(this) };
        }

        public override PinsDescriptor InputDescriptor
        {
            get { return inputDesc; }
        }

        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, 
            FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            ShaderCompiler.Operand dst = compiler.CreateTemporary(operands[1].Format, operands[1].ArraySize);
            compiler.BeginIf(operands[0]);
            compiler.Mov(operands[1], dst);
            compiler.Else();
            compiler.Mov(operands[2], dst);
            compiler.EndIf();
            return new ShaderCompiler.Operand[] { dst };
            
        }

        #endregion
    }
}
