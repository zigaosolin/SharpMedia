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
using SharpMedia.Testing;
using System.Collections.ObjectModel;
using SharpMedia.Math;

namespace SharpMedia.Input.Mappings
{

    /// <summary>
    /// Action mapping allows to map actions in very flexible way.
    /// </summary>
    [Serializable]
    public sealed class ActionMapping : IEnumerable<Action>
    {
        #region Private Members
        [NonSerialized]
        EventProcessor processor;
        [NonSerialized]
        uint processingFrame = 0;

        // Actions.
        SortedList<string, Action> actions = new SortedList<string, Action>();
        #endregion

        #region Private Methods

        private void InitializeActions(bool bind)
        {
            foreach (Action action in actions.Values)
            {
                action.Initialize(processor, bind);
            }
        }

        #endregion

        #region Constructors

        public ActionMapping()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Using this id, triggers can identify if action was triggered in this frame.
        /// </summary>
        public uint ProcessingFrame
        {
            get
            {
                return processingFrame;
            }
        }

        /// <summary>
        /// Gets a named action.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Action this[string name]
        {
            get
            {
                lock (actions)
                {
                    return actions[name];
                }
            }
        }

        /// <summary>
        /// Adds an action.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Action AddAction(string name)
        {
            lock (actions)
            {
                if (actions.IndexOfKey(name) >= 0)
                {
                    throw new ArgumentException("Action with such name already exists.");
                }

                Action action = new Action(this, name);
                actions.Add(name, action);
                return action;
            }
        }

        /// <summary>
        /// Removes an action.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RemoveAction(string name)
        {
            lock (actions)
            {
                return actions.Remove(name);
            }
        }

        /// <summary>
        /// An input provider.
        /// </summary>
        public EventProcessor InputProvider
        {
            get
            {
                return processor;
            }
            set
            {
                lock (actions)
                {
                    processingFrame++;
                    if (processor != null)
                    {
                        InitializeActions(false);
                    }

                    processor = value;
                    InitializeActions(true);
                }
            }
        }

        /// <summary>
        /// Processes all/some events. Triggers are fired and actions fire their own events.
        /// </summary>
        public void Process(uint maxEvents)
        {
            for (int i = 0; i < maxEvents; i++)
            {
                if (processor.Process() == null) break;
            }
        }

        #endregion

        #region IEnumerable<Action> Members

        public IEnumerator<Action> GetEnumerator()
        {
            return actions.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return actions.Values.GetEnumerator();
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class ActionMappingTest
    {
        Vector2f position;

        [CorrectnessTest]
        public void Usage()
        {
            InputService service = new InputService(null);
            EventPump pump = new EventPump(service.CreateDevice(InputDeviceType.Mouse),
                                           service.CreateDevice(InputDeviceType.Keyboard));
           
            // We create action mapping.
            ActionMapping mapping = new ActionMapping();
            mapping.InputProvider = new EventProcessor(pump);

            // We create some actions.
            Action castFirebolt = mapping.AddAction("Firebolt");
            castFirebolt.Description = "Cast a firebolt. Damage: 2500";
            castFirebolt.AddTrigger(new ButtonTrigger(KeyCodes.T, true));
            castFirebolt.AddTrigger(new ButtonTrigger(KeyCodes.Key1, true));

            castFirebolt.Event += CastFirebolt;

            // For continious events (movement), 
            // you should use direct device pooling, not events.
        }

        void CastFirebolt(float state)
        {
            // State will always be 1.0
            // ...
        }


    }

#endif
}
