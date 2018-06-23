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
using SharpMedia.Math;
using SharpMedia.Math.Shapes;
using SharpMedia.Graphics.Vector;
using SharpMedia.Components;
using SharpMedia.Input;
using System.Threading;
using SharpMedia.Graphics.GUI.Metrics;
using SharpMedia.Graphics.GUI.Animations;
using SharpMedia.Graphics.GUI.Widgets.Containers;
using SharpMedia.Graphics.GUI.Styles;
using SharpMedia.Math.Interpolation;
using SharpMedia.Graphics.GUI.Styles.SubStyles;
using System.Runtime.Serialization;
using SharpMedia.Graphics.Vector.Fonts;
using System.Collections.ObjectModel;

namespace SharpMedia.Graphics.GUI.Widgets
{

    /// <summary>
    /// A change context helper, for using pattern.
    /// </summary>
    /// <example>
    /// <code>
    /// using(someWidget.Enter())
    /// {
    ///    // someWidget is mutable
    /// }
    /// // Immutable once again
    /// </code>
    /// </example>
    public class ChangeContext : IDisposable
    {
        #region Private Members
        Area area;

        internal ChangeContext(Area area)
        {
            this.area = area;
        }
        #endregion

        #region IDisposable

        public void Dispose()
        {
            area.ExitChange();
        }

        #endregion
    }

    /// <summary>
    /// A possibly bordered area on a GUI canvas
    /// </summary>
    /// <remarks>Area supports Normal, MouseOver and MouseDown styles.</remarks>
    [Serializable]
    public class Area : IWidget, ISerializable
    {
        #region Private Members
        protected object syncRoot = new object();
        protected uint mutableCount = 0;

        // Rendering.
        protected Style style;
        protected Themes.IGuiRenderer renderer;
        protected uint zOrder = 0;
        protected bool isVisible = true;

        // Parents.
        protected IWidget parent;
        protected GuiManager manager;

        // Description.
        protected string description;
        protected float containedDescriptionShowUpTime = float.PositiveInfinity;
        protected LayoutAnchor containedDescriptionPositioning = LayoutAnchor.None;
        protected IDisplayObject containedDescriptionObject;


        // Layouting & positioning.
        protected LayoutAnchor anchor = LayoutAnchor.None;
        protected GuiVector2 minSize;
        protected GuiVector2 maxSize;
        protected GuiVector2 preferredSize;
        protected GuiRect preferredRect;
        protected GuiScalar marginLeft;
        protected GuiScalar marginRight;
        protected GuiScalar marginTop;
        protected GuiScalar marginBottom;
        protected bool preserveRatio = false;

        // Positioning.
        protected Vector2f boundingRectMin = Vector2f.Zero;
        protected Vector2f boundingRectMax = Vector2f.Zero;
        protected IShapef shape = new Rectf(Vector3f.Zero, Vector3f.Zero, Vector3f.Zero);
        protected IPathf outline = new Rectf(Vector3f.Zero, Vector3f.Zero, Vector3f.Zero);

        // Animation.
        protected StyleAnimationController animationState = new StyleAnimationController();
        protected List<AnimationProcess> activeAnimations = new List<AnimationProcess>();

        // Events.
        protected AreaEvents eventHolder;
        protected Action<IPreChangeNotifier> onChange;
        protected Action2<Area, IPointer> onMouseOver;
        protected Action2<Area, IPointer> onMouseLeave;
        protected Action4<Area, IPointer, uint, InputEventModifier> onMousePressed;
        protected Action3<Area, IPointer, uint> onMouseReleased;
        protected Action3<Area, IPointer, float> onWheel;
        protected Action3<Area, IPointer, Vector2f> onMouseMove;
        protected Action<Area> onFocusGain;
        protected Action<Area> onFocusLost;
        protected Action5<Area, IPointer, KeyCodes, KeyboardModifiers, InputEventModifier> onKeyPressed;
        protected Action4<Area, IPointer, KeyCodes, KeyboardModifiers> onKeyReleased;
        protected Action2<Area, IWidget> onDescriptionShow;
        protected Action2<Area, IWidget> onDescriptionHide;
        protected Action3<Area, StyleState, StyleState> onStyleStateChange;

        // Serialization
        protected WidgetSerialization serializationOptions = WidgetSerialization.Default;

        internal void EnsureMutability(IPreChangeNotifier notifier)
        {
            if (mutableCount == 0)
            {
                throw new InvalidOperationException("The area is not in mutable state, cannot change it or any of its members.");
            }
        }

        protected internal void AssertMutable()
        {
            if (mutableCount == 0)
            {
                throw new InvalidOperationException("The area is not in mutable state, cannot change it or any of its members.");
            }
        }

        /// <summary>
        /// No dummy contructor.
        /// </summary>
        /// <param name="dummy"></param>
        protected Area(object dummy)
        {
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Creates an area.
        /// </summary>
        public Area()
        {
            eventHolder = new AreaEvents(this);

            InitializeInternal();
        }

        /// <summary>
        /// Area with preferred rect initialization.
        /// </summary>
        /// <param name="preferredRect"></param>
        public Area(GuiRect preferredRect)
            : this()
        {
            this.preferredRect = preferredRect;
        }

        /// <summary>
        /// Area with preferred rect and style.
        /// </summary>
        /// <param name="preferredRect"></param>
        /// <param name="style"></param>
        public Area(GuiRect preferredRect, Style style)
            : this(preferredRect)
        {
            if (!IsStyleCompatible(style))
            {
                throw new ArgumentException("Style is not compatible with area.");
            }
            this.style = style;
        }

        /// <summary>
        /// Area with anchor.
        /// </summary>
        /// <param name="anchor"></param>
        public Area(LayoutAnchor anchor)
            : this()
        {
            this.anchor = anchor;
        }

        /// <summary>
        /// Area with anchor and style.
        /// </summary>
        /// <param name="anchor"></param>
        public Area(LayoutAnchor anchor, Style style)
            : this(anchor)
        {
            if (!IsStyleCompatible(style))
            {
                throw new ArgumentException("Style is not compatible with area.");
            }
            this.style = style;
        }

        /// <summary>
        /// Constructor with preferred size.
        /// </summary>
        /// <param name="preferredSize"></param>
        public Area(GuiVector2 preferredSize)
        {
            this.preferredSize = preferredSize;
        }

        /// <summary>
        /// Constructor with preferred size.
        /// </summary>
        /// <param name="preferredSize"></param>
        public Area(GuiVector2 preferredSize, Style style)
            : this(preferredSize)
        {
            if (!IsStyleCompatible(style))
            {
                throw new ArgumentException("Style is not compatible with area.");
            }
            this.style = style;
        }

        /// <summary>
        /// Constructor with preferred size.
        /// </summary>
        /// <param name="preferredSize"></param>
        public Area(GuiVector2 preferredSize, GuiVector2 minSize, GuiVector2 maxSize)
            : this(preferredSize)
        {
            this.minSize = minSize;
            this.maxSize = maxSize;
        }

        /// <summary>
        /// Constructor with preferred size.
        /// </summary>
        /// <param name="preferredSize"></param>
        public Area(GuiVector2 preferredSize, GuiVector2 minSize, GuiVector2 maxSize, Style style)
            : this(preferredSize, minSize, maxSize)
        {
            if (!IsStyleCompatible(style))
            {
                throw new ArgumentException("Style is not compatible with area.");
            }
            this.style = style;
        }

        #endregion

        #region Protected Overrides

        protected virtual void OnSetFocusInternal()
        {
        }

        protected virtual void OnLostFocusInternal()
        {
        }

        /// <summary>
        /// This is called at construction and allows initialization for auto-generated classes.
        /// </summary>
        protected virtual void InitializeInternal()
        {
        }

        protected virtual bool IsStyleCompatible(Style style)
        {
            return style.Type == typeof(AreaStyle);
        }

        protected virtual Vector2f GetPreferredSizeInternal(ICanvas canvas, IPositionable parent)
        {
            return preferredSize.ToParentSize(canvas, parent);
        }

        protected virtual Vector2f GetRequiredMinimumSizeInternal(ICanvas canvas, IPositionable parent)
        {
            return minSize.ToParentSize(canvas, parent);
        }

        protected virtual Vector2f GetRequiredMaximumSizeInternal(ICanvas canvas, IPositionable parent)
        {
            return maxSize.ToParentSize(canvas, parent);
        }

        protected virtual void ApplyBoundingRectInternal(ICanvas canvas, 
            Vector2f minPosition, Vector2f maxPosition)
        {
            Rectf rect = new Rectf(minPosition, maxPosition);
            this.shape = rect;
            this.outline = rect;
        }

        protected virtual void OnPointerEnterInternal(IPointer cursor)
        {
            animationState.TransistTo(CommonStyleStates.PointerOver);
        }

        protected virtual void OnPointerLeaveInternal(IPointer cursor)
        {
            animationState.TransistTo(CommonStyleStates.Normal);
        }

        protected virtual void OnPointerMoveInternal(IPointer cursor, Vector2f deltaMove)
        {
        }

        protected virtual void OnPointerPressInternal(IPointer cursor, uint button, InputEventModifier modifiers)
        {
        }

        protected virtual void OnPointerReleaseInternal(IPointer cursor, uint button)
        {
        }

        protected virtual void OnWheelInternal(IPointer cursor, float deltaWheel)
        {
        }

        protected virtual void OnKeyPressInternal(IPointer cursor, KeyCodes code, 
            KeyboardModifiers modifiers, InputEventModifier eventModifiers)
        {
        }

        protected virtual void OnKeyReleaseInternal(IPointer cursor, KeyCodes code, KeyboardModifiers modifiers)
        {
        }

        protected virtual void UpdateAnimationsInternal(float deltaTime)
        {
            animationState.Animate(deltaTime);

            // It is thread safe, locked outside.
            for (int i = 0; i < activeAnimations.Count; i++)
            {
                activeAnimations[i].Update(deltaTime, this);

                if (activeAnimations[i].IsOver)
                {
                    activeAnimations.RemoveAt(i--);
                }
            }
        }

        protected virtual bool GetPreferredRectInternal(ICanvas canvas, IPositionable parent, out Vector2f leftBottom, out Vector2f rightTop)
        {
            if (preferredRect == null)
            {
                leftBottom = rightTop = Vector2f.Zero;
                return false;
            }
            else
            {
                preferredRect.ToCanvasRect(canvas, parent, out leftBottom, out rightTop);
                return true;
            }
        }

        protected virtual Vector4f GetMarginInternal(ICanvas canvas, IPositionable parent)
        {
            return new Vector4f(marginLeft.ToCanvasSize(canvas, parent), marginRight.ToCanvasSize(canvas, parent),
                marginTop.ToCanvasSize(canvas, parent), marginBottom.ToCanvasSize(canvas, parent));
        }


        #endregion

        #region Properties

        /// <summary>
        /// Events getter.
        /// </summary>
        public AreaEvents Events
        {
            get
            {
                return eventHolder;
            }
        }

        /// <summary>
        /// Is layouting ratio preserved.
        /// </summary>
        public bool PreserveRatio
        {
            get
            {
                return preserveRatio;
            }
            set
            {
                AssertMutable();
                preserveRatio = value;
            }
        }

        /// <summary>
        /// Is the widget visible.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                AssertMutable();
                isVisible = value;
            }
        }

        /// <summary>
        /// Obtains GUI manager.
        /// </summary>
        /// <remarks>This may be null if not linked to manager.</remarks>
        public GuiManager Manager
        {
            get
            {
                return manager;
            }
        }

        /// <summary>
        /// Description of widget, if not overwritten, it is used by contained descriptions.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { AssertMutable(); description = value; }
        }

        /// <summary>
        /// When should contained description be invoked, default is float.PositiveInfinite.
        /// </summary>
        public float ContainedDescriptionShowUpTime
        {
            get { return containedDescriptionShowUpTime; }
            set { AssertMutable(); ContainedDescriptionShowUpTime = value; }
        }

        /// <summary>
        /// How is the description positioned.
        /// </summary>
        public LayoutAnchor ContainedDescriptionPositioning
        {
            get
            {
                return containedDescriptionPositioning;
            }
            set
            {
                AssertMutable();
                containedDescriptionPositioning = value;
            }
        }

        /// <summary>
        /// Contained description custumizable display object.
        /// </summary>
        public IDisplayObject ContainedDescriptionObject
        {
            get
            {
                return containedDescriptionObject;
            }
            set
            {
                AssertMutable();
                containedDescriptionObject = value;
            }
        }

        /// <summary>
        /// The style.
        /// </summary>
        public Style Style
        {
            set
            {
                AssertMutable();

                if (value != null && !this.IsStyleCompatible(value))
                {
                    throw new ArgumentException(string.Format(
                        "Style is not compatible with type {0}", this.GetType()));
                }

                if (style != null)
                {
                    style.OnChange -= EnsureMutability;
                }

                style = value;

                if (style != null)
                {
                    style.OnChange += EnsureMutability;
                }
            }
            get
            {
                return style;
            }
        }

        /// <summary>
        /// Skin, the renderer of widget.
        /// </summary>
        public Themes.IGuiRenderer Skin
        {
            set
            {
                AssertMutable();

                renderer = value;
            }
            get
            {
                return renderer;
            }
        }

        /// <summary>
        /// Z-order of widget.
        /// </summary>
        public uint ZOrder
        {
            set 
            { 
                AssertMutable();
                zOrder = value; 
            }
            get { return zOrder; }
        }

        /// <summary>
        /// Layouting anchor.
        /// </summary>
        public LayoutAnchor LayoutAnchor
        {
            get { return anchor; }
            set 
            { 
                AssertMutable();
                anchor = value; 
            }
        }

        /// <summary>
        /// Preferred size of area, used by some layouts.
        /// </summary>
        public GuiVector2 PreferredSize
        {
            get { return preferredSize; }
            set { AssertMutable(); preferredSize = value; }
        }

        /// <summary>
        /// Minimum size of area, used by some layouts.
        /// </summary>
        public GuiVector2 MinimumSize
        {
            get { return minSize; }
            set { AssertMutable(); minSize = value; }
        }

        /// <summary>
        /// Maximum size of area, used by some layouts.
        /// </summary>
        public GuiVector2 MaximumSize
        {
            get { return maxSize; }
            set { AssertMutable(); maxSize = value; }
        }

        /// <summary>
        /// Preferred rectangle, used by some layouts.
        /// </summary>
        public GuiRect PreferredRect
        {
            get { return preferredRect; }
            set { AssertMutable(); preferredRect = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// An area event collection
        /// </summary>
        public class AreaEvents
        {
            #region Protected Members
            internal protected object syncRoot = new object();
            internal protected Area parent;
            internal protected AreaEvents(Area parent)
            {
                this.parent = parent;
            }
            #endregion

            #region Events

            /// <summary>
            /// A mouse over event.
            /// </summary>
            public event Action2<Area, IPointer> MouseOver
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onMouseOver += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onMouseOver -= value;
                    }
                }
            }

            /// <summary>
            /// A mouse leave event.
            /// </summary>
            public event Action2<Area, IPointer> MouseLeave
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onMouseLeave += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onMouseLeave -= value;
                    }
                }
            }

            /// <summary>
            /// A mouse wheel event.
            /// </summary>
            public event Action3<Area, IPointer, float> MouseWheel
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onWheel += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onWheel -= value;
                    }
                }
            }

            /// <summary>
            /// A mouse move event.
            /// </summary>
            public event Action3<Area, IPointer, Vector2f> MouseMove
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onMouseMove += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onMouseMove -= value;
                    }
                }
            }

            /// <summary>
            /// A mouse pressed event.
            /// </summary>
            public event Action4<Area, IPointer, uint, InputEventModifier> MousePressed
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onMousePressed += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onMousePressed -= value;
                    }
                }
            }

            /// <summary>
            /// A mouse release event.
            /// </summary>
            public event Action3<Area, IPointer, uint> MouseRelease
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onMouseReleased += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onMouseReleased -= value;
                    }
                }
            }

            /// <summary>
            /// A key press event.
            /// </summary>
            public event Action5<Area, IPointer, KeyCodes, KeyboardModifiers, InputEventModifier> KeyPress
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onKeyPressed += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onKeyPressed -= value;
                    }
                }
            }

            /// <summary>
            /// A key release event.
            /// </summary>
            public event Action4<Area, IPointer, KeyCodes, KeyboardModifiers> KeyRelease
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onKeyReleased += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onKeyReleased -= value;
                    }
                }
            }

            /// <summary>
            /// Focus gained.
            /// </summary>
            public event Action<Area> FocusGain
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onFocusGain += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onFocusGain -= value;
                    }
                }
            }

            /// <summary>
            /// Focus lost.
            /// </summary>
            public event Action<Area> FocusLost
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onFocusLost += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onFocusLost -= value;
                    }
                }
            }

            /// <summary>
            /// Triggered when description is shown (before).
            /// </summary>
            public event Action2<Area, IWidget> DescriptionShow
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onDescriptionShow += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onDescriptionShow -= value;
                    }
                }
            }

            /// <summary>
            /// Triggered when description is hidden (before).
            /// </summary>
            public event Action2<Area, IWidget> DescriptionHide
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onDescriptionHide += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onDescriptionHide -= value;
                    }
                }
            }

            public event Action3<Area, StyleState, StyleState> StyleStateChange
            {
                add
                {
                    lock (syncRoot)
                    {
                        parent.onStyleStateChange += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        parent.onStyleStateChange -= value;
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Style

        /// <summary>
        /// An area style.
        /// </summary>
        public class AreaStyle : IStateStyle
        {
            #region Private Members
            protected object syncRoot = new object();

            protected AreaStyle parentStyle;
            protected Action<IPreChangeNotifier> onChange;
            protected BackgroundStyle background = new BackgroundStyle();
            protected BorderStyle border = new BorderStyle();

            protected void Changed()
            {
                Action<IPreChangeNotifier> n = onChange;
                if (n != null)
                {
                    n(this);
                }
            }


            protected void TriggerChanged(IPreChangeNotifier n)
            {
                this.Changed();
            }


            protected void LinkSubStyle(ISubStyle style, ISubStyle parentProvided)
            {
                if (style == null) return;

                style.OnChange += TriggerChanged;

                if (parentProvided == null) return;

                // We link it to parent provided.
                while (style.Parent != null) style = style.Parent;

                if (style != parentProvided) style.Parent = parentProvided;

            }

            protected void UnLinkSubStyle(ISubStyle style, ISubStyle parentProvided)
            {
                if (style == null) return;

                style.OnChange -= TriggerChanged;

                if (parentProvided == null) return;

                // We remove parent provided style.
                while (style.Parent != null)
                {
                    if (style.Parent == parentProvided)
                    {
                        style.Parent = null;
                        return;
                    }

                    style = style.Parent;
                }
            }

            protected void ReparentSubStyle(ISubStyle style,
                ISubStyle prevParentProvided, ISubStyle currentParentProvided)
            {
                UnLinkSubStyle(style, prevParentProvided);
                LinkSubStyle(style, currentParentProvided);
            }

            protected void ReparentAllStyles(IStateStyle prevParent, IStateStyle currentParent)
            {
                ISubStyle[] styles1 = prevParent != null ? prevParent.SubStyles : null;
                ISubStyle[] styles2 = currentParent != null ? currentParent.SubStyles : null;
                ISubStyle[] myStyles = SubStyles;

                for (int i = 0; i < myStyles.Length; i++)
                {
                    ReparentSubStyle(myStyles[i], styles1 != null ? styles1[i] : null,
                        styles2 != null ? styles2[i] : null);
                }
            }

            #endregion

            #region IStateStyle Members

            public virtual ISubStyle[] SubStyles
            {
                get { return new ISubStyle[] { Background, Border }; }
            }

            public IStateStyle Parent
            {
                get { return parentStyle; }
                set
                {
                    if (value.GetType() != this.GetType()) throw new ArgumentException("The style is not compatible.");

                    Changed();

                    lock (syncRoot)
                    {
                        if (parentStyle != null)
                        {
                            parentStyle.OnChange -= TriggerChanged;
                        }

                        // We reparent all styles.
                        ReparentAllStyles(parentStyle, value);

                        parentStyle = value as AreaStyle;

                        if (parentStyle != null)
                        {
                            parentStyle.OnChange += TriggerChanged;
                        }

                    }

                }
            }

            #endregion

            #region Properties

            /// <summary>
            /// Sets the background of button.
            /// </summary>
            public BackgroundStyle Background
            {
                get
                {
                    if (background == null)
                    {
                        if (parentStyle != null) return parentStyle.Background;
                    }
                    return background;
                }
                set
                {
                    Changed();
                    lock (syncRoot)
                    {
                        UnLinkSubStyle(background, parentStyle != null ? parentStyle.Background : null);
                        background = value;
                        LinkSubStyle(background, parentStyle != null ? parentStyle.Background : null);
                    }

                }
            }

            /// <summary>
            /// Gets or sets border style.
            /// </summary>
            public BorderStyle Border
            {
                get
                {
                    if (border == null)
                    {
                        if (border != null) return parentStyle.Border;
                    }
                    return border;
                }
                set
                {
                    Changed();
                    lock (syncRoot)
                    {
                        UnLinkSubStyle(border, parentStyle != null ? parentStyle.Border : null);
                        border = value;
                        LinkSubStyle(border, parentStyle != null ? parentStyle.Border : null);
                    }


                }
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

            #region Constructors

            public AreaStyle()
            {
            }

            #endregion

            #region ICloneable<IStateStyle> Members

            public virtual IStateStyle Clone()
            {
                AreaStyle style = new AreaStyle();
                style.Background = background != null ? background.Clone() as BackgroundStyle : null;
                style.Border = border != null ? border.Clone() as BorderStyle : null;
                style.Parent = parentStyle;

                return style;
            }

            #endregion
        }

        #endregion

        #region IPositionable Members

        public virtual IPathf Outline
        {
            get { return outline; }
        }

        public virtual IShapef Shape
        {
            get { return shape; }
        }

        public virtual void GetBoundingBox(out Vector2f leftBottom, out Vector2f rightTop)
        {
            leftBottom = this.boundingRectMin;
            rightTop = this.boundingRectMax;
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

        #region ILayoutNegotiation Members

        Vector2f ILayoutNegotiation.GetPreferredSize(ICanvas canvas, IPositionable parent)
        {
            return GetPreferredSizeInternal(canvas, parent);
        }

        Vector2f ILayoutNegotiation.GetRequiredMinimumSize(ICanvas canvas, IPositionable parent)
        {
            return GetRequiredMaximumSizeInternal(canvas, parent);
        }

        Vector2f ILayoutNegotiation.GetRequiredMaximumSize(ICanvas canvas, IPositionable parent)
        {
            return GetRequiredMinimumSizeInternal(canvas, parent);
        }

        void IPositionable.ApplyBoundingRect(ICanvas canvas, Vector2f minPosition, Vector2f maxPosition)
        {
            boundingRectMin = minPosition;
            boundingRectMax = maxPosition;

            ApplyBoundingRectInternal(canvas, minPosition, maxPosition);
        }

        #endregion

        #region IUserInteractive Members

        void IUserInteractive.OnPointerEnter(IPointer cursor)
        {
            OnPointerEnterInternal(cursor);

            // We fire event.
            Action2<Area, IPointer> t = onMouseOver;
            if (t != null)
            {
                t(this, cursor);
            }
        }

        void IUserInteractive.OnPointerMove(IPointer cursor, Vector2f deltaMove)
        {
            OnPointerMoveInternal(cursor, deltaMove);

            // We fire event.
            Action3<Area, IPointer, Vector2f> t = onMouseMove;
            if (t != null)
            {
                t(this, cursor, deltaMove);
            }
        }

        void IUserInteractive.OnPointerLeave(IPointer cursor)
        {
            OnPointerLeaveInternal(cursor);

            // We fire event.
            Action2<Area, IPointer> t = onMouseLeave;
            if (t != null)
            {
                t(this, cursor);
            }
        }

        void IUserInteractive.OnPointerPress(IPointer cursor, uint button, InputEventModifier modifiers)
        {
            // We become focused.
            if (parent is IContainer)
            {
                (parent as IContainer).Focused = this;
            }
            else
            {
                FocusChange(true);
            }

            if (this is IContainer)
            {
                // We make sure no child is focused.
                (this as IContainer).Focused = null;
            }

            OnPointerPressInternal(cursor, button, modifiers);


            Action4<Area, IPointer, uint, InputEventModifier> t = onMousePressed;
            if (t != null)
            {
                t(this, cursor, button, modifiers);
            }
        }

        void IUserInteractive.OnPointerRelease(IPointer cursor, uint button)
        {
            OnPointerReleaseInternal(cursor, button);

            Action3<Area, IPointer, uint> t = onMouseReleased;
            if (t != null)
            {
                t(this, cursor, button);
            }
        }

        void IUserInteractive.OnWheel(IPointer cursor, float deltaWheel)
        {
            OnWheelInternal(cursor, deltaWheel);

            Action3<Area, IPointer, float> t = onWheel;
            if (t != null)
            {
                t(this, cursor, deltaWheel);
            }

        }

        void IUserInteractive.OnKeyPress(IPointer cursor, KeyCodes code, KeyboardModifiers modifiers, InputEventModifier eventModifiers)
        {
            OnKeyPressInternal(cursor, code, modifiers, eventModifiers);

            Action5<Area, IPointer, KeyCodes, KeyboardModifiers, InputEventModifier> t = onKeyPressed;
            if (t != null)
            {
                t(this, cursor, code, modifiers, eventModifiers);
            }

        }

        void IUserInteractive.OnKeyRelease(IPointer cursor, KeyCodes code, KeyboardModifiers modifiers)
        {
            OnKeyReleaseInternal(cursor, code, modifiers);

            Action4<Area, IPointer, KeyCodes, KeyboardModifiers> t = onKeyReleased;
            if (t != null)
            {
                t(this, cursor, code, modifiers);
            }
        }

        #endregion

        #region ILayoutNegotiation Members

        bool ILayoutNegotiation.GetPreferredRect(ICanvas canvas, IPositionable parent, out Vector2f leftBottom, out Vector2f rightTop)
        {
            return GetPreferredRectInternal(canvas, parent, out leftBottom, out rightTop);
        }

        Vector4f ILayoutNegotiation.GetMargin(ICanvas canvas, IPositionable parent)
        {
            return GetMarginInternal(canvas, parent);
        }

        #endregion

        #region IAnimatable Members

        void IAnimatable.Update(float deltaTime)
        {
            if (this is IContainer)
            {
                foreach (object obj in (this as IContainer).Children)
                {
                    if (obj is IAnimatable) (obj as IAnimatable).Update(deltaTime);
                }
            }

            lock (activeAnimations)
            {
                UpdateAnimationsInternal(deltaTime);
            }
        }

        public virtual void AddAnimation(IAnimation animation)
        {
            lock (activeAnimations)
            {
                activeAnimations.Add(new AnimationProcess(animation));
            }
        }

        public virtual void RemoveAnimation(IAnimation animation)
        {
            lock (activeAnimations)
            {
                activeAnimations.RemoveAll(delegate(AnimationProcess proc) { return proc.Animation == animation; });
            }
        }

        #endregion

        #region IWidget Members

        public void EnterChange()
        {
            Monitor.Enter(syncRoot);

            mutableCount++;
        }

        public ChangeContext Enter()
        {
            EnterChange();

            return new ChangeContext(this);
        }

        public void ExitChange()
        {
            mutableCount--;

            Monitor.Exit(syncRoot);
        }

        public IWidget Parent
        {
            get { return parent; }
        }

        void IWidget.SetParent(IWidget widget, GuiManager manager)
        {
            this.parent = widget;
            this.manager = manager;

            // We must also apply to all children.
            if (this is Containers.IContainer)
            {
                foreach (object child in (this as Containers.IContainer).Children)
                {
                    if (child is IWidget)
                    {
                        (child as IWidget).SetParent(this, manager);
                    }
                }
            }
        }

        public WidgetSerialization SerializationOptions
        {
            get
            {
                return serializationOptions;
            }
            set
            {
                AssertMutable();
                serializationOptions = value;
            }
        }


        public void FocusChange(bool gained)
        {
            if (gained)
            {
                OnSetFocusInternal();
            }
            else
            {
                OnLostFocusInternal();
            }

            Action<Area> t = gained ? onFocusGain : onFocusLost;
            if (t != null)
            {
                t(this);
            }
        }

        #endregion

        #region IDisplayObject Members

        public StyleAnimationController StyleAnimation
        {
            get { return animationState; }
        }

        public virtual IDisplayObject[] SubDisplayObjects
        {
            get
            {
                return null;
            }
        }

        #endregion

        #region ISerializable Members

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SerializeOptions", serializationOptions);
            info.AddValue("ZOrder", zOrder);
            info.AddValue("Parent", parent);
            info.AddValue("Description", description);
            info.AddValue("ContainedDescriptionShowUpTime", containedDescriptionShowUpTime);
            info.AddValue("ContainedDescriptionPositioning", containedDescriptionPositioning);
            info.AddValue("ContainedDescriptionObject", containedDescriptionObject);
            info.AddValue("Anchor", anchor);
            info.AddValue("MinSize", minSize);
            info.AddValue("MaxSize", maxSize);
            info.AddValue("PreferredSize", preferredSize);
            info.AddValue("PreserveRatio", preserveRatio);
            info.AddValue("PreferredRect", preferredRect);
            info.AddValue("MarginLeft", marginLeft);
            info.AddValue("MarginRight", marginRight);
            info.AddValue("MarginTop", marginTop);
            info.AddValue("MarginBottom", marginBottom);

            if((serializationOptions & WidgetSerialization.PersistAllStyles) > 0)
            {
                info.AddValue("Style", style);
            } else if((serializationOptions & WidgetSerialization.PersistNonThemeStyles) > 0)
            {
                if(!style.IsTheme)
                {
                    info.AddValue("Style", style);
                }
            }

            if((serializationOptions & WidgetSerialization.PersistRenderer) > 0)
            {
                info.AddValue("Renderer", renderer);
            }

            if((serializationOptions & WidgetSerialization.PersistState) > 0)
            {
                info.AddValue("IsVisible", isVisible);
            }

            if((serializationOptions & WidgetSerialization.PersistAnimations) > 0)
            {
                info.AddValue("StyleAnimationController", animationState);
                info.AddValue("ActiveAnimations", activeAnimations);
            }

            if((serializationOptions & WidgetSerialization.PersistEventBindings) > 0)
            {
                info.AddValue("OnChange", onChange);
                info.AddValue("OnMouseOver", onMouseOver);
                info.AddValue("OnMouseLeave", onMouseLeave);
                info.AddValue("OnMousePressed", onMousePressed);
                info.AddValue("OnMouseReleased", onMouseReleased);
                info.AddValue("OnWheel", onWheel);
                info.AddValue("OnMouseMove", onMouseMove);
                info.AddValue("OnFocusGain", onFocusGain);
                info.AddValue("OnFocusLost", onFocusLost);
                info.AddValue("OnKeyPressed", onKeyPressed);
                info.AddValue("OnKeyReleased", onKeyReleased);
                info.AddValue("OnDescriptionShow", onDescriptionShow);
                info.AddValue("OnDescriptionHide", onDescriptionHide);
                info.AddValue("OnStyleStateChange", onStyleStateChange);
            }
        }

        protected Area(SerializationInfo info, StreamingContext context)
        {
            serializationOptions = (WidgetSerialization)info.GetValue("SerializeOptions", typeof(WidgetSerialization));
            zOrder = info.GetUInt32("ZOrder");
            parent = (IWidget)info.GetValue("Parent", typeof(IWidget));
            description = info.GetString("Description");
            containedDescriptionShowUpTime = info.GetSingle("ContainedDescriptionShowUpTime");
            containedDescriptionPositioning = (LayoutAnchor)info.GetValue("ContainedDescriptionPositioning", typeof(LayoutAnchor));
            containedDescriptionObject = (IDisplayObject)info.GetValue("ContainedDescriptionObject", typeof(IDisplayObject));
            anchor = (LayoutAnchor)info.GetValue("Anchor", typeof(LayoutAnchor));
            minSize = (GuiVector2)info.GetValue("MinSize", typeof(GuiVector2));
            maxSize = (GuiVector2)info.GetValue("MaxSize", typeof(GuiVector2));
            preferredSize = (GuiVector2)info.GetValue("PreferredSize", typeof(GuiVector2));
            preserveRatio = info.GetBoolean("PreserveRatio");
            preferredRect = (GuiRect)info.GetValue("PreferredRect", typeof(GuiRect));
            marginLeft = (GuiScalar)info.GetValue("MarginLeft", typeof(GuiScalar));
            marginRight = (GuiScalar)info.GetValue("MarginRight", typeof(GuiScalar));
            marginTop = (GuiScalar)info.GetValue("MarginTop", typeof(GuiScalar));
            marginBottom = (GuiScalar)info.GetValue("MarginBottom", typeof(GuiScalar));

            if ((serializationOptions & WidgetSerialization.PersistAllStyles) > 0)
            {
                style = (Style)info.GetValue("Style", typeof(Style));
            }
            else if ((serializationOptions & WidgetSerialization.PersistNonThemeStyles) > 0)
            {
                style = (Style)info.GetValue("Style", typeof(Style));
                
            }

            if ((serializationOptions & WidgetSerialization.PersistRenderer) > 0)
            {
                renderer = (Themes.IGuiRenderer)info.GetValue("Renderer", typeof(Themes.IGuiRenderer));
            }

            if ((serializationOptions & WidgetSerialization.PersistState) > 0)
            {
                isVisible = info.GetBoolean("IsVisible");
            }

            if ((serializationOptions & WidgetSerialization.PersistAnimations) > 0)
            {
                animationState = (StyleAnimationController)info.GetValue("StyleAnimationController", typeof(StyleAnimationController));
                activeAnimations = (List<AnimationProcess>)info.GetValue("ActiveAnimations", typeof(List<AnimationProcess>));
            }

            if ((serializationOptions & WidgetSerialization.PersistEventBindings) > 0)
            {
                onChange = (Action<IPreChangeNotifier>)info.GetValue("OnChange", typeof(Action<IPreChangeNotifier>));
                onMouseOver = (Action2<Area, IPointer>)info.GetValue("OnMouseOver", typeof(Action2<Area, IPointer>));
                onMouseLeave = (Action2<Area, IPointer>)info.GetValue("OnMouseLeave", typeof(Action2<Area, IPointer>));
                onMousePressed = (Action4<Area, IPointer, uint, InputEventModifier>)info.GetValue("OnMousePressed", 
                    typeof(Action4<Area, IPointer, uint, InputEventModifier>));
                onMouseReleased = (Action3<Area, IPointer, uint>)info.GetValue("OnMouseReleased", typeof(Action3<Area, IPointer, uint>));
                onWheel = (Action3<Area, IPointer, float>)info.GetValue("OnWheel", typeof(Action3<Area, IPointer, float>));
                onMouseMove = (Action3<Area, IPointer, Vector2f>)info.GetValue("OnMouseMove", typeof(Action3<Area, IPointer, Vector2f>));
                onFocusGain = (Action<Area>)info.GetValue("OnFocusGain", typeof(Action<Area>));
                onFocusLost = (Action<Area>)info.GetValue("OnFocusLost", typeof(Action<Area>));
                onKeyPressed = (Action5<Area, IPointer, KeyCodes, KeyboardModifiers, InputEventModifier>)info.GetValue(
                    "OnKeyPressed", typeof(Action5<Area, IPointer, KeyCodes, KeyboardModifiers, InputEventModifier>));
                onKeyReleased = (Action4<Area, IPointer, KeyCodes, KeyboardModifiers>)info.GetValue("OnKeyReleased", typeof(Action4<Area, IPointer, KeyCodes, KeyboardModifiers>));
                onDescriptionShow = (Action2<Area, IWidget>)info.GetValue("OnDescriptionShow", typeof(Action2<Area, IWidget>));
                onDescriptionHide = (Action2<Area, IWidget>)info.GetValue("OnDescriptionHide", typeof(Action2<Area, IWidget>));
                onStyleStateChange = (Action3<Area, StyleState, StyleState>)info.GetValue("OnStyleStateChange", typeof(Action3<Area, StyleState, StyleState>));
            }
        }

        #endregion

    }
}
