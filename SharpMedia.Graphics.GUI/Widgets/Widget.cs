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

namespace SharpMedia.Graphics.GUI.Widgets
{

    /// <summary>
    /// One or more or-ed options.
    /// </summary>
    [Flags]
    public enum WidgetSerialization
    {
        /// <summary>
        /// Properties are serialized (non-state).
        /// </summary>
        Basic = 0,

        /// <summary>
        /// Default persists non-theme styles.
        /// </summary>
        Default = PersistNonThemeStyles,

        /// <summary>
        /// State is serialized, such as child's position.
        /// </summary>
        PersistState = 1,

        /// <summary>
        /// Event bindings are serialized, along with all links. Not recommended.
        /// </summary>
        PersistEventBindings = 8,

        /// <summary>
        /// Non-theme (overwritten) styles are serialized.
        /// </summary>
        PersistNonThemeStyles = 16,

        /// <summary>
        /// All styles are persisted.
        /// </summary>
        PersistAllStyles = 32 | PersistNonThemeStyles,

        /// <summary>
        /// Is renderer persisted.
        /// </summary>
        PersistRenderer = 64,

        /// <summary>
        /// Persists animations.
        /// </summary>
        PersistAnimations = 128
    }

    /// <summary>
    /// A widget object.
    /// </summary>
    /// <remarks>Each widget must implement CreateChangeContext().</remarks>
    public interface IWidget : IDisplayObject, ILayoutNegotiation, IUserInteractive, IAnimatable
    {
        /// <summary>
        /// Enters change context.
        /// </summary>
        /// <returns></returns>
        void EnterChange();

        /// <summary>
        /// Exits change context.
        /// </summary>
        void ExitChange();

        /// <summary>
        /// A parent widget.
        /// </summary>
        IWidget Parent { get; }

        /// <summary>
        /// Is the widget visible.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Widget's serialization options.
        /// </summary>
        WidgetSerialization SerializationOptions { get; set; }

        /// <summary>
        /// Sets parent, callable only internally. Should implement this method explicitally.
        /// </summary>
        /// <remarks>Container is responsible for setting the parent once applied to it.</remarks>
        /// <param name="widget"></param>
        [Linkable(LinkMask.SecurityLevel2)]
        void SetParent(IWidget widget, GuiManager manager);

        /// <summary>
        /// Focus was lost.
        /// </summary>
        [Linkable(LinkMask.SecurityLevel2)]
        void FocusChange(bool gained);
    }
}
