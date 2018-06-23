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

namespace SharpMedia.Resources
{
    /// <summary>
    /// A resource loader is a interface that defines a service's functionality.
    /// </summary>
    public interface IResourceLoader
    {
        /// <summary>
        /// The resource type this loader can handle.
        /// </summary>
        Type ResourceType
        {
            get;
        }

        /// <summary>
        /// A Loading policy specifies the actions that are taken when loading.
        /// </summary>
        LoadingPolicy Policy
        {
            get;
            [param:NotNull]
            set;
        }

        /// <summary>
        /// Adds a resource reference, binding it to resource.
        /// </summary>
        /// <param name="reference">The reference.</param>
        void AddRef([NotNull] IReference reference);

        /// <summary>
        /// Removes the resource reference, binding it from resource.
        /// </summary>
        /// <param name="reference">The reference.</param>
        void RemoveRef([NotNull] IReference reference);

        /// <summary>
        /// Adds a one-way loader target service.
        /// </summary>
        /// <param name="loader">The loader that can be asked for resources.</param>
        /// <param name="speed">The bandwidth, measured in kB/s.</param>
        /// <param name="computer">The computer name where loader resides. may be the same computer.</param>
        void AddLoader([NotNull] IResourceLoader loader, float speed, [NotEmpty] string computer);

        /// <summary>
        /// Removes the resource loader.
        /// </summary>
        /// <param name="loader">The loader.</param>
        void RemoveLoader([NotNull] IResourceLoader loader);

        #region Statistics

        /// <summary>
        /// Number of resources served.
        /// </summary>
        uint ServedResources
        {
            get;
        }

        /// <summary>
        /// Number of resources loaded.
        /// </summary>
        uint LoadedResources
        {
            get;
        }

        #endregion

    }
}
