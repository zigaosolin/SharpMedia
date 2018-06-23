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
    /// Holds read-only data, shared between arithmetic operations.
    /// </summary>
    public abstract class BinaryMathOperation : Operation
    {
        #region Static Part

        /// <summary>
        /// All formats except samplers.
        /// </summary>
        internal static readonly PinFormat[] AllFormats = new PinFormat[] { 
            PinFormat.Float, PinFormat.Floatx2, PinFormat.Floatx3, PinFormat.Floatx4,
            PinFormat.Integer, PinFormat.Integerx2, PinFormat.Integerx3, PinFormat.Integerx4,
            PinFormat.UInteger, PinFormat.UIntegerx2, PinFormat.UIntegerx3, PinFormat.UIntegerx4,
            PinFormat.UNorm, PinFormat.UNormx2, PinFormat.UNormx3, PinFormat.UNormx4,
            PinFormat.SNorm, PinFormat.SNormx2, PinFormat.SNormx3, PinFormat.SNormx4,
            PinFormat.Float2x2, PinFormat.Float3x3, PinFormat.Float4x4
        };

        protected static readonly PinsDescriptor InputDesc;

        /// <summary>
        /// A static constructor.
        /// </summary>
        static BinaryMathOperation()
        {
            // Input descriptor.
            PinDescriptor in1 = new PinDescriptor(BinaryMathOperation.AllFormats, "Source 1");
            PinDescriptor in2 = new PinDescriptor(BinaryMathOperation.AllFormats, "Source 2");
            IPinRelation req = new PinEqual(in1, in2);
            InputDesc = new PinsDescriptor(req, in1, in2);
        }

        #endregion

        #region Implemented

        protected override Pin[] CreateOutputs()
        {
            return new Pin[] { inputs[0].Clone(this) };
        }

        #endregion

        #region IOperation Members

        public override PinsDescriptor InputDescriptor
        {
            get { return InputDesc; }
        }

        #endregion

        
    }

    /// <summary>
    /// Adds two pins. All operand types supported.
    /// </summary>
    [Serializable]
    public class AddOperation : BinaryMathOperation
    {
        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            return new ShaderCompiler.Operand[]{
                compiler.Add(operands[0], operands[1])
            };
        }
    }

    /// <summary>
    /// Substracts two pins. All operand types supported.
    /// </summary>
    [Serializable]
    public class SubstractOperation : BinaryMathOperation
    {
        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            return new ShaderCompiler.Operand[]{
                compiler.Sub(operands[0], operands[1])
            };
        }
    }

    /// <summary>
    /// Divides two pins. All operand types supported.
    /// </summary>
    [Serializable]
    public class DivideOperation : BinaryMathOperation
    {
        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            return new ShaderCompiler.Operand[]{
                compiler.Div(operands[0], operands[1])
            };
        }
    }


    /// <summary>
    /// A dot product. Suppoted only for vectors and scalars.
    /// </summary>
    [Serializable]
    public class DotProductOperation : Operation
    {
        #region Static Part

        /// <summary>
        /// Vectors and scalars.
        /// </summary>
        protected static readonly PinFormat[] InputFormats = new PinFormat[] { 
            PinFormat.Float, PinFormat.Floatx2, PinFormat.Floatx3, PinFormat.Floatx4,
            PinFormat.Integer, PinFormat.Integerx2, PinFormat.Integerx3, PinFormat.Integerx4
        };

        protected static readonly PinFormat[] OutputFormats = new PinFormat[] { 
            PinFormat.Float, PinFormat.Integer
        };

        protected static PinsDescriptor InputDesc;

        /// <summary>
        /// A static constructor.
        /// </summary>
        static DotProductOperation()
        {
            // Input descriptor.
            PinDescriptor in1 = new PinDescriptor(DotProductOperation.InputFormats, "Source 1");
            PinDescriptor in2 = new PinDescriptor(DotProductOperation.InputFormats, "Source 2");
            IPinRelation req = new PinEqual(in1, in2);
            InputDesc = new PinsDescriptor(req, in1, in2);
        }

        #endregion

        #region IBaseOperation Members

        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            return new ShaderCompiler.Operand[]{
                compiler.Dot(operands[0], operands[1])
            };
        }

        #endregion

        #region Implementation

        protected override Pin[] CreateOutputs()
        {
            PinFormat scalar;
            PinFormatHelper.IsVector(inputs[0].Format, out scalar);

            // We now create result.
            return new Pin[] { new Pin(scalar, Pin.NotArray, this) };
        }

        public override PinsDescriptor InputDescriptor
        {
            get { return InputDesc; }
        }

        #endregion
    }

}
