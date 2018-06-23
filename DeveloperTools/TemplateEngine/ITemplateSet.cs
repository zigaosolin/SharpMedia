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
    /// A template resolver.
    /// </summary>
    public interface ITemplateResolver
    {
        /// <summary>
        /// Resolves template set.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        ITemplateSet Resolve(string reference);
    }

    /// <summary>
    /// A template.
    /// </summary>
    public interface ITemplateSet
    {
        /// <summary>
        /// The "global" name of set.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Available templates.
        /// </summary>
        string[] Available { get; }

        /// <summary>
        /// Resolves templates references.
        /// </summary>
        /// <param name="resolver"></param>
        void Resolve(ITemplateResolver resolver);

        /// <summary>
        /// Provides a template corresponding to the name. A template may be
        /// a string or ITemplate.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object Provide(string name);
    }
}
