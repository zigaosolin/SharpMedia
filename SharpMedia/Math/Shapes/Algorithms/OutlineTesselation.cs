// This file was generated by TemplateEngine from template source 'OutlineTesselation'
// using template 'OutlineTesselation. Do not modify this file directly, modify it from template source.

using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.AspectOriented;

namespace SharpMedia.Math.Shapes.Algorithms
{

    /// <summary>
    /// The outline type identifier.
    /// </summary>
    public enum OutlineType
    {
        /// <summary>
        /// A solid line.
        /// </summary>
        Line,

        /// <summary>
        /// Dots are placed.
        /// </summary>
        Dot,

        /// <summary>
        /// The broken line.
        /// </summary>
        BrokenLine
    }

    /// <summary>
    /// The outline end.
    /// </summary>
    public enum OutlineEnd
    {
        /// <summary>
        /// This forces the first and the last to be equal.
        /// </summary>
        NoEnd,

        /// <summary>
        /// A square ending.
        /// </summary>
        Square,

        /// <summary>
        /// A half circle ending.
        /// </summary>
        HalfCircle

    }



    /// <summary>
    /// A polygon outline tesselation.
    /// </summary>
    public static class OutlineTesselation
    {
        #region Public Subclasses
        
		//#foreach instanced to 'Vector2d'


        /// <summary>
        /// The outline tesselation options.
        /// </summary>
        public sealed class TesselationOptionsd : ICloneable<TesselationOptionsd>
        {
            #region Private Members
            OutlineType outlineType = OutlineType.Line;
            OutlineEnd outlineEnd = OutlineEnd.Square;
            double param1 = 0.0;
            double param2 = 0.0;
            #endregion

            #region Constructors

            /// <summary>
            /// Constructor.
            /// </summary>
            public TesselationOptionsd()
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// The outline type.
            /// </summary>
            public OutlineType OutlineType
            {
                get { return outlineType; }
                set { outlineType = value; }
            }

            /// <summary>
            /// The outline ending.
            /// </summary>
            public OutlineEnd OutlineEnd
            {
                get { return outlineEnd; }
                set { outlineEnd = value; }
            }

            /// <summary>
            /// The line thickness.
            /// </summary>
            /// <remarks>Available only if outline type is line or broken line.</remarks>
            public double LineThickness
            {
                get { return param1; }
                set 
                {
                    if (outlineType != OutlineType.Line && outlineType != OutlineType.BrokenLine)
                    {
                        throw new InvalidOperationException("Outline type is invalid, cannot set line thickness.");
                    }
                    param1 = value; 
                }
            }

            /// <summary>
            /// The dot radius.
            /// </summary>
            public double DotRadius
            {
                get { return param1; }
                set
                {
                    if (outlineType != OutlineType.Dot)
                    {
                        throw new InvalidOperationException("Outline type is invalid, cannot set dot radius.");
                    }
                    param1 = value;
                }
            }

            /// <summary>
            /// The space between two broken lines.
            /// </summary>
            public double BrokenLineSpacing
            {
                get { return param2; }
                set
                {
                    if (outlineType != OutlineType.BrokenLine)
                    {
                        throw new InvalidOperationException("Outline type is invalid, cannot set broken line spacing.");
                    }
                    param2 = value;
                }
            }

            #endregion

            #region TesselationOptionsd Members

            public TesselationOptionsd Clone()
            {
                TesselationOptionsd clone = new TesselationOptionsd();
                clone.outlineType = outlineType;
                clone.outlineEnd = outlineEnd;
                clone.param1 = param1;
                clone.param2 = param2;

                return clone;
            }

            #endregion
        }

        //#endfor instanced to 'Vector2d'

		//#foreach instanced to 'Vector2f'


        /// <summary>
        /// The outline tesselation options.
        /// </summary>
        public sealed class TesselationOptionsf : ICloneable<TesselationOptionsf>
        {
            #region Private Members
            OutlineType outlineType = OutlineType.Line;
            OutlineEnd outlineEnd = OutlineEnd.Square;
            float param1 = 0.0f;
            float param2 = 0.0f;
            #endregion

            #region Constructors

            /// <summary>
            /// Constructor.
            /// </summary>
            public TesselationOptionsf()
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// The outline type.
            /// </summary>
            public OutlineType OutlineType
            {
                get { return outlineType; }
                set { outlineType = value; }
            }

            /// <summary>
            /// The outline ending.
            /// </summary>
            public OutlineEnd OutlineEnd
            {
                get { return outlineEnd; }
                set { outlineEnd = value; }
            }

            /// <summary>
            /// The line thickness.
            /// </summary>
            /// <remarks>Available only if outline type is line or broken line.</remarks>
            public float LineThickness
            {
                get { return param1; }
                set 
                {
                    if (outlineType != OutlineType.Line && outlineType != OutlineType.BrokenLine)
                    {
                        throw new InvalidOperationException("Outline type is invalid, cannot set line thickness.");
                    }
                    param1 = value; 
                }
            }

            /// <summary>
            /// The dot radius.
            /// </summary>
            public float DotRadius
            {
                get { return param1; }
                set
                {
                    if (outlineType != OutlineType.Dot)
                    {
                        throw new InvalidOperationException("Outline type is invalid, cannot set dot radius.");
                    }
                    param1 = value;
                }
            }

            /// <summary>
            /// The space between two broken lines.
            /// </summary>
            public float BrokenLineSpacing
            {
                get { return param2; }
                set
                {
                    if (outlineType != OutlineType.BrokenLine)
                    {
                        throw new InvalidOperationException("Outline type is invalid, cannot set broken line spacing.");
                    }
                    param2 = value;
                }
            }

            #endregion

            #region TesselationOptionsf Members

            public TesselationOptionsf Clone()
            {
                TesselationOptionsf clone = new TesselationOptionsf();
                clone.outlineType = outlineType;
                clone.outlineEnd = outlineEnd;
                clone.param1 = param1;
                clone.param2 = param2;

                return clone;
            }

            #endregion
        }

        //#endfor instanced to 'Vector2f'

        #endregion

        #region Private Members
        
		//#foreach instanced to 'Vector2d'


        /// <summary>
        /// Returns true, if p0 is upper point, p1, p2 are lower, false otherwise.
        /// </summary>
        static bool CalcPathIntersection(Vector2d a, Vector2d b, Vector2d c, double width,
            out Vector2d p0, out Vector2d p1, out Vector2d p2)
        {
            // We first generate two vectors.
            Vector2d dir1 = b - a;
            Vector2d dir2 = c - b;

            // We generate two perpendicular vectors.
            Vector2d grad1 = new Vector2d(-dir1.Y, dir1.X).Normal;
            Vector2d grad2 = new Vector2d(-dir2.Y, dir2.X).Normal;

            // We now generate 4 line segments.
            LineSegment2d seg1Up = new LineSegment2d(a + grad1 * width, b + grad1 * width);
            LineSegment2d seg1Down = new LineSegment2d(a - grad1 * width, b - grad1 * width);
            LineSegment2d seg2Up = new LineSegment2d(b + grad2 * width, c + grad2 * width);
            LineSegment2d seg2Down = new LineSegment2d(b - grad2 * width, c - grad2 * width);


            // We calculate intersections.
            double t1, t2;

            if (Intersection.Intersect(seg1Up, seg2Up, out t1, out t2))
            {
                // If they intersect, we have point 0.
                p0 = seg1Up.Sample(t1);

                p1 = seg1Down.B;
                p2 = seg2Down.A;

                return true;
            }
            else if (Intersection.Intersect(seg1Down, seg2Down, out t1, out t2))
            {
                p0 = seg1Down.Sample(t1);

                p1 = seg1Up.B;
                p2 = seg2Up.A;

                return false;

            }
            else
            {

                // If result is this, we have no intersections, numeric error. We use variables of one.
                p0 = seg1Up.B;

                p1 = p2 = seg1Down.B;

                return true;
            }
        }

        /// <summary>
        /// Tesselates outline as line (most common).
        /// </summary>
        /// <param name="points"></param>
        /// <param name="options"></param>
        /// <param name="mesh"></param>
        static void TesselateLine(Vector2d[] points, TesselationOptionsd options,
            Storage.Builders.ITriangleBuilder2d mesh)
        {
            // This is actually half width, because we go both ways.
            double width = options.LineThickness * (double)0.5;

            // Previous lower and upper coordinates, usually pen dependant.
            Vector2d grad = points[1] - points[0];
            grad = new Vector2d(-grad.Y, grad.X).Normal;
            Vector2d prevLower = points[0] - grad * width,
                     prevUpper = points[0] + grad * width;

            // We check if it is closed.
            bool isClosed = false;
            if (Vector2d.NearEqual(points[0], points[points.Length - 1]))
            {
                isClosed = true;

                // We correct prevLower and prevUpper.
                Vector2d p0, p1, p2;
                if (CalcPathIntersection(points[points.Length - 2], points[0], points[1],
                    width, out p0, out p1, out p2))
                {
                    prevUpper = p0;
                    prevLower = p2;
                }
                else
                {
                    prevUpper = p2;
                    prevLower = p0;
                }

            }
            else
            {
                if (options.OutlineEnd == OutlineEnd.NoEnd)
                {
                    throw new ArgumentException("The outline must be 'linked' in order to have no end.");
                }
            }

            // We insert prevUpper and predLower.
            uint prevUpperId = mesh.AddControlPoints(prevUpper, prevLower);
            uint prevLowerId = prevUpperId + 1;


            // We go through all points.
            for (int i = 1; i < points.Length - 1; i++)
            {
                Vector2d p0, p1, p2;
                if (CalcPathIntersection(points[i - 1], points[i], points[i + 1],
                    width, out p0, out p1, out p2))
                {
                    // We now add new values.
                    uint p0Id = mesh.AddControlPoints(p0, p1, p2);
                    uint p1Id = p0Id + 1;
                    uint p2Id = p0Id + 2;

                    // We now add triangles.
                    mesh.AddIndexedTriangles(prevLowerId, p0Id, prevUpperId,
                                             prevLowerId, p1Id, p0Id,
                                             p0Id, p1Id, p2Id);

                    prevLowerId = p2Id;
                    prevUpperId = p0Id;
                }
                else
                {
                    // We now add new values.
                    uint p0Id = mesh.AddControlPoints(p0, p1, p2);
                    uint p1Id = p0Id + 1;
                    uint p2Id = p0Id + 2;

                    mesh.AddIndexedTriangles(prevLowerId, p1Id, prevUpperId,
                                             prevLowerId, p0Id, p1Id,
                                             p0Id, p1Id, p2Id);

                    prevLowerId = p0Id;
                    prevUpperId = p2Id;
                }
            }

            if (isClosed)
            {
                // If it is closed, we must close it correctly.
                Vector2d p0, p1, p2;
                if (CalcPathIntersection(points[points.Length - 2], points[0], points[1],
                    width, out p0, out p1, out p2))
                {
                    // We now add new values.
                    uint p0Id = mesh.AddControlPoints(p0, p1, p2);
                    uint p1Id = p0Id + 1;
                    uint p2Id = p0Id + 2;

                    // We now add triangles.
                    mesh.AddIndexedTriangles(prevLowerId, p0Id, prevUpperId,
                                             prevLowerId, p1Id, p0Id,
                                             p0Id, p1Id, p2Id);

                }
                else
                {
                    // We now add new values.
                    uint p0Id = mesh.AddControlPoints(p0, p1, p2);
                    uint p1Id = p0Id + 1;
                    uint p2Id = p0Id + 2;

                    mesh.AddIndexedTriangles(prevLowerId, p1Id, prevUpperId,
                                             prevLowerId, p0Id, p1Id,
                                             p0Id, p1Id, p2Id);
                }
            }
            else
            {
                // TODO: may need to implement other endings.
                if (options.OutlineEnd != OutlineEnd.Square)
                {
                    throw new NotImplementedException("No other endings but 'Square' implemented.");
                }

                // Last point special again.
                Vector2d last = points[points.Length - 1];
                grad = last - points[points.Length - 2];
                grad = new Vector2d(-grad.Y, grad.X).Normal;

                Vector2d lower = last - grad * width;
                Vector2d upper = last + grad * width;

                // The lower id.
                uint lowerId = mesh.AddControlPoints(lower, upper);
                uint upperId = lowerId + 1;

                mesh.AddIndexedTriangles(prevLowerId, lowerId, upperId,
                                         prevLowerId, upperId, prevUpperId);

            }
        }


        //#endfor instanced to 'Vector2d'

		//#foreach instanced to 'Vector2f'


        /// <summary>
        /// Returns true, if p0 is upper point, p1, p2 are lower, false otherwise.
        /// </summary>
        static bool CalcPathIntersection(Vector2f a, Vector2f b, Vector2f c, float width,
            out Vector2f p0, out Vector2f p1, out Vector2f p2)
        {
            // We first generate two vectors.
            Vector2f dir1 = b - a;
            Vector2f dir2 = c - b;

            // We generate two perpendicular vectors.
            Vector2f grad1 = new Vector2f(-dir1.Y, dir1.X).Normal;
            Vector2f grad2 = new Vector2f(-dir2.Y, dir2.X).Normal;

            // We now generate 4 line segments.
            LineSegment2f seg1Up = new LineSegment2f(a + grad1 * width, b + grad1 * width);
            LineSegment2f seg1Down = new LineSegment2f(a - grad1 * width, b - grad1 * width);
            LineSegment2f seg2Up = new LineSegment2f(b + grad2 * width, c + grad2 * width);
            LineSegment2f seg2Down = new LineSegment2f(b - grad2 * width, c - grad2 * width);


            // We calculate intersections.
            float t1, t2;

            if (Intersection.Intersect(seg1Up, seg2Up, out t1, out t2))
            {
                // If they intersect, we have point 0.
                p0 = seg1Up.Sample(t1);

                p1 = seg1Down.B;
                p2 = seg2Down.A;

                return true;
            }
            else if (Intersection.Intersect(seg1Down, seg2Down, out t1, out t2))
            {
                p0 = seg1Down.Sample(t1);

                p1 = seg1Up.B;
                p2 = seg2Up.A;

                return false;

            }
            else
            {

                // If result is this, we have no intersections, numeric error. We use variables of one.
                p0 = seg1Up.B;

                p1 = p2 = seg1Down.B;

                return true;
            }
        }

        /// <summary>
        /// Tesselates outline as line (most common).
        /// </summary>
        /// <param name="points"></param>
        /// <param name="options"></param>
        /// <param name="mesh"></param>
        static void TesselateLine(Vector2f[] points, TesselationOptionsf options,
            Storage.Builders.ITriangleBuilder2f mesh)
        {
            // This is actually half width, because we go both ways.
            float width = options.LineThickness * (float)0.5;

            // Previous lower and upper coordinates, usually pen dependant.
            Vector2f grad = points[1] - points[0];
            grad = new Vector2f(-grad.Y, grad.X).Normal;
            Vector2f prevLower = points[0] - grad * width,
                     prevUpper = points[0] + grad * width;

            // We check if it is closed.
            bool isClosed = false;
            if (Vector2f.NearEqual(points[0], points[points.Length - 1]))
            {
                isClosed = true;

                // We correct prevLower and prevUpper.
                Vector2f p0, p1, p2;
                if (CalcPathIntersection(points[points.Length - 2], points[0], points[1],
                    width, out p0, out p1, out p2))
                {
                    prevUpper = p0;
                    prevLower = p2;
                }
                else
                {
                    prevUpper = p2;
                    prevLower = p0;
                }

            }
            else
            {
                if (options.OutlineEnd == OutlineEnd.NoEnd)
                {
                    throw new ArgumentException("The outline must be 'linked' in order to have no end.");
                }
            }

            // We insert prevUpper and predLower.
            uint prevUpperId = mesh.AddControlPoints(prevUpper, prevLower);
            uint prevLowerId = prevUpperId + 1;


            // We go through all points.
            for (int i = 1; i < points.Length - 1; i++)
            {
                Vector2f p0, p1, p2;
                if (CalcPathIntersection(points[i - 1], points[i], points[i + 1],
                    width, out p0, out p1, out p2))
                {
                    // We now add new values.
                    uint p0Id = mesh.AddControlPoints(p0, p1, p2);
                    uint p1Id = p0Id + 1;
                    uint p2Id = p0Id + 2;

                    // We now add triangles.
                    mesh.AddIndexedTriangles(prevLowerId, p0Id, prevUpperId,
                                             prevLowerId, p1Id, p0Id,
                                             p0Id, p1Id, p2Id);

                    prevLowerId = p2Id;
                    prevUpperId = p0Id;
                }
                else
                {
                    // We now add new values.
                    uint p0Id = mesh.AddControlPoints(p0, p1, p2);
                    uint p1Id = p0Id + 1;
                    uint p2Id = p0Id + 2;

                    mesh.AddIndexedTriangles(prevLowerId, p1Id, prevUpperId,
                                             prevLowerId, p0Id, p1Id,
                                             p0Id, p1Id, p2Id);

                    prevLowerId = p0Id;
                    prevUpperId = p2Id;
                }
            }

            if (isClosed)
            {
                // If it is closed, we must close it correctly.
                Vector2f p0, p1, p2;
                if (CalcPathIntersection(points[points.Length - 2], points[0], points[1],
                    width, out p0, out p1, out p2))
                {
                    // We now add new values.
                    uint p0Id = mesh.AddControlPoints(p0, p1, p2);
                    uint p1Id = p0Id + 1;
                    uint p2Id = p0Id + 2;

                    // We now add triangles.
                    mesh.AddIndexedTriangles(prevLowerId, p0Id, prevUpperId,
                                             prevLowerId, p1Id, p0Id,
                                             p0Id, p1Id, p2Id);

                }
                else
                {
                    // We now add new values.
                    uint p0Id = mesh.AddControlPoints(p0, p1, p2);
                    uint p1Id = p0Id + 1;
                    uint p2Id = p0Id + 2;

                    mesh.AddIndexedTriangles(prevLowerId, p1Id, prevUpperId,
                                             prevLowerId, p0Id, p1Id,
                                             p0Id, p1Id, p2Id);
                }
            }
            else
            {
                // TODO: may need to implement other endings.
                if (options.OutlineEnd != OutlineEnd.Square)
                {
                    throw new NotImplementedException("No other endings but 'Square' implemented.");
                }

                // Last point special again.
                Vector2f last = points[points.Length - 1];
                grad = last - points[points.Length - 2];
                grad = new Vector2f(-grad.Y, grad.X).Normal;

                Vector2f lower = last - grad * width;
                Vector2f upper = last + grad * width;

                // The lower id.
                uint lowerId = mesh.AddControlPoints(lower, upper);
                uint upperId = lowerId + 1;

                mesh.AddIndexedTriangles(prevLowerId, lowerId, upperId,
                                         prevLowerId, upperId, prevUpperId);

            }
        }


        //#endfor instanced to 'Vector2f'

        #endregion

        #region Public Members
        
		//#foreach instanced to 'Vector2d'


        /// <summary>
        /// Tesselates an outline using options to array of triangles.
        /// </summary>
        /// <param name="data">The input data. If options has OutlineEnd.NoEnd options, last and first element
        /// will be linked automatically (as with polygons). Otherwise, all points will be used.</param>
        /// <param name="options">The options of tesselation.</param>
        /// <param name="builder">The buolder that is used to build the mesh.</param>
        public static void Tesselate([NotNull] Vector2d[] data, [NotNull] TesselationOptionsd options, 
            [NotNull] Storage.Builders.ITriangleBuilder2d builder)
        {
            if (data.Length < 2) throw new ArgumentException("The data length must be at least 2, otherwise cannot tesselate.");

            if (options.OutlineType == OutlineType.Line)
            {
                TesselateLine(data, options, builder);
            }
            else
            {
                throw new NotImplementedException("The feature not yet implemented.");
            }
        }

        //#endfor instanced to 'Vector2d'

		//#foreach instanced to 'Vector2f'


        /// <summary>
        /// Tesselates an outline using options to array of triangles.
        /// </summary>
        /// <param name="data">The input data. If options has OutlineEnd.NoEnd options, last and first element
        /// will be linked automatically (as with polygons). Otherwise, all points will be used.</param>
        /// <param name="options">The options of tesselation.</param>
        /// <param name="builder">The buolder that is used to build the mesh.</param>
        public static void Tesselate([NotNull] Vector2f[] data, [NotNull] TesselationOptionsf options, 
            [NotNull] Storage.Builders.ITriangleBuilder2f builder)
        {
            if (data.Length < 2) throw new ArgumentException("The data length must be at least 2, otherwise cannot tesselate.");

            if (options.OutlineType == OutlineType.Line)
            {
                TesselateLine(data, options, builder);
            }
            else
            {
                throw new NotImplementedException("The feature not yet implemented.");
            }
        }

        //#endfor instanced to 'Vector2f'

        #endregion
    }
}