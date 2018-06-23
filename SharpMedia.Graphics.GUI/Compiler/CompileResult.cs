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

namespace SharpMedia.Graphics.GUI.Compiler
{

    /// <summary>
    /// An error.
    /// </summary>
    public sealed class Message
    {

        
    }

    /// <summary>
    /// Importing results.
    /// </summary>
    public sealed class CompileResult
    {
        /// <summary>
        /// Was compilation successful.
        /// </summary>
        public bool IsSuccessful;

        /// <summary>
        /// The on-the-fly assembly, if this option was tagged.
        /// </summary>
        public Assembly OnFlyAssembly;

        /// <summary>
        /// Errors.
        /// </summary>
        public Message[] Errors;

        /// <summary>
        /// Warnings.
        /// </summary>
        public Message[] Warnings; 

    }
}
