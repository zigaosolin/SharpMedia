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

namespace SharpMedia.Components.Configuration.ConfigurationValues
{
    [Serializable]
    public class ProviderValue:  IConfigurationValue, IRequiresComponentDirectory
    {
        IComponentProvider provider;
        IComponentDirectory directory;

        #region IConfigurationValue Members

        public object GetInstance(string destinationType)
        {
            return provider.GetInstance(directory, null, string.Empty, string.Empty);
        }

        #endregion

        #region IRequiresComponentDirectory Members

        public IComponentDirectory ComponentDirectory
        {
            set { directory = value; }
        }

        #endregion

        public ProviderValue(IComponentProvider provider)
        {
            this.provider = provider;
        }
    }
}
