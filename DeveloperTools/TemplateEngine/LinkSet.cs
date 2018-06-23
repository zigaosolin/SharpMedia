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

namespace TemplateEngine
{
    /// <summary>
    /// A set that links to something.
    /// </summary>
    public class LinkSet : ITemplateSet
    {
        #region Private Members
        string name;
        ITemplateSet resolved;
        string link;
        #endregion

        #region Public Members

        public LinkSet(string name, string link)
        {
            this.name = name;
            this.link = link;
        }

        #endregion

        #region ITemplateSet Members

        public string Name
        {
            get { return name; }
        }

        public string[] Available
        {
            get { return resolved.Available; }
        }

        public void Resolve(ITemplateResolver resolver)
        {
            resolved = resolver.Resolve(link);
            if (resolved == null) throw new Exception(string.Format("Could not resolve {0} link", link));
        }

        public object Provide(string name)
        {
            return resolved.Provide(name);
        }

        #endregion
    }
}
