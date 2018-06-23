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

namespace SharpMedia.Components.Services
{
    /// <summary>
    /// Controls a Service
    /// </summary>
    public interface IServiceControl
    {
        /// <summary>
        /// Stop the service
        /// </summary>
        /// <returns>True for the service</returns>
        bool Stop();

        /// <summary>
        /// Start the service
        /// </summary>
        /// <returns></returns>
        bool Start();

        /// <summary>
        /// True if started
        /// </summary>
        bool Started { get; }

        /// <summary>
        /// True if this service is to be started automatically, at
        /// system startup
        /// </summary>
        bool AutomaticStartup
        {
            get; set;
        }
    }
}
