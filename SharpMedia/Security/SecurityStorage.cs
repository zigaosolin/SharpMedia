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

namespace SharpMedia.Security
{
    public class StorageEntry
    {
        Type capability;
        object from;
        object to;

        public bool IncludesReceiver(object obj)
        {
            if (from is Predicate<object>)
            {
                return ((Predicate<object>)from)(obj);
            }
            else return Object.Equals(from, obj);
        }

        public bool IncludesTarget(object obj)
        {
            if (from is Predicate<object>)
            {
                return ((Predicate<object>)to)(obj);
            }
            else return Object.Equals(to, obj);
        }

        public Type Capability { get { return capability; } }

        public StorageEntry(object f, object t, Type cap)
        {
            from = f; to = t; capability = cap;
        }
    }

    /// <summary>
    /// An interface that stores and retrieves capabilities of objects in an abstract manner
    /// </summary>
    public interface ISecurityStorage
    {
        /// <summary>
        /// Finds all grants applying to the specified object
        /// </summary>
        /// <param name="receiver">The object we are looking for</param>
        /// <param name="filter">If not null, only filter and its subclasses are considered</param>
        /// <returns>An iterateable object that yields appropriate storage entries</returns>
        /// <remarks>Whenever possible, set the filter to non-null</remarks>
        IEnumerable<StorageEntry> GrantsApplyingTo(object receiver, Type filter);

        /// <summary>
        /// Finds all grants applying to the specified object and the receiver to
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="to"></param>
        /// <param name="filter">If not null, only filter and its subclasses are considered</param>
        /// <returns>An iterateable object that yields appropriate storage entries</returns>
        /// <remarks>Whenever possible, set the filter to non-null</remarks>
        IEnumerable<StorageEntry> GrantsAppliyingTo(object receiver, object to, Type filter);

        /// <summary>
        /// Adds an entry
        /// </summary>
        /// <param name="entry">The entry to remove</param>
        void Add(StorageEntry entry);

        /// <summary>
        /// Removes an entry
        /// </summary>
        /// <param name="entry">The entry to remove</param>
        void Remove(StorageEntry entry);
    }
}
