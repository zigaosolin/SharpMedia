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
using System.IO;

namespace SharpMedia.Components.BaseSystem.TextShell.ExecutionTree
{
    /// <summary>
    /// Pipe-in command.
    /// </summary>
    [Serializable]
    internal sealed class PipeCommand : IExecutionCommand
    {
        #region Private Members
        IExecutionCommand inCommand;
        IExecutionCommand outCommand;
        #endregion

        #region Constructors

        /// <summary>
        /// Pipes.
        /// </summary>
        /// <param name="inCommand"></param>
        /// <param name="outCommand"></param>
        public PipeCommand(IExecutionCommand inCommand, IExecutionCommand outCommand)
        {
            this.inCommand = inCommand;
            this.outCommand = outCommand;
        }

        #endregion

        #region IExecutionCommand Members

        public int Exec(ExecutionContext context, out object[] inlineOutput)
        {
            // We wrap the output stream.
            MemoryStream outputStream = new MemoryStream();
            ExecutionContext context1 = context.ShellApp.CreateSubContext(context, inCommand);
            context1.Output = new System.IO.StreamWriter(outputStream);

            // We execute first command.
            int r = inCommand.Exec(context, out inlineOutput);
            if(r < 0)
            {
                return r;
            }

            // We execute other command.
            ExecutionContext context2 = context.ShellApp.CreateSubContext(context, outCommand);
            context2.Input = new System.IO.StreamReader(outputStream);

            return outCommand.Exec(context2, out inlineOutput);
        }

        #endregion
    }
}
