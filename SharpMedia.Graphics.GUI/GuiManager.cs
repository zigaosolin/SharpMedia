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
using SharpMedia.Graphics.Vector;
using SharpMedia.Input;
using SharpMedia.AspectOriented;
using System.Threading;
using SharpMedia.Graphics.GUI.Widgets.Containers;
using SharpMedia.Math;
using SharpMedia.Graphics.GUI.Widgets;
using System.Collections.ObjectModel;
using SharpMedia.Graphics.GUI.Themes;

namespace SharpMedia.Graphics.GUI
{

    /// <summary>
    /// A GUI root class, manager that "joins" rendering, layout engine and more.
    /// </summary>
    /// <remarks>Input events are sent through IUserInteractive. All actions are immediate and thread safe.
    /// GUI is set to "dirty" state when it needs redrawing. Rendering is done is a seperate thread
    /// whenever GUI is put into dirty state. Rendering is non-blocking.</remarks>
    public sealed class GuiManager : IUserInteractive, IDisposable
    {
        #region Private Members
        object syncRoot = new object();

        // Rendering data.
        IDeviceCanvas canvas;
        volatile bool isDirty = false;

        // Data (must be display).
        IDisplayObject rootObject;
        List<IDisplayObject> auxilaryObjects = new List<IDisplayObject>();

        // Events.
        Action<GuiManager> onRendered;
        Action<GuiManager> onPreRender;

        // Helper property.
        IUserInteractive rootObjectInteractive
        {
            get
            {
                if (rootObject is IUserInteractive) return rootObject as IUserInteractive;
                return null;
            }
        }

        #endregion

        #region Private Methods

        void SetDirty()
        {
            isDirty = true;
        }

        void Changed(IPreChangeNotifier obj)
        {
            lock (syncRoot)
            {
                SetDirty();
            }
        }


        void RenderContents()
        {
            // 1) We first lock them.


            // 2) We process layouting.
            if (rootObject is IWidget)
            {
                // We simply apply bounding rect to it and trigger further processing.
                (rootObject as IWidget).ApplyBoundingRect(canvas, new Vector2f(0, 0), canvas.CanvasUnitSize);
            }

            // 3) We call prerender event.
            using (DeviceLock l = canvas.Device.Lock())
            {
                Action<GuiManager> t = onPreRender;
                if (t != null) t(this);


                canvas.Begin(CanvasRenderFlags.None);

                try
                {
                    // 4) We render them
                    if (rootObject is IDisplayObject)
                    {
                        RenderObject(rootObject as IDisplayObject);
                    }

                    foreach (IDisplayObject obj in auxilaryObjects)
                    {
                        RenderObject(obj);
                    }

                }
                finally
                {
                    canvas.End();
                }

                // 5) We call rendered event.
                t = onRendered;
                if (t != null) t(this);
            }

            // 6) We unlock them.
        }

        /// <summary>
        /// Renders object and children recursevelly.
        /// </summary>
        /// <param name="obj"></param>
        void RenderObject(IDisplayObject obj)
        {
            IGuiRenderer renderer = obj.Skin;

            if (renderer == null) throw new InvalidOperationException();

            renderer.Render(canvas, obj);

            if (obj is IContainer)
            {
                foreach (object child in (obj as IContainer).Children)
                {
                    if (child is IDisplayObject) RenderObject(child as IDisplayObject);
                }
            }
        }

        ~GuiManager()
        {
            Dispose(true);
        }

        void Dispose(bool fin)
        {
            // We also free other resources.
            if (!fin)
            {
                canvas = null;
                rootObject = null;

                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a GUI.
        /// </summary>
        public GuiManager(IDeviceCanvas canvas)
        {
            this.canvas = canvas;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// The event that is fired after GUI was rendered.
        /// </summary>
        /// <remarks>This event executes in rendering thread.</remarks>
        public event Action<GuiManager> Rendered
        {
            add
            {
                lock (syncRoot)
                {
                    onRendered += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onRendered -= value;
                }
            }
        }

        /// <summary>
        /// The event that is fired before GUI is rendered. You should clear render
        /// target asociated with canvas in this step.
        /// </summary>
        /// <remarks>This event executes in rendering thread.</remarks>
        public event Action<GuiManager> PreRendering
        {
            add
            {
                lock (syncRoot)
                {
                    onPreRender += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onPreRender -= value;
                }
            }
        }

        /// <summary>
        /// Updates all animations.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (rootObject is IAnimatable)
            {
                (rootObject as IAnimatable).Update(deltaTime);
            }
        }

        /// <summary>
        /// Renders GUI to render target.
        /// </summary>
        /// <remarks>It may use previous result.</remarks>
        public void Render()
        {
            
            //if (!isDirty) return;

            lock (syncRoot)
            {
                isDirty = false;

                RenderContents();
            }

        }

        /// <summary>
        /// Only signals rendering is needed and returns.
        /// </summary>
        /// <returns></returns>
        public void Invalidate()
        {
            // Forces dirty state.
            isDirty = true;
        }

        /// <summary>
        /// Is GUI in dirty state.
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
        }

        /// <summary>
        /// Gets or sets root object.
        /// </summary>
        public IDisplayObject RootObject
        {
            get
            {
                return rootObject;
            }
            set
            {
                lock (syncRoot)
                {
                    // We first remove event from previous.
                    if (rootObject != null)
                    {
                        if (rootObject is IWidget)
                        {
                            (rootObject as IWidget).SetParent(null, null);
                        }
                        rootObject.OnChange -= Changed;
                    }

                    rootObject = value;

                    // Make sure we reparent it.
                    if (rootObject is IWidget)
                    {
                        (rootObject as IWidget).SetParent(null, this);
                    }

                    // We set new root handler.
                    if (rootObject != null)
                    {
                        rootObject.OnChange += Changed;
                    }

                    SetDirty();
                }
            }
        }

        /// <summary>
        /// Adds non-layouting object.
        /// </summary>
        /// <param name="obj"></param>
        public void AddNLObject(IDisplayObject obj)
        {
            auxilaryObjects.Add(obj);
        }

        /// <summary>
        /// Non-layouting display objects.
        /// </summary>
        public ReadOnlyCollection<IDisplayObject> NLObjects
        {
            get
            {
                return new ReadOnlyCollection<IDisplayObject>(auxilaryObjects);
            }
        }

        /// <summary>
        /// Removes auxilary display object.
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveNLObject(IDisplayObject obj)
        {
            auxilaryObjects.Remove(obj);
        }

        /// <summary>
        /// The canvas.
        /// </summary>
        public IDeviceCanvas Canvas
        {
            get
            {
                return canvas;
            }
            set
            {
                lock (syncRoot)
                {
                    // Resets canvas.
                    canvas = value;
                    SetDirty();
                }
            }
        }

        /// <summary>
        /// The focused object last in group).
        /// </summary>
        public IWidget FocusedWidget
        {
            get
            {
                IDisplayObject root = rootObject;
                if (!(root is IWidget)) return null;

                // We get it.
                IWidget w = root as IWidget;
                while (w is IContainer)
                {
                    IWidget n = (w as IContainer).Focused;
                    if (n != null)
                    {
                        w = n;
                    }
                    else
                    {
                        break;
                    }
                }
                return w;
            }
        }

        #endregion

        #region IUserInteractive Members

        void IUserInteractive.OnPointerEnter(IPointer cursor)
        {
            if (rootObjectInteractive != null)
            {
                rootObjectInteractive.OnPointerEnter(cursor);
            }
        }

        void IUserInteractive.OnPointerMove(IPointer cursor, Vector2f deltaMove)
        {
            if (rootObjectInteractive != null)
            {
                rootObjectInteractive.OnPointerMove(cursor, deltaMove);
            }
        }

        void IUserInteractive.OnPointerLeave(IPointer cursor)
        {
            if (rootObjectInteractive != null)
            {
                rootObjectInteractive.OnPointerLeave(cursor);
            }
        }

        void IUserInteractive.OnPointerPress(IPointer cursor, uint button, InputEventModifier modifiers)
        {
            if (rootObjectInteractive != null)
            {
                rootObjectInteractive.OnPointerPress(cursor, button, modifiers);
            }
        }

        void IUserInteractive.OnPointerRelease(IPointer cursor, uint button)
        {
            if (rootObjectInteractive != null)
            {
                rootObjectInteractive.OnPointerRelease(cursor, button);
            }
        }

        void IUserInteractive.OnWheel(IPointer cursor, float deltaMove)
        {
            if (rootObjectInteractive != null)
            {
                rootObjectInteractive.OnWheel(cursor, deltaMove);
            }
        }

        void IUserInteractive.OnKeyPress(IPointer cursor, KeyCodes code, SharpMedia.Input.KeyboardModifiers modifiers, InputEventModifier eventModifiers)
        {
            IWidget focus = FocusedWidget;
            if (focus != null)
            {
                focus.OnKeyPress(cursor, code, modifiers, eventModifiers);
            }
        }

        void IUserInteractive.OnKeyRelease(IPointer cursor, KeyCodes code, SharpMedia.Input.KeyboardModifiers modifiers)
        {
            IWidget focus = FocusedWidget;
            if (focus != null)
            {
                focus.OnKeyRelease(cursor, code, modifiers);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (syncRoot)
            {
                Dispose(false);
            }
        }

        #endregion
    }
}
