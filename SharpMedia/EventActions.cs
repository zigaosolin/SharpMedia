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

namespace SharpMedia
{
    /// <summary>
    /// Hears no evil, returns no evil
    /// </summary>
    public delegate void ReturnsVoid();

    /// <summary>
    /// Action with 2 arguments.
    /// </summary>
    public delegate void Action2<T, T2>            (T a, T2 b);

    /// <summary>
    /// Action with 3 arguments.
    /// </summary>
    public delegate void Action3<T, T2, T3>        (T a, T2 b, T3 c);

    /// <summary>
    /// Action with 4 arguments.
    /// </summary>
    public delegate void Action4<T, T2, T3, T4>    (T a, T2 b, T3 c, T4 d);

    /// <summary>
    /// Action with 5 arguments.
    /// </summary>
    public delegate void Action5<T, T2, T3, T4, T5>(T a, T2 b, T3 c, T4 d, T5 e);

    /// <summary>
    /// Predicate (returns true or false) with 2 arguments.
    /// </summary>
    public delegate bool Predicate2<T, T2>              (T a, T2 b);

    /// <summary>
    /// Predicate (returns true or false) with 3 arguments.
    /// </summary>
    public delegate bool Predicate3<T, T2, T3>          (T a, T2 b, T3 c);

    /// <summary>
    /// Predicate (returns true or false) with 4 arguments.
    /// </summary>
    public delegate bool Predicate4<T, T2, T3, T4>      (T a, T2 b, T3 c, T4 d);

    /// <summary>
    /// Predicate (returns true or false) with 5 arguments.
    /// </summary>
    public delegate bool Predicate5<T, T2, T3, T4, T5>  (T a, T2 b, T3 c, T4 d, T5 e);
}
