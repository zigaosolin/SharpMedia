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
using SharpMedia.Input;
using SharpMedia.Math;
using SharpMedia.Graphics.Vector;
using SharpMedia.Math.Shapes;
using SharpMedia.Graphics.GUI.Styles;
using SharpMedia.Graphics.GUI.Themes;
using SharpMedia.Graphics.GUI.Widgets;

namespace SharpMedia.Graphics.GUI.Standalone
{

    /// <summary>
    /// A Gui pointer implementation.
    /// </summary>
    public sealed class GuiPointer : IPointer, IDisplayObject
    {
        #region Private Members
        object syncRoot = new object();
        GuiManager manager;
        InputDevice mouse;
        IUserInteractive interactive { get { return manager; } }
        Sensitivity sensitivity;

        // Canvas relative data.
        Vector2f position = Vector2f.Zero;
        float wheelPosition = 0.0f;

        // Gui attached data.
        List<AttachedData> attachedData = new List<AttachedData>();
        Action<IPreChangeNotifier> onChange;
        object cursor = new object();

        // Rendering.
        IGuiRenderer renderer;
        Style style;
        StyleAnimationController styleState = new StyleAnimationController();
        #endregion

        #region Routing Events

        void OnChanged()
        {
            Action<IPreChangeNotifier> t = onChange;
            if (t != null)
            {
                t(this);
            }
        }

        void ClampPositionAndSet(Vector2f position)
        {
            // We first bound it to region.
            if (position.X < 0.0f) position.X = 0.0f;
            if (position.Y < 0.0f) position.Y = 0.0f;

            Vector2f canvasSize = manager.Canvas.CanvasUnitSize;
            if (position.X > canvasSize.X) position.X = canvasSize.X;
            if (position.Y > canvasSize.Y) position.Y = canvasSize.Y;

            this.position = position;
        }

        void MouseAxis(InputEvent ev)
        {
            if (ev.AxisId == 2)
            {

                float move = ev.DeltaAxisState * sensitivity.CalcWheelSensitivity(manager.Canvas);
                wheelPosition += move;

                // We now fire events.
                interactive.OnWheel(this, move);
            }
        }

        void CursorMove(InputEvent ev)
        {
            if (ev.AxisId == 0)
            {
                float cpos = manager.Canvas.CanvasUnitSize.X * (float)ev.AxisState 
                    / (float)manager.Canvas.CanvasPixelSize.X;

                float cmove = manager.Canvas.CanvasUnitSize.X * ev.DeltaAxisState /
                    manager.Canvas.CanvasPixelSize.X;

                ClampPositionAndSet(new Vector2f(cpos, position.Y));

                // We can now fire event, may fire subevents (enter/move).
                interactive.OnPointerMove(this, new Vector2f(cmove, 0));
            }
            else if (ev.AxisId == 1)
            {
                float cpos = manager.Canvas.CanvasUnitSize.Y * (float)ev.AxisState
                    / (float)manager.Canvas.CanvasPixelSize.Y;

                float cmove = manager.Canvas.CanvasUnitSize.Y * ev.DeltaAxisState /
                    manager.Canvas.CanvasPixelSize.Y;

                ClampPositionAndSet(new Vector2f(position.X, cpos));

                // We can now fire event, may fire subevents (enter/move).
                interactive.OnPointerMove(this, new Vector2f(0, cmove));
            }

            
            

        }


        void ButtonUp(InputEvent ev)
        {
            interactive.OnPointerRelease(this, ev.ButtonId);
        }

        void ButtonDown(InputEvent ev)
        {
            interactive.OnPointerPress(this, ev.ButtonId, ev.EventModifiers);
        }

        #endregion

        #region Internal Members

        /// <summary>
        /// The mouse.
        /// </summary>
        /// <param name="mouse"></param>
        public GuiPointer(GuiManager manager,
            EventProcessor processor, Style style, 
            IGuiRenderer renderer, Sensitivity sensitivity)
        {
            this.manager = manager;
            this.mouse = processor.EventPump.PrimaryMouse;
            this.sensitivity = sensitivity;
            this.style = style;
            this.renderer = renderer;

       
            processor.MouseButtonDown += ButtonDown;
            processor.MouseButtonUp += ButtonUp;
            processor.MouseAxis += MouseAxis;
            processor.CursorMoved += CursorMove;

            // We fire mouse enter event immediatelly.
            interactive.OnPointerEnter(this);
        }

        #endregion

        #region IPointer Members

        public SharpMedia.Math.Vector2f CanvasPosition
        {
            get
            {
                return position;
            }
            set
            {
                lock (syncRoot)
                {
                    ClampPositionAndSet(value);
                }
                OnChanged();
            }
        }

        public SharpMedia.Math.Vector2f GetRelative(IDisplayObject obj)
        {
            // We convert to object's rectangle.
            Vector2f rightBottom, topRight;
            GetBoundingBox(out rightBottom, out topRight);

            throw new NotImplementedException();
        }

        public bool IsButtonDown(uint id)
        {
            return mouse[id];
        }

        public float WheelPosition
        {
            get { return wheelPosition; }
        }

        public void AttachData(AttachedData data)
        {
            lock (syncRoot)
            {
                attachedData.Add(data);
            }

            OnChanged();
        }

        public void DetachData(AttachedData data)
        {
            lock (syncRoot)
            {
                attachedData.Remove(data);
            }

            OnChanged();
        }

        public void DetachAllData()
        {
            lock (syncRoot)
            {
                attachedData.Clear();
            }
            OnChanged();
        }

        public object Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                lock (syncRoot)
                {
                    cursor = value;
                }
                OnChanged();
            }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<AttachedData> AttachedData
        {
            get { return new System.Collections.ObjectModel.ReadOnlyCollection<AttachedData>(attachedData); }
        }

        #endregion

        #region Style

        /// <summary>
        /// A Gui pointer style.
        /// </summary>
        public class GuiPointerStyle : Area.AreaStyle
        {
            public GuiPointerStyle()
                : base()
            {
            }
        }

        #endregion

        #region IDisplayObject Members

        public IGuiRenderer Skin
        {
            get { return renderer; }
            set { renderer = value; }
        }

        public Style Style
        {
            get { return style; }
            set { style = value; }
        }

        public StyleAnimationController StyleAnimation
        {
            get { return styleState; }
        }

        public uint ZOrder
        {
            get { return uint.MaxValue; }
        }

        #endregion

        #region IPositionable Members

        public void GetBoundingBox(out Vector2f rightBottom, out Vector2f leftTop)
        {
            rightBottom = position;
            leftTop = position;
        }

        public SharpMedia.Math.Shapes.IPathf Outline
        {
            get { return null /*new Pointf(position)*/; }
        }

        public SharpMedia.Math.Shapes.IShapef Shape
        {
            get { return new Pointf(position); }
        }

        #endregion

        #region IPreChangeNotifier Members

        public event Action<IPreChangeNotifier> OnChange
        {
            add
            {
                lock (syncRoot)
                {
                    onChange += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onChange -= value;
                }
            }
        }

        #endregion

        #region IDisplayObject Members

        public bool IsStyleCompatible(Style style)
        {
            return true;
        }

        public IDisplayObject[] SubDisplayObjects
        {
            get { return null; }
        }

        #endregion

        #region IPositionable Members

        void IPositionable.ApplyBoundingRect(ICanvas canvas, Vector2f minPosition, Vector2f maxPosition)
        {
            throw new InvalidOperationException();
        }

        #endregion
    }
}
