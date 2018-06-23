// This file was generated by TemplateEngine from template source 'OutlineCompund'
// using template 'OutlineCompound3f. Do not modify this file directly, modify it from template source.

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
using SharpMedia.Math.Matrix;
using SharpMedia.Math.Shapes.Algorithms;

namespace SharpMedia.Math.Shapes.Compounds
{



    /// <summary>
    /// An outline compund.
    /// </summary>
    /// <remarks>Outline class is provided to enable "easy" access to tesselation and storage
    /// of linked shapes. Other operations are also provided.</remarks>
    [Serializable]
    public sealed class OutlineCompound3f :
        IArea3f, IOutline3f, ITransformable3f, ICloneable<OutlineCompound3f>
    {
        #region Private Class

        /// <summary>
        /// A helper class that is used to create line contours.
        /// </summary>
        private class ContourBuilder : Storage.Builders.ILineBuilder3f
        {
            #region Private Members
            List<Vector3f[]> contours = new List<Vector3f[]>();
            #endregion

            #region Properties

            /// <summary>
            /// Obtains contours.
            /// </summary>
            public List<Vector3f[]> Contours
            {
                get
                {
                    return contours;
                }
            }

            #endregion

            #region Contructors

            /// <summary>
            /// Default constructor.
            /// </summary>
            public ContourBuilder()
            {
            }

            #endregion

            #region Helper Methods

            void FinishContour()
            {
                if (contours.Count == 0) return;
                Vector3f[] lastCountour = contours[contours.Count - 1];
                if (lastCountour == null) return;
                contours.Add(null);
            }


            #endregion

            #region Builders.ILineBuilder3f Members

            public bool IsIndexed
            {
                get { return false; }
            }


            public uint AddControlPoints(params Vector3f[] data)
            {
                // We try to figure out strips, if possible.

                throw new NotImplementedException();
            }


            public void AddIndexedLines(params uint[] indices)
            {
                throw new InvalidOperationException("The call to AddIndexedLines is invalid because builder is not indexed.");
            }

            public void AddLineStrip(bool linkFirstToLast, params Vector3f[] data)
            {
                // We try to inteligently add a strip.
                if (linkFirstToLast)
                {
                    // We make sure strip is finised.
                    FinishContour();

                    // We surely have a seperate strip.
                    Vector3f[] newData = new Vector3f[data.Length + 1];
                    data.CopyTo(newData, 0);
                    newData[data.Length] = data[0];
                    contours.Add(newData);

                    // We make sure strip is finished.
                    FinishContour();
                }
                else
                {
                    // We check if we can continue strip.
                    if (contours.Count > 0)
                    {
                        Vector3f[] lastContour = contours[contours.Count - 1];

                        // It means it is ending.
                        if (lastContour == null)
                        {
                            contours.Add(data);
                        }

                        // Else we check for append.
                        if (lastContour != null && Vector3f.NearEqual(lastContour[lastContour.Length - 1], data[0]))
                        {
                            // We append data.
                            Vector3f[] newData = new Vector3f[data.Length + lastContour.Length];
                            int i;
                            for (i = 0; i < lastContour.Length; i++)
                            {
                                newData[i] = lastContour[i];
                            }
                            for (; i < newData.Length; i++)
                            {
                                newData[i] = data[i - lastContour.Length];
                            }

                            contours[contours.Count - 1] = newData;
                            if (Vector3f.NearEqual(newData[0], newData[newData.Length - 1])) FinishContour();

                        }
                        else
                        {
                            FinishContour();
                            contours.Add(data);
                        }
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Public Members
        IOutline3f[] outlines;
        #endregion

        #region Properties

        /// <summary>
        /// Outline indexer.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IOutline3f this[uint index]
        {
            get
            {
                return outlines[index];
            }
        }

        /// <summary>
        /// Is the path closed.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The stop indices, if any.
        /// </summary>
        public uint[] StopIndices
        {
            get
            {
                List<uint> stops = new List<uint>();
                for (int i = 1; i < outlines.Length; i++)
                {
                    if (Vector3f.NearEqual(outlines[i].Sample(0.0f), outlines[i - 1].Sample(1.0f)))
                    {
                        stops.Add((uint)(i - 1));
                    }
                }
                return stops.ToArray();
            }
        }



        #endregion

        #region Public Members

        

        #endregion

        #region Constructors

        /// <summary>
        /// Triangle constructor.
        /// </summary>
        public OutlineCompound3f()
        {
        }

        /// <summary>
        /// Triangle construction.
        /// </summary>
        /// <param name="data"></param>
        public OutlineCompound3f(params IOutline3f[] outlines)
        {
            this.outlines = outlines;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is OutlineCompound3f)
            {
                return this.Equals((OutlineCompound3f)obj);
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
                return float.NaN;
            }
        }

        public void Tesselate(float resolution, Storage.Builders.ITriangleBuilder3f builder)
        {
            // We create a special line builder that will create contours.
            ContourBuilder contourBuilder = new ContourBuilder();
            Sample(resolution, contourBuilder);

            // We now invoke the algorithm.
            Algorithms.PolygonTesselation.Tesselate(contourBuilder.Contours, builder);
        }

        #endregion

        #region IOutline3f Members

        public Vector3f Sample(float t)
        {
            uint outlineIndex = (uint)(t * (float)outlines.Length);
            float interp = t * (float)outlines.Length - (float)outlineIndex;

            return outlines[outlineIndex].Sample(interp);
        }

        public void Sample(float resolution, Storage.Builders.ILineBuilder3f builder)
        {
            for (int i = 0; i < outlines.Length; i++)
            {
                outlines[i].Sample(resolution, builder);
            }
        }

        #endregion

        #region IOutlinef Members

        public float OutlineLength
        {
            get
            {
                float length = 0.0f;
                for (int i = 0; i < outlines.Length; i++)
                {
                    length += outlines[i].OutlineLength;
                }
                return length;
            }

        }

        #endregion

        #region ITransformable3f

        public void Transform(Matrix.Matrix4x4f matrix)
        {
            for (int i = 0; i < outlines.Length; i++)
            {
                if (!(outlines[i] is ITransformable3f))
                {
                    throw new InvalidOperationException(
                        string.Format("The outline '{0}' at index '{1}' is not transformable", outlines[i], i));
                }
            }

            for (int i = 0; i < outlines.Length; i++)
            {
                (outlines[i] as ITransformable3f).Transform(matrix);
            }
        }

        #endregion

        #region ICloneable Members

        public OutlineCompound3f Clone()
        {
            return new OutlineCompound3f(outlines);
        }

        #endregion
    }


#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class Test_OutlineCompound3f
    {
        [CorrectnessTest]
        public void Construction()
        {
            
        }

       

    }
#endif
}
