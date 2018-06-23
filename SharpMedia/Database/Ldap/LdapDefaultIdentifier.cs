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
    /// A default identifier.
    /// </summary>
    public class LdapDefaultIdentifier : ILdapIdentifier
    {
        #region ILdapIdentifier Members

        public string Identify(string propertyName)
        {
            return typeof(LdapTuple).FullName;
        }

        public object Read(System.DirectoryServices.DirectoryEntry entry, string propertyName, uint index)
        {
            // We read it as LdapEntry.
            return new LdapTuple(propertyName, index, entry.Properties[propertyName][(int)index]);
        }

        public void Write(System.DirectoryServices.DirectoryEntry entry, string propertyName, uint index, object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
