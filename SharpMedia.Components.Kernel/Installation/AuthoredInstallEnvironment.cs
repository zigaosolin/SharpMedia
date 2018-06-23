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
using SharpMedia.Components.Installation;
using System.Xml;
using System.Reflection;
using SharpMedia.Components.Database;
using SharpMedia.Components.Kernel.Configuration;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;
using SharpMedia.Components.Configuration.ConfigurationValues;

namespace SharpMedia.Components.Kernel.Installation
{
    /// <summary>
    /// An install environment that loads and saves from/to authored packages
    /// </summary>
    [Serializable]
    internal class AuthoredInstallEnvironment : InstallEnvironment //, IRuntimeInstallEnvironment
    {
        internal AuthoredInstallEnvironment(AuthoredPackage pkg)
        {
            // TODO: Extract information from the pkg
            root    = pkg;
            package = pkg;
        }

        internal AuthoredInstallEnvironment() { }

        [NonSerialized]
        private IComponentDirectory componentEnvironment;
        /// <summary>
        /// Component environmnent
        /// </summary>
        public IComponentDirectory ComponentEnvironment
        {
            get { return componentEnvironment; }
            set { componentEnvironment = value; }
        }

        #region InstallEnvironment Members

        private string sourcePath;
        public string SourcePath
        {
            get { return sourcePath; }
            set { sourcePath = value; }
        }

        /// <summary>
        /// install to the local node
        /// </summary>
        private string destinationPath = "/";
        public string DestinationPath
        {
            get { return destinationPath; }
            set { destinationPath = value; }
        }

        Dictionary<string, List<InstallSource>> sourceFileLists = new Dictionary<string, List<InstallSource>>();
        public Dictionary<string, List<InstallSource>> SourceFileLists
        {
            get { return sourceFileLists; }
        }

        Dictionary<string, List<InstallDestination>> destinationFileLists = new Dictionary<string, List<InstallDestination>>();
        public Dictionary<string, List<InstallDestination>> DestinationFileLists
        {
            get { return destinationFileLists; }
        }

        [NonSerialized]
        IPackage package = null;
        public IPackage Package
        {
            get { return package; }
            set { package = value; }
        }

        [NonSerialized]
        IPackage root;
        public IPackage Root
        {
            get { return root; }
            set { root = value; }
        }

        [NonSerialized]
        List<ICommand> commandsToExecute = new List<ICommand>();
        public List<ICommand> CommandsToExecute
        {
            get { return commandsToExecute; }
            set { commandsToExecute = value; }
        }

        #endregion

        internal void LoadFromXML(XmlDocument doc)
        {
            XmlNode ipackageNode = doc["InstallPackage"];
            XmlNode destinations = ipackageNode["Destinations"];
            XmlNode sources      = ipackageNode["Sources"];

            foreach (XmlNode dest in destinations.ChildNodes)
            {
                if (!(dest is XmlElement)) continue;

                string destName = dest.Name;
                List<InstallDestination> destList = new List<InstallDestination>();

                foreach (XmlNode sdest in dest.ChildNodes)
                {
                    if (!(sdest is XmlElement)) continue;

                    destList.Add(CreateDestination(sdest));
                }

                destinationFileLists[destName] = destList;
            }

            foreach (XmlNode source in sources.ChildNodes)
            {
                if (!(source is XmlElement)) continue;

                string srcName = source.Name;
                List<InstallSource> srcList = new List<InstallSource>();

                foreach (XmlNode src in source.ChildNodes)
                {
                    if (!(src is XmlElement)) continue;

                    srcList.Add(CreateSource(src));
                }

                sourceFileLists[srcName] = srcList;
            }
        }

        /// <summary>
        /// Attribute to look for to reference assemblies with if needed
        /// </summary>
        static readonly string assemblyAttribute = "assembly";

        /// <summary>
        /// Attribute to look for to reference namespaces with if needed
        /// </summary>
        static readonly string namespaceAttribute = "namespace";

        /// <summary>
        /// Default assembly to load sources and destinations from
        /// </summary>
        static readonly string defaultAssembly = "SharpMedia";

        /// <summary>
        /// Default namespace containing install sources
        /// </summary>
        static readonly string defaultSourceNamespace = "SharpMedia.Components.Installation.Sources";

        /// <summary>
        /// Default namespace containing install destinations
        /// </summary>
        static readonly string defaultDestinationNamespace = "SharpMedia.Components.Installation.Destinations";

        /// <summary>
        /// Name of the property used to assign child sources/destinations with
        /// </summary>
        static readonly string childPropertyName = "Child";

        private InstallDestination CreateDestination(XmlNode sdest)
        {
            string asm = defaultAssembly;
            if ((sdest as XmlElement).HasAttribute(assemblyAttribute))
            {
                asm = sdest.Attributes[assemblyAttribute].Value;
            }

            string nsp = defaultDestinationNamespace;
            if ((sdest as XmlElement).HasAttribute(namespaceAttribute))
            {
                asm = sdest.Attributes[namespaceAttribute].Value;
            }

            string fullName = String.Format("{0}.{1}, {2}", nsp, sdest.Name, asm);
            Type t = Type.GetType(fullName);
            if (t != null)
            {
                InstallDestination inspected = t.GetConstructor(Type.EmptyTypes).Invoke(null) as InstallDestination;

                IComponentConfiguration config = new StandardConfiguration();
                foreach (XmlAttribute attribute in sdest.Attributes)
                {
                    config.Values[attribute.Name] = new Simple(attribute.Value);
                }

                return ConfiguredComponent.ReconfigureInstance(inspected, inspected.GetType(), componentEnvironment, config);
            }

            throw new Exception(String.Format("The Install Destination {0} is not supported.", sdest.Name));
        }

        private InstallSource CreateSource(XmlNode src)
        {
            string asm = defaultAssembly;
            if ((src as XmlElement).HasAttribute(assemblyAttribute))
            {
                asm = src.Attributes[assemblyAttribute].Value;
            }

            string nsp = defaultSourceNamespace;
            if ((src as XmlElement).HasAttribute(namespaceAttribute))
            {
                asm = src.Attributes[namespaceAttribute].Value;
            }

            string fullName = String.Format("{0}.{1}, {2}", nsp, src.Name, asm);

            Type t = Type.GetType(fullName);
            if (t != null)
            {
                InstallSource inspected = t.GetConstructor(Type.EmptyTypes).Invoke(null) as InstallSource;

                IComponentConfiguration config = new StandardConfiguration();
                foreach (XmlAttribute attribute in src.Attributes)
                {
                    config.Values[attribute.Name] = new Simple(attribute.Value);
                }

                inspected = ConfiguredComponent.ReconfigureInstance(
                    inspected, inspected.GetType(), componentEnvironment, config);


                if (src.ChildNodes.Count == 1)
                {
                    PropertyInfo pinfo = t.GetProperty(childPropertyName);
                    if (pinfo == null)
                    {
                        throw new Exception(String.Format("Source {0} does not allow child sources", src.Name));
                    }
                    InstallSource child = CreateSource(src.ChildNodes[0]);
                    pinfo.SetValue(inspected, child, null);
                }

                else if(src.ChildNodes.Count > 1)
                {
                    PropertyInfo pinfo = t.GetProperty(childPropertyName);
                    if (pinfo == null)
                    {
                        throw new Exception(String.Format("Source {0} does not allow child sources", src.Name));
                    }

                    if (pinfo.GetIndexParameters().Length == 0)
                    {
                        throw new Exception(String.Format("Source {0} does not allow multiple child sources", src.Name));
                    }
                    int i = 0;
                    foreach (XmlNode node in src.ChildNodes)
                    {
                        InstallSource child = CreateSource(src.ChildNodes[0]);
                        pinfo.SetValue(inspected, child, new object[] { i });
                    }
                }

                return inspected;
            }

            throw new Exception(String.Format("The Install Source {0} is not supported.", src.Name));
        }

        IComponentProvider cloneComponentDefinition = 
            new ConfiguredComponent(typeof(AuthoredInstallEnvironment).FullName);

        internal AuthoredInstallEnvironment Clone()
        {
            AuthoredInstallEnvironment clone =
                componentEnvironment.ConfigureInlineComponent(cloneComponentDefinition) as AuthoredInstallEnvironment;

            clone.SourcePath      = SourcePath;
            clone.DestinationPath = DestinationPath;

            clone.commandsToExecute    = new List<ICommand>(commandsToExecute);
            clone.destinationFileLists = new Dictionary<string, List<InstallDestination>>(destinationFileLists);
            clone.sourceFileLists      = new Dictionary<string, List<InstallSource>>(sourceFileLists);

            return clone;
        }
    }
}
