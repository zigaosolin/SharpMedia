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
    /// A query expression with join.
    /// </summary>
    /// <typeparam name="Selectable"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public interface IJoinExpression<Selectable, T, Key> : IQueryExpression<Selectable, T> 
        where Key : IComparable<Key>
    {

        /// <summary>
        /// A join key of object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Key JoinKey(T obj);

    }
}
