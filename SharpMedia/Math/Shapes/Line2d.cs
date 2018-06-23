// This file was generated by TemplateEngine from template source 'Line'
// using template 'Line2d. Do not modify this file directly, modify it from template source.

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
using SharpMedia.Testing;
using SharpMedia.AspectOriented;

namespace SharpMedia.Math.Shapes
{


    /// <summary>
    /// A line in 2D space. Line is determined by two points, A and B point.
    /// </summary>
    [Serializable]
    public sealed class Line2d :
        IOutline2d, IControlPoints2d, IContainsPoint2d, ITransformable2d,
        IEquatable<Line2d>, IComparable<Line2d>,
        IEnumerable<Vector2d>, ICloneable<Line2d>
    {

        #region Public Members

        /// <summary>
        /// The start control point.
        /// </summary>
        public Vector2d A;

        /// <summary>
        /// Ending control point.
        /// </summary>
        public Vector2d B;

        #endregion

        #region Constructors


        /// <summary>
        /// Empty contructor.
        /// </summary>
        public Line2d()
        {
        }

        /// <summary>
        /// Line constructor.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="to">To point.</param>
        public Line2d(Vector2d a, Vector2d b)
        {
            this.A = a;
            this.B = b;
        }

        /// <summary>
        /// Line constructor.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="length">Length of line in direction.</param>
        public Line2d(Vector2d from, Vector2d direction, double length)
        {
            this.A = from;
            this.B = from + length * direction;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Are two lines parallel.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <returns>Are they parallel.</returns>
        public bool IsParallel([NotNull] Line2d other)
        {
            if (Vector2d.NearEqual(Direction, other.Direction)) return true;
            return false;
        }

        /// <summary>
        /// The lines are perpBicular.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <returns>Are lines perpBicular.</returns>
        public bool IsPerpendicular([NotNull] Line2d other)
        {
            Vector2d special;
            //#ifdef 2D

            special = new Vector2d((double) - 1, (double) - 1);
            //#endif

            return Vector2d.NearEqual(Direction,
                Vector2d.ComponentDivision(
                special, Direction));
        }

        /// <summary>
        /// Distance from one point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>Distance.</returns>
        public double PointDistance(Vector3d p)
        {
            // The formula is:
            // d = |Direction x (A-p)| / |Direction|^2
            return (Direction ^ (A - p)).Length / Direction.Length2;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The direction of line. It's length is always the length
        /// of line.
        /// </summary>
        public Vector2d Direction
        {
            get { return B - A; }
            set
            {
                B = A + value;
            }
        }

        /// <summary>
        /// A unit direction.
        /// </summary>
        public Vector2d UnitDirection
        {
            get { return Direction.Normal; }
            set
            {
                double l = Length;
                B = A + value * l;
            }
        }

        /// <summary>
        /// Length of line.
        /// </summary>
        public double Length
        {
            get { return (A - B).Length; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Length cannot be negative");
                }
                else if (value == 0)
                {
                    B = A;
                }
                else if (Length != 0.0)
                {
                    // scaling
                    B = A + ((B - A) / Length * value);
                }
                else throw new Exception("Cannot scale zero-length lines (points)");
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(60);
            builder.Append("LineSegment : {");
            builder.Append(A.ToString());
            builder.Append(", ");
            builder.Append(B.ToString());
            builder.Append("}");
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Line2d) return this.Equals((Line2d)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IContainsPoint2d Members

        public bool ContainsPoint(Vector2d p)
        {
            // We get direction of one and direction of second.
            Vector2d x = p - A;

            // If p is the same as control.
            if (x.Length2 == 0.0) return true;

            if (!Vector2d.NearEqual(x.Normal, Direction.Normal)) return false;
            double t = x.Length2 / Direction.Length2;
            return t >= 0.0 && t <= 1.0;
        }

        #endregion

        #region IOutline2d Members

        public Vector2d Sample(double t)
        {
            return (1.0 - t) * A + t * B;
        }

        public void Sample(double resolution, Storage.Builders.ILineBuilder2d builder)
        {
            if (resolution < 0.0)
            {
                builder.AddLineStrip(false, A, B);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IOutlined Members

        public double OutlineLength
        {
            get { return (B - A).Length; }
        }

        #endregion

        #region IControlPoints2d Members

        public Vector2d[] ControlPoints
        {
            get
            {
                return new Vector2d[] { A, B };
            }
            set
            {
                if (value.Length != 2) throw new ArgumentException("Two control points expected.");
                A = value[0];
                B = value[1];
            }
        }

        public void SetControlPoints(uint index, Vector2d cp)
        {
            switch (index)
            {
                case 0:
                    A = cp;
                    break;
                case 1:
                    B = cp;
                    break;
                default:
                    throw new ArgumentException("Index out of range, must be 0-1 for line segment.");
            }
        }

        public Vector2d GetControlPoint(uint index)
        {
            switch (index)
            {
                case 0:
                    return A;
                case 1:
                    return B;
                default:
                    throw new ArgumentException("Index out of range, must be 0-1 for line segment.");
            }
        }

        #endregion

        #region IControlPointsd Members

        public uint ControlPointCount
        {
            get { return 2; }
        }

        #endregion

        #region ITransformable2d

        public void Transform(Matrix.Matrix3x3d matrix)
        {
            A = matrix * A;
            B = matrix * B;
        }

        #endregion

        #region ICloneable<Line2d> Members

        public Line2d Clone()
        {
            return new Line2d(A, B);
        }

        #endregion

        #region IEquatable<Line2d> Members

        public bool Equals(Line2d other)
        {
            if (Vector2d.NearEqual(this.Direction, other.Direction) &&
                this.ContainsPoint(other.A)) return true;
            return false;
        }
        #endregion

        #region IComparable<ClassName> Members

        public int CompareTo(Line2d other)
        {
            int cmp = A.CompareTo(other.A);
            if (cmp != 0) return cmp;
            return B.CompareTo(other.B);

        }

        #endregion

        #region IEnumerable<Vector2d> Members

        public IEnumerator<Vector2d> GetEnumerator()
        {
            yield return A;
            yield return B;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield return A;
            yield return B;
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class Line2d_Test
    {
        /*
        [CorrectnessTest]
        public void LineConstruction()
        {
            Line2d line1 = new Line2d(new Vector2d(0, 0, 0), new Vector2d(3, 3, 3));
            Line2d line2 = new Line2d(new Vector2d(0, 0, 0), new Vector2d(1, 1, 1), 3.0);

            Assert.IsTrue(line1.Equals(line1));
            Assert.IsTrue(line2.Equals(line2));
            Assert.IsTrue(line1.Equals(line2));
        }

        [CorrectnessTest]
        public void Properties()
        {
            Line2d line1 = new Line2d(new Vector2d(0, 0, 0), new Vector2d(2, 0, 0));
            Assert.AreEqual(2.0, line1.Length);
            Assert.AreEqual(1, line1.LineSegmentCount);
            Assert.AreEqual(1, line1.Lines.Length);
            Assert.AreEqual(line1.UnitDirection, new Vector2d(1, 0, 0));
        }

        [CorrectnessTest]
        public void Contains()
        {
            Line2d line = new Line2d(new Vector2d(1, 0, 0), new Vector2d(2, 1, 0));
            Assert.IsTrue(line.ContainsPoint(new Vector2d(1.5, 0.5, 0)));
        }

        [CorrectnessTest]
        public void Intersection()
        {
            Line2d line1 = new Line2d(new Vector2d(0, 0, 0), new Vector2d(3, 0, 0));
            Line2d line2 = new Line2d(new Vector2d(1, -1, 0), new Vector2d(1, 5, 0));

            Assert.IsTrue(line1.IntersectsWith(line2));
            Pointd p = (Pointd)line1.Intersection(line2)[0];
            Assert.AreEqual(new Pointd(1, 0, 0), p);
        }*/
    }
#endif
}