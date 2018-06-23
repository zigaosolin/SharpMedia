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
using SharpMedia.Math;
using SharpMedia.Graphics.GUI;

namespace SharpMedia.Shell
{

    /// <summary>
    /// Serves to send back events to listener.
    /// </summary>
    public interface IWindowBackend : IUserInteractive
    {
        /// <summary>
        /// Window was resized, you usually recreate render target here.
        /// </summary>
        /// <param name="newSize"></param>
        void Resized(Vector2i newSize);

        /// <summary>
        /// Closed was clicked.
        /// </summary>
        void Closed();

        /// <summary>
        /// Window was minimized.
        /// </summary>
        void Minimized();

        /// <summary>
        /// Window was maximized.
        /// </summary>
        void Maximized();

        /// <summary>
        /// Window was fullscreened.
        /// </summary>
        void Fullscreened();

        /// <summary>
        /// Window gain focus.
        /// </summary>
        void GainFocus();

        /// <summary>
        /// Window lost focus.
        /// </summary>
        void LostFocus();

    }
}
