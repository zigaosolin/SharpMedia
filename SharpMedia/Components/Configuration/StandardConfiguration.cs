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
using SharpMedia.Components.Configuration.ConfigurationValues;

namespace SharpMedia.Components.Configuration
{
    /// <summary>
    /// A Standard component configuration store
    /// </summary>
    [Serializable]
    public class StandardConfiguration : IComponentConfiguration, IRequiresComponentDirectory
    {
        #region IComponentConfiguration Members

        Dictionary<string, IConfigurationValue> values = new Dictionary<string,IConfigurationValue>();

        public Dictionary<string, IConfigurationValue> Values
        {
            get { return values; }
        }

        #endregion

        #region Constructors

        public StandardConfiguration() { }

        /// <param name="node">XML Node to configure self from - may be null</param>
        public StandardConfiguration(XmlNode node)
        {
            if (node == null) return;

            foreach (XmlNode valueNode in node.ChildNodes)
            {
                if (values.ContainsKey(valueNode.Name))
                {
                    throw new Exception(
                        String.Format("Duplicate Configuration provided for item {0}", valueNode.Name));
                }

                values[valueNode.Name] = ParseValue(valueNode);
            }
        }

        #endregion

        #region Helper Methods

        private IConfigurationValue ParseValue(XmlNode valueNode)
        {
            string text = valueNode.InnerText.Trim();

            // TODO: Add your configuration value parsing here
            if (text.StartsWith("${"))
            {
                return new ComponentRef(text);
            }
            else
            {
                return new Simple(text);
            }
        }

        #endregion

        #region IRequiresComponentDirectory Members

        IComponentDirectory componentDirectory;
        public IComponentDirectory ComponentDirectory
        {
            set 
            {
                this.componentDirectory = value;
                foreach (IConfigurationValue config in values.Values)
                {
                    if (config is IRequiresComponentDirectory)
                    {
                        (config as IRequiresComponentDirectory).ComponentDirectory = value;
                    }
                }
            }
        }

        #endregion
    }
}
