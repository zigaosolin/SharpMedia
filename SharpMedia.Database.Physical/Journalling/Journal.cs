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

namespace SharpMedia.Database.Physical.Journalling
{
    /// <summary>
    /// A journal interface.
    /// </summary>
    public interface IJournal
    {
        /// <summary>
        /// This method is called when the database is mounted to ensure
        /// all previous operation were successful.
        /// </summary>
        /// <param name="cache">The block cached provider.</param>
        /// <param name="recovery">The recovery, can use Default.</param>
        /// <returns>True if recovery was needed.</returns>
        bool Startup(Caching.BlockCache cache, JournalRecovery recovery);

        /// <summary>
        /// Executes the operation in MT mode.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        void Execute(IOperation operation);

        /// <summary>
        /// Read service, available to everybody. It is thread safe.
        /// </summary>
        IReadService ReadService { get; }

        /// <summary>
        /// The physical location of the specified address.
        /// </summary>
        string GetPhysicalLocation(ulong address);

        /// <summary>
        /// Gets the free space in bytes.
        /// </summary>
        /// <value>The free space.</value>
        ulong FreeSpace { get; }

        /// <summary>
        /// Gets the device storage in bytes.
        /// </summary>
        /// <value>The device storage.</value>
        ulong DeviceStorage { get; }
    }
}
