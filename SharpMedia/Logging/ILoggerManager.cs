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

namespace SharpMedia.Logging
{
    /// <summary>
    /// Logging options.
    /// </summary>
    [Flags]
    public enum LoggingOptions
    {
        SupressMessages = 1,
        SupressWarnings = 2,
        SupressErrors = 4,
        SupressAll = 1|2|4

    }

    /// <summary>
    /// A logger manager.
    /// </summary>
    public interface ILoggerManager
    {

        /// <summary>
        /// Obtains logger for specific type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ILogger this[Type type]
        {
            get;
        }


        /// <summary>
        /// Configures logger.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="options"></param>
        void Configure(Type type, LoggingOptions options);

    }
}
