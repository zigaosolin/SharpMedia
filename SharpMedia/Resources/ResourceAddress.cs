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
using SharpMedia.AspectOriented;

namespace SharpMedia.Resources
{

    /// <summary>
    /// A full address of resource. 
    /// </summary>
    public class ResourceAddress : IEquatable<ResourceAddress>
    {
        #region Private Members
        string      computer;
        Placement   location;
        #endregion

        #region Properties

        /// <summary>
        /// Returns the location of address.
        /// </summary>
        public Placement Location
        {
            get
            {
                return location;
            }
        }

        /// <summary>
        /// Gets the name of the computer.
        /// </summary>
        /// <value>The name of the computer.</value>
        public string ComputerName
        {
            get
            {
                return computer;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAddress"/> class.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="location">The location.</param>
        public ResourceAddress([NotEmpty] string machineName, Placement location)
        {
            this.computer = machineName;
            this.location = location;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAddress"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        public ResourceAddress(Placement location)
        {
            this.computer = Environment.MachineName;
            this.location = location;
        }

        #endregion

        #region IEquatable<ResourceAddress> Members

        public bool Equals(ResourceAddress other)
        {
            if (this == other) return true;
            if (computer != other.computer) return false;
            if (location != other.location) return false;
            return true;
        }

        #endregion
    }
}
