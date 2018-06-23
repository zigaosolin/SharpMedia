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
using SharpMedia.Database;
using SharpMedia.Components.Installation;
using System.Runtime.CompilerServices;
using SharpMedia.Components.Database;
using System.Xml;
using SharpMedia.Components.Kernel.Configuration;
using System.IO;
using System.Runtime.Serialization;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Components.Kernel.Installation
{
    /// <summary>
    /// Governs creation and manipulation of install packages
    /// </summary>
    internal class AuthoringInstallationService : MarshalByRefObject, InstallationService
    {
        #region Component Dependencies

        private DatabaseManager databaseManager;
        [Required]
        public DatabaseManager DatabaseManager
        {
            get { return databaseManager;  }
            set { databaseManager = value; }
        }

        private IComponentDirectory componentEnvironment;
        [Required]
        public IComponentDirectory ComponentEnvironment
        {
            get { return componentEnvironment; }
            set { componentEnvironment = value; }
        }

        private string installDirectory = "/System/Runtime/Installations";
        public string InstallDirectory
        {
            get { return installDirectory;  }
            set { installDirectory = value; }
        }

        #endregion

        #region Private

        private bool initialized = false;
        Node<object> myRoot = null;

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AssureInitialized()
        {
            if (!initialized)
            {
                // DebugFS();
                myRoot = databaseManager.Find(installDirectory);

                /* temporary error, or so we hope */
                if (myRoot == null) return;

                foreach (Node<object> pkgNode in myRoot.ChildrenWithType<IPackage>())
                {
                    installedPackages.Add(pkgNode.ObjectOfType<IPackage>());
                }

                initialized = true;
            }
        }

        private void DebugFS()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Out.WriteLine("+++ Start DebugFS");
            DebugFS(databaseManager.Root, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Out.WriteLine("--- End DebugFS");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void DebugFS(Node<object> node, int tab)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine("{0} - {1}", Tabs(tab), node.Name);
            Console.ForegroundColor = ConsoleColor.White;


            foreach (Type t in node.TypedStreams)
            {
                int count = (int) node.OpenForReading().Count;
                if (count != 0)
                {
                    Console.Out.WriteLine("{0} + [{1}] ({2})", Tabs(tab + 1), 
                        t.FullName.Replace("SharpMedia","SM").Replace("ComponentOS","COS"), count);
                }
            }
             
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (Node<object> child in node.Children)
            {
                DebugFS(child, tab + 1);
            }
        }

        private string Tabs(int tab)
        {
            string s = "";
            for (int i = 0; i < tab; ++i) s += "  ";
            return s;
        }

        private List<IPackage> installedPackages = new List<IPackage>();

        private void ExecuteCommands(List<ICommand> list, AuthoredInstallEnvironment environ)
        {
            // Console.Out.WriteLine("\tExecute: {0} commands", list.Count);

            environ.CommandsToExecute = new List<ICommand>(list);
            foreach (ICommand cmd in list)
            {
                // Console.Out.WriteLine("\t\tExecuting: {0}", cmd.GetType().Name);
                environ.CommandsToExecute.RemoveAt(0);
                cmd.Execute(environ);

                // DebugFS();
            }
        }

        private void Install(AuthoredPackage pkg, AuthoredInstallEnvironment environ)
        {
            if (!pkg.Selected) return;

            foreach (IPackage subPkg in pkg.SubPackages)
            {
                environ.Package = subPkg;
                Install(subPkg as AuthoredPackage, environ);
            }

            // Console.Out.WriteLine(">> PreInstallCheck: {0}", pkg.Name);
            ExecuteCommands(pkg.PreInstallCheck, environ);
            // Console.Out.WriteLine(">> Installation: {0}", pkg.Name);
            ExecuteCommands(pkg.Installation, environ);
            // FIXME: We only allow this for the BaseSystem pkg
            AssureInitialized(); 
            if (myRoot == null)
            {
                throw new ServiceNotReadyException();
            }

            // ... write the recipt ...
            myRoot.Create<IPackage>(pkg.Id.ToString(), StreamOptions.SingleObject | StreamOptions.AllowDerivedTypes).Object = pkg;
            installedPackages.Add(pkg);

            // DebugFS();
        }

        public void Uninstall(AuthoredPackage pkg, AuthoredInstallEnvironment environ)
        {
            if (!pkg.Selected) return;
         
            foreach (IPackage subPkg in pkg.SubPackages)
            {
                Uninstall(subPkg as AuthoredPackage, environ);
            }

            ExecuteCommands(pkg.PreUninstallCheck, environ);
            ExecuteCommands(pkg.Uninstallation, environ);
            AssureInitialized();
            if (myRoot == null)
            {
                throw new ServiceNotReadyException();
            }
            myRoot.Delete(pkg.Id.ToString());
            installedPackages.RemoveAll(delegate(IPackage xpkg) { return xpkg.Id == pkg.Id; });
        }

        #endregion

        #region InstallationService Members

        public bool IsPackageInstalled(Guid id)
        {
            // DebugFS();
            AssureInitialized();
            return installedPackages.Find(
                delegate(IPackage pkg) { return pkg.Id == id; }) != null;
        }

        public IPackage[] InstalledPackages
        {
            get
            {
                AssureInitialized();
                return installedPackages.ToArray();
            }
        }

        public IPackage[] InstalledPackagesOfPublisher(string publisherName)
        {
            AssureInitialized();
            return installedPackages.FindAll(
                delegate(IPackage pkg)
                {
                    return pkg.PublisherName == publisherName;
                }).ToArray();
        }

        public IPackage OpenPackageForInstallation(string path)
        {
            Node<object> objNode = databaseManager.Find(path);

            if (objNode.TypedStreamExists(typeof(XmlDocument)))
            {
                XmlDocument doc = objNode.ObjectOfType<XmlDocument>();
                AuthoredPackage pkg = AuthoredPackage.LoadFromXML(doc, componentEnvironment, path);
                pkg.Environment.SourcePath = Path.GetDirectoryName(path).Replace('\\','/');
                return pkg;
            }
            else if(objNode.TypedStreamExists(typeof(AuthoredPackage)))
            {
                AuthoredPackage pkg = objNode.ObjectOfType<AuthoredPackage>();
                pkg.Environment.SourcePath = path;
                /** FIXME: We need an CC.ReconfigureInstance() */
                return pkg;
            }

            throw new Exception(String.Format("Path {0} is not supported", path));
        }

        public void Install(IPackage pkg)
        {
            if (!(pkg is AuthoredPackage))
            {
                throw new PackageNotFoundException("Package is not of the correct type");
            }

            AuthoredPackage xpkg = pkg as AuthoredPackage;
            Console.Out.WriteLine("Installing from: {0} ({1})", xpkg.Environment.SourcePath, xpkg.Name);
            this.Install(xpkg, xpkg.Environment);
        }

        #endregion
    }
}