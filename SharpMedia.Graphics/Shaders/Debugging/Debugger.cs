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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Shaders.Debugging
{

    /// <summary>
    /// A debugger operates on one ShaderCode. You can insert breakpoints, inspects certain values
    /// and display "values" (possibly transformed) on display. Debugger is interactive and
    /// can be used on every valid renderer configuration. Result of debugger is multipass
    /// rendering solution (each solution is from beginning until the breakpoint/check).
    /// </summary>
    public class Debugger
    {
        #region Private Members
        ShaderCode dag;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a debugger based on ShaderCode. ShaderCode must not change during debugging.
        /// </summary>
        /// <param name="scope">The ShaderCode used to debug.</param>
        public Debugger([NotNull] ShaderCode dag)
        {
            // Forces ShaderCode to be immutable.
            dag.Immutable = true;
            this.dag = dag;
        }

        #endregion

        #region BreakPoints

        // Must allow breakpoints to be placed in ShaderCode

        #endregion

        #region MultiChecks

        // Must allow inspecting pins at for value ranges; e.g. must be in range [0,1].

        #endregion

        #region Visualization

        // Can visualize data using RGBA and MRT channels.

        #endregion

        #region Data Analysis

        // Analyses data by GPU/CPU and writes results somewhere or displays them.

        #endregion
    }
}
