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
using SharpMedia.Components.Database;

namespace SharpMedia.Components.Installation
{
    /// <summary>
    /// An installation command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes in an installation environment.
        /// May throw exceptions for the installation to roll back
        /// </summary>
        /// <param name="env">Installation environment</param>
        void Execute(InstallEnvironment env);
    }

    /// <summary>
    /// Thrown from a command to indicate that the full installation must abort,
    /// not only the task that it was a part of
    /// </summary>
    [Serializable]
    public class AbortInstallationException : Exception
    {
        public AbortInstallationException() { }
        public AbortInstallationException(string message) : base(message) { }
        public AbortInstallationException(string message, Exception inner) : base(message, inner) { }
        protected AbortInstallationException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}
