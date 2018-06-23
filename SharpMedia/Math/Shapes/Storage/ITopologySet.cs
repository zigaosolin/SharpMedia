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

namespace SharpMedia.Math.Shapes.Storage
{

    /// <summary>
    /// This is a base class for topology set.
    /// </summary>
    public interface ITopologySet
    {
        /// <summary>
        /// Number of shapes.
        /// </summary>
        uint ShapeCount { get; }

        /// <summary>
        /// Number of control points needed for shape; if it is variable, 0 is returned.
        /// </summary>
        uint ControlPointsPerShape { get; }

        /// <summary>
        /// A control point query accessor, to be given direct access to control points.
        /// </summary>
        ControlPointQuery Query { get; }

        /// <summary>
        /// The index buffer used, can be null.
        /// </summary>
        ShapeIndexBufferView IndexBuffer { get; }

        /// <summary>
        /// Maps all buffers to gain access to given components.
        /// </summary>
        /// <param name="components"></param>
        void Map(MapOptions op, params string[] components);

        /// <summary>
        /// Mapps all buffers.
        /// </summary>
        void Map(MapOptions op);

        /// <summary>
        /// Unmaps all buffers.
        /// </summary>
        void UnMap();

    }

 
}
