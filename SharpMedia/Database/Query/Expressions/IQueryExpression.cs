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

namespace SharpMedia.Database.Query.Expressions
{
    /// <summary>
    /// Query filter.
    /// </summary>
    [Flags]
    public enum QueryFilter
    {
        None = 0,
        Index = 1,
        Object = 2,
        Sort = 4,
        JoinWithKey = 8
    }

    /// <summary>
    /// An query expression.
    /// </summary>
    public interface IQueryExpression<Selectable, T>
    {
        /// <summary>
        /// A query expression mask - filter.
        /// </summary>
        QueryFilter Filter { get; }

        /// <summary>
        /// An object query based on indexed properties (no object yet available).
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="indexable"></param>
        /// <returns></returns>
        bool IsSatisfied(uint index, Dictionary<string, object> indexable);

        /// <summary>
        /// An object query expression.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IsSatisfied(T obj);

        /// <summary>
        /// Selects an object.
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        Selectable Select(T obj);

        /// <summary>
        /// Sorts results based on query expression.
        /// </summary>
        /// <param name="results"></param>
        void Sort(List<Selectable> results);

    }
}
