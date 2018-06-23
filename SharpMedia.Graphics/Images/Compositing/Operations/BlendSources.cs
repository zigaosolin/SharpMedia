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
using SharpMedia.Math;
using SharpMedia.Graphics.Shaders;

namespace SharpMedia.Graphics.Images.Compositing.Operations
{

    /// <summary>
    /// Blends two sources.
    /// </summary>
    public class BlendSources : ICompositingOperation, ICompositeInterface
    {
        #region Private Members
        ICompositingOperation source1;
        ICompositingOperation source2;
        Vector4f factor1;
        Vector4f factor2;

        static ParameterDescription[] paramDescription = new ParameterDescription[]
            {
                new ParameterDescription("Factor 1", new Pin(PinFormat.Floatx4, Pin.NotArray, null)),
                new ParameterDescription("Factor 2", new Pin(PinFormat.Floatx4, Pin.NotArray, null)),
            };
        #endregion

        #region Constructors

        /// <summary>
        /// Blends sources.
        /// </summary>
        /// <param name="factor1"></param>
        /// <param name="factor2"></param>
        public BlendSources(Vector4f factor1, Vector4f factor2)
        {
            this.factor1 = factor1;
            this.factor2 = factor2;

        }

        #endregion

        #region ICompositingOperation Members

        public OperationType OperationType
        {
            get { return OperationType.TwoSources; }
        }

        public ICompositingOperation Source1
        {
            get
            {
                return source1;
            }
            set
            {
                source1 = value;
            }
        }

        public ICompositingOperation Source2
        {
            get
            {
                return source2;
            }
            set
            {
                source2 = value;
            }
        }

        public ICompositeInterface Interface
        {
            get { return this; }
        }

        public Vector2i Size
        {
            get { return source1.Size; }
        }

        #endregion

        #region ICompositeInterface Members

        object[] ICompositeInterface.ParameterValues
        {
            get
            {
                return new object[] { factor1, factor2 };
            }
        }

        ShaderCompiler.Operand ICompositeInterface.GetPixel(ShaderCompiler compiler, 
            ShaderCompiler.Operand absolutePosition, Dictionary<ICompositeInterface, ShaderCompiler.Operand[]> constants)
        {
            ShaderCompiler.Operand[] inputs = constants[this];

            return compiler.Add(
                compiler.Mul(source1.Interface.GetPixel(compiler, absolutePosition, constants), inputs[0]),
                compiler.Mul(source2.Interface.GetPixel(compiler, absolutePosition, constants), inputs[1])
            );

        }

        #endregion

        #region IInterface Members

        Type SharpMedia.Graphics.Shaders.IInterface.TargetOperationType
        {
            get { return typeof(CompositingOperation);  }
        }

        ParameterDescription[] IInterface.AdditionalParameters
        {
            get { return paramDescription; }
        }

        #endregion
    }
}
