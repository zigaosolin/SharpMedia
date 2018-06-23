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

namespace SharpMedia.Graphics.Vector.Fills
{

    /// <summary>
    /// A fill element, select colour of pixel. It also accept IFill interfaces.
    /// </summary>
    public class FillElementOperation : IOperation
    {
        public const uint MaxCustomAttributes = 2;

        #region Static Private Members
        static PinsDescriptor inputDesc = new PinsDescriptor(
            new PinDescriptor(PinFormat.Floatx2, "Position"),
            new PinDescriptor(PinFormat.Floatx2, "Texture Coordinate"),
            new PinDescriptor(PinFormat.Floatx4, "Custom Attribute 1"),
            new PinDescriptor(PinFormat.Integer, "Fill ID"),
            new PinDescriptor(PinFormat.Interface, "Fill Interfaces", Pin.DynamicArray));

        #endregion

        #region Private Members
        Pin[] inputs = null;
        Pin[] outputs = null;
        string comment = string.Empty;
        IScope scope;
        #endregion

        #region IOperation Members

        public PinsDescriptor InputDescriptor
        {
            get { return inputDesc; }
        }

        public void BindInputs(params Pin[] pins)
        {
            // We validate against specifications.
            inputDesc.ValidateThrow(pins);

            // Obtains the scope.
            ScopeHelper.GetScope(ref scope, pins);

            inputs = pins;

            // We create the output pin.
            outputs = new Pin[] { new Pin(PinFormat.Floatx4, Pin.NotArray, this) };
        }

        public Pin[] Inputs
        {
            get { return inputs; }
        }

        public Pin[] Outputs
        {
            get { return outputs; }
        }

        public IScope Scope
        {
            get { return scope; }
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
                comment = value;
            }
        }

        #endregion

        #region IBaseOperation Members

        public ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref SharpMedia.Graphics.Shaders.Operations.DualShareContext shareContext)
        {
            // We obtain interfaces (CPU resolve).
            object[] interfaces = InterfaceHelper.ResolveInterfaceArray(inputs[4], parameters);

            // We first register all our "constants" (for fill IDs).
            ShaderCompiler.Operand[] integerConstants = new ShaderCompiler.Operand[interfaces.Length];
            for (int i = 0; i < integerConstants.Length; i++)
            {
                integerConstants[i] = compiler.CreateFixed(PinFormat.Integer, Pin.NotArray, i);
            }

            // We pack out "attributes".
            ShaderCompiler.Operand[] attributes = new ShaderCompiler.Operand[] { operands[2] };

            // We create output colour constant.
            ShaderCompiler.Operand outColour = compiler.CreateTemporary(PinFormat.Floatx4, Pin.NotArray);

            // We switch
            compiler.BeginSwitch(operands[3]);

            // We go through all interfaces.
            for (int i = 0; i < interfaces.Length; i++)
            {
                compiler.BeginCase(integerConstants[i]);

                // We cast to fill.
                IFill fill = interfaces[i] as IFill;

                ShaderCompiler.Operand[] constants = 
                    InterfaceHelper.RegisterInterfaceConstants(compiler, 
                    string.Format("Fills[{0}]", i), parameters, fill);

                fill.Compile(compiler, operands[0], operands[1], attributes, null, constants, outColour);

                compiler.EndCase();
            }

            // We also add "default" handler.
            compiler.BeginDefault();
            compiler.Mov(compiler.CreateFixed(PinFormat.Floatx4, Pin.NotArray, new Math.Vector4f(1.0f, 0.0f, 0.0f, 1.0f)),
                         outColour);
            compiler.EndCase();

            // And end switch.
            compiler.EndSwitch();

            return new ShaderCompiler.Operand[] { outColour };
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
