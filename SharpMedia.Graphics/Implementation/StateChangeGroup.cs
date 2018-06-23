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

namespace SharpMedia.Graphics.Implementation
{
    internal struct StateChangeGroup
    {
        /// <summary>
        /// Minimum changes per frame.
        /// </summary>
        public uint MinPerFrame;

        /// <summary>
        /// Maximum changes per frame.
        /// </summary>
        public uint MaxPerFrame;

        /// <summary>
        /// All changes from now on.
        /// </summary>
        public ulong All;

        /// <summary>
        /// Current number of changes in this frame.
        /// </summary>
        public uint Current;

        /// <summary>
        /// Average.
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        public uint Average(ulong frames)
        {
            return (uint)(All / frames);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            All += Current;
            if (Current > MaxPerFrame) MaxPerFrame = Current;
            if (Current < MinPerFrame) MinPerFrame = Current;
            Current = 0;
        }

        /// <summary>
        /// Adds a chnaged attribute.
        /// </summary>
        public void Changed()
        {
            Current++;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            MinPerFrame = uint.MaxValue;
            MaxPerFrame = uint.MinValue;
            All = 0;
            Current = 0;

        }

    }
}
