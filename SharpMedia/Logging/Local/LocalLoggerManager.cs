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
using System.IO;

namespace SharpMedia.Logging.Local
{
    /// <summary>
    /// A local logger manager.
    /// </summary>
    public sealed class LocalLoggerManager : ILoggerManager, IDisposable
    {
        #region Private Members
        object syncRoot = new object();
        bool disposed = false;
        StreamWriter log;

        // A logger collection.
        SortedDictionary<string, ILogger> logger =
            new SortedDictionary<string, ILogger>();

        #endregion

        #region Private Members

        ~LocalLoggerManager()
        {
            Dispose(true);
        }

        void Dispose(bool fin)
        {
            if (disposed) return;

            // Ensure thread safety.
            StreamWriter t = log;

            // Make sure writes still valid.
            log = new StreamWriter(new FakeStream());
            t.Dispose();

            disposed = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The log used. You must not dispose it manually.
        /// </summary>
        public StreamWriter Log
        {
            get
            {
                return log;
            }
        }

        #endregion

        #region Configuration

        public LocalLoggerManager(string file)
        {
            this.log = File.CreateText(file);
            
        }

        public LocalLoggerManager(StreamWriter log)
        {
            this.log = log;
        }

        #endregion

        #region ILoggerManager Members

        public ILogger this[Type type]
        {
            get
            {
                lock (syncRoot)
                {
                    ILogger searched;
                    if (logger.TryGetValue(type.FullName, out searched)) return searched;

                    // We create it.
                    searched = new LocalLogger(this);
                    logger[type.FullName] = searched;
                    return searched;
                }
            }
        }

        public void Configure(Type type, LoggingOptions options)
        {
            lock (syncRoot)
            {
                LocalLogger l = this[type] as LocalLogger;
                l.Options = options;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (syncRoot)
            {
                Dispose(false);
            }
        }

        #endregion
    }
}
