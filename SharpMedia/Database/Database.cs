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
using SharpMedia;

namespace SharpMedia.Database
{
    /// <summary>
    /// IDatabase exposes methods needed to make an object
    /// act as a database. Every database driver must supply it.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IDatabase
    {
        /// <summary>
        /// Obtains the root node of the database. The root node is considered the node,
        /// that is a mounting point. 
        /// <see cref="DatabaseManager"/> wraps the name of root node with some mounting point
        /// name.
        /// </summary>
        /// <returns>Root node of database.</returns>
        IDriverNode Root
        {
            get;
        }

        /// <summary>
        /// Obtains the free space for database in bytes.
        /// </summary>
        /// <returns>Space in bytes.</returns>
        ulong FreeSpace
        {
            get;
        }

        /// <summary>
        /// Obtains all space avilable by device, also that one that was allocated.
        /// </summary>
        /// <returns>Space in bytes.</returns>
        ulong DeviceStorage
        {
            get;
        }
    }
}
