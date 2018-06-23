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
using PostSharp.CodeModel;

namespace SharpMedia.AspectOriented.Framework
{
    /// <summary>
    /// Applies processing to an assembly
    /// </summary>
    public interface IAssemblyProcessor
    {
        /// <summary>
        /// Returns true if the processor is to process an assembly (match by name)
        /// </summary>
        /// <param name="asmName">Assembly name</param>
        bool Match(AssemblyName asmName);

        /// <summary>
        /// Returns true if the processor is to process an assembly (match by value)
        /// </summary>
        /// <param name="assembly">Assembly</param>
        bool Match(IAssembly assembly);

        /// <summary>
        /// Process the assembly
        /// </summary>
        /// <param name="assemblyIn">Input assembly</param>
        /// <param name="assemblyOut">Output assembly</param>
        bool Process(IAssembly assemblyIn, ref IAssembly assemblyOut);
    }
}
