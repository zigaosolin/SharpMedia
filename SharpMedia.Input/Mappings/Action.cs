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
using SharpMedia.AspectOriented;

namespace SharpMedia.Input.Mappings
{
    /// <summary>
    /// A triggered action delegate.
    /// </summary>
    public delegate void TrigerredAction(float triggerData);

    /// <summary>
    /// An action.
    /// </summary>
    /// <remarks>Should add possibilities for "merging" triggers in one frame.</remarks>
    [Serializable]
    public sealed class Action : IActionTriggerable
    {
        #region Private Members
        ActionMapping mapping;
        List<IActionTrigger> triggers = new List<IActionTrigger>();
        string name;
        TrigerredAction events = null;
        string desc = string.Empty;
        #endregion

        #region Protected Methods

        internal void Initialize(EventProcessor processor, bool bind)
        {
            foreach (IActionTrigger trigger in triggers)
            {
                trigger.Initialize(processor, bind);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an action
        /// </summary>
        /// <param name="name"></param>
        internal Action([NotNull] ActionMapping mapping, string name)
        {
            this.mapping = mapping;
            this.name = name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a trigger to action.
        /// </summary>
        /// <param name="trigger"></param>
        public void AddTrigger(IActionTrigger trigger)
        {
            lock (triggers)
            {
                triggers.Add(trigger);
                trigger.BindTo(this);
                trigger.Initialize(mapping.InputProvider, true);
            }
        }

        /// <summary>
        /// Removes trigger from action.
        /// </summary>
        /// <param name="trigger"></param>
        public void RemoveTrigger(IActionTrigger trigger)
        {
            lock (triggers)
            {
                if (triggers.Remove(trigger))
                {
                    trigger.Initialize(mapping.InputProvider, false);
                    trigger.BindTo(null);
                }
            }
        }

        /// <summary>
        /// Triggers the action, usually through triggers (but can use manual trigger here).
        /// </summary>
        /// <param name="stateData">Usually used by axis triggers to </param>
        public void Trigger(float stateData)
        {
            TrigerredAction ev = events;
            if (ev != null)
            {
                ev(stateData);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The mapping this action belongs to.
        /// </summary>
        public ActionMapping Mapping
        {
            get
            {
                return mapping;
            }
        }

        /// <summary>
        /// All action triggers.
        /// </summary>
        public ReadOnlyCollection<IActionTrigger> ActionTriggers
        {
            get
            {
                return new ReadOnlyCollection<IActionTrigger>(triggers);
            }
        }

        /// <summary>
        /// The name of action.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Description of action.
        /// </summary>
        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }

        /// <summary>
        /// Event trigger, including the a
        /// </summary>
        public event TrigerredAction Event
        {
            add
            {
                lock (triggers)
                {
                    events += value;
                }
            }
            remove
            {
                lock (triggers)
                {
                    events -= value;
                }
            }
        }

        #endregion

        #region IActionTriggerable Members

        void IActionTriggerable.Trigger(IActionTrigger trigger, float stateData)
        {
            Trigger(stateData);
        }

        #endregion
    }
}
