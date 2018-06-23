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

namespace SharpMedia
{
    /// <summary>
    /// Base class for all tags.
    /// </summary>
    /// <remarks>Tags are elements that are allowed to be set per-instance, not only per class.</remarks>
    public abstract class TagAttribute : Attribute
    {

        /// <summary>
        /// A constructor.
        /// </summary>
        public TagAttribute()
        {
        }
    }

    /// <summary>
    /// A taggable interface
    /// </summary>
    public interface ITaggable
    {
        /// <summary>
        /// Adds a tag to 
        /// </summary>
        /// <remarks>Make sure to check for tag correct handling (access, multi-instance).</remarks>
        void AddTag(TagAttribute tag);

        /// <summary>
        /// Removes tag.
        /// </summary>
        /// <returns>Returns true if it was removed, false otherwise (if not found).</returns>
        bool RemoveTag(TagAttribute tag);

        /// <summary>
        /// Obtains all attributes of object (combination of reflection attributes and tags).
        /// </summary>
        Attribute[] Attributes { get; }

        /// <summary>
        /// Obtains all tags of object.
        /// </summary>
        TagAttribute[] Tags { get; }

    }
}
