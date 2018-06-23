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

namespace SharpMedia.Resources
{
    /// <summary>
    /// The resource type.
    /// </summary>
    public enum ResourceMixing
    {
        /// <summary>
        /// Resource is not mixed, it exists.
        /// </summary>
        NoMixing,

        /// <summary>
        /// Resource is procedural, created at runtime.
        /// </summary>
        Mixing,

        /// <summary>
        /// Procedural resource with other unmixing dependencies.
        /// </summary>
        MixingWithDependencies
    }



    /// <summary>
    /// IReference is a lightweight resources reference. It allows different kinds
    /// of bindings, such as bindings to procedural, loaded or composed resources.
    /// </summary>
    /// <remarks>
    /// After reference is disposed, it does not track anymore.
    /// </remarks>
    public interface IReference : IDisposable
    {
        /// <summary>
        /// The resource type of this reference.
        /// </summary>
        Type ResourceType
        {
            get;
        }

        /// <summary>
        /// The type of resource that is referenced.
        /// </summary>
        ResourceMixing Mixing
        {
            get;
        }

        /// <summary>
        /// Obtains all locations of resource.
        /// </summary>
        Placement Locations
        {
            get;
        }

        /// <summary>
        /// All addresses of resources.
        /// </summary>
        ResourceAddress[] Addresses
        {
            get;
        }

        /// <summary>
        /// Gets or sets persistency information. Setting new persistency
        /// information makes this reference unaware of other reference on
        /// the same resource.
        /// </summary>
        IPersistencyInfo Persistency
        {
            get;
            set;
        }

        /// <summary>
        /// The reference count.
        /// </summary>
        uint ReferenceCount
        {
            get;
        }

        /// <summary>
        /// Forces the loading into placement.
        /// </summary>
        /// <param name="where">Where to load the resource.</param>
        void ForceLoaded(Placement where);

    }
}
