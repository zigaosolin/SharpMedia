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
using SharpMedia.Graphics.GUI.Widgets.Containers;
using System.Threading;

namespace SharpMedia.Graphics.GUI.Widgets.Specialized
{


    /// <summary>
    /// A window skeleton.
    /// </summary>
    public class WindowSkeleton : Container
    {
        #region Protected Members
        protected internal Button closeButton;
        protected internal Button maximizeButton;
        protected internal Button minimizeButton;
        protected internal Button fullscreenButton;
        protected internal Icon applicationIcon;
        protected internal string applicationTitle;
        #endregion

        #region Constructors

        public WindowSkeleton()
            : base(null)
        {

        }

        #endregion

        #region Events

        /// <summary>
        /// Window skeleton event holder.
        /// </summary>
        public class WindowSkeletonEvents : ContainerEvents
        {
            #region Protected Members
            internal protected WindowSkeletonEvents(WindowSkeleton s)
                : base(s)
            {
            }
            #endregion
        }

        #endregion

        #region Overrides

        public new WindowSkeletonEvents Events
        {
            get { return eventHolder as WindowSkeletonEvents; }
        }

        #endregion

        #region Methods

        public Button CloseButton
        {
            get { return closeButton; }
            set { AssertMutable();  closeButton = value; }
        }

        public Button MinimizeButton
        {
            get { return minimizeButton; }
            set { AssertMutable(); minimizeButton = value; }
        }

        public Button MaximizeButton
        {
            get { return maximizeButton; }
            set { AssertMutable(); maximizeButton = value; }
        }

        public Button FullscreenButton
        {
            get { return fullscreenButton; }
            set { AssertMutable(); fullscreenButton = value; }
        }

        public Icon ApplicationIcon
        {
            get { return applicationIcon; }
            set { AssertMutable(); applicationIcon = value; }
        }

        public string ApplicationTitle
        {
            get { return applicationTitle; }
            set { AssertMutable(); applicationTitle = value; }
        }

        public override void AddChild(object child)
        {
            throw new InvalidOperationException("Cannot add child to window skeleton, all children are typed");
        }

        public override void RemoveChild(object child)
        {
            if (child == closeButton) closeButton = null;
            if (child == applicationIcon) applicationIcon = null;
            if (child == closeButton) closeButton = null;
            if (child == fullscreenButton) fullscreenButton = null;
        }

        #endregion

    }
}
