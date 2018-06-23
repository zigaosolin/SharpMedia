// This file was generated by TemplateEngine from template source 'ShapeIndexBufferView'
// using template 'ShapeIndexBufferView. Do not modify this file directly, modify it from template source.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SharpMedia.Math.Shapes.Storage
{
    /// <summary>
    /// A shape index buffer format.
    /// </summary>
    /// <remarks>Allowed index types are: byte, ushort, uint.</remarks>
    [Serializable]
    public class ShapeIndexFormat
    {
        #region Private Members
        Type indexType;
        #endregion

        #region Constructors

        /// <summary>
        /// Constuctor.
        /// </summary>
        /// <param name="type"></param>
        public ShapeIndexFormat(Type type)
        {
            if (type != typeof(uint) && type != typeof(ushort) && type != typeof(byte))
                throw new ArgumentException("Unsupported index type, only uint, ushort and byte supported.");
            this.indexType = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The byte size.
        /// </summary>
        public uint ByteSize
        {
            get
            {
                if (IsUInt32) return 4;
                if (IsUInt16) return 2;
                return 1;
            }
        }

        /// <summary>
        /// Is the type unsigned integer (4 bytes).
        /// </summary>
        public bool IsUInt32
        {
            get { return indexType == typeof(uint); }
        }

        /// <summary>
        /// Is the type unsigned integer (2 bytes).
        /// </summary>
        public bool IsUInt16
        {
            get { return indexType == typeof(ushort); }
        }

        /// <summary>
        /// Is the type unsigned integer (1 byte).
        /// </summary>
        public bool IsByte
        {
            get { return indexType == typeof(byte); }
        }

        /// <summary>
        /// The actual type accessor.
        /// </summary>
        public Type Type
        {
            get { return indexType; }
        }

        #endregion

    }

    /// <summary>
    /// A shape index buffer view, similiar to control point buffer view.
    /// </summary>
    [Serializable]
    public class ShapeIndexBufferView : IMapable<byte[]>, IDisposable
    {
        #region Private Members
        object syncRoot = new object();
        ShapeIndexFormat format;
        IMapable<byte[]> mappable;
        ulong offset = 0;
        byte[] mappedData;
        MapOptions mapOptions;
        #endregion

        #region Private Members

        ~ShapeIndexBufferView()
        {
            if (mappable != null)
            {
                if (mappedData != null)
                    UnMap();
                mappable = null;
            }
        }

        void AssertMapped()
        {
            if (!IsMapped)
            {
                throw new InvalidOperationException("The buffer must be mapped for this operation.");
            }
        }

        void AssertUnMapped()
        {
            if (IsMapped)
            {
                throw new InvalidOperationException("The buffer must be un-mapped for this operation.");
            }
        }

        void AssertWritable()
        {
            if (!IsMapped || mapOptions == MapOptions.Read)
            {
                throw new InvalidOperationException("The index buffer view is not mapped or is not mapped in write mode.");
            }
        }

        void AssertReadable()
        {
            if (!IsMapped || mapOptions == MapOptions.Write)
            {
                throw new InvalidOperationException("The index buffer view is not mapped or is not mapped in read mode.");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="mappable"></param>
        public ShapeIndexBufferView(ShapeIndexFormat format, IMapable<byte[]> mappable, ulong offset)
        {
            this.format = format;
            this.mappable = mappable;
            this.offset = 0;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="mappable"></param>
        public ShapeIndexBufferView(ShapeIndexFormat format, IMapable<byte[]> mappable)
            : this(format, mappable, 0)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// A mappable buffer.
        /// </summary>
        public IMapable<byte[]> Mappable
        {
            get { return mappable; }
        }

        /// <summary>
        /// Is it already mapped by view.
        /// </summary>
        public bool IsMapped
        {
            get { return mappedData != null; }
        }

        /// <summary>
        /// Mapped data.
        /// </summary>
        public byte[] MappedData
        {
            get { return mappedData; }
        }

        /// <summary>
        /// The associated format.
        /// </summary>
        public ShapeIndexFormat Format
        {
            get { return format; }
        }

        /// <summary>
        /// The offset of buffer.
        /// </summary>
        public ulong Offset
        {
            get { return offset; }
        }

        /// <summary>
        /// The mapping options.
        /// </summary>
        public MapOptions MapOptions
        {
            get { return mapOptions; }
        }

        #endregion

        #region Read Queries

        
		//#foreach instanced to 'Byte'


        /// <summary>
        /// Obtains a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe  byte Getb(uint index)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {
                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        return (byte)(*pp);

                    }
                    else if (format.IsUInt16)
                    {
                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        return (byte)(*pp);
                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        return (byte)(*pp);
                    }
                }

                
            }
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] Getb(uint index, uint count)
        {
            byte[] data = new byte[count];
            Get(index, data);
            return data;
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void Get(uint index, byte[] data)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {
                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (byte)pp[i];
                        }
                    }
                    else if (format.IsUInt16)
                    {
                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (byte)pp[i];
                        }
                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (byte)pp[i];
                        }
                    }
                }

                
            }
        }

        //#endfor instanced to 'Byte'

		//#foreach instanced to 'ByteSafe'


        /// <summary>
        /// Obtains a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe  byte GetSafeb(uint index)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(byte))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(byte)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                byte* pp = (byte*)(p + offset + format.ByteSize * index);
                return *pp;

                //#endif
            }
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] GetSafeb(uint index, uint count)
        {
            byte[] data = new byte[count];
            Get(index, data);
            return data;
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void GetSafe(uint index, byte[] data)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(byte))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(byte)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                byte* pp = (byte*)(p + offset + format.ByteSize * index);
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = pp[i];
                }

                //#endif
            }
        }

        //#endfor instanced to 'ByteSafe'

		//#foreach instanced to 'UInt'


        /// <summary>
        /// Obtains a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe  uint Getui(uint index)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {
                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        return (uint)(*pp);

                    }
                    else if (format.IsUInt16)
                    {
                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        return (uint)(*pp);
                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        return (uint)(*pp);
                    }
                }

                
            }
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public uint[] Getui(uint index, uint count)
        {
            uint[] data = new uint[count];
            Get(index, data);
            return data;
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void Get(uint index, uint[] data)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {
                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (uint)pp[i];
                        }
                    }
                    else if (format.IsUInt16)
                    {
                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (uint)pp[i];
                        }
                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (uint)pp[i];
                        }
                    }
                }

                
            }
        }

        //#endfor instanced to 'UInt'

		//#foreach instanced to 'UIntSafe'


        /// <summary>
        /// Obtains a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe  uint GetSafeui(uint index)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(uint))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(uint)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                uint* pp = (uint*)(p + offset + format.ByteSize * index);
                return *pp;

                //#endif
            }
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public uint[] GetSafeui(uint index, uint count)
        {
            uint[] data = new uint[count];
            Get(index, data);
            return data;
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void GetSafe(uint index, uint[] data)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(uint))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(uint)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                uint* pp = (uint*)(p + offset + format.ByteSize * index);
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = pp[i];
                }

                //#endif
            }
        }

        //#endfor instanced to 'UIntSafe'

		//#foreach instanced to 'UShort'


        /// <summary>
        /// Obtains a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe  ushort Getus(uint index)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {
                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        return (ushort)(*pp);

                    }
                    else if (format.IsUInt16)
                    {
                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        return (ushort)(*pp);
                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        return (ushort)(*pp);
                    }
                }

                
            }
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ushort[] Getus(uint index, uint count)
        {
            ushort[] data = new ushort[count];
            Get(index, data);
            return data;
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void Get(uint index, ushort[] data)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {
                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (ushort)pp[i];
                        }
                    }
                    else if (format.IsUInt16)
                    {
                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (ushort)pp[i];
                        }
                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (ushort)pp[i];
                        }
                    }
                }

                
            }
        }

        //#endfor instanced to 'UShort'

		//#foreach instanced to 'UShortSafe'


        /// <summary>
        /// Obtains a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe  ushort GetSafeus(uint index)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(ushort))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(ushort)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                return *pp;

                //#endif
            }
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ushort[] GetSafeus(uint index, uint count)
        {
            ushort[] data = new ushort[count];
            Get(index, data);
            return data;
        }

        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void GetSafe(uint index, ushort[] data)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(ushort))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(ushort)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = pp[i];
                }

                //#endif
            }
        }

        //#endfor instanced to 'UShortSafe'


        #endregion

        #region Write Queries

        
		//#foreach instanced to 'Byte'


        /// <summary>
        /// Sets a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe void Set(uint index, byte data)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {

                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        (*pp) = (byte)data;
                    }
                    else if (format.IsUInt16)
                    {
                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        (*pp) = (ushort)data;
                        
                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        (*pp) = (uint)data;
                    }
                }

                
            }
        }


        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void Set(uint index, byte[] data)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {
                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        for (int i = 0; i < data.Length; i++)
                        {
                            pp[i] = (byte)data[i];
                        }

                    }
                    else if (format.IsUInt16)
                    {

                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            pp[i] = (ushort)data[i];
                        }


                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            pp[i] = (uint)data[i];
                        }
                    }
                }

                
            }
        }

        //#endfor instanced to 'Byte'

		//#foreach instanced to 'ByteSafe'


        /// <summary>
        /// Sets a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe void SetSafe(uint index, byte data)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(byte))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(byte)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                byte* pp = (byte*)(p + offset + format.ByteSize * index);
                *pp = data;

                //#endif
            }
        }


        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void SetSafe(uint index, byte[] data)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(byte))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(byte)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                byte* pp = (byte*)(p + offset + format.ByteSize * index);
                for (int i = 0; i < data.Length; i++)
                {
                    pp[i] = data[i];
                }

                //#endif
            }
        }

        //#endfor instanced to 'ByteSafe'

		//#foreach instanced to 'UInt'


        /// <summary>
        /// Sets a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe void Set(uint index, uint data)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {

                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        (*pp) = (byte)data;
                    }
                    else if (format.IsUInt16)
                    {
                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        (*pp) = (ushort)data;
                        
                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        (*pp) = (uint)data;
                    }
                }

                
            }
        }


        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void Set(uint index, uint[] data)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {
                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        for (int i = 0; i < data.Length; i++)
                        {
                            pp[i] = (byte)data[i];
                        }

                    }
                    else if (format.IsUInt16)
                    {

                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            pp[i] = (ushort)data[i];
                        }


                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            pp[i] = (uint)data[i];
                        }
                    }
                }

                
            }
        }

        //#endfor instanced to 'UInt'

		//#foreach instanced to 'UIntSafe'


        /// <summary>
        /// Sets a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe void SetSafe(uint index, uint data)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(uint))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(uint)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                uint* pp = (uint*)(p + offset + format.ByteSize * index);
                *pp = data;

                //#endif
            }
        }


        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void SetSafe(uint index, uint[] data)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(uint))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(uint)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                uint* pp = (uint*)(p + offset + format.ByteSize * index);
                for (int i = 0; i < data.Length; i++)
                {
                    pp[i] = data[i];
                }

                //#endif
            }
        }

        //#endfor instanced to 'UIntSafe'

		//#foreach instanced to 'UShort'


        /// <summary>
        /// Sets a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe void Set(uint index, ushort data)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {

                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        (*pp) = (byte)data;
                    }
                    else if (format.IsUInt16)
                    {
                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        (*pp) = (ushort)data;
                        
                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        (*pp) = (uint)data;
                    }
                }

                
            }
        }


        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void Set(uint index, ushort[] data)
        {
            AssertReadable();

            

            fixed (byte* p = mappedData)
            {
                

                checked
                {
                    if (format.IsByte)
                    {
                        byte* pp = p + offset + format.ByteSize * index;
                        for (int i = 0; i < data.Length; i++)
                        {
                            pp[i] = (byte)data[i];
                        }

                    }
                    else if (format.IsUInt16)
                    {

                        ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            pp[i] = (ushort)data[i];
                        }


                    }
                    else
                    {
                        uint* pp = (uint*)(p + offset + format.ByteSize * index);
                        for (int i = 0; i < data.Length; i++)
                        {
                            pp[i] = (uint)data[i];
                        }
                    }
                }

                
            }
        }

        //#endfor instanced to 'UShort'

		//#foreach instanced to 'UShortSafe'


        /// <summary>
        /// Sets a data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe void SetSafe(uint index, ushort data)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(ushort))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(ushort)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                *pp = data;

                //#endif
            }
        }


        /// <summary>
        /// Obtains an array of data at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public unsafe void SetSafe(uint index, ushort[] data)
        {
            AssertReadable();

            //#ifdef TypeSafe


            if (format.Type != typeof(ushort))
            {
                throw new InvalidOperationException(
                    string.Format("The format defines '{0}', the given type was '{1}'",
                    format.Type, typeof(ushort)));
            }

            //#endif

            fixed (byte* p = mappedData)
            {
                //#ifdef TypeSafe


                ushort* pp = (ushort*)(p + offset + format.ByteSize * index);
                for (int i = 0; i < data.Length; i++)
                {
                    pp[i] = data[i];
                }

                //#endif
            }
        }

        //#endfor instanced to 'UShortSafe'


        #endregion

        #region IMapable<byte[]> Members

        public byte[] Map(MapOptions op)
        {
            AssertUnMapped();

            Monitor.Enter(syncRoot);

            try
            {
                mappedData = mappable.Map(op);
                mapOptions = op;
            }
            catch (Exception ex)
            {
                Monitor.Exit(syncRoot);
                throw ex;
            }

            return mappedData;
        }

        public void UnMap()
        {
            AssertMapped();

            try
            {
                mappable.UnMap();
            }
            finally
            {
                mappedData = null;
                Monitor.Exit(syncRoot);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (syncRoot)
            {
                if (mappable != null)
                {
                    if (mappedData != null)
                        UnMap();
                    mappable = null;
                }
            }
        }

        #endregion
    }
}