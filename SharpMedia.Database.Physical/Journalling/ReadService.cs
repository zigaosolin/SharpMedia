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
using SharpMedia.Database.Physical.Provider;
using SharpMedia.Database.Physical.Caching;

namespace SharpMedia.Database.Physical.Journalling
{
    /// <summary>
    /// This is the read service based on provider.
    /// </summary>
    internal class ReadService : IReadService
    {
        #region Private Data
        BlockCache provider;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadService"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public ReadService(BlockCache provider)
        {
            this.provider = provider;
        }

        #endregion

        #region IReadService Members

        public uint BlockSize
        {
            get { return provider.BlockSize; }
        }

        public Block Read(SharpMedia.Database.Physical.Caching.BlockType type, ulong address)
        {
            return new Block(provider.Read(type, address));
        }

        #endregion
    }
}
