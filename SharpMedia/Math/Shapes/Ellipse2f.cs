// This file was generated by TemplateEngine from template source 'Ellipse'
// using template 'Ellipse2f. Do not modify this file directly, modify it from template source.

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
    /// An 2D ellipse.
    /// </summary>
    [Serializable]
    public class Ellipse2f :
        IArea2f, IOutline2f, IControlPoints2f, IContainsPoint2f, ITransformable2f,
        IEquatable<Ellipse2f>, IComparable<Ellipse2f>,
        IEnumerable<Vector2f>, ICloneable<Ellipse2f>
        

    {
        #region Public Members

        /// <summary>
        /// The first axis, length is the radius of ellipse in that direction.
        /// </summary>
        /// <remarks>Must be perpendicular to AxisV.</remarks>
        public Vector2f AxisU;

        /// <summary>
        /// The second axis, length is the radius of ellipse in that direction.
        /// </summary>
        /// <remarks>Must be perpendicular to AxisU.</remarks>
        public Vector2f AxisV;

        /// <summary>
        /// The center of ellipse.
        /// </summary>
        public Vector2f Center;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructor of ellipse in central position.
        /// </summary>
        /// <param name="axisU">Radius in x axis.</param>
        /// <param name="axisV">Radius in y axis.</param>
        public Ellipse2f(Vector2f axisU, Vector2f axisV)
        {
            AxisU = axisU;
            AxisV = axisV;
        }

        /// <summary>
        /// Displaced ellipse.
        /// </summary>
        /// <param name="axisU">Radius in x axis.</param>
        /// <param name="axisV">Radius in y axis.</param>
        /// <param name="center">Displacement.</param>
        public Ellipse2f(Vector2f axisU, Vector2f axisV, Vector2f center)
            : this(axisU, axisV)
        {
            Center = center;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The radius in u direction.
        /// </summary>
        public float A
        {
            get { return AxisU.Length; }
            set { AxisU.Length = value; }
        }

        /// <summary>
        /// Radius in v direction.
        /// </summary>
        public float B
        {
            get { return AxisV.Length; }
            set { AxisV.Length = value; }
        }

        /// <summary>
        /// The eccentricity.
        /// </summary>
        public float E
        {
            get
            {
                if (AxisU.Length2 > AxisV.Length2) return MathHelper.Sqrt(AxisU.Length2 - AxisV.Length2);
                return MathHelper.Sqrt(AxisV.Length2 - AxisU.Length2);
            }
        }

        /// <summary>
        /// The left/bottom focus of ellipse.
        /// </summary>
        public Vector2f Focus1
        {
            get
            {
                if (AxisU.Length2 > AxisV.Length2)
                {
                    return Center - AxisU.Normal * E;
                }
                else
                {
                    return Center - AxisV.Normal * E;
                }
            }
        }

        /// <summary>
        /// The right/top focus of ellipse.
        /// </summary>
        public Vector2f Focus2
        {
            get
            {
                if (AxisU.Length2 > AxisV.Length2)
                {
                    return Center + AxisU.Normal * E;
                }
                else
                {
                    return Center + AxisV.Normal * E;
                }
            }
        }

        /// <summary>
        ///  The Eccentricity, or how much the ellipse is squized.
        /// </summary>
        public float Eccentricity
        {
            get
            {
                if (AxisU.Length2 > AxisV.Length2)
                {
                    return MathHelper.Sqrt(1.0f - (AxisV.Length2) / (AxisU.Length2));
                }
                else
                {
                    return MathHelper.Sqrt(1.0f - (AxisU.Length2) / (AxisV.Length2));
                }
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(100);
            builder.Append("Ellipse : {");
            builder.Append(Center.ToString());
            builder.Append(", AxisU:");
            builder.Append(AxisU.ToString());
            builder.Append(", AxisV:");
            builder.Append(AxisV.ToString());
            builder.Append("}");
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Ellipse2f)
            {
                return this.Equals((Ellipse2f)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IArea2f Members

        public float Area
        {
            get
            {
                return (float)MathHelper.PI * AxisU.Length * AxisV.Length;
            }
        }

        public void Tesselate(float resolution, Storage.Builders.ITriangleBuilder2f builder)
        {
            throw new NotImplementedException();

        }

        #endregion

        #region IOutline2f Members

        public Vector2f Sample(float t)
        {
            t *= (float)(2.0 * MathHelper.PI);
            //#ifdef 2D

            Vector2f sample = new Vector2f(MathHelper.Cos(t), MathHelper.Sin(t));

            // We transform by axis.
            Matrix.Matrix2x2f transform = 
                new Matrix.Matrix2x2f(AxisU.X, AxisV.X,
                                               AxisU.Y, AxisV.Y);
            sample = transform * sample;

            // We return transformed position.
            return Center + sample;

            //#endif
        }

        public void Sample(float resolution, Storage.Builders.ILineBuilder2f builder)
        {
            // Adaptive and normal sampling the same (curvative either way).
            if (resolution < 0.0f) resolution = -resolution;

            throw new NotImplementedException();
        }

        #endregion

        #region IOutlinef Members

        public float OutlineLength
        {
            get
            {
                float a = A, b = B;
                float tmp = (float)3.0 * ((a-b)*(a-b))/((a+b)*(a+b));

                // We provide an approximation (source: http://en.wikipedia.org/wiki/Ellipse).
                return (float)MathHelper.PI * (A + B) *
                    (1.0f + tmp / ((float)10.0 + MathHelper.Sqrt((float)4.0 - tmp)));

            }
        }

        #endregion

        #region IControlPoints2f Members

        public Vector2f[] ControlPoints
        {
            get
            {
                return new Vector2f[] { Center };
            }
            set
            {
                if (value.Length != 1) throw new ArgumentException("One control points expected.");
                Center = value[0];
            }
        }

        public void SetControlPoints(uint index, Vector2f cp)
        {
            switch (index)
            {
                case 0:
                    Center = cp;
                    break;

                default:
                    throw new ArgumentException("Index out of range, must be 0 for ellpise.");
            }
        }

        public Vector2f GetControlPoint(uint index)
        {
            switch (index)
            {
                case 0:
                    return Center;
                default:
                    throw new ArgumentException("Index out of range, must be 0 for ellipse.");
            }
        }

        #endregion

        #region IControlPointsf Members

        public uint ControlPointCount
        {
            get { return 1; }
        }

        #endregion

        #region IContainsPoint2f Members

        public bool ContainsPoint(Vector2f point)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITransformable2f

        public void Transform(Matrix.Matrix3x3f matrix)
        {
            Center = matrix * Center;
        }

        #endregion

        

        #region IEnumerable<Vector2f> Members

        public IEnumerator<Vector2f> GetEnumerator()
        {
            yield return Center;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield return Center;
        }

        #endregion

        #region IEquatable<Ellipse2f> Members

        public bool Equals(Ellipse2f other)
        {
            if (Vector2f.NearEqual(Center, other.Center) &&
                Vector2f.NearEqual(AxisU, other.AxisU) &&
                Vector2f.NearEqual(AxisV, other.AxisV))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ICloneable Members

        public Ellipse2f Clone()
        {
            return new Ellipse2f(AxisU, AxisV, Center);
        }

        #endregion

        #region IComparable<Triangled> Members

        public int CompareTo(Ellipse2f other)
        {
            float a1 = Area, a2 = other.Area;
            if (a1 < a2) return -1;
            else if (a1 == a2) return 0;
            return 1;
        }

        #endregion

    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class Test_Ellipse2f
    {
        /*
        [CorrectnessTest]
        public void Properties()
        {
            Ellipse2f el = new Ellipse2f(10.0, 8.0);
            Assert.AreEqual(10.0, el.A);
            Assert.AreEqual(8.0, el.B);
            Assert.AreEqual(MathHelper.PI * 10.0 * 8.0, el.Area);
            Assert.AreEqual(6.0 / 10.0, el.Eccentricity);
            Assert.AreEqual(Vector2f.Zero, el.Center);
        }

        [CorrectnessTest]
        public void Tesselation()
        {
            Ellipse2f el = new Ellipse2f(10.0, 8.0);
            TriangleMeshd result = el.TriangleTesselation(10);
            //Assert.AreEqual(10, result.Count);

        }*/
    }
#endif
}
