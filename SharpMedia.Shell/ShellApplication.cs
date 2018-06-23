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
using SharpMedia.Components.Applications;
using SharpMedia.Graphics;
using SharpMedia.Components.Configuration;
using SharpMedia.Input;
using SharpMedia.Components;
using SharpMedia.Components.Configuration.ComponentProviders;
using SharpMedia.Graphics.GUI.Themes;

namespace SharpMedia.Shell
{

    /// <summary>
    /// A shell startup application.
    /// </summary>
    public sealed class ShellApplication : DocumentApplication
    {
        #region Private Members
        GraphicsService graphicsService;
        InputService inputService;
        RenderTargetParameters rtParameters;
        bool graphicsDebug = false;
        IComponentDirectory componentDirectory;
        IGuiTheme defaultTheme;
        #endregion

        #region Configuration

        /// <summary>
        /// A graphics service.
        /// </summary>
        [Required]
        public GraphicsService GraphicsService
        {
            get { return graphicsService; }
            set { graphicsService = value; }
        }

        /// <summary>
        /// Input service.
        /// </summary>
        [Required]
        public InputService InputService
        {
            get { return inputService; }
            set { inputService = value; }
        }

        /// <summary>
        /// Component directory.
        /// </summary>
        [Required]
        public IComponentDirectory ComponentDirectory
        {
            get { return componentDirectory; }
            set { componentDirectory = value; }
        }

        /// <summary>
        /// Render target parameters for startup.
        /// </summary>
        public RenderTargetParameters RenderTargetParameters
        {
            get { return rtParameters; }
            set { rtParameters = value; }
        }

        /// <summary>
        /// Should graphics be debugged.
        /// </summary>
        public bool DebugGraphics
        {
            get { return graphicsDebug; }
            set { graphicsDebug = value; }
        }

        /// <summary>
        /// Default GUI theme.
        /// </summary>
        public IGuiTheme DefaultTheme
        {
            get { return defaultTheme; }
            set { defaultTheme = value; }
        }

        #endregion

        #region Startup

        public override int StartDocument(DocumentParameter[] parameters)
        {
            if (parameters.Length > 0)
            {

                throw new Exception("Shell application does not allow any parameters.");
            }

            // We first check if render target parameters are set.
            if (rtParameters == null)
            {
                rtParameters = new RenderTargetParameters();
                rtParameters.BackBufferCount = 2;
                rtParameters.BackBufferWidth = 1024;
                rtParameters.BackBufferHeight = 768;
                rtParameters.DepthStencilCommonFormat = CommonPixelFormatLayout.D24_UNORM_S8_UINT;
                rtParameters.FormatCommon = CommonPixelFormatLayout.X8Y8Z8W8_UNORM;
                rtParameters.MultiSampleQuality = 0;
                rtParameters.MultiSampleType = 1;
                rtParameters.RefreshRate = 0;
                rtParameters.Windowed = true;
            }


            // We create graphics device.
            Window window = null;
            GraphicsDevice device = null;
            IWindowManager manager = null;

            try {
                // 1) We create graphics device in owned mode.
                device = graphicsService.CreateDevice(true, graphicsDebug, rtParameters, out window);
            
                // 2) We initialize input.
                inputService.Initialize(window);

                // 3) We create window manager.
                manager = new Default.DefaultWindowManager(device, device.SwapChain, inputService, defaultTheme);

                // 4) We replace component directory (add new stuff in shell enviorment).
                IComponentDirectory newDirectory = new ComponentDirectory(componentDirectory, "Shell");
                newDirectory.Register(new Instance(true, "ShellFinishedConfiguring"));
                newDirectory.Register(new Instance(manager));
                newDirectory.Register(new Instance(defaultTheme));

                // 5) We replace console by Shell provided console.
                newDirectory.Register(new AlwaysNewConfiguredComponent(
                    typeof(ShellTextConsole).FullName));

                // 6) We set the component directory.
                
                

                // 7) We unregister component directory.

            } finally {
                if(manager != null && manager is IDisposable) (manager as IDisposable).Dispose();
                if(inputService != null) inputService.Dispose();
                if(device != null) device.Dispose();
            }

            return 0;
        }

        #endregion

        
    }
}
