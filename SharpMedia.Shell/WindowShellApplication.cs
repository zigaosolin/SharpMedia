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
using SharpMedia.Components;
using SharpMedia.Components.Configuration;
using SharpMedia.Math;
using SharpMedia.Graphics.GUI;
using SharpMedia.Graphics.GUI.Themes;
using SharpMedia.Graphics;

namespace SharpMedia.Shell
{

    /// <summary>
    /// A window shell application base class.
    /// </summary>
    /// <remarks>Provides binding for Gui based applications that use Shell.</remarks>
    public abstract class WindowShellApplication : DocumentApplication
    {
        #region Properted Members
        bool shellFinishedConfiguring = false;
        string title;
        string group;
        WindowOptions? windowOptions;
        Vector2i? position;
        Vector2i? size;
        Vector2i? minSize;
        Vector2i? maxSize;
        WindowState? windowState;
        IGuiTheme theme;
        
        protected IWindowManager windowManager;
        protected RootWindow clientWindow;
        protected GraphicsDevice graphicsDevice;
        #endregion

        #region Properties (Configurable)

        /// <summary>
        /// This is called the last, to finish configuration.
        /// </summary>
        [Required, After("")]
        public bool ShellFinishedConfiguring
        {
            get { return shellFinishedConfiguring; }
            set { shellFinishedConfiguring = value; }
        }

        /// <summary>
        /// Theme used for Gui elements.
        /// </summary>
        [Required]
        public IGuiTheme GuiTheme
        {
            get { return theme; }
            set { theme = value; }
        }

        /// <summary>
        /// Graphics device used for rendering.
        /// </summary>
        [Required]
        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
            set { graphicsDevice = value; }
        }

        /// <summary>
        /// Window Manager.
        /// </summary>
        [Required]
        public IWindowManager WindowManager
        {
            get { return windowManager; }
            set { windowManager = value; }
        }

        /// <summary>
        /// Title of window.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Title group.
        /// </summary>
        public string TitleGroup
        {
            get { return group; }
            set { group = value; }
        }

        /// <summary>
        /// Window options.
        /// </summary>
        public WindowOptions WindowOptions
        {
            get { return windowOptions.GetValueOrDefault(); }
            set 
            { 
                // Ignore it once we have valid value.
                if(windowOptions.HasValue && shellFinishedConfiguring) return;
                windowOptions = value; 
            }
        }

        /// <summary>
        /// Window position.
        /// </summary>
        public Vector2i Position
        {
            get { return position.GetValueOrDefault(); }
            set
            {
                if (position.HasValue && shellFinishedConfiguring) return;
                position = value;
            }
        }

        /// <summary>
        /// Size of window.
        /// </summary>
        public Vector2i Size
        {
            get { return size.GetValueOrDefault(); }
            set
            {
                if (size.HasValue && shellFinishedConfiguring) return;
                size = value;
            }
        }

        /// <summary>
        /// Size of window.
        /// </summary>
        public Vector2i MinSize
        {
            get { return minSize.GetValueOrDefault(); }
            set
            {
                if (minSize.HasValue && shellFinishedConfiguring) return;
                minSize = value;
            }
        }

        /// <summary>
        /// Size of window.
        /// </summary>
        public Vector2i MaxSize
        {
            get { return maxSize.GetValueOrDefault(); }
            set
            {
                if (maxSize.HasValue && shellFinishedConfiguring) return;
                maxSize = value;
            }
        }

        
        public WindowState WindowState
        {
            get { return windowState.GetValueOrDefault(); }
            set
            {
                if (windowState.HasValue && shellFinishedConfiguring) return;
                windowState = value;
            }
        }

        #endregion

        #region Procedural

        /// <summary>
        /// Obtains client window.
        /// </summary>
        public RootWindow ClientWindow
        {
            get { return clientWindow; }
        }

        /// <summary>
        /// Creates root based on Gui manager and other configurations.
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public RootWindow CreateRoot(Graphics.GUI.Widgets.IWidget root)
        {
            return null;
        }

        #endregion

    }
}
