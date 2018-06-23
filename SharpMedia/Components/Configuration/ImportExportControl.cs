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

namespace SharpMedia.Components.Configuration
{
    /// <summary>
    /// How ImportExportControl implementations resolve
    /// dependencies
    /// </summary>
    public enum ImportPolicy
    {
        /// <summary>
        /// All of the available providers are checked
        /// </summary>
        Full,

        /// <summary>
        /// Only the parent process is checked
        /// </summary>
        ParentOnly
    }

    /// <summary>
    /// When exporting a component, defines the scope of availability
    /// </summary>
    public enum ExportScope
    {
        /// <summary>
        /// Only to the originating process
        /// </summary>
        Process,

        /// <summary>
        /// The parent process
        /// </summary>
        Parent,

        /// <summary>
        /// The machine it is running on
        /// </summary>
        Machine,

        /// <summary>
        /// Cluster of machines that are sharing components
        /// </summary>
        /// <remarks>Component forwarding over machine boundaries may be
        /// subject to additional rules and checks regarding marshalling</remarks>
        Cluster
    }

    /// <summary>
    /// Behaviour taken by the IEC when a host of an exported component
    /// exits
    /// </summary>
    public enum ProcessExitBehaviour
    {
        /// <summary>
        /// All of the clients retain the access to the component, even though the
        /// process exited
        /// </summary>
        Retain,

        /// <summary>
        /// Throws a ComponentHostExitedException on methods invoked by clients
        /// on the component.
        /// </summary>
        /// <remarks>Future</remarks>
        Throw,

        /// <summary>
        /// Monitors all objects spawned by the component, and wraps them as well,
        /// so that when the process exits, ALL of the objects created by the component
        /// throw ComponentHostExitedException exceptions.
        /// </summary>
        /// <remarks>Future</remarks>
        ThrowAndMonitor
    }

    /// <summary>
    /// Controls import and export of controls
    /// </summary>
    public interface ImportExportControl
    {
        /// <summary>
        /// The import policy of the IEC
        /// </summary>
        ImportPolicy ImportPolicy { get; set;}

        /// <summary>
        /// Registers a component provider for exporting
        /// </summary>
        /// <param name="provider">Component provider</param>
        /// <param name="scope">Scope of registration</param>
        /// <param name="onExit">Behaviour when the host process exits</param>
        void Register(IComponentProvider provider, ExportScope scope, ProcessExitBehaviour onExit);
    }
}
