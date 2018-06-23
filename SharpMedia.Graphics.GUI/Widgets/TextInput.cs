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

namespace SharpMedia.Graphics.GUI.Widgets
{


    /// <summary>
    /// A text enabled text editing by user.
    /// </summary>
    [Serializable]
    public class TextInput : Label
    {
        #region Private Members

        // Validation.
        protected Validation.IValidator<string> validator;
        protected bool isTextValid;
        protected Validation.ValidationResponse validationResponse;
        protected uint maxTextLength = uint.MaxValue;
        protected bool isPasswordMode;

        // Events.
        protected Predicate<char> onCharAllowed;
        protected Action2<TextInput, string> onTextValidation;
        protected Action<TextInput> onCharLimitExceeded;
        
        
        protected TextInput(object dummy) 
            : base(dummy)
        {
        }
        #endregion

        #region Events

        /// <summary>
        /// Text events.
        /// </summary>
        public class TextEvents : LabelEvents
        {
            #region Private Members
            TextInput text { get { return this.parent as TextInput; } }

            internal protected TextEvents(TextInput text)
                : base(text)
            { }

            #endregion

            #region Events

            /// <summary>
            /// Trigerred on text validation (when text is changed).
            /// </summary>
            public event Action2<TextInput, string> TextValidation
            {
                add
                {
                    lock (syncRoot)
                    {
                        text.onTextValidation += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        text.onTextValidation -= value;
                    }
                }
            }

            /// <summary>
            /// A special "validation" that allows to forbid certain characters.
            /// </summary>
            public event Predicate<char> CharAllowed
            {
                add
                {
                    lock (syncRoot)
                    {
                        text.onCharAllowed += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        text.onCharAllowed -= value;
                    }
                }
            }

            /// <summary>
            /// A character limit was exceeded.
            /// </summary>
            public event Action<TextInput> CharLimitExceeded
            {
                add
                {
                    lock (syncRoot)
                    {
                        text.onCharLimitExceeded += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        text.onCharLimitExceeded -= value;
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Creates a text element.
        /// </summary>
        public TextInput()
            : base(null)
        {
            this.eventHolder = new TextEvents(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns events.
        /// </summary>
        public new TextEvents Events
        {
            get
            {
                return eventHolder as TextEvents;
            }
        }

        /// <summary>
        /// Is password mode used (text hidden).
        /// </summary>
        public bool IsPasswordMode
        {
            get
            {
                return isPasswordMode;
            }
            set
            {
                AssertMutable();
                isPasswordMode = value;
            }
        }

        /// <summary>
        /// Is text valid at current state.
        /// </summary>
        public bool IsTextValid
        {
            get
            {
                return isTextValid;
            }
            set
            {
                AssertMutable();
                isTextValid = value;
            }
        }

        /// <summary>
        /// A validation response.
        /// </summary>
        public Validation.ValidationResponse ValidationResponse
        {
            get
            {
                return validationResponse;
            }
            set
            {
                AssertMutable();
                validationResponse = value;
            }
        }

        /// <summary>
        /// A validator used. If not set, validation response is ignored.
        /// </summary>
        public Validation.IValidator<string> Validator
        {
            get
            {
                return validator;
            }
            set
            {
                AssertMutable();
                validator = value;
            }
        }

        /// <summary>
        /// Maximum text length.
        /// </summary>
        public uint MaxTextLength
        {
            get
            {
                return maxTextLength;
            }
            set
            {
                AssertMutable();
                maxTextLength = value;
            }
        }



        #endregion

        #region Overrides



        #endregion
    }
}
