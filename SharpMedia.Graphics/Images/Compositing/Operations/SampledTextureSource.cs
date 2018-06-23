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
using SharpMedia.Graphics.States;
using SharpMedia.Math;

namespace SharpMedia.Graphics.Images.Compositing.Operations
{

    /// <summary>
    /// Sampled texture source.
    /// </summary>
    public class SampledTextureSource : ICompositingOperation, ICompositeInterface
    {
        #region Private Members
        TextureView view;
        SamplerState state;
        ParameterDescription[] description = new ParameterDescription[3];
        #endregion

        #region Public members

        public SampledTextureSource(TextureView view, SamplerState state)
        {
            this.view = view;
            this.state = state;

            this.description[0] = new ParameterDescription("Texture",
                new Pin(PinFormat.Texture2D, PinFormat.Floatx4, Pin.NotArray, null));
            this.description[2] = new ParameterDescription("Scale",
                new Pin(PinFormat.Floatx2, Pin.NotArray, null));
            this.description[1] = new ParameterDescription("Sampler",
                new Pin(PinFormat.Sampler, Pin.NotArray, null));
        }

        #endregion

        #region ICompositingOperation Members

        public OperationType OperationType
        {
            get { return OperationType.NoSource; }
        }

        public ICompositingOperation Source1
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public ICompositingOperation Source2
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public ICompositeInterface Interface
        {
            get { return this; }
        }

        public Vector2i Size
        {
            get { return new Vector2i((int)view.Width, (int)view.Height); }
        }

        #endregion

        #region ICompositeInterface Members

        object[] ICompositeInterface.ParameterValues
        {
            get { return new object[] { view, state, 
                new Vector2f(1.0f/(float)view.Width, 1.0f/(float)view.Height) }; }
        }

        ShaderCompiler.Operand ICompositeInterface.GetPixel(ShaderCompiler compiler, 
            ShaderCompiler.Operand absolutePosition, Dictionary<ICompositeInterface, ShaderCompiler.Operand[]> constants)
        {
            ShaderCompiler.Operand[] inputs = constants[this];
 
            return compiler.Sample(inputs[1], inputs[0], 
                compiler.CreateFixed(PinFormat.Floatx2, Pin.NotArray, new Vector2f(0,0)),
                compiler.Mul(absolutePosition, inputs[2])
                );
        }

        #endregion

        #region IInterface Members

        Type SharpMedia.Graphics.Shaders.IInterface.TargetOperationType
        {
            get { return typeof(CompositingOperation); }
        }

        ParameterDescription[] IInterface.AdditionalParameters
        {
            get { return description; }
        }

        #endregion
    }
}
