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
using SharpMedia.Components.Configuration;
using System.Reflection;

namespace SharpMedia.AspectOriented.Framework
{
    /// <summary>
    /// Thrown when an assembly is not found for weaving
    /// </summary>
    [global::System.Serializable]
    public class AssemblyNotFoundException : Exception
    {
        public AssemblyNotFoundException() { }
        public AssemblyNotFoundException(string message) : base(message) { }
        public AssemblyNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected AssemblyNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Performs Post# Assembly Weaving
    /// </summary>
    public class ModuleWeaving
    {
        /// <summary>
        /// Locator for assemblies
        /// </summary>
        [Required]
        public IAssemblyLocator AssemblyLocator { get; set; }

        /// <summary>
        /// Performs weaving of an assembly
        /// </summary>
        /// <param name="assemblyNameOrPath">Assembly name</param>
        /// <param name="tasks">Tasks to perform during the weaving</param>
        /// <param name="weavedAssemblyBytes">Weaved assembly in byte[] raw (COFF) form</param>
        /// <returns>True if weaving is successfull, false otherwise</returns>
        /// <exception cref="AssemblyNotFoundException">When assembly is not found</exception>
        /// <exception cref="Exception">When an error occurs that warrants more than returning false</exception>
        public bool PerformWeaving(string assemblyNameOrPath, object[] tasks, out byte[] weavedAssemblyBytes)
        {
            weavedAssemblyBytes = null;

            // Added by ziga to fix error!
            string assemblyPath = string.Empty;
            IAssemblyLoader loader = AssemblyLocator.GetLoader(assemblyPath);
            if (loader == null)
            {
                throw new AssemblyNotFoundException();
            }

            // FIXME: WE WANT THIS IN byte[] FORM
            Assembly assembly = loader.Load();

            // FIXME: SETUP PROJECT
            foreach (object task in tasks)
            {
                
            }

            return true;
        }
    }
}
