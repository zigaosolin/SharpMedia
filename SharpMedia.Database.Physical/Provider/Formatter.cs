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
using SharpMedia.Database.Physical.StorageStructs;
using SharpMedia.Database.Physical.Journalling;

namespace SharpMedia.Database.Physical.Provider
{
    /// <summary>
    /// A formatter, the class that prepares database or manipulates it when not mounted.
    /// </summary>
    public static class Formatter
    {


        /// <summary>
        /// Enlarges the specified provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="blockCount">The block count.</param>
        public unsafe static void Enlarge(IProvider provider, ulong blockCount)
        {

        }

        /// <summary>
        /// Formats the database with provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="databaseName">Name of the database, maximum 63 characters long.</param>
        /// <param name="blockCount">The block count.</param>
        /// <param name="journalSectorFreq">The journal frequency.</param>
        /// <param name="journalSize">Size of the journal.</param>
        /// <param name="journalFrequency">The journal frequency, meaning depends on sector frequency.</param>
        /// <returns></returns>
        public unsafe static bool Format(IProvider provider, string databaseName, ulong blockCount,
                    uint journalFrequency)
        {
            if (databaseName == null || databaseName == string.Empty || databaseName.Length > 63)
            {
                throw new ArgumentException("Database name was either invalid or too long.");
            }

            // 1) We first enlarge the provider (must be at least that long).
            if (!provider.Enlarge(blockCount)) return false;

            // 2) We create the header node.
            Block block = new Block(provider.BlockSize);
            fixed (byte* p = block.Data)
            {
                DatabaseHeader* header = (DatabaseHeader*)p;
                header->BlockSize = provider.BlockSize;
                header->BlockCount = blockCount;
                header->HeaderMagic1 = DatabaseHeader.Magic1;
                header->HeaderMagic2 = DatabaseHeader.Magic2;
                header->JournalFrequency = journalFrequency;
                header->RootObjectAddress = 0;

                // Copy the name.
                int i;
                for (i = 0; i < databaseName.Length; i++)
                {
                    header->DatabaseName[i] = databaseName[i];
                }
                header->DatabaseName[i] = '\0';
            }

            // We write it.
            provider.Write(0, block.Data);

            // Make an empty (zero block).
            block.ZeroMemory();

            // The one allocated block.
            Block fullBlock = new Block(provider.BlockSize);
            fullBlock.ZeroMemory();
            BoolArray fullBlockArray = new BoolArray(fullBlock.Data);
            fullBlockArray[0] = true;

            // 3) We clear allocation blocks.
            for (ulong superBlock = BlockHelper.FirstSuperBlockAddress; superBlock != 0;
                superBlock = BlockHelper.GetNextSuperBlock(provider.BlockSize, superBlock, blockCount))
            {
                provider.Write(superBlock, block.Data);
            }

            for (ulong allocBlock = BlockHelper.FirstSuperBlockAddress + 1; allocBlock != 0;
                allocBlock = BlockHelper.GetNextAllocationBlock(provider.BlockSize, allocBlock, blockCount))
            {
                if (JournalLog.IsJournalLog(allocBlock+1, journalFrequency, provider.BlockSize))
                {
                    // We write the allocated.
                    provider.Write(allocBlock, fullBlock.Data);

                    // Next block is filled with header.
                    if (allocBlock + 1 < blockCount)
                    {
                        provider.Write(allocBlock + 1, block.Data);
                    }
                }
                else
                {
                    provider.Write(allocBlock, block.Data);
                }
            }


            return true;
        }

    }
}
