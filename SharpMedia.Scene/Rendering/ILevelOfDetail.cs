using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Scene.Rendering
{
    /// <summary>
    /// A level of detail component.
    /// </summary>
    [SceneComponent(QueryType=typeof(ILevelOfDetail))]
    public interface ILevelOfDetail
    {
        /// <summary>
        /// Obtains a level of detail.
        /// </summary>
        /// <param name="camera">The camera's scene node.</param>
        /// <param name="node">The scene node.</param>
        /// <returns></returns>
        uint GetLevelOfDetail(SceneNode camera, SceneNode node);

    }

    /// <summary>
    /// A distance level of detail object.
    /// </summary>
    public class DistanceLevelOfDetail : ILevelOfDetail
    {
        #region Private Members
        double[] distances = null;
        #endregion

        #region Constructors

        public DistanceLevelOfDetail(double[] distances)
        {
            this.distances = distances;
        }

        #endregion


        #region ILevelOfDetail Members

        public uint GetLevelOfDetail(SceneNode camera, SceneNode node)
        {
            double distance = (camera.Position - node.Position).Length;

            // Do bisection.
            int idx = Array.BinarySearch<double>(distances, distances);

            throw new Exception();
            
        }

        #endregion
    }

    /// <summary>
    /// No level of detail.
    /// </summary>
    public class NoLevelOfDetail : ILevelOfDetail
    {

        #region ILevelOfDetail Members

        public uint GetLevelOfDetail(SceneNode camera, SceneNode node)
        {
            return 0;
        }

        #endregion
    }
}
