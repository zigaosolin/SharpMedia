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

using SharpMedia.Components.Applications;

namespace SharpMedia.Components.Kernel
{
    /// <summary>
    /// Additions to the application interface for kernel usage
    /// </summary>
    internal interface IOSApplication : IApplicationBase
    {
        /// <summary>
        /// Informs the application that it should terminate. When this call returns, the 
        /// application domain it is residing in may be unloaded.
        /// </summary>
        bool Kill();

        /// <summary>
        /// Internal application callback.
        /// </summary>
        IApplicationCallback Callback { set; }
    }
}
