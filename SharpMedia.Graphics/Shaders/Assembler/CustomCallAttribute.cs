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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Shaders.Assembler
{
    /// <summary>
    /// CustomCall can be applied to static methods that are in place of certain (custom) operation.
    /// It is most usable in conjuction with custom interface operations.
    /// 
    /// Custom operation must implement default constructor.
    /// </summary>
    /// <remarks>
    /// The best practice is to put a static method into the operation itself. The static method must
    /// implement CPU emulation of the same operation on GPU.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CustomCallAttribute : GPUCallableAttribute
    {
        #region Private Members
        Type operationType;
        #endregion

        #region Public Members


        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCallAttribute"/> class.
        /// </summary>
        /// <param name="operationType">Type of the operation.</param>
        public CustomCallAttribute([NotNull] Type operationType)
        {
            if (!Common.IsTypeSameOrDerived(typeof(IOperation), operationType))
            {
                throw new ArgumentException("The replacing operation is not of type " + typeof(IOperation).ToString());
            }

            this.operationType = operationType;
        }

        /// <summary>
        /// Gets the type of the operation.
        /// </summary>
        /// <value>The type of the operation.</value>
        public Type OperationType
        {
            get { return operationType; }
        }

        #endregion
    }
}
