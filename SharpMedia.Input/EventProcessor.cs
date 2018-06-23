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

namespace SharpMedia.Input
{

    /// <summary>
    /// Action on specific event.
    /// </summary>
    /// <param name="?"></param>
    public delegate void EventAction(InputEvent @event);

    /// <summary>
    /// Event processor adds firing capabilities.
    /// </summary>
    public class EventProcessor 
    {
        #region Private Members
        EventPump pump;
        EventAction mouseAxis;
        EventAction mouseButtonDown;
        EventAction mouseButtonUp;
        EventAction keyDown;
        EventAction keyUp;
        EventAction allActions;
        EventAction nonProcessedActions;
        EventAction cursorMoved;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructs event processor.
        /// </summary>
        /// <param name="devices"></param>
        public EventProcessor(EventPump pump)
        {
            this.pump = pump;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// An event pump
        /// </summary>
        public EventPump EventPump
        {
            get
            {
                return pump;
            }
        }

        /// <summary>
        /// Processes the event.
        /// </summary>
        /// <returns></returns>
        public InputEvent Process()
        {
            InputEvent ev = pump.Get();
            if (ev == null)
            {
                return null;
            }

            EventAction action = null;
            switch (ev.Spawner.Descriptor.DeviceType)
            {
                case InputDeviceType.Mouse:
                    if (ev.EventType == InputEventType.Axis)
                    {
                        action = mouseAxis;
                        if (action != null)
                        {
                            action(ev);
                        }
                    }
                    else
                    {
                        action = ev.ButtonState ? mouseButtonDown : mouseButtonUp;
                        if (action != null)
                        {
                            action(ev);
                        }
                    }
                    break;
                case InputDeviceType.Keyboard:
                    action = ev.ButtonState ? keyDown : keyUp;
                    if (action != null)
                    {
                        action(ev);
                    }
                    break;
                case InputDeviceType.Cursor:
                    action = cursorMoved;
                    if (action != null)
                    {
                        action(ev);
                    }

                    break;
                default:
                    action = nonProcessedActions;
                    if (action != null)
                    {
                        action(ev);
                    }
                    break;
            }

            action = allActions;
            if (action != null)
            {
                action(ev);
            }

            return ev;
        }


        #endregion

        #region Events

        /// <summary>
        /// Subscribes event to mouse axis change.
        /// </summary>
        public event EventAction MouseAxis
        {
            add
            {
                mouseAxis += value;
            }
            remove
            {
                mouseAxis -= value;
            }
        }

        /// <summary>
        /// Subscribes event to mouse button change.
        /// </summary>
        public event EventAction MouseButtonDown
        {
            add
            {
                mouseButtonDown += value;
            }
            remove
            {
                mouseButtonDown -= value;
            }
        }

        /// <summary>
        /// Subscribes event to mouse button change.
        /// </summary>
        public event EventAction MouseButtonUp
        {
            add
            {
                mouseButtonUp += value;
            }
            remove
            {
                mouseButtonUp -= value;
            }
        }

        /// <summary>
        /// Subscribes event to key button change.
        /// </summary>
        public event EventAction KeyDown
        {
            add
            {
                keyDown += value;
            }
            remove
            {
                keyDown -= value;
            }
        }

        /// <summary>
        /// Subscribes event to key button change.
        /// </summary>
        public event EventAction KeyUp
        {
            add
            {
                keyUp += value;
            }
            remove
            {
                keyUp -= value;
            }
        }

        /// <summary>
        /// Subscribes event for all actions.
        /// </summary>
        public event EventAction AnyEvent
        {
            add
            {
                allActions += value;
            }
            remove
            {
                allActions -= value;
            }
        }

        /// <summary>
        /// Subscribes event to non-processed events.
        /// </summary>
        public event EventAction CustomEvent
        {
            add
            {
                nonProcessedActions += value;
            }
            remove
            {
                nonProcessedActions -= value;
            }
        }

        /// <summary>
        /// A cursor (OS supplied) moved event.
        /// </summary>
        public event EventAction CursorMoved
        {
            add
            {
                cursorMoved += value;
            }
            remove
            {
                cursorMoved -= value;
            }

        }


        // TODO: maybe "translated" events

        #endregion
    }
}
