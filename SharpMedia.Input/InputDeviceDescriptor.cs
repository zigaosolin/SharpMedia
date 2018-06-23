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

namespace SharpMedia.Input
{

    /// <summary>
    /// Input device descriptor.
    /// </summary>
    public sealed class InputDeviceDescriptor
    {
        /// <summary>
        /// Device type.
        /// </summary>
        public readonly InputDeviceType DeviceType;

        /// <summary>
        /// Name of device.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Device ID within the type, 0 means primary.
        /// </summary>
        public readonly uint DeviceId;

        /// <summary>
        /// Number of buttons.
        /// </summary>
        public readonly uint ButtonCount;

        /// <summary>
        /// Number of axes.
        /// </summary>
        public readonly uint AxisCount;

        /// <summary>
        /// Constructor.
        /// </summary>
        public InputDeviceDescriptor(InputDeviceType type, string name,
            uint deviceId, uint buttonCount, uint axisCount)
        {
            this.DeviceType = type;
            this.Name = name;
            this.DeviceId = deviceId;
            this.ButtonCount = buttonCount;
            this.AxisCount = axisCount;

        }

    }
}
