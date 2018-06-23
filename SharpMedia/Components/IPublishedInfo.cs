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

namespace SharpMedia.Components
{
    /// <summary>
    /// Publishing information. Any objects that are published should implement this
    /// </summary>
    public interface IPublishedInfo
    {
        /// <summary>
        /// Name of the publishing entity that has published the object
        /// </summary>
        string PublisherName { get; }

        /// <summary>
        /// Authors of the object
        /// </summary>
        string[] Authors { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IPublishedInfoAuthoring : IPublishedInfo
    {
        new string PublisherName { set; }
        new string[] Authors { set; }
    }
}
