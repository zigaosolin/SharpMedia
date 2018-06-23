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

namespace SharpMedia.Tools.Graphical
{

    /// <summary>
    /// A window based (Shell) "application" for UI providing.
    /// </summary>
    public class GuiAutomaticUI : IAutomaticUI
    {

        #region IAutomaticUI Members

        public bool Run(Type toolName, SharpMedia.Tools.Parameters.IToolParameter[] toolParameters)
        {
            // FIXME: create a shell application with window and all parameters (with proper grouping).
            // The Configure button (or OK button) should only reveal itself after all the required
            // parameters have been specified. 

            // RULES: 
            //  - integer, strings etc are handled as edit boxes
            //  - integer/float with bounds is handled as 
            //  - enumerator is a combo box
            //  - node path is handled with a "built-in" search path dialog, similiary typed stream
            throw new NotImplementedException();
        }

        #endregion
    }
}
