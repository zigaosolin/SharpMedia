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
using SharpMedia.Math;

namespace SharpMedia.Graphics.GUI.Widgets.Graphs
{
    /// <summary>
    /// A graph data provider.
    /// </summary>
    public interface ISampleProvider
    {
        /// <summary>
        /// Range in X coordinate.
        /// </summary>
        Intervald RangeX { get; }

        /// <summary>
        /// Range in Y coordinate.
        /// </summary>
        Intervald RangeY { get; }

        /// <summary>
        /// Provides data in array form.
        /// </summary>
        Vector2d[] Provide2d(Intervald xInterval, double optimalDelta);

        
    }

    /// <summary>
    /// A sample provider for 3d graphs.
    /// </summary>
    public interface ISampleProvider3d : ISampleProvider
    {

        /// <summary>
        /// Range in Z coordinate.
        /// </summary>
        Intervald RangeZ { get; }

        /// <summary>
        /// Provides data in array form.
        /// </summary>
        Vector3d[] Provide3d(Intervald xInterval, double optimalDelta);
    }
}
