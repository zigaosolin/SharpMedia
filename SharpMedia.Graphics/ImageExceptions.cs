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
using System.Runtime.Serialization;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// Thrown when trying to render using invalid state. The validation step
    /// is always performed before rendering.
    /// </summary>
    public class InvalidRenderingStateConfiguration : Exception
    {

        /// <summary>
        /// Default contructor, with default text.
        /// </summary>
        public InvalidRenderingStateConfiguration()
            : base("Invalid state configuration.")
        {
        }

        /// <summary>
        /// Construction with symbolicName.
        /// </summary>
        public InvalidRenderingStateConfiguration(string m)
            : base(m)
        {
        }

        /// <summary>
        /// Construction with symbolicName.
        /// </summary>
        public InvalidRenderingStateConfiguration(string m, Exception inner)
            : base(m, inner)
        {
        }

        /// <summary>
        /// Serialization provider.
        /// </summary>
        protected InvalidRenderingStateConfiguration(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// A texture exception, base class for all texture exceptions.
    /// </summary>
    [Serializable]
    public class ImageException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageException"/> class.
        /// </summary>
        public ImageException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ImageException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public ImageException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected ImageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    

}
