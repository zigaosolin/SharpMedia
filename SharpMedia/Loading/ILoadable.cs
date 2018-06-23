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
using System.IO;
using SharpMedia.AspectOriented;

namespace SharpMedia.Loading
{
    /// <summary>
    /// ILoadable interface can load or save certain object to/from stream. The
    /// difference between serializing and saving is, that serialization is considered
    /// to make the class the same as it was, while loading/saving can take
    /// advantage of specific compressions. 
    /// 
    /// Resource that implements loading/saving usually also implements serializing.
    /// </summary>
    public interface ILoadable
    {
        /// <summary>
        /// Loads resource from stream.
        /// </summary>
        /// <param name="s">The stream object.</param>
        /// <returns>Was the resource properly loaded.</returns>
        bool Load([NotNull] Stream s);

        /// <summary>
        /// Saves object to stream.
        /// </summary>
        /// <param name="s">The stream object.</param>
        /// <returns>Indicator if resource was properly saved.</returns>
        bool Save([NotNull] Stream s);

        /// <summary>
        /// Obtains size of resource, to allocate big enough stream.
        /// </summary>
        /// <returns>The size or null if size is unknown
        /// (or too time consuming to precompute).</returns>
        ulong? Size
        {
            get;
        }
    }
}
