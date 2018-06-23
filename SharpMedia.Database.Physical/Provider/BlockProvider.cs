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

namespace SharpMedia.Database.Physical.Provider
{
    /// <summary>
    /// A block provider interface. Block size is configurable.
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Gets or sets the size of the block.
        /// </summary>
        /// <value>The size of the block.</value>
        uint BlockSize
        {
            get;
            set;
        }

        /// <summary>
        /// Tries to enlarge provider. 
        /// </summary>
        /// <param name="size">The number of blocks it should contain.</param>
        /// <returns>True if could resize, otherwise false.</returns>
        bool Enlarge(ulong size);

        /// <summary>
        /// Writes the block to specified address.
        /// </summary>
        /// <param name="addr">The address of block.</param>
        /// <param name="block">The block contents.</param>
        void Write(ulong addr, byte[] block);


        /// <summary>
        /// Reads the specified block at address.
        /// </summary>
        /// <param name="addr">The address.</param>
        /// <returns>Bytes of block.</returns>
        byte[] Read(ulong addr);

        /// <summary>
        /// Obtains physical location of address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>The location.</returns>
        string GetPhysicalLocation(ulong address);

    }
}
