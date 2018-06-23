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
using SharpMedia.Math.Shapes.Storage;

namespace SharpMedia.Graphics
{
    /// <summary>
    /// Defines an index format. Only 16- and 32- bit index elements are supported.
    /// Index format is immutable resource.
    /// </summary>
    [Serializable]
    public class IndexFormat : IComparable<IndexFormat>, IComparable
    {
        #region Private Members

        /// <summary>
        /// Is the format wide, e.g. 32-bit.
        /// </summary>
        protected bool isWide;

        #endregion

        #region Properties

        /// <summary>
        /// Is the format wide.
        /// </summary>
        public bool IsWide
        {
            get { return isWide; }
        }

        /// <summary>
        /// Is the format short, not wide.
        /// </summary>
        public bool IsShort
        {
            get { return !isWide; }
        }

        /// <summary>
        /// The size of one element.
        /// </summary>
        public uint ByteSize
        {
            get { return isWide == true ? 4u : 2u; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts to shape format.
        /// </summary>
        /// <returns></returns>
        public ShapeIndexFormat ToShapeFormat()
        {
            return new ShapeIndexFormat(isWide ? typeof(uint) : typeof(ushort));
        }

        #endregion

        #region Constructors

        /// <summary>
        /// The wide flag constructor.
        /// </summary>
        /// <param name="wide">Is it wide.</param>
        public IndexFormat(bool wide)
        {
            isWide = wide;
        }

        #endregion

        #region IComparable<IndexFormat> Members

        public int CompareTo(IndexFormat other)
        {
            return this.isWide.CompareTo(other.isWide);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is IndexFormat)
            {
                return isWide.CompareTo(((IndexFormat)obj).isWide);
            }
            throw new ArgumentException("Cannot depthCompare non-index format object.");
        }

        #endregion
    }
}
