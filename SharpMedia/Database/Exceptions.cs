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
using System.Runtime.Serialization;

namespace SharpMedia.Database
{

    /// <summary>
    /// A mount exception.
    /// </summary>
    public class MountException : DatabaseException
    {
        public MountException(string message)
            : base(message)
        {
        }

        public MountException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected MountException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// An unmount exception.
    /// </summary>
    public class UnMountException : DatabaseException
    {
        public UnMountException(string message)
            : base(message)
        {
        }

        public UnMountException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected UnMountException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
    
    [Serializable]
    public class DatabaseException : Exception
    {
        public DatabaseException() { }
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception inner) : base(message, inner) { }
        protected DatabaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown when name is invalid (includes invalid symbols).
    /// </summary>
    [Serializable]
    public class InvalidNameException : DatabaseException
    {
        public InvalidNameException() { }
        public InvalidNameException(string message) : base(message) { }
        public InvalidNameException(string message, Exception inner) : base(message, inner) { }
        protected InvalidNameException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown when path is invalid (includes invalid symbols).
    /// </summary>
    [Serializable]
    public class InvalidPathException : DatabaseException
    {
        public InvalidPathException() { }
        public InvalidPathException(string message) : base(message) { }
        public InvalidPathException(string message, Exception inner) : base(message, inner) { }
        protected InvalidPathException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown when a specified node was not found
    /// </summary>
    [Serializable]
    public class NodeNotFoundException : DatabaseException
    {
        public NodeNotFoundException()            : base()     { }
        public NodeNotFoundException(string info) : base(info) { }
        protected NodeNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public static NodeNotFoundException FromPath(string path)
        {
            return new NodeNotFoundException("Node " + path + " not found");
        }
    }

    /// <summary>
    /// Thrown when a specified typed stream was not found
    /// </summary>
    [Serializable]
    public class TypedStreamNotFoundException : DatabaseException
    {
        public TypedStreamNotFoundException()            : base()     { }
        public TypedStreamNotFoundException(string info) : base(info) { }
        protected TypedStreamNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public static TypedStreamNotFoundException FromType(Type type)
        {
            return new TypedStreamNotFoundException("Typed stream " + type + " not found");
        }
    }

    /// <summary>
    /// Thrown when a specified index in a typed stream was not found
    /// </summary>
    [Serializable]
    public class ObjectIndexNotFoundException : DatabaseException
    {
        public ObjectIndexNotFoundException() : base() { }
        public ObjectIndexNotFoundException(string info) : base(info) { }
        protected ObjectIndexNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public static ObjectIndexNotFoundException FromTypeAndIndex(Type type, int index)
        {
            return new ObjectIndexNotFoundException("Object in typed stream " + type + " with index " + index + " not found");
        }
    }

    /// <summary>
    /// Thrown when multiple write accesses are issued.
    /// </summary>
    [Serializable]
    public class MultiWriteAccessException : DatabaseException
    {
        public MultiWriteAccessException() { }
        public MultiWriteAccessException(string message) : base(message) { }
        public MultiWriteAccessException(string message, Exception inner) : base(message, inner) { }
        protected MultiWriteAccessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown when trying to create existing typed stream.
    /// </summary>
    [Serializable]
    public class TypedStreamAlreadyExistsException : DatabaseException
    {

        public TypedStreamAlreadyExistsException() { }
        public TypedStreamAlreadyExistsException(string message) : base(message) { }
        public TypedStreamAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected TypedStreamAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}