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
    /// We make sure database header is sequential with packing 1 byte. It always
    /// resides in 512 bytes (and can be thus used by any block size).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal unsafe struct DatabaseHeader
    {
        /// <summary>
        /// Random magic 1.
        /// </summary>
        public const ulong Magic1 = 0xAF76AD6397BCCDF4;

        /// <summary>
        /// Random magic 2.
        /// </summary>
        public const ulong Magic2 = ~Magic1;

        /// <summary>
        /// Must contain magic 1, this is just a verificator.
        /// </summary>
        public ulong HeaderMagic1;

        /// <summary>
        /// Must contain magic 2, this is just a verificator.
        /// </summary>
        public ulong HeaderMagic2;

        /// <summary>
        /// The name of database. It is limited to 64 unicode characters.
        /// </summary>
        public fixed char DatabaseName[64];

        /// <summary>
        /// Size of one block within a database.
        /// </summary>
        public uint BlockSize;

        /// <summary>
        /// Number of blocks.
        /// </summary>
        public ulong BlockCount;

        /// <summary>
        /// The root object address; object is created on first mount.
        /// </summary>
        public ulong RootObjectAddress;

        /// <summary>
        /// The frequency of journal, measured in allocation blocks.
        /// </summary>
        public uint JournalFrequency;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseHeader"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="blockSize">Size of the block.</param>
        /// <param name="blockCount">Number of block is DB.</param>
        /// <param name="journalFrequency">Journal frequency in number of allocations blocks. 0 means only
        /// head journal.</param>
        public DatabaseHeader(string name, uint blockSize, ulong blockCount, uint journalFrequency)
        {
            if (!BlockHelper.IsValidSize(blockSize))
            {
                throw new ArgumentException("Block size is invalid.");
            }
            if (name.Length >= 64)
            {
                throw new ArgumentException("Name of database is maximum 64 characters long.");
            }

            // We copy the name to static array.
            int i;
            fixed (char* n = DatabaseName)
            {
                for (i = 0; i < name.Length; i++)
                {
                    n[i] = name[i];
                }

                // Null terminated.
                n[i] = '\0';
            }

            

            // We have both magics.
            HeaderMagic1 = Magic1;
            HeaderMagic2 = Magic2;

            // We set block size.
            BlockSize = blockSize;
            BlockCount = blockCount;
            JournalFrequency = journalFrequency;
            RootObjectAddress = 0;
        }

        /// <summary>
        /// Is the header valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (HeaderMagic1 != Magic1 ||
                   HeaderMagic2 != Magic2 ||
                   !BlockHelper.IsValidSize(BlockSize))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
