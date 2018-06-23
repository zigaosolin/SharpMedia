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
    /// Selection delegate.
    /// </summary>
    public delegate Selectable SelectDelegate<Selectable, T>(T obj);

    /// <summary>
    /// A key of object.
    /// </summary>
    public delegate Key JoinKeyDelegate<Key, T>(T obj);

    /// <summary>
    /// A delegate based expression
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateExpression<Selectable, T> : IQueryExpression<Selectable, T>
    {
        #region Private Members
        Predicate<T> isSatisfied;
        Predicate2<uint, Dictionary<string, object>> isIndexSatisfied;
        SelectDelegate<Selectable, T> select;
        Action<List<Selectable>> sort;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with select only.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public DelegateExpression([NotNull] SelectDelegate<Selectable, T> select)
        {
            this.select = select;
        }

        /// <summary>
        /// Constructor with satisfication and selection.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public DelegateExpression(Predicate<T> isSatisfied,
                                  [NotNull] SelectDelegate<Selectable, T> select)
        {
            this.isSatisfied = isSatisfied;
            this.select = select;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public DelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  [NotNull] SelectDelegate<Selectable, T> select)
        {
            this.isIndexSatisfied = isIndexSatisfied;
            this.select = select;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public DelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  Predicate<T> isSatisfied,
                                  [NotNull] SelectDelegate<Selectable, T> select)
        {
            this.isIndexSatisfied = isIndexSatisfied;
            this.select = select;
            this.isSatisfied = isSatisfied;
        }

        /// <summary>
        /// Constructor with select only.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public DelegateExpression([NotNull] SelectDelegate<Selectable, T> select, Action<List<Selectable>> sort)
        {
            this.select = select;
            this.sort = sort;
        }

        /// <summary>
        /// Constructor with satisfication and selection.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public DelegateExpression(Predicate<T> isSatisfied,
                                  [NotNull] SelectDelegate<Selectable, T> select,
                                 Action<List<Selectable>> sort)
        {
            this.isSatisfied = isSatisfied;
            this.select = select;
            this.sort = sort;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public DelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  [NotNull] SelectDelegate<Selectable, T> select, Action<List<Selectable>> sort)
        {
            this.isIndexSatisfied = isIndexSatisfied;
            this.select = select;
            this.sort = sort;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public DelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  Predicate<T> isSatisfied, [NotNull] SelectDelegate<Selectable, T> select, 
                                  Action<List<Selectable>> sort)
        {
            this.isIndexSatisfied = isIndexSatisfied;
            this.select = select;
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

        public bool IsSatisfied(T obj)
        {
            Predicate<T> t = isSatisfied;
            if (t != null)
            {
                return t(obj);
            }

            return true;
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

        public Selectable Select(T obj)
        {
            return select(obj);
        }

        public void Sort(List<Selectable> results)
        {
            Action<List<Selectable>> t = sort;
            if (t != null)
            {
                t(results);
            }
        }

        #endregion

    }

    /// <summary>
    /// A join delegate expression.
    /// </summary>
    public class JoinDelegateExpression<Selectable, T, Key> : DelegateExpression<Selectable, T>,
        IJoinExpression<Selectable, T, Key> where Key : IComparable<Key>
    {
        #region Private Members
        JoinKeyDelegate<Key, T> joinKey;
        #endregion

        #region IJoinExpression<Selectable,T,Key> Members

        public override QueryFilter Filter
        {
            get
            {
                return joinKey != null ? base.Filter | QueryFilter.JoinWithKey : base.Filter;
            }
        }

        public Key JoinKey(T obj)
        {
            JoinKeyDelegate<Key, T> t = joinKey;
            if (t != null)
            {
                return t(obj);
            }
            return default(Key);
        }

        #endregion

        #region Constructors

                /// <summary>
        /// Constructor with select only.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public JoinDelegateExpression([NotNull] SelectDelegate<Selectable, T> select, 
            JoinKeyDelegate<Key, T> join)
            : base(select)
        {
            this.joinKey = join;
        }

        /// <summary>
        /// Constructor with satisfication and selection.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public JoinDelegateExpression(Predicate<T> isSatisfied,
                                  [NotNull] SelectDelegate<Selectable, T> select,
                                 JoinKeyDelegate<Key, T> join)
            : base(isSatisfied, select)
        {
            this.joinKey = join;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public JoinDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  [NotNull] SelectDelegate<Selectable, T> select, JoinKeyDelegate<Key, T> join)
            : base(isIndexSatisfied, select)
        {
            this.joinKey = join;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public JoinDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  Predicate<T> isSatisfied,
                                  [NotNull] SelectDelegate<Selectable, T> select,
                                  JoinKeyDelegate<Key, T> join)
            : base(isIndexSatisfied, isSatisfied, select)
        {
            this.joinKey = join;
        }

        /// <summary>
        /// Constructor with select only.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public JoinDelegateExpression([NotNull] SelectDelegate<Selectable, T> select,
                    Action<List<Selectable>> sort, JoinKeyDelegate<Key, T> join)
            : base(select, sort)
        {
            this.joinKey = join;
        }

        /// <summary>
        /// Constructor with satisfication and selection.
        /// </summary>
        /// <param name="isSatisfied"></param>
        /// <param name="select"></param>
        public JoinDelegateExpression(Predicate<T> isSatisfied,
                                  [NotNull] SelectDelegate<Selectable, T> select,
                                 Action<List<Selectable>> sort,
                                   JoinKeyDelegate<Key, T> join)
            : base(isSatisfied, select, sort)
        {
            this.joinKey = join;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public JoinDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  [NotNull] SelectDelegate<Selectable, T> select, Action<List<Selectable>> sort,
                                   JoinKeyDelegate<Key, T> join)
            : base(isIndexSatisfied, select, sort)
        {
            this.joinKey = join;
        }

        /// <summary>
        /// Constructor with index selection.
        /// </summary>
        /// <param name="isIndexSatisfied"></param>
        /// <param name="select"></param>
        public JoinDelegateExpression(Predicate2<uint, Dictionary<string, object>> isIndexSatisfied,
                                  Predicate<T> isSatisfied, [NotNull] SelectDelegate<Selectable, T> select,
                                  Action<List<Selectable>> sort, JoinKeyDelegate<Key, T> join)
            : base(isIndexSatisfied, isSatisfied, select, sort)
        {
            this.joinKey = join;
        }

        #endregion
    }

}
