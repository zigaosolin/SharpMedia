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
    /// A deformable body vertex,
    /// </summary>
    public struct DeformableBodyVertex
    {
        /// <summary>
        /// Is the vertex terable.
        /// </summary>
        public bool Tearable;

        /// <summary>
        /// The mass of vertex.
        /// </summary>
        public double Mass;

        /// <summary>
        /// Position of vertex.
        /// </summary>
        public Vector3d Position;
    }

    /// <summary>
    /// A deformable object is composed of tetrahedra.
    /// </summary>
    public sealed class DeformableBody : IPhysicsObject
    {
        #region Private Members
        bool tearable = false;
        #endregion

        #region Properties

        /// <summary>
        /// Is deformable mesh tearable
        /// </summary>
        public bool IsTearable
        {
            get
            {
                return tearable;
            }
            set
            {
                tearable = value;
            }
        }

        /// <summary>
        /// Material of deformable body. Some parts may not be used by driver.
        /// </summary>
        public Material Material
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double Damping
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double VolumeStiffness
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public Vector3d ExternalAcceleration
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public double ParticleRadius
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public double StretchingStiffness
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public double Density
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

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

        #region IPhysicsObject Members

        public void AddForce(Vector3d force)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddForceAtPosition(Vector3d force, Vector3d position)
        {
            throw new Exception("The method or operation is not implemented.");
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
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }
}
