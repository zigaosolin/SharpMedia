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

namespace SharpMedia.Physics
{

    /// <summary>
    /// A combine mode, determines how 
    /// </summary>
    public enum CombineMode
    {
        /// <summary>
        /// Uses the maximum value.
        /// </summary>
        Max,

        /// <summary>
        /// Uses the minimum value.
        /// </summary>
        Min,

        /// <summary>
        /// Uses the average.
        /// </summary>
        Average,

        /// <summary>
        /// Combines by adding data.
        /// </summary>
        Add
    }

    /// <summary>
    /// A collision mask. Note that mask is not obligatory, only components that are selected must
    /// be checked for collision.
    /// </summary>
    /// <remarks>
    /// Collision groups and masks are still applied.
    /// </remarks>
    [Flags]
    public enum CollisionMask
    {
        /// <summary>
        /// *-Rigid body collisions taken into account.
        /// </summary>
        RigidBody = 1,

        /// <summary>
        /// *-Deformable body collision taken into account.
        /// </summary>
        DeformableBody = 2,

        /// <summary>
        /// *-Fluid collision taken into account.
        /// </summary>
        Fluid = 4,

        /// <summary>
        /// *-Cloth collision taken into account.
        /// </summary>
        Cloth = 8,

        /// <summary>
        /// All collision taken into account.
        /// </summary>
        All = RigidBody | DeformableBody | Fluid | Cloth
    }
}
