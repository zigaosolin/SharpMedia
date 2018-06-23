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
using SharpMedia.Input;

namespace SharpMedia.Graphics.GUI
{



    /// <summary>
    /// A GUI object that may be interacted with by the user
    /// </summary>
    public interface IUserInteractive
    {
        /// <summary>
        /// Fired on pointer entered event.
        /// </summary>
        void OnPointerEnter  (IPointer cursor);

        /// <summary>
        /// Fired on pointer leave event.
        /// </summary>
        void OnPointerLeave(IPointer cursor);

        /// <summary>
        /// A button was pressed.
        /// </summary>
        /// <remarks>May fire button pressed and not button released if pointer leaves.</remarks>
        void OnPointerPress  (IPointer cursor, uint button, InputEventModifier modifiers);

        /// <summary>
        /// A button was released.
        /// </summary>
        /// <remarks>May fire button pressed and not button released if pointer leaves.</remarks>
        void OnPointerRelease(IPointer cursor, uint button);

        /// <summary>
        /// On wheel event was fired.
        /// </summary>
        /// <param name="cursor">The wheel.</param>
        void OnWheel(IPointer cursor, float deltaWheel);

        /// <summary>
        /// On Mouse move event.
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="mouseDeltaMove"></param>
        void OnPointerMove(IPointer cursor, Vector2f mouseDeltaMove);

        /// <summary>
        /// A button was pressed.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="translatedCharacter"></param>
        void OnKeyPress(IPointer cursor, KeyCodes code, 
            KeyboardModifiers modifiers, InputEventModifier eventModifiers);

        /// <summary>
        /// A button was released.
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="translatedCharacter"></param>
        /// <param name="modifiers"></param>
        /// <remarks>Button is not considered released, if focus is lost.</remarks>
        void OnKeyRelease(IPointer cursor, KeyCodes code, KeyboardModifiers modifiers);
    }
}
