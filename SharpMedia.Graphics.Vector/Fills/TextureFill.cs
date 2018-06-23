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
using SharpMedia.Graphics.States;
using SharpMedia.AspectOriented;
using SharpMedia.Graphics.Shaders;

namespace SharpMedia.Graphics.Vector.Fills
{
    /// <summary>
    /// A texture fill. Texture coordinates are used to sample pixels.
    /// </summary>
    /// <remarks>The texture view is not owned by the fill and must be disposed seperatelly.</remarks>
    public class TextureFill : IFill
    {
        #region Private Members
        object syncRoot = new object();
        TextureView texture;
        SamplerState samplerState;
        #endregion

        #region Properties

        /// <summary>
        /// The texture that is used.
        /// </summary>
        public TextureView Texture
        {
            get { return texture; }
            [param: NotNull]
            set 
            {
                lock (syncRoot)
                {
                    if (value.ViewType != PinFormat.Texture2D)
                    {
                        throw new ArgumentException("Only Texture2D views are supported.");
                    }
                    texture = value;
                }
            }
        }

        /// <summary>
        /// Filter when sampling.
        /// </summary>
        /// <remarks>May not return the same object by ref (but by value) because state is interned.</remarks>
        public SamplerState SamplerState
        {
            get { return samplerState; }
            [param: NotNull]
            set
            {
                lock (syncRoot)
                {
                    samplerState = value;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new texture fill.
        /// </summary>
        /// <param name="texture"></param>
        public TextureFill([NotNull] TextureView texture)
        {
            Texture = texture;
            SamplerState = new SamplerState();
            SamplerState.AddressU = AddressMode.Wrap;
            SamplerState.AddressV = AddressMode.Wrap;
        }

        /// <summary>
        /// Constructs a new texture fill.
        /// </summary>
        /// <param name="texture"></param>
        public TextureFill([NotNull] TextureView texture, SamplerState state)
        {
            Texture = texture;
            SamplerState = state;
        }

        #endregion

        #region IFill Members

        public object[] ParameterValues
        {
            get 
            {
                lock (syncRoot)
                {
                    object[] data = new object[2];

                    // Make sure sampler state is boundable.
                    samplerState = StateManager.Intern(samplerState);

                    data[0] = texture;
                    data[1] = samplerState;

                    return data;
                }
            }
        }

        public void Compile(ShaderCompiler compiler, ShaderCompiler.Operand position, ShaderCompiler.Operand texCoord, ShaderCompiler.Operand[] attributes,
            ShaderCompiler.Operand borderDistance, ShaderCompiler.Operand[] constants, ShaderCompiler.Operand outputColour)
        {
            ShaderCompiler.Operand texture = constants[0];
            ShaderCompiler.Operand sampler = constants[1];

            // We perform a sampling.
            compiler.Sample(sampler, texture, texCoord, outputColour);
            
        }

        public bool IsInstanceDependant
        {
            get
            {
                return true;
            }
        }

        public uint CustomAttributeCount
        {
            get { return 0; }
        }

        public SharpMedia.Math.Vector4f CalcCustomAttribute(uint id, object shape, SharpMedia.Math.Vector2f pos)
        {
            throw new InvalidOperationException("Fill has no custom attributes.");
        }

        #endregion

        #region IInterface Members

        public Type TargetOperationType
        {
            get { return typeof(FillElementOperation); }
        }

        public ParameterDescription[] AdditionalParameters
        {
            get 
            {
                return new ParameterDescription[] {
                    new ParameterDescription("Texture", new Pin(PinFormat.Texture2D, PinFormat.Floatx4, Pin.NotArray, null)),
                    new ParameterDescription("Sampler", new Pin(PinFormat.Sampler, Pin.NotArray, null))
                };

            }
        }

        #endregion

        #region IEquatable<IFill> Members

        public bool Equals(IFill other)
        {
            if (other is TextureFill)
            {
                TextureFill fill = (TextureFill)other;
                if (!fill.SamplerState.Equals(SamplerState)) return false;
                if (fill.texture != texture) return false;
                return true;
            }
            return false;
        }

        #endregion
    }
}
