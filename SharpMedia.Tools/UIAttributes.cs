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
using SharpMedia.Components.Applications;
using System.Text.RegularExpressions;
using SharpMedia.Database;

namespace SharpMedia.Tools
{

    /// <summary>
    /// An UI attribute base class, for all kinds of UIs.
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.Property,
        AllowMultiple = false, Inherited = true)]
    public class UIAttribute : Attribute
    {
        #region Protected Members
        bool isOptional = false;
        string uiGroup = null;
        string friendlyName = null;
        string description = null;
        #endregion

        #region Properties

        /// <summary>
        /// Is the attribute optional.
        /// </summary>
        /// <remarks>The tool UI will force to provide this attribute, even if not required
        /// by application.</remarks>
        public bool IsOptional
        {
            get { return isOptional; }
            set { isOptional = value; }
        }

        /// <summary>
        /// UI group, depends on representation.
        /// </summary>
        public string UIGroup
        {
            get { return uiGroup; }
            set { uiGroup = value; }
        }

        /// <summary>
        /// Description of attribute.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Friendly name of attribute.
        /// </summary>
        public string FriendlyName
        {
            get { return friendlyName; }
            set { friendlyName = value; }
        }

        #endregion

    }


    /// <summary>
    /// A node attribute.
    /// </summary>
    /// <remarks>Most UIs will provide a string->node conversion and validation.</remarks>
    public class NodeUIAttribute : UIAttribute
    {
        #region Private Members
        Type defaultType = null;
        Type[] reqTypes = new Type[0];
        #endregion

        #region Properties

        /// <summary>
        /// The default type of node.
        /// </summary>
        public Type DefaultType
        {
            get { return defaultType; }
            set { defaultType = value; }
        }

        /// <summary>
        /// Required types on node.
        /// </summary>
        public Type[] RequiredTypes
        {
            get { return reqTypes; }
            set { reqTypes = value; }
        }

        #endregion
    }

    /// <summary>
    /// A typed stream attribute.
    /// </summary>
    public class TypedStreamUIAttribute : UIAttribute
    {
        #region Private Members
        uint minObjects = 0;
        uint maxObjects = uint.MaxValue;
        Type type = typeof(object);
        bool disallowDerived = false;
        OpenMode openMode = OpenMode.ReadWrite;
        #endregion

        #region Properties

        /// <summary>
        /// The open mode.
        /// </summary>
        public OpenMode OpenMode
        {
            get { return openMode; }
            set { openMode = value; }
        }

        /// <summary>
        /// Sets the explicit object count.
        /// </summary>
        public uint RequiredObjectCount
        {
            set { minObjects = maxObjects = value; }
        }

        /// <summary>
        /// Minimum number of objects.
        /// </summary>
        public uint MinObjectCount
        {
            get { return minObjects; }
            set { minObjects = value; }
        }

        /// <summary>
        /// Maximum number of objects.
        /// </summary>
        public uint MaxObjectCount
        {
            get { return maxObjects; }
            set { maxObjects = value; }
        }

        /// <summary>
        /// The type of attribute.
        /// </summary>
        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Are derived types allowed.
        /// </summary>
        public bool DissallowDerivedTypes
        {
            get { return disallowDerived; }
            set { disallowDerived = value; }
        }

        #endregion
    }

}
