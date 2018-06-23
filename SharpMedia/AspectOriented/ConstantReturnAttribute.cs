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

namespace SharpMedia.AspectOriented
{
    /// <summary>
    /// This attribute can be applied to get properties or any method that returns some value.
    /// It checks (at compile time) that the value returned is constant - e.g. not computed or
    /// based on something else.
    /// </summary>
    public class TypeConstantReturnAttribute : Attribute
    {
    }
}
