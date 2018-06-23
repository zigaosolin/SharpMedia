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
using System.Collections.ObjectModel;
using SharpMedia.Graphics;
using SharpMedia.Math;
using SharpMedia.Graphics.GUI;

namespace SharpMedia.Shell.Default
{
    /// <summary>
    /// The rendering state of window.
    /// </summary>
    internal enum RenderingState
    {
        Working,
        Updated,
        NeedsUpdate,
        Disposed
    }

    

    /// <summary>
    /// A window implementation
    /// </summary>
    internal sealed class DefaultWindow : IWindow
    {
        #region Private Members
        internal object syncRoot = new object();
        internal DefaultWindowManager manager;

        // Rendering.
        internal List<IWindowEffect> effects = new List<IWindowEffect>();
        internal RenderingState renderingState = RenderingState.Updated;
        internal Guid renderDataId;
        internal TextureView renderData;

        // Back communication.
        internal IWindowBackend listener;
        internal IPointer pointer;
        internal IWindow parentWindow;
        internal bool blockInputToParent;

        // Managment data.
        internal Vector2i position;
        internal Vector2i size;
        internal Vector2i minSize;
        internal Vector2i maxSize;
        internal WindowState windowState;
        internal bool isVisible;
        internal bool isFocused;

        // Data.
        internal string title;
        internal string group;
        internal WindowOptions options;

        // Events
        Action<IWindow> onDisposing;
        #endregion

        #region Internal Methods

        void AssertNotDisposed()
        {
            if (renderingState == RenderingState.Disposed) throw new ObjectDisposedException("Window already disposed.");
        }

        void Dispose(bool fin)
        {
            if (renderingState == RenderingState.Disposed) return;

            manager.WindowDisposed(this);
            renderData.Dispose();

            if (!fin)
            {
                GC.SuppressFinalize(this);
                renderData = null;
                listener = null;
                effects = null;
                manager = null;
            }

            Action<IWindow> t = onDisposing;
            if (t != null)
            {
                t(this);
            }
        }

        ~DefaultWindow()
        {
            Dispose(true);
        }

        void AdjustSize()
        {
            if (size.X > maxSize.X) size.X = maxSize.X;
            if (size.Y > maxSize.Y) size.Y = maxSize.Y;
            if (size.X < minSize.X) size.X = minSize.X;
            if (size.Y < minSize.Y) size.Y = minSize.Y;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        public DefaultWindow(DefaultWindowManager manager, Guid resourceId, string title, string windowGroup, 
                             IWindowBackend listener, WindowOptions options, Vector2i position, Vector2i size,
                             Vector2i minSize, Vector2i maxSize, WindowState windowState, IWindow parentWindow,
                            bool blockParentInput)
        {
            this.manager = manager;
            this.renderDataId = resourceId;
            this.title = title;
            this.group = windowGroup;
            this.listener = listener;
            this.options = options;
            this.position = position;
            this.size = size;
            this.minSize = minSize;
            this.maxSize = maxSize;
            this.windowState = windowState;
            this.blockInputToParent = blockParentInput;
            this.parentWindow = parentWindow;

            TypelessTexture texture = manager.Device.GetShared(resourceId);

            // We create texture view upon it.
            this.renderData = (texture as TypelessTexture2D).CreateTexture();
        }

        #endregion

        #region IWindow Members

        public bool AttachEffect(IWindowEffect effect)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                if (!manager.SupportEffect(effect.GetType())) return false;
                effects.Add(effect);

                return true;
            }
        }

        public ReadOnlyCollection<IWindowEffect> WindowEffects
        {
            get { return new ReadOnlyCollection<IWindowEffect>(effects); }
        }

        public bool DetachEffect(IWindowEffect effect)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                return effects.Remove(effect);
            }
        }

        public void ChangeSource(Guid source, SharpMedia.Math.Vector2i? newSize)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                if (newSize.HasValue)
                {
                    size = newSize.Value;
                    AdjustSize();
                }

                renderData.Dispose();

                renderDataId = source;
                renderData = (manager.Device.GetShared(source) as TypelessTexture2D).CreateTexture();
            }
        }

        public void SetZOrder(IWindow reference, bool after)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                manager.SetZOrder(this, reference, after);
            }
        }

        public void Minimize()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                windowState |= WindowState.Minimized;
            }
        }

        public void Maximize()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                windowState |= WindowState.Maximized;
            }
        }

        public void Working()
        {
            renderingState = RenderingState.Working;
        }

        public void Rendered()
        {
            renderingState = RenderingState.NeedsUpdate;
        }

        public SharpMedia.Math.Vector2i Position
        {
            get
            {
                return position;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    position = value;
                }
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    title = value;
                }
            }
        }

        public WindowOptions Options
        {
            get
            {
                return options;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    options = value;
                }
            }
        }

        public SharpMedia.Math.Vector2i Size
        {
            get
            {
                return maxSize;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    size = value;

                    AdjustSize();
                }
            }
        }

        public SharpMedia.Math.Vector2i MaxSize
        {
            get
            {
                return maxSize;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    maxSize = value;

                    AdjustSize();
                }
            }
        }

        public SharpMedia.Math.Vector2i MinSize
        {
            get
            {
                return minSize;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    minSize = value;
                    AdjustSize();
                }
            }
        }

        public WindowState State
        {
            get
            {
                return windowState;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    bool prevFs = (windowState & WindowState.Fullscreen) != 0;
                    windowState = value;
                    bool currFs = (windowState & WindowState.Fullscreen) != 0;
                    if(prevFs != currFs)
                    {
                        manager.WindowFullscreened(this, currFs);
                    }
                    
                }
            }
        }


        public event Action<IWindow> Disposing
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

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                lock (syncRoot)
                {
                    isVisible = value;
                }
            }
        }

        public bool HasFocus
        {
            get
            {
                return isFocused;
            }
            set
            {
                lock (syncRoot)
                {
                    isFocused = true;
                    manager.SetFocused(this);
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (syncRoot)
            {
                Dispose(true);
            }
        }

        #endregion

    }
}
