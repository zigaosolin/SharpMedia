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

namespace SharpMedia.Graphics.Shaders.Operations.Implementation
{
    /// <summary>
    /// While loop input part.
    /// </summary>
    [Serializable]
    internal class WhileInputOperation : Operation
    {
        #region Private Members
        WhileOperation parent;

        #endregion

        #region Public Methods

        public WhileInputOperation(WhileOperation parent)
        {
            this.parent = parent;
        }

        protected override Pin[] CreateOutputs()
        {
            Pin[] outputs = new Pin[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                outputs[i] = inputs[i].Clone(this);
            }

            return outputs;
        }

        public override PinsDescriptor InputDescriptor
        {
            get { return WhileOperation.inputDesc; }
        }

        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            // We prepare operands.
            ShaderCompiler.Operand[] ret = new ShaderCompiler.Operand[operands.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = compiler.CreateTemporary(operands[i].Format, operands[i].ArraySize);
                compiler.Mov(operands[i], ret[i]);
            }

            // We begin while.
            compiler.BeginWhile();

            // We also fill dual share context.
            ShaderCompiler.Operand[] shared = new ShaderCompiler.Operand[ret.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                shared[i] = ret[i];
            }

            shareContext = new DualShareContext(shared, parent.outputOperation);

            // We now go forward.
            return ret;
        }

        #endregion
    }

    /// <summary>
    /// While loop output part.
    /// </summary>
    [Serializable]
    internal class WhileOutputOperation : Operation
    {
        #region Private Members
        WhileOperation parent;
        #endregion

        #region Public Members

        public WhileOutputOperation(WhileOperation parent)
        {
            this.parent = parent;
        }

        protected override Pin[] CreateOutputs()
        {
            // We skip the boolean (always false when loop ended).
            Pin[] ret = new Pin[inputs.Length-1];
            for (int i = 1; i < inputs.Length; i++)
            {
                ret[i] = inputs[i].Clone(this);
            }
            return ret;
        }

        public override PinsDescriptor InputDescriptor
        {
            get
            {
                Pin[] outputs = parent.inputOperation.Outputs;

                // We need data from 1-end.
                PinDescriptor[] ret = new PinDescriptor[outputs.Length - 1];
                for (int i = 1; i < outputs.Length; i++)
                {
                    ret[i - 1] = new PinDescriptor(outputs[i].Format, string.Format("Loop value {0}", i), outputs[i].Size);
                }

                return new PinsDescriptor(ret);
            }
        }

        public override ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands,
            FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            // We first break if input[0] is false.
            compiler.Break(operands[0]);

            // We obtain input from operands and update shared context. We move all
            // operands.
            for (int i = 1; i < operands.Length; i++)
            {
                compiler.Mov(operands[i], shareContext.Operands[i - 1]);
            }

            // We now end loop.
            compiler.EndWhile();

            // We return all values.
            return shareContext.Operands;
        }

        #endregion
    }
}
