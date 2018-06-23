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
using System.DirectoryServices;

namespace SharpMedia.Database.Ldap
{

    /// <summary>
    /// A Ldap typed stream.
    /// </summary>
    internal class LdapTypedStream : IDriverTypedStream
    {
        #region Private Members
        string type = string.Empty;
        List<string> properties;
        DirectoryEntry entry;
        ILdapIdentifier identifier;
        #endregion

        public LdapTypedStream(List<string> properties, DirectoryEntry entry, ILdapIdentifier identifier)
        {
            this.properties = properties;
            this.entry = entry;
            this.identifier = identifier;
        }


        #region IDriverTypedStream Members

        public bool UsesRaw
        {
            get { return false; }
        }

        public StreamOptions Flags
        {
            get { return StreamOptions.None; }
        }

        public string StreamType
        {
            get { return type; }
        }

        public object Read(uint index)
        {
            // We find index.
            throw new Exception();
        }

        public ulong GetByteSize(uint index)
        {
            return 0;
        }

        public ulong ByteSize
        {
            get { return 0; }
        }

        public ulong ByteOverheadSize
        {
            get { return 0; }
        }

        public object[] Read(uint startIndex, uint numObjects)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public uint[] ObjectLocations
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string GetObjectType(uint index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(uint index, string type, object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WriteObjects(uint index, string[] types, object[] objects)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void InsertBefore(uint index, string type, object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void InsertAfter(uint index, string type, object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Erase(uint index, uint numObjects, bool makeEmpty)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public uint Count
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
