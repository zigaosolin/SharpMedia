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
using System.Data;
using System.Configuration;
using System.Reflection;
using SharpMedia.AspectOriented;

namespace SharpMedia
{
    /// <summary>
    /// A loader class, can load assemblies.
    /// </summary>
    public class DomainAssemblyLoader : MarshalByRefObject
    {
        IAssemblyLocator locator = null;

        static DomainAssemblyLoader self = null;

        /// <summary>
        /// Creates a loader.
        /// </summary>
        /// <param name="obj">The locator.</param>
        public DomainAssemblyLoader([NotNull] object obj) 
        {
            self = this;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            this.locator = obj as IAssemblyLocator;
        }

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.FullName == args.Name) return a;
            }

            // Console.Out.WriteLine("{1}: load '{0}' in AppD '{2}'", args.Name.Split(',')[0], this, AppDomain.CurrentDomain.FriendlyName);
            IAssemblyLoader loader = locator.GetLoader(args.Name);

            // Console.Out.WriteLine("{0}: Got '{1}' as the loader result. In AppDomain {2}", this, loader == null ? "null" : loader.AssemblyName, AppDomain.CurrentDomain.FriendlyName);
            if (loader == null) return null;

            Assembly assembly = loader.Load();
            // Console.Out.WriteLine("{0}: Assembly '{1}' loaded from loader", this, assembly.FullName);

            return assembly;
        }
    }
}
