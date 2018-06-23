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
    /// A copy target operation.
    /// </summary>
    public class CopyToTarget : ITerminalCompositingOperation, ICompositeInterface
    {
        #region Private Members
        protected ICompositingOperation source;
        protected Vector2i offset = Vector2i.Zero;

        static ParameterDescription[] inputParameters = new ParameterDescription[]
        {
            new ParameterDescription("Offset", new Pin(PinFormat.Integerx2, Pin.NotArray, null))
        };
        #endregion

        #region Constructors

        public CopyToTarget(Vector2i offset)
        {
            this.offset = offset;
        }

        #endregion

        #region ITerminalCompositingOperation Members

        public virtual void CompositeTo(CompositingResources resources, RenderTargetView view)
        {
            Vector2i size = source.Size;
            if (size.X + offset.X > (int)view.Width) size.X = (int)view.Width - offset.X;
            if (size.Y + offset.Y > (int)view.Height) size.Y = (int)view.Height - offset.Y;

            // We compute target position.
            Region2i viewport = new Region2i((Vector2i)offset, offset + size);

            resources.CompositeToSource(this, null, Colour.White, viewport, view);
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
            get { return source.Size; }
        }

        #endregion

        #region ICompositeInterface Members

        object[] ICompositeInterface.ParameterValues
        {
            get { return new object[] { offset }; }
        }

        ShaderCompiler.Operand ICompositeInterface.GetPixel(ShaderCompiler compiler, ShaderCompiler.Operand absolutePosition, 
            Dictionary<ICompositeInterface, ShaderCompiler.Operand[]> constants)
        {
            return source.Interface.GetPixel(compiler, absolutePosition, constants);
        }

        #endregion

        #region IInterface Members

        Type SharpMedia.Graphics.Shaders.IInterface.TargetOperationType
        {
            get { return typeof(CompositingOperation); }
        }

        ParameterDescription[] IInterface.AdditionalParameters
        {
            get { return inputParameters; }
        }

        #endregion
    }
}
