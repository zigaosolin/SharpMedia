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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Shaders.Operations
{
    /// <summary>
    /// Indexes an array (static or dynamic).
    /// </summary>
    [Serializable]
    public class IndexInArrayOperation : Operation
    {
        #region Static Members
        static readonly PinFormat[] InputFormats = new PinFormat[] { 
            PinFormat.Float, PinFormat.Floatx2, PinFormat.Floatx3, PinFormat.Floatx4,
            PinFormat.Integer, PinFormat.Integerx2, PinFormat.Integerx3, PinFormat.Integerx4,
            PinFormat.UInteger, PinFormat.UIntegerx2, PinFormat.UIntegerx3, PinFormat.UIntegerx4,
            PinFormat.UNorm, PinFormat.UNormx2, PinFormat.UNormx3, PinFormat.UNormx4,
            PinFormat.SNorm, PinFormat.SNormx2, PinFormat.SNormx3, PinFormat.SNormx4,
            PinFormat.Float2x2, PinFormat.Float3x3, PinFormat.Float4x4
        };

        static PinsDescriptor inputDesc = new PinsDescriptor(
            new PinDescriptor(InputFormats, "Array", Pin.DynamicArray),
            new PinDescriptor(new PinFormat[]{PinFormat.Integer, PinFormat.UInteger}, "Index"));
        #endregion

        #region Public Methods

        protected override Pin[] CreateOutputs()
        {
            return new Pin[] { new Pin(inputs[0].Format, Pin.NotArray, this) };
        }

        public override PinsDescriptor InputDescriptor
        {
            get { return inputDesc; }
        }

        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            return new ShaderCompiler.Operand[] {compiler.IndexInArray(operands[0], operands[1]) };
        }

        #endregion
    }
}
