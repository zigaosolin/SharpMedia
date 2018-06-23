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
using SharpMedia.Math.Shapes;
using SharpMedia.Math;

namespace SharpMedia.Physics.Driver
{

    /// <summary>
    /// A physical rigid body, that participates in simulation.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IRigidBody : IDisposable
    {
        /// <summary>
        /// A shape of object for collision. Can be compounds shape.
        /// </summary>
        /// <remarks>
        /// Setting any shape is not supported. Check physics engine's capabilities for support.
        /// </remarks>
        IShaped Shape { get; }

        /// <summary>
        /// Synhronizes.
        /// </summary>
        /// <returns></returns>
        bool Sync(ref Vector3d position, ref Vector3d velocity, ref Quaterniond orientation,
                  ref Vector3d angVelocity, bool force);


        /// <summary>
        /// Is the object resting.
        /// </summary>
        bool IsResting { get; set; }

        /// <summary>
        /// Adds force and torque to physical object.
        /// </summary>
        /// <param name="force">The force applied.</param>
        /// <param name="globalPosition">Global position where force is applied. If it is the center
        /// of object, torque is zero.</param>
        void AddForce(Vector3d force, Vector3d globalPosition);

        /// <summary>
        /// Adds force at center.
        /// </summary>
        void AddForce(Vector3d force);

        /// <summary>
        /// Adds torque to object.
        /// </summary>
        /// <param name="torque"></param>
        void AddTorque(Vector3d torque);
    }
}
