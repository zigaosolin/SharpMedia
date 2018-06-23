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
using SharpMedia.AspectOriented;

namespace SharpMedia.Database.Aspects
{



    /// <summary>
    /// Typed stream class represents strongly typed object information. Through
    /// this class, you can read and write objects to stream. Stream is always a
    /// sequence of zero or more objects of the same type (or the same base type in
    /// case of interface streams). All streams are created by <see cref="INode"/>.
    /// </summary>
    internal interface ITypedStream : IDisposable
    {

        /// <summary>
        /// The flags priority.
        /// </summary>
        /// <remarks>
        /// Setting new flags may be ignored by implementation. Try retrieving flags to check
        /// if they were really set.
        /// </remarks>
        StreamOptions Flags
        {
            get;
        }

        /// <summary>
        /// The type of the stream; e.g. type of objects/structs it contains.
        /// </summary>
        Type StreamType
        {
            get;
        }
    
        
        /// <summary>
        /// Reads object at index.
        /// </summary>
        /// <param name="index">The index to read.</param>
        /// <returns>The object read, or null.</returns>
        Object Read(uint index);

        /// <summary>
        /// The byte size of specific object.
        /// </summary>
        /// <param name="index">Index of object.</param>
        /// <returns>The count of bytes.</returns>
        ulong GetByteSize(uint index);

        /// <summary>
        /// The byte size of this stream, the same as adding all the
        /// objects together.
        /// </summary>
        /// <returns>The size of stream.</returns>
        ulong ByteSize
        {
            get;
        }

        /// <summary>
        /// The byte overhead size, the number of bytes occupied by additional
        /// structures.
        /// </summary>
        ulong ByteOverheadSize
        {
            get;
        }
    
        /// <summary>
        /// Reads several objects at once.
        /// </summary>
        /// <remarks>
        /// Reading several objects at once may be faster then reading one by one.
        /// </remarks>
        /// <param name="startIndex">The index of first object to read.</param>
        /// <param name="numObjects">The number of objects to read.</param>
        /// <returns>The collection of objects read.</returns>
        Object[] Read(uint startIndex, uint numObjects);
    
        /// <summary>
        /// Only useful in conjuction to AllowDerivedTypes flag.
        /// </summary>
        /// <param name="index">The index of object.</param>
        /// <returns>Returns the actual type of object
        /// to read, or null if no object available.</returns>
        Type GetObjectType(uint index);
    
        /// <summary>
        /// Write command overwrites the object that exist at current seek
        /// position. Increments current seek position.
        /// </summary>
        /// <param name="index">Index where to write.</param>
        /// <param name="obj">The object to write.</param>
        void Write(uint index, Object obj);

    
        /// <summary>
        /// Writes several object at once, in sequence indices.
        /// </summary>
        /// <param name="index">The index to write first object, next is written at index+1 ...</param>
        /// <param name="objects">The object to write.</param>
        /// <remarks>On journalled system, multiple-object write is considered atomic
        /// operation if the database system does not specify otherwise.</remarks>
        void WriteObjects(uint index, [NotNull] Object[] objects);
    

        /// <summary>
        /// Inserts object before a certain index. All objects following the
        /// index position have their indices incremented.
        /// </summary>
        /// <param name="index">The index where to insert object.</param>
        /// <param name="obj">The object to insert. It is valid to insert object
        /// null object.</param>
        /// <remarks>Inserting cost is implementation dependant, however it is not
        /// recommended.</remarks>
        void InsertBefore(uint index, Object obj);
    
        /// <summary>
        /// Inserts object after a certain index. All objects following the
        /// index position have their indices incremented.
        /// </summary>
        /// <param name="index">The index where to insert object.</param>
        /// <param name="obj">The object to insert. It is valid to insert null object.</param>
        /// <remarks>Inserting cost is implementation dependant, however it is not
        /// recommended.</remarks>
        void InsertAfter(uint index, Object obj);
    

    
        /// <summary>
        /// Erases object from stream, as an atomic operation.
        /// </summary>
        /// <param name="index">The starting index.</param>
        /// <param name="numObjects">The number of sequent objects.</param>
        /// <param name="makeEmpty">If makeEmpty is true, all objects having greater indices
        /// keep their indices and indices of deleted objects are considered empty; if makeEmpty
        /// is false, all objects following have their index position substracted by numObjects.</param>
        /// <example>
        /// The following code has the objects at indices 0:a, 1:b, 2:c, 3:d, 4:e. Calling
        /// <code>
        /// ITypedStream stream = GetStream();
        /// stream.Erase(1,2,false);
        /// </code>
        /// will result in objects b and c being deleted and stream will have the following
        /// structure: 0:a,1:d,2:e.
        /// </example>
        void Erase(uint index, uint numObjects, bool makeEmpty);

        /// <summary>
        /// Obtains locations of all objects in typed stream.
        /// </summary>
        uint[] ObjectLocations { get; }

        /// <summary>
        /// Copies data to another typed stream.
        /// </summary>
        /// <param name="stream">Must be compatible in types.</param>
        /// <remarks>Data at specific indices is overwritten.</remarks>
        void CopyTo([NotNull] ITypedStream stream);

        /// <summary>
        /// The count of objects in stream.
        /// </summary>
        uint Count 
        {
           get;
        }

        /// <summary>
        /// As read-only wrapper.
        /// </summary>
        /// <returns></returns>
        ITypedStream AsReadOnly();
    
    }
}
