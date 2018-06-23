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
using SharpMedia.Graphics.Driver;

namespace SharpMedia.Graphics
{



    /// <summary>
    /// An OS window class. SwapChain, Window and Device are always closely related. One window
    /// always has one device and one swap chain associated.
    /// </summary>
    public sealed class Window
    {
        #region Messager Class

        private class WindowMessager : IWindow
        {
            Window window;

            public WindowMessager(Window window)
            {
                this.window = window;
            }

            #region IWindow Members

            public void Resized(uint width, uint height)
            {
                // First resize it.
                lock (window.syncRoot)
                {
                    window.Device.SwapChain.Resized(width, height);
                    window.width = width;
                    window.height = height;
                }

                // Fire event.
                Action<Window> r = window.resized;
                if(r != null)
                {
                    r(window);
                }
            }

            public void Closed()
            {
                Action<Window> c = window.closed;
                if (c != null)
                {
                    c(window);
                }
            }

            public void Minimized(bool minimize)
            {
                Action<Window> m = window.minimized;
                if (m != null)
                {
                    window.isMinimized = minimize;
                    m(window);
                }
            }

            public void Focused(bool focus)
            {
                Action<Window> f = window.focus;
                if (f != null)
                {
                    window.hasFocus = focus;
                    f(window);
                }
            }

            #endregion
        }

        #endregion

        #region Private Members
        GraphicsDevice device;
        WindowMessager messanger;
        Driver.IWindowBackend backend;

        object syncRoot = new object();
        uint width;
        uint height;
        bool hasFocus = true;
        bool isMinimized = false;
        Action<Window> closed;
        Action<Window> resized;
        Action<Window> focus;
        Action<Window> minimized;
        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        internal Window(GraphicsDevice device, uint width, uint height, Driver.IWindowBackend backend)
        {
            this.device = device;
            this.width = width;
            this.height = height;
            this.backend = backend;
            this.messanger = new WindowMessager(this);

            // We register messanger.
            backend.SetListener(messanger);

        }

        internal void Update(uint width, uint height)
        {
            this.width = width;
            this.height = height;
            this.hasFocus = false;
            this.isMinimized = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A window handle (native).
        /// </summary>
        public IntPtr WindowHandle
        {
            get
            {
                return backend.Handle;
            }
        }

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
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public uint Width
        {
            get
            {
                return width;
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public uint Height
        {
            get
            {
                return height;
            }
        }

        /// <summary>
        /// Does the events. Must be called at least once per frame.
        /// </summary>
        public void DoEvents()
        {
            lock (syncRoot)
            {
                backend.DoEvents();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Triggered when the window close button is pressed.
        /// </summary>
        public event Action<Window> Closed
        {
            add
            {
                lock (syncRoot)
                {
                    closed += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    closed -= value;
                }
            }
        }

        /// <summary>
        /// Triggered when window is resized. Device takes care of swap chain and window adjustion.
        /// </summary>
        public event Action<Window> Resized
        {
            add
            {
                lock(syncRoot)
                {
                    resized += value;
                }
            }
            remove
            {
                lock(syncRoot)
                {
                    resized -= value;
                }
            }
        }

        /// <summary>
        /// Triggered when window gains or looses focus.
        /// </summary>
        public event Action<Window> Focused
        {
            add
            {
                lock (syncRoot)
                {
                    focus += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    focus -= value;
                }
            }
        }

        /// <summary>
        /// Called when window is minimized or reopened, focus is also triggered.
        /// </summary>
        public event Action<Window> Minimized
        {
            add
            {
                lock (syncRoot)
                {
                    minimized += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    minimized -= value;
                }
            }
        }

        #endregion

    }
}
