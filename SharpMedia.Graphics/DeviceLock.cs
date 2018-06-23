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

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A Device lock object, usable when trying to lock device more safely. Using
    /// statement can be used.
    /// </summary>
    public class DeviceLock : IDisposable
    {
        #region Private Members
        GraphicsDevice device;
        bool isDisposed = false;
        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceLock"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal DeviceLock(GraphicsDevice device)
        {
            this.device = device;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (!isDisposed)
            {
                device.Exit();
                isDisposed = true;
            }
        }

        #endregion
    }
}
