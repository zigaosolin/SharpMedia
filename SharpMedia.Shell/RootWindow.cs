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
using SharpMedia.Graphics.GUI;
using SharpMedia.Input;
using SharpMedia.Math;
using System.Collections.ObjectModel;
using SharpMedia.Graphics;
using SharpMedia.Graphics.GUI.Widgets;
using SharpMedia.Graphics.Vector;
using SharpMedia.Graphics.GUI.Themes;

namespace SharpMedia.Shell
{

    /// <summary>
    /// A helper class that mixes window and GUI.
    /// </summary>
    public class RootWindow : IWindowBackend, IDisposable
    {
        #region Private Members
        object syncRoot = new object();
        GuiManager root;
        IWindow window;
        GraphicsDevice graphicsDevice;
        
        // Other properties.
        string titleGroup;
        #endregion

        #region Private Members

        void PreRenderingInternal(GuiManager obj)
        {
            graphicsDevice.Clear(obj.Canvas.Target, Colour.Black);
        }

        void RenderedInternal(GuiManager obj)
        {
            // We were updated.
            window.Rendered();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a client window.
        /// </summary>
        public RootWindow(GraphicsDevice graphicsDevice, IWindowManager windowManager, string title, string titleGroup,
            WindowOptions windowOptions, Vector2i position, Vector2i size, Vector2i? minSize, Vector2i? maxSize, 
            WindowState windowState, IWidget widget, uint multisampleCount, uint multisampleQuality)
        {
            // 1) Copy data.
            this.graphicsDevice = graphicsDevice;
            
            // 2) We create render target.
            TypelessTexture2D texture = new TypelessTexture2D(graphicsDevice, Usage.Default, TextureUsage.RenderTarget | TextureUsage.Texture,
                CPUAccess.None, PixelFormat.Parse("R.UN8 G.UN8 B.UN8 A.UN8"), (uint)size.X, (uint)size.Y, 1,
                multisampleCount, multisampleQuality, GraphicsLocality.DeviceOrSystemMemory, null);
            texture.DisposeOnViewDispose = true;

            Guid shareGuid = graphicsDevice.RegisterShared(texture, TextureUsage.Texture);

            RenderTargetView renderTarget = texture.CreateRenderTarget(
                PixelFormat.Parse("R.UN8 G.UN8 B.UN8 A.UN8"));

            window = windowManager.CreateWindow(shareGuid, title, titleGroup, this, windowOptions,
                position, size, minSize, maxSize, windowState, null, false);

            // 3) We create graphics canvas.
            GraphicsCanvas canvas = new GraphicsCanvas(graphicsDevice, renderTarget, new Vector2f(1, 1));
            
            // 4) We create GUI manager.
            GuiManager manager = new GuiManager(canvas);
            manager.RootObject = widget;
            manager.PreRendering += new Action<GuiManager>(PreRenderingInternal);
            manager.Rendered += new Action<GuiManager>(RenderedInternal);

            this.root = manager;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Position of window.
        /// </summary>
        public Vector2i Position
        {
            get
            {
                return window.Position;
            }
            set
            {
                window.Position = value;
            }
        }

        /// <summary>
        /// Is window visible.
        /// </summary>
        public bool IsVisible
        {
            get { return window.IsVisible; }
            set { window.IsVisible = value; }
        }

        /// <summary>
        /// Size of window.
        /// </summary>
        public Vector2i Size
        {
            get
            {
                return window.Size;
            }
            set
            {

                window.Size = value;
                
            }
        }

        /// <summary>
        /// Window state.
        /// </summary>
        public WindowState WindowState
        {
            get
            {
                return window.State;
            }
            set
            {
                window.State = value;
            }
        }

        /// <summary>
        /// Window options.
        /// </summary>
        public WindowOptions WindowOptions
        {
            get
            {
                return window.Options;
            }
            set
            {
                window.Options = value;
            }
        }

        /// <summary>
        /// Title window.
        /// </summary>
        public string Title
        {
            get
            {
                return window.Title;
            }
            set
            {
                window.Title = value;
            }
        }

        /// <summary>
        /// Group of window.
        /// </summary>
        public string Group
        {
            get
            {
                return titleGroup;
            }
        }

        /// <summary>
        /// Minimum size of window.
        /// </summary>
        public Vector2i MinSize
        {
            get
            {
                return window.MinSize;
            }
            set
            {
                window.MinSize = value;
            }
        }

        /// <summary>
        /// Maximum size of window.
        /// </summary>
        public Vector2i MaxSize
        {
            get
            {
                return window.MaxSize;
            }
            set
            {
                window.MaxSize = value;
            }
        }

        /// <summary>
        /// Does window have focus.
        /// </summary>
        public bool HasFocus
        {
            get
            {
                return window.HasFocus;
            }
            set
            {
                window.HasFocus = value;
            }
        }

        /// <summary>
        /// Attached window effects.
        /// </summary>
        public ReadOnlyCollection<IWindowEffect> Effects
        {
            get
            {
                return window.WindowEffects;
            }
        }

        /// <summary>
        /// Gui manager associated.
        /// </summary>
        public GuiManager GuiManager
        {
            get { return root; }
        }

        /// <summary>
        /// Obtains the "wrapped" window.
        /// </summary>
        public IWindow Window
        {
            get
            {
                return window;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void Close()
        {
            window.Dispose();
        }

        /// <summary>
        /// Indicates that window is not frozen but something is being processed.
        /// </summary>
        public void Working()
        {
            window.Working();
        }

        /// <summary>
        /// Attaches a specific effect.
        /// </summary>
        /// <param name="effect"></param>
        public void AttachEffect(IWindowEffect effect)
        {
            window.AttachEffect(effect);
        }

        /// <summary>
        /// Detaches an effect.
        /// </summary>
        /// <param name="effect"></param>
        public void DetachEffect(IWindowEffect effect)
        {
            window.DetachEffect(effect);
        }

        /// <summary>
        /// Detaches all effects.
        /// </summary>
        public void DetachAllEffects()
        {
            foreach (IWindowEffect effect in window.WindowEffects)
            {
                DetachEffect(effect);
            }
        }

        /// <summary>
        /// Sets Z-order. Reference may be null (representing all windows).
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="after"></param>
        public void SetZOrder(IWindow reference, bool after)
        {
            window.SetZOrder(reference, after);
        }

        /// <summary>
        /// Brings window to front.
        /// </summary>
        public void BringToFront()
        {
            window.SetZOrder(null, true);
        }

        /// <summary>
        /// Brings window to background.
        /// </summary>
        public void BringToBack()
        {
            window.SetZOrder(null, false);
        }

        /// <summary>
        /// Minimizes the window.
        /// </summary>
        public void Minimize()
        {
            window.Minimize();
        }

        /// <summary>
        /// Maximizes the window.
        /// </summary>
        public void Maximize()
        {
            window.Maximize();
        }


        #region Events

        #endregion

        #endregion

        #region IWindowBackend Members

        void IWindowBackend.Resized(SharpMedia.Math.Vector2i newSize)
        {
            lock (syncRoot)
            {
                // We resize render target.
                RenderTargetView currentRenderTarget = root.Canvas.Target;
                RenderTargetView newRenderTarget = currentRenderTarget.CloneSameType(currentRenderTarget.Format,
                    (uint)newSize.X, (uint)newSize.Y, 1, 1, currentRenderTarget.MipmapCount) as RenderTargetView;

                root.Canvas.Target = newRenderTarget;

                // We unregister it and register new.
                graphicsDevice.UnRegisterShared(currentRenderTarget.TypelessResource as TypelessTexture);
                window.ChangeSource(graphicsDevice.RegisterShared(newRenderTarget.TypelessResource as TypelessTexture,
                    TextureUsage.Texture), newSize);

                // Current render target not used anymore.
                currentRenderTarget.Dispose();
            }
        }

        void IWindowBackend.Closed()
        {
            lock (syncRoot)
            {
                (this as IDisposable).Dispose();
            }
        }

        void IWindowBackend.Minimized()
        {
        }

        void IWindowBackend.Maximized()
        {
        }

        void IWindowBackend.GainFocus()
        {
        }

        void IWindowBackend.LostFocus()
        {
        }

        void IWindowBackend.Fullscreened()
        {
        }

        #endregion

        #region IUserInteractive Members

        void IUserInteractive.OnPointerEnter(IPointer cursor)
        {
            (root as IUserInteractive).OnPointerEnter(cursor);
        }

        void IUserInteractive.OnPointerLeave(IPointer cursor)
        {
            (root as IUserInteractive).OnPointerLeave(cursor);
        }

        void IUserInteractive.OnPointerPress(IPointer cursor, uint button, InputEventModifier modifiers)
        {
            (root as IUserInteractive).OnPointerPress(cursor, button, modifiers);
        }

        void IUserInteractive.OnPointerRelease(IPointer cursor, uint button)
        {
            (root as IUserInteractive).OnPointerRelease(cursor, button);
        }

        void IUserInteractive.OnWheel(IPointer cursor, float deltaWheel)
        {
            (root as IUserInteractive).OnWheel(cursor, deltaWheel);
        }

        void IUserInteractive.OnPointerMove(IPointer cursor, SharpMedia.Math.Vector2f mouseDeltaMove)
        {
            (root as IUserInteractive).OnPointerMove(cursor, mouseDeltaMove);
        }

        void IUserInteractive.OnKeyPress(IPointer cursor, KeyCodes code, KeyboardModifiers modifiers, InputEventModifier eventModifiers)
        {
            (root as IUserInteractive).OnKeyPress(cursor, code, modifiers, eventModifiers);
        }

        void IUserInteractive.OnKeyRelease(IPointer cursor, KeyCodes code, KeyboardModifiers modifiers)
        {
            (root as IUserInteractive).OnKeyRelease(cursor, code, modifiers);
        }

        #endregion

        #region IDisposable Members

        ~RootWindow()
        {
            (this as IDisposable).Dispose();
        }

        void IDisposable.Dispose()
        {
            lock (syncRoot)
            {
                if (root != null)
                {
                    root.Dispose();
                    window.Dispose();

                    window = null;
                    root = null;
                }
            }
        }

        #endregion
    }
}
