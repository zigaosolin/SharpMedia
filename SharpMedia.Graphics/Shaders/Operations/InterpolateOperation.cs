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
    /// An interpolation operation, can interpolate between 2 or more points (depends on mode).
    /// </summary>
    [Serializable]
    public class InterpolateOperation : IOperation
    {
        #region Private Members
        InterpolationMode mode;
        #endregion

        #region Constructors

        public InterpolateOperation(InterpolationMode mode)
        {
            this.mode = mode;
        }

        #endregion

        #region IOperation Members

        public PinsDescriptor InputDescriptor
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void BindInputs(params Pin[] pins)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Pin[] Inputs
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Pin[] Outputs
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IScope Scope
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

        public ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
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
