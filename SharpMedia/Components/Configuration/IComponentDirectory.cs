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

namespace SharpMedia.Components.Configuration
{
    /// <summary>
    /// A directory of component providers
    /// </summary>
    public interface IComponentDirectory : Libraries.ILibraryDirectory
    {
        /// <summary>
        /// Finds components by name
        /// </summary>
        /// <param name="name">Name of the component</param>
        IComponentProvider[] FindByName(string name);

        /// <summary>
        /// Finds components by supported type
        /// </summary>
        /// <param name="type">Type</param>
        IComponentProvider[] FindByType(string type);

        /// <summary>
        /// Registers a component provider
        /// </summary>
        /// <param name="provider">Component provider</param>
        void                 Register(IComponentProvider provider);

        /// <summary>
        /// Unregistersd a component provider
        /// </summary>
        /// <param name="provider"></param>
        void                 UnRegister(IComponentProvider provider);

        /// <summary>
        /// Obtains an component instance by name
        /// </summary>
        /// <param name="name">Component name</param>
        object               GetInstance(string name);

        /// <summary>
        /// Obtains an instance by type
        /// </summary>
        /// <param name="typeName">Type name</param>
        object               GetInstanceByType(string typeName);

        /// <summary>
        /// Obtains an instance by type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        T                    GetInstance<T>();

        /// <summary>
        /// Configures an inline component (without registering it anywhere)
        /// </summary>
        /// <param name="cloneComponentDefinition">Component provider to use</param>
        /// <returns>A component instance from the provider</returns>
        object ConfigureInlineComponent(IComponentProvider cloneComponentDefinition);

        /// <summary>
        /// All of the registered component providers present in the ICD
        /// </summary>
        IComponentProvider[] RegisteredProviders { get; }
    }
}
