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
    /// The class helps constructing operations. 
    /// </summary>
    public static class InterfaceHelper
    {
        /// <summary>
        /// Resolves an interface by tracking it until it is spawned.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ResolveInterface(Pin pin, FixedShaderParameters parameters)
        {
            if (pin.Owner is ConstantOperation)
            {
                ConstantOperation c = pin.Owner as ConstantOperation;

                // We now extract interface.
                return parameters.GetInterface(c.Name);
            }
            else
            {
                throw new NotSupportedException("Resolving 'operation on interfaces' not yet supported.");
            }
        }

        /// <summary>
        /// Resolve an interface array by tracing until it is spawned.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object[] ResolveInterfaceArray(Pin pin, FixedShaderParameters parameters)
        {
            if (pin.Owner is ConstantOperation)
            {
                ConstantOperation c = pin.Owner as ConstantOperation;

                // We extract interface array.
                return parameters.GetInterfaceArray(c.Name);
            }
            else
            {
                throw new NotSupportedException("Resolving 'operation on interfaces' not yet supported.");
            }
        }

        /// <summary>
        /// Registers parameters of interface.
        /// </summary>
        /// <param name="xname"></param>
        /// <param name="interface"></param>
        /// <returns></returns>
        public static ShaderCompiler.Operand[] RegisterInterfaceConstants(
            ShaderCompiler compiler, string xname, 
            FixedShaderParameters parameters, IInterface @interface)
        {
            // We first extract interface parameters and register them
            ParameterDescription[] descs = @interface.AdditionalParameters;
            ShaderCompiler.Operand[] constants = new ShaderCompiler.Operand[descs.Length];
            for (int j = 0; j < descs.Length; j++)
            {
                ParameterDescription desc = descs[j];

                string name = string.Format("{0}.{1}", xname, desc.Name);

                if (desc.IsFixed)
                {
                    if (PinFormatHelper.IsTexture(desc.Pin.Format))
                    {
                        // We check the id.
                        uint regId = (uint)parameters[name];

                        constants[j] = compiler.CreateTexture(desc.Pin.Format, desc.Pin.TextureFormat, regId);
                    }
                    else if (desc.Pin.Format == PinFormat.Sampler)
                    {
                        uint regId = (uint)parameters[name];

                        constants[j] = compiler.CreateSampler(regId);
                    }
                    else
                    {
                        throw new NotImplementedException("Fixed data not yet implemented.");
                    }
                }
                else
                {
                    uint layoutID;

                    // If it is not fixed, we create constant.
                    uint offset = parameters.GetOffset(name, out layoutID);

                    constants[j] = compiler.CreateConstant(desc.Pin.Format, desc.Pin.Size, layoutID, offset);
                }
            }

            return constants;
        }

        /// <summary>
        /// Applies interface constants to layout. Textures and samplers are appended.
        /// </summary>
        public static void ApplyInterfaceConstants(string name, IInterface @interface, ConstantBufferLayoutBuilder builder, FixedShaderParameters pparams,
                                                   List<TextureView> textures, List<States.SamplerState> samplers, params object[] parameterValues)
        {
            string prefix = name + ".";

            object[] parameters = parameterValues;
            ParameterDescription[] descs = @interface.AdditionalParameters;

            if (parameters.Length != descs.Length)
            {
                throw new Exception("Lengths of descriptor and parameter array incompatible.");
            }

            // We now go through all elements and append them.
            for (int j = 0; j < parameters.Length; j++)
            {
                ParameterDescription desc = descs[j];
                object value = parameters[j];

                switch (desc.Pin.Format)
                {
                    case PinFormat.Integer:
                    case PinFormat.Integerx2:
                    case PinFormat.Integerx3:
                    case PinFormat.Integerx4:
                    case PinFormat.UInteger:
                    case PinFormat.UIntegerx2:
                    case PinFormat.UIntegerx3:
                    case PinFormat.UIntegerx4:
                    case PinFormat.Bool:
                    case PinFormat.Boolx2:
                    case PinFormat.Boolx3:
                    case PinFormat.Boolx4:
                    case PinFormat.SNorm:
                    case PinFormat.SNormx2:
                    case PinFormat.SNormx3:
                    case PinFormat.SNormx4:
                    case PinFormat.UNorm:
                    case PinFormat.UNormx2:
                    case PinFormat.UNormx3:
                    case PinFormat.UNormx4:
                    case PinFormat.Float:
                    case PinFormat.Floatx2:
                    case PinFormat.Floatx3:
                    case PinFormat.Floatx4:
                    case PinFormat.Float2x2:
                    case PinFormat.Float3x3:
                    case PinFormat.Float4x4:
                    case PinFormat.Integer2x2:
                    case PinFormat.Integer3x3:
                    case PinFormat.Integer4x4:
                    case PinFormat.UInteger2x2:
                    case PinFormat.UInteger3x3:
                    case PinFormat.UInteger4x4:
                    case PinFormat.SNorm2x2:
                    case PinFormat.SNorm3x3:
                    case PinFormat.SNorm4x4:
                    case PinFormat.UNorm2x2:
                    case PinFormat.UNorm3x3:
                    case PinFormat.UNorm4x4:
                        builder.AppendElement(prefix + desc.Name, desc.Pin.Format);
                        // TODO: may optimize setting data directly (map/unmap).
                        break;
                    case PinFormat.Texture1D:
                    case PinFormat.Texture1DArray:
                    case PinFormat.Texture2D:
                    case PinFormat.Texture2DArray:
                    case PinFormat.TextureCube:
                    case PinFormat.Texture3D:
                    case PinFormat.BufferTexture:
                        pparams.SetParameter(prefix + desc.Name, (uint)textures.Count);
                        textures.Add(value as TextureView);
                        break;
                    case PinFormat.Sampler:
                        pparams.SetParameter(prefix + desc.Name, (uint)samplers.Count);
                        samplers.Add(value as States.SamplerState);
                        break;
                    default:
                        throw new NotSupportedException("Nested interfaces or some other excotic feature not supported.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="interface"></param>
        /// <param name="parameters"></param>
        public static void FillInterfaceConstants(string name, IInterface @interface, 
            ConstantBufferView constantBuffer, params object[] parameters)
        {
            string prefix = name  + ".";

            ParameterDescription[] descs = @interface.AdditionalParameters;

            // We now go through all elements and append them.
            for (int j = 0; j < parameters.Length; j++)
            {
                if (!descs[j].IsFixed)
                {
                    constantBuffer.SetConstant(prefix + descs[j].Name, parameters[j]);
                }
            }
        }
    }
}
