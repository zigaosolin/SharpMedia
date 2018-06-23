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

namespace SharpMedia.Caching
{


    /// <summary>
    /// A cacheable state.
    /// </summary>
    public enum CacheableState
    {
        /// <summary>
        /// Normal state (in cache).
        /// </summary>
        Normal,

        /// <summary>
        /// Evicted from cache.
        /// </summary>
        Evicted,

        /// <summary>
        /// Disposed cache.
        /// </summary>
        Disposed
    }

    /// <summary>
    /// Each cachable element can be evicted or touched.
    /// </summary>
    public interface ICacheable : IDisposable
    {
        /// <summary>
        /// An element is touched, e.g. used.
        /// </summary>
        /// <remarks>Allows touching only to assemblies with cache controll.</remarks>
        [Linkable(LinkMask.CacheControlling)]
        void Touch();

        /// <summary>
        /// Evicts a cacheable. OnTouch event is invalidated.
        /// </summary>
        [Linkable(LinkMask.CacheControlling)]
        void Evict();

        /// <summary>
        /// The cachable was added to cache. This can be only called
        /// by cache. If item is already cached, exception must be thrown.
        /// </summary>
        [Linkable(LinkMask.CacheControlling)]
        void Cached();

        /// <summary>
        /// The cacheable state.
        /// </summary>
        CacheableState State
        {
            get;
        }

        /// <summary>
        /// An on-touched event subscriptor.
        /// </summary>
        event Action<ICacheable> OnTouch;
    }
}
