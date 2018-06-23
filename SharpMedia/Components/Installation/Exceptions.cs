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

namespace SharpMedia.Components.Installation
{
    /// <summary>
    /// Thrown when a Service is not ready for use
    /// </summary>
    [global::System.Serializable]
    public class ServiceNotReadyException : Exception
    {
        public ServiceNotReadyException() { }
        public ServiceNotReadyException(string message) : base(message) { }
        public ServiceNotReadyException(string message, Exception inner) : base(message, inner) { }
        protected ServiceNotReadyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown when a package format is not supported
    /// </summary>
    [global::System.Serializable]
    public class UnsupportedPackageFormatException : Exception
    {
        public UnsupportedPackageFormatException() : base("Installation service does not support this installation package format") { }
        public UnsupportedPackageFormatException(string message) : base(message) { }
        public UnsupportedPackageFormatException(string message, Exception inner) : base(message, inner) { }
        protected UnsupportedPackageFormatException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown if the package data was not found
    /// </summary>
    [global::System.Serializable]
    public class PackageNotFoundException : Exception
    {
        public PackageNotFoundException() : base("Package data was not found") { }
        public PackageNotFoundException(string message) : base(message) { }
        public PackageNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected PackageNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
