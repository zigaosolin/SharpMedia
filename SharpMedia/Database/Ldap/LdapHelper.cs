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
    /// A static helper class that does Ldap communication.
    /// </summary>
    public static class LdapHelper
    {
        /// <summary>
        /// Sets password to Ldap node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static void SetPassword<T>(Node<T> node, string userName, string password)
        {
            // We first convert to Ldap node.
            Node<LdapEntry> ldapNode = node.As<LdapEntry>();

            // We obtain entry, fill in the blanks and write it.
            LdapEntry entry = ldapNode.Object;

            // We set password and username.
            entry.Password = password;
            entry.Username = userName;

            // Recommit.
            ldapNode.Object = entry;
        }

        /// <summary>
        /// Updates the information about node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        public static void Update<T>(Node<T> node)
        {
            // We first convert to Ldap node.
            Node<LdapEntry> ldapNode = node.As<LdapEntry>();

            // We obtain entry, fill in the blanks and write it.
            LdapEntry entry = ldapNode.Object;

            // Issues an update.
            entry.IssueUpdate = true;
            ldapNode.Object = entry;


        }

    }
}
