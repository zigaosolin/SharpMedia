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
using SharpMedia.AspectOriented;

namespace SharpMedia.Security
{
    /// <summary>
    /// Thrown when a capability is not found, and access is denied
    /// </summary>
    [Serializable]
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
        /// <summary>
        /// Constructs a new AccessDenied exception with a default message
        /// </summary>
        public AccessDeniedException()
            : base("Access Denied")
        {
        }
        /// <summary>
        /// Constructs a new AccessDenied exception with an existing inner exception
        /// </summary>
        /// <param name="inner">the inner exception</param>
        public AccessDeniedException(Exception inner)
            : base("Access Denied", inner)
        {
        }
        /// <summary>
        /// Constructs a new AccessDenied exception with a custom message
        /// </summary>
        /// <param name="message">the custom message</param>
        /// <remarks>Prepends "Access Denied: " to the message!</remarks>
        public AccessDeniedException(string message)
            : base("Access Denied: " + message)
        {
        }
    }
    /// <summary>
    /// A security engine interface
    /// </summary>
    /// <remarks></remarks>
    public interface ISecurityEngine
    {
        /// <summary>
        /// Assers whether an object is granted some capability (can do something) to another object.
        /// If the receiving p1 is obvious, it may be null, but that depends on the capability
        /// </summary>
        /// <param name="who">The object that access is being queried for</param>
        /// <param name="what">The capability in question</param>
        /// <param name="toWhom">Object that we are asking for, or null</param>
        /// <returns>True if the object "who" can do "what" to "toWhom", false otherwise</returns>
        /// <exception cref="AccessDenied">If who cannot ask toWhom for capability information</exception>
        bool Can([NotNull] object who, [NotNull] Type what, object toWhom);
        
        /// <summary>
        /// Grants a capability to some object to execute some operations on another object
        /// </summary>
        /// <param name="by">The issuer of the grant</param>
        /// <param name="who">The receiver of the grant</param>
        /// <param name="what">The capability in question</param>
        /// <param name="toWhom">The object or selector that is the target of the capability</param>
        /// <remarks>This statement should be read as: "by" grants "fromWho" to do "what" to "toWhom"</remarks>
        /// <exception cref="AccessDenied"></exception>
        void Grant([NotNull] object by, [NotNull] object who, [NotNull] Type what, object toWhom);

        /// <summary>
        /// Revokes a capability of some object to execute some operations on another object
        /// </summary>
        /// <param name="by">The revoker of the grant</param>
        /// <param name="from">The holder of the existing grant</param>
        /// <param name="what">The capability in question</param>
        /// <param name="toWhom">The object or sleector that is the target of the capability</param>
        /// <remarks>This statement should read as: "by" revokes a grant for "from" to do "what" to "toWhom"</remarks>
        void Revoke([NotNull] object by, [NotNull]object from, [NotNull] Type what, object toWhom);
    }
}
