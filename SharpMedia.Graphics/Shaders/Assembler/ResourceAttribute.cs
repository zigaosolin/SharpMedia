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

namespace SharpMedia.Graphics.Shaders.Assembler
{

    /// <summary>
    /// The resource attribute, can be applied to texture or sampler input parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ResourceAttribute : Attribute
    {
        #region Private Members
        uint slot = uint.MaxValue;
        PinFormat resourceType = PinFormat.Sampler;
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAttribute"/> class.
        /// </summary>
        /// <param name="resourceType">Must be one of Sampler, Texture1D, Texture2D, Texture3D,
        /// Texture1DArray, Texture2DArray, TextureCube.</param>
        public ResourceAttribute(PinFormat resourceType)
        {
            this.resourceType = resourceType;
        }

        /// <summary>
        /// Gets or sets the slot where resource is placed. This value is optional, slot is
        /// otherwise determined by the runtime.
        /// </summary>
        /// <value>The slot.</value>
        public uint Slot
        {
            set { slot = value; }
            get { return slot; }
        }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <value>The type of the resource.</value>
        public PinFormat ResourceType
        {
            get { return resourceType; }
        }

        #endregion

    }
}
