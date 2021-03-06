// This file was generated by TemplateEngine from template source 'LineSegment'
// using template 'LineSegment2f. Do not modify this file directly, modify it from template source.

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
using SharpMedia.Math.Shapes.Volumes;

namespace SharpMedia.Math.Shapes
{


    /// <summary>
    /// A line in 2D space. Line is determined by two points, A and B point.
    /// </summary>
    [Serializable]
    public sealed class LineSegment2f :
        IOutline2f, IControlPoints2f, IContainsPoint2f, ITransformable2f,
        IEquatable<LineSegment2f>, IComparable<LineSegment2f>,
        IEnumerable<Vector2f>, ICloneable<LineSegment2f>
        
    {

        #region Public Members

        /// <summary>
        /// The start control point.
        /// </summary>
        public Vector2f A;

        /// <summary>
        /// Ending control point.
        /// </summary>
        public Vector2f B;

        #endregion

        #region Constructors


        /// <summary>
        /// Empty contructor.
        /// </summary>
        public LineSegment2f()
        {
        }

        /// <summary>
        /// Line constructor.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="to">To point.</param>
        public LineSegment2f(Vector2f a, Vector2f b)
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
        public LineSegment2f(Vector2f from, Vector2f direction, float length)
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
        public bool IsParallel([NotNull] LineSegment2f other)
        {
            if (Vector2f.NearEqual(Direction, other.Direction)) return true;
            return false;
        }

        /// <summary>
        /// The lines are perpendicular.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <returns>Are lines perpBicular.</returns>
        public bool IsPerpendicular([NotNull] LineSegment2f other)
        {
            Vector2f special;
            //#ifdef 2D

            special = new Vector2f((float) - 1, (float) - 1);
            //#endif

            return Vector2f.NearEqual(Direction,
                Vector2f.ComponentDivision(
                special, Direction));
        }

        #endregion

        #region Properties

        /// <summary>
        /// The direction of line. It's length is always the length
        /// of line.
        /// </summary>
        public Vector2f Direction
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
        public Vector2f UnitDirection
        {
            get { return Direction.Normal; }
            set
            {
                float l = Length;
                B = A + value * l;
            }
        }

        /// <summary>
        /// Length of line.
        /// </summary>
        public float Length
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
                else if (Length != 0.0f)
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
            if (obj is LineSegment2f) return this.Equals((LineSegment2f)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IContainsPoint2f Members

        public bool ContainsPoint(Vector2f p)
        {
            // We get direction of one and direction of second.
            Vector2f x = p - A;

            // If p is the same as control.
            if (x.Length2 == 0.0f) return true;

            if (!Vector2f.NearEqual(x.Normal, Direction.Normal)) return false;
            float t = x.Length2 / Direction.Length2;
            return t >= 0.0f && t <= 1.0f;
        }

        #endregion

        #region IOutline2f Members

        public Vector2f Sample(float t)
        {
            return (1.0f - t) * A + t * B;
        }

        public void Sample(float resolution, Storage.Builders.ILineBuilder2f builder)
        {
            if (resolution < 0.0f)
            {
                builder.AddLineStrip(false, A, B);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IOutlinef Members

        public float OutlineLength
        {
            get { return (B - A).Length; }
        }

        #endregion

        #region IControlPoints2f Members

        public Vector2f[] ControlPoints
        {
            get
            {
                return new Vector2f[] { A, B };
            }
            set
            {
                if (value.Length != 2) throw new ArgumentException("Two control points expected.");
                A = value[0];
                B = value[1];
            }
        }

        public void SetControlPoints(uint index, Vector2f cp)
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

        public Vector2f GetControlPoint(uint index)
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

        #region IControlPointsf Members

        public uint ControlPointCount
        {
            get { return 2; }
        }

        #endregion

        #region ITransformable2f

        public void Transform(Matrix.Matrix3x3f matrix)
        {
            A = matrix * A;
            B = matrix * B;
        }

        #endregion

        

        #region ICloneable<LineSegment2f> Members

        public LineSegment2f Clone()
        {
            return new LineSegment2f(A, B);
        }

        #endregion

        #region IEquatable<LineSegment2f> Members

        public bool Equals(LineSegment2f other)
        {
            if (Vector2f.NearEqual(this.A, other.A) &&
                Vector2f.NearEqual(this.B, other.B)) return true;
            return false;
        }
        #endregion

        #region IComparable<ClassName> Members

        public int CompareTo(LineSegment2f other)
        {
            int cmp = A.CompareTo(other.A);
            if (cmp != 0) return cmp;
            return B.CompareTo(other.B);

        }

        #endregion

        #region IEnumerable<Vector2f> Members

        public IEnumerator<Vector2f> GetEnumerator()
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
    internal class LineSegment2f_Test
    {
        /*
        [CorrectnessTest]
        public void LineConstruction()
        {
            LineSegment2f line1 = new LineSegment2f(new Vector2f(0, 0, 0), new Vector2f(3, 3, 3));
            LineSegment2f line2 = new LineSegment2f(new Vector2f(0, 0, 0), new Vector2f(1, 1, 1), 3.0);

            Assert.IsTrue(line1.Equals(line1));
            Assert.IsTrue(line2.Equals(line2));
            Assert.IsTrue(line1.Equals(line2));
        }

        [CorrectnessTest]
        public void Properties()
        {
            LineSegment2f line1 = new LineSegment2f(new Vector2f(0, 0, 0), new Vector2f(2, 0, 0));
            Assert.AreEqual(2.0, line1.Length);
            Assert.AreEqual(1, line1.LineSegmentCount);
            Assert.AreEqual(1, line1.Lines.Length);
            Assert.AreEqual(line1.UnitDirection, new Vector2f(1, 0, 0));
        }

        [CorrectnessTest]
        public void Contains()
        {
            LineSegment2f line = new LineSegment2f(new Vector2f(1, 0, 0), new Vector2f(2, 1, 0));
            Assert.IsTrue(line.ContainsPoint(new Vector2f(1.5, 0.5, 0)));
        }

        [CorrectnessTest]
        public void Intersection()
        {
            LineSegment2f line1 = new LineSegment2f(new Vector2f(0, 0, 0), new Vector2f(3, 0, 0));
            LineSegment2f line2 = new LineSegment2f(new Vector2f(1, -1, 0), new Vector2f(1, 5, 0));

            Assert.IsTrue(line1.IntersectsWith(line2));
            Pointd p = (Pointd)line1.Intersection(line2)[0];
            Assert.AreEqual(new Pointd(1, 0, 0), p);
        }*/
    }
#endif
}
