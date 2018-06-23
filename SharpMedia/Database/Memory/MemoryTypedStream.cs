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

namespace SharpMedia.Database.Memory
{

    /// <summary>
    /// Implementation of in memory typed stream.
    /// </summary>
    [Serializable]
    internal class MemoryTypedStream : IDriverTypedStream
    {
        [Serializable]
        private class TSObject
        {
            public string Type = string.Empty;
            public byte[] Data;

            /// <summary>
            /// Initializes a new instance of the <see cref="TSObject"/> class.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="data">The data.</param>
            public TSObject(string type, byte[] data)
            {
                Type = type;
                Data = data;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TSObject"/> class.
            /// </summary>
            /// <param name="data">The data.</param>
            public TSObject(byte[] data)
            {
                Data = data;
            }
        }

        #region Private Members
        string                      type;
        StreamOptions               options;
        SortedList<uint, TSObject>  objects = new SortedList<uint, TSObject>();
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryTypedStream"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="op">The options.</param>
        public MemoryTypedStream(string type, StreamOptions op)
        {
            this.type = type;
            this.options = op;
        }

        #endregion

        #region IDriverTypedStream Members

        public uint[] ObjectLocations
        {
            get
            {
                IList<uint> keys = objects.Keys;
                uint[] retVal = new uint[keys.Count];
                for (int i = 0; i < keys.Count; i++)
                {
                    retVal[i] = keys[i];
                }

                return retVal;
            }
        }

        public StreamOptions Flags
        {
            get { return options; }
        }

        public string StreamType
        {
            get { return type; }
        }

        public ulong GetByteSize(uint index)
        {
            TSObject value;
            if (objects.TryGetValue(index, out value))
            {
                return (ulong)value.Data.LongLength;
            }
            return 0;
        }

        public ulong ByteSize
        {
            get 
            {
                ulong size = 0;
                foreach (KeyValuePair<uint, TSObject> v in objects)
                {
                    size += (ulong)v.Value.Data.LongLength;
                }
                return size;
            }
        }

        public ulong ByteOverheadSize
        {
            get { return 0; }
        }

        public string GetObjectType(uint index)
        {
            TSObject value;
            if (objects.TryGetValue(index, out value))
            {
                if (value.Type == string.Empty) return type;
                return value.Type;
            }
            return string.Empty;
        }

        public void Erase(uint index, uint numObjects, bool makeEmpty)
        {
            if (!makeEmpty)
            {
                SortedList<uint, TSObject> objs = new SortedList<uint, TSObject>(objects.Count + 1);

                // We reinsert them all.
                foreach (KeyValuePair<uint, TSObject> pair in objects)
                {
                    if (pair.Key > index && pair.Key < index + numObjects)
                    {
                        // This one is removed.
                        continue;
                    }
                    else if (pair.Key > index)
                    {
                        objs.Add(pair.Key - numObjects, pair.Value);
                    }
                    else
                    {
                        objs.Add(pair.Key, pair.Value);
                    }
                }

                // Release previous objects.
                objects = objs;
            }
            else
            {
                for (uint i = 0; i < numObjects; i++)
                {
                    objects.Remove(index + i);
                }
            }
        }

        public uint Count
        {
            get { return (uint)objects.Count; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IDriverTypedStream Members

        public bool UsesRaw
        {
            get { return true; }
        }

        public void Write(uint index, string type, object obj)
        {
            objects[index] = new TSObject(type, obj as byte[]);
        }

        public void WriteObjects(uint index, string[] types, object[] objects)
        {
            for (uint i = 0; i < objects.Length; i++)
            {
                Write(index + i, types[i], objects[i]);
            }
        }

        public void InsertBefore(uint index, string type, object @object)
        {
            byte[] obj = @object as byte[];

            SortedList<uint, TSObject> objs = new SortedList<uint, TSObject>(objects.Count + 1);

            // We reinsert them all.
            foreach (KeyValuePair<uint, TSObject> pair in objects)
            {
                if (pair.Key >= index)
                {
                    objs.Add(pair.Key + 1, pair.Value);
                }
                else
                {
                    objs.Add(pair.Key, pair.Value);
                }
            }

            // Insert the object.
            objs[index] = new TSObject(type, obj);

            // Release previous objects.
            objects = objs;
        }

        public void InsertAfter(uint index, string type, object @object)
        {
            byte[] obj = @object as byte[];

            SortedList<uint, TSObject> objs = new SortedList<uint, TSObject>(objects.Count + 1);

            // We reinsert them all.
            foreach (KeyValuePair<uint, TSObject> pair in objects)
            {
                if (pair.Key > index)
                {
                    objs.Add(pair.Key + 1, pair.Value);
                }
                else
                {
                    objs.Add(pair.Key, pair.Value);
                }
            }

            // Insert the object.
            objs[index + 1] = new TSObject(type, obj);

            // Release previous objects.
            objects = objs;
        }

        public object Read(uint index)
        {
            TSObject value;
            if (objects.TryGetValue(index, out value)) return value.Data;
            return null;
        }

        public object[] Read(uint startIndex, uint numObjects)
        {
            /* QUESTION: Why did this have new object[numObjects][] ?? */
            object[] array = new object[numObjects];
            for (uint i = 0; i < numObjects; i++)
            {
                array[i] = Read(startIndex + i);
            }
            return array;
        }

        #endregion
    }
}
