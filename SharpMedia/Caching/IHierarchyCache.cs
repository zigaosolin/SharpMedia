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
using System.Collections.ObjectModel;

namespace SharpMedia.Caching
{
    /// <summary>
    /// A hierarchy cache adds a parent-child cache relation. Also, updating parent
    /// cache will trigger child cache updates, possibly with lower frequency (every second call ...)
    /// </summary>
    public interface IHierarchyCache<T> : ICache<T> where T : IComparable<T>
    {
        /// <summary>
        /// The parent cache, can be null.
        /// </summary>
        IHierarchyCache<T> Parent { get; }

        /// <summary>
        /// A read-only children collection, can be empty if leaf node.
        /// </summary>
        ReadOnlyCollection<IHierarchyCache<T>> Children { get; }

    }
}
