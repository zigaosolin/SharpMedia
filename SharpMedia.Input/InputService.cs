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
using SharpMedia.Graphics;
using SharpMedia.Components;
using SharpMedia.AspectOriented;
using System.Timers;

namespace SharpMedia.Input
{

    /// <summary>
    /// An input service.
    /// </summary>
    public sealed class InputService : IDisposable
    {
        #region Private Members
        object syncRoot = new object();
        Driver.IInputService inputService;

        // Caching data.
        Window window;
        Dictionary<InputDeviceDescriptor, InputDevice.DeviceBucket> inputDevices
            = new Dictionary<InputDeviceDescriptor, InputDevice.DeviceBucket>();
        #endregion

        #region Private Members

        void AssertInitialized()
        {
            if (window == null) throw new InvalidOperationException("Input service not initialized.");
        }

        ~InputService()
        {
            if (inputService != null) inputService.Dispose();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an input based on specific driver.
        /// </summary>
        /// <param name="input"></param>
        public InputService([NotNull] Driver.IInputService input)
        {
            this.inputService = input;
        }

        /// <summary>
        /// Initialized input service.
        /// </summary>
        /// <param name="window"></param>
        /// <remarks>May not need initialization if Shell driver.</remarks>
        public void Initialize([NotNull] Window window)
        {
            lock (syncRoot)
            {
                this.inputService.Initialize(window);

                this.window = window;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Lists all supported devices.
        /// </summary>
        public InputDeviceDescriptor[] SupportedDevices
        {
            get
            {
                AssertInitialized();
                return inputService.SupportedDevices;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a device baed on description.
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public InputDevice CreateDevice(InputDeviceDescriptor desc)
        {
            lock (syncRoot)
            {
                AssertInitialized();

                InputDevice.DeviceBucket bucket;
                // We first search in cache.
                if (inputDevices.TryGetValue(desc, out bucket))
                {
                    lock (bucket)
                    {
                        if (bucket.UseCount > 0)
                        {
                            bucket.UseCount++;
                            return new InputDevice(this, bucket, desc);
                        }
                    }
                }

                // Otherwise, we create it.
                bucket = new InputDevice.DeviceBucket();
                bucket.Device = inputService.Create(desc);

                inputDevices[desc] = bucket;

                return new InputDevice(this, bucket, desc);
            }
        }

        #endregion

        #region Device Searching

        /// <summary>
        /// Creates primary device of type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public InputDevice CreateDevice(InputDeviceType type)
        {
            return CreateDevice(Find(type));
        }

        /// <summary>
        /// Creates predicate selected device of type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public InputDevice CreateDevice(InputDeviceType type, Predicate<InputDeviceDescriptor> desc)
        {
            return CreateDevice(Find(type, desc));
        }

        /// <summary>
        /// Finds any device of this type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public InputDeviceDescriptor Find(InputDeviceType type, Predicate<InputDeviceDescriptor> predicate)
        {
            InputDeviceDescriptor[] descs = SupportedDevices;
            for (int i = 0; i < descs.Length; i++)
            {
                if (descs[i].DeviceType != type) continue;
                
                // We check predicate.
                if (predicate(descs[i])) return descs[i];
            }

            return null;
        }

        /// <summary>
        /// Finds primary device of type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public InputDeviceDescriptor Find(InputDeviceType type)
        {
            return Find(type, delegate(InputDeviceDescriptor desc) { return desc.DeviceId == 0; });
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (syncRoot)
            {
                if (inputService != null)
                {
                    // FIXME: may need to release all devices alive

                    inputService.Dispose();
                    inputService = null;

                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion
    }
}
