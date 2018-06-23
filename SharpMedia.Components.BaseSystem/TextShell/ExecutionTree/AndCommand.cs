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
    /// And command.
    /// </summary>
    [Serializable]
    internal sealed class AndCommand : IExecutionCommand
    {
        #region Private Members
        IExecutionCommand first;
        IExecutionCommand second;
        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public AndCommand(IExecutionCommand first, IExecutionCommand second)
        {
            this.first = first;
            this.second = second;
        }

        #endregion

        #region IExecutionCommand Members

        public int Exec(ExecutionContext context, out object[] inlineOutput)
        {
            // We execute first command.
            int result = first.Exec(context, out inlineOutput);

            // If suceeded, we execute other command.
            if (result >= 0)
            {
                result = second.Exec(context, out inlineOutput);
            }

            return result;
        }

        #endregion
    }
}
