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

namespace SharpMedia.Physics.Joints
{
    /// <summary>
    /// A spring is class that describes spring.
    /// </summary>
    public sealed class Spring : Joint
    {
        #region Private Members
        double damper = 0.0;
        double spring = 0.0;
        double minDistance = 0.0f;
        double maxDistance = float.PositiveInfinity;
        #endregion

        #region Constructors

        public Spring(double spring, double damper)
        {
            this.spring = spring;
            this.damper = damper;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Damper effect.
        /// </summary>
        public double DamperConstant
        {
            get
            {
                return damper;
            }
            set
            {
                spring = value;
                // TODO:
            }
        }

        /// <summary>
        /// Spring coeficient.
        /// </summary>
        public double SpringConstant
        {
            get
            {
                return spring;
            }
            set
            {
                spring = value;
                // TODO:
            }
        }

        public double MinDistance
        {
            get { return minDistance; }
            set { minDistance = value; }
        }

        public double MaxDistance
        {
            get { return maxDistance; }
            set { maxDistance = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets only force without damping.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetForce(double x)
        {
            return - spring * x;
        }

        /// <summary>
        /// Gets the damping od spring.
        /// </summary>
        /// <param name="relativeVelocity"></param>
        /// <returns></returns>
        public double GetDamping(double relativeVelocity)
        {
            return -relativeVelocity * damper;
        }

        /// <summary>
        /// Returns the force applied if x displacement from rest position.
        /// The force is positive, is x is negative and vice versa. Damping is
        /// applied through delta velocity.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetForce(double x, double relativeVelocity)
        {
            return GetForce(x) + GetDamping(relativeVelocity);
        }

        #endregion
    }
}
