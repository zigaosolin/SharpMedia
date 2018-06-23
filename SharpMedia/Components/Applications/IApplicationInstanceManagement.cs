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
using SharpMedia.Components.Configuration;

namespace SharpMedia.Components
{
    /// <summary>
    /// A directory containing information about all of the installed applications
    /// </summary>
    public interface IApplicationInstances
    {
        /// <summary>
        /// Find all running instances of a specified application
        /// </summary>
        IApplicationInstance[] FindInstances(Guid applicationId);

        /// <summary>
        /// Find a specific application instance by its id
        /// </summary>
        IApplicationInstance   FindInstance (Guid instanceId);

        /// <summary>
        /// All instances
        /// </summary>
        IApplicationInstance[] Instances { get; }

        /// <summary>
        /// Run an application found at the path
        /// </summary>
        IApplicationInstance   Run(string path, string verb, params string[] args);

        /// <summary>
        /// Run an application found at the path with a specified environment 
        /// </summary>
        IApplicationInstance Run(string path, string verb, string[] args, IComponentDirectory environ);

        /// <summary>
        /// Kill an application instance
        /// </summary>
        void Kill(IApplicationInstance app);
    }
}
