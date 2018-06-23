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
using SharpMedia.Math;
using SharpMedia.Graphics.GUI.Styles;
using SharpMedia.Graphics.GUI.Styles.SubStyles;
using SharpMedia.Graphics.Vector.Fonts;
using System.Runtime.Serialization;
using SharpMedia.Input;

namespace SharpMedia.Graphics.GUI.Widgets
{


    /// <summary>
    /// A label is area with text.
    /// </summary>
    [Serializable]
    public class Label : Area
    {
        #region Private Members
        protected internal string text = string.Empty;
        protected internal bool enabled = true;
        protected internal Vector2i selectedRange = new Vector2i(0, -1);
        protected internal int cursorTextPositon = -1;


        // Event.
        protected internal Action2<Label, Vector2i> onTextSelected;
        protected internal Action<Label> onTextDeselected;
        protected internal Action3<Label, Vector2i, StringBuilder> onTextCopy;
        protected internal Action2<Label, int> onCursorTextPositionChanged;
        protected internal Action2<Label, string> onTextChanged;

        // Text info.
        TextRenderInfo textRenderInfo;

        // Dragging data.
        Helpers.DragController dragController = new Helpers.DragController();

        protected Label(object dummy)
            : base(dummy)
        {
        }

        protected Label(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            enabled = info.GetBoolean("Enabled");
            text = info.GetString("Text");
        }
        #endregion

        #region Public Members

        /// <summary>
        /// Creates a label.
        /// </summary>
        public Label()
            : base((object)null)
        {
            this.eventHolder = new LabelEvents(this);
        }

        /// <summary>
        /// Creates a text-initialized label.
        /// </summary>
        /// <param name="text"></param>
        public Label(string text)
            : this()
        {
            this.text = text;
        }

        /// <summary>
        /// Creates a label with text and style.
        /// </summary>
        public Label(string text, Style style)
            : this(text)
        {
            this.style = style;
        }

        /// <summary>
        /// Creates a label with text, style and enabled/disabled state.
        /// </summary>
        public Label(string text, Style style, bool enabled)
            : this(text, style)
        {
            this.enabled = enabled;
        }

        #endregion

        #region Events

        /// <summary>
        /// An event class.
        /// </summary>
        public class LabelEvents : Area.AreaEvents
        {
            #region Protected Members
            Label label { get { return base.parent as Label; } }

            internal protected LabelEvents(Label label) : base(label) { }
            #endregion

            #region Events

            /// <summary>
            /// Trigerred when text is selected or selection is altered.
            /// </summary>
            public event Action2<Label, Vector2i> TextSelect
            {
                add
                {
                    lock (syncRoot)
                    {
                        label.onTextSelected += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        label.onTextSelected -= value;
                    }
                }
            }


            /// <summary>
            /// Trigerred when text is deselected completelly.
            /// </summary>
            public event Action<Label> TextDeselect
            {
                add
                {
                    lock (syncRoot)
                    {
                        label.onTextDeselected += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        label.onTextDeselected -= value;
                    }
                }
            }

            /// <summary>
            /// Trigerred when CTRL-C is used to copy text.
            /// </summary>
            public event Action3<Label, Vector2i, StringBuilder> TextCopy
            {
                add
                {
                    lock (syncRoot)
                    {
                        label.onTextCopy += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        label.onTextCopy -= value;
                    }
                }
            }

            /// <summary>
            /// Fired when cursor position in text is changed.
            /// </summary>
            public event Action2<Label, int> CursorTextPositionChange
            {
                add
                {
                    lock (syncRoot)
                    {
                        label.onCursorTextPositionChanged += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        label.onCursorTextPositionChanged -= value;
                    }
                }
            }

            /// <summary>
            /// Fired on text change.
            /// </summary>
            public event Action2<Label, string> TextChange
            {
                add
                {
                    lock (syncRoot)
                    {
                        label.onTextChanged += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        label.onTextChanged -= value;
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Selected range of the text on Label.
        /// </summary>
        /// <remarks>Some derived classes may ignore this.</remarks>
        public Vector2i TextSelectedRange
        {
            get
            {
                return selectedRange;
            }
            set
            {
                AssertMutable();
                selectedRange = value;
            }
        }

        /// <summary>
        /// A cursor text position, or negative value if no position.
        /// </summary>
        public int CursorTextPosition
        {
            get
            {
                return cursorTextPositon;
            }
        }

        /// <summary>
        /// Is label enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return enabled;
            }
            set
            {
                AssertMutable();
                enabled = value;
            }
        }

        /// <summary>
        /// The text.
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                AssertMutable();
                text = value;

                Action2<Label, string> t = onTextChanged;
                if (t != null)
                {
                    t(this, value);
                }
            }
        }

        /// <summary>
        /// Selected text of label.
        /// </summary>
        public string SelectedText
        {
            get
            {
                if (selectedRange.Y < selectedRange.X) return string.Empty;
                return text.Substring(selectedRange.X, selectedRange.Y - selectedRange.X + 1);
            }
        }

        /// <summary>
        /// Label events.
        /// </summary>
        public new LabelEvents Events
        {
            get
            {
                return base.eventHolder as LabelEvents;
            }
        }

        #endregion

        #region Overrides

        protected override void OnLostFocusInternal()
        {
            base.OnLostFocusInternal();

            animationState.TransistTo(CommonStyleStates.Normal);
        }

        protected override void OnSetFocusInternal()
        {
            base.OnSetFocusInternal();

            animationState.TransistTo(CommonStyleStates.Focused);
        }

        protected override bool IsStyleCompatible(Style style)
        {
            return style.Type == typeof(LabelStyle);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // We add additional information.
            info.AddValue("Text", text);
            info.AddValue("Enabled", enabled);
        }

        protected override void OnPointerPressInternal(IPointer cursor, uint button, 
            SharpMedia.Input.InputEventModifier modifiers)
        {
            base.OnPointerPressInternal(cursor, button, modifiers);

            // 1) If clicked somewhere, text is deselect.
            if (!enabled) return;

            TextDeselect();

            // We only allow primary button.
            if (button != 0) return;

            if ((modifiers & InputEventModifier.DoubleClick) != 0)
            {
                // Double click starts a word selection.
                WordSelection(cursor.CanvasPosition);
            }
            else
            {
                // Pointer pressed starts drag event.
                dragController.Start(cursor);
            }
        }

        protected override void OnPointerReleaseInternal(IPointer cursor, uint button)
        {
            base.OnPointerReleaseInternal(cursor, button);

            if (button != 0 || !enabled) return;

            dragController.End(cursor);

            // No need for event if nothing dragged.
            if (selectedRange.Y < selectedRange.X) return;

            // We also fire selection event.
            Action2<Label, Vector2i> t = onTextSelected;
            if (t != null)
            {
                t(this, this.selectedRange);
            }
        }


        protected override void OnPointerMoveInternal(IPointer cursor, Vector2f deltaMove)
        {
            base.OnPointerMoveInternal(cursor, deltaMove);

            dragController.Update(cursor);
            UpdateDrag();
        }

        protected override void OnKeyPressInternal(IPointer cursor, KeyCodes code,
            KeyboardModifiers modifiers, InputEventModifier eventModifiers)
        {
            base.OnKeyPressInternal(cursor, code, modifiers, eventModifiers);

            // We are focused.
            animationState.TransistTo(CommonStyleStates.Focused);

            if (code == KeyCodes.C && (modifiers & KeyboardModifiers.LCtrl) != 0)
            {
                if (selectedRange.Y < selectedRange.X) return;

                // We have a copy event.
                Action3<Label, Vector2i, StringBuilder> t = onTextCopy;
                StringBuilder b = new StringBuilder(SelectedText);
                
                // Events may alter copy or react on it.
                if (t != null)
                {
                    t(this, selectedRange, b);
                }

                // We add copy to cursor.
                AttachedData attachedData = new AttachedData();
                attachedData.ApplicationID = Guid.Empty; //< FIXME
                attachedData.Data = b.ToString();
                attachedData.Representation = null;

                // We attach data.
                cursor.AttachData(attachedData);
            }
        }

        protected override void OnPointerEnterInternal(IPointer cursor)
        {
            if (animationState.CurrentState != CommonStyleStates.Focused)
            {
                base.OnPointerEnterInternal(cursor);
            }
        }

        protected override void OnPointerLeaveInternal(IPointer cursor)
        {
            if (!animationState.IsOrTransistingTo(CommonStyleStates.Focused))
            {
                base.OnPointerLeaveInternal(cursor);
            }
        }


        #endregion

        #region Helper Methods

        /// <summary>
        /// Must be called by theme.
        /// </summary>
        /// <param name="info"></param>
        public void SetTextInfo(TextRenderInfo info)
        {
            lock (syncRoot)
            {
                textRenderInfo = info;
            }
        }

        void TextDeselect()
        {
            lock (syncRoot)
            {
                this.cursorTextPositon = -1;
                this.selectedRange = new Vector2i(0, -1);
            }

            // We fire deselect event.
            Action<Label> t = onTextDeselected;
            if (t != null)
            {
                t(this);
            }
            
        }

        void WordSelection(Vector2f position)
        {
            Vector2i range = Vector2i.Zero;

            lock (syncRoot)
            {
                
                // TODO:
                return;
            }

            // We fire selection event.
            Action2<Label, Vector2i> t = onTextSelected;
            if (t != null)
            {
                t(this, range);
            }
        }

        void UpdateDrag()
        {
            lock (syncRoot)
            {
                if (!dragController.IsDragging || !enabled) return;

                Vector2f min, max;
                dragController.GetRect(out min, out max);

                // We update selected text.
                if (textRenderInfo == null)
                {
                    this.selectedRange = new Vector2i(0, -1);
                }
                else
                {
                    this.selectedRange = this.textRenderInfo.GetRange(min, max, IncludeType.PartyIncluded);
                }

            }
        }

        #endregion

        #region Styles

        /// <summary>
        /// A label style.
        /// </summary>
        public class LabelStyle : AreaStyle
        {
            #region Private Members
            protected FontStyle textFont;
            protected PartialFontStyle selectedTextFont;
            BackgroundStyle selectionBackground;


            private LabelStyle labelParent
            {
                get { return parentStyle as LabelStyle; }
            }
            #endregion

            #region Public Members

            /// <summary>
            /// Font style of text.
            /// </summary>
            public FontStyle TextFont
            {
                get
                {
                    if (textFont == null && parentStyle != null)
                    {
                        return (parentStyle as LabelStyle).TextFont;
                    }
                    return textFont;
                }
                set
                {
                    Changed();
                    lock (syncRoot)
                    {
                        UnLinkSubStyle(textFont, parentStyle != null ? labelParent.TextFont : null);
                        textFont = value;
                        LinkSubStyle(textFont, parentStyle != null ? labelParent.TextFont : null);
                    }
                }
            }

            /// <summary>
            /// Font style of selected text.
            /// </summary>
            public PartialFontStyle SelectedTextFont
            {
                get
                {
                    if (selectedTextFont == null && parentStyle != null)
                    {
                        return (parentStyle as LabelStyle).SelectedTextFont;
                    }
                    return selectedTextFont;
                }
                set
                {
                    Changed();
                    lock (syncRoot)
                    {
                        UnLinkSubStyle(selectedTextFont, parentStyle != null ? labelParent.SelectedTextFont : null);
                        selectedTextFont = value;
                        LinkSubStyle(selectedTextFont, parentStyle != null ? labelParent.SelectedTextFont : null);
                    }
                }
            }

            /// <summary>
            /// The style of background of selection.
            /// </summary>
            public BackgroundStyle SelectionBackground
            {
                get
                {
                    if (selectionBackground == null && parentStyle != null)
                    {
                        return (parentStyle as LabelStyle).SelectionBackground;
                    }
                    return selectionBackground;
                }
                set
                {
                    Changed();
                    lock (syncRoot)
                    {
                        UnLinkSubStyle(selectionBackground, parentStyle != null ? labelParent.SelectionBackground : null);
                        selectionBackground = value;
                        LinkSubStyle(selectionBackground, parentStyle != null ? labelParent.SelectionBackground : null);
                    }
                }
            }

            #endregion

            #region Overrides

            public override IStateStyle Clone()
            {
                LabelStyle style = new LabelStyle();
                style.Background = background != null ? background.Clone() as BackgroundStyle : null;
                style.Border = border != null ? background.Clone() as BorderStyle : null;
                style.SelectedTextFont = selectedTextFont != null ? selectedTextFont.Clone() as PartialFontStyle : null;
                style.TextFont = textFont != null ? textFont.Clone() as FontStyle : null;
                style.Parent = parentStyle;

                return style;
            }

            public override ISubStyle[] SubStyles
            {
                get
                {
                    return new ISubStyle[] 
                    { Background, Border, SelectedTextFont, 
                        SelectionBackground, TextFont };
                }
            }

            #endregion
        }

        #endregion

    }
}
