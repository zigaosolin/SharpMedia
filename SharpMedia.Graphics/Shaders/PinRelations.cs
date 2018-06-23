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

namespace SharpMedia.Graphics.Shaders
{
    /// <summary>
    /// A pin relation, it can be checked.
    /// </summary>
    public interface IPinRelation
    {
        /// <summary>
        /// To which pins does it apply.
        /// </summary>
        PinDescriptor[] AppliesTo { get; }

        /// <summary>
        /// Validates if relation holds.
        /// </summary>
        /// <param name="pins"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Validate(Pin[] pins, out string message);

    }

    /// <summary>
    /// Pins must be equal.
    /// </summary>
    public sealed class PinEqual : IPinRelation
    {
        #region Private Members
        PinDescriptor[] descs;
        #endregion

        #region Public Members

        /// <summary>
        /// Creates pin equal relation.
        /// </summary>
        /// <param name="descs"></param>
        public PinEqual(params PinDescriptor[] descs)
        {
            this.descs = descs;
        }

        #endregion

        #region IPinRelations Members

        public PinDescriptor[] AppliesTo
        {
            get { return descs; }
        }

        public bool Validate(Pin[] pins, out string message)
        {
            // All pins must be equal.
            for (int i = 1; i < pins.Length; i++)
            {
                if (!pins[i].Equals(pins[i - 1]))
                {
                    message = string.Format("Pins {0} and {1} are not equal.", pins[i - 1], pins[i]);
                    return false;
                }
            }

            message = null;
            return true;
        }

        #endregion
    }

    /// <summary>
    /// Pins are multiplicable.
    /// </summary>
    public sealed class PinMultipliable : IPinRelation
    {
        #region Private Members
        PinDescriptor src1;
        PinDescriptor src2;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates pin multiplicable relation.
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        public PinMultipliable(PinDescriptor src1, PinDescriptor src2)
        {
            this.src1 = src1;
            this.src2 = src2;
        }

        #endregion

        #region IPinRelations Members

        public PinDescriptor[] AppliesTo
        {
            get { return new PinDescriptor[] { src1, src2 }; }
        }

        public bool Validate(Pin[] pins, out string message)
        {
            message = null;
            return true;
        }

        #endregion
    }

    /// <summary>
    /// Texture and address are compatible.
    /// </summary>
    public sealed class PinTextureAddressable : IPinRelation
    {
        #region Private Members
        PinDescriptor src1;
        PinDescriptor src2;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates pin multiplicable relation.
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        public PinTextureAddressable(PinDescriptor src1, PinDescriptor src2)
        {
            this.src1 = src1;
            this.src2 = src2;
        }

        #endregion

        #region IPinRelations Members

        public PinDescriptor[] AppliesTo
        {
            get { return new PinDescriptor[] { src1, src2 }; }
        }

        public bool Validate(Pin[] pins, out string message)
        {
            message = null;
            return true;
        }

        #endregion
    }

    /// <summary>
    /// Texture and address are compatible.
    /// </summary>
    public sealed class PinTextureMipmappedAddressable : IPinRelation
    {
        #region Private Members
        PinDescriptor src1;
        PinDescriptor src2;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates pin multiplicable relation.
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        public PinTextureMipmappedAddressable(PinDescriptor src1, PinDescriptor src2)
        {
            this.src1 = src1;
            this.src2 = src2;
        }

        #endregion

        #region IPinRelations Members

        public PinDescriptor[] AppliesTo
        {
            get { return new PinDescriptor[] { src1, src2 }; }
        }

        public bool Validate(Pin[] pins, out string message)
        {
            
            message = null;
            return true;
        }

        #endregion
    }


}
