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

namespace SharpMedia.Graphics.GUI.Widgets.Containers
{
    /// <summary>
    /// An input rounter helper.
    /// </summary>
    internal static class InputRouting
    {


        /// <summary>
        /// Returns the child with focus.
        /// </summary>
        /// <param name="children"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static IWidget GetChildWithFocus(IContainer container, GuiManager manager)
        {
            // Obtains focused object.
            IWidget focused = manager.FocusedWidget;

            // We transverse.
            while (focused != null && focused.Parent != container)
            {
                focused = focused.Parent;
            }

            return focused;
        }

        /// <summary>
        /// Returns first matching child at position.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="manager"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static IWidget GetChildAtPosition(IContainer container, GuiManager manager, Vector2f position)
        {
            ReadOnlyCollection<object> children = container.Children;
            for (int i = 0; i < children.Count; i++)
            {
                if (!(children[i] is IWidget)) continue;

                IWidget positionable = children[i] as IWidget;

                // We check if it is inside it.
                Vector2f leftBottom, rightTop;
                positionable.GetBoundingBox(out leftBottom, out rightTop);
 
                // If it is inside, we return it.
                if (position.X >= leftBottom.X && position.X <= rightTop.X &&
                   position.Y >= leftBottom.Y && position.Y <= rightTop.Y)
                {
                    return positionable;
                }
            }
            return null;
        }
    }
}
