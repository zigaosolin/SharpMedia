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
using SharpMedia.Graphics.GUI.Widgets;
using SharpMedia.Graphics;

namespace SharpMedia.Shell.Default
{

    /// <summary>
    /// A desktop icon held by desktop.
    /// </summary>
    [Serializable]
    internal class DefaultDesktopIcon : Icon, IDisposable
    {
        #region Private Members
        DesktopIcon icon;
        Image iconImage;
        #endregion

        #region Public Members

        /// <summary>
        /// A default desktop icon.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="iconImage"></param>
        public DefaultDesktopIcon(DesktopIcon icon, Image iconImage)
        {

        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            lock (syncRoot)
            {
                if (iconImage == null) return;

                iconImage.Dispose();
                iconImage = null;
                icon = null;
            }
        }

        #endregion
    }
}
