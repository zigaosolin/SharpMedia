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
    /// The input device type.
    /// </summary>
    [Flags]
    public enum InputDeviceType
    {
        Mouse = 1,
        Keyboard = 2,
        Joystick = 4,
        Wheel = 8,
        Flightstick = 16,

        /// <summary>
        /// A special "cursor" device tracks the OS cursor.
        /// </summary>
        Cursor = 32
    }

    /// <summary>
    /// An input device class.
    /// </summary>
    /// <remarks>Multiple input devices can be created, but they all share the same driver device.</remarks>
    public sealed class InputDevice : IDisposable
    {
        #region Private Members
        InputService service;
        InputDeviceDescriptor desc;
        DeviceBucket bucket;

        // State variables.
        bool[] buttonStates;
        long[] axisStates;

        internal class DeviceBucket
        {
            public uint UseCount = 1;
            public Driver.IInputDevice Device;
        };

        #endregion

        #region Internal Methods

        void AssertNotDisposed()
        {
            if (bucket == null)
            {
                throw new ObjectDisposedException(
                    string.Format("Input device '{0}' already disposed.", desc.ToString()));
            }
        }

        internal InputDevice(InputService service, 
            DeviceBucket bucket, InputDeviceDescriptor desc)
        {
            this.service = service;
            this.bucket = bucket;
            this.desc = desc;

            this.buttonStates = new bool[desc.ButtonCount];
            this.axisStates = new long[desc.AxisCount];
        }

        void Dispose(bool fin)
        {
            if (bucket != null)
            {
                bucket.UseCount--;
                if (bucket.UseCount <= 0 && !fin)
                {
                    bucket.Device.Dispose();
                    bucket.Device = null;
                }
                bucket = null;
                service = null;
            }

            if (!fin) GC.SuppressFinalize(this);
        }

        ~InputDevice()
        {
            Dispose(true);
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Obtains the root input.
        /// </summary>
        public InputService InputService
        { 
            get 
            {
                return this.service; 
            }
        }

        /// <summary>
        /// The device descriptor.
        /// </summary>
        public InputDeviceDescriptor Descriptor
        {
            get
            {
                return desc;
            }
        }

        /// <summary>
        /// Checks if button is down.
        /// </summary>
        /// <param name="buttonId"></param>
        /// <returns></returns>
        public bool this[uint buttonId]
        {
            get
            {
                if (buttonId >= desc.ButtonCount) return false;
                return buttonStates[(int)buttonId];
            }
        }

        /// <summary>
        /// Gets button state (it is cloned).
        /// </summary>
        public bool[] ButtonState
        {
            get
            {
                return buttonStates.Clone() as bool[];
            }
        }

        /// <summary>
        /// Obtains axis data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public long GetAxis(uint id)
        {
            if (id >= desc.AxisCount) return 0;
            return axisStates[id];
        }

        /// <summary>
        /// Obtains axis state.
        /// </summary>
        public long[] AxisState
        {
            get
            {
                return axisStates.Clone() as long[];
            }
        }

        /// <summary>
        /// Synhronizes the state.
        /// </summary>
        public void Sync()
        {
            AssertNotDisposed();
            lock (bucket)
            {
                bucket.Device.GetState(buttonStates, axisStates);
            }
        }

        /// <summary>
        /// Tries to synhonize.
        /// </summary>
        public bool TrySync()
        {
            AssertNotDisposed();
            if (Monitor.TryEnter(bucket))
            {
                try
                {
                    bucket.Device.GetState(buttonStates, axisStates);
                }
                finally
                {
                    Monitor.Exit(bucket);
                }
                return true;
            }
            return false;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (bucket == null) return;
            lock (bucket)
            {
                Dispose(false);
            }
        }

        #endregion
    }
}
