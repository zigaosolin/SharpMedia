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
using SharpMedia.Components.Applications;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Tools
{
    /// <summary>
    /// Generates automatic user interface based on tool description.
    /// </summary>
    /// <remarks>Automatic UI is a component.</remarks>
    public interface IAutomaticUI
    {
        /// <summary>
        /// Runs the UI where data is collected.
        /// </summary>
        /// <param name="toolParameters">The parameters generated from tool description. Those must be filled.</param>
        /// <param name="arguments">The additional arguments (unprocessed). They can be used by provider to set some
        /// parameters.</param>
        /// <param name="outputPath">The output path. If null, it will be run, otherwise it will be serialized to path.</param>
        /// <returns>Unprocessed arguments that will be sent to the tool.</returns>
        bool Run(Type toolName, Parameters.IToolParameter[] toolParameters);
    }
}
