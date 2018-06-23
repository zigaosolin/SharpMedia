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

namespace SharpMedia.Components.Documents
{
    /// <summary>
    /// Interface for managing the document association
    /// </summary>
    public interface IDocumentManagementRegistry
    {
        /// <summary>
        /// Registers an application for usage with the specified type
        /// </summary>
        /// <param name="documentType">Full name of the document type</param>
        /// <param name="app">An application (path) that is assigned to this type</param>
        /// <param name="supportedVerbs">Supported verbs</param>
        void RegisterDocumentUsage(string documentType, string app, string[] supportedVerbs);

        /// <summary>
        /// Deletes application information from the registry
        /// </summary>
        /// <param name="appInfo">Application</param>
        void DeleteApplicationInformation(string appInfo);

        /// <summary>
        /// Deletes information about the specified type
        /// </summary>
        /// <param name="type">Type</param>
        void DeleteTypeInformation(string type);

        /// <summary>
        /// Selects a default action
        /// </summary>
        /// <param name="documentType">Document type</param>
        /// <param name="app">Application</param>
        /// <param name="verb">Verb</param>
        void SelectDefault(string documentType, string app, string verb);
    }
}
