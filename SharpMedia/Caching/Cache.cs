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
using SharpMedia.Testing;
using System.Threading;

namespace SharpMedia.Caching
{

    /// <summary>
    /// A normal (one level) caching class.
    /// </summary>
    public class Cache<T> : ICache<T>
        where T : IComparable<T>
    {
        #region Private Members
        private class CachableData
        {
            public float Score;
            public ICacheable Data;
            public object[] EvalData;
        }

        float initialScore = 1.0f;
        float maxScore = 10.0f;
        object syncRoot = new object();
        IEvaluator<T>[] evaluators;
        SortedDictionary<T, CachableData> sorted = new SortedDictionary<T,CachableData>();
        DateTime lastUpdate = DateTime.Now;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Cache&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="initial">The initial score of resources.</param>
        /// <param name="maxScore">Max score saturating.</param>
        /// <param name="evaluators">The evaluators.</param>
        public Cache(float initial, float maxScore, params IEvaluator<T>[] evaluators)
        {
            if (evaluators == null || evaluators.Length == 0)
            {
                throw new ArgumentException("evaluators");
            }
            this.initialScore = initial;
            this.evaluators = evaluators;
        }

        #endregion

        #region ICache<T> Members

        public bool Add(T key, ICacheable cacheable)
        {
            lock (syncRoot)
            {
                if (cacheable == null) throw new ArgumentNullException("cacheable");

                CachableData dummy;
                if (sorted.TryGetValue(key, out dummy)) return false;

                // Cacheable was cached, must throw exception if already cached.
                cacheable.Cached();

                // Create data.
                CachableData data = new CachableData();
                data.Data = cacheable;
                data.Score = initialScore;
                data.EvalData = new object[evaluators.Length];
                for (int i = 0; i < evaluators.Length; i++)
                {
                    data.EvalData[i] = evaluators[i].Data;
                }

                // Add touched event.
                data.Data.OnTouch += new Action<ICacheable>(delegate(ICacheable unused)
                {
                    lock (syncRoot)
                    {
                        // Perform the touch.
                        for (int j = 0; j < evaluators.Length; j++)
                        {
                            data.Score = evaluators[j].Touch(data.EvalData[j], data.Score);
                        }

                        // Touching certainly ensures at least 0.0f value.
                        data.Score = Math.MathHelper.Max(0.0f, data.Score);
                        data.Score = Math.MathHelper.Min(maxScore, data.Score);
                    }
                });

                
                // Resource is created in touched state, no need to re-touch. This
                // method may throw if key already exists, but we already prechecked for uniquness.
                sorted.Add(key, data);

                return true;
                
            }
        }

        public void ReLink(T key, T newKey, ICacheable c)
        {
            lock (syncRoot)
            {
                CachableData data;
                if (sorted.TryGetValue(key, out data))
                {
                    // We first add to new collection.
                    sorted.Add(newKey, data);

                    // And remove (must succeed).
                    sorted.Remove(key);
                }
                else
                {
                    // Must insert it.
                    Add(newKey, c);
                }
            }
        }

        public void Evict(T key)
        {
            lock (syncRoot)
            {
                CachableData data;
                if (sorted.TryGetValue(key, out data))
                {
                    // Trigger evict event.
                    data.Data.Evict();

                    // Removes the key.
                    sorted.Remove(key);
                }
            }
        }

        public void EvictAll()
        {
            lock (syncRoot)
            {
                foreach (KeyValuePair<T, CachableData> pair in sorted)
                {
                    pair.Value.Data.Evict();
                }
                sorted.Clear();
            }
        }

        public ICacheable Find(T key)
        {
            lock (syncRoot)
            {
                CachableData data;
                if (sorted.TryGetValue(key, out data))
                {
                    return data.Data;
                }
                return null;
            }
        }

        public uint Update()
        {
            lock (syncRoot)
            {
                List<T> toDelete = new List<T>();

                DateTime nowTime = DateTime.Now;
                TimeSpan span = nowTime - this.lastUpdate;

                // We go through all.
                foreach (KeyValuePair<T, CachableData> pair in sorted)
                {
                    CachableData c = pair.Value;

                    for (int i = 0; i < evaluators.Length; i++)
                    {
                        c.Score = evaluators[i].Update(c.EvalData[i], span, c.Score);
                    }

                    if (c.Score < 0.0f) {
                        toDelete.Add(pair.Key);
                        c.Data.Dispose();
                        continue;
                    }

                    c.Score = Math.MathHelper.Min(maxScore, c.Score);
                }

                // We delete all keys.
                for (int i = 0; i < toDelete.Count; i++)
                {
                    sorted.Remove(toDelete[i]);
                }


                lastUpdate = nowTime;

                return (uint)toDelete.Count;
            }
        }

        public void Touch(ICacheable cacheable)
        {
            cacheable.Touch();
        }


        public ICacheable FindAndTouch(T key)
        {
            lock(syncRoot)
            {
                ICacheable c = this.Find(key);
                if(c != null) c.Touch();
                return c;
            }
            
        }

        public uint ItemCount
        {
            get
            {
                lock (syncRoot) 
                {
                    return (uint)sorted.Count;
                }
            }
        }

        public ulong MaxCacheSize
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class CacheTest
    {

        public class X : ICacheable
        {
            Action<ICacheable> touchEvent;
            CacheableState state = CacheableState.Evicted;

            #region ICacheable Members

            public void Touch()
            {
                Action<ICacheable> t = touchEvent;
                if (t != null)
                {
                    t(this);
                }
            }

            public void Evict()
            {
                if (state == CacheableState.Normal)
                {
                    state = CacheableState.Evicted;
                    touchEvent = null;
                }
            }

            public void Cached()
            {
                if (state != CacheableState.Evicted)
                {
                    throw new Exception();
                }
                state = CacheableState.Normal;
            }

            public CacheableState State
            {
                get { return state; }
            }

            public event Action<ICacheable> OnTouch
            {
                add
                {
                    touchEvent += value;
                }
                remove
                {
                    touchEvent -= value;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                state = CacheableState.Disposed;
            }

            #endregion
        }

        [CorrectnessTest]
        public void Cache()
        {
            Cache<string> c = new Cache<string>(1.0f, 10.0f, new LRU<string>(1.0f, 1.0f));

            for (int i = 0; i < 1000; i++)
            {
                c.Add(i.ToString(), new X());
                c.FindAndTouch((i-5).ToString());
                c.Update();
                Console.WriteLine(c.ItemCount);
                Thread.Sleep(5);
            }

            
        }

    }
#endif
}
