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
using System.Reflection;

namespace SharpMedia.Database.Indexing
{
    /// <summary>
    /// Indexed typed table.
    /// </summary>
    [Serializable]
    public sealed class StreamIndexTable : IEnumerable<KeyValuePair<uint, Dictionary<string,object>>>
    {
        #region Private Members
        SortedDictionary<uint, Dictionary<string, object>> indexed =
            new SortedDictionary<uint, Dictionary<string, object>>();
        #endregion

        #region Static Members

        /// <summary>
        /// Extracts index data of object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> IndexData([NotNull] object obj)
        {
            Dictionary<string, object> indexed = new Dictionary<string, object>();

            // We go through all properties and fields of object and extract indexed.
            Type type = obj.GetType();

            PropertyInfo[] properties = type.GetProperties();
            FieldInfo[] fields = type.GetFields();

            // Properties
            for (int i = 0; i < properties.Length; i++)
            {
                if(properties[i].GetCustomAttributes(typeof(IndexedAttribute), true).Length == 0) continue;

                indexed[properties[i].Name] = properties[i].GetValue(obj, null);
            }

            // Fields.
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].GetCustomAttributes(typeof(IndexedAttribute), true).Length == 0) continue;

                indexed[fields[i].Name] = fields[i].GetValue(obj);
            }

            return indexed;
            
        }

        /// <summary>
        /// Constructs object's index.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> IndexOfObject<T>(StreamIndexTable table, 
            TypedStream<T> typedStream, uint index, out T obj)
        {
            obj = default(T);
            return null;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Obtains data for specific object.
        /// </summary>
        public Dictionary<string, object> this[uint index]
        {
            get
            {
                Dictionary<string, object> ret;
                if (indexed.TryGetValue(index, out ret))
                {
                    return ret;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    indexed.Remove(index);
                }
                else
                {
                    indexed[index] = value;
                }
            }
        }


        #endregion

        #region IEnumerable<KeyValuePair<uint,Dictionary<string,object>>> Members

        public IEnumerator<KeyValuePair<uint, Dictionary<string, object>>> GetEnumerator()
        {
            return indexed.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (indexed as System.Collections.IEnumerable).GetEnumerator();
        }

        #endregion
    }
}
