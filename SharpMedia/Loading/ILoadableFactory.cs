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
    /// Factory wrapper for ILoadables
    /// </summary>
    public interface ILoadableFactory
    {
        /// <summary>
        /// Load the object from a stream
        /// </summary>
        /// <param name="s">Source stream</param>
        /// <returns>Loaded object</returns>
        /// <exception cref="Exception">On unsuccess</exception>
        object Load([NotNull] Stream s);

        /// <summary>
        /// Saves object to stream.
        /// </summary>
        /// <param name="value">Object to save</param>
        /// <param name="s"></param>
        void Save([NotNull] object value, [NotNull] Stream s);

        /// <summary>
        /// Obtains size of resource
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The size or null if size is unknown</returns>
        ulong? Size([NotNull] object value);

        /// <summary>
        /// Returns True if more objects can be loaded from the stream
        /// </summary>
        bool CanLoadMore([NotNull] Stream s);

        /// <summary>
        /// The type of loadable this factory can serve.
        /// </summary>
        Type LoadableType { get; }
    }
}
