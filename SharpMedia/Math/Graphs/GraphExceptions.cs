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

namespace SharpMedia.Math.Graphs
{
    /// <summary>
    /// A Graph exception.
    /// </summary>
    [Serializable]
    public class GraphException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphException"/> class.
        /// </summary>
        public GraphException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public GraphException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public GraphException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// 
        /// Initializes a new instance of the <see cref="GraphException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected GraphException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Graph options are not valid.
    /// </summary>
    [Serializable]
    public class InvalidGraphPropertiesException : GraphException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGraphOptionsException"/> class.
        /// </summary>
        public InvalidGraphPropertiesException() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGraphOptionsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidGraphPropertiesException(string message) : base(message) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGraphOptionsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public InvalidGraphPropertiesException(string message, Exception inner) : base(message, inner) { }

        public static InvalidGraphPropertiesException MustNotHave(GraphProperties property)
        {
            return new InvalidGraphPropertiesException("The property " + property.ToString() + " is set but must not exist for this algorithm.");
        }

        public static InvalidGraphPropertiesException MustHave(GraphProperties property)
        {
            return new InvalidGraphPropertiesException("The property " + property.ToString() + " is not set but must exist for this algorithm.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGraphOptionsException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected InvalidGraphPropertiesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
