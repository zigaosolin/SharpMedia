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
using System.Collections.Specialized;

namespace SharpMedia.Database
{
    /// <summary>
    /// A driver typed stream must implement this class.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IDriverTypedStream : IDisposable
    {
        bool UsesRaw { get; }
        StreamOptions Flags { get; }
        string StreamType { get; }
        object Read(uint index);
        ulong GetByteSize(uint index);
        ulong ByteSize { get; }
        ulong ByteOverheadSize { get; }
        object[] Read(uint startIndex, uint numObjects);
        uint[] ObjectLocations { get; }
        string GetObjectType(uint index);
        void Write(uint index, string type, object obj);
        void WriteObjects(uint index, string[] types, object[] objects);
        void InsertBefore(uint index, string type, object obj);
        void InsertAfter(uint index, string type, object obj);
        void Erase(uint index, uint numObjects, bool makeEmpty);
        uint Count { get; }
    }

    /// <summary>
    ///  A driver node must implement this class.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IDriverNode : IDisposable
    {
        string Name { set; }
        string DefaultType { get; }
        ulong ByteOverhead { get; }
        IDriverTypedStream GetTypedStream(OpenMode mode, string type);
        void AddTypedStream(string type, StreamOptions flags);
        void RemoveTypedStream(string type);
        void ChangeDefaultStream(string type);
        ulong Version { get; }
        IDriverNode GetVersion(ulong version);
        IDriverNode CreateNewVersion(string defaultType, StreamOptions flags);
        IDriverNode Find(string path);
        IDriverNode CreateChild(string name, string defaultType, StreamOptions flags);
        void DeleteChild(string name);
        void DeleteVersion(ulong version);
        DateTime CreationTime { get; }
        DateTime LastModifiedTime { get; }
        DateTime LastReadTime { get; }
        string PhysicalLocation { get; }
        StringCollection TypedStreams { get; }
        StringCollection Children { get; }
        ulong[] AvailableVersions { get; }
    }
}
