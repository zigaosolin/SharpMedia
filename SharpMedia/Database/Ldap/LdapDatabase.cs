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
    /// A Ldap database.
    /// </summary>
    /// <remarks>Note that this port of Ldap to database system is not fully transparent
    /// for secure access. You can open any node, but you may get errors if you do not fill
    /// in the LdapDirectory node containing</remarks>
    public sealed class LdapDatabase : IDatabase
    {
        #region Private Members
        LdapNode root;
        #endregion

        #region Public Members

        /// <summary>
        /// Creates a database.
        /// </summary>
        /// <param name="path"></param>
        public LdapDatabase(string path)
        {
            root = new LdapNode(path, new LdapDefaultIdentifier());
        }

        #endregion

        #region IDatabase Members

        public IDriverNode Root
        {
            get { return root; }
        }

        public ulong FreeSpace
        {
            get { return 0; }
        }

        public ulong DeviceStorage
        {
            get { return 0; }
        }

        #endregion
    }
}
