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

namespace SharpMedia.Input.Mappings
{

    /// <summary>
    /// A simple button click/release trigger.
    /// </summary>
    [Serializable]
    public class ButtonTrigger : IActionTrigger
    {
        #region Private Members
        IActionTriggerable action;
        InputDeviceType deviceType;
        uint deviceId;
        uint buttonId;
        bool whenButtonPressed;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for mouse buttons.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="onPress"></param>
        public ButtonTrigger(uint button, bool onPress)
        {
            this.deviceId = 0;
            this.deviceType = InputDeviceType.Mouse;
            this.buttonId = button;
            this.whenButtonPressed = onPress;
        }

        /// <summary>
        /// Constructor for keyboard buttons.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="onPress"></param>
        public ButtonTrigger(KeyCodes code, bool onPress)
        {
            this.deviceId = 0;
            this.deviceType = InputDeviceType.Keyboard;
            this.buttonId = (uint)code;
            this.whenButtonPressed = onPress;
        }

        /// <summary>
        /// Generic button trigger.
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="deviceId"></param>
        /// <param name="button"></param>
        /// <param name="onPress"></param>
        public ButtonTrigger(InputDeviceType deviceType, uint deviceId,
            uint button, bool onPress)
        {
            this.deviceType = deviceType;
            this.deviceId = deviceId;
            this.buttonId = button;
            this.whenButtonPressed = onPress;
        }

        #endregion

        #region Processing

        void SpecialEvent(InputEvent ev)
        {
            // Button filtering.
            if (ev.ButtonId != buttonId) return;

            TriggerAction();
        }

        void GenericEvent(InputEvent ev)
        {
            // State filtering.
            if (ev.ButtonState != whenButtonPressed) return;
            if (ev.ButtonId != buttonId) return;
            if (deviceType != ev.Spawner.Descriptor.DeviceType) return;
            if (deviceId != ev.Spawner.Descriptor.DeviceId) return;

            TriggerAction();
        }

        void TriggerAction()
        {
            if (action != null)
            {
                action.Trigger(this, whenButtonPressed ? 1.0f : 0.0f);
            }
        }

        #endregion
    
        #region IActionTrigger Members

        void IActionTrigger.Initialize(EventProcessor processor, bool bind)
        {
 	        if(bind)
            {
                // We have special mouse event (faster)
                if(deviceId == 0 && deviceType == InputDeviceType.Mouse)
                {
                    if(whenButtonPressed)
                    {
                        processor.MouseButtonDown += SpecialEvent;
                    } else {
                        processor.MouseButtonUp += SpecialEvent;
                    }
                // We have special keyboard (faster)
                } else if(deviceId == 0 && deviceType == InputDeviceType.Keyboard)
                {
                    if (whenButtonPressed)
                    {
                        processor.KeyDown += SpecialEvent;
                    }
                    else
                    {
                        processor.KeyUp += SpecialEvent;
                    }
                } else {
                    if (whenButtonPressed)
                    {
                        processor.CustomEvent += GenericEvent;
                    }
                    else
                    {
                        processor.CustomEvent += GenericEvent;
                    }
                }
            } else {
                // We have special mouse event (faster)
                if (deviceId == 0 && deviceType == InputDeviceType.Mouse)
                {
                    if (whenButtonPressed)
                    {
                        processor.MouseButtonDown -= SpecialEvent;
                    }
                    else
                    {
                        processor.MouseButtonUp -= SpecialEvent;
                    }
                    // We have special keyboard (faster)
                }
                else if (deviceId == 0 && deviceType == InputDeviceType.Keyboard)
                {
                    if (whenButtonPressed)
                    {
                        processor.KeyDown -= SpecialEvent;
                    }
                    else
                    {
                        processor.KeyUp -= SpecialEvent;
                    }
                }
                else
                {
                    if (whenButtonPressed)
                    {
                        processor.CustomEvent -= GenericEvent;
                    }
                    else
                    {
                        processor.CustomEvent -= GenericEvent;
                    }
                }
            }
        }

        void IActionTrigger.BindTo(IActionTriggerable action)
        {
 	        this.action = action;
        }

        #endregion
}

}
