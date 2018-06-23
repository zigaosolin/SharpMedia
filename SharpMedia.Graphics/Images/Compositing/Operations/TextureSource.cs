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
using SharpMedia.Math;

namespace SharpMedia.Graphics.Images.Compositing.Operations
{

    /// <summary>
    /// A texture source view.
    /// </summary>
    public sealed class TextureSource : ICompositingOperation, ICompositeInterface
    {
        #region Private Members
        TextureView view;
        ParameterDescription[] description = new ParameterDescription[1];
        #endregion

        #region Public members

        public TextureSource(TextureView view)
        {
            this.view = view;

            this.description[0] = new ParameterDescription("Texture",
                new Pin(PinFormat.Texture2D, PinFormat.Floatx4, Pin.NotArray, null));
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
            get { return new object[] { view }; }
        }

        ShaderCompiler.Operand ICompositeInterface.GetPixel(ShaderCompiler compiler, 
            ShaderCompiler.Operand absolutePosition, Dictionary<ICompositeInterface, ShaderCompiler.Operand[]> constants)
        {
            ShaderCompiler.Operand[] inputs = constants[this];
            ShaderCompiler.Operand offset = compiler.CreateFixed(PinFormat.Integerx2, Pin.NotArray, new Vector2i(0, 0));

            return compiler.Load(inputs[0], compiler.Expand(
                compiler.Convert(absolutePosition, PinFormat.Integerx2), 
                PinFormat.Integerx3, ExpandType.AddZeros), offset);
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
