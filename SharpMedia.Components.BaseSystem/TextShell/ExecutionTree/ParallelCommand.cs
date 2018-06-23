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

namespace SharpMedia.Components.BaseSystem.TextShell.ExecutionTree
{

    /// <summary>
    /// Parallel command.
    /// </summary>
    [Serializable]
    internal sealed class ParallelCommand : IExecutionCommand
    {
        #region Private Members
        IExecutionCommand commands1;
        IExecutionCommand commands2;
        #endregion

        #region Constructors

        public ParallelCommand(IExecutionCommand commands1, IExecutionCommand commands2)
        {
            this.commands1 = commands1;
            this.commands2 = commands2;
        }

        #endregion

        #region IExecutionCommand Members

        public int Exec(ExecutionContext context, out object[] inlineOutput)
        {
            // Execute first one in silent context.
            ExecutionContext parallelContext = 
                context.ShellApp.CreateParallelContext(context, commands1);

            object[] dummy;
            commands1.Exec(parallelContext, out dummy);
            

            // Last one is triggered normally (if invoked by some other parallel, context is
            // parallel anyways).
            return commands2.Exec(context, out inlineOutput);
        }

        #endregion
    }
}
