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
using SharpMedia.Components.Database;

namespace SharpMedia.Components.Configuration.Libraries
{
    /// <summary>
    /// A Library directory
    /// </summary>
    public interface ILibraryDirectory
    {
        /// <summary>
        /// (Fully) loaded libraries
        /// </summary>
        LibraryDesc[] LoadedLibraries { get; }

        /// <summary>
        /// Loads a specific component from a library
        /// </summary>
        /// <param name="libName">Name of the library</param>
        /// <param name="componentName">Name of the component</param>
        /// <param name="throwOnFailure">When true an exception is thrown instead of a null return</param>
        /// <returns>The desired component provider</returns>
        IComponentProvider LoadFromLibrary(string libName, string componentName, bool throwOnFailure);

        /// <summary>
        /// Looads a specified library
        /// </summary>
        /// <param name="libName">Library name</param>
        /// <returns>The loaded Library</returns>
        LibraryDesc LoadLibrary(string libName, bool throwOnFailure);
    }
}
