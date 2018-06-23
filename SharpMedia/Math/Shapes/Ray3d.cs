// This file was generated by TemplateEngine from template source 'Ray'
// using template 'Ray3d. Do not modify this file directly, modify it from template source.

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

namespace SharpMedia.Math.Shapes
{

    /// <summary>
    /// A 3D ray.
    /// </summary>
    [Serializable]
    public sealed class Ray3d :
        IControlPoints3d, IContainsPoint3d, ITransformable3d,
        ICloneable<Ray3d>, IEquatable<Ray3d>, IComparable<Ray3d>,
        IEnumerable<Vector3d>
    {

        #region Public Members

        /// <summary>
        /// The origin of ray.
        /// </summary>
        public Vector3d Origin;

        /// <summary>
        /// The direction of ray.
        /// </summary>
        public Vector3d Direction;

        #endregion

        #region Constructors

        /// <summary>
        /// Contructor with vector and direction.
        /// </summary>
        /// <param name="p1">The starting point.</param>
        /// <param name="d">Direction of vector.</param>
        public Ray3d(Vector3d p1, Vector3d d)
        {
            Origin = p1;
            Direction = d;
        }

        /// <summary>
        /// Creates the ray from points.
        /// </summary>
        /// <param name="start">Starting point.</param>
        /// <param name="other">The other point.</param>
        /// <returns>Ray that begins in start and goes through other.</returns>
        public static Ray3d FromPoints(Vector3d start, Vector3d other)
        {
            return new Ray3d(start, start - other);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Other point on ray.
        /// </summary>
        public Vector3d OtherPoint
        {
            get { return Origin + Direction; }
            set { Direction = (value - Origin).Normal; }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(70);
            builder.Append("Ray : {");
            builder.Append(Origin);
            builder.Append(", ");
            builder.Append(Direction);
            builder.Append("}");
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Ray3d) return this.Equals((Ray3d)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IControlPoints3d Members

        public Vector3d[] ControlPoints
        {
            get
            {
                return new Vector3d[] { Origin, Origin + Direction };
            }
            set
            {
                if (value.Length != 2) throw new ArgumentException("Two control points expected.");
                Origin = value[0];
                Direction = value[1] - value[0];
            }
        }

        public void SetControlPoints(uint index, Vector3d cp)
        {
            switch (index)
            {
                case 0:
                    Origin = cp;
                    break;
                case 1:
                    Direction = cp - Origin;
                    break;
                default:
                    throw new ArgumentException("Index out of range, must be 0-1 for control points.");
            }
        }

        public Vector3d GetControlPoint(uint index)
        {
            switch (index)
            {
                case 0:
                    return Origin;
                case 1:
                    return Origin + Direction;
                default:
                    throw new ArgumentException("Index out of range, must be 0-1 for ray.");
            }
        }

        #endregion

        #region IControlPointsd Members

        public uint ControlPointCount
        {
            get { return 2; }
        }

        #endregion

        #region IContainsPoint3d Members

        public bool ContainsPoint(Vector3d point)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITransformable3d

        public void Transform(Matrix.Matrix4x4d matrix)
        {
            Vector3d other = OtherPoint;
            Origin = matrix * Origin;
            other = matrix * other;
            OtherPoint = other;
        }

        #endregion

        #region IEnumerable<Vector3d> Members

        public IEnumerator<Vector3d> GetEnumerator()
        {
            yield return Origin;
            yield return Origin + Direction;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield return Origin;
            yield return Origin + Direction;
        }

        #endregion

        #region IEquatable<Ray3d> Members

        public bool Equals(Ray3d other)
        {
            if (Vector3d.NearEqual(Origin, other.Origin) &&
                Vector3d.NearEqual(Direction, other.Direction))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ICloneable Members

        public Ray3d Clone()
        {
            return new Ray3d(Origin, Direction);
        }

        #endregion

        #region IComparable<Triangled> Members

        public int CompareTo(Ray3d other)
        {
            int cmp = Origin.CompareTo(other.Origin);
            if (cmp == 0) return Direction.CompareTo(other.Direction);
            return cmp;
        }

        #endregion
    }


}
