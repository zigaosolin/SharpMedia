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

namespace SharpMedia.Logging.Local
{
    /// <summary>
    /// A logger.
    /// </summary>
    internal class LocalLogger : ILogger
    {
        #region Private Members
        LocalLoggerManager manager;
        LoggingOptions options;
        #endregion

        #region Public Members

        public LocalLogger(LocalLoggerManager manager)
        {
            this.manager = manager;
        }

        public LoggingOptions Options
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
            }
        }

        #endregion

        #region ILogger Members

        public void Warning(string message)
        {
            if ((options & LoggingOptions.SupressWarnings) == 0)
            {
                manager.Log.WriteLine("Warning: " + message);
            }
        }

        public void Error(string message)
        {
            if ((options & LoggingOptions.SupressErrors) == 0)
            {
                manager.Log.WriteLine("Error: " + message);
            }
        }

        public void Message(string message)
        {
            if ((options & LoggingOptions.SupressMessages) == 0)
            {
                manager.Log.WriteLine("Log: " + message);
            }
        }

        #endregion
    }
}
