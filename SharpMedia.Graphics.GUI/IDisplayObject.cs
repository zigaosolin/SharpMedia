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
using SharpMedia.Graphics.Vector;
using SharpMedia.Math.Shapes;
using SharpMedia.Math;
using SharpMedia.AspectOriented;
using SharpMedia.Graphics.GUI.Styles;
using System.Collections.ObjectModel;

namespace SharpMedia.Graphics.GUI
{


    /// <summary>
    /// An object that can be added to a display list and rendered
    /// </summary>
    public interface IDisplayObject : IPositionable, IPreChangeNotifier
    {
        /// <summary>
        /// Skin (renderer) to use to render this display object
        /// </summary>
        Themes.IGuiRenderer Skin { get; set; }

        /// <summary>
        /// Style (renderer information) to use to render this display object
        /// </summary>
        /// <remarks>Style is immutable if object is not in change context (exception is thrown).</remarks>
        Style Style { get; set; }

        /// <summary>
        /// The state of display object.
        /// </summary>
        /// <remarks>State IS mutable.</remarks>
        StyleAnimationController StyleAnimation { get; }

        /// <summary>
        /// Z order of display object, usually 0.
        /// </summary>
        uint ZOrder { get; }

        /// <summary>
        /// Sub display objects.
        /// </summary>
        IDisplayObject[] SubDisplayObjects { get; }
    }
}
