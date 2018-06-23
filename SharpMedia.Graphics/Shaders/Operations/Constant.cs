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
    /// A constant provider operation. Constant can be used as fixed (specified at 
    /// shader compile time) or as constant (specified at shader usage context). You can force
    /// constant to be fixed by not specifying it's name (null name).
    /// </summary>
    /// <remarks>
    /// Operation can only be constructed from ShaderCode.
    /// </remarks>
    public class ConstantOperation : IOperation
    {
        #region Private Members
        string name;

        // Used by fixed constants.
        object value; 

        ShaderCode scope;
        Pin output;
        string comment;

        internal void Discard()
        {
            scope = null;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a non-fixed constant.
        /// </summary>
        internal ConstantOperation([NotEmpty] string name, PinFormat fmt, uint size, [NotNull] ShaderCode scope)
        {
            this.name = name;
            this.scope = scope;

            // We create the pin.
            output = new Pin(fmt, size, this);
        }

        /// <summary>
        /// Creates a non-fixed constant.
        /// </summary>
        internal ConstantOperation([NotEmpty] string name, PinFormat fmt, uint size, object value, [NotNull] ShaderCode scope)
        {
            this.name = name;
            this.scope = scope;
            this.value = value;

            // We create the pin.
            output = new Pin(fmt, size, this);
        }

        /// <summary>
        /// Creates a texture constant.
        /// </summary>
        internal ConstantOperation([NotEmpty] string name, PinFormat texture, PinFormat textureFormat, [NotNull] ShaderCode scope)
        {
            this.name = name;
            this.scope = scope;

            output = new Pin(texture, textureFormat, Pin.NotArray, this); 
        }


        #endregion

        #region Properties

        /// <summary>
        /// Obtains parameter description of constant.
        /// </summary>
        public ParameterDescription ParameterDescription
        {
            get
            {
                if (name == null) return null;
                return new ParameterDescription(name, output);
            }
        }

        /// <summary>
        /// Can we reference a constant, e.g. does it have a name.
        /// If we cannot, it is always fixed.
        /// </summary>
        public bool IsReferencable
        {
            get
            {
                return name != null;
            }
        }

        /// <summary>
        /// The name of operation.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// The pin of constant (easy access).
        /// </summary>
        public Pin Pin
        {
            get
            {
                return output;
            }
        }

        #endregion

        #region IOperation Members

        public PinsDescriptor InputDescriptor
        {
            get 
            {
                return PinsDescriptor.Empty;
            }
        }

        public void BindInputs(params Pin[] pins)
        {
            if (pins.Length != 0)
            {
                throw new ArgumentException("Cannot bind non-null input array to constant provider.");
            }
        }

        public Pin[] Inputs
        {
            get
            {
                return new Pin[] { };
            }
        }

        public Pin[] Outputs
        {
            get 
            {
                return new Pin[] { output };
            }
        }

        public IScope Scope
        {
            get
            {
                return scope;
            }
        }

        #endregion

        #region ICommentable Members

        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                if (scope != null) scope.SignalChanged();
                comment = value;
            }
        }

        #endregion

        #region IBaseOperation Members

        public ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {

            // We checked if it is fixed on "constant" level (not named).
            if(!IsReferencable)
            {
                return new ShaderCompiler.Operand[] {
                    compiler.CreateFixed(output.Format, output.Size, value)
                };
            }


            // We first check if this parameter is fixed.          
            if (parameters.IsParameterFixed(name))
            {
                // If it is fixed, we create a fixed constant.
                switch (output.Format)
                {
                    
                    case PinFormat.Texture1D:
                    case PinFormat.Texture1DArray:
                    case PinFormat.Texture2D:
                    case PinFormat.Texture2DArray:
                    case PinFormat.TextureCube:
                    case PinFormat.Texture3D:
                    case PinFormat.Sampler:
                    case PinFormat.BufferTexture:
                    case PinFormat.Interface:
                        // We return "unresolved" parameter, it must be obtained directly.
                        return new ShaderCompiler.Operand[1] { null };
                    default: 
                        break;
                   
                }

                return new ShaderCompiler.Operand[]
                {
                    compiler.CreateFixed(output.Format, output.Size, parameters[name])
                };

            }

            // We locate it.
            uint bufferID = 0;
            uint offset = 0;
            ConstantBufferLayout[] layouts = parameters.ConstantBuffers;
            foreach(ConstantBufferLayout layout in layouts)
            {
                if(layout.TryGetOffset(name, out offset))
                {
                    break;
                }
                bufferID++;
            }

            // We must emit code.
            return new ShaderCompiler.Operand[]{
                compiler.CreateConstant(output.Format, output.Size, bufferID, offset)
            };

            
        }

        #endregion

        #region IEquatable<IOperation> Members

        public bool Equals(IOperation other)
        {
            return object.ReferenceEquals(this, other);
        }

        #endregion
    }
}
