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
using System.Runtime.Serialization;

namespace SharpMedia.Graphics.Shaders
{
    /// <summary>
    /// Thrown when pin inputs are not compatible.
    /// </summary>
    [Serializable]
    public class InvalidPinException : Exception
    {
        public InvalidPinException() { }
        public InvalidPinException(string message) : base(message) { }
        public InvalidPinException(string message, Exception inner) : base(message, inner) { }
        protected InvalidPinException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }

    /// <summary>
    /// Each operation should statically allocate one descriptor for input and one for output
    /// pin. Internal requirements are described in this descriptor.
    /// </summary>
    public sealed class PinsDescriptor
    {
        #region Private Members
        IList<PinDescriptor> pins;
        IList<IPinRelation> reqs;
        #endregion

        #region Properties

        /// <summary>
        /// An empty (immutable) pin.
        /// </summary>
        public static PinsDescriptor Empty = new PinsDescriptor(new PinDescriptor[0]);

        /// <summary>
        /// Obtains pin descriptor at index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Pin descriptor.</returns>
        public PinDescriptor this[uint index]
        {
            get
            {
                return pins[(int)index];
            }
        }

        /// <summary>
        /// Number of elements.
        /// </summary>
        public uint Count
        {
            get
            {
                return (uint)pins.Count;
            }
        }

        /// <summary>
        /// Obtains read-only requirements.
        /// </summary>
        public IList<IPinRelation> RequiredRelations
        {
            get
            {
                return reqs;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// PinsDescriptor construction.
        /// </summary>
        public PinsDescriptor(params PinDescriptor[] desc)
        {
            pins = Array.AsReadOnly(desc);
            reqs = Array.AsReadOnly(new IPinRelation[] { });
        }

        /// <summary>
        /// A pins descriptor.
        /// </summary>
        /// <param name="req">A requirement array.</param>
        /// <param name="desc">The actual descriptors.</param>
        public PinsDescriptor(IPinRelation[] req, params PinDescriptor[] desc)
        {
            pins = Array.AsReadOnly(desc);
            reqs = Array.AsReadOnly(req);
        }

        /// <summary>
        /// A pins descriptor.
        /// </summary>
        /// <param name="req">A requirement array.</param>
        /// <param name="desc">The actual descriptors.</param>
        public PinsDescriptor(IPinRelation req, params PinDescriptor[] desc)
        {
            pins = Array.AsReadOnly(desc);
            reqs = Array.AsReadOnly(new IPinRelation[] { req });
        }


        #endregion

        #region Methods

        /// <summary>
        /// Validates input pins against pin descriptor.
        /// </summary>
        /// <param name="pins">The pins.</param>
        /// <returns>Is it compatible.</returns>
        public bool Validate([NotNull] params Pin[] pins)
        {
            return true;
        }

        /// <summary>
        /// Validate input and returns message.
        /// </summary>
        /// <param name="pins"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Validate([NotNull] Pin[] pins, out string message)
        {
            message = null;
            return true;
        }

        /// <summary>
        /// Validates input pins against pin descriptor and throws in
        /// case of failure.
        /// </summary>
        /// <param name="pins">Input pins.</param>
        public void ValidateThrow([NotNull] params Pin[] pins)
        {
            string message;
            if (!Validate(pins, out message))
            {
                throw new InvalidPinException(message);
            }
        }

        #endregion
    }

    /// <summary>
    /// Pin descriptor, when operations describe them. It is immutable; e.g. can
    /// be shared by many pins. 
    /// </summary>
    /// <remarks>If Pin.DynamicArray is used, the array can be either dynamic or static.</remarks>
    public sealed class PinDescriptor
    {
        #region Private Members
        bool isOptional = false;
        uint size = Pin.NotArray;
        PinFormat[] formats = null;
        string symbolicName;
        PinComponent component = PinComponent.None;
        #endregion

        #region Constructors

        /// <summary>
        /// One format descriptor.
        /// </summary>
        public PinDescriptor(PinFormat fmt, [NotNull] string desc)
        {
            formats = new PinFormat[] { fmt };
            symbolicName = desc;
        }

        /// <summary>
        /// One format descriptor.
        /// </summary>
        public PinDescriptor(PinFormat fmt, [NotNull] string desc, uint arraySize)
        {
            formats = new PinFormat[] { fmt };
            symbolicName = desc;
            size = arraySize;
        }

        /// <summary>
        /// One format descriptor.
        /// </summary>
        public PinDescriptor(PinFormat fmt, [NotNull] string desc, PinComponent c)
        {
            formats = new PinFormat[] { fmt };
            symbolicName = desc;
            component = c;
        }

        /// <summary>
        /// One format descriptor and optional descriptor.
        /// </summary>
        public PinDescriptor(PinFormat fmt, bool optional, [NotNull] string desc)
            : this(fmt, desc)
        {
            isOptional = optional;
        }


        /// <summary>
        /// Multiple formats.
        /// </summary>
        public PinDescriptor([NotEmptyArray] PinFormat[] fmts, [NotNull] string desc)
        {
            formats = fmts;
            symbolicName = desc;
        }

        /// <summary>
        /// Multiple formats.
        /// </summary>
        public PinDescriptor([NotEmptyArray] PinFormat[] fmt, [NotNull] string desc, uint arraySize)
        {
            formats = fmt;
            symbolicName = desc;
            size = arraySize;
        }

        /// <summary>
        /// Multiple formats.
        /// </summary>
        public PinDescriptor([NotEmptyArray] PinFormat[] fmts, bool optional, [NotNull] string desc)
        {
            formats = fmts;
            isOptional = optional;
            symbolicName = desc;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Is the pin optional, not necessary supplied.
        /// </summary>
        public bool IsOptional
        {
            get { return isOptional; }
        }

        /// <summary>
        /// Component of this pin, if it exists, otherwise it is none.
        /// </summary>
        public PinComponent Component
        {
            get { return component; }
        }

        /// <summary>
        /// The format of pin.
        /// </summary>
        public PinFormat Format
        {
            get { return formats[0]; }
        }

        /// <summary>
        /// Multiple formats.
        /// </summary>
        public PinFormat[] Formats
        {
            get { return formats; }
        }

        /// <summary>
        /// Symbolic name of this component.
        /// </summary>
        public string SymbolicName
        {
            get { return symbolicName; }
        }

        /// <summary>
        /// Are there multiple formats avaiable.
        /// </summary>
        public bool IsMultiFormat
        {
            get { return formats.Length > 1; }
        }

        /// <summary>
        /// The array size.
        /// </summary>
        public uint ArraySize
        {
            get
            {
                return size;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is pin compatible with descriptor.
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        public bool IsCompatible(Pin pin)
        {
            // Check format first.
            int i = 0;
            for (; i < formats.Length; i++)
            {
                if (pin.Format == formats[i]) break;
            }

            // No conversion succeed case.
            if (i == formats.Length) return false;

            return true;
        }

        /// <summary>
        /// Is pin convertible to format.
        /// </summary>
        /// <param name="pin">The pin.</param>
        /// <returns>Does operation exist that converts to this format.</returns>
        public bool IsConvertible(Pin pin)
        {
            return true;
        }

        #endregion
    }

}
