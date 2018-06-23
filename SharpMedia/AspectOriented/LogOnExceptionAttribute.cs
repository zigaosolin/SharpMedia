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
//using PostSharp.Laos;

namespace SharpMedia.AspectOriented
{

    /// <summary>
    /// Logs on exception attribute.
    /// </summary>
    [Serializable]
    public sealed class LogOnExceptionAttribute //: OnExceptionAspect
    {
        #region Private Members
        string trace;
        string message;
        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        public LogOnExceptionAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
        /*
        public override void CompileTimeInitialize(System.Reflection.MethodBase method)
        {
            base.CompileTimeInitialize(method);

            trace = method.Name;
        }

        public override void OnException(MethodExecutionEventArgs eventArgs)
        {
            Common.Warning(eventArgs.Method.ReflectedType, string.Format("{0}:{1}", trace, message));
        }*/

        #endregion
    }
}
