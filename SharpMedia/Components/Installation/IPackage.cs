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
    /// Installation package
    /// </summary>
    public interface IPackage : IPublishedInfo
    {
        /// <summary>
        /// The name of the installation package
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Id of the package
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Sub packages
        /// </summary>
        IPackage[] SubPackages { get; }

        /// <summary>
        /// True if the package is selectable
        /// </summary>
        bool Selectable { get; }

        /// <summary>
        /// True if the package is selected
        /// </summary>
        bool Selected { get; set; }
    }
}