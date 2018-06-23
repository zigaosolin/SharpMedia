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
using SharpMedia.AspectOriented;

namespace SharpMedia.Physics.Joints
{
    /// <summary>
    /// A joint class.
    /// </summary>
    public abstract class Joint : IPhysicsBase
    {
        #region Private Members
        bool collisionBetweenJointEnabled = false;
        double maxForce = double.PositiveInfinity;
        double maxTorque = double.PositiveInfinity;
        PhysicsScene scene;
        RigidBody rigidBody1;
        RigidBody rigidBody2;
        #endregion

        #region Protected Members

        protected Joint(PhysicsScene scene, RigidBody rigidBody1, RigidBody rigidBody2)
        {
            this.scene = scene;
            this.rigidBody1 = rigidBody1;
            this.rigidBody2 = rigidBody2;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Enables collision between bodies linked to joined.
        /// </summary>
        public bool EnableCollisionBetweenBodies
        {
            get
            {
                return collisionBetweenJointEnabled;
            }
            set
            {
                collisionBetweenJointEnabled = value;
                // TODO:
            }
        }

        /// <summary>
        /// Maximum force this joint can apply, otherwise it breaks.
        /// </summary>
        /// <remarks>Set MaxTorque and MaxForce to double.PositiveInfinite for non-breakable.</remarks>
        public double MaxForce
        {
            get
            {
                return maxForce;
            }
            [param: MinDouble(0.0)]
            set
            {
                maxForce = value;
                // TODO:
            }
        }

        /// <summary>
        /// Maximum torque this joint can apply, otherwise it breaks.
        /// </summary>
        /// <remarks>Set MaxTorque and MaxForce to double.PositiveInfinite for non-breakable.</remarks>
        public double MaxTorque
        {
            get
            {
                return maxTorque;
            }
            [param: MinDouble(0.0)]
            set
            {
                maxTorque = value;
                // TODO:
            }
        }

        /// <summary>
        /// Is the joint broken. Can also manually break the joint.
        /// </summary>
        public bool IsBroken
        {
            get
            {
                return IsBroken;
            }
            set
            {

            }
        }

        /// <summary>
        /// First rigid body.
        /// </summary>
        public RigidBody RigidBody1
        {
            get
            {
                return rigidBody1;
            }
        }

        /// <summary>
        /// Second rigid body, can be null.
        /// </summary>
        public RigidBody RigidBody2
        {
            get
            {
                return rigidBody2;
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
            throw new Exception("The method or operation is not implemented.");
        }

        public void Sync()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
