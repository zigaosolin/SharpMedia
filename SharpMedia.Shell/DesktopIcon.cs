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

namespace SharpMedia.Shell
{
    /// <summary>
    /// A desktop icon.
    /// </summary>
    public sealed class DesktopIcon
    {
        #region Private Member
        Vector2f position;
        string nodePath;
        string iconPath;
        #endregion

        #region Properties

        /// <summary>
        /// Position of desktop icon.
        /// </summary>
        public Vector2f Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        /// <summary>
        /// Icon path, may be null if node links to icon (e.g. is application).
        /// </summary>
        public string IconPath
        {
            get
            {
                return iconPath;
            }
            set
            {
                iconPath = value;
            }
        }

        /// <summary>
        /// Node path, must be valid of desktop icon is invalid.
        /// </summary>
        public string NodePath
        {
            get
            {
                return nodePath;
            }
            set
            {
                nodePath = value;
            }
        }

        #endregion
    }
}
