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

namespace SharpMedia.Physics.Effectors
{
    /// <summary>
    /// A constant force, such as gravity. It can apply to all objects in scene, to masked ones
    /// or to specifically selected ones.
    /// </summary>
    public sealed class ConstantForce : IEffector
    {
        #region Private Members
        bool isEnabled = true;
        Vector3d force = Vector3d.Zero;
        #endregion

        #region Constants

        /// <summary>
        /// Applies constant force to all objects.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="objects"></param>
        public ConstantForce(Vector3d force)
        {
            this.force = force;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the force.
        /// </summary>
        public Vector3d Force
        {
            get
            {
                return force;
            }
            set
            {
                force = value;
            }
        }

        #endregion

        #region IEffector Members

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = true;
            }
        }

        public void ApplyTo(IPhysicsObject obj)
        {
            obj.AddForce(force);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

    }
}
