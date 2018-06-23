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

namespace SharpMedia.Graphics.GUI.Compiler
{

    /// <summary>
    /// Importing options.
    /// </summary>
    public sealed class CompileOptions
    {
        /// <summary>
        /// Generates all classes on the fly; otherwise, code is generated.
        /// </summary>
        /// <remarks>This implies that all code is presented in XML.</remarks>
        public bool GenerateOnFly = false;

        /// <summary>
        /// Output file to be created.
        /// </summary>
        public string OutputFile = null;

        /// <summary>
        /// Should we use and emit configuration instead of hardcoding data.
        /// </summary>
        public string ConfigurationFile = null;

        /// <summary>
        /// If additional namespace for widgets are provided, otherwise only built-in are used.
        /// </summary>
        public SortedDictionary<string, string> AdditionalNamespacesMapper;



    }
}
