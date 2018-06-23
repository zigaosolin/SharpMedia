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

namespace SharpMedia.Caching
{

    /// <summary>
    /// A least recently used evaluator, thread safe.
    /// </summary>
    public class LRU<T> : IEvaluator<T>
        where T : IComparable<T>
    {
        #region Private Members
        float touchScoreRaise;
        float scoreFallPerSecond;
        #endregion

        #region Constructors

        /// <summary>
        /// A LRU evaluator constructor.
        /// </summary>
        /// <param name="touchRaise">The touch raise score.</param>
        /// <param name="scoreFallPerSec">The </param>
        public LRU(float touchRaise, float scoreFallPerSec)
        {
            if (touchRaise < 0.0f) throw new ArgumentOutOfRangeException("There must a raise on touch.");
            if (scoreFallPerSec < 0.0f) throw new ArgumentOutOfRangeException("There must be a decrease of score in time.");
            
            touchScoreRaise = touchRaise;
            scoreFallPerSecond = scoreFallPerSec;
        }

        #endregion

        #region IEvaluator<T> Members

        public object Data
        {
            get { return null; }
        }

        public float Touch(object data, float score)
        {
            return score + touchScoreRaise;
        }

        public float Update(object data, TimeSpan span, float score)
        {
            return score - (float)span.Ticks * 10e-7f * scoreFallPerSecond;
        }

        #endregion
    }
}
