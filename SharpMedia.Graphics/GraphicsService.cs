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
using SharpMedia.AspectOriented;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Graphics
{
    /// <summary>
    /// Takes care of graphics initialization.
    /// </summary>
    public class GraphicsService : IDisposable
    {
        #region Private Members
        Driver.IGraphicsService service;
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsInit"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public GraphicsService([Required, NotNull] Driver.IGraphicsService service)
        {
             this.service = service as Driver.IGraphicsService;
        }

        /// <summary>
        /// Does the device exist; all explicit initialization will fail.
        /// </summary>
        public bool DeviceExists
        {
            get
            {
                return service.IsDeviceActive;
            }
        }

        /// <summary>
        /// Obtains an already created device.
        /// </summary>
        /// <param name="parameters">Parameters for default rendering surface, may be null for shared mode only.</param>
        /// <returns></returns>
        public GraphicsDevice ObtainDevice(RenderTargetParameters parameters)
        {
            GraphicsDevice device = new GraphicsDevice(service.Obtain());

            // We now create a render target.
            if (parameters != null)
            {
                // TODO account for multisampling.

                TypelessTexture2D rt = new TypelessTexture2D(Usage.Default, TextureUsage.RenderTarget, CPUAccess.None,
                    parameters.Format, parameters.BackBufferWidth, parameters.BackBufferHeight, 1,
                    GraphicsLocality.DeviceMemoryOnly, null);

                if (parameters.DepthStencilCommonFormat != CommonPixelFormatLayout.NotCommonLayout)
                {
                    TypelessTexture2D dt = new TypelessTexture2D(Usage.Default, TextureUsage.DepthStencilTarget, CPUAccess.None,
                        parameters.DepthStencilFormat, parameters.BackBufferWidth, parameters.BackBufferHeight, 1,
                        GraphicsLocality.DeviceMemoryOnly, null);

                    // Associate render target and depth stencil with graphics device.
                    device.Initialize(rt.CreateRenderTarget(), dt.CreateDepthStencil());
                }
                else
                {
                    device.Initialize(rt.CreateRenderTarget(), null);
                }

                // They will be auto-bound.
            }
            else
            {
                device.Initialize(null, null);
            }

            return device;
        }

        /// <summary>
        /// Creates the device.
        /// </summary>
        /// <param name="shared">Is the device shared (can call obtain device from other processes).</param>
        /// <param name="parameters">The parameters that define swap chain.</param>
        /// <param name="window">Window created with device (and swap chain).</param>
        /// <returns>Device object.</returns>
        public GraphicsDevice CreateDevice(bool shared, bool debug, RenderTargetParameters parameters, out Window window)
        {
            Driver.ISwapChain dr_chain;
            Driver.IWindowBackend dr_window;
            Driver.IDevice dr_device = service.Create(shared, parameters, out dr_chain, out dr_window, debug);

            GraphicsDevice device = new GraphicsDevice(dr_device);

            // For now set null for device backend.
            window = new Window(device, parameters.BackBufferWidth, 
                parameters.BackBufferHeight, dr_window);

            SwapChain chain = new SwapChain(device, window, parameters.Format, !parameters.Windowed, dr_chain);

            // TODO: create depth stencil.

            // Intialize device.
            device.Initialize(chain, null);

            return device;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Can be disposed anytime when not needed anymore.
        /// </summary>
        public void Dispose()
        {
            // We clear all states.
            StateManager.Collect();

            // We collect all objects (until bug is fixed).
            GC.Collect();

            service.Dispose();
        }

        #endregion
    }
}
