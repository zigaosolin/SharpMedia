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
using System.Reflection;
using SharpMedia.AspectOriented;

namespace SharpMedia
{
    /// <summary>
    /// An interface that converts between assembly names and IAssemblyLoaders to
    /// load the respective assemblies with.
    /// </summary>
    public interface IAssemblyLocator
    {
        /// <summary>
        /// Obtains the loader for the given assembly name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly to obtain the loader for.</param>
        /// <returns>An IAssemblyLoader if possible, or null </returns>
        IAssemblyLoader GetLoader([NotEmpty] string assemblyName);

        /// <summary>
        /// Adds specified assemblies to the locator
        /// </summary>
        /// <param name="assemblies">The assemblies to use</param>
        void AddAssemblies([NotNull] Assembly[] assemblies);
    }
}
