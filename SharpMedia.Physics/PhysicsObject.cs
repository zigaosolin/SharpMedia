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

namespace SharpMedia.Physics
{
    /// <summary>
    /// A physics base object.
    /// </summary>
    public interface IPhysicsBase : IDisposable
    {
        /// <summary>
        /// The physics scene this object belongs to.
        /// </summary>
        PhysicsScene PhysicsScene { get; }

        /// <summary>
        /// Tries to synhronize representation of driver part and what is held in object.
        /// </summary>
        /// <remarks>Must be called otherwise previous frame's data is returned.</remarks>
        /// <returns>In case of successful sync, true is returned, otherwise false is returned.</returns>
        bool TrySync();

        /// <summary>
        /// Synhronizes driver and object's representation.
        /// </summary>
        void Sync();

        
    }

    /// <summary>
    /// A physics object interface (dynamic physics entity).
    /// </summary>
    /// <remarks>
    /// Because physics objects are highly 
    /// </remarks>
    public interface IPhysicsObject : IPhysicsBase
    {

        /// <summary>
        /// Adds force to object.
        /// </summary>
        /// <param name="force"></param>
        /// <remarks>Synhronization is not necessary for force method.</remarks>
        void AddForce(Vector3d force);

        /// <summary>
        /// Adds force at position.
        /// </summary>
        /// <param name="force"></param>
        /// <param name="position"></param>
        /// <remarks>Synhronization is not necessary for force method.</remarks>
        void AddForceAtPosition(Vector3d force, Vector3d position);

        /// <summary>
        /// Collision mask of object. It is or-ed with other body to check if collision should occur.
        /// </summary>
        ulong CollisionMask { set; get; }

        /// <summary>
        /// Is the object sleeping.
        /// </summary>
        bool IsSleeping { get; set; }
    }
}
