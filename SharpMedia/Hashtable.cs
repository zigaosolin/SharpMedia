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

namespace SharpMedia
{
    /// <summary>
    /// Implements a generic version of hashtable with the same key/value and static size.
    /// </summary>
    /// <typeparam name="T">The type of data.</typeparam>
    [Serializable]
    public class Hashtable<T> : ICollection<T>, ICloneable<Hashtable<T>> where T : IEquatable<T>
    {
        #region Private Members
        int count = 0;
        List<T>[] hashtable;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Hashtable&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public Hashtable(uint size)
        {
            hashtable = new List<T>[size];
        }

        /// <summary>
        /// Private constructor (for clone).
        /// </summary>
        private Hashtable()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Obtains the item in hashtable that equal the specified item.
        /// </summary>
        /// <param name="item">The item to search.</param>
        /// <returns>Item in hashtable.</returns>
        public T IsInterned(T item)
        {
            int hashCode = item.GetHashCode() % hashtable.Length;
            if (hashtable[hashCode] == null) return default(T);

            for(int i = 0; i < hashtable[hashCode].Count; i++)
            {
                if (hashtable[hashCode][i].Equals(item))
                {
                    return hashtable[hashCode][i];
                }
            }
            return default(T);
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            count++;
            int hashCode = item.GetHashCode() % hashtable.Length;
            if(hashtable[hashCode]== null)
            {
                hashtable[hashCode] = new List<T>();
            }
            hashtable[hashCode].Add(item);
        }

        public void Clear()
        {
            for (int i = 0; i < hashtable.Length; i++)
            {
                if(hashtable[i] != null)
                    hashtable[i].Clear();
            }
        }

        public bool Contains(T item)
        {
            int hashCode = item.GetHashCode() % hashtable.Length;

            if (hashtable[hashCode] == null) return false;
            return hashtable[hashCode].Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            // We copy all to array.
            int offset = 0;
            for (int i = 0; i < hashtable.Length; i++)
            {
                if (hashtable[i] == null) continue;
                hashtable[i].CopyTo(array, arrayIndex + offset);
                offset += hashtable[i].Count;
            }
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            // We get our hash code.
            int hashCode = item.GetHashCode() % hashtable.Length;

            // We try to remove.
            List<T> toRemove = hashtable[hashCode];
            if (toRemove == null) return false;

            // Remove the item.
            if (toRemove.Remove(item))
            {
                count--;
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < hashtable.Length; i++)
            {
                if (hashtable[i] == null) continue;

                for (int j = 0; j < hashtable[i].Count; j++)
                {
                    yield return hashtable[i][j];
                }
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < hashtable.Length; i++)
            {
                if (hashtable[i] == null) continue;

                for (int j = 0; j < hashtable[i].Count; j++)
                {
                    yield return hashtable[i][j];
                }
            }
        }

        #endregion

        #region ICloneable<Hashtable<T>> Members

        public Hashtable<T> Clone()
        {
            Hashtable<T> h = new Hashtable<T>();
            h.count = count;
            h.hashtable = hashtable.Clone() as List<T>[];
            return h;
        }

        #endregion

    }
}
