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

using SharpMedia.Components.Configuration.ComponentProviders;
using SharpMedia.AspectOriented.Framework;

namespace SharpMedia.Components.Kernel.Installation.Processors
{
    /// <summary>
    /// Used by the installation to prevent linking of unallowed modules
    /// </summary>
    internal class PreventIncorrectLinkage : AssemblyInstallationProcessor
    {
        public override bool Process(
            AssemblyName assemblyIn, 
            ref AssemblyName assemblyOut)
        {
            AssemblyName name     = new AssemblyName(assemblyIn.FullName);
            LinkMask     linkMask = LinkMask.All;

            /* PSEUDOCODE:
             * 
             * LinkMask linkMask = InstallEnvironment.AssemblyLinkage(name);
             * 
             */

            /* PSEUDOCODE
            foreach (ModuleDeclaration module in assemblyIn.GetAssemblyEnvelope().Modules)
            {
                foreach (ModuleRefDeclaration xref in module.ModuleRefs)
                {

                     if(InstallEnvironment.AssemblyLinkage(xref.Name) < linkMask)
                     
                     throw new Exception();
                     
                }
            }*/


            assemblyOut = assemblyIn;

            return true;
        }
    }
}
