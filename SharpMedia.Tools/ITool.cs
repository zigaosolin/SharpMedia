using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Tools
{

    /// <summary>
    /// A tool, repeatable task that can be run.
    /// </summary>
    /// <remarks>Tool will be autmatically configured using UIAttributes.</remarks>
    public interface ITool
    {
        /// <summary>
        /// Runs a tool with arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The return code.</returns>
        int Run(params string[] args);

    }
}
