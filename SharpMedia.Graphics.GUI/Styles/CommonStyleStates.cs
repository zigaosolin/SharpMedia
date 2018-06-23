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

namespace SharpMedia.Graphics.GUI.Styles
{
    /// <summary>
    /// Common style states.
    /// </summary>
    public static class CommonStyleStates
    {
        #region Private Members
        

        static CommonStyleStates()
        {
            Normal = new StyleState("Normal", 
                "The widget is in normal state", null);
            PointerOver = new StyleState("PointerOver", 
                "Pointer is over the widget", Normal);
            Invalid = new StyleState("Invalid", 
                "Widget is in invalid state, usually due to failed validation", Normal);
            FocusedInvalid = new StyleState("FocusedInvalid", 
                "Widget is in focused and invalid state", Invalid);
            PointerOverInvalid = new StyleState("PointerOverInvalid", 
                "Widget has pointer over and is in invalid state", FocusedInvalid);
            Focused = new StyleState("Focused", 
                "Widget is focused", PointerOver);
            Clicked = new StyleState("Clicked",
                "Widget is clicked", PointerOver);
            All = new StyleState("All",
                "Indicates all states, only used by StyleAnimationController", null);
            Null = new StyleState("Null",
                "Indicates no state", null);

        }
        #endregion

        #region Static Members

        /// <summary>
        /// Normal state.
        /// </summary>
        public static StyleState Normal;

        /// <summary>
        /// Pointer over state.
        /// </summary>
        public static StyleState PointerOver;

        /// <summary>
        /// Invalid, validation failed state.
        /// </summary>
        public static StyleState Invalid;

        /// <summary>
        /// Focused invalid state.
        /// </summary>
        public static StyleState FocusedInvalid;

        /// <summary>
        /// Pointer over invalid state.
        /// </summary>
        public static StyleState PointerOverInvalid;

        /// <summary>
        /// Focused state.
        /// </summary>
        public static StyleState Focused;

        /// <summary>
        /// Clicked state.
        /// </summary>
        public static StyleState Clicked;

        /// <summary>
        /// Used by StyleAnimationController to specify all style state.
        /// </summary>
        public static StyleState All;

        /// <summary>
        /// Used by StyleAnimationController to specify null style state.
        /// </summary>
        public static StyleState Null;

        #endregion

    }
}
