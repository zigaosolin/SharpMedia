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
    /// <summary>
    /// This class is a wrapper around actual ITypedStream and allows more typesafe access to
    /// typed stream with a lot of additonal helpers.
    /// </summary>
    public class TypedStream<T> : IDisposable, IEnumerable<T>
    {
        #region Private Members
        ITypedStream internalTS;
        #endregion

        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="TypedStream&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="typedStream">The typed stream.</param>
        internal TypedStream(ITypedStream typedStream)
        {
            if (typedStream == null) throw new ArgumentNullException("Typed stream must be non-null.");
            if (!Common.IsTypeSameOrDerived(typeof(T), typedStream.StreamType))
            {
                throw new InvalidCastException("Invalid cast on typed stream.");
            }
            internalTS = typedStream;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <value>The object at index.</value>
        public T this[uint index]
        {
            get
            {
                return (T)internalTS.Read(index);
            }
            set
            {
                internalTS.Write(index, value);
            }
        }

        public TypedStream<T> AsReadOnly()
        {
            return new TypedStream<T>(internalTS.AsReadOnly());
        }

        /// <summary>
        /// Locations of objects.
        /// </summary>
        public uint[] ObjectLocations
        {
            get
            {
                return internalTS.ObjectLocations;
            }
        }

        /// <summary>
        /// Can access typed stream as a cached array. If stream is not packed,
        /// not all elements will be returned and some will be null.
        /// </summary>
        /// <remarks>Example is 3 element at indices 0, 1, 5. An array of 3 
        /// objects is returned with {0,1,null}.</remarks>
        public T[] Array
        {
            get
            {
                return Read(0, internalTS.Count);

            }
            set
            {
                WriteObjects(0, value as object[]);
            }
        }

        /// <summary>
        /// Access typed stream as a array that is repacked. This means that all elements
        /// are returned even if some are missing in between.
        /// </summary>
        /// <remarks>Example is 3 element at indices 0, 1, 5. An array of 3 objects is
        /// returned with {0,1,5}.</remarks>
        public T[] ArrayRepacked
        {
            get
            {
                uint[] locations = internalTS.ObjectLocations;

                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is single object.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is single object; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingleObject
        {
            get
            {
                return (internalTS.Flags & StreamOptions.SingleObject) != 0;
            }
        }

        /// <summary>
        /// Gets or sets the object at index 0. If single object stream, this is the
        /// only object that is allowed.
        /// </summary>
        /// <value>The object.</value>
        public T Object
        {
            get
            {
                return (T)Read(0);
            }
            set
            {
                Write(0, value);
            }
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Converts the typed stream.
        /// </summary>
        public TypedStream<TSomething> As<TSomething>()
        {
            return new TypedStream<TSomething>(internalTS);
        }

        /// <summary>
        /// Is it the correct type.
        /// </summary>
        public bool Is<TSomething>()
        {
            return Common.IsTypeSameOrDerived(internalTS.StreamType, typeof(T));
        }

        /// <summary>
        /// Is it the correct type.
        /// </summary>
        public bool Is(Type type)
        {
            return Common.IsTypeSameOrDerived(internalTS.StreamType, type);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            internalTS.Dispose();
        }

        #endregion

        #region ITypedStream Members

        public StreamOptions Flags
        {
            get { return internalTS.Flags; }
        }

        public Type StreamType
        {
            get { return internalTS.StreamType; }
        }

        public T Read(uint index)
        {
            return (T)internalTS.Read(index);
        }

        public ulong GetByteSize(uint index)
        {
            return internalTS.GetByteSize(index);
        }

        public ulong ByteSize
        {
            get { return internalTS.ByteSize; }
        }

        public ulong ByteOverheadSize
        {
            get { return internalTS.ByteOverheadSize; }
        }

        public T[] Read(uint startIndex, uint numObjects)
        {
            object[] objs = internalTS.Read(startIndex, numObjects);
            T[] ret = new T[numObjects];
            for (int i = 0; i < numObjects; i++)
            {
                ret[i] = (T)objs[i];
            }
            return ret;
        }

        public Type GetObjectType(uint index)
        {
            return internalTS.GetObjectType(index);
        }

        public void Write(uint index, object obj)
        {
            internalTS.Write(index, obj);
        }

        public void WriteObjects(uint index, object[] objects)
        {
            internalTS.WriteObjects(index, objects);
        }

        public void InsertBefore(uint index, object obj)
        {
            internalTS.InsertBefore(index, obj);
        }

        public void InsertAfter(uint index, object obj)
        {
            internalTS.InsertAfter(index, obj);
        }

        public void Erase(uint index, uint numObjects, bool makeEmpty)
        {
            internalTS.Erase(index, numObjects, makeEmpty);
        }

        public uint Count
        {
            get { return internalTS.Count; }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            // This works only if packed (this is not necessary).
            for (uint i = 0; i < internalTS.Count; i++)
            {
                yield return this[i];
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            // This works only if packed (this is not necessary).
            for (uint i = 0; i < internalTS.Count; i++)
            {
                yield return this[i];
            }
        }

        #endregion
    }
}
