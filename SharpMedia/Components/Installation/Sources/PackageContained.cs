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
using System.IO;
using SharpMedia.Components.Database;
using System.Xml;
using SharpMedia.Components.Configuration;
using SharpMedia.Database;

namespace SharpMedia.Components.Installation.Sources
{
    /// <summary>
    /// Package contained sources
    /// </summary>
    [Serializable]
    public class PackageContained : InstallSource
    {
        private string relative;
        /// <summary>
        /// Relative path (from the package)
        /// </summary>
        public string Relative
        {
            get { return relative; }
            set { relative = value; }
        }

        [NonSerialized]
        DatabaseManager databaseManager;

        [Required]
        DatabaseManager DatabaseManager
        {
            get { return databaseManager; }
            set { databaseManager = value; }
        }

        #region Internals

        private string AbsolutePath
        {
            get
            {
                return (installEnvironment.SourcePath + "/" + Relative);
            }
        }

        #endregion

        #region InstallSource Members

        public Type[] Types
        {
            get 
            {
                return databaseManager.Find(AbsolutePath).TypedStreams;
            }
        }

        public TypedStream<object> OpenForReading(Type t)
        {
            return databaseManager.Find(AbsolutePath).Open(t, OpenMode.Read);
        }

        public TypedStream<T> OpenForReading<T>()
        {
            return databaseManager.Find(AbsolutePath).OpenForReading<T>();
        }

        #endregion

        #region UsesInstallEnvironment Members

        [NonSerialized]
        InstallEnvironment installEnvironment;

        public InstallEnvironment InstallEnvironment
        {
            set { installEnvironment = value; }
        }

        #endregion
    }
}
