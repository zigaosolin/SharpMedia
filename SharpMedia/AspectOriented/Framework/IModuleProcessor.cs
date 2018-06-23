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
using PostSharp.CodeModel;

namespace SharpMedia.AspectOriented.Framework
{
    /// <summary>
    /// Applies processing to a module
    /// </summary>
    public interface IModuleProcessor
    {
        /// <summary>
        /// Returns true if the processor is to process a module (match by name)
        /// </summary>
        /// <param name="asmName">Module name</param>
        bool Match(string moduleName);

        /// <summary>
        /// Process the module
        /// </summary>
        /// <param name="moduleIn">Input module</param>
        /// <param name="moduleOut">Output module</param>
        bool Process(IModule moduleIn, ref IModule moduleOut);
    }
}
