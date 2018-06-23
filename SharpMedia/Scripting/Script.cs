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

namespace SharpMedia.Scripting
{
    /// <summary>
    /// A script type.
    /// </summary>
    public enum ScriptType
    {
        /// <summary>
        /// A script is a single method to be executed.
        /// </summary>
        Method,

        /// <summary>
        /// A script defines a class (e.g. can construct objects).
        /// </summary>
        Class,

        /// <summary>
        /// A script defines multiple classes.
        /// </summary>
        Library
    }

    /// <summary>
    /// A script to be executed.
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Obtains script's type.
        /// </summary>
        ScriptType ScriptType { get; }

        /// <summary>
        /// Obtains the object type this script defines.
        /// </summary>
        /// <remarks>If type is Method, this is the static class 
        /// (for most languages) in which method is defined. If it is assembly, null is returned.</remarks>
        Type ObjectType { get; }

        /// <summary>
        /// Obtains the method info defined by the script.
        /// </summary>
        /// <remarks>If script type is Class or Assembly, null is returned.</remarks>
        MethodInfo MethodInfo { get; }

        /// <summary>
        /// Obtains the assembly that was compiled as a script.
        /// </summary>
        Assembly AssemblyInfo { get; }
    }
}
