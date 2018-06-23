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

namespace SharpMedia.Graphics.GUI
{



    /// <summary>
    /// Attached data to pointer.
    /// </summary>
    public sealed class AttachedData
    {
        /// <summary>
        /// The application id from where it came from, for security purpuses.
        /// </summary>
        public Guid ApplicationID;

        /// <summary>
        /// Representation of attached data, can be null.
        /// </summary>
        public object Representation;

        /// <summary>
        /// Data, usually in string or byte[] serialized stream form.
        /// </summary>
        public object Data;

    }

    /// <summary>
    /// Common cursor states enumerator, can be set to Cursors property.
    /// </summary>
    public sealed class Cursors
    {
        public const string Normal = "Normal";
        public const string Busy = "Busy";
        public const string Hand = "Hand";
        public const string OverText = "OverText";
    }

    /// <summary>
    /// A pointer interface.
    /// </summary>
    /// <remarks>Pointer is implemented in GUI specific enviorment. In GUI standalone mode, it
    /// is implemented by GUI (and implements IDisplayObject), otherwise by Windows system.</remarks>
    public interface IPointer
    {
        /// <summary>
        /// Gets or sets absolute cursor position.
        /// </summary>
        Vector2f CanvasPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets relative coordinate.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Vector2f GetRelative(IDisplayObject obj);

        /// <summary>
        /// Checks if button is down.
        /// </summary>
        /// <param name="id">The button id</param>
        /// <remarks>If button does not exist, false is returned.</remarks>
        bool IsButtonDown(uint id);

        /// <summary>
        /// Obtains wheel position. If it does not exist, 0 is returned.
        /// </summary>
        float WheelPosition { get; }

        /// <summary>
        /// Attaches data to pointer.
        /// </summary>
        /// <param name="data"></param>
        void AttachData(AttachedData data);

        /// <summary>
        /// Detaches data from pointer.
        /// </summary>
        /// <param name="data"></param>
        void DetachData(AttachedData data);

        /// <summary>
        /// Detaches all accessible data from pointer.
        /// </summary>
        void DetachAllData();

        /// <summary>
        /// A cursor used for pointer. It can be one of "built-in" strings or any
        /// other object that is allowed by Shell/Gui system (usually IDisplayObject).
        /// </summary>
        object Cursor { get; set; }

        /// <summary>
        /// Gets all accessible data.
        /// </summary>
        ReadOnlyCollection<AttachedData> AttachedData { get; }

 
    }
}
