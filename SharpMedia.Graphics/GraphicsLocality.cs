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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics
{
    /// <summary>
    /// The graphics locality enumerator.
    /// </summary>
    public enum GraphicsLocality
    {
        /// <summary>
        /// Resource is always held in system memory only.
        /// </summary>
        SystemMemoryOnly,

        /// <summary>
        /// Resource is always held in device memory.
        /// </summary>
        /// <remarks>
        /// Such resource cannot be prepared for reseting and must be
        /// manually reset.
        /// </remarks>
        DeviceMemoryOnly,

        /// <summary>
        /// Data is held in system memory and also in device memory, when
        /// driver part is available.
        /// </summary>
        DeviceAndSystemMemory,

        /// <summary>
        /// Data is held either in system memory (if not bound to device, e.g. not prepared)
        /// or in device memory only.
        /// </summary>
        DeviceOrSystemMemory
    }

    /// <summary>
    /// A locality of resource interface.
    /// </summary>
    public interface IGraphicsLocality
    {
        /// <summary>
        /// The locality of resource.
        /// </summary>
        GraphicsLocality Locality
        {
            get;
            [param: NotNull]
            set;
        }

        /// <summary>
        /// Prepares the resource for device, e.g. makes device part available
        /// (this call is throws an exception for SystemMemoryOnly resources).
        /// </summary>
        /// <param name="device">Device object.</param>
        void BindToDevice([NotNull] GraphicsDevice device);

        /// <summary>
        /// Makes sure that driver part does not exist. This is invalid for
        /// DeviceMemoryOnly resources and throws an exception.
        /// </summary>
        void UnBindFromDevice();

        /// <summary>
        /// Is the resource bound to device.
        /// </summary>
        bool IsBoundToDevice { get; }

    }
}
