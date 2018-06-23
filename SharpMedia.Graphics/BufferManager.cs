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

namespace SharpMedia.Graphics
{
    /// <summary>
    /// Buffer manager constains buffer specific data (buffer resources and buffer views).
    /// </summary>
    public sealed class BufferManager
    {
        #region Private Members
        GraphicsDevice device;
        uint bufferCount = 0;
        uint bufferViewCount = 0;
        uint inSystemMemoryBufferCount = 0;
        uint inDeviceMemoryBufferCount = 0;
        ulong usedSystemMemory = 0;
        ulong usedDeviceMemory = 0;
        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferManager"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal BufferManager(GraphicsDevice device)
        {
            this.device = device;
        }

        /// <summary>
        /// Adds the buffer.
        /// </summary>
        internal void AddBuffer()
        {
            bufferCount++; 
        }

        /// <summary>
        /// Removes the buffer.
        /// </summary>
        internal void RemoveBuffer() 
        { 
            bufferCount--;
        }

        /// <summary>
        /// Adds the system buffer usage.
        /// </summary>
        /// <param name="size">The size.</param>
        internal void AddSystemBufferUsage(ulong size)
        {
            inSystemMemoryBufferCount++;
            usedSystemMemory += size;
        }

        /// <summary>
        /// Adds the device buffer usage.
        /// </summary>
        /// <param name="size">The size.</param>
        internal void AddDeviceBufferUsage(ulong size)
        {
            inDeviceMemoryBufferCount++;
            usedDeviceMemory += size;
        }

        /// <summary>
        /// Remove the system buffer usage.
        /// </summary>
        /// <param name="size">The size.</param>
        internal void RemoveSystemBufferUsage(ulong size)
        {
            inSystemMemoryBufferCount--;
            usedSystemMemory -= size;
        }

        /// <summary>
        /// Remove the device buffer usage.
        /// </summary>
        /// <param name="size">The size.</param>
        internal void RemoveDeviceBufferUsage(ulong size)
        {
            inDeviceMemoryBufferCount--;
            usedDeviceMemory -= size;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the available device's memory.
        /// </summary>
        /// <value>The available texture memory.</value>
        public ulong AvailableDeviceMemory
        {
            get
            {
                return device.DriverDevice.DeviceMemory;
            }
        }

        /// <summary>
        /// Gets the used device memory.
        /// </summary>
        /// <value>The used device memory.</value>
        public ulong UsedDeviceMemory
        {
            get
            {
                return usedDeviceMemory + device.TextureManager.UsedDeviceMemoryByTextures;
            }
        }

        /// <summary>
        /// Gets the used device memory by buffers.
        /// </summary>
        /// <value>The used device memory by buffers.</value>
        public ulong UsedDeviceMemoryByBuffers
        {
            get
            {
                return usedDeviceMemory;
            }
        }

        /// <summary>
        /// Gets the in system memory buffer count.
        /// </summary>
        /// <value>The in system memory buffer count.</value>
        public uint InSystemMemoryBufferCount
        {
            get
            {
                return inSystemMemoryBufferCount;
            }
        }

        /// <summary>
        /// Gets the in device memory buffer count.
        /// </summary>
        /// <value>The in device memory buffer count.</value>
        public uint InDeviceMemoryBufferCount
        {
            get
            {
                return inDeviceMemoryBufferCount;
            }
        }

        /// <summary>
        /// Gets the buffer count.
        /// </summary>
        /// <value>The buffer count.</value>
        public uint BufferCount
        {
            get
            {
                return bufferCount;
            }
        }

        /// <summary>
        /// Gets the buffer view count.
        /// </summary>
        /// <value>The buffer view count.</value>
        public uint BufferViewCount
        {
            get
            {
                return bufferViewCount;
            }
        }

        #endregion

    }
}
