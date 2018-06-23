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

namespace SharpMedia.Graphics.GUI.Validation
{
    /// <summary>
    /// A validation response, used by widgets.
    /// </summary>
    [Flags]
    public enum ValidationResponse
    {
        /// <summary>
        /// Disables validation.
        /// </summary>
        Disable = 0,

        /// <summary>
        /// Does not allow losing focus if invalid.
        /// </summary>
        NotAllowLoseFocus = 1,

        /// <summary>
        /// Same as NowAllowLoseFocus with beep, if GuiManager has sound context.
        /// </summary>
        NowAllowLoseFocusWithBeep = NotAllowLoseFocus | 2,

        /// <summary>
        /// Marks data in different colour or somehow else.
        /// </summary>
        Mark = 4,

        /// <summary>
        /// Shows validation status somewhere (at side).
        /// </summary>
        Status = 8
    }

    /// <summary>
    /// When is validation executed.
    /// </summary>
    [Flags]
    public enum ValidationExecution
    {
        /// <summary>
        /// Only manual validation.
        /// </summary>
        Never,

        /// <summary>
        /// When data is changed (entered).
        /// </summary>
        ReceiveData = 1,

        /// <summary>
        /// When data is set manually.
        /// </summary>
        SetData = 2,

        /// <summary>
        /// When focus is lost.
        /// </summary>
        LostFocus = 4
    }
}
