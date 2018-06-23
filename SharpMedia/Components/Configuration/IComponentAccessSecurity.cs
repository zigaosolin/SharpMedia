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
using System.Text.RegularExpressions;

namespace SharpMedia.Components.Configuration
{
    /// <summary>
    /// Interface to control access to components
    /// </summary>
    public interface IComponentAccessSecurity
    {
        /// <summary>
        /// Sets permissions for access to named components
        /// </summary>
        /// <param name="name">Regular expression to match the name with</param>
        /// <param name="allow">Whether to allow access to the component names matched</param>
        /// <param name="allowParentLookup">Whether to allow falling back to the parent ICD</param>
        void SetNameAccess(Regex name, bool allow, bool allowParentLookup);

        /// <summary>
        /// Sets permissions for access to typed components
        /// </summary>
        /// <param name="type">Regular expression to match the type with</param>
        /// <param name="allow">Whether to allow access to the component names matched</param>
        /// <param name="allowParentLookup">Whether to allow falling back to the parent ICD</param>
        void SetTypeAccess(Regex type, bool allow, bool allowParentLookup);
    }
}
