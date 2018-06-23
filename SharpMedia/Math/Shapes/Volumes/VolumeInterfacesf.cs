// This file was generated by TemplateEngine from template source 'VolumeInterfaces'
// using template 'VolumeInterfacesf. Do not modify this file directly, modify it from template source.

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

namespace SharpMedia.Math.Shapes.Volumes
{
    /// <summary>
    /// A shape that is Axis-aligned box boundable.
    /// </summary>
    public interface IAABoxBoundablef
    {
        /// <summary>
        /// The smallest axis aligned box that encloses geometry.
        /// </summary>
        AABoxf BoundingAABox { get; }
    }

    /// <summary>
    /// A shape that is sphere boundable
    /// </summary>
    public interface ISphereBoundablef
    {
        /// <summary>
        /// The smallest bounding sphere that encloses geometry.
        /// </summary>
        Spheref BoundingSphere { get; }
    }

    /// <summary>
    /// A volume measurable interface.
    /// </summary>
    public interface IVolumef : IContainsPoint3f
    {
        /// <summary>
        /// The volume of volumeric object.
        /// </summary>
        /// <remarks>Can be approximation or in special cases, unknown.</remarks>
        float Volume { get; }

    }

}
