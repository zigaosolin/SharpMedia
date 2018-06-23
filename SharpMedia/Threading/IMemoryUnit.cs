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

namespace SharpMedia.Threading
{

    /// <summary>
    /// A memory unit, usually RAM.
    /// </summary>
    public interface IMemoryUnit
    {
        /// <summary>
        /// Bandwidth performance of memory unit.
        /// </summary>
        Performance.BandwidthPerformance BandwidthPerformance { get; }

        /// <summary>
        /// Free bytes of memory unit.
        /// </summary>
        ulong FreeBytes { get; }

        /// <summary>
        /// Available bytes of memory unit, without virtualized memory.
        /// </summary>
        ulong AvailableBytes { get; }

        /// <summary>
        /// Virtual memory in bytes.
        /// </summary>
        ulong VirtualBytes { get; }

        /// <summary>
        /// Consumed bytes of memory unit.
        /// </summary>
        ulong ConsumedBytes { get; }

    }
}
