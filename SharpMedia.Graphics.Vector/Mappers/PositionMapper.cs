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

namespace SharpMedia.Graphics.Vector.Mappers
{
    /// <summary>
    /// A position mapper class. 
    /// </summary>
    /// <remarks>
    /// It calculated bouding region using positions and than
    /// creates the mapping coordinates so the lower bottom corner of rect gets mapping 
    /// coordinate (0,0) and the upper right coordinate gets mapping coordinate (1,1).
    /// </remarks>
    public sealed class PositionMapper : IMapper
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PositionMapper()
        {
        }

        #endregion

        #region IMapper Members

        public SharpMedia.Math.Vector2f[] Generate(object shape, Vector2f[] positions)
        {
            // Maps positions. First find min and max (bounding box)
            Vector2f max = new Vector2f(float.NegativeInfinity, float.NegativeInfinity);
            Vector2f min = new Vector2f(float.PositiveInfinity, float.PositiveInfinity);

            for (int i = 0; i < positions.Length; i++)
            {
                Vector2f v = positions[i];

                max.X = max.X > v.X ? max.X : v.X;
                max.Y = max.Y > v.Y ? max.Y : v.Y;
                min.X = min.X < v.X ? min.X : v.X;
                min.Y = min.Y < v.Y ? min.Y : v.Y;
            }

            // We now remap uniformy based on the box to range [0,1]x[0,1].
            Vector2f diff = max - min;
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = Vector2f.ComponentDivision(positions[i] - min, diff);
            }

            return positions;
        }

        #endregion
    }
}
