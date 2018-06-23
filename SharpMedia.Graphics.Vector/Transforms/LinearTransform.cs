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

namespace SharpMedia.Graphics.Vector.Transforms
{
    /// <summary>
    /// A linear transform.
    /// </summary>
    public sealed class LinearTransform : ITransform
    {
        #region Private Members
        Matrix4x4f transform = Matrix4x4f.Identity;
        #endregion

        #region Constructors

        /// <summary>
        /// Identity transform.
        /// </summary>
        public LinearTransform()
        {
        }

        /// <summary>
        /// Linear transform from matrix.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public LinearTransform(Matrix4x4f transform)
        {
            this.transform = transform;
        }

        #endregion

        #region ICloneable<ITransform> Members

        public ITransform Clone()
        {
            return new LinearTransform(transform.Clone());
        }

        #endregion

        #region ITransform Members

        public bool NeedsPreprocess
        {
            get { return false; }
        }

        public Matrix4x4f RuntimeForm
        {
            get { return transform; }
        }

        public Vector2f[] Preprocess(Vector2f[] data)
        {
            // No-op.
            throw new InvalidOperationException();
        }

        public object Preprocess(PreprocessDataType type, object data)
        {
            // No-op.
            throw new InvalidOperationException();
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Blends linear transforms.
        /// </summary>
        /// <param name="transform1"></param>
        /// <param name="transform2"></param>
        /// <returns></returns>
        public static LinearTransform Blend(float blend, LinearTransform transform1, LinearTransform transform2)
        {
            if (transform1 == null && transform2 == null)
                return new LinearTransform();
            if (transform1 == null) return transform2;
            if (transform2 == null) return transform1;

            // A linear interpolation (in future, we can do better;
            // spline interpolation for rotations and linear for transformations.
            return new LinearTransform(transform1.RuntimeForm * (1.0f - blend)
                + transform2.RuntimeForm * blend);
        }

        #endregion
    }
}
