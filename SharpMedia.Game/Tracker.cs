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

namespace SharpMedia.Game
{
    /// <summary>
    /// A tracker is any object that can track other objects. Tracking can be direct
    /// or AI calculated (for example, missiles usually track directly if they are
    /// not smart, and players track using AI pathfinding).
    /// </summary>
    public interface ITracker
    {
        /// <summary>
        /// Are we currently tracking an object.
        /// </summary>
        bool IsTracking
        {
            get;
        }

        /// <summary>
        /// The goal where to come.
        /// </summary>
        IGameObject TrackingGoal
        {
            get;
        }

        /// <summary>
        /// The real distance to goal (for pathfinding, this is where we must go to get there).
        /// </summary>
        double TrackingDistanceToGoal
        {
            get;
        }

    }
}
