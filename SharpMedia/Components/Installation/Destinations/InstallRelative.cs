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
using SharpMedia.Database;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Components.Installation.Destinations
{
    /// <summary>
    /// An installation destination that is relative to the destination URL.
    /// </summary>
    [Serializable]
    public class InstallRelative : InstallDestination
    {
        [NonSerialized]
        InstallEnvironment environment = null;

        [NonSerialized]
        DatabaseManager database = null;

        [Required]
        public DatabaseManager Database
        {
            get { return database; }
            set { database = value; }
        }

        private string relative;
        /// <summary>
        /// Relative path to treat as installation destination
        /// </summary>
        public string Relative
        {
            get { return relative; }
            set { relative = value; }
        }

        string Absolute
        {
            get { return environment.DestinationPath + Relative; }
        }

        public override string ToString()
        {
            return Absolute;
        }

        #region InstallDestination Members
        public bool Exists
        {
            get 
            {
                return database.Find(Absolute) != null;
            }
        }

        public void Delete()
        {
            database.Root.Delete(Absolute);
        }

        public TypedStream<T> OpenForWriting<T>()
        {
            if (!Exists)
            {
                database.Create(Absolute, typeof(T), StreamOptions.AllowDerivedTypes);
            }
            if (!ExistsType(typeof(T)))
            {
                database.Find(Absolute).AddTypedStream<T>(StreamOptions.AllowDerivedTypes);
            }
            return database.Find(Absolute).OpenForWriting<T>();
        }

        public TypedStream<object> OpenForWriting(Type t)
        {
            if (!Exists)
            {
                database.Create(Absolute, t, StreamOptions.AllowDerivedTypes);
            }
            if (!ExistsType(t))
            {
                database.Find(Absolute).AddTypedStream(t, StreamOptions.AllowDerivedTypes);
            }
            return database.Find(Absolute).Open(t, OpenMode.Write);
        }

        public bool ExistsType(Type t)
        {
            if (Exists)
            {
                return database.Find(Absolute).TypedStreamExists(t);
            }
            return false;
        }

        public bool DeleteType(Type t)
        {
            if (ExistsType(t))
            {
                database.Find(Absolute).RemoveTypedStream(t);
                return true;
            }
            return false;
        }

        public InstallSource AsSource
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region UsesInstallEnvironment Members

        public InstallEnvironment InstallEnvironment
        {
            set { environment = value; }
        }

        #endregion
    }
}
