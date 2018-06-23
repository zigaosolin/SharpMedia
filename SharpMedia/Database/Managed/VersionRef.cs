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
using SharpMedia.AspectOriented;
using SharpMedia.Database.Aspects;

namespace SharpMedia.Database.Managed
{
    /// <summary>
    /// A version reference.
    /// </summary>
    internal sealed class VersionRef
    {
        #region Private Members
        string path;
        ulong version;
        #endregion

        #region Constructors

        /// <summary>
        /// A refence construction.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="version">The version.</param>
        public VersionRef([NotNull] string path, ulong version)
        {
            this.path = path;
            this.version = version;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Constructs version reference the node.
        /// </summary>
        /// <param name="node">The node.</param>
        public VersionRef FromNode([NotNull] INode node)
        {
            return new VersionRef(node.Path, node.Version);
        }

        #endregion

        #region Member Methods

        public INode Find([NotNull] ManagedDatabaseManager manager)
        {
            INode node = manager.Find(path);
            if (node == null) return null;
            return node[version];
        }

        #endregion


    }
}
