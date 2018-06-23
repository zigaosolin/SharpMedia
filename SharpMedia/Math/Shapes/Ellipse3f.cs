// This file was generated by TemplateEngine from template source 'Ellipse'
// using template 'Ellipse3f. Do not modify this file directly, modify it from template source.

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
    /// An 3D ellipse.
    /// </summary>
    [Serializable]
    public class Ellipse3f :
        IArea3f, IOutline3f, IControlPoints3f, IContainsPoint3f, ITransformable3f,
        IEquatable<Ellipse3f>, IComparable<Ellipse3f>,
        IEnumerable<Vector3f>, ICloneable<Ellipse3f>
        //#ifdef 3D

        , IAABoxBoundablef, ISphereBoundablef
        //#endif

    {
        #region Public Members

        /// <summary>
        /// The first axis, length is the radius of ellipse in that direction.
        /// </summary>
        /// <remarks>Must be perpendicular to AxisV.</remarks>
        public Vector3f AxisU;

        /// <summary>
        /// The second axis, length is the radius of ellipse in that direction.
        /// </summary>
        /// <remarks>Must be perpendicular to AxisU.</remarks>
        public Vector3f AxisV;

        /// <summary>
        /// The center of ellipse.
        /// </summary>
        public Vector3f Center;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructor of ellipse in central position.
        /// </summary>
        /// <param name="axisU">Radius in x axis.</param>
        /// <param name="axisV">Radius in y axis.</param>
        public Ellipse3f(Vector3f axisU, Vector3f axisV)
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
        public Ellipse3f(Vector3f axisU, Vector3f axisV, Vector3f center)
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
        public Vector3f Focus1
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
        public Vector3f Focus2
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
            if (obj is Ellipse3f)
            {
                return this.Equals((Ellipse3f)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IArea3f Members

        public float Area
        {
            get
            {
                return (float)MathHelper.PI * AxisU.Length * AxisV.Length;
            }
        }

        public void Tesselate(float resolution, Storage.Builders.ITriangleBuilder3f builder)
        {
            throw new NotImplementedException();

        }

        #endregion

        #region IOutline3f Members

        public Vector3f Sample(float t)
        {
            t *= (float)(2.0 * MathHelper.PI);
            
            Vector3f sample = new Vector3f(MathHelper.Cos(t), MathHelper.Sin(t), 0.0f);

            // We transform by axis.
            Vector3f other = AxisU ^ AxisV;
            Matrix.Matrix3x3f transform = 
                                     new Matrix.Matrix3x3f(AxisU.X, AxisV.X, other.X,
                                                                    AxisU.Y, AxisV.Y, other.Y,
                                                                    AxisU.Z, AxisV.Z, other.Z);
            sample = transform * sample;

            // We return transformed position.
            return Center + sample;
            
        }

        public void Sample(float resolution, Storage.Builders.ILineBuilder3f builder)
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

        #region IControlPoints3f Members

        public Vector3f[] ControlPoints
        {
            get
            {
                return new Vector3f[] { Center };
            }
            set
            {
                if (value.Length != 1) throw new ArgumentException("One control points expected.");
                Center = value[0];
            }
        }

        public void SetControlPoints(uint index, Vector3f cp)
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

        public Vector3f GetControlPoint(uint index)
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

        #region IContainsPoint3f Members

        public bool ContainsPoint(Vector3f point)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITransformable3f

        public void Transform(Matrix.Matrix4x4f matrix)
        {
            Center = matrix * Center;
        }

        #endregion

        //#ifdef 3D


        #region IAABoxBoundablef Members

        public AABoxf BoundingAABox
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ISphereBoundablef Members

        public Spheref BoundingSphere
        {
            get
            {
                return new Spheref(Center, MathHelper.Max(A, B));
            }
        }

        #endregion

        //#endif

        #region IEnumerable<Vector3f> Members

        public IEnumerator<Vector3f> GetEnumerator()
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

        #region IEquatable<Ellipse3f> Members

        public bool Equals(Ellipse3f other)
        {
            if (Vector3f.NearEqual(Center, other.Center) &&
                Vector3f.NearEqual(AxisU, other.AxisU) &&
                Vector3f.NearEqual(AxisV, other.AxisV))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ICloneable Members

        public Ellipse3f Clone()
        {
            return new Ellipse3f(AxisU, AxisV, Center);
        }

        #endregion

        #region IComparable<Triangled> Members

        public int CompareTo(Ellipse3f other)
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
    internal class Test_Ellipse3f
    {
        /*
        [CorrectnessTest]
        public void Properties()
        {
            Ellipse3f el = new Ellipse3f(10.0, 8.0);
            Assert.AreEqual(10.0, el.A);
            Assert.AreEqual(8.0, el.B);
            Assert.AreEqual(MathHelper.PI * 10.0 * 8.0, el.Area);
            Assert.AreEqual(6.0 / 10.0, el.Eccentricity);
            Assert.AreEqual(Vector3f.Zero, el.Center);
        }

        [CorrectnessTest]
        public void Tesselation()
        {
            Ellipse3f el = new Ellipse3f(10.0, 8.0);
            TriangleMeshd result = el.TriangleTesselation(10);
            //Assert.AreEqual(10, result.Count);

        }*/
    }
#endif
}