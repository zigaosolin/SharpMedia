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

namespace SharpMedia.Database.Managed
{

    /// <summary>
    /// A typed stream container that serves as driver isolator, used by managed node and managed
    /// typed stream.
    /// </summary>
    internal class TypedStreamContainer
    {
        #region Private Members
        IDriverTypedStream internalTS;
        uint readCount = 0;
        uint writeCount = 0;
        string type;
        bool firstRun = true;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="inner">The internal typed stream.</param>
        public TypedStreamContainer(string t, IDriverTypedStream inner)
        {
            type = t;
            internalTS = inner;
        }

        /// <summary>
        /// Gets the typed stream.
        /// </summary>
        /// <value>The typed stream.</value>
        public IDriverTypedStream TypedStream
        {
            get
            {
                return internalTS;
            }
        }

        /// <summary>
        /// Gets the read count.
        /// </summary>
        /// <value>The read count.</value>
        public uint ReadCount
        {
            get
            {
                return readCount;
            }
        }

        /// <summary>
        /// Is the typed stream alive.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                lock (internalTS)
                {
                    return readCount + writeCount > 0;
                }
            }
        }

        /// <summary>
        /// Notifies that it was disposed.
        /// </summary>
        /// <param name="mode"></param>
        internal void NotifyDisposed(OpenMode mode)
        {
            lock (internalTS)
            {
                switch (mode)
                {
                    case OpenMode.Read:
                        readCount--;
                        break;
                    case OpenMode.ReadWrite:
                        readCount--;
                        writeCount--;
                        break;
                    case OpenMode.Write:
                        writeCount--;
                        break;
                }

                // Make sure we dispose it, it can be ignored.
                if (writeCount == 0 && readCount == 0)
                {
                    internalTS.Dispose();
                }
            }
        }


        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Creates new stream on container.
        /// </summary>
        /// <param name="mode">Mode of new stream.</param>
        /// <returns>Managed typed stream.</returns>
        public ManagedTypedStream Create(OpenMode mode)
        {
            lock (internalTS)
            {
                if (!firstRun)
                {
                    // Must recreate stream.
                    if (!IsAlive) return null;

                    if (writeCount > 0 && mode != OpenMode.Read)
                    {
                        throw new MultiWriteAccessException("Trying to access stream with more than one write access stream.");
                    }
                }
                firstRun = false;

                switch (mode)
                {
                    case OpenMode.Read:
                        readCount++;
                        break;
                    case OpenMode.ReadWrite:
                        readCount++;
                        writeCount++;
                        break;
                    case OpenMode.Write:
                        writeCount++;
                        break;
                }

                return new ManagedTypedStream(this, mode);
            }
        }
    }
}
