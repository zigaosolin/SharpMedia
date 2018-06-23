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
using System.Collections.Specialized;
using SharpMedia;
using SharpMedia.AspectOriented;

namespace SharpMedia.Database.Aspects
{

    /// <summary>
    /// An abstraction layer to support remote access, thread safety and more.
    /// </summary>
    internal interface INode
    {

        IDriverNode DriverAspect
        {
            get;
        }

        /// <summary>
        /// The name of node. This name is always just current, never full name.
        /// </summary>
        string Name 
        {
            [param:NotEmpty]
            set;
            get;
        }

        /// <summary>
        /// Obtains full path of node.
        /// </summary>
        string Path
        {
            get;
        }

        /// <summary>
        /// Default type of node.
        /// </summary>
        Type DefaultType
        {
            get;
        }

        /// <summary>
        /// Number of bytes used to hold node data together. This includes all version
        /// data and children pointers, but no TS data because TS have their own byte
        /// overhead getter.
        /// </summary>
        ulong ByteOverhead
        {
            get;
        }

        /// <summary>
        /// Opens default stream with mode and access node.
        /// </summary>
        /// <param name="mode">One of modes to open the stream.</param>
        /// <returns>The instance of typed stream.</returns>
        ITypedStream OpenDefaultStream(OpenMode mode);
     

        /// <summary>
        /// Obtains typed stream.
        /// </summary>
        /// <param name="mode">The opening mode of typed stream.</param>
        /// <param name="t">The type of stream.</param>
        /// <returns>Instance of typed stream.</returns>
        ITypedStream GetTypedStream(OpenMode mode, [NotNull] Type t);

	
        /// <summary>
        /// Adds typed stream to INode.
        /// </summary>
        /// <param name="t">The type of stream. If type already exists, exception is thrown.</param>
        /// <param name="flags">The flags for newely created stream.</param>
	    void AddTypedStream([NotNull] Type t, StreamOptions flags);
	
	
        /// <summary>
        /// Removes the typed stream from node. All objects are immediatelly lost.
        /// </summary>
        /// <remarks>
        /// No thread must be have this typed stream open or this method fails. User
        /// must have enogh priorities.
        /// </remarks>
        /// <param name="t">The type identifier of stream.</param>
        void RemoveTypedStream([NotNull] Type t);

   
        /// <summary>
        /// Changes the default typed stream to another (existing) typed stream.
        /// </summary>
        /// <param name="t">The stream identifier that exists in this INode.</param>
        void ChangeDefaultStream([NotNull] Type t);
  
	
	    /// <summary>
	    /// The version of this node and all it's data. Children are shared between versions.
        /// The version 1 indicated the first version (and means that node has no previous version).
	    /// </summary>
        ulong Version
        {
            get;
        }

	    /// <summary>
	    /// The previous version of this node, null if none. The previous version is not previous
        /// index, but previous existing version (you can walk through all versions this way).
	    /// </summary>
        INode PreviousVersion
        {
            get;
        }

        /// <summary>
        /// Obtains node of that version.
        /// </summary>
        /// <param name="version">The version to obtain.</param>
        /// <returns>The INode of version, or null if it does not exist.</returns>
        INode this[ulong version]
        {
            get;
        }

        /// <summary>
        /// Creates new version of this node.
        /// </summary>
        /// <param name="defaultType">Type of the default.</param>
        /// <param name="options">The options.</param>
        /// <returns>New version node.</returns>
        INode CreateNewVersion([NotNull] Type defaultType, StreamOptions options);
        
	
	    /// <summary>
	    /// The parent of this node.
	    /// </summary>
        INode Parent
        {
            get;
        }
	    

        /// <summary>
        /// Finds one of the parents. The format can be scoped, using '/' as a seperator.
        /// '.' means the same node, '..' means parent node. If name cannot be resolved,
        /// null is returned (no exception is thrown).
        /// </summary>
        /// <param name="accessNode">Security information.</param>
        /// <returns>The node at that path, or null if not found.</returns>
        INode Find([NotNull] string path);

        /// <summary>
        /// Creates a child on this node.
        /// </summary>
        /// <param name="name">The name of child. Name must not exist and must not
        /// include the characters '/', '?', '[' and ']'</param>
        /// <param name="defaultType">The default type of node and also default stream.</param>
        /// <param name="flags">Flags for default stream.</param>
        /// <returns>The newly craeted child.</returns>
        INode CreateChild([NotNull] string name, [NotNull] Type defaultType, StreamOptions flags);


        /// <summary>
        /// Deletes the child.
        /// </summary>
        /// <param name="name">Name must be valid child.</param>
        void DeleteChild([NotEmpty] string name);


        /// <summary>
        /// Moves the node to new parent.
        /// </summary>
        /// <param name="node">The new parent..</param>
        /// <returns>The returned node instance.</returns>
        INode MoveTo([NotNull] INode newParent);

        /// <summary>
        /// Copies the node to parent.
        /// </summary>
        /// <param name="newParent">The node where to copy.</param>
        /// <returns>The node copied.</returns>
        INode CopyTo([NotNull] INode newParent);

        /// <summary>
        /// Deletes a specified version. Version must be valid. Cannot delete current version,
        /// version from where you are calling it or any version that is being read.
        /// </summary>
        /// <param name="version">The version's index.</param>
        void DeleteVersion(ulong version);

        /// <summary>
        /// The creation time.
        /// </summary>
        DateTime CreationTime
        {
            get;
        }

        /// <summary>
        /// Last modified property.
        /// </summary>
        DateTime LastModifiedTime
        {
            get;
        }

        /// <summary>
        /// Last read time.
        /// </summary>
        DateTime LastReadTime
        {
            get;
        }

        /// <summary>
        /// Obtains physical location of node.
        /// </summary>
        string PhysicalLocation
        {
            get;
        }

        /// <summary>
        /// Typed stream supported by this node.
        /// </summary>
        Type[] TypedStreams
        {
            get;
        }

        /// <summary>
        /// Children of this node, represented as strings.
        /// </summary>
        StringCollection Children
        {
            get;
        }

        /// <summary>
        /// Obtains all available versions.
        /// </summary>
        ulong[] AvailableVersions
        {
            get;
        }

        /// <summary>
        /// Base node, this node inherits from it. It can be used as an smbolic link. Null
        /// is returned if base node does not exist or cannot be addressed. Links always
        /// to most current version.
        /// </summary>
        /// <remarks>
        /// The driver should leave this property blank.
        /// </remarks>
        INode Base
        {
            get;
            [param:NotNull]
            set;
        }

        /// <summary>
        /// Almost the same as base but links to specified version and new versions do not
        /// alter the link.
        /// </summary>
        /// <remarks>
        /// The driver should leave this property blank.
        /// </remarks>
        INode BaseWithVersion
        {
            [param:NotNull]
            set;
        }
    }
}
