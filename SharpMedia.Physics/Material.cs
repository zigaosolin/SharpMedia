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

namespace SharpMedia.Physics
{
    /// <summary>
    /// A physics material.
    /// </summary>
    public sealed class Material : IEquatable<Material>, ICloneable<Material>
    {
        #region Private Members
        Vector2d staticFriction = new Vector2d(0.5, 0.5);
        Vector2d dynamicFriction = new Vector2d(0.5, 0.5);
        double bounciness = 0.0;
        Vector3d anisotropyDirection = Vector3d.AxisX;
        CombineMode frictionCombine = CombineMode.Average;
        CombineMode bouncinessCombine = CombineMode.Add;
        #endregion

        #region Properties

        /// <summary>
        /// Static friction.
        /// </summary>
        public double StaticFriction
        {
            get
            {
                return staticFriction.X;
            }
            [param: MinDouble(0.0)]
            set
            {
                StaticFrictionUV = new Vector2d(value, value);
            }
        }

        /// <summary>
        /// Set anisotropic static friction.
        /// </summary>
        public Vector2d StaticFrictionUV
        {
            get
            {
                return staticFriction;
            }
            set
            {
                if(value.X < 0 || value.Y < 0) throw new ArgumentException("Friction must be positive.");

                staticFriction = value;

                // Dynamic friction must be smaller.
                if (dynamicFriction.X > staticFriction.X) dynamicFriction.X = staticFriction.X;
                if (dynamicFriction.Y > staticFriction.Y) dynamicFriction.Y = staticFriction.Y;
            }
        }

        /// <summary>
        /// Dynamic friction.
        /// </summary>
        public double DynamicFriction
        {
            get
            {
                return dynamicFriction.X;
            }
            [param: MinDouble(0.0)]
            set
            {
                DynamicFrictionUV = new Vector2d(value, value);
            }
        }

        /// <summary>
        /// Sets anisotropic dynamic friction.
        /// </summary>
        public Vector2d DynamicFrictionUV
        {
            get
            {
                return dynamicFriction;
            }
            set
            {
                if(value.X < 0 || value.Y < 0) throw new ArgumentException("Friction must be positive.");

                dynamicFriction = value;

                // Static friction must be bigger.
                if (staticFriction.X < dynamicFriction.X) staticFriction.X = dynamicFriction.X;
                if (staticFriction.Y < dynamicFriction.Y) staticFriction.Y = dynamicFriction.Y;
            }
        }

        /// <summary>
        /// The bouciness, must be in range [0,1].
        /// </summary>
        public double Bounciness
        {
            get
            {
                return bounciness;
            }
            [InBoundsDouble(0.0, 1.0)]
            set
            {
                bounciness = value;
            }
        }

        /// <summary>
        /// Direction of anisotropy. Used if Friction is different in U and V.
        /// </summary>
        public Vector3d DirectionOfAnisotropy
        {
            get
            {
                return anisotropyDirection;
            }
            set
            {
                anisotropyDirection = value.Normal;
            }
        }

        /// <summary>
        /// Sets or gets friction combine mode.
        /// </summary>
        public CombineMode FrictionCombineMode
        {
            get
            {
                return frictionCombine;
            }
            set
            {
                frictionCombine = value;
            }
        }

        /// <summary>
        /// Sets or gets bounciness combine mode.
        /// </summary>
        public CombineMode BouncinessCombineMode
        {
            get
            {
                return bouncinessCombine;
            }
            set
            {
                bouncinessCombine = value;
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// The material constructor.
        /// </summary>
        public Material()
        {
        }

        #endregion

        #region IEquatable<Material> Members

        public bool Equals([NotNull] Material other)
        {
            if (staticFriction != other.staticFriction) return false;
            if (dynamicFriction != other.dynamicFriction) return false;
            if (bounciness != other.bounciness) return false;
            if (bouncinessCombine != other.bouncinessCombine) return false;
            if (frictionCombine != other.frictionCombine) return false;

            // Check if direction of anisotropy is relavant.
            if (staticFriction.X != staticFriction.Y || dynamicFriction.X != dynamicFriction.Y)
            {
                if (anisotropyDirection != other.anisotropyDirection) return false;
            }
            return true;
        }

        #endregion

        #region ICloneable<Material> Members

        public Material Clone()
        {
            Material m = new Material();
            m.anisotropyDirection = anisotropyDirection;
            m.frictionCombine = frictionCombine;
            m.bouncinessCombine = bouncinessCombine;
            m.bounciness = bounciness;
            m.dynamicFriction = dynamicFriction;
            m.staticFriction = staticFriction;

            return m;
        }

        #endregion
    }
}
