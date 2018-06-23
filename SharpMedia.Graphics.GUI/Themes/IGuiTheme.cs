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

namespace SharpMedia.Graphics.GUI.Themes
{
    /// <summary>
    /// A GUI theme interface
    /// </summary>
    public interface IGuiTheme
    {
        /// <summary>
        /// Obtains the theme style for element.
        /// </summary>
        /// <param name="objectType">The type of widget/object.</param>
        /// <param name="specifier">The additional specifier, usually null.</param>
        /// <returns></returns>
        Style ObtainStyle([NotNull] Type objectType, object specifier);

        /// <summary>
        /// Obtains default renderer for this theme.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        IGuiRenderer ObtainRenderer([NotNull] Type objectType, object additionalData);

        /// <summary>
        /// Applies default widget and renderer to widget.
        /// </summary>
        /// <param name="obj"></param>
        void AutomaticApply(object obj, bool transverse);

        /// <summary>
        /// Removes all theme styles.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="transverse"></param>
        void AutomaticUnApply(object obj, bool transverse);
    }
}
