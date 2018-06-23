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

namespace SharpMedia.Components.Configuration.ComponentProviders
{
    public class NameOverride : IComponentProvider
    {
        IComponentProvider provider;
        string nameOverride;

        /// <summary>
        /// Initializes a NameOverride provider
        /// </summary>
        /// <param name="provider">A provider to override the name for</param>
        /// <param name="nameOverride">New name to override the provider name to</param>
        public NameOverride(IComponentProvider provider, string nameOverride)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            this.provider = provider;
            this.nameOverride = nameOverride;
        }

        #region IComponentProvider Members

        public object GetInstance(IComponentDirectory componentDirectory, object clientInstance, string requirementName, string requirementType)
        {
            return provider.GetInstance(componentDirectory, clientInstance, requirementName, requirementType);
        }

        public string[] MatchedTypes
        {
            get { return provider.MatchedTypes; }
        }

        public string MatchedName
        {
            get { return nameOverride; }
        }

        public bool MatchAgainstNameAllowed
        {
            get { return provider.MatchAgainstNameAllowed; }
        }

        public bool MatchAgainstTypeAllowed
        {
            get { return provider.MatchAgainstTypeAllowed; }
        }

        #endregion
    }
}
