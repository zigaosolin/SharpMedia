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

namespace SharpMedia.Math.Shapes.Storage
{

    /// <summary>
    /// A memory buffer
    /// </summary>
    [Serializable]
    public class MemoryBuffer : IMapable<byte[]>
    {
        #region Private Members
        byte[] data;
        #endregion

        #region Constructors

        /// <summary>
        /// A data based constructor.
        /// </summary>
        /// <param name="data"></param>
        public MemoryBuffer([NotNull] byte[] data)
        {
            this.data = data;
        }

        /// <summary>
        /// A length based constructor.
        /// </summary>
        /// <param name="length"></param>
        public MemoryBuffer(uint length)
        {
            this.data = new byte[length];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Data of buffer.
        /// </summary>
        public byte[] Data
        {
            get { return data; }
        }

        /// <summary>
        /// Length od data.
        /// </summary>
        public uint Length
        {
            get { return (uint)data.Length; }
        }

        #endregion

        #region IMapable<byte[]> Members

        public byte[] Map(MapOptions op)
        {
            // No-op.
            return data;
        }

        public void UnMap()
        {
            // No-op.
        }

        #endregion
    }
}
