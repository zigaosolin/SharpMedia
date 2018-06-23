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

namespace SharpMedia.Math.Random
{

    /// <summary>
    /// A .NET provided generator wrapper.
    /// </summary>
    public sealed class NativeGenerator : IResetableRandomGeneratord, IResetableRandomGeneratorf
    {
        #region Private Members
        global::System.Random random;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a generator with automatic (time based) seed.
        /// </summary>
        public NativeGenerator()
        {
            random = new System.Random();
        }

        /// <summary>
        /// Creates a generator with initial seed.
        /// </summary>
        /// <param name="seed"></param>
        public NativeGenerator(int seed)
        {
            random = new System.Random(seed);
        }

        #endregion

        #region IResetableRandomGeneratord Members

        public void Reset()
        {
            random = new System.Random();
        }

        public void Reset(int seed)
        {
            random = new System.Random(seed);
        }

        #endregion

        #region IRandomGeneratord Members

        public double NextRandom()
        {
            return random.NextDouble();
        }

        #endregion

        #region IRandomGeneratorf Members

        float IRandomGeneratorf.NextRandom()
        {
            return (float)random.NextDouble();
        }

        #endregion
    }
}
