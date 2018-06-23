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

namespace SharpMedia.Caching
{

    /// <summary>
    /// An eveluator interface.
    /// </summary>
    /// <typeparam name="T">The typename, same as for ICache.</typeparam>
    public interface IEvaluator<T> 
        where T : IComparable<T>
    {

        /// <summary>
        /// Creates a new data for new object. Must be lightweight.
        /// </summary>
        object Data
        {
            get;
        }

        /// <summary>
        /// Touches the cacheable, resulting in retuned score.
        /// </summary>
        /// <param name="data">The per-cacheable data.</param>
        /// <param name="span">Time span since last update or cache's creation.</param>
        /// <param name="score">The cacheable's score.</param>
        /// <returns>Returns new score.</returns>
        float Touch([NotNull] object data, float score);

        /// <summary>
        /// Updates the object.
        /// </summary>
        /// <param name="data">The per cacheable data.</param>
        /// <param name="span">Time span from last update (not touch).</param>
        /// <param name="score">Currect score.</param>
        /// <returns>New score.</returns>
        float Update([NotNull] object data, TimeSpan span, float score);

    }
}
