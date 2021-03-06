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

namespace SharpMedia.Components.BaseSystem.TextShell
{
    /// <summary>
    /// An execution context.
    /// </summary>
    public sealed class ExecutionContext : ICloneable<ExecutionContext>
    {
        #region Public Members

        // Console communication.
        public System.IO.TextReader Input;
        public System.IO.TextWriter Output;
        public System.IO.TextWriter Error;

        // Should we wait for end.
        public bool IsParallel = false;

        public ShellApp ShellApp;

        #endregion

        #region ICloneable<ExecutionContext> Members

        public ExecutionContext Clone()
        {
            ExecutionContext clone = new ExecutionContext();
            clone.Input = Input;
            clone.Output = Output;
            clone.Error = Error;

            clone.ShellApp = ShellApp;
            

            return clone;
        }

        #endregion
    }
}
