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
using SharpMedia.Math.Matrix;
using SharpMedia.Math;
using SharpMedia.AspectOriented;
using SharpMedia.Testing;

namespace SharpMedia.Physics
{
    /// <summary>
    /// The mass of rigid body and it's distribution.
    /// </summary>
    /// <remarks>Object is immutable.</remarks>
    public sealed class Mass : IEquatable<Mass>
    {
        #region Private Members
        Vector3d centerOfMass;
        Matrix3x3d inertia;
        double mass;
        #endregion

        #region Operators

        /// <summary>
        /// Adds mases.
        /// </summary>
        public static Mass operator +([NotNull] Mass m1, [NotNull] Mass m2)
        {
            return m1.Add(m2);
        }

        /// <summary>
        /// Rotates mass.
        /// </summary>
        public static Mass operator *([NotNull] Mass m1, Quaterniond q)
        {
            return m1.Rotate(q);
        }

        /// <summary>
        /// Rotates mass.
        /// </summary>
        public static Mass operator *([NotNull] Mass m1, Matrix3x3d m)
        {
            return m1.Rotate(m);
        }

        /// <summary>
        /// Translates mass.
        /// </summary>
        public static Mass operator *([NotNull] Mass m1, Vector3d v)
        {
            return m1.Translate(v);
        }

        /// <summary>
        /// Rotates mass.
        /// </summary>
        public static Mass operator *(Quaterniond q, [NotNull] Mass m1)
        {
            return m1.Rotate(q);
        }

        /// <summary>
        /// Rotates mass.
        /// </summary>
        public static Mass operator *(Matrix3x3d m, [NotNull] Mass m1)
        {
            return m1.Rotate(m);
        }

        /// <summary>
        /// Translates mass.
        /// </summary>
        public static Mass operator *(Vector3d v, [NotNull] Mass m1)
        {
            return m1.Translate(v);
        }

        public static bool operator ==([NotNull] Mass m1, [NotNull] Mass m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator !=(Mass m1, Mass m2)
        {
            return !(m1 == m2);
        }

        #endregion

        #region Properties

        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return (Mass)obj == this;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("Mass: {0} Center: {1}", mass, centerOfMass);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Constructs the mass object.
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="centerOfMass"></param>
        /// <param name="inertia"></param>
        public Mass([MinDouble(0.0)] double mass, Vector3d centerOfMass, Matrix3x3d inertia)
        {
            if (!MatrixHelper.IsSymetric(inertia))
            {
                throw new ArgumentException("Inertia must be simetric matric.");
            }

            this.mass = mass;
            this.centerOfMass = centerOfMass;
            this.inertia = inertia;
        }

        /// <summary>
        /// Returns the center of mass.
        /// </summary>
        public Vector3d CenterOfMass
        {
            get
            {
                return centerOfMass;
            }
        }

        /// <summary>
        /// Returns the inertia.
        /// </summary>
        public Matrix3x3d Inertia
        {
            get
            {
                return inertia;
            }
        }

        /// <summary>
        /// Returns the mass.
        /// </summary>
        public double TotalMass
        {
            get
            {
                return mass;
            }
        }

        

        #endregion

        #region Public Methods

        /// <summary>
        /// Translates the mass.
        /// </summary>
        public Mass Translate(Vector3d m)
        {
            return new Mass(mass, centerOfMass + m, inertia);
        }

        /// <summary>
        /// Rotates the mass.
        /// </summary>
        public Mass Rotate(Quaterniond q)
        {
            return Rotate(q.ToMatrix());
        }

        /// <summary>
        /// Rotates the mass.
        /// </summary>
        public Mass Rotate(Matrix3x3d m)
        {
            return new Mass(mass, centerOfMass, inertia * m);
        }

        /// <summary>
        /// Adds two masses.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Mass Add(Mass m)
        {
            // A normalization factor.
            double d = 1.0 / (m.mass + mass);

            return new Mass(m.mass + mass,
                mass * d * centerOfMass + m.mass * d * m.centerOfMass,
                inertia + m.inertia);

        }

        /// <summary>
        /// Adjusts the mass.
        /// </summary>
        /// <param name="mass"></param>
        /// <returns></returns>
        public Mass Adjust(double mass)
        {
            return new Mass(mass, centerOfMass, inertia);
        }

        /// <summary>
        /// Returns the average density.
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public double AverageDensity(double volume)
        {
            return mass / volume;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Creates a spherical mass distribution.
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Mass CreateSphere(double mass, double radius)
        {
            double I = 2.0 / 5.0 * mass * radius * radius;

            return new Mass(mass, Vector3d.Zero,
                new Matrix3x3d(I, 0, 0,
                               0, I, 0,
                               0, 0, I));
        }

        /// <summary>
        /// Creates sphere given a density.
        /// </summary>
        /// <param name="density"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Mass CreateSphereWithDensity(double density, double radius)
        {
            return CreateSphere(density * 4.0 / 3.0 * MathHelper.PI * radius * radius * radius, radius);
        }



        #endregion

        #region IEquatable<Mass> Members

        public bool Equals(Mass other)
        {
            if (object.ReferenceEquals(this, other)) return true;

            if (!MathHelper.NearEqual(mass, other.mass)) return false;
            if (!Vector3d.NearEqual(centerOfMass, other.centerOfMass)) return false;
            if (!Matrix3x3d.NearEqual(inertia, other.inertia)) return false;
            return true;
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class MassTest
    {
        [CorrectnessTest]
        public void Usage()
        {
            // Creates mass by adding two spherical masses.
            {
                Mass mass1 = Mass.CreateSphere(3.0, 0.2).Translate(Vector3d.AxisX);
                Mass mass2 = Mass.CreateSphere(3.0, 0.2).Translate(-Vector3d.AxisX);
                Mass mass = mass1.Add(mass2);
            }

            // Creates masses by adding them in operator form.
            {
                Mass mass1 = Mass.CreateSphere(3.0, 0.2) * Vector3d.AxisX;
                Mass mass2 = Mass.CreateSphere(3.0, 0.2) * -Vector3d.AxisX;
                Mass mass = mass1 + mass2;
            }
        }
    }

#endif
}
