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

namespace SharpMedia.Database.Ldap
{
    /// <summary>
    /// A typed stream default type for all Ldap nodes. It contains basic information
    /// about Ldap directory. It is allowed to get or to set state.
    /// </summary>
    [Serializable]
    public sealed class LdapEntry : IEquatable<LdapEntry>, ICloneable<LdapEntry>, ITransparent
    {
        #region Private Members

        // Properties
        string domainName;
        string clientName;

        // Setters (otherwise null)
        string password;
        string userName;
        bool autoCommit = true;

        // This means that commit is issued.
        bool issueCommit = false;
        bool issueUpdate = false;

        #endregion

        #region Properties

        /// <summary>
        /// The domain name.
        /// </summary>
        public string DomainName
        {
            get
            {
                return domainName;
            }
            set
            {
                domainName = value;
            }
        }

        /// <summary>
        /// The client name
        /// </summary>
        public string ClientName
        {
            get
            {
                return clientName;
            }
            set
            {
                clientName = value;
            }
        }

        /// <summary>
        /// Can set the password.
        /// </summary>
        public string Password
        {
            set
            {
                password = value;
            }
        }

        /// <summary>
        /// Can set the username.
        /// </summary>
        public string Username
        {
            set
            {
                userName = value;
            }
        }

        /// <summary>
        /// Auto commit.
        /// </summary>
        public bool AutoCommit
        {
            get
            {
                return autoCommit;
            }
            set
            {
                autoCommit = value;
            }
        }

        /// <summary>
        /// Should issue commit on write.
        /// </summary>
        public bool IssueCommit
        {
            set
            {
                issueCommit = value;
            }
            get
            {
                return issueCommit;
            }
        }

        /// <summary>
        /// Should we issue update (refresh).
        /// </summary>
        public bool IssueUpdate
        {
            set
            {
                issueUpdate = value;
            }
            get
            {
                return issueUpdate;
            }
        }

        #endregion

        #region Public Members

        public LdapEntry()
        {
        }


        #endregion

        #region IEquatable<LdapDirectory> Members

        public bool Equals(LdapEntry other)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ICloneable<LdapDirectory> Members

        public LdapEntry Clone()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITransparent Members

        bool ITransparent.IsTransparent(IDriverTypedStream typedStream)
        {
            return typedStream is LdapTypedStream;
        }

        #endregion
    }
}
