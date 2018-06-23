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
using SharpMedia.Math;
using SharpMedia.Shell.StandardEffects;
using System.Threading;
using SharpMedia.Graphics.GUI;
using SharpMedia.Graphics.Vector;
using SharpMedia.Graphics.States;
using SharpMedia.Input;
using SharpMedia.Graphics.GUI.Themes;

namespace SharpMedia.Shell.Default
{
    /// <summary>
    /// A default implementation of window manager.
    /// </summary>
    public sealed class DefaultWindowManager : IWindowManager, IWindowManagerControl
    {
        #region Private Members
        object syncRoot = new object();

        // Rendering and sizing data.
        GraphicsDevice device;
        RenderTargetView renderTarget;

        // Event processor.
        EventProcessor processor;

        // Windows in z-order (back to front).
        List<DefaultWindow> windows = new List<DefaultWindow>();

        // If in fullscreen mode, this window gets full attention.
        DefaultWindow fullscreenWindow;

        // Desktop renderer and current desktop.
        GuiManager desktopManager;
        DefaultDesktop currentDesktop;

        // Events.
        Action<IWindowManager> onPreRendering;
        Action<IWindowManager> onPostRendering;
        Action<IWindowManager> onDisposing;

        // A pointer.
        SharpMedia.Graphics.GUI.Standalone.GuiPointer pointer;

        // A compositing framework.
        Compositor compositor;

        DefaultWindow focused;

        // Is it dirty (one of window was changed, cursor was moved ...).
        volatile bool isDirty = true;
        #endregion

        #region Private Methods

        ~DefaultWindowManager()
        {
            Dispose(true);
        }

        void Dispose(bool fin)
        {
            if (renderTarget != null)
            {
                // Render target and device not owned.
                compositor.Dispose();
                processor.EventPump.Dispose();
                processor = null;
                device = null;
                renderTarget = null;

                if (!fin) GC.SuppressFinalize(this);
            }
        }

        void AssertNotDisposed()
        {
            if (renderTarget == null) throw new ObjectDisposedException("WindowManager was already disposed.");
        }

        /// <summary>
        /// Window rendering code.
        /// </summary>
        /// <param name="window"></param>
        void RenderWindow(DefaultWindow window)
        {
            if (!window.IsVisible) return;
            if (window.windowState == WindowState.Minimized) return;

            // We get bounds.
            Vector2i position = window.Position;
            Vector2i size = window.Size;

            // We now composite them.
            compositor.Push(window.renderData);
            //compositor.Push(Colour.Blue);
            //compositor.Add();

            // We first resize to match size.
            compositor.Resize((uint)size.X, (uint)size.Y);

            // We apply all affects if necessary.
            foreach (IWindowEffect effect in window.effects)
            {
                // We handle single image effects.

                // Should be some
            }


            // We handle possible compositing effects (only one possible).
            foreach (IWindowEffect effect in window.effects)
            {
                if (effect is WindowBlend)
                {
                    WindowBlend blendEffect = (WindowBlend)effect;

                    if (!blendEffect.UseRenderTargetAlpha)
                    {
                        // We apply it.
                        compositor.BlendTo((uint)position.X, (uint)position.Y, BlendOperation.Add,
                            BlendOperand.BlendFactorInverse, BlendOperand.BlendFactor, blendEffect.BlendFactor, renderTarget);
                    }
                    else
                    {
                        compositor.BlendTo((uint)position.X, (uint)position.Y, BlendOperation.Add,
                            BlendOperand.SrcAlphaInverse, BlendOperand.SrcAlpha, blendEffect.BlendFactor, renderTarget);
                    }

                    return;

                }
            }

            // Otherwise, we do normal composition.
            compositor.CopyTo((uint)position.X, (uint)position.Y, renderTarget);
        }

        /// <summary>
        /// We render window's title 
        /// </summary>
        /// <param name="window"></param>
        void RenderWindowTitleBar(DefaultWindow window)
        {
            if (!window.IsVisible) return;
            if (window.windowState == WindowState.Minimized) return;

            // TODO:
        }


        #endregion

        #region Internal Methods

        internal void SetFocused(DefaultWindow window)
        {
            focused = window;
        }

        internal void SetZOrder(DefaultWindow window, IWindow reference, bool after)
        {
            lock (syncRoot)
            {
                int idx = windows.IndexOf(window);

                // We must have found it.
                if (reference == null)
                {
                    windows.RemoveAt(idx);
                    if (after)
                    {
                        windows.Add(window);
                    }
                    else
                    {
                        windows.Insert(0, window);
                    }
                }
                else
                {
                    int refIdx = windows.IndexOf(reference as DefaultWindow);

                    if (refIdx < 0)
                    {
                        throw new InvalidOperationException("Reference window is invalid.");
                    }


                    if (after)
                    {
                        // Special case last.
                        if (refIdx + 1 == windows.Count)
                        {
                            windows.RemoveAt(idx);
                            windows.Add(window);
                        }
                        else
                        {
                            // Removing change indexing of window.
                            windows.RemoveAt(idx);
                            if (refIdx >= idx)
                            {
                                windows.Insert(refIdx, window);
                            }
                            else
                            {
                                windows.Insert(refIdx + 1, window);
                            }
                        }
                    }
                    else
                    {
                        // Removing change indexing of window.
                        windows.RemoveAt(idx);
                        if (refIdx >= idx)
                        {
                            windows.Insert(refIdx - 1 > 0 ? refIdx - 1 : 0, window);
                        }
                        else
                        {
                            windows.Insert(refIdx, window);
                        }
                    }
                }
            }
        }

        internal void WindowFullscreened(DefaultWindow window, bool state)
        {
            if (state) fullscreenWindow = window;
            else fullscreenWindow = null;
        }

        internal void WindowDisposed(DefaultWindow window)
        {
            lock (syncRoot)
            {
                windows.Remove(window);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtains device.
        /// </summary>
        public GraphicsDevice Device
        {
            get
            {
                return device;
            }
        }

        #endregion

        #region Eventing

        void KeyDown(InputEvent ev)
        {
            if (focused != null && focused.listener != null)
            {
                focused.listener.OnKeyPress(pointer, ev.KeyCode, ev.KeyboardModifiers, ev.EventModifiers);
            }
        }

        void KeyUp(InputEvent ev)
        {
            if (focused != null && focused.listener != null)
            {
                focused.listener.OnKeyRelease(pointer, ev.KeyCode, ev.KeyboardModifiers);
            }
        }

        void MouseButtonDown(InputEvent ev)
        {
            // TODO: may change focus.

            // TODO: issue event to focused.
        }

        void MouseButtonUp(InputEvent ev)
        {
            // TODO: may change focus.

            // TODO: issue event to focused.
        }

        void MouseAxis(InputEvent ev)
        {
            // TODO: find the topmost window over.

            // Issue mouse move event.

            // FIXME: for wheel, process to focused.
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Constructs a default window manager that holds the device (owner) in shared mode. 
        /// </summary>
        public DefaultWindowManager(GraphicsDevice device, RenderTargetView renderTarget, 
            Input.InputService input, IGuiTheme theme)
        {
            // Rendering data.
            this.device = device;
            this.renderTarget = renderTarget;

            // Input data.
            this.processor = new EventProcessor(new EventPump(input.CreateDevice(InputDeviceType.Mouse),
                input.CreateDevice(InputDeviceType.Keyboard)));

            // We bind events.
            this.processor.KeyDown += KeyDown;
            this.processor.KeyUp += KeyUp;
            this.processor.MouseButtonDown += MouseButtonDown;
            this.processor.MouseButtonUp += MouseButtonUp;
            this.processor.MouseAxis += MouseAxis;

            // We create "desktop manager".
            this.desktopManager = new GuiManager(new GraphicsCanvas(device, renderTarget, new Vector2f(1.0f, 1.0f)));
            this.pointer = new SharpMedia.Graphics.GUI.Standalone.GuiPointer(desktopManager, processor,
                null, null, new SharpMedia.Graphics.GUI.Standalone.Sensitivity()) ;

            theme.AutomaticApply(this.pointer, false);

            // FIXME: add pointer somehow to be "forward"
            this.desktopManager.AddNLObject(pointer);

            // We create composition.
            this.compositor = new Compositor(device);

        }

        #endregion

        #region Rendering

        /// <summary>
        /// Tries to render, will fail if in busy state (already rendering or processing other events).
        /// </summary>
        public bool TryRender()
        {

            // Otherwise we render. We render only if we can aquire lock in 1ms. If
            // we cannot acquire it, we are probably already rendering.
            if (!Monitor.TryEnter(syncRoot, 1)) return false;
            try
            {
                Render();
            }
            finally
            {
                Monitor.Exit(syncRoot);
            }

            return true;
        }

        /// <summary>
        /// Rendering code, forces rendering.
        /// </summary>
        public void Render()
        {
            lock (syncRoot)
            {
                // We do not render if everything is synced.
                if (!isDirty) return;

                using (DeviceLock l = device.Lock())
                {

                    // 1) First fire pre-rendering the event.
                    Action<IWindowManager> t = onPreRendering;
                    if (t != null) t(this);

                    // 2) Fullscreen rendering or not
                    if (fullscreenWindow != null)
                    {
                        lock (fullscreenWindow.syncRoot)
                        {
                            // We copy the window's target to ours.
                            compositor.Push(fullscreenWindow.renderData);

                            // We resize (a null operation in most cases).
                            compositor.Resize(renderTarget.Width, renderTarget.Height);

                            // We copy fullscreen application.
                            compositor.CopyTo(renderTarget);

                        }
                        return;
                    }
                    else
                    {

                        // 3) We render background (desktop).
                        desktopManager.Render();

                        // 4) We render windows in z-order.
                        for (int i = 0; i < windows.Count; i++)
                        {
                            DefaultWindow window = windows[i] as DefaultWindow;
                            lock (window.syncRoot)
                            {
                                // Renders window by compositing image.
                                RenderWindow(window);

                                // We also render title bar.
                                RenderWindowTitleBar(window);

                            }
                        }
                    }

                    // 5) We fire post-rendering event.
                    t = onPostRendering;
                    if (t != null) t(this);
                }

            }
        }


        #endregion

        #region IWindowManager Members

        public Vector2i DesktopSize
        {
            get 
            {
                AssertNotDisposed();
                return new Vector2i((int)renderTarget.Width, (int)renderTarget.Height); 
            }
        }

        public IWindow CreateWindow(Guid resourceId, string title, string windowGroup, 
            IWindowBackend listener, WindowOptions options, Vector2i position, 
            Vector2i size, Vector2i? minSize, Vector2i? maxSize, WindowState state,
            IWindow parentWindow, bool blockParentInput)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();


                DefaultWindow window = new DefaultWindow(this, resourceId, title, windowGroup,
                    listener, options, position, size, minSize.GetValueOrDefault(new Vector2i(0, 0)),
                    maxSize.GetValueOrDefault(new Vector2i(int.MaxValue, int.MaxValue)), state,
                    parentWindow, blockParentInput);

                windows.Add(window);

                return window;
            }
        }

        public IDesktop CreateDesktop(IDesktop parent, string name)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                return new DefaultDesktop(name, parent);
            }
        }

        public void SetCurrentDesktop(IDesktop desktop)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                currentDesktop = desktop as DefaultDesktop;
            }
        }

        public bool SupportEffect(Type type)
        {
            if (type == typeof(WindowBlend)) return true;
            return false;
        }

        public event Action<IWindowManager> PreRendering
        {
            add
            {
                lock (syncRoot)
                {
                    onPreRendering += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onPreRendering -= value;
                }
            }
        }

        public event Action<IWindowManager> PostRendering
        {
            add
            {
                lock (syncRoot)
                {
                    onPostRendering += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onPostRendering -= value;
                }
            }
        }

        public event Action<IWindowManager> Disposing
        {
            add
            {
                lock (syncRoot)
                {
                    onDisposing += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onDisposing -= value;
                }
            }
        }

        #endregion

        #region IWindowManagerControl Members

        bool IWindowManagerControl.Update()
        {
            // We first process events.
            while (processor.Process() != null) ;
            Render();
            return true;
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            lock (syncRoot)
            {
                Dispose(false);
            }
        }

        #endregion
    }
}
