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
using System.Reflection;

namespace SharpMedia.Graphics.Shaders.Assembler
{

    /// <summary>
    /// This exception is thrown if delegate does not match the specifications to be compiled
    /// to assembled to ShaderCode.
    /// </summary>
    [Serializable]
    public class IncompatibleMethodException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncompatibleMethodException"/> class.
        /// </summary>
        public IncompatibleMethodException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompatibleMethodException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public IncompatibleMethodException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompatibleMethodException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public IncompatibleMethodException(string message, Exception inner) 
            : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompatibleMethodException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected IncompatibleMethodException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Shader assembler can assembly code (with a lot of restristions) from IL code, that is
    /// from method bodies and their headers.
    /// </summary>
    public static class ShaderAssembler
    {

        /// <summary>
        /// Assembles the delegate.
        /// </summary>
        /// <param name="del">The delegate.</param>
        /// <returns>ShaderCode, that can be compiled to shader.</returns>
        public static ShaderCode Assemble(Delegate del)
        {
            // We allow only static methods.
            if (del.Target != null)
            {
                throw new IncompatibleMethodException("Cannot assemble a non-static method.");
            }

            return Assemble(del.Method);
        }

        /// <summary>
        /// Assembles a ShaderCode that can be compiled to shader. Delegate must meet conditions.
        /// </summary>
        /// <param name="del">The delegate.</param>
        /// <returns>Valid ShaderCode object.</returns>
        public static ShaderCode Assemble(MethodInfo info)
        {
            MethodBody b = info.GetMethodBody();
            
            
            throw new NotImplementedException();
        }

    }
}
