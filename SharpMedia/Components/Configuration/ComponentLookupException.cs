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

namespace SharpMedia.Components.Configuration
{
    /// <summary>
    /// Thrown when component lookup fails
    /// </summary>
    [global::System.Serializable]
    public class ComponentLookupException : Exception
    {
        /// <summary>
        /// Throws the exception when a name is not found
        /// </summary>
        public static void NameNotFound(string componentName)
        {
            throw new ComponentLookupException(String.Format("Component {0} not found by name", componentName));
        }

        /// <summary>
        /// Throws the exception when a tyoe is not found
        /// </summary>
        public static void TypeNotFound(string componentType)
        {
            throw new ComponentLookupException(String.Format("Component of type {0} not found", componentType));
        }

        public ComponentLookupException() { }
        public ComponentLookupException(string message) : base(message) { }
        public ComponentLookupException(string message, Exception inner) : base(message, inner) { }
        protected ComponentLookupException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
