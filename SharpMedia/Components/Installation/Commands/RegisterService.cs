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
using SharpMedia.Components.Database;
using SharpMedia.Components.Services;
using SharpMedia.Database;

namespace SharpMedia.Components.Installation.Commands
{
    /// <summary>
    /// Registers a service to the service registry
    /// </summary>
    [Serializable]
    public class RegisterService : ICommand
    {
        private string applicationPath;
        [Ignore]
        public string ApplicationPath
        {
            get { return applicationPath; }
            set { applicationPath = value; }
        }

        private bool autoStart;
        [Ignore]
        public string AutoStart
        {
            get { return autoStart.ToString(); }
            set { autoStart = Boolean.Parse(value); }
        }

        private string name;
        [Ignore]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [NonSerialized]
        DatabaseManager databaseManager;

        [Required]
        public DatabaseManager DatabaseManager
        {
            get { return databaseManager; }
            set { databaseManager = value; }
        }

        #region ICommand Members

        public void Execute(InstallEnvironment env)
        {
            Service service         = new Service(Name, autoStart);
            service.ApplicationPath = ApplicationPath;

            Node<object> theNode = databaseManager.Find("/System/Runtime/Services");
            if (!theNode.TypedStreamExists<Service>())
            {
                theNode.AddTypedStream<Service>(StreamOptions.Indexed | StreamOptions.AllowDerivedTypes);
            }

            using (TypedStream<Service> ts = theNode.OpenForWriting<Service>())
            {
                ts.Write(ts.Count, service);
            }
        }

        #endregion
    }
}
