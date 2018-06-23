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

namespace SharpMedia.Graphics.Shaders
{
    

    /// <summary>
    /// A constant buffer layout describes how parameters fit into constant buffer.
    /// </summary>
    /// <remarks>Can be only contructed using ConstantLayoutBufferBuilder. Elements are indexed
    /// as byte4[] array. No vector can span through 4-byte4 boundary. Same location may be shared by
    /// the same constant.</remarks>
    public class ConstantBufferLayout : IEquatable<ConstantBufferLayout>, IComparable<ConstantBufferLayout>
    {
        #region Static Members

        /// <summary>
        /// Maximum number of constant buffer bindings.
        /// </summary>
        public const uint MaxConstantBufferBindingSlots = 16;

        #endregion

        #region Private Members

        internal struct ParamOffset
        {
            public ParameterDescription Description;
            public uint Offset;
        }

        // Constant buffer maps ParameterDescription to index.
        SortedList<string, ParamOffset> parameterLocations = new SortedList<string, ParamOffset>();

        #endregion

        #region Internal Methods

        internal ConstantBufferLayout(SortedList<string, ParamOffset> parLoc)
        {
            parameterLocations = parLoc;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns number of parameters.
        /// </summary>
        public uint ParameterCount
        {
            get
            {
                return (uint)parameterLocations.Count;
            }
        }

        /// <summary>
        /// Minimum size of buffer in byte4.
        /// </summary>
        public uint MinimumBufferSize
        {
            get
            {
                // No minimum size of empty layout.
                if (parameterLocations.Count == 0) return 0;

                uint maxOffset = parameterLocations.Values[parameterLocations.Count - 1].Offset;
                uint sizeOfLast = (parameterLocations.Values[parameterLocations.Count - 1].Description.SizeInBytes + 3) / 4;

                // We round to upper 4-vector.
                return ((maxOffset + sizeOfLast + 3) >> 2) << 2;

            }
        }

        /// <summary>
        /// Minimum size of buffer in bytes (so all parameters fit in it).
        /// </summary>
        public uint MinimumBufferSizeInBytes
        {
            get
            {
                return MinimumBufferSize * sizeof(float);
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Checks if parameter is defined by this layout.
        /// </summary>
        public bool IsDefined([NotNull] string desc)
        {
            return parameterLocations.Keys.Contains(desc);
        }

        /// <summary>
        /// Obtains the offset of parameter.
        /// </summary>
        /// <param name="desc">The parameter we are looking for.</param>
        /// <returns></returns>
        public uint GetOffset([NotNull] string desc)
        {
            return parameterLocations[desc].Offset;
        }

        /// <summary>
        /// Tries to obstain data.
        /// </summary>
        /// <param name="desc">The data name.</param>
        /// <param name="offset">Offset of data.</param>
        /// <param name="format">Data format.</param>
        /// <returns></returns>
        public bool TryGetData([NotNull] string desc, out uint offset, out PinFormat format, out uint arraySize)
        {
            ParamOffset val;
            if (parameterLocations.TryGetValue(desc, out val))
            {
                offset = val.Offset;
                format = val.Description.Pin.Format;
                arraySize = val.Description.Pin.Size;
                return true;
            }
            offset = 0;
            format = PinFormat.Undefined;
            arraySize = 0;
            return false;
        }

        /// <summary>
        /// Tries to obtain offset of parameter. Fails if parameter not defined in this layout.
        /// </summary>
        public bool TryGetOffset([NotNull] string desc, out uint offset)
        {
            ParamOffset val;
            if (parameterLocations.TryGetValue(desc, out val))
            {
                offset = val.Offset;
                return true;
            }
            offset = 0;
            return false;
        }


        #endregion

        #region IEquatable<ConstantBufferLayout> Members

        public bool Equals([NotNull] ConstantBufferLayout other)
        {
            return this.CompareTo(other) == 0;
        }

        #endregion

        #region IComparable<ConstantBufferLayout> Members

        public int CompareTo(ConstantBufferLayout other)
        {
            int cmp = this.parameterLocations.Count.CompareTo(other.parameterLocations.Count);
            if (cmp != 0) return cmp;

            // We now go for each parameter.
            for (int i = 0; i < parameterLocations.Count; i++)
            {
                // We check offset.
                cmp = parameterLocations.Values[i].Offset.CompareTo(other.parameterLocations.Values[i].Offset);
                if (cmp != 0) return cmp;

                // We check name (if name match, the values values also match).
                cmp = parameterLocations.Values[i].Description.Name.CompareTo(
                    other.parameterLocations.Values[i].Description.Name);
                if (cmp != 0) return cmp;
            }

            return 0;
        }

        #endregion
    }
}
