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
using SharpMedia.Database.Physical.Journalling;
using SharpMedia.Database.Physical.Caching;
using SharpMedia.Database.Physical.StorageStructs;
using System.Diagnostics;

namespace SharpMedia.Database.Physical
{

    /// <summary>
    /// A physical database implementation.
    /// </summary>
    public class PhysicalDatabase : IDatabase
    {
        #region Private Members
        ulong rootAddress;
        Journalling.IJournal journal;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDatabase"/> class.
        /// </summary>
        /// <param name="journal">The journal.</param>
        public unsafe PhysicalDatabase(IJournal journal, Caching.BlockCache cache, JournalRecovery recovery)
        {
            this.journal = journal;
            this.journal.Startup(cache, recovery);

            // Must make sure we construct the first node if not already there ("default node").
            Block block = journal.ReadService.Read(BlockType.NoCache, 0);
            fixed (byte* p = block.Data)
            {
                DatabaseHeader* header = (DatabaseHeader*)p;
                rootAddress = header->RootObjectAddress;
            }

            // We must initialize it.
            if (rootAddress == 0)
            {
                journal.Execute(new Operations.CreateRootObject());
                block = journal.ReadService.Read(BlockType.NoCache, 0);
                fixed (byte* pp = block.Data)
                {
                    DatabaseHeader* header = (DatabaseHeader*)pp;
                    rootAddress = header->RootObjectAddress;
                }

                Debug.Assert(rootAddress != 0);
            }

        }

        #endregion

        #region IDatabase Members

        public IDriverNode Root
        {
            get { return new PhysicalNode("ROOT", rootAddress, journal); }
        }

        public ulong FreeSpace
        {
            get { return journal.FreeSpace; }
        }

        public ulong DeviceStorage
        {
            get { return journal.DeviceStorage;  }
        }

        #endregion
    }
}
