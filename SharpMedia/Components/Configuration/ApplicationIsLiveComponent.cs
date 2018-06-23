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

namespace SharpMedia.Components.Configuration
{
    /// <summary>
    /// A Component Provider wrapper that unregisters itself when
    /// an application instance is not live anymore
    /// </summary>
    public class ApplicationIsLiveComponent : IComponentProvider
    {
        private IComponentProvider provider;

        /// <summary>
        /// Component provider
        /// </summary>
        public IComponentProvider Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        private IComponentDirectory directory;

        /// <summary>
        /// Component directory to unregister from
        /// </summary>
        public IComponentDirectory Directory
        {
            get { return directory; }
            set { directory = value; }
        }

        private IApplicationInstance appInstance;

        /// <summary>
        /// Application to check
        /// </summary>
        public IApplicationInstance ApplicationInstance
        {
            get { return appInstance; }
            set { appInstance = value; }
        }

        private bool IsApplicationLive()
        {
            if (!appInstance.IsRunning)
            {
                if (directory != null)
                {
                    directory.UnRegister(this);
                    directory = null;
                }
                return false;
            }
            return true;
        }

        #region IComponentProvider Members

        public object GetInstance(IComponentDirectory componentDirectory, object clientInstance, string requirementName, string requirementType)
        {
            if (!IsApplicationLive()) return null;

            return provider.GetInstance(componentDirectory, clientInstance, requirementName, requirementType);
        }

        public string[] MatchedTypes
        {
            get 
            {
                if (!IsApplicationLive()) return new String[] { String.Empty };

                return provider.MatchedTypes;
            }
        }

        public string MatchedName
        {
            get
            {
                if (!IsApplicationLive()) return String.Empty;

                return provider.MatchedName;
            }
        }

        public bool MatchAgainstNameAllowed
        {
            get
            {
                if (!IsApplicationLive()) return false;

                return provider.MatchAgainstNameAllowed;
            }
        }

        public bool MatchAgainstTypeAllowed
        {
            get
            {
                if (!IsApplicationLive()) return false;

                return provider.MatchAgainstTypeAllowed;
            }
        }

        #endregion
    }
}
