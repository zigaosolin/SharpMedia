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
using System.Runtime.InteropServices;

namespace SharpMedia.Database.Physical.StorageStructs
{
    /// <summary>
    /// The log kind.
    /// </summary>
    public enum JournalLogKind : uint
    {
        /// <summary>
        /// An allocation is presented as log entry (rollback is deallocation).
        /// </summary>
        AllocateBlock,

        /// <summary>
        /// A deallocation is presented as log entry (rollback is allocation).
        /// </summary>
        DeallocateBlock,

        /// <summary>
        /// Operation data is present.
        /// </summary>
        OperationData,

        /// <summary>
        /// A safe block update.
        /// </summary>
        UpdateBlock
    }

    /// <summary>
    /// Represents a single journal log (32 bytes).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct JournalLogData
    {
        /// <summary>
        /// The next log entry block, or 0 if no more entries.
        /// </summary>
        public ulong NextLogEntryBlock;

        /// <summary>
        /// Next log entry offset, or 0 if no next entry.
        /// </summary>
        public uint NextLogEntryOffset;

        /// <summary>
        /// The kind of log.
        /// </summary>
        public JournalLogKind LogKind;

        /// <summary>
        /// The data interpretation depends on LogKind:
        /// - AllocateBlock --> The block that was (will be) allocated
        /// - DeallocateBlock --> The block that was (will be) deallocated
        /// - OperationData --> The address in log where data is serialized
        /// - UpdateBlock --> The address where block data is kept
        /// </summary>
        public ulong Data1;

        /// <summary>
        /// Only valid for UpdateData --> Contains original address of that block.
        /// </summary>
        public ulong Data2;

        /// <summary>
        /// Initializes a new instance of the <see cref="JournalLogData"/> class.
        /// </summary>
        /// <param name="kind">The kind.</param>
        /// <param name="data1">The data1.</param>
        /// <param name="data2">The data2.</param>
        public JournalLogData(JournalLogKind kind, ulong data1, ulong data2)
        {
            this.NextLogEntryOffset = 0;
            this.NextLogEntryBlock = 0;
            this.LogKind = kind;
            this.Data1 = data1;
            this.Data2 = data2;
        }

    }
}
