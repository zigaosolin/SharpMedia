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

namespace SharpMedia.Components.Installation
{
    /// <summary>
    /// Installation service
    /// </summary>
    public interface InstallationService
    {
        /// <summary>
        /// Returns true if a package is installed
        /// </summary>
        /// <param name="id">id of the package</param>
        /// <returns></returns>
        bool      IsPackageInstalled(Guid id);

        /// <summary>
        /// All installed packages
        /// </summary>
        IPackage[] InstalledPackages { get; }

        /// <summary>
        /// Returns all installed packages by a specific publisher
        /// </summary>
        /// <param name="publisherName">Name of the publisher</param>
        IPackage[] InstalledPackagesOfPublisher(string publisherName);

        /// <summary>
        /// Opens a package for installation
        /// </summary>
        /// <param name="url">URL of the package</param>
        IPackage   OpenPackageForInstallation(string url);

        /// <summary>
        /// Installs a package
        /// </summary>
        void       Install  (IPackage pkg);
    }
}
