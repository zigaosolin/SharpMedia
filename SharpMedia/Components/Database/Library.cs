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
using SharpMedia.Components.Configuration.ComponentProviders;

namespace SharpMedia.Components.Database
{
    /// <summary>
    /// A description of a COS Library
    /// </summary>
    [Serializable]
    public class LibraryDesc
    {
        private string[] preloadAssemblies;
        /// <summary>
        /// Preload these assemblies for the library
        /// </summary>
        public string[] PreloadAssemblies
        {
            get { return preloadAssemblies; }
            set { preloadAssemblies = value; }
        }

        private ConfiguredComponent[] components;
        /// <summary>
        /// Components provided by this library
        /// </summary>
        public ConfiguredComponent[] Components
        {
            get { return components; }
            set { components = value; }
        }
    }
}
