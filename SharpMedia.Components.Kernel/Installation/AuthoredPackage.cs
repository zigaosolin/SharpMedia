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
using SharpMedia.Database;
using SharpMedia.Components.Database;
using System.Xml;
using SharpMedia.Components.Installation.Commands;
using System.Reflection;
using SharpMedia.Components.Kernel.Configuration;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;

namespace SharpMedia.Components.Kernel.Installation
{
    /// <summary>
    /// A physical representation of a package
    /// </summary>
    [Serializable]
    internal class AuthoredPackage : IPackage, IPackageAuthoring
    {
        // [NonSerialized]
        List<IPackage> subPackages = new List<IPackage>();

        // [NonSerialized]
        private AuthoredInstallEnvironment environment;
        /// <summary>
        /// Authored installation environment of this package
        /// </summary>
        [Required, DefaultClass(typeof(AuthoredInstallEnvironment))]
        public AuthoredInstallEnvironment Environment
        {
            get { return environment; }
            set { environment = value; environment.Package = this; environment.Root = this; }
        }

        [NonSerialized]
        private IComponentDirectory componentEnvironment;
        /// <summary>
        /// Used to activate commands with
        /// </summary>
        [Required]
        public IComponentDirectory ComponentEnvironment
        {
            get { return componentEnvironment; }
            set { componentEnvironment = value; }
        }

        private string sourceLocation;
        /// <summary>
        /// Source URL of the package
        /// </summary>
        public string SourceLocation
        {
            get { return sourceLocation; }
            set { sourceLocation = value; }
        }

        #region Package Members

        string name;
        public string Name
        {
            get { return name;  }
            set { name = value; }
        }

        Guid id;
        public Guid Id
        {
            get { return id;  }
            set { id = value; }
        }

        public IPackage[] SubPackages
        {
            get { return subPackages.ToArray(); }
        }

        bool selectable = true;
        public bool Selectable
        {
            get { return selectable; }
        }

        bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selectable && value)
                {
                    selected = value;
                }
                else selected = false;
            }
        }

        #endregion

        #region PackageAuthoring Members

        // [NonSerialized]
        List<ICommand> preInstallCheck = new List<ICommand>();
        public List<ICommand> PreInstallCheck
        {
            get { return preInstallCheck; }
        }

        // [NonSerialized]
        List<ICommand> installation = new List<ICommand>();
        public List<ICommand> Installation
        {
            get { return installation; }
        }

        List<ICommand> preUninstallCheck = new List<ICommand>();
        public List<ICommand> PreUninstallCheck
        {
            get { return preUninstallCheck; }
        }

        // [NonSerialized]
        List<ICommand> uninstallation = new List<ICommand>();
        public List<ICommand> Uninstallation
        {
            get { return uninstallation; }
        }

        string publisherName;
        public string Publisher
        {
            get
            {
                return publisherName;
            }
            set
            {
                publisherName = value;
            }
        }

        public void Sign(Node<object> privateKey, Node<object> publicKey)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(Node<object> destination)
        {
            destination.AddTypedStream<IPackage>(StreamOptions.None);
            destination.As<IPackage>().Object = this;
        }

        #endregion

        static readonly Version expectedVersion = new Version(1, 0, 0);

        static readonly IComponentProvider authoredPackageProvider =
            new AlwaysNewConfiguredComponent(typeof(AuthoredPackage).FullName);

        internal static AuthoredPackage LoadFromXML(XmlDocument doc, IComponentDirectory environment, string srcUrl)
        {
            /** let's load our stuff **/
            XmlElement ipackageNode = doc["InstallPackage"] as XmlElement;
            if (new Version(ipackageNode.GetAttribute("xversion")) > expectedVersion)
            {
                throw new UnsupportedPackageFormatException("Expected xversion " + expectedVersion);
            }

            AuthoredPackage package = 
                environment.ConfigureInlineComponent(authoredPackageProvider) as AuthoredPackage;

            package.name = ipackageNode.GetAttribute("name");
            package.id = new Guid(ipackageNode.GetAttribute("id"));
            package.publisherName = ipackageNode.GetAttribute("publisher");
            package.sourceLocation = srcUrl;

            // package.environment = new AuthoredInstallEnvironment(package);
            package.environment.LoadFromXML(doc);

            /** commands **/
            if (ipackageNode["PreInstallCheck"] != null)
            {
                ImportCommands(ipackageNode["PreInstallCheck"], package.preInstallCheck, environment);
            }

            if (ipackageNode["Installation"] != null)
            {
                ImportCommands(ipackageNode["Installation"], package.installation, environment);
            }

            if (ipackageNode["PreUninstallCheck"] != null)
            {
                ImportCommands(ipackageNode["PreUninstallCheck"], package.preUninstallCheck, environment);
            }

            if (ipackageNode["Uninstallation"] != null)
            {
                ImportCommands(ipackageNode["Uninstallation"], package.uninstallation, environment);
            }

            return package;
        }

        private static void ImportCommands(XmlElement xmlElement, List<ICommand> list, IComponentDirectory componentEnvironment)
        {
            foreach (XmlNode node in xmlElement.ChildNodes)
            {
                if (!(node is XmlElement)) continue;
                XmlElement cmdElement = node as XmlElement;

                try
                {
                    string asmName = "assembly";  string asm = "SharpMedia";
                    string nspName = "namespace"; string nsp = "SharpMedia.Components.Installation.Commands";

                    if (cmdElement.HasAttribute(asmName)) asm = cmdElement.GetAttribute(asmName);
                    if (cmdElement.HasAttribute(nspName)) nsp = cmdElement.GetAttribute(nspName);

                    /* fix the instance */
                    object instance = componentEnvironment.ConfigureInlineComponent(
                        new ConfiguredComponent(
                            String.Format("{0}.{1},{2}", nsp, cmdElement.Name, asm),
                            new StandardConfiguration(cmdElement)));


                    foreach (XmlAttribute attrib in cmdElement.Attributes)
                    {
                        if (attrib.Name == asmName || attrib.Name == nspName) continue;

                        PropertyInfo pinfo = instance.GetType().GetProperty(attrib.Name);
                        if (pinfo == null)
                        {
                            throw new Exception(String.Format("Command {0} does not support the {1} parameter", instance.GetType(), attrib.Name));
                        }

                        if (!pinfo.PropertyType.IsArray)
                        {
                            pinfo.SetValue(instance, attrib.Value, null);
                        }
                        else
                        {
                            pinfo.SetValue(instance, attrib.Value.Split(','), null);
                        }
                    }
                    list.Add(instance as ICommand);
                }
                catch (Exception e)
                {
                    throw new UnsupportedPackageFormatException("Command " + cmdElement.Name + " did not read properly", e);
                }
            }
        }

        #region IPublishedInfo Members

        public string PublisherName
        {
            get { return publisherName; }
        }

        string[] authors = new string[0];
        public string[] Authors
        {
            get { return authors; }
        }

        #endregion

        #region IPublishedInfoAuthoring Members

        string IPublishedInfoAuthoring.PublisherName
        {
            set { publisherName = value; }
        }

        string[] IPublishedInfoAuthoring.Authors
        {
            set { authors = value.Clone() as string[]; }
        }

        #endregion
    }
}
