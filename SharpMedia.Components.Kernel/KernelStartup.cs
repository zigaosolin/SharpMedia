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
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SharpMedia.Database;
using System.Threading;

using SharpMedia.Components.Database;
using SharpMedia.Components.Installation;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;
using SharpMedia.Components.TextConsole;
using SharpMedia.Components.Applications;
using SharpMedia.Components.Services;
using SharpMedia.Components.Kernel.Configuration;
using SharpMedia.Components.Kernel.Services;
using SharpMedia.Components.Kernel.Documents;
using SharpMedia.Threading.Default;

namespace SharpMedia.Components.Kernel
{
    // starts up everything
    public class KernelStartup
    {
        /// <summary>
        /// GUID of the base system
        /// </summary>
        static readonly Guid baseSystemId = new Guid("A0B61AB8-84E1-11DC-A6DF-B5C056D89593");

        /// <summary>
        /// Root environment
        /// </summary>
        static KernelComponentDirectory kernelEnvironment = new KernelComponentDirectory();

        static KernelAssemblyLoader assemblyLoader = null;

        public static void Execute(DatabaseManager dbMgr, string[] args)
        {
            //---- Hook up the assembly loading ----
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            AppDomain.CurrentDomain.TypeResolve     += new ResolveEventHandler(CurrentDomain_TypeResolve);

            assemblyLoader = new KernelAssemblyLoader();
            assemblyLoader.DatabaseManager = dbMgr;

            kernelEnvironment.Register(
                new Instance(assemblyLoader, "AssemblyLoader"));

            kernelEnvironment.Register(
                new ConfiguredComponent(
                    typeof(DocumentManagement), "DocumentManagement"));

            kernelEnvironment.Register(
                new ConfiguredComponent(
                    typeof(KernelApplicationsDirectory), "ApplicationManagement"));

            kernelEnvironment.Register(
                new ConfiguredComponent(
                    typeof(Installation.AuthoringInstallationService), "InstallationService"));

            kernelEnvironment.Register(
                new Instance(dbMgr, "DatabaseManagerDriver"));

            kernelEnvironment.Register(
                new AlwaysNewConfiguredComponent(typeof(DatabaseManager), "DatabaseManager"));

            kernelEnvironment.Register(
                new ConfiguredComponent(typeof(StandardConsole), "Console"));

            kernelEnvironment.Register(
                new ConfiguredComponent(typeof(ServiceManager), "Services"));
            kernelEnvironment.GetInstance<IServiceRegistry>();

            kernelEnvironment.Register(
                new ConfiguredComponent(typeof(ThreadControl), "ThreadControl"));

            //---- INIT ----
            InstallationService installService = kernelEnvironment.GetInstance<InstallationService>();

            if (!installService.IsPackageInstalled(baseSystemId))
            {
                IPackage pkg = 
                    installService.OpenPackageForInstallation("/Volumes/Host/Installation/BaseSystem.ipkg");

                pkg.Selected = true;
                installService.Install(pkg);
     
                pkg =
                    installService.OpenPackageForInstallation("/Volumes/Host/Installation/Graphics.ipkg");
                
                pkg.Selected = true;
                installService.Install(pkg);

                
                pkg =
                    installService.OpenPackageForInstallation("/Volumes/Host/Installation/Direct3D10Driver.ipkg");

                pkg.Selected = true;
                installService.Install(pkg);
                 
                /*
                pkg = 
                    installService.OpenPackageForInstallation("/Volumes/Host/Installation/Tools.ipkg");
                pkg.Selected = true;
                installService.Install(pkg);*/
            }

            IApplicationInstances appInstances = kernelEnvironment.GetInstance<IApplicationInstances>();
            IApplicationInstance init = appInstances.Run("/System/Applications/Components/Init", string.Empty, args);

            while (init.IsRunning) Thread.Sleep(100);
        }

        static System.Reflection.Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            string debugInfo = "";
            string tmp = "";
            foreach (string sub in args.Name.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
            {
                tmp = tmp == "" ? sub : tmp + "." + sub;

                if (tmp != args.Name)
                {
                    debugInfo = "";
                    System.Reflection.Assembly asm = assemblyLoader.Load(tmp, out debugInfo);
                    if (asm != null)
                    {
                        if (asm.GetType(args.Name) != null) return asm;
                    }
                }
            }

            if (debugInfo.Trim() != string.Empty)
            {
                Console.Out.WriteLine(debugInfo);
            }
            Console.Out.WriteLine("> Failed to get type {0}", args.Name);

            return null;
        }

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string debugInfo;
            return assemblyLoader.Load(args.Name, out debugInfo);

            if (debugInfo.Trim() != string.Empty)
            {
                Console.Out.WriteLine(debugInfo);
            }
        }
    }
}