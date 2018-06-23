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
    /// Requires configuration
    /// </summary>
    public interface IRequiresConfiguration : ICloneable<IRequiresConfiguration>
    {
        /// <summary>
        /// Initializers that may be invoked to satisfy requirements in advance
        /// </summary>
        IRequiresConfiguration[] PreInitializers { get; }

        /// <summary>
        /// Requirements. Correct ordering is done by the IRequiresConfiguration implementation
        /// </summary>
        IRequirement[]           Requirements { get; }

        /// <summary>
        /// Object instance that was created by filling in the configuration requirements
        /// </summary>
        object                   Instance { get; }

        /// <summary>
        /// Object instance that is available for manual configuration and manipulation before it is configured
        /// </summary>
        object                   UnconfiguredInstance { get; }

        /// <summary>
        /// Executes a pre-initializer and binds its result to this component
        /// </summary>
        /// <param name="config">pre initializer</param>
        void Preinitialize(IRequiresConfiguration config);
    }
}
