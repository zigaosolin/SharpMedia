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
    /// Resizes source, nearest filter.
    /// </summary>
    public class ResizeSource: ICompositingOperation, ICompositeInterface
    {
        #region Private Members
        ICompositingOperation source;
        Vector2i size;

        static ParameterDescription[] paramDescription = new ParameterDescription[]
            {
                new ParameterDescription("Scale", new Pin(PinFormat.Floatx2, Pin.NotArray, null))
            };
        #endregion

        #region Constructors

        /// <summary>
        /// Blends sources.
        /// </summary>
        /// <param name="factor1"></param>
        /// <param name="factor2"></param>
        public ResizeSource(Vector2i size)
        {
            this.size = size;
        }

        #endregion

        #region ICompositingOperation Members

        public OperationType OperationType
        {
            get { return OperationType.OneSource; }
        }

        public ICompositingOperation Source1
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
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
            get { return size; }
        }

        #endregion

        #region ICompositeInterface Members

        object[] ICompositeInterface.ParameterValues
        {
            get
            {
                Vector2i refSize = source.Size;

                return new object[] { new Vector2f((float)refSize.X / ((float)size.X),
                                                   (float)refSize.Y / ((float)size.Y)) };
            }
        }

        ShaderCompiler.Operand ICompositeInterface.GetPixel(ShaderCompiler compiler, 
            ShaderCompiler.Operand absolutePosition, Dictionary<ICompositeInterface, ShaderCompiler.Operand[]> constants)
        {
            return source.Interface.GetPixel(compiler, compiler.Mul(absolutePosition, constants[this][0]),
                constants);

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
