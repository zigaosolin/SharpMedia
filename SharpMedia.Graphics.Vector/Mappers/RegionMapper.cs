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
using SharpMedia.Graphics.States;
using SharpMedia.Math.Shapes.Storage;

namespace SharpMedia.Graphics.Vector.Mappers
{


    /// <summary>
    /// Creates mapping coordinates given a region. This can only be used for "precached" position mapping.
    /// </summary>
    public class RegionMapper : IMapper
    {
        #region Private Members
        Region2f region;
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RegionMapper()
            : this(Region2f.Null)
        {
        }

        /// <summary>
        /// Creates a region mapper.
        /// </summary>
        /// <param name="region"></param>
        public RegionMapper(Region2f region)
        {
            this.region = region;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The region positions are mapped to.
        /// </summary>
        public Region2f Region
        {
            get { return region; }
            set { region = value; }
        }

        #endregion

        #region IMapper Members

        public Vector2f[] Generate(object shape, Vector2f[] positions)
        {
            Vector2f dif = region.Dimensions;

            // We clamp at border.
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = Vector2f.ComponentDivision(positions[i] - region.LeftBottom, dif);

            }

            // We return "transformed" positions as result.
            return positions;
        }

        #endregion
    }
}
