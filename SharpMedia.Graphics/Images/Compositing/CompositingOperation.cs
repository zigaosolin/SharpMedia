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
using SharpMedia.Graphics.Shaders.Operations;
using SharpMedia.Graphics.Shaders;

namespace SharpMedia.Graphics.Images.Compositing
{

    /// <summary>
    /// A compositing operation in shader.
    /// </summary>
    /// <remarks>The compositing framework uses ordered pixel providers (DFS order) to identify
    /// shader. Otherwise, only root (that is last) provider is used for calculations (and this one
    /// transverses other providers).</remarks>
    internal sealed class CompositingOperation : Operation
    {
        #region Private Members
        static PinsDescriptor inputDesc = new PinsDescriptor(
            new PinDescriptor(PinFormat.Floatx2,   "Pixel position (absolute)"),
            new PinDescriptor(PinFormat.Interface, "Pixel Processors", Pin.DynamicArray));

        #endregion

        #region Overrides

        protected override Pin[] CreateOutputs()
        {
            return new Pin[] { new Pin(PinFormat.Floatx4, Pin.NotArray, this) };
        }

        public override PinsDescriptor InputDescriptor
        {
            get 
            {
                return inputDesc;
            }
        }

        public override ShaderCompiler.Operand[] Compile(
            ShaderCompiler compiler,  ShaderCompiler.Operand[] operands, 
            FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            // We resolve interface arrays.
            object[] interfaces = InterfaceHelper.ResolveInterfaceArray(inputs[1], parameters);

            Dictionary<ICompositeInterface, ShaderCompiler.Operand[]> constants 
                = new Dictionary<ICompositeInterface, ShaderCompiler.Operand[]>(interfaces.Length);

            // We extract interface parameters and register them.
            for(int i = 0; i < interfaces.Length; i++)
            {
                ICompositeInterface provider = interfaces[i] as ICompositeInterface;

                // We now register it.
                constants.Add(provider, InterfaceHelper.RegisterInterfaceConstants(compiler, 
                    string.Format("Composite[{0}]", i), parameters, interfaces[i] as ICompositeInterface));
            }

            // We only need to execute last element.
            ICompositeInterface pixelProvider = interfaces[interfaces.Length - 1] as ICompositeInterface;

            // We execute it.
            return new ShaderCompiler.Operand[] {
                pixelProvider.GetPixel(compiler, operands[0], constants)
            };
        }

        #endregion
    }
}
