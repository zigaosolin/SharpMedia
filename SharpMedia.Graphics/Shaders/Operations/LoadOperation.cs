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
    /// A load operation allows loading data from texture.
    /// </summary>
    [Serializable]
    public class LoadOperation : Operation
    {
        #region Static Members
        static readonly PinsDescriptor inputDesc;

        static LoadOperation()
        {
            PinDescriptor texture = new PinDescriptor(new PinFormat[] { 
                PinFormat.Texture1D, PinFormat.BufferTexture, PinFormat.Texture1DArray, PinFormat.Texture2D, 
                PinFormat.Texture2DArray, PinFormat.Texture3D }, "Texture");
            PinDescriptor addr = new PinDescriptor(new PinFormat[] {
                PinFormat.Integerx2, PinFormat.Integerx3, PinFormat.Integerx4 }, "Address");

            PinDescriptor offset = new PinDescriptor(new PinFormat[]
                { PinFormat.Integer, PinFormat.Integerx2, PinFormat.Integerx3 }, true, "Offset");


            inputDesc = new PinsDescriptor(new IPinRelation[] {
                new PinTextureMipmappedAddressable(texture, addr), 
                new PinTextureAddressable(texture, offset), },
                texture, addr, offset);
        }
        #endregion

        #region Public Members

        public LoadOperation()
        {
        }

        protected override Pin[] CreateOutputs()
        {
            return new Pin[] { new Pin(inputs[0].TextureFormat, Pin.NotArray, this) };
        }

        public override PinsDescriptor InputDescriptor
        {
            get { return inputDesc; }
        }

        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            return new ShaderCompiler.Operand[] {
                compiler.Load(operands[0], operands[1], operands.Length >= 3 ? operands[2] : null)
            };
        }

        #endregion
    }
}
