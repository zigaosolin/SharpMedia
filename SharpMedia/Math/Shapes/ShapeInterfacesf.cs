// This file was generated by TemplateEngine from template source 'ShapeInterfaces'
// using template 'ShapeInterfacesf. Do not modify this file directly, modify it from template source.

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

namespace SharpMedia.Math.Shapes
{

    /// <summary>
    /// Can calculate if point is contained in shape.
    /// </summary>
    public interface IContainsPoint2f
    {
        /// <summary>
        /// Is the point contained within shape.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        bool ContainsPoint(Vector2f point);
    }

    /// <summary>
    /// Can calculate if point is contained within shape.
    /// </summary>
    public interface IContainsPoint3f
    {
        /// <summary>
        /// Is the point contained within shape. Also applies to volumes.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        bool ContainsPoint(Vector3f point);
    }

    /// <summary>
    /// Transformability for shapes.
    /// </summary>
    public interface ITransformable2f
    {
        /// <summary>
        /// Transforms a shape by matrix.
        /// </summary>
        /// <param name="matrix"></param>
        void Transform(Matrix.Matrix3x3f matrix);
    }

    /// <summary>
    /// Transformability for shapes.
    /// </summary>
    public interface ITransformable3f
    {
        /// <summary>
        /// Transforms a shape by matrix.
        /// </summary>
        /// <param name="matrix"></param>
        void Transform(Matrix.Matrix4x4f matrix);
    }

    /// <summary>
    /// A shape that has vertices that control its shape.
    /// </summary>
    public interface IControlPointsf
    {
        /// <summary>
        /// Number of control points.
        /// </summary>
        uint ControlPointCount { get; }
    }

    /// <summary>
    /// A 2D-shape vertex controllable.
    /// </summary>
    public interface IControlPoints2f : IControlPointsf
    {
        /// <summary>
        /// Can get or set all control points at the same time.
        /// </summary>
        Vector2f[] ControlPoints { get; set; }

        /// <summary>
        /// Sets a specific control point.
        /// </summary>
        /// <param name="index">The index, zero based.</param>
        /// <param name="cp">The control point data.</param>
        /// <remarks>This may fail with some shapes, resulting in exception or altering
        /// of other control points in order to have a valid shape.</remarks>
        void SetControlPoints(uint index, Vector2f cp);

        /// <summary>
        /// Obtains an indexed control point.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Vector2f GetControlPoint(uint index);
    }

    /// <summary>
    /// A 3D-shape vertex controllable.
    /// </summary>
    public interface IControlPoints3f : IControlPointsf
    {
        /// <summary>
        /// Can get or set all control points at the same time.
        /// </summary>
        Vector3f[] ControlPoints { get; set; }

        /// <summary>
        /// Sets a specific control point.
        /// </summary>
        /// <param name="index">The index, zero based.</param>
        /// <param name="cp">The control point data.</param>
        /// <remarks>This may fail with some shapes, resulting in exception or altering
        /// of other control points in order to have a valid shape.</remarks>
        void SetControlPoints(uint index, Vector3f cp);

        /// <summary>
        /// Obtains an indexed control point.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Vector3f GetControlPoint(uint index);
    }

    public interface IAreaf
    {
        /// <summary>
        /// Obtains the area of shape.
        /// </summary>
        /// <remarks>If it is hard to calculate, it may return negative value.</remarks>
        float Area { get; }
    }

    /// <summary>
    /// The area of shape, if it exists. Shapes without are do not implement this interface.
    /// </summary>
    public interface IArea2f : IAreaf
    {
        /// <summary>
        /// Tesselates area to buffer.
        /// </summary>
        /// <param name="resolution">Resolution of tesselation (maximum error in world units),
        /// negative resolution means adaptive tesselation.</param>
        /// <param name="geometryOutputStream">The geometry output stream, with options of tesselation.</param>
        void Tesselate(float resolution, Storage.Builders.ITriangleBuilder2f builder);
    }

    /// <summary>
    /// The area of shape, if it exists. Shapes without are do not implement this interface.
    /// </summary>
    public interface IArea3f : IAreaf
    {
        /// <summary>
        /// Tesselates area to buffer.
        /// </summary>
        /// <param name="resolution">Resolution of tesselation (maximum error in world units),
        /// negative resolution means adaptive tesselation.</param>
        /// <param name="geometryOutputStream">The geometry output stream, with options of tesselation.</param>
        void Tesselate(float resolution, Storage.Builders.ITriangleBuilder3f builder);
    }

    /// <summary>
    /// Outline of a shape, or path if it has no area.
    /// </summary>
    /// <remarks>Outlines work with parameter t that is always in range [0,1]. Interpolation
    /// can be done at any basis</remarks>
    public interface IOutlinef
    {
        /// <summary>
        /// The length of outline, same as GetSectionLength(0, 1).
        /// </summary>
        float OutlineLength { get; }
    }

    /// <summary>
    /// A 2D outline.
    /// </summary>
    public interface IOutline2f : IOutlinef
    {
        /// <summary>
        /// Samples outline at t in range od [0,1].
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Vector2f Sample(float t);

        /// <summary>
        /// Samples itself to vertex buffer given a resolution.
        /// </summary>
        /// <param name="resolution">The resolution of sampling (maximum error in world units),
        /// negative resolution means adaptive tesselation.</param>
        /// <param name="outlineOutputStream">Buffer with all available contraints.</param>
        void Sample(float resolution, Storage.Builders.ILineBuilder2f lineStripBuilder);
    }

    /// <summary>
    /// A 3D outline.
    /// </summary>
    public interface IOutline3f : IOutlinef
    {
        /// <summary>
        /// Samples outline at t in range od [0,1].
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Vector3f Sample(float t);

        /// <summary>
        /// Samples itself to vertex buffer given a resolution.
        /// </summary>
        /// <param name="resolution">The resolution of sampling (maximum error in world units),
        /// negative resolution means adaptive tesselation.</param>
        /// <param name="outlineOutputStream">Buffer with all available contraints.</param>
        void Sample(float resolution, Storage.Builders.ILineBuilder3f outlineOutputStream);
    }

    /// <summary>
    /// Weighted control points interface.
    /// </summary>
    public interface IWeightedControlPointsf : IControlPointsf
    {
        /// <summary>
        /// Sets or gets all weights, amutomatically renormalizes if they do not sum up.
        /// </summary>
        float[] Weights { set; get; } 
    }


}
