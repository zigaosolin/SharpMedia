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
    /// Method operation hides "internal", possibly shared code. It has it's own caching,
    /// much like shader code.
    /// </summary>
    /// <remarks>In future, method may be "linked" operation, meaning that binding
    /// (not inlining) will be done. This is why it is disposable.</remarks>
    public sealed class MethodOperation : IScope, IOperation, IDisposable
    {
        #region IScope Members

        public void SignalChanged()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsInScope(IScope other)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IScope ParentScope
        {
            get { throw new Exception("The method or operation is not implemented."); }
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

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
