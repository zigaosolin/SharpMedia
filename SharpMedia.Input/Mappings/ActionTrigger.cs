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

namespace SharpMedia.Input.Mappings
{

    /// <summary>
    /// A triggerable.
    /// </summary>
    public interface IActionTriggerable
    {

        /// <summary>
        /// A trigger signal.
        /// </summary>
        /// <param name="trigger"></param>
        void Trigger(IActionTrigger trigger, float stateData);

    }

    /// <summary>
    /// An action trigger.
    /// </summary>
    /// <remarks>Action should implement this methods explicitly.</remarks>
    public interface IActionTrigger
    {
        /// <summary>
        /// Initializes trigger with input service.
        /// </summary>
        /// <remarks>If service is null, this means that trigger must be released.</remarks>
        /// <param name="service"></param>
        void Initialize(EventProcessor processor, bool bind);

        /// <summary>
        /// Binds trigger to action (an be null).
        /// </summary>
        /// <param name="action"></param>
        void BindTo(IActionTriggerable action);

    }
}
