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
using SharpMedia.Graphics;

namespace SharpMedia.Scene.GI
{

    /// <summary>
    /// A collection of global illumination algorithms, with different options.
    /// </summary>
    public static class GlobalIllumination
    {
        /// <summary>
        /// Precission of each element at startup.
        /// </summary>
        public enum ElementPrecission
        {
            /// <summary>
            /// Per-texel global illumination evaluation, with multiple samples for each
            /// texel (area is not differential). Each texel of bound texture
            /// is an "element" and affects all other elements.
            /// </summary>
            /// <remarks>
            /// Use this only if really good quality is needed.
            /// </remarks>
            PerTexelSampled,

            /// <summary>
            /// Per-texel global illumination evaluation, with one Sample for each texel
            /// (area is assumed to be differential). Each texel is an element.
            /// </summary>
            PerTexel,

            /// <summary>
            /// Evalution for each vertex. Each vertex is an element.
            /// </summary>
            PerVertex,

            /// <summary>
            /// Evaluation for each shape. Most algorithms first convert geometry to triangles
            /// only mesh.
            /// </summary>
            PerShape
        }

        /// <summary>
        /// Adaptive tesselation technique, if used/available.
        /// </summary>
        public enum AdaptiveTesselation
        {
            /// <summary>
            /// No further adaptive tesselation.
            /// </summary>
            None,

            /// <summary>
            /// Uniform tesselation to get more quality, tesselation will not effect geometry.
            /// </summary>
            Uniform,

            /// <summary>
            /// Hierarchical tesselation, also uniform but at the same time uses hierarchical system
            /// to calculate less interactions.
            /// </summary>
            HierarchicalUniform
        }



    }
}
