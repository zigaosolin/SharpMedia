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
    /// Dual share context.
    /// </summary>
    /// <remarks>Enables the operations to communicate "behind the scene".</remarks>
    public class DualShareContext
    {
        #region Private Members
        IOperation destinationOperation;
        ShaderCompiler.Operand[] operands;
        #endregion

        #region Public Membes

        public DualShareContext(ShaderCompiler.Operand[] operands, IOperation dst)
        {
            destinationOperation = dst;
            this.operands = operands;
        }

        /// <summary>
        /// Returns destination operation.
        /// </summary>
        public IOperation DestinationOperation
        {
            get
            {
                return destinationOperation;
            }
        }

        /// <summary>
        /// Returns operands.
        /// </summary>
        public ShaderCompiler.Operand[] Operands
        {
            get
            {
                return operands;
            }
        }

        #endregion
    }

    /// <summary>
    /// Base operation of shader. It must only support compiling.
    /// </summary>
    public interface IBaseOperation
    {
        /// <summary>
        /// Operation describes itself to shader compiler. System must garantie that
        /// operations are called in right order. Validation step is also done here
        /// (arguments have correct types, uniforms exist).
        /// </summary>
        /// <param name="compiler">The shader compiler.</param>
        /// <param name="operands">Input operands, indexes in the array should correspond with
        /// pin indices. Those operands are provided by other operations.</param>
        /// <param name="dualOperands">Used only by dual operation to communicate.</param>
        /// <param name="parameters">Fixed paramters, may be empty but must be non-null.</param>
        ShaderCompiler.Operand[] Compile([NotNull] ShaderCompiler compiler, [NotNull] ShaderCompiler.Operand[] operands,
            [NotNull] FixedShaderParameters parameters, ref DualShareContext shareContext);
    }

    /// <summary>
    /// An operation of shader that allows argument accessing.
    /// </summary>
    public interface IOperation : ICommentable, IBaseOperation, IEquatable<IOperation>
    {
        /// <summary>
        /// Description of all inputs this operacion needs.
        /// </summary>
        /// <remarks>If the specifications cannot be represented by descriptor, null is valid
        /// value. In this case, BindInputs throws if something is wrong. This is not recommended
        /// for in-editor operations (cannot validate shaders at construction time).</remarks>
        PinsDescriptor InputDescriptor
        {
            get;
        }

        /// <summary>
        /// Binds all pins. The order is defined by pin descriptor. If this method succeeds, outputs
        /// are available. Re-binding invalidates all output pins of this operation.
        /// </summary>
        /// <param name="pins">All pins.</param>
        void BindInputs([NotNull] params Pin[] pins);

        /// <summary>
        /// Inputs bound.
        /// </summary>
        Pin[] Inputs
        {
            get;
        }

        /// <summary>
        /// Outputs, available when all pins are bound.
        /// </summary>
        Pin[] Outputs
        {
            get;
        }

        /// <summary>
        /// The scope of operation.
        /// </summary>
        IScope Scope
        {
            get;
        }
    }

    /// <summary>
    /// An operation implementation based on interface. Provides helpers and common code.
    /// </summary>
    public abstract class Operation : IOperation
    {
        #region Protected Members
        protected string comment = string.Empty;
        protected Pin[] inputs;
        protected Pin[] outputs;
        protected IScope scope;
        #endregion

        #region Abstract Methods

        /// <summary>
        /// Should create outputs based on inputs.
        /// </summary>
        /// <returns></returns>
        protected abstract Pin[] CreateOutputs();

        #endregion

        #region IOperation Members

        public abstract PinsDescriptor InputDescriptor
        {
            get;
        }

        public void BindInputs(params Pin[] pins)
        {
            // Validates pins and does scoping.
            if (InputDescriptor != null)
            {
                InputDescriptor.ValidateThrow(pins);
            }
            ScopeHelper.GetScope(ref scope, pins);

            // Invalidate output.
            // TODO:

            inputs = pins;
            outputs = CreateOutputs();
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

        public abstract ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, 
            FixedShaderParameters parameters, ref DualShareContext shareContext);

        #endregion

        #region IEquatable<IOperation> Members

        public bool Equals(IOperation other)
        {
            return object.ReferenceEquals(this, other);
        }

        #endregion
    }

    /// <summary>
    /// A dual operation is two-sided operation. This means it has an input and output.
    /// It is actually only two-way operation communicator.
    /// </summary>
    /// <remarks>Dual operations are required for loops.</remarks>
    public interface IDualOperation
    {
        /// <summary>
        /// The input operation.
        /// </summary>
        IOperation InputOperation { get; }

        /// <summary>
        /// The output operation.
        /// </summary>
        IOperation OutputOperation { get; }
    }

}
