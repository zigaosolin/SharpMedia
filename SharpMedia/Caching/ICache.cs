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

namespace SharpMedia.Caching
{
    /// <summary>
    /// A cache, given a key. Cache does not implement locking.
    /// </summary>
    public interface ICache<T> 
        where T : IComparable<T>
    {

        /// <summary>
        /// Adds a new (unique) data to cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="cacheable">The cachable.</param>
        /// <returns>False if the key already exists.</returns>
        bool Add(T key, [NotNull] ICacheable cacheable);

        /// <summary>
        /// Relinks a resource with key to new key.
        /// </summary>
        /// <param name="key">The previous key.</param>
        /// <param name="newKey">The new key.</param>
        /// <param name="cachable">The cachable.</param>
        /// <returns>Was it relinked (it is not relinked if not found).</returns>
        void ReLink(T key, T newKey, [NotNull] ICacheable cachable);

        /// <summary>
        /// Touches the resource with that key.
        /// </summary>
        /// <param name="cacheable">The cacheable, can be reinserted if fallen out of cache.</param>
        void Touch([NotNull] ICacheable cacheable);

        /// <summary>
        /// Evicts a certain cacheable.
        /// </summary>
        /// <param name="key">The key.</param>
        void Evict([NotNull] T key);

        /// <summary>
        /// Evicts all cacheables.
        /// </summary>
        void EvictAll();

        /// <summary>
        /// Finds a cacheable, given a key.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>Element, if found, otherwise null.</returns>
        ICacheable Find(T key);

        /// <summary>
        /// Finds a cacheable, given a key and touches it if found.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>Element, if found, otherwise null.</returns>
        ICacheable FindAndTouch(T key);

        /// <summary>
        /// Updates the cache, returning number of objects disposed.
        /// </summary>
        uint Update();

        /// <summary>
        /// Number of elements in cache.
        /// </summary>
        uint ItemCount { get; }

        /// <summary>
        /// The maximum cache size in bytes.
        /// </summary>
        ulong MaxCacheSize { get; set; }
    }
}
