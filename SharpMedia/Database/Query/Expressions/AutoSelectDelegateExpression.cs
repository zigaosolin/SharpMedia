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

namespace SharpMedia.Database.Query.Expressions
{
    /// <summary>
    /// Automatically selection is used.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoSelectDelegateExpression<T> : IQueryExpression<T, T>
    {

       #region Private Members
        Predicate<T> isSatisfied;
        Predicate2<uint, Dictionary<string, object>> isIndexSatisfied;
        Action<List<T>> sort;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with auto-select select only.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectDelegateExpression()
        {
        }

        /// <summary>
        /// Constructor with satisfication and selection.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectDelegateExpression(Predicate<T> isSatisfied)
        {
            this.isSatisfied = isSatisfied;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied)
        {
            this.isIndexSatisfied = isIndexSatisfied;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  Predicate<T> isSatisfied)
        {
            this.isIndexSatisfied = isIndexSatisfied;
            this.isSatisfied = isSatisfied;
        }

        /// <summary>
        /// Constructor with select only.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectDelegateExpression(Action<List<T>> sort)
        {
            this.sort = sort;
        }

        /// <summary>
        /// Constructor with satisfication and selection.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectDelegateExpression(Predicate<T> isSatisfied,
                                 Action<List<T>> sort)
        {
            this.isSatisfied = isSatisfied;
            this.sort = sort;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                            Action<List<T>> sort)
        {
            this.isIndexSatisfied = isIndexSatisfied;
            this.sort = sort;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  Predicate<T> isSatisfied, Action<List<T>> sort)
        {
            this.isIndexSatisfied = isIndexSatisfied;
            this.isSatisfied = isSatisfied;
            this.sort = sort;
        }

        #endregion

        #region IQueryExpression<T> Members

        public virtual QueryFilter Filter
        {
            get
            {
                QueryFilter filter = QueryFilter.None;
                if (isSatisfied != null) filter |= QueryFilter.Object;
                if (isIndexSatisfied != null) filter |= QueryFilter.Index;
                if (sort != null) filter |= QueryFilter.Sort;

                return filter;
            }
        }

        public bool IsSatisfied(uint index, Dictionary<string, object> indexable)
        {
            Predicate2<uint, Dictionary<string, object>> t = isIndexSatisfied;
            if (t != null)
            {
                return t(index, indexable);
            }

            return true;
        }

        public bool IsSatisfied(T obj)
        {
            Predicate<T> t = isSatisfied;
            if (t != null)
            {
                return t(obj);
            }

            return true;
        }

        public T Select(T obj)
        {
            return obj;
        }

        public void Sort(List<T> results)
        {
            Action<List<T>> t = sort;
            if (t != null)
            {
                t(results);
            }
        }

        #endregion
    }



    /// <summary>
    /// Auto select with join.
    /// </summary>
    public class AutoSelectJoinDelegateExpression<T, Key> : AutoSelectDelegateExpression<T>, 
        IJoinExpression<T, T, Key> where Key : IComparable<Key>
    {
        #region Private Members
        JoinKeyDelegate<Key, T> joinDelegate;
        #endregion

        #region IJoinExpression<T,T,Key> Members

        public Key JoinKey(T obj)
        {
            JoinKeyDelegate<Key, T> t = joinDelegate;
            if (t != null)
            {
                return t(obj);
            }
            return default(Key);
        }

        public override QueryFilter Filter
        {
            get
            {
                return joinDelegate != null ? base.Filter | QueryFilter.JoinWithKey : base.Filter;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with auto-select select only.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectJoinDelegateExpression(JoinKeyDelegate<Key, T> join)
        {
            this.joinDelegate = join;
        }

        /// <summary>
        /// Constructor with satisfication and selection.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectJoinDelegateExpression(Predicate<T> isSatisfied, JoinKeyDelegate<Key, T> join)
            : base(isSatisfied)
        {
            this.joinDelegate = join;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectJoinDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                                JoinKeyDelegate<Key, T> join)
            : base(isIndexSatisfied)
        {
            this.joinDelegate = join;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectJoinDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  Predicate<T> isSatisfied, JoinKeyDelegate<Key, T> join)
            : base(isIndexSatisfied, isSatisfied)
        {
            this.joinDelegate = join;
        }

        /// <summary>
        /// Constructor with select only.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectJoinDelegateExpression(Action<List<T>> sort, JoinKeyDelegate<Key, T> join)
            : base(sort)
        {
            this.joinDelegate = join;
        }

        /// <summary>
        /// Constructor with satisfication and selection.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectJoinDelegateExpression(Predicate<T> isSatisfied,
                                 Action<List<T>> sort, JoinKeyDelegate<Key, T> join)
            : base(isSatisfied, sort)
        {
            this.joinDelegate = join;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectJoinDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                            Action<List<T>> sort, JoinKeyDelegate<Key, T> join)
            : base(isIndexSatisfied, sort)
        {
            this.joinDelegate = join;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public AutoSelectJoinDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  Predicate<T> isSatisfied, Action<List<T>> sort, JoinKeyDelegate<Key, T> join)
            : base(isIndexSatisfied, isSatisfied, sort)
        {
            this.joinDelegate = join;
        }

        #endregion
    }
}
