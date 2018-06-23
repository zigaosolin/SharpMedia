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

namespace SharpMedia.Database.Physical
{

    /// <summary>
    /// Identifies one or more children under the same hash.
    /// </summary>
    [Serializable]
    internal class ChildTag
    {
        /// <summary>
        /// The hash code.
        /// </summary>
        public int HashCode;

        /// <summary>
        /// The children names.
        /// </summary>
        public List<KeyValuePair<string, ulong>> Children = new List<KeyValuePair<string,ulong>>();

        /// <summary>
        /// Adds an entry.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="address">Root node address.</param>
        public void Add(string name, ulong address)
        {
            Children.Add(new KeyValuePair<string, ulong>(name, address));
        }

        /// <summary>
        /// Removes an entry.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name)
        {
            Children.RemoveAll(delegate(KeyValuePair<string, ulong> value)
            {
                return value.Key == name;
            });
        }

        /// <summary>
        /// Finds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ulong? Find(string name)
        {
            KeyValuePair<string,ulong> res = Children.Find(
                delegate(KeyValuePair<string,ulong> n) 
                { 
                    return name == n.Key; 
                });
            if (res.Key == null) return null;
            return res.Value;
        }

        /// <summary>
        /// Is the children tag empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Children.Count == 0;
            }
        }
    }
}
