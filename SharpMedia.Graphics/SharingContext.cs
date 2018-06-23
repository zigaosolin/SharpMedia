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

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A sharing context, applied to all textures that are either owned by the
    /// process or referenced by the process.
    /// </summary>
    public sealed class SharingContext
    {
        #region Private Members
        Guid guid;
        bool owned;
        #endregion

        #region Constructors

        internal SharingContext()
        {
            this.owned = true;
        }

        /// <summary>
        /// Creates sharing context.
        /// </summary>
        internal SharingContext(Guid guid)
        {
            this.owned = false;
            this.guid = guid;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Guid of sharing context.
        /// </summary>
        public Guid Guid
        {
            get
            {
                return guid;
            }
            internal set
            {
                guid = value;
            }
        }

        /// <summary>
        /// Is it shared.
        /// </summary>
        public bool IsShared
        {
            get
            {
                return guid != Guid.Empty;
            }
        }

        /// <summary>
        /// Is context owned by this resource.
        /// </summary>
        public bool IsOwned
        {
            get
            {
                return owned;
            }
        }

        #endregion
    }
}
