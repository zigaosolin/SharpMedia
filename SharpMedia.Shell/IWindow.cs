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
using System.Collections.ObjectModel;

namespace SharpMedia.Shell
{

    /// <summary>
    /// State of window.
    /// </summary>
    public enum WindowState
    {
        Minimized,
        Maximized,
        Normal,
        Fullscreen
    }

    /// <summary>
    /// A window interface
    /// </summary>
    public interface IWindow : IDisposable
    {
        /// <summary>
        /// Ataches effect, returns false if could not attach (effect conflicts or not supported).
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        bool AttachEffect(IWindowEffect effect);

        /// <summary>
        /// Gets window effects.
        /// </summary>
        ReadOnlyCollection<IWindowEffect> WindowEffects { get; }

        /// <summary>
        /// Detaches effect from window.
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        bool DetachEffect(IWindowEffect effect);

        /// <summary>
        /// Changes the window source (render target). It is also allowed to change
        /// render target size here.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newSize">If newsize is null, new size is calculated from render target's size.
        /// Otherwise, you are free to set any size and rendering will automatically strech the rectangle.</param>
        void ChangeSource(Guid source, Vector2i? newSize);

        /// <summary>
        /// Sets Z order.
        /// </summary>
        /// <param name="reference">If reference is null, it represents "all" windows.</param>
        /// <param name="after">After of before the window.</param>
        void SetZOrder(IWindow reference, bool after);

        /// <summary>
        /// Minimizes window.
        /// </summary>
        void Minimize();

        /// <summary>
        /// Maximizes window.
        /// </summary>
        void Maximize();

        /// <summary>
        /// Is window visible.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// This is a way to notify that the progress has not frozen but
        /// is working and currently cannot render so fast.
        /// </summary>
        void Working();

        /// <summary>
        /// The render target was updated, must also update output.
        /// </summary>
        void Rendered();

        /// <summary>
        /// Window position.
        /// </summary>
        Vector2i Position { get; set; }

        /// <summary>
        /// Size of window.
        /// </summary>
        Vector2i Size { get; set; }

        /// <summary>
        /// The title of window.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Does the window have focus.
        /// </summary>
        bool HasFocus { get; set; }

        /// <summary>
        /// Options of window.
        /// </summary>
        WindowOptions Options { get; set; }

        /// <summary>
        /// Maximum size.
        /// </summary>
        Vector2i MaxSize { get; set; }

        /// <summary>
        /// Minimum size.
        /// </summary>
        Vector2i MinSize { get; set; }

        /// <summary>
        /// The state of the window.
        /// </summary>
        /// <remarks>Minimization and maximization can also be set here.</remarks>
        WindowState State { get; set; }

        /// <summary>
        /// A disposing event.
        /// </summary>
        event Action<IWindow> Disposing;

    }
}
