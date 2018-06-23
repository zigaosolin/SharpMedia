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

namespace SharpMedia.Database.Indexing
{

    /// <summary>
    /// Contains all ind
    /// </summary>
    [Serializable]
    public sealed class IndexTable : IEnumerable<KeyValuePair<Type, StreamIndexTable>>
    {
        #region Private Members
        SortedDictionary<string, StreamIndexTable> streamTable = 
            new SortedDictionary<string, StreamIndexTable>();
        #endregion

        #region Public Members

        /// <summary>
        /// Creates an empty index table.
        /// </summary>
        public IndexTable()
        {
        }

        /// <summary>
        /// Typed helper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public StreamIndexTable GetTypedTable<T>()
        {
            return this[typeof(T)];
        }

        /// <summary>
        /// Adds an object to index.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        public void AddObject(Type type, object obj)
        {

        }

        /// <summary>
        /// Adds a prepared object to index.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="primary"></param>
        /// <param name="indexedData"></param>
        public void AddObject(Type type, uint primary, Dictionary<string, object> indexedData)
        {

        }

        /// <summary>
        /// Accessor to tables.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StreamIndexTable this[Type type]
        {
            get
            {
                StreamIndexTable val;
                if (streamTable.TryGetValue(type.FullName, out val))
                {
                    return val;
                }
                return null;
            }
            [param: NotNull]
            set
            {
                streamTable[type.FullName] = value;
            }
        }


        #endregion

        #region IEnumerable<KeyValuePair<Type,StreamIndexTable>> Members

        public IEnumerator<KeyValuePair<Type, StreamIndexTable>> GetEnumerator()
        {
            foreach (KeyValuePair<string, StreamIndexTable> keyValue in this.streamTable)
            {
                yield return new KeyValuePair<Type, StreamIndexTable>(
                    Type.GetType(keyValue.Key), keyValue.Value);
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (KeyValuePair<string, StreamIndexTable> keyValue in this.streamTable)
            {
                yield return new KeyValuePair<Type, StreamIndexTable>(
                    Type.GetType(keyValue.Key), keyValue.Value);
            }
        }

        #endregion
    }
}
