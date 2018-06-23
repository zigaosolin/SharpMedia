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
    /// Application Type
    /// </summary>
    public enum DocumentSupport
    {
        /// <summary>
        /// The application supports a single document. Multiple Documents will be opened in parallel applications.
        /// </summary>
        /// <remarks>This implies that the application will receive at most one Start request</remarks>
        SingleDocument,

        /// <summary>
        /// The application supports multiple documents. Calls to multiple documents using Start are supported.
        /// </summary>
        MultipleDocuments
    }

    /// <summary>
    /// Application information
    /// </summary>
    public interface IApplicationInfo : IPublishedInfo
    {
        /// <summary>
        /// Name of the Application
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Unique identifier of this application
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Friendly name, as passed on to the user
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Type of the application
        /// </summary>
        DocumentSupport DocumentSupport { get; }
    }

    /// <summary>
    /// Information about a running application
    /// </summary>
    public interface IApplicationInstance
    {
        /// <summary>
        /// Serial number of the application instance
        /// </summary>
        ulong InstanceNumber { get; }

        /// <summary>
        /// Unique Instance identifier
        /// </summary>
        Guid  InstanceId     { get; }

        /// <summary>
        /// Obtain the application information
        /// </summary>
        IApplicationInfo ApplicationInfo { get; }

        /// <summary>
        /// True if the application instance is still running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// When IsRunning is false, and ExitException is null,
        /// the result that the application instance has returned
        /// </summary>
        int ExitResult { get; }

        /// <summary>
        /// Exception thrown by the execution of the application 
        /// </summary>
        Exception ExitException { get; }

        // FIXME: Add events :]
    }
}