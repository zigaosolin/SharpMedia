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

namespace SharpMedia.Components.Applications
{
    /// <summary>
    /// Interface used by an IApplication to hook calls back to the OS
    /// </summary>
    public interface IApplicationCallback
    {
        /// <summary>
        /// Informs the OS that the application has exited normally, with a return value
        /// </summary>
        void Exited(IApplicationInstance sender, int returnValue);

        /// <summary>
        /// Informs the OS that the application has exited abnormally, with an exception
        /// </summary>
        void Exited(IApplicationInstance sender, Exception exception);
    }

    /// <summary>
    /// Interface implemented by all applications
    /// </summary>
    public interface IApplicationBase 
    {
        /// <summary>
        /// Starts an application.
        /// </summary>
        /// <param name="verb">The verb application was start with.</param>
        /// <param name="arguments">Additional arguments.</param>
        /// <returns>Exit code.</returns>
        int Start(string verb, params string[] arguments);

    }


}
