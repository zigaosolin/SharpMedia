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

namespace SharpMedia.Graphics.GUI.Widgets.Helpers
{

    /// <summary>
    /// A drag controller is responsible for dragging events.
    /// </summary>
    public class DragController
    {
        #region Private Members
        Vector2f startDrag;
        Vector2f endDrag;
        bool isDragging = false;
        #endregion

        #region Public Methods

        /// <summary>
        /// Starts a drag.
        /// </summary>
        /// <param name="pointer"></param>
        public virtual void Start(IPointer pointer)
        {
            startDrag = pointer.CanvasPosition;
            endDrag = pointer.CanvasPosition;
            isDragging = true;
        }

        /// <summary>
        /// Ends a drag.
        /// </summary>
        /// <param name="pointer"></param>
        public virtual void End(IPointer pointer)
        {
            endDrag = pointer.CanvasPosition;
            isDragging = false;
        }

        /// <summary>
        /// Updates a drag.
        /// </summary>
        /// <param name="pointer"></param>
        public virtual void Update(IPointer pointer)
        {
            endDrag = pointer.CanvasPosition;
        }

        /// <summary>
        /// Obtains a drag rectangle, either current or last.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void GetRect(out Vector2f min, out Vector2f max)
        {
            min = new Vector2f(
                    startDrag.X < endDrag.X ? startDrag.X : endDrag.X,
                    startDrag.Y < endDrag.Y ? startDrag.Y : endDrag.Y);
            max = new Vector2f(
                    startDrag.X > endDrag.X ? startDrag.X : endDrag.X,
                    startDrag.Y > endDrag.Y ? startDrag.Y : endDrag.Y);
        }

        /// <summary>
        /// Is it dragging.
        /// </summary>
        public bool IsDragging
        {
            get
            {
                return isDragging;
            }
        }

        #endregion

    }

}
