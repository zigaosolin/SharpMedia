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
using SharpMedia.Components.Database;
using System.Xml;
using SharpMedia.Components.Configuration.ComponentProviders;
using SharpMedia.Database;

namespace SharpMedia.Components.Installation.Sources
{
    /// <summary>
    /// A source that casts to an ApplicationDescriptor
    /// </summary>
    [Serializable]
    public class ApplicationDescriptorSource : InstallSource
    {
        #region Private
        [NonSerialized]
        private InstallEnvironment environment = null;

        private InstallSource child;
        /// <summary>
        /// Child source from where to extract XML
        /// </summary>
        public InstallSource Child
        {
            get { return child; }
            set { child = value; }
        }

        ApplicationDesc applicationDescriptor = null;

        static readonly Type[] appDescTypes = new Type[] { typeof(ApplicationDesc) };

        #endregion

        #region InstallSource Members

        public Type[] Types
        {
            get { return appDescTypes; }
        }

        public TypedStream<T> OpenForReading<T>()
        {
            CacheDeserialization();

            return SharpMedia.Database.Memory.MemoryDatabase.CreateInlineTypedStream<T>((T)(object)applicationDescriptor);
        }

        public TypedStream<object> OpenForReading(Type t)
        {
            CacheDeserialization();

            return SharpMedia.Database.Memory.MemoryDatabase.CreateInlineTypedStream<object>(applicationDescriptor);
        }

        #endregion

        #region UsesInstallEnvironment Members

        public InstallEnvironment InstallEnvironment
        {
            set { environment = value; }
        }

        #endregion

        #region XML Parsing
        private void CacheDeserialization()
        {
            Child.InstallEnvironment = environment;
            SerializableXmlDocument doc = Child.OpenForReading<SerializableXmlDocument>().Object;
            doc.Normalize();

            XmlElement node = doc.FirstChild.NextSibling as XmlElement;

            // assert root node
            if (node.Name != "Application")
            {
                throw new AbortInstallationException("Source is not an application descriptor");
            }

            // check version
            if (new Version(node.GetAttribute("xversion")) > new Version(1, 0, 0))
            {
                throw new AbortInstallationException("Application descriptor is of an incorrect version");
            }

            string name = node.GetAttribute("name");
            Guid id = new Guid(node.GetAttribute("id"));
            string rootComponentName = node.GetAttribute("root");
            DocumentSupport apptype = (DocumentSupport)Enum.Parse(typeof(DocumentSupport), node.GetAttribute("type"));
            string friendlyName = node.GetAttribute("friendly");

            List<ConfiguredComponent> defs = new List<ConfiguredComponent>();

            /** ok, construct **/
            ApplicationDesc app = new ApplicationDesc(id, apptype, name, friendlyName);

            foreach (XmlNode firstNode in node.ChildNodes)
            {
                if (firstNode.Name == "Components")
                {
                    /* load all components */
                    foreach (XmlNode componentNode in firstNode.ChildNodes)
                    {
                        if (componentNode.Name.StartsWith("#")) continue;

                        if (componentNode.Name != "Component")
                            throw new AbortInstallationException("Application description is not well-formed. Only Component nodes are allowed in the Components section");

                        defs.Add(new ConfiguredComponent(componentNode));
                    }
                }
                if (firstNode.Name == "Bindings")
                {
                    foreach (XmlNode bindingNode in firstNode.ChildNodes)
                    {
                        if (bindingNode.Name != "Binding") continue;

                        app.DefaultBindings[((XmlElement)bindingNode).GetAttribute("Type")] =
                            Array.ConvertAll<string, string>(
                                ((XmlElement)bindingNode).GetAttribute("Verbs").Split(','),
                                delegate(string s) { return s.Trim(); });
                    }
                }
            }

            app.Components           = defs.ToArray();
            app.ApplicationComponent = rootComponentName;

            applicationDescriptor = app;
        }
        #endregion
    }
}
