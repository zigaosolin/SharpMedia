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
using SharpMedia.Components.Configuration;

namespace SharpMedia.Components.Database
{
    /// <summary>
    /// Describes an application
    /// </summary>
    [Serializable]
    public class ApplicationDesc : LibraryDesc, IApplicationInfo
    {
        private string applicationComponent;
        public string ApplicationComponent
        {
            get { return applicationComponent; }
            set { applicationComponent = value; }
        }

        private Dictionary<string, string[]> defaultBindings = new Dictionary<string,string[]>();
        /// <summary>
        /// Default bindings for the app
        /// </summary>
        [Ignore]
        public Dictionary<string, string[]> DefaultBindings
        {
            get { return defaultBindings; }
            set { defaultBindings = value; }
        }
        
        #region IApplicationInfo Members

        string name;
        public string Name
        {
            get { return name; }
        }

        Guid id;
        public Guid Id
        {
            get { return id; }
        }

        string friendlyName;
        public string FriendlyName
        {
            get { return friendlyName; }
        }

        DocumentSupport type;
        public DocumentSupport DocumentSupport
        {
            get { return type; }
        }

        #endregion

        #region IPublishedInfo Members

        string publisherName;
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

        public ApplicationDesc()
        {
        }

        public ApplicationDesc(Guid id, DocumentSupport type, string name, string friendlyName)
        {
            this.id   = id;
            this.type = type;
            this.name = name;
            this.friendlyName = friendlyName;
        }

        public ApplicationDesc(Guid id, DocumentSupport type, string name, string friendlyName, IPublishedInfo info)
        {
            this.id            = id;
            this.type          = type;
            this.name          = name;
            this.friendlyName  = friendlyName;
            this.publisherName = info.PublisherName;
            this.authors       = info.Authors.Clone() as string[];
        }
    }
}
