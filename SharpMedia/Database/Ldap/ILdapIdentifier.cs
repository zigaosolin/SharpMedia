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
using System.DirectoryServices;

namespace SharpMedia.Database.Ldap
{
    /// <summary>
    /// An identifier that helps converting (string, object[]) properties to (type, object[]) kind.
    /// </summary>
    public interface ILdapIdentifier
    {
        /// <summary>
        /// Returns identifier object.
        /// </summary>
        string Identify(string propertyName);

        /// <summary>
        /// Constructs the specific object.
        /// </summary> 
        object Read(DirectoryEntry entry, string propertyName, uint index);

        /// <summary>
        /// Writes the specified object.
        /// </summary>
        void Write(DirectoryEntry entry, string propertyName, uint index, object value);

    }
}
