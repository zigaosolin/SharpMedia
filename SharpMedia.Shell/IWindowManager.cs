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

namespace SharpMedia.Shell
{
    /// <summary>
    /// Window options.
    /// </summary>
    [Flags]
    public enum WindowOptions
    {
        /// <summary>
        /// Has minimize button
        /// </summary>
        Minimize = 1,

        /// <summary>
        /// Has maximize button.
        /// </summary>
        Maximimize = 2,

        /// <summary>
        /// Has close button
        /// </summary>
        Close = 4,

        /// <summary>
        /// It is resizable.
        /// </summary>
        Resizable = 8,

        /// <summary>
        /// It is fullscreenable.
        /// </summary>
        Fullscreenable = 16
    }

    /// <summary>
    /// Window manager is a service, capable of serving windows.
    /// </summary>
    public interface IWindowManager
    {
        /// <summary>
        /// This size of desktop, in pixels.
        /// </summary>
        Vector2i DesktopSize { get; }

        /// <summary>
        /// Does manager support effect.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool SupportEffect(Type type);

        /// <summary>
        /// Creates a new window.
        /// </summary>
        /// <param name="resourceId">The id of already registered shared 2d 
        /// texture that will be used for drawing.</param>
        /// <param name="size">Size does not necessarily match resource's size.</param>
        IWindow CreateWindow(Guid resourceId, string title, string windowGroup, IWindowBackend listener,
                             WindowOptions options, Vector2i position, Vector2i size,
                             Vector2i? minSize, Vector2i? maxSize, WindowState windowState,
                             IWindow parentWindow, bool blockInputToParent);

        /// <summary>
        /// Creates a new desktop.
        /// </summary>
        IDesktop CreateDesktop(IDesktop parent, string name);

        /// <summary>
        /// Sets current desktop.
        /// </summary>
        /// <remarks>Permission may not be granted.</remarks>
        void SetCurrentDesktop(IDesktop desktop);

        /// <summary>
        /// Event triggered after rendering. Manager still locks.
        /// </summary>
        event Action<IWindowManager> PreRendering;

        /// <summary>
        /// Event triggered post rendering. Manager still locks.
        /// </summary>
        event Action<IWindowManager> PostRendering;

        /// <summary>
        /// Event triggered on disposing.
        /// </summary>
        event Action<IWindowManager> Disposing;
    }
}
