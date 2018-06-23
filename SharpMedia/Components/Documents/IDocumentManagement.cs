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

using SharpMedia.Components.Database;

namespace SharpMedia.Components.Documents
{
    public class DocumentVerb
    {
        /// <summary>
        /// The document will be opened (no distinction between view and edit)
        /// </summary>
        public static readonly string Open    = "open";

        /// <summary>
        /// The document will be opened for preview
        /// </summary>
        public static readonly string Preview = "preview";

        /// <summary>
        /// The document will be opened for editing
        /// </summary>
        public static readonly string Edit    = "edit";

        /// <summary>
        /// The document will be opened for printing
        /// </summary>
        public static readonly string Print   = "print";

        /// <summary>
        /// The document will be closed in all applications having it open
        /// </summary>
        public static readonly string Close   = "close";
    }

    /// <summary>
    /// Manages documents
    /// </summary>
    public interface IDocumentManagement
    {
        /// <summary>
        /// Lists available applications for a given document type
        /// </summary>
        /// <param name="type">The document type</param>
        /// <param name="verb">The verb to use on the document</param>
        /// <returns>List of applications that match the search criteria</returns>
        string[] AvailableApplications(string type, string verb);

        /// <summary>
        /// Lists Available verbs on the specified document type
        /// </summary>
        /// <param name="app">Application to use, may be null if we are interested in all/any applications</param>
        /// <param name="type">The document type</param>
        /// <returns>List of verbs that the specified application(s) support</returns>
        string[] AvailableVerbs(string app, string type);
    }
}
