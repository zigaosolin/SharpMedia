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

namespace SharpMedia.Database.Indexing
{

    /// <summary>
    /// A member that implements this provides access to index table.
    /// </summary>
    internal interface IIndexAccess
    {
        /// <summary>
        /// Performs index sync.
        /// </summary>
        /// <param name="repair">If repair is true, all indexed objects are deserialized,
        /// their indexed items are inserted to index table and the table is stored.</param>
        void Sync(bool repair);

        /// <summary>
        /// Is indexing enabled.
        /// </summary>
        bool EnableIndexing { set; }

        /// <summary>
        /// Gets or sets index table.
        /// </summary>
        IndexTable IndexTable { get; }

    }
}
