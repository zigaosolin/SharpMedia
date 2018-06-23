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
using SharpMedia.AspectOriented;
using SharpMedia.Math.Matrix;
using SharpMedia.Math.Shapes;
using SharpMedia.Physics.Joints;

namespace SharpMedia.Physics
{

    /// <summary>
    /// A rigid body. 
    /// </summary>
    public sealed class RigidBody : IPhysicsObject
    {
        #region Private Members
        PhysicsScene scene;
        Material material;
        Mass mass;

        // Synced data.
        Vector3d position;
        Vector3d velocity;
        Vector3d angularVelocity;
        Quaterniond orientation;


        Driver.IRigidBody rigidBody;
        #endregion

        #region Constructors


        public RigidBody([NotNull] PhysicsScene scene, [NotNull] Mass mass, [NotNull] Material material,
                        Vector3d position, Quaterniond orientation, params object[] shapes)
        {
            this.scene = scene;

            rigidBody = null; // ...
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets physics material of rigid body.
        /// </summary>
        public Material Material
        {
            get
            {
                return material.Clone();
            }
            [param: NotNull] 
            set
            {
                material = value;
                // TODO:
            }
        }

        /// <summary>
        /// Sets or gets the mass and its distribution of rigid body.
        /// </summary>
        public Mass Mass
        {
            get
            {
                return mass;
            }
            [param: NotNull] 
            set
            {
                mass = value;
                // TODO:
            }
        }

        /// <summary>
        /// The position of rigid body.
        /// </summary>
        public Vector3d Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// The orientation of rigid body.
        /// </summary>
        public Quaterniond Orientation
        {
            get
            {
                return orientation;
            }
        }

        /// <summary>
        /// Current velocity of rigid body.
        /// </summary>
        public Vector3d Velocity
        {
            get
            {
                return velocity;
            }
        }

        /// <summary>
        /// Angular velocity of rigid body.
        /// </summary>
        public Vector3d AngularVelocity
        {
            get
            {
                return angularVelocity;
            }
        }

        /// <summary>
        /// Attached joints to rigid body.
        /// </summary>
        public Joint[] AttachedJoints
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// A pose matrix.
        /// </summary>
        public Matrix4x4d Pose
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Sets or gets if object is kinematic.
        /// </summary>
        /// <remarks>Forces do not effect kinematic objects but 
        /// object applies forces to enviorment.</remarks>
        public bool IsKinematic
        {
            get { return false; }
            set { }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Transforms global position to local frame.
        /// </summary>
        /// <param name="globalPos">If last coordiante is 0.0, it is treated as vector, otherwise
        /// as position.</param>
        /// <returns></returns>
        public Vector3d TransformToLocal(Vector4d globalPos)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Transforms local position to global frame.
        /// </summary>
        /// <param name="localPos">Local position, if last coordinate is 0, it is treated as
        /// vector, otherwise as position.</param>
        /// <returns></returns>
        public Vector3d TransformToGlobal(Vector4d localPos)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets rigid body information.
        /// </summary>
        public void GetInfo(out Vector3d position, out Quaterniond orientation,
            out Vector3d velocity, out Vector3d angularVelocity)
        {
            position = this.position;
            velocity = this.velocity;
            angularVelocity = this.angularVelocity;
            orientation = this.orientation;
        }

        /// <summary>
        /// Gets rigid body information.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="orientation"></param>
        public void GetInfo(out Vector3d position, out Quaterniond orientation)
        {
            position = this.position;
            orientation = this.orientation;
        }

        /// <summary>
        /// Performs a kinematic move.
        /// </summary>
        /// <remarks>The move must be small in order to make physics to function correctly.</remarks>
        /// <param name="deltaOrientation">The delta orientation.</param>
        /// <param name="deltaPosition">The delta (move) position.</param>
        public void KinematicMove(Vector3d deltaPosition, Quaterniond deltaOrientation)
        {

        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            rigidBody.Dispose();
        }

        #endregion

        #region IPhysicsObject Members

        public void AddForce(Vector3d force)
        {
            rigidBody.AddForce(force);
        }

        public void AddForceAtPosition(Vector3d force, Vector3d position)
        {
            rigidBody.AddForce(force, position);
        }

        public ulong CollisionMask
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool IsSleeping
        {
            get
            {
                return rigidBody.IsResting;
            }
            set
            {
                rigidBody.IsResting = value;
            }
        }

        #endregion

        #region IPhysicsBase Members

        public PhysicsScene PhysicsScene
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool TrySync()
        {
            return rigidBody.Sync(ref position, ref velocity, ref orientation, ref angularVelocity, false);
        }

        public void Sync()
        {
            if (!rigidBody.Sync(ref position, ref velocity, ref orientation, ref angularVelocity, true))
            {
                throw new InvalidOperationException("Synhronization failed.");
            }
        }

        #endregion
    }
}
