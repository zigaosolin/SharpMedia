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
    /// Texture manager tracks texture usage by process. It is asociated with Device.
    /// </summary>
    public sealed class TextureManager
    {
        #region Private Members
        GraphicsDevice device;

        ulong usedDeviceTextureMemory = 0;
        ulong usedSystemTextureMemory = 0;
        uint textureCount = 0;
        uint textureViewsCount = 0;
        uint inSystemMemoryTextureCount = 0;
        uint inDeviceMemoryTextureCount = 0;
        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureManager"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal TextureManager(GraphicsDevice device)
        {
            this.device = device;
        }

        /// <summary>
        /// Adds the system texture.
        /// </summary>
        /// <param name="size">The size.</param>
        internal void AddSystemTextureUsage(ulong size)
        {
            inSystemMemoryTextureCount++;
            usedSystemTextureMemory += size;
        }

        /// <summary>
        /// Removes the system texture.
        /// </summary>
        /// <param name="size">The size.</param>
        internal void RemoveSystemTextureUsage(ulong size)
        {
            inSystemMemoryTextureCount--;
            usedSystemTextureMemory -= size;
        }

        /// <summary>
        /// Adds the system texture.
        /// </summary>
        /// <param name="size">The size.</param>
        internal void AddDeviceTextureUsage(ulong size)
        {
            inDeviceMemoryTextureCount++;
            usedDeviceTextureMemory += size;
        }

        /// <summary>
        /// Removes the system texture.
        /// </summary>
        /// <param name="size">The size.</param>
        internal void RemoveDeviceTextureUsage(ulong size)
        {
            inDeviceMemoryTextureCount--;
            usedDeviceTextureMemory -= size;
        }

        /// <summary>
        /// Adds the texture.
        /// </summary>
        internal void AddTexture()
        {
            textureCount++;
        }

        /// <summary>
        /// Removes texture.
        /// </summary>
        internal void RemoveTexture()
        {
            textureCount--;
        }

        /// <summary>
        /// Adds the texture view.
        /// </summary>
        internal void AddTextureView()
        {
            textureViewsCount++;
        }

        /// <summary>
        /// Removes the texture.
        /// </summary>
        internal void RemoveTextureView()
        {
            textureViewsCount--;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <value>The device.</value>
        public GraphicsDevice Device
        {
            get
            {
                return device;
            }
        }

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
        /// Gets the used device's memory by textures and buffers.
        /// </summary>
        /// <value>The used texture memory.</value>
        public ulong UsedDeviceMemory
        {
            get
            {
                return usedDeviceTextureMemory + device.BufferManager.UsedDeviceMemoryByBuffers;
            }
        }

        /// <summary>
        /// Gets the used device memory by textures.
        /// </summary>
        /// <value>The used device memory by textures.</value>
        public ulong UsedDeviceMemoryByTextures
        {
            get
            {
                return usedDeviceTextureMemory;
            }
        }

        /// <summary>
        /// Gets the used system texture memory.
        /// </summary>
        /// <value>The used system texture memory.</value>
        public ulong UsedSystemTextureMemory
        {
            get
            {
                return usedSystemTextureMemory;
            }
        }

        /// <summary>
        /// The number of textures.
        /// </summary>
        public uint TextureCount
        {
            get
            {
                return textureCount;
            }
        }

        /// <summary>
        /// Number of texture views. Views of buffers also categorize as such views.
        /// </summary>
        public uint TextureViewsCount
        {
            get
            {
                return textureViewsCount;
            }
        }

        /// <summary>
        /// In system memory texture count.
        /// </summary>
        /// <remarks>
        /// This may be lower than actual, the texture is added when first made aware
        /// to Device (on bound or preparation).
        /// </remarks>
        public uint InSystemMemoryTextureCount
        {
            get
            {
                return inSystemMemoryTextureCount;
            }
        }

        /// <summary>
        /// Number of textures in device memory.
        /// </summary>
        public uint InDeviceMemoryTextureCount
        {
            get
            {
                return inDeviceMemoryTextureCount;
            }
        }

        #endregion
    }
}
