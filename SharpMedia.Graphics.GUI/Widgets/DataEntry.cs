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
using SharpMedia.Graphics.GUI.Validation;

namespace SharpMedia.Graphics.GUI.Widgets
{


    /// <summary>
    /// An abstract class for all entry-based widgets.
    /// </summary>
    [Serializable]
    public abstract class DataEntry : Area
    {
        #region Protected Members
        protected bool enabled = true;
        protected ValidationExecution validationExec;

        // Events.
        protected Action2<DataEntry, object> onDataChange;
        protected Action3<DataEntry, object, ValidationResult> onDataValidation;

        #endregion

        #region Constructors

        public DataEntry()
        {
        }

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="dummy"></param>
        protected DataEntry(object dummy)
            : base(dummy)
        {
        }

        #endregion

        #region Events

        /// <summary>
        /// Data entry events.
        /// </summary>
        public class DataEntryEvents : AreaEvents
        {
            #region Protected Members
            DataEntry dataEntry { get { return parent as DataEntry; } }

            internal protected DataEntryEvents(DataEntry entry)
                : base(entry)
            {
            }

            #endregion

            #region Public Members

            /// <summary>
            /// Fired on data change (input or manual based).
            /// </summary>
            public event Action2<DataEntry, object> DataChange
            {
                add
                {
                    lock (syncRoot)
                    {
                        dataEntry.onDataChange += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        dataEntry.onDataChange -= value;
                    }
                }
            }

            /// <summary>
            /// Fired when data validation was performanced
            /// </summary>
            public event Action3<DataEntry, object, ValidationResult> DataValidation
            {
                add
                {
                    lock (syncRoot)
                    {
                        dataEntry.onDataValidation += value;
                    }
                }
                remove
                {
                    lock (syncRoot)
                    {
                        dataEntry.onDataValidation -= value;
                    }
                }
            }


            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Is data entry enabled.
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
        /// Validation execution.
        /// </summary>
        public ValidationExecution ValidationExecution
        {
            get
            {
                return validationExec;
            }
            set
            {
                AssertMutable();
                validationExec = value;
            }
        }

        /// <summary>
        /// Data entry events.
        /// </summary>
        public new DataEntryEvents Events
        {
            get { return eventHolder as DataEntryEvents; }
        }

        #endregion

    }
}
