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
    /// A block helper class. Block allows serving it's bytes.
    /// </summary>
    public class Block
    {
        #region Private Members
        byte[] data;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Block"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Block(byte[] data)
        {
            this.data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Block"/> class.
        /// </summary>
        /// <param name="lenght">The lenght.</param>
        public Block(uint lenght)
        {
            data = new byte[lenght];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obtains data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// Zeroes the memory.
        /// </summary>
        public void ZeroMemory()
        {
            for (int i = 0; i < data.Length; i++) data[i] = 0;
        }

        public void Memset(byte mem)
        {
            for (int i = 0; i < data.Length; i++) data[i] = mem;
        }

        #endregion
    }
}
