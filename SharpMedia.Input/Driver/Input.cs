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

namespace SharpMedia.Input.Driver
{

    /// <summary>
    /// The input driver interface.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IInputService : IDisposable
    {
        /// <summary>
        /// Initializes the driver.
        /// </summary>
        /// <param name="window"></param>
        void Initialize(SharpMedia.Graphics.Window window);

        /// <summary>
        /// Name of driver.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The supported devices enumerator.
        /// </summary>
        InputDeviceDescriptor[] SupportedDevices { get; }

        /// <summary>
        /// Gets or creates input device.
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        IInputDevice Create(InputDeviceDescriptor desc);
    }
}
