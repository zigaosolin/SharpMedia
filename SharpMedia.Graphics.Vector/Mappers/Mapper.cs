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
using SharpMedia.Math.Matrix;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Vector.Mappers
{

    /// <summary>
    /// A mapper interface can generate mapping coordiantes for shape.
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Generates mapping coordinates given the positions.
        /// </summary>
        /// <param name="shape">The shape reference.</param>
        /// <param name="positions">Positions, they are read-write (can return them as result).</param>
        /// <remarks>Number of mapping coordiantes must match number of positions.</remarks>
        Vector2f[] Generate(object shape, Vector2f[] positions);

    }
}
