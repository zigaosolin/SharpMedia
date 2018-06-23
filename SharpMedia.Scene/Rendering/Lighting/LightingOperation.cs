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

namespace SharpMedia.Scene.Lighting
{
    /// <summary>
    /// A lighting operation.
    /// </summary>
    /// <remarks>Needs ILight[], IColourCombiner and IBDRF interface.</remarks>
    public class LightingOperation : IOperation
    {
        #region IOperation Members

        public SharpMedia.Graphics.Shaders.PinsDescriptor InputDescriptor
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void BindInputs(params SharpMedia.Graphics.Shaders.Pin[] pins)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public SharpMedia.Graphics.Shaders.Pin[] Inputs
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public SharpMedia.Graphics.Shaders.Pin[] Outputs
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public SharpMedia.Graphics.Shaders.IScope Scope
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region ICommentable Members

        public string Comment
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region IBaseOperation Members

        public SharpMedia.Graphics.Shaders.ShaderCompiler.Operand[] Compile(SharpMedia.Graphics.Shaders.ShaderCompiler compiler, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand[] operands, SharpMedia.Graphics.Shaders.FixedShaderParameters parameters, ref SharpMedia.Graphics.Shaders.Operations.DualShareContext shareContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEquatable<IOperation> Members

        public bool Equals(IOperation other)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
