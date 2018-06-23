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
using SharpMedia.Caching;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.States
{

    /// <summary>
    /// The state interface. Each state is capable of applying itself and recording
    /// itself from device.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Prepares state for possible quicker state changes.
        /// </summary>
        /// <param name="device">The device.</param>
        void Prepare([NotNull] GraphicsDevice device);

        /// <summary>
        /// Is the state interned; that is, shared. Once interned, in cannot be uninterned.
        /// </summary>
        /// <remarks>Use internig through StateManager because it provides duplicate
        /// state elimination.</remarks>
        bool IsInterned { get; }
    }

}
