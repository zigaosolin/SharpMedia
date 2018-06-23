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
using SharpMedia.Graphics.GUI;

namespace SharpMedia.Shell
{
    /// <summary>
    /// Icon rearange crtiteria.
    /// </summary>
    public enum RearrangeCriteria
    {
        Name,
        Relavance,
        Size
    }

    /// <summary>
    /// A desktop, must be serializable.
    /// </summary>
    public interface IDesktop : IPreChangeNotifier
    {
        /// <summary>
        /// Name of desktop.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Parent desktop.
        /// </summary>
        IDesktop Parent { get; }

        /// <summary>
        /// List of desktop icons, belonging to this desktop.
        /// </summary>
        DesktopIcon[] Icons { get; }

        /// <summary>
        /// Adds an icon.
        /// </summary>
        /// <param name="icon"></param>
        void AddIcon([NotNull] DesktopIcon icon);

        /// <summary>
        /// Removes the icon.
        /// </summary>
        void RemoveIcon([NotNull] DesktopIcon icon);

        /// <summary>
        /// Rearange icons.
        /// </summary>
        void RearrangeIcons(RearrangeCriteria criteria);



    }
}
