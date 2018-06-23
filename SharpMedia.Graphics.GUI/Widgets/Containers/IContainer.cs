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
using System.Collections.ObjectModel;
using SharpMedia.Graphics.GUI.Metrics;

namespace SharpMedia.Graphics.GUI.Widgets.Containers
{

    /// <summary>
    /// The container mode.
    /// </summary>
    [Flags]
    public enum ContainerMode
    {
        None = 0,

        /// <summary>
        /// Vertical slider is allowed.
        /// </summary>
        /// <remarks>Only done by scrollable container.</remarks>
        VSlider = 1,

        /// <summary>
        /// Horizontal slider is allowed.
        /// </summary>
        /// <remarks>Only done by scollable container.</remarks>
        HSlider = 2,

        /// <summary>
        /// Allows the container to be on top of other widget; this is
        /// usually used by drop down lists.
        /// </summary>
        /// <remarks>We do it by not extending container's bounding box but
        /// still drawing out of it.</remarks>
        AllowOnTop = 4

    }

    /// <summary>
    /// A container object, contains objects.
    /// </summary>
    public interface IContainer : IWidget
    {
        /// <summary>
        /// Children of this container
        /// </summary>
        ReadOnlyCollection<object> Children { get; }

        /// <summary>
        /// The container mode.
        /// </summary>
        ContainerMode Mode { get; }

        /// <summary>
        /// Focused child.
        /// </summary>
        IWidget Focused { get; set; }
    }
}