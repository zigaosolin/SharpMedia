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
using SharpMedia.Components.Database;
using SharpMedia.Database;

namespace SharpMedia.Components.Installation
{
    /// <summary>
    /// Used to create an installation package
    /// </summary>
    public interface IPackageAuthoring : IPublishedInfoAuthoring
    {
        /// <summary>
        /// Commands executed to determine the feasability of the install
        /// </summary>
        List<ICommand> PreInstallCheck { get; }

        /// <summary>
        /// Commands executed to install the package
        /// </summary>
        List<ICommand> Installation { get; }

        /// <summary>
        /// Commands executed to determine the feasability of the un-install
        /// </summary>
        List<ICommand> PreUninstallCheck { get; }

        /// <summary>
        /// Commands executed to determine the 
        /// </summary>
        List<ICommand> Uninstallation { get; }

        /// <summary>
        /// Name of the install package
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Id of the install package
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Sign the package with a private and public key of the publisher. Publisher Id is thus set.
        /// </summary>
        void Sign(Node<object> privateKey, Node<object> publicKey);

        /// <summary>
        /// Writes out the package to a specific destination
        /// </summary>
        void Write(Node<object> destination);
    }
}
