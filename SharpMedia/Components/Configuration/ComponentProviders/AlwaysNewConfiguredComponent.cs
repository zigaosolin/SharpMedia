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
using System.Xml;

namespace SharpMedia.Components.Configuration.ComponentProviders
{
    /// <summary>
    /// A variant of the configured component that is always newly spawned and reconfigured when its instance is requested
    /// </summary>
    [Serializable]
    public class AlwaysNewConfiguredComponent : ConfiguredComponent
    {
        public override object GetInstance(
            IComponentDirectory componentDirectory, 
            object clientInstance, 
            string requirementName, 
            string requirementType)
        {
            IRequiresConfiguration old = this.requiredConfiguration;
            try
            {
                requiredConfiguration = requiredConfiguration.Clone();
                return base.GetInstance(componentDirectory, clientInstance, requirementName, requirementType);
            }
            finally
            {
                requiredConfiguration = old;
            }
        }

        #region Constructors
        /// <summary>
        /// Constructs from a type name
        /// </summary>
        public AlwaysNewConfiguredComponent(string typeName) : base(typeName)
        {
        }

        /// <summary>
        /// Constructs from a type name
        /// </summary>
        public AlwaysNewConfiguredComponent(string typeName, string configName)
            : base(typeName, configName)
        {
        }

        /// <summary>
        /// Constructs from a type name
        /// </summary>
        public AlwaysNewConfiguredComponent(Type type, string configName)
            : base(type, configName)
        {
        }

        /// <summary>
        /// Constructs from XML
        /// </summary>
        /// <param name="typeName"></param>
        public AlwaysNewConfiguredComponent(XmlNode node) : base(node)
        {
        }

        /// <summary>
        /// Constructs from a type name and a provided configuration
        /// </summary>
        public AlwaysNewConfiguredComponent(string typeName, IComponentConfiguration configProvided) : base(typeName, configProvided)
        {
        }

        /// <summary>
        /// Constructs from required configuration
        /// </summary>
        public AlwaysNewConfiguredComponent(IRequiresConfiguration configRequired) : base(configRequired)
        {
        }

        /// <summary>
        /// Creates a new configured component from required configuration and provided configuration
        /// </summary>
        public AlwaysNewConfiguredComponent(IRequiresConfiguration configRequired, IComponentConfiguration configProvided) : base(configRequired, configProvided)
        {
        }
        #endregion
    }
}
