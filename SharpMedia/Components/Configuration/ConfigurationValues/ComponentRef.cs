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

namespace SharpMedia.Components.Configuration.ConfigurationValues
{
    /// <summary>
    /// References a component by name
    /// </summary>
    [Serializable]
    public class ComponentRef : IConfigurationValue, IRequiresComponentDirectory
    {
        string componentName;

        private IComponentDirectory componentDirectory;
        /// <summary>
        /// A component directory
        /// </summary>
        public IComponentDirectory ComponentDirectory
        {
            get { return componentDirectory; }
            set { componentDirectory = value; }
        }

        #region IConfigurationValue Members

        public object GetInstance(string destinationType)
        {
            return componentDirectory.GetInstance(componentName);
        }

        #endregion

        public ComponentRef(string text)
        {
            text = text.Substring("${".Length);
            text = text.Substring(0, text.Length - "}".Length);

            this.componentName = text;
        }
    }
}
