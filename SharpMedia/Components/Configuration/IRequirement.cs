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
    /// A configuration requirement
    /// </summary>
    public interface IRequirement
    {
        /// <summary>
        /// True if the requirement does not allow for a type-level search,
        /// usually when value types are requested
        /// </summary>
        bool ValueTypesOnly { get; }

        /// <summary>
        /// True if this requirement is a hard requirement, i.e. not satisfying it
        /// will raise a configuration error
        /// </summary>
        bool Hard { get; }

        /// <summary>
        /// Name of the requirement
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Type name of the requirement
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Sets the value of the requirement
        /// </summary>
        /// <param name="value"></param>
        void SetValue(object value);

        /// <summary>
        /// True if the value has been set and is valid
        /// </summary>
        bool Satisfied { get; }

        /// <summary>
        /// The default value used by the engine if no others can be found
        /// </summary>
        IConfigurationValue Default { get; set; }
    }
}
