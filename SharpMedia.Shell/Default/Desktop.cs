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
using SharpMedia.Graphics.GUI.Widgets.Containers;

namespace SharpMedia.Shell.Default
{
    /// <summary>
    /// A desktop, implemented as sheet.
    /// </summary>
    [Serializable]
    internal sealed class DefaultDesktop : Container, IDesktop
    {
        #region Private Members
        string name;
        IDesktop parentDesktop;
        List<DesktopIcon> icons = new List<DesktopIcon>();
        #endregion

        #region Constructors

        public DefaultDesktop([NotNull] string name, IDesktop parent)
        {
            this.parentDesktop = parent;
            this.name = name;
        }

        #endregion

        #region IDesktop Members

        public new IDesktop Parent
        {
            get { return parentDesktop; }
        }

        public DesktopIcon[] Icons
        {
            get { lock (syncRoot) { return icons.ToArray(); } }
        }

        public void AddIcon(DesktopIcon icon)
        {
            lock (syncRoot)
            {
                icons.Add(icon);
            }
        }

        public void RemoveIcon(DesktopIcon icon)
        {
            lock (syncRoot)
            {
                icons.Remove(icon);
            }
        }

        public void RearrangeIcons(RearrangeCriteria criteria)
        {
            lock (syncRoot)
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        #endregion

    }
}
