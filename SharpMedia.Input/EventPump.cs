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
using System.Threading;

namespace SharpMedia.Input
{
    /// <summary>
    /// The event type.
    /// </summary>
    public enum InputEventType
    {
        Button,
        Axis
    }

    /// <summary>
    /// Input event.
    /// </summary>
    public sealed class InputEvent
    {
        #region Private Members
        InputDevice owner;
        DateTime timeIdentification = DateTime.Now;
        InputEventType type;
        uint id;
        long delta;
        long stateRecord;
        InputEventModifier inputModifiers = InputEventModifier.None;
        KeyboardModifiers keyboardModifiers = KeyboardModifiers.None;
        #endregion

        #region Constructors

        /// <summary>
        /// Event for buttons.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="buttonId"></param>
        /// <param name="state"></param>
        public InputEvent(InputDevice owner, uint buttonId, bool state, 
            InputEventModifier modifiers, KeyboardModifiers keyModifiers)
        {
            this.owner = owner;
            this.type = InputEventType.Button;
            this.stateRecord = state ? 1 : 0;
            this.id = buttonId;
            this.inputModifiers = modifiers;
            this.keyboardModifiers = keyModifiers;
        }

        /// <summary>
        /// Event for axis changes.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="axisId"></param>
        /// <param name="delta"></param>
        /// <param name="state"></param>
        public InputEvent(InputDevice owner, uint axisId, long delta, long state)
        {
            this.owner = owner;
            this.id = axisId;
            this.delta = delta;
            this.stateRecord = state;
            this.type = InputEventType.Axis;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The spawner of event.
        /// </summary>
        public InputDevice Spawner
        {
            get
            {
                return owner;
            }
        }

        /// <summary>
        /// Keyboard modifiers.
        /// </summary>
        public KeyboardModifiers KeyboardModifiers
        {
            get
            {
                return keyboardModifiers;
            }
        }

        /// <summary>
        /// Input event modifiers (why it was triggered).
        /// </summary>
        public InputEventModifier EventModifiers
        {
            get
            {
                return inputModifiers;
            }
        }

        /// <summary>
        /// The spawn time.
        /// </summary>
        public DateTime SpawnTime
        {
            get
            {
                return timeIdentification;
            }
        }

        /// <summary>
        /// Button state.
        /// </summary>
        public bool ButtonState
        {
            get
            {
                return stateRecord == 0 ? false : true;
            }
        }

        /// <summary>
        /// Axis state.
        /// </summary>
        public long AxisState
        {
            get
            {
                return stateRecord;
            }
        }

        /// <summary>
        /// Delta axis state.
        /// </summary>
        public long DeltaAxisState
        {
            get
            {
                return delta;
            }
        }

        /// <summary>
        /// The event type.
        /// </summary>
        public InputEventType EventType
        {
            get { return type; }
        }

        /// <summary>
        /// The key code, if keyboard device.
        /// </summary>
        public KeyCodes KeyCode
        {
            get
            {
                return (KeyCodes)this.id;
            }
        }

        /// <summary>
        /// Button id, if button was triggered.
        /// </summary>
        public uint ButtonId
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Axis id, if axis event.
        /// </summary>
        public uint AxisId
        {
            get
            {
                return this.id;
            }
        }

        #endregion
    }

    /// <summary>
    /// A delegate that returns the maximum distance between two clicks for treating as
    /// double click event.
    /// </summary>
    public delegate TimeSpan ButtonDoubleClick(InputDevice device, uint button);

    /// <summary>
    /// A delegate that returns time lag between firing new events for button down.
    /// </summary>
    public delegate TimeSpan ButtonHeldRepeat(InputDevice device, uint button, uint repeatCount);

    /// <summary>
    /// An event pump checks state of devices continiously and creates events.
    /// </summary>
    /// <remarks>Devices are not owned by pump.</remarks>
    public sealed class EventPump : IDisposable
    {
        #region Private Members
        bool deviceOwned = true;
        InputDevice[] boundDevices;
        System.Threading.Thread poolThread;
        TimeSpan poolInterval = TimeSpan.FromTicks(20000); // 50Hz
        volatile bool exitThread = false;
        volatile Queue<InputEvent> events = new Queue<InputEvent>();

        ButtonHeldRepeat buttonRepeat = null;
        ButtonDoubleClick buttonDoubleClick = null;
        #endregion

        #region Internal Methods

        ~EventPump()
        {
            Dispose(true);
        }

        internal void ThreadExec()
        {
            while (!exitThread)
            {

                if (Monitor.TryEnter(poolThread))
                {

                    // We pool the device.
                    for (int j = 0; j < boundDevices.Length; j++)
                    {
                        InputDevice device = boundDevices[j];
                        bool[] buttonState = device.ButtonState;
                        long[] axisState = device.AxisState;

                        // We sync it.
                        // FIXME: maybe try-sync or sync out of the loop.
                        device.Sync();

                        // We check if any state changes.
                        bool[] buttonStateAfter = device.ButtonState;
                        long[] axisStateAfter = device.AxisState;

                        // We extact modifiers.
                        KeyboardModifiers mod = KeyboardModifiers.None;
                        if (buttonStateAfter.Length >= 256)
                        {
                            if (buttonStateAfter[(int)KeyCodes.LCONTROL]) mod |= KeyboardModifiers.LCtrl;
                            if (buttonStateAfter[(int)KeyCodes.RCONTROL]) mod |= KeyboardModifiers.RCtrl;
                            if (buttonStateAfter[(int)KeyCodes.LSHIFT]) mod |= KeyboardModifiers.LShift;
                            if (buttonStateAfter[(int)KeyCodes.RSHIFT]) mod |= KeyboardModifiers.RShift;
                            // TODO: alts
                            if (buttonStateAfter[(int)KeyCodes.LWIN] || 
                                buttonStateAfter[(int)KeyCodes.RWIN]) mod |= KeyboardModifiers.Win;

                        }

                        for (int i = 0; i < buttonState.Length; i++)
                        {
                            if (buttonState[i] != buttonStateAfter[i])
                            {
                                InputEvent ev = new InputEvent(device, (uint)i, 
                                    buttonStateAfter[i], InputEventModifier.None, 
                                    mod);
                                events.Enqueue(ev);

                                Console.WriteLine(((KeyCodes)i).ToString());
                            }
                        }

                        for (int i = 0; i < axisState.Length; i++)
                        {
                            if (axisState[i] != axisStateAfter[i])
                            {
                                InputEvent ev = new InputEvent(device, (uint)i,
                                    axisStateAfter[i] - axisState[i], axisStateAfter[i]);
                                events.Enqueue(ev);
                            }
                        }
                    }

                    Monitor.Exit(poolThread);
                    Thread.Sleep(0);
                }

            }
        }

        void Dispose(bool fin)
        {
            if (exitThread) return;

            exitThread = true;
            poolThread.Join();

            if (deviceOwned)
            {
                foreach (InputDevice dev in boundDevices)
                {
                    dev.Dispose();
                }
            }

            if (!fin) GC.SuppressFinalize(this);
        }

        void AssertNotDisposed()
        {
            if (exitThread) throw new ObjectDisposedException("EventPump already disposed.");
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates event pump.
        /// </summary>
        /// <param name="boundDevices"></param>
        public EventPump(params InputDevice[] boundDevices)
        {
            this.boundDevices = boundDevices.Clone() as InputDevice[];

            // We create a small stack thread.
            this.poolThread = new System.Threading.Thread(ThreadExec, 10 * 1024);
            this.poolThread.Start();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Number of non-processed events.
        /// </summary>
        public uint EventCount
        {
            get
            {
                lock (poolThread)
                {
                    AssertNotDisposed();
                    return (uint)events.Count;
                }
            }
        }

        /// <summary>
        /// A button double click range provider.
        /// </summary>
        public ButtonDoubleClick DoubleClickSpan
        {
            set
            {
                AssertNotDisposed();
                buttonDoubleClick = value;
            }
            get
            {
                return buttonDoubleClick;
            }
        }

        /// <summary>
        /// A button held repeat provider.
        /// </summary>
        public ButtonHeldRepeat ButtonHeldRepeat
        {
            set
            {
                AssertNotDisposed();
                buttonRepeat = value;
            }
            get
            {
                return buttonRepeat;
            }
        }

        /// <summary>
        /// Returns the primary mouse.
        /// </summary>
        public InputDevice PrimaryMouse
        {
            get
            {
                AssertNotDisposed();
                for (int i = 0; i < this.boundDevices.Length; i++)
                {
                    if (boundDevices[i].Descriptor.DeviceType == InputDeviceType.Mouse &&
                       boundDevices[i].Descriptor.DeviceId == 0)
                    {
                        return boundDevices[i];
                    }
                }
                return null;

            }
        }

        /// <summary>
        /// Returns the primary cursor (OS cursor listener device).
        /// </summary>
        public InputDevice PrimaryCursor
        {
            get
            {
                AssertNotDisposed();
                for (int i = 0; i < this.boundDevices.Length; i++)
                {
                    if (boundDevices[i].Descriptor.DeviceType == InputDeviceType.Cursor &&
                       boundDevices[i].Descriptor.DeviceId == 0)
                    {
                        return boundDevices[i];
                    }
                }
                return null;

            }
        }

        /// <summary>
        /// Returns all input devices.
        /// </summary>
        public InputDevice[] InputDevices
        {
            get
            {
                return boundDevices.Clone() as InputDevice[];
            }
        }

        /// <summary>
        /// Are devices owned by pump.
        /// </summary>
        public bool DevicesOwned
        {
            get
            {
                return deviceOwned;
            }
            set
            {
                AssertNotDisposed();
                deviceOwned = value;
            }
        }

        /// <summary>
        /// Returns primary keyboard.
        /// </summary>
        public InputDevice PrimaryKeyboard
        {
            get
            {
                AssertNotDisposed();
                for (int i = 0; i < this.boundDevices.Length; i++)
                {
                    if (boundDevices[i].Descriptor.DeviceType == InputDeviceType.Keyboard &&
                       boundDevices[i].Descriptor.DeviceId == 0)
                    {
                        return boundDevices[i];
                    }
                }
                return null;
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Obtains the next event.
        /// </summary>
        /// <returns></returns>
        public InputEvent Get()
        {
            lock (poolThread)
            {
                AssertNotDisposed();
                if (events.Count > 0)
                {
                    return events.Dequeue();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Peeks the input event.
        /// </summary>
        /// <returns></returns>
        public InputEvent Peek()
        {
            lock (poolThread)
            {
                AssertNotDisposed();
                if (events.Count > 0)
                {
                    return events.Peek();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Enqueues event.
        /// </summary>
        /// <param name="event"></param>
        public void Add(InputEvent @event)
        {
            lock (poolThread)
            {
                AssertNotDisposed();
                events.Enqueue(@event);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(false);
        }

        #endregion
    }


}
