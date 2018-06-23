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

namespace SharpMedia.Database
{
    /// <summary>
    /// A persistency makes sure that resource can be persisted.
    /// </summary>
    [Serializable]
    public sealed class DatabasePersistency : Resources.IPersistencyInfo
    {
        #region Private Members
        string computerName;
        string path;
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabasePersistency"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public DatabasePersistency(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabasePersistency"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="computerName">Name of the computer.</param>
        public DatabasePersistency(string path, string computerName)
        {
            this.path = path;
            this.computerName = computerName;
        }

        #endregion

        #region IPersistencyInfo Members

        public void Persist(bool newVersion)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string Path
        {
            get
            {
                return path;
            }
        }

        #endregion
    }
}
