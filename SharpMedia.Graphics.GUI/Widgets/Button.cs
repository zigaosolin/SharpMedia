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
using System.Threading;
using SharpMedia.Graphics.GUI.Metrics;
using SharpMedia.Graphics.GUI.Styles;
using SharpMedia.Input;
using SharpMedia.Math;
using SharpMedia.Math.Shapes;
using SharpMedia.Graphics.Vector;

namespace SharpMedia.Graphics.GUI.Widgets
{

    /// <summary>
    /// An ordinary button, with display object as child.
    /// </summary>
    [Serializable]
    public class Button : Area
    {
        #region Private Members
        protected IDisplayObject displayObject;

        // Events
        Action<Button> onClicked;
        #endregion

        #region Helper Methods

        void SetStyleState(StyleState stateTo)
        {
            IDisplayObject dObject = displayObject;
            if (dObject != null)
            {
                dObject.StyleAnimation.TransistTo(stateTo);
            }

            animationState.TransistTo(stateTo);

        }

        #endregion

        #region Events

        /// <summary>
        /// Button event holder.
        /// </summary>
        public class ButtonEvents : AreaEvents
        {
            #region Private Members

            Button button { get { return this.parent as Button; } }

            internal protected ButtonEvents(Button button)
                : base(button)
            {
            }

            #endregion

            #region Events

            /// <summary>
            /// Button clicked event, fired when button is pressed with the mouse still over it.
            /// </summary>
            public event Action<Button> ButtonClicked
            {
                add
                {
                    lock (syncRoot)
                    {
                        button.onClicked += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        button.onClicked -= value;
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Creates a button.
        /// </summary>
        public Button()
            : base((object)null)
        {
            this.eventHolder = new ButtonEvents(this);
        }

        /// <summary>
        /// Creates a label based button.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="labelStyle"></param>
        public Button(string text, Style labelStyle)
            : this()
        {
            this.displayObject = new Label(text, labelStyle);
        }

        /// <summary>
        /// Creates a display object based button.
        /// </summary>
        /// <param name="displayObject"></param>
        public Button(IDisplayObject displayObject)
            : this()
        {
            this.displayObject = displayObject;
        }

        /// <summary>
        /// Creates a label based button.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="labelStyle"></param>
        public Button(GuiRect preferredRect, string text, Style labelStyle)
            : this(text, labelStyle)
        {
            this.preferredRect = preferredRect;
        }

        /// <summary>
        /// Creates a display object based button.
        /// </summary>
        /// <param name="displayObject"></param>
        public Button(GuiRect preferredRect, IDisplayObject displayObject)
            : this(displayObject)
        {
            this.preferredRect = preferredRect;
        }

        /// <summary>
        /// Creates a label based button.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="labelStyle"></param>
        public Button(LayoutAnchor anchor, string text, Style labelStyle)
            : this(text, labelStyle)
        {
            this.anchor = anchor;
        }

        /// <summary>
        /// Creates a display object based button.
        /// </summary>
        /// <param name="displayObject"></param>
        public Button(LayoutAnchor anchor, IDisplayObject displayObject)
            : this(displayObject)
        {
            this.anchor = anchor;
        }

        /// <summary>
        /// Returns button events.
        /// </summary>
        public new ButtonEvents Events
        {
            get
            {
                return eventHolder as ButtonEvents;
            }
        }

        /// <summary>
        /// Returns the display object.
        /// </summary>
        public IDisplayObject DisplayObject
        {
            get
            {
                return displayObject;
            }
            set
            {
                AssertMutable();
                displayObject = value;
            }
        }

        /// <summary>
        /// Returns the text of label, if display object is label.
        /// </summary>
        public string Text
        {
            get
            {
                if (displayObject is Label)
                {
                    return (displayObject as Label).Text;
                }
                return string.Empty;
            }
            set
            {
                AssertMutable();

                displayObject = new Label(value);
            }
        }

        #endregion

        #region Overrides

        public override IDisplayObject[] SubDisplayObjects
        {
            get
            {
                return new IDisplayObject[] { displayObject };
            }
        }

        protected override void ApplyBoundingRectInternal(ICanvas canvas, Vector2f minPosition, Vector2f maxPosition)
        {
            base.ApplyBoundingRectInternal(canvas, minPosition, maxPosition);


            IDisplayObject obj = displayObject;
            if (obj != null)
            {
                obj.ApplyBoundingRect(canvas, minPosition, maxPosition);
            }
        }

        protected override void OnPointerPressInternal(IPointer cursor, uint button, InputEventModifier modifier)
        {
            base.OnPointerPressInternal(cursor, button, modifier);

            if (button == 0)
            {
                SetStyleState(CommonStyleStates.Clicked);
            }
        }

        protected override void OnPointerReleaseInternal(IPointer cursor, uint button)
        {
            base.OnPointerReleaseInternal(cursor, button);

            if (button == 0)
            {
                SetStyleState(CommonStyleStates.PointerOver);

                // We trigger the event.
                Action<Button> t = onClicked;
                if (t != null)
                {
                    t(this);
                }
            }
        }

        protected override void OnPointerEnterInternal(IPointer cursor)
        {
            if (cursor.IsButtonDown(0))
            {
                animationState.TransistTo(CommonStyleStates.Clicked);
            }
            else
            {
                base.OnPointerEnterInternal(cursor);
            }
        }

        protected override bool IsStyleCompatible(Style style)
        {
            return style.Type == typeof(ButtonStyle);
        }


        #endregion

        #region Style

        /// <summary>
        /// A button style.
        /// </summary>
        public class ButtonStyle : AreaStyle
        {
            /// <summary>
            /// A button style.
            /// </summary>
            public ButtonStyle() : base() { }
        }

        #endregion
    }
}
