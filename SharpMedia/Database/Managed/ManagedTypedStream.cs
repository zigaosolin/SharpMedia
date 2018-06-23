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
using SharpMedia.AspectOriented;
using SharpMedia.Database.Aspects;

namespace SharpMedia.Database.Managed
{
    /// <summary>
    /// Managed typed stream provides shared (multithreaded) abstraction of internal typed stream.
    /// </summary>
    internal class ManagedTypedStream : MarshalByRefObject, ITypedStream, IDriverTypedStream
    {
        #region Private Members
        TypedStreamContainer container;
        OpenMode mode;
        bool usesRaw;

        private void AssertNotDisposed()
        {
            if (container == null)
            {
                throw new ObjectDisposedException("Typed steam was already disposed.");
            }
        }

        private void AssertCorrectType(object obj)
        {
            // We must make sure object is of correct type.
            if (obj.GetType().FullName != container.Type)
            {
                if ((Inner.Flags & StreamOptions.AllowDerivedTypes) == 0)
                {
                    throw new InvalidCastException("Cannot cast the object to stream type.");
                }

                // We now make sure it is derived.
                if (!Common.IsTypeSameOrDerived(Type.GetType(container.Type), obj.GetType()))
                {
                    throw new InvalidCastException("Cannot cast the object to stream type.");
                }
            }
        }

        private IDriverTypedStream Inner
        {
            get
            {
                return container.TypedStream;
            }
        }


        private object SerializeObject(object obj)
        {
            if (obj is ITransparent)
            {
                if (((ITransparent)obj).IsTransparent(container.TypedStream)) return obj;
            }

            if (!usesRaw)
            {
                return obj;
            }
            return Common.SerializeToArray(obj);
        }

        private object DeserializeObject(object obj)
        {
            if (obj is ITransparent)
            {
                if (((ITransparent)obj).IsTransparent(container.TypedStream)) return obj;
            }

            if (!usesRaw) return obj;
            return Common.DeserializeFromArray(obj as byte[]);
        }

        #endregion

        #region ITypedStream Members

        public uint[] ObjectLocations
        {
            get
            {
                AssertNotDisposed();
                lock (Inner)
                {
                    return Inner.ObjectLocations;
                }
            }
        }

        public void CopyTo([NotNull] ITypedStream s)
        {
            AssertNotDisposed();
            ManagedTypedStream stream = s as ManagedTypedStream;
            lock (Inner)
            {
                stream.AssertNotDisposed();
                lock (stream.Inner)
                {
                    // Implementation notes: should be journalled in future.
                    uint[] locations = Inner.ObjectLocations;
                    for (int i = 0; i < locations.Length; i++)
                    {
                        string type = Inner.GetObjectType(locations[i]);

                        if (usesRaw)
                        {
                            byte[] data = Inner.Read(locations[i]) as byte[];
                            stream.Inner.Write(locations[i], type, data);
                        }
                        else
                        {
                            object data = Inner.Read(locations[i]);
                            stream.Inner.Write(locations[i], type, data);
                        }
                    }
                }
            }
        }

        public StreamOptions Flags
        {
            get
            {
                AssertNotDisposed();
                lock (Inner)
                {
                    return Inner.Flags;
                }
            }
        }

        public Type StreamType
        {
            get
            {
                AssertNotDisposed();
                lock (Inner)
                {
                    return Type.GetType(container.Type);
                }
            }
        }

        public object Read(uint index)
        {
            if (mode == OpenMode.Write) throw new NotSupportedException("Cannot read from non-read stream.");
            AssertNotDisposed();
            lock (Inner)
            {
                object data = Inner.Read(index);
                if (data == null) return null;
                return DeserializeObject(data);
            }
        }

        public object[] Read(uint startIndex, uint numObjects)
        {
            if (mode == OpenMode.Write) throw new NotSupportedException("Cannot read from non-read stream.");
            AssertNotDisposed();
            lock (Inner)
            {
                object[] r = new object[numObjects];
                object[] b = Inner.Read(startIndex, numObjects);
                for (uint i = 0; i < numObjects; i++)
                {
                    r[i] = DeserializeObject(b[i]);
                }
                return r;
            }
        }

        public Type GetObjectType(uint index)
        {
            AssertNotDisposed();

            lock (Inner)
            {
                string typename = Inner.GetObjectType(index);
                if (typename != null)
                {
                    return Type.GetType(typename);
                }

                // This means object type query not available, we must read
                // the object (this should be avoided by driver).
                object obj = Inner.Read(index);
                if (obj == null)
                {
                    return null;
                }
                return DeserializeObject(obj).GetType();
            }
        }

        public void Write(uint index, object obj)
        {
            if (mode == OpenMode.Read) throw new NotSupportedException("Cannot write to non-write stream.");
            AssertNotDisposed();
            lock (Inner)
            {
                if (obj == null)
                {
                    Inner.Erase(index, 1, true);
                }
                else
                {

                    AssertCorrectType(obj);
                    Inner.Write(index, obj.GetType().FullName, SerializeObject(obj));
                }
            }

        }

        public void WriteObjects(uint index, object[] objects)
        {
            if (mode == OpenMode.Read) throw new NotSupportedException("Cannot write to non-write stream.");
            if (objects == null) throw new ArgumentNullException("The object to write must be nonnull.");
            AssertNotDisposed();
            lock (Inner)
            {
                string[] types = new string[objects.Length];
                object[] data = new byte[objects.Length][];

                for (int i = 0; i < objects.Length; i++)
                {
                    types[i] = objects[i].GetType().FullName;
                    data[i] = SerializeObject(objects[i]);

                }

                Inner.WriteObjects(index, types, data);   
            }
        }

        public void InsertBefore(uint index, object obj)
        {
            if (mode == OpenMode.Read) throw new NotSupportedException("Cannot write to non-write stream.");
            AssertNotDisposed();

            lock (Inner)
            {
                AssertCorrectType(obj);
                Inner.InsertBefore(index, obj.GetType().FullName, SerializeObject(obj));
            }
        }

        public void InsertAfter(uint index, object obj)
        {
            if (mode == OpenMode.Read) throw new NotSupportedException("Cannot write to non-write stream.");
            AssertNotDisposed();

            lock (Inner)
            {
                AssertCorrectType(obj);

                Inner.InsertAfter(index, obj.GetType().FullName, SerializeObject(obj));
                
                
            }
        }

        public void Erase(uint index, uint numObjects, bool makeEmpty)
        {
            if (mode == OpenMode.Read) throw new NotSupportedException("Cannot erase from non-write stream.");
            AssertNotDisposed();

            lock (Inner)
            {
                Inner.Erase(index, numObjects, makeEmpty);
            }
        }

        public uint Count
        {
            get 
            { 
                AssertNotDisposed(); 
                lock (Inner) { return Inner.Count; }
            }
        }


        public ulong GetByteSize(uint index)
        {
            lock (Inner)
            {
                AssertNotDisposed();
                return Inner.GetByteSize(index);
            }
        }

        public ulong ByteSize
        {
            get {
                AssertNotDisposed();
                lock (Inner)
                {
                    return Inner.ByteSize;
                }
            }
        }

        public ulong ByteOverheadSize
        {
            get 
            {
                AssertNotDisposed();
                lock (Inner)
                {
                    return Inner.ByteOverheadSize;
                }
            }
        }

        public ITypedStream AsReadOnly()
        {
            AssertNotDisposed();
            lock (Inner)
            {
                if (mode == OpenMode.Write)
                {
                    throw new InvalidOperationException("Write-only stream does not have read privilegies.");
                }
                return container.Create(OpenMode.Read);
            }
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="c">Container.</param>
        /// <param name="openMode">The open mode.</param>
        internal ManagedTypedStream(TypedStreamContainer container, OpenMode openMode)
        {
            this.container = container;
            mode = openMode;
            usesRaw = Inner.UsesRaw;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ManagedTypedStream"/> is reclaimed by garbage collection.
        /// </summary>
        ~ManagedTypedStream()
        {
            if (!IsDisposed) Dispose();
        }

        /// <summary>
        /// Stream is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return container == null;
            }
        }

        public override string ToString()
        {
            AssertNotDisposed();
            lock (Inner)
            {
                string access = string.Empty;
                switch (mode)
                {
                    case OpenMode.Read: 
                        access = ": read";
                        break;
                    case OpenMode.ReadWrite:
                        access = ": read/write";
                        break;
                    case OpenMode.Write:
                        access = ": write";
                        break;
                }
                return container.Type + access;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            object tmp = Inner;
            if (tmp == null) throw new ObjectDisposedException("Cannot dispose already disposed object.");
            lock (tmp)
            {
                container.NotifyDisposed(mode);
                container = null;

                // No need for finalizer.
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region IDriverTypedStream Members

        string IDriverTypedStream.StreamType
        {
            get 
            {
                lock (Inner)
                {
                    return container.Type;
                }
            }
        }

        object IDriverTypedStream.Read(uint index)
        {
            lock (Inner)
            {
                return Inner.Read(index);
            }
        }

        object[] IDriverTypedStream.Read(uint startIndex, uint numObjects)
        {
            lock (Inner)
            {
                return Inner.Read(startIndex, numObjects);
            }
        }

        string IDriverTypedStream.GetObjectType(uint index)
        {
            lock (Inner)
            {
                return Inner.GetObjectType(index);
            }
        }

        void IDriverTypedStream.Write(uint index, string type, object obj)
        {
            lock (Inner)
            {
                Inner.Write(index, type, obj);
            }
        }

        void IDriverTypedStream.WriteObjects(uint index, string[] types, object[] objects)
        {
            lock (Inner)
            {
                Inner.WriteObjects(index, types, objects);
            }
        }

        void IDriverTypedStream.InsertBefore(uint index, string type, object obj)
        {
            lock (Inner)
            {
                Inner.InsertBefore(index, type, obj);
            }
        }

        void IDriverTypedStream.InsertAfter(uint index, string type, object obj)
        {
            lock (Inner)
            {
                Inner.InsertAfter(index, type, obj);
            }
        }

        bool IDriverTypedStream.UsesRaw
        {
            get
            {
                return Inner.UsesRaw;
            }


        }

        #endregion

    }

}
