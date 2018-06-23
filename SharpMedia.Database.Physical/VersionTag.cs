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
    /// The version(s) tag.
    /// </summary>
    [Serializable]
    public class VersionTag
    {
        /// <summary>
        /// The hash code.
        /// </summary>
        public int HashCode;

        /// <summary>
        /// The version-address pair.
        /// </summary>
        public List<KeyValuePair<ulong, ulong>> VersionAddress = new List<KeyValuePair<ulong,ulong>>();

        /// <summary>
        /// Adds an entry to this hash.
        /// </summary>
        /// <param name="hashCode">The hash code.</param>
        /// <param name="version">The version.</param>
        /// <param name="address">The address.</param>
        public void Add(ulong version, ulong address)
        {
            VersionAddress.Add(new KeyValuePair<ulong,ulong>(version, address));
        }

        /// <summary>
        /// Finds the specified version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public ulong? Find(ulong version)
        {
            for (int i = 0; i < VersionAddress.Count; i++)
            {
                if (VersionAddress[i].Key == version) return VersionAddress[i].Value;
            }
            return null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionTag"/> class.
        /// </summary>
        /// <param name="hashCode">The hash code.</param>
        public VersionTag(int hashCode)
        {
            this.HashCode = hashCode;
        }
    }
}
