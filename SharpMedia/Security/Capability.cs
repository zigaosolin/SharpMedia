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

namespace SharpMedia.Security
{
    /// <summary>
    /// A security capability
    /// </summary>
    public class Capability
    {
    }

    /// <summary>
    /// This capability allows the object it has been granted to grant capabilities,
    /// in general, a capability that allows granting of other capabilities should derive
    /// from this class and provide an override for the GrantsCapabilities property
    /// </summary>
    [Serializable]
    public class GrantingCapability : Capability
    {
        public virtual IEnumerable<Capability> GrantsCapabilities
        {
            get { return null;  }
        }
    }

    /// <summary>
    /// This capability allows the object it has been granted to revoke capabilities.
    /// in general, a capability that allows revoking of other capabilities should derive
    /// from this class and provide an override for the RevokesCapabilities property
    /// </summary>
    [Serializable]
    public class RevocationCapability : Capability
    {
        public virtual IEnumerable<Capability> RevokesCapabilities
        {
            get { return null; }
        }
    }

    /// <summary>
    /// This capability allows the object it has been granted to query which capabilities
    /// have been granted to the object, through the usage of the ISecurityEngine.Can method
    /// </summary>
    [Serializable]
    public class QueryCapabilties : Capability
    {
    }
}
