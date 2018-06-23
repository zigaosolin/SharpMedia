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

namespace SharpMedia.Database.Host
{

    /// <summary>
    /// A null typed stream.
    /// </summary>
    internal class NullTypedStream : IDriverTypedStream
    {
        #region IDriverTypedStream Members

        public bool UsesRaw
        {
            get { return true; }
        }

        public StreamOptions Flags
        {
            get { return StreamOptions.None; }
        }

        public string StreamType
        {
            get { return typeof(object).FullName; }
        }

        public object Read(uint index)
        {
            return null;
        }

        public ulong GetByteSize(uint index)
        {
            return 0;
        }

        public ulong ByteSize
        {
            get { return 0;  }
        }

        public ulong ByteOverheadSize
        {
            get { return 0; }
        }

        public object[] Read(uint startIndex, uint numObjects)
        {
            return new object[numObjects];
        }

        public uint[] ObjectLocations
        {
            get { return new uint[0]; }
        }

        public string GetObjectType(uint index)
        {
            return null;
        }

        public void Write(uint index, string type, object obj)
        {
            throw new NotSupportedException("Cannot write to null stream.");
        }

        public void WriteObjects(uint index, string[] types, object[] objects)
        {
            throw new NotSupportedException("Cannot write to null stream.");
        }

        public void InsertBefore(uint index, string type, object obj)
        {
            throw new NotSupportedException("Cannot write to null stream.");
        }

        public void InsertAfter(uint index, string type, object obj)
        {
            throw new NotSupportedException("Cannot write to null stream.");
        }

        public void Erase(uint index, uint numObjects, bool makeEmpty)
        {
        }

        public uint Count
        {
            get { return 0; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
