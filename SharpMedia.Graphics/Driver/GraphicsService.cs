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

namespace SharpMedia.Graphics.Driver
{
    /// <summary>
    /// A graphics service, entry class.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IGraphicsService : IDisposable
    {
        /// <summary>
        /// Is the device active.
        /// </summary>
        bool IsDeviceActive
        {
            get;
        }

        /// <summary>
        /// Creates a new device, including swap chain and everything.
        /// </summary>
        IDevice Create(bool shared, RenderTargetParameters parameters,
                        out ISwapChain chain, out IWindowBackend window,
            bool debug);

        /// <summary>
        /// Obtains a device.
        /// </summary>
        /// <returns>The device.</returns>
        IDevice Obtain();

    }
}
