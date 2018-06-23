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
    /// A shell command attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public sealed class ShellCommandAttribute : Attribute
    {
        #region Private Members
        bool caseSensitive = false;
        bool canParallel = false;
        #endregion

        #region Public Members

        public ShellCommandAttribute()
        {
        }

        /// <summary>
        /// Is command case sensitive.
        /// </summary>
        public bool CaseSensitive
        {
            get { return caseSensitive; }
            set { caseSensitive = value; }
        }

        /// <summary>
        /// Is command parallalizable.
        /// </summary>
        public bool CanParallel
        {
            get { return canParallel; }
            set { canParallel = value; }
        }

        #endregion
    }
}
