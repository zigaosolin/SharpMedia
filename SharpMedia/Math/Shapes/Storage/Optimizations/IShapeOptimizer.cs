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

namespace SharpMedia.Math.Shapes.Storage.Optimizations
{

    /// <summary>
    /// An optimization tool that "optimizes" certain aspect of shape.
    /// </summary>
    public interface IShapeOptimizer
    {
        /// <summary>
        /// Runs an optimizer on set.
        /// </summary>
        /// <remarks>The set must either be mapped with correct attributes (usually read-write) or
        /// not mapped at all (it will be mapped and unmapped by optimizer).</remarks>
        /// <param name="set"></param>
        void Run(ITopologySet set);
    }

    /// <summary>
    /// A shape optmimization helper.
    /// </summary>
    internal static class ShapeOptimizerHelper
    {

        public static bool MapBuffers(ITopologySet set, MapOptions op, string[] components, bool indexBuffer)
        {
            // TODO:
            return false;
        }

        public static void UnMapBuffers(ITopologySet set, bool mapped)
        {
            if (!mapped) return;

            // Unmap all buffers.
        }

    }

    
}
