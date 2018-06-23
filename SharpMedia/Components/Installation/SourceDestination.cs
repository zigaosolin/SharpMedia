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
using SharpMedia.Database;

namespace SharpMedia.Components.Installation
{
    /// <summary>
    /// Interfaces implementing this interface use the installation environment 
    /// </summary>
    public interface UsesInstallEnvironment
    {
        /// <summary>
        /// Installation environment to use
        /// </summary>
        InstallEnvironment InstallEnvironment { set; }
    }

    /// <summary>
    /// Denotes an installation source
    /// </summary>
    public interface InstallSource : UsesInstallEnvironment
    {
        /// <summary>
        /// Array of types supported by this source
        /// </summary>
        Type[] Types { get; }

        /// <summary>
        /// Opens a typed stream for reading, using a specified type
        /// </summary>
        TypedStream<T> OpenForReading<T>();

        /// <summary>
        /// Opens a typed stream for reading, using a specified type
        /// </summary>
        /// <param name="t">Type of TypedStream to open</param>
        TypedStream<Object> OpenForReading(Type t);
    }

    /// <summary>
    /// Denotes an installation destination
    /// </summary>
    public interface InstallDestination : UsesInstallEnvironment
    {
        /// <summary>
        /// True if the destination exists
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Deletes the destination
        /// </summary>
        void Delete();

        /// <summary>
        /// Opens a typed stream for writing data on the destination
        /// </summary>
        TypedStream<T> OpenForWriting<T>();

        /// <summary>
        /// Opens a typed stream for writing data on the destination
        /// </summary>
        TypedStream<object> OpenForWriting(Type t);

        /// <summary>
        /// Returns true if the type t exists in this destination
        /// </summary>
        bool ExistsType(Type t);

        /// <summary>
        /// Deletes a type t from the destination
        /// </summary>
        bool DeleteType(Type t);

        /// <summary>
        /// Installation source form of this installation destination. Only valid if Exists is true
        /// </summary>
        InstallSource AsSource { get; }
    }
}
