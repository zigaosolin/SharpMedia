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
    /// A base class for major part of topology sets.
    /// </summary>
    public abstract class BaseTopologySet : ITopologySet
    {
        #region Protected Members
        protected uint shapeCount;
        protected ControlPointQuery query;
        protected ShapeIndexBufferView indexBuffer;
        #endregion

        #region ITopologySet Members

        public uint ShapeCount
        {
            get { return shapeCount; }
        }

        public abstract uint ControlPointsPerShape
        {
            get;
        }

        public ControlPointQuery Query
        {
            get { return query; }
        }

        public ShapeIndexBufferView IndexBuffer
        {
            get { return indexBuffer; }
        }

        public void Map(MapOptions op, params string[] components)
        {
            if (indexBuffer != null) indexBuffer.Map(op);
            query.Map(op, components);
        }

        public void Map(MapOptions op)
        {
            if (indexBuffer != null) indexBuffer.Map(op);
            query.Map(op);
        }

        public void UnMap()
        {
            if (indexBuffer != null) indexBuffer.UnMap();
            query.UnMap();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Is the topology set indexed.
        /// </summary>
        public bool IsIndexed
        {
            get { return indexBuffer != null; }
        }

        #endregion
    }
}
