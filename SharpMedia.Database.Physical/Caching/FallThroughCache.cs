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

namespace SharpMedia.Database.Physical.Caching
{

    /// <summary>
    /// Fall through cache is used when no caching is desired; this
    /// is usually needed for testing reasons.
    /// </summary>
    internal class FallThroughCache : BlockCache
    {
        #region Overriden Methods

        protected override void WriteToCache(BlockType type, ulong address, byte[] data)
        {
            // Never writes to cache.
        }

        protected override byte[] ReadFromCache(BlockType type, ulong address)
        {
            // Always fails.
            return null;
        }

        public override ulong CachedBlockCount
        {
            get { return 0; }
        }

        #endregion

        #region Constructors

        public FallThroughCache(Provider.IProvider p)
            : base(p)
        {
        }

        #endregion
    }
}
