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

namespace SharpMedia.Components.Configuration
{
    /// <summary>
    /// A provider of a component
    /// </summary>
    public interface IComponentProvider
    {
        /// <summary>
        /// Obtain an instance from the supplied parameters
        /// </summary>
        /// <param name="componentDirectory">A directory of components</param>
        /// <param name="clientInstance">Client instance that requires the component</param>
        /// <param name="requirementName">Name of the requirement</param>
        /// <param name="requirementType">Type of the requirement</param>
        /// <returns>Component instance or null</returns>
        object   GetInstance(IComponentDirectory componentDirectory, object clientInstance, string requirementName, string requirementType);

        /// <summary>
        /// Types that this component provider can be matched against
        /// </summary>
        string[] MatchedTypes { get; }

        /// <summary>
        /// Name that may be matched agains
        /// </summary>
        string   MatchedName { get; }

        /// <summary>
        /// True if matching against this components' name is allowed
        /// </summary>
        bool MatchAgainstNameAllowed { get; }

        /// <summary>
        /// True if matching against this components' type(s) is allowed
        /// </summary>
        bool MatchAgainstTypeAllowed { get; }
    }
}
