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
    /// A const colour source.
    /// </summary>
    public sealed class ColourSource : ICompositingOperation, ICompositeInterface
    {
        #region Private Members
        private Colour colour;
        static ParameterDescription[] parameterDescription = new ParameterDescription[]
            {
                new ParameterDescription("Colour", new Pin(PinFormat.Floatx4, Pin.NotArray, null))
            };
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="colour"></param>
        public ColourSource(Colour colour)
        {
            this.colour = colour;
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
            get { return new Vector2i(1, 1); }
        }

        #endregion

        #region ICompositeInterface Members

        object[] ICompositeInterface.ParameterValues
        {
            get { return new object[] { colour.RGBA }; }
        }

        ShaderCompiler.Operand ICompositeInterface.GetPixel(SharpMedia.Graphics.Shaders.ShaderCompiler compiler, 
            ShaderCompiler.Operand absolutePosition, Dictionary<ICompositeInterface, ShaderCompiler.Operand[]> constants)
        {
            return constants[this][0];
        }

        #endregion

        #region IInterface Members

        Type SharpMedia.Graphics.Shaders.IInterface.TargetOperationType
        {
            get { return typeof(CompositingOperation); }
        }

        ParameterDescription[] IInterface.AdditionalParameters
        {
            get { return parameterDescription; }
        }

        #endregion
    }
}
