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
    /// A resource can be referenced and cached.
    /// </summary>
    public interface IResource : Caching.ICacheable
    {
        /// <summary>
        /// Event is fired on writing.
        /// </summary>
        event Action<IResource> OnWritten;

        /// <summary>
        /// Full address of resource.
        /// </summary>
        ResourceAddress Address
        {
            get;
        }

        /// <summary>
        /// Is the resource disposed.
        /// </summary>
        bool IsDisposed
        {
            get;
        }

        /// <summary>
        /// Occurs when resource is disposed.
        /// </summary>
        event Action<IResource> Disposed;
    }

    

}
