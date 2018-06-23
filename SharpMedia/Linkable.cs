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

namespace SharpMedia
{

    /// <summary>
    /// The link mask.
    /// </summary>
    [Flags]
    public enum LinkMask
    {
        /// <summary>
        /// No target for linking - only inside class. 
        /// </summary>
        /// <remarks>It is better to use internal resources for that.</remarks>
        NoTarget = 0,

        /// <summary>
        /// Drivers can access this code.
        /// </summary>
        Drivers = 1,

        /// <summary>
        /// Application with security level 1 can use code.
        /// </summary>
        SecurityLevel1 = 2,

        /// <summary>
        /// Application with security level 2 can use code.
        /// </summary>
        SecurityLevel2 = 4,

        /// <summary>
        /// Applications can access code.
        /// </summary>
        Application = 8,

        /// <summary>
        /// Allows cache controlling.
        /// </summary>
        /// <remarks>
        /// Cache controlling are services or libraries that hold resources in cache. This flag is
        /// usually applied to public methods that must be accessed by those caches but should not
        /// be accessed outside cache controller. For example, Cached method in ICacheable should
        /// only be called by cache and not by user code.
        /// </remarks>
        CacheControlling = 16,

        /// <summary>
        /// Only kernel has control.
        /// </summary>
        Kernel = 32,

        /// <summary>
        /// All linkage is allowed.
        /// </summary>
        All = Drivers|SecurityLevel1|SecurityLevel2|Application|CacheControlling

    }

    /// <summary>
    /// Linkable attribute is used to tag specific classes if they are accessable and what priviliges are
    /// required in order to be accessable.
    /// </summary>
    /// <remarks>If multiple Linkable attributes are defined (one at assembly level, one at class level),
    /// the attributes are combined by default. You can apply </remarks>
    [AttributeUsage(AttributeTargets.All, AllowMultiple=false,Inherited=true)]
    public class LinkableAttribute : Attribute
    {
        #region Private Members
        bool combine = true;
        LinkMask mask = LinkMask.All;
        #endregion

        #region Public Methods

        /// <summary>
        /// The link mask.
        /// </summary>
        public LinkMask LinkMask
        {
            get
            {
                return mask;
            }
        }

        /// <summary>
        /// Is the linkable combined with attributes from higher scope.
        /// </summary>
        public bool Combine
        {
            get
            {
                return combine;
            }
            set
            {
                combine = value;
            }
        }

        /// <summary>
        /// Constructor with usage.
        /// </summary>
        /// <param name="linkMask"></param>
        public LinkableAttribute(LinkMask linkMask)
        {
            mask = linkMask;
        }

        #endregion
    }
}
