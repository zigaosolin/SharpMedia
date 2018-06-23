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
using SharpMedia.Graphics.Shaders.Operations;
using System.Runtime.Serialization;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Shaders
{




    /// <summary>
    /// A pin is a linking element between inputs and outputs of operations. You must never
    /// create cycles using pins (thus directed acyclic graph). Each pin is strongly typed.
    /// </summary>
    /// <remarks>
    /// Class cannot change it's parent and information, but can change linking nodes.
    /// </remarks>
    [Serializable]
    public class Pin : IEquatable<Pin>, IComparable<Pin>
    {
        #region Constants

        /// <summary>
        /// The pin is dynamic array (size not known yet).
        /// </summary>
        public const uint DynamicArray = uint.MaxValue - 1;

        /// <summary>
        /// The pin is not an array.
        /// </summary>
        public const uint NotArray = uint.MaxValue;

        #endregion

        #region Private Members
        PinFormat format;
        PinFormat textureFormat = PinFormat.Undefined;
        uint size = 0;
        IOperation spawningOp;
        #endregion

        #region Properties

        /// <summary>
        /// The texture format of pin, if pin is texture.
        /// </summary>
        public PinFormat TextureFormat
        {
            get
            {
                return textureFormat;
            }
        }

        /// <summary>
        /// Is the pin an array (static or dynamic).
        /// </summary>
        public bool IsArray
        {
            get
            {
                return size != NotArray;
            }
        }

        /// <summary>
        /// Is the pin a dynamic array.
        /// </summary>
        public bool IsDynamicArray
        {
            get
            {
                return size == DynamicArray;
            }
        }

        /// <summary>
        /// Is the pin static array.
        /// </summary>
        public bool IsStaticArray
        {
            get
            {
                return size != DynamicArray && size != NotArray;
            }
        }

        /// <summary>
        /// The format of pin.
        /// </summary>
        public PinFormat Format
        {
            get { return format; }
        }

        /// <summary>
        /// Size of array, or 0.
        /// </summary>
        public uint Size
        {
            get { return size; }
        }

        /// <summary>
        /// Owner of pin, spawning operation.
        /// </summary>
        public IOperation Owner
        {
            get { return spawningOp; }
        }

        #endregion

        #region Constructors 

        /// <summary>
        /// Normal pin construction.
        /// </summary>
        /// <param name="fmt">The format.</param>
        /// <param name="attributes">The attributes of pin.</param>
        /// <param name="size">Size of array. Use Pin.NotArray (e.g. not an array of pins) 
        /// or Pin.DynamicArray (e.g. dynamic array of pins) for special value.</param>
        /// <remarks></remarks>
        public Pin(PinFormat fmt, uint size, IOperation op)
        {
            this.format = fmt;
            this.size = size;
            this.spawningOp = op;
        }

        /// <summary>
        /// Texture pin construction.
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="textureFmt"></param>
        /// <param name="size"></param>
        /// <param name="op"></param>
        public Pin(PinFormat fmt, PinFormat textureFmt, uint size, IOperation op)
            : this(fmt, size, op)
        {
            this.textureFormat = textureFmt;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clones pin.
        /// </summary>
        /// <returns>The pin cloned.</returns>
        public Pin Clone([NotNull] IOperation op)
        {
            return new Pin(format, this.textureFormat, size, op);
        }


        #endregion

        #region IEquatable<Pin> Members

        public bool Equals([NotNull] Pin other)
        {
            if (this.Format != other.Format) return false;
            if (this.Size != other.Size) return false;
            if (this.TextureFormat != other.textureFormat) return false;
            return true;
        }

        #endregion

        #region IComparable<Pin> Members

        public int CompareTo(Pin other)
        {
            // We first compare based on size.
            if (this.Size != other.Size)
            {
                return Size.CompareTo(other.Size);
            }

            // We compare formats.
            if (this.Format != other.Format)
            {
                return ((uint)Format).CompareTo((uint)other.Format);
            }

            // Equal
            return 0;
        }

        #endregion
    }


}
