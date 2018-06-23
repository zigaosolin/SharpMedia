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

namespace SharpMedia.Database.Remote
{

    /// <summary>
    /// A remote database.
    /// </summary>
    public class RemoteDatabase : MarshalByRefObject, IDatabase
    {
        #region Private Members
        IDriverNode root;

        internal RemoteDatabase()
        {
        }
        #endregion

        #region Public Members

        /// <summary>
        /// Locates a remote database.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static RemoteDatabase Locate(string url)
        {
            return Activator.GetObject(typeof(RemoteDatabase), url) as RemoteDatabase;
        }

        /// <summary>
        /// The root of hierarchy.
        /// </summary>
        public IDriverNode Root
        {
            get { return root; }
            internal set
            {
                if (root == null) root = value;
                else
                {
                    throw new InvalidOperationException("Can set root only once.");
                }
            }
        }

        #endregion

        #region IDatabase Members

        IDriverNode IDatabase.Root
        {
            get { return root; }
        }

        ulong IDatabase.FreeSpace
        {
            get { return 0; }
        }

        ulong IDatabase.DeviceStorage
        {
            get { return 0; }
        }

        #endregion
    }
}
