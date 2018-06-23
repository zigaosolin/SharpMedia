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
using SharpMedia.Database.Managed;
using SharpMedia.Database.Aspects;

namespace SharpMedia.Database
{
    internal class SingleValueTypedStreamInt : ITypedStream
    {
        object value;
        Type   type;
        uint serializedSize = 0;

        public SingleValueTypedStreamInt(Type t, object v)
        {
            this.value = v;
            this.type = t;

            try
            {
                serializedSize = (uint)Common.GetSerializedObjectSize(value);
            }
            catch (Exception) { }
        }

        #region ITypedStream Members

        public StreamOptions Flags
        {
            get { return StreamOptions.AllowDerivedTypes | StreamOptions.SingleObject; }
        }

        public Type StreamType
        {
            get { return type; }
        }

        public object Read(uint index)
        {
            if (index == 0)
            {
                return value;
            }

            return null;
        }

        public ulong GetByteSize(uint index)
        {
            return index == 0 ? serializedSize : 0;
        }

        public ulong ByteSize
        {
            get { return serializedSize; }
        }

        public ulong ByteOverheadSize
        {
            get { return 0; }
        }

        public object[] Read(uint startIndex, uint numObjects)
        {
            return startIndex == 0 && numObjects == 1 ? new object[] { value } : new object[0];
        }

        public Type GetObjectType(uint index)
        {
            return index == 0 ? type : null;
        }

        public void Write(uint index, object obj)
        {
            if (index == 0) { value = obj; }
        }

        public void WriteObjects(uint index, object[] objects)
        {
            if (index == 0 && objects.Length == 1)
            {
                value = objects[0];
            }
        }

        public void InsertBefore(uint index, object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void InsertAfter(uint index, object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Erase(uint index, uint numObjects, bool makeEmpty)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        static readonly uint[] objectLocations = new uint[] { 0 };
        public uint[] ObjectLocations
        {
            get { return objectLocations; }
        }

        public void CopyTo(ITypedStream stream)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public uint Count
        {
            get { return 1; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region ITypedStream Members


        public ITypedStream AsReadOnly()
        {
            return this;
        }

        #endregion
    }

    /// <summary>
    /// A Single value typed stream "accessor"
    /// </summary>
    public class SingleValueTypedStream
    {
        /// <summary>
        /// Creates a new single value typed stream
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="value">Value</param>
        /// <returns>New Typed Stream with a value</returns>
        public static TypedStream<T> Create<T>(T value)
        {
            return new TypedStream<T>(
                new SingleValueTypedStreamInt(typeof(T), value));
        }
    }
}
