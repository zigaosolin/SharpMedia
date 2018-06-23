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

namespace SharpMedia.Database
{
    /// <summary>
    /// Options for typed stream. Many of the options are just hints for the underlaying
    /// implementation and implementation is free to ignore hints.
    /// </summary>
    [Flags]
    public enum StreamOptions : int
    {
        /// <summary>
        /// No options.
        /// </summary>
        None = 0,

        /// <summary>
        /// The typed stream can compress objects (using whatever compression it wants). 
        /// Sometimes, you wish to hint it that the compression is desirable. Implementation
        /// is free to ignore this flag, or to compress data without this data specified.
        /// </summary>
        Compressed = 1,

        /// <summary>
        /// This flags allows derived classes to be stored in typed stream. Sometimes, it is
        /// desirable to store all types that derive from some base class in the same stream.
        /// </summary>
        /// <remarks>
        /// This actually allows creating very generic streams, such as stream with type "System.Object".
        /// We suggest against that because this way, you loose most of the benefits of object database.
        /// </remarks>
        AllowDerivedTypes = 2,

        /// <summary>
        /// A hint, will try to write all write all streams marked as interleaved.
        /// </summary>
        Interleaved = 4,

        /// <summary>
        /// Only one object is allowed in stream. This is restriction hint; some systems may be
        /// able to optimize this. 
        /// </summary>
        SingleObject = 8,

        /// <summary>
        /// The value is indexed. Indexing is done through reflection for [Indexed] attributes.
        /// </summary>
        Indexed = 16
    }

    /// <summary>
    /// Specifies the open mode of stream. Always force most restrictive mode because the database
    /// may optimize read-only usages or write usages may not even be allowed for some databases
    /// (especially remote databases).
    /// </summary>
    public enum OpenMode
    {
        /// <summary>
        /// Real-only access to stream. Multithreading is supported fully and all threads may read
        /// simuntaniously.
        /// </summary>
        Read,

        /// <summary>
        /// Read-Write access to stream. Only one thread can write, we allow multiple read threads.
        /// </summary>
        ReadWrite,

        /// <summary>
        /// Write-only access. Only one thread can open stream in this mode.
        /// </summary>
        Write
    }

}
