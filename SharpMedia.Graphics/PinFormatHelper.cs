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
using SharpMedia.Math;
using SharpMedia.Math.Matrix;
using SharpMedia.AspectOriented;
using SharpMedia.Graphics.Shaders;

namespace SharpMedia.Graphics
{
    /// <summary>
    /// A helper class for vertex formats.
    /// </summary>
    public static class PinFormatHelper
    {

        public static readonly PinFormat[] AllStandardFormats = new PinFormat[] { 
            PinFormat.Float, PinFormat.Floatx2, PinFormat.Floatx3, PinFormat.Floatx4,
            PinFormat.Integer, PinFormat.Integerx2, PinFormat.Integerx3, PinFormat.Integerx4,
            PinFormat.UInteger, PinFormat.UIntegerx2, PinFormat.UIntegerx3, PinFormat.UIntegerx4,
            PinFormat.UNorm, PinFormat.UNormx2, PinFormat.UNormx3, PinFormat.UNormx4,
            PinFormat.SNorm, PinFormat.SNormx2, PinFormat.SNormx3, PinFormat.SNormx4,
            PinFormat.Float2x2, PinFormat.Float3x3, PinFormat.Float4x4,
            PinFormat.Bool, PinFormat.Boolx2, PinFormat.Boolx3, PinFormat.Boolx4
        };



        /// <summary>
        /// Is format a texture.
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static bool IsTexture(PinFormat fmt)
        {
            switch (fmt)
            {
                case PinFormat.Texture1D:
                case PinFormat.BufferTexture:
                case PinFormat.Texture1DArray:
                case PinFormat.Texture2D:
                case PinFormat.Texture2DArray:
                case PinFormat.TextureCube:
                case PinFormat.Texture3D:
                    return true;
                default:
                    return false;
               
            } 
        }

       

        /// <summary>
        /// The size of format in bytes.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static uint SizeOf(PinFormat format)
        {
            switch (format)
            {
                case PinFormat.Integer:
                    return 4;
                case PinFormat.Integerx2:
                    return 4 * 2;
                case PinFormat.Integerx3:
                    return 4 * 3;
                case PinFormat.Integerx4:
                    return 4 * 4;
                case PinFormat.UInteger:
                    return 4;
                case PinFormat.UIntegerx2:
                    return 4 * 2;
                case PinFormat.UIntegerx3:
                    return 4 * 3;
                case PinFormat.UIntegerx4:
                    return 4 * 4;
                case PinFormat.Bool:
                    return 1;
                case PinFormat.Boolx2:
                    return 2;
                case PinFormat.Boolx3:
                    return 3;
                case PinFormat.Boolx4:
                    return 4;
                case PinFormat.SNorm:
                    return 2;
                case PinFormat.SNormx2:
                    return 2 * 2;
                case PinFormat.SNormx3:
                    return 2 * 3;
                case PinFormat.SNormx4:
                    return 2 * 4;
                case PinFormat.UNorm:
                    return 2;
                case PinFormat.UNormx2:
                    return 2 * 2;
                case PinFormat.UNormx3:
                    return 2 * 3;
                case PinFormat.UNormx4:
                    return 2 * 4;
                case PinFormat.Float:
                    return 4;
                case PinFormat.Floatx2:
                    return 4 * 2;
                case PinFormat.Floatx3:
                    return 4 * 3; 
                case PinFormat.Floatx4:
                    return 4 * 4;
                case PinFormat.Float2x2:
                    return 4 * 2 * 2;
                case PinFormat.Float3x3:
                    return 4 * 3 * 3;
                case PinFormat.Float4x4:
                    return 4 * 4 * 4;
                case PinFormat.Integer2x2:
                    return 4 * 2 * 2;
                case PinFormat.Integer3x3:
                    return 4 * 3 * 3;
                case PinFormat.Integer4x4:
                    return 4 * 4 * 4;
                case PinFormat.UInteger2x2:
                    return 4 * 2 * 2;
                case PinFormat.UInteger3x3:
                    return 4 * 3 * 3;
                case PinFormat.UInteger4x4:
                    return 4 * 4 * 4;
                case PinFormat.SNorm2x2:
                    return 2 * 2 * 2;
                case PinFormat.SNorm3x3:
                    return 2 * 3 * 3;
                case PinFormat.SNorm4x4:
                    return 2 * 4 * 4;
                case PinFormat.UNorm2x2:
                    return 2 * 2 * 2;
                case PinFormat.UNorm3x3:
                    return 2 * 3 * 3;
                case PinFormat.UNorm4x4:
                    return 2 * 4 * 4;
                default:
                    throw new InvalidOperationException("Invalid format, size not determined.");
            }
        }

        /// <summary>
        /// Alligns the byte offset for format.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static uint Align(PinFormat format, uint offset)
        {
            uint size = SizeOf(format);

            // For now, align all to 16-byte boundaries.
            return ((offset + 15) / 16) * 16;

        }

        /// <summary>
        /// Converts from pixel format layout.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static PinFormat FromPixelFormat(PixelFormat format)
        {
            return FromPixelFormat(format.CommonFormatLayout);
        }

        /// <summary>
        /// Converts from pixel format layout.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static PinFormat FromPixelFormat(CommonPixelFormatLayout format)
        {
            switch (format)
            {
                case CommonPixelFormatLayout.D24_UNORM_S8_UINT:
                    break;
                case CommonPixelFormatLayout.D32_FLOAT:
                    break;
                case CommonPixelFormatLayout.D32_FLOAT_S24_UINT:
                    break;
                case CommonPixelFormatLayout.X8Y8Z8W8_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16W16_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X32Y32Z32W32_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X8Y8Z8W8_UINT:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16W16_UINT:
                    break;
                case CommonPixelFormatLayout.X32Y32Z32W32_UINT:
                    break;
                case CommonPixelFormatLayout.X8Y8Z8W8_SINT:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16W16_SINT:
                    break;
                case CommonPixelFormatLayout.X32Y32Z32W32_SINT:
                    break;
                case CommonPixelFormatLayout.X8Y8Z8W8_UNORM:
                    return PinFormat.UNormx4;
                case CommonPixelFormatLayout.X16Y16Z16W16_UNORM:
                    break;
                case CommonPixelFormatLayout.X8Y8Z8W8_SNORM:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16W16_SNORM:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16W16_FLOAT:
                    break;
                case CommonPixelFormatLayout.X32Y32Z32W32_FLOAT:
                    break;
                case CommonPixelFormatLayout.X8Y8Z8_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X32Y32Z32_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X8Y8Z8_UINT:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16_UINT:
                    break;
                case CommonPixelFormatLayout.X32Y32Z32_UINT:
                    break;
                case CommonPixelFormatLayout.X8Y8Z8_SINT:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16_SINT:
                    break;
                case CommonPixelFormatLayout.X32Y32Z32_SINT:
                    break;
                case CommonPixelFormatLayout.X8Y8Z8_UNORM:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16_UNORM:
                    break;
                case CommonPixelFormatLayout.X8Y8Z8_SNORM:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16_SNORM:
                    break;
                case CommonPixelFormatLayout.X16Y16Z16_FLOAT:
                    break;
                case CommonPixelFormatLayout.X32Y32Z32_FLOAT:
                    break;
                case CommonPixelFormatLayout.X8Y8_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X16Y16_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X32Y32_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X8Y8_UINT:
                    break;
                case CommonPixelFormatLayout.X16Y16_UINT:
                    break;
                case CommonPixelFormatLayout.X32Y32_UINT:
                    break;
                case CommonPixelFormatLayout.X8Y8_SINT:
                    break;
                case CommonPixelFormatLayout.X16Y16_SINT:
                    break;
                case CommonPixelFormatLayout.X32Y32_SINT:
                    break;
                case CommonPixelFormatLayout.X8Y8_UNORM:
                    break;
                case CommonPixelFormatLayout.X16Y16_UNORM:
                    break;
                case CommonPixelFormatLayout.X8Y8_SNORM:
                    break;
                case CommonPixelFormatLayout.X16Y16_SNORM:
                    break;
                case CommonPixelFormatLayout.X16Y16_FLOAT:
                    break;
                case CommonPixelFormatLayout.X32Y32_FLOAT:
                    break;
                case CommonPixelFormatLayout.X8_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X16_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X32_TYPELESS:
                    break;
                case CommonPixelFormatLayout.X8_UINT:
                    break;
                case CommonPixelFormatLayout.X16_UINT:
                    break;
                case CommonPixelFormatLayout.X32_UINT:
                    break;
                case CommonPixelFormatLayout.X8_SINT:
                    break;
                case CommonPixelFormatLayout.X16_SINT:
                    break;
                case CommonPixelFormatLayout.X32_SINT:
                    break;
                case CommonPixelFormatLayout.X8_UNORM:
                    break;
                case CommonPixelFormatLayout.X16_UNORM:
                    break;
                case CommonPixelFormatLayout.X8_SNORM:
                    break;
                case CommonPixelFormatLayout.X16_SNORM:
                    break;
                case CommonPixelFormatLayout.X16_FLOAT:
                    break;
                case CommonPixelFormatLayout.X32_FLOAT:
                    break;
                case CommonPixelFormatLayout.NotCommonLayout:
                    break;
                default:
                    break;
            }

            return PinFormat.Undefined;
        }

        /// <summary>
        /// Advances the byte offset for format and size.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static uint Advance(PinFormat format, uint arraySize)
        {
            // They are assummed to be 16-byte aligned.
            return SizeOf(format) * ((arraySize == Pin.NotArray) ? (uint)1 : arraySize);
        }

        /// <summary>
        /// Converts vector formats to scalars.
        /// </summary>
        /// <param name="fmt">The format.</param>
        /// <returns></returns>
        public static PinFormat ToScalar(PinFormat fmt)
        {
            PinFormat outFormat;
            if (fmt == PinFormat.Float) return fmt;
            if (fmt == PinFormat.Integer) return fmt;
            if (fmt == PinFormat.SNorm) return fmt;
            if (fmt == PinFormat.UNorm) return fmt;
            if (fmt == PinFormat.UInteger) return fmt;

            if (!IsVector(fmt, out outFormat))
            {
                if (!IsMatrix(fmt, out outFormat))
                {
                    return PinFormat.Undefined;
                }
            }
            return outFormat;
        }

        /// <summary>
        /// Is the format a vector.
        /// </summary>
        /// <param name="fmt">The format.</param>
        /// <param name="scalarType">Scalr type; if Floatx3, it is Float.</param>
        /// <returns>Is it a vector.</returns>
        public static bool IsVector(PinFormat fmt, out PinFormat scalarType)
        {
            scalarType = PinFormat.Undefined;

            switch (fmt)
            {
                case PinFormat.Integerx2:
                case PinFormat.Integerx3:
                case PinFormat.Integerx4:
                    scalarType = PinFormat.Integer;
                    break;
                case PinFormat.Floatx2:
                case PinFormat.Floatx3:
                case PinFormat.Floatx4:
                    scalarType = PinFormat.Float;
                    break;
                default:
                    break;
            }

            return scalarType != PinFormat.Undefined;
        }

        static PinFormat[] OrderedFloats = new PinFormat[]
        {
                PinFormat.Float, PinFormat.Floatx2, PinFormat.Floatx3, PinFormat.Floatx4,
                PinFormat.Float2x2, PinFormat.Float3x3, PinFormat.Float4x4
        };

        static PinFormat[] AfterComponent(PinFormat fmt, PinFormat[] inputs)
        {
            int i;
            for (i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] == fmt) break;
            }

            // We create array.
            PinFormat[] r = new PinFormat[inputs.Length - i];
            for (int j = 0; i < inputs.Length; i++, j++)
            {
                r[j] = inputs[i];
            }

            return r;
        }

        /// <summary>
        /// Returns all formats that constain all components of input format.
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static PinFormat[] ContainsComponents(PinFormat fmt)
        {
            switch (fmt)
            {
                case PinFormat.Undefined:
                    break;
                case PinFormat.Integer:
                    break;
                case PinFormat.Integerx2:
                    break;
                case PinFormat.Integerx3:
                    break;
                case PinFormat.Integerx4:
                    break;
                case PinFormat.UInteger:
                    break;
                case PinFormat.UIntegerx2:
                    break;
                case PinFormat.UIntegerx3:
                    break;
                case PinFormat.UIntegerx4:
                    break;
                case PinFormat.Bool:
                    break;
                case PinFormat.Boolx2:
                    break;
                case PinFormat.Boolx3:
                    break;
                case PinFormat.Boolx4:
                    break;
                case PinFormat.SNorm:
                    break;
                case PinFormat.SNormx2:
                    break;
                case PinFormat.SNormx3:
                    break;
                case PinFormat.SNormx4:
                    break;
                case PinFormat.UNorm:
                    break;
                case PinFormat.UNormx2:
                    break;
                case PinFormat.UNormx3:
                    break;
                case PinFormat.UNormx4:
                    break;
                case PinFormat.Float:
                case PinFormat.Floatx2:
                case PinFormat.Floatx3:
                case PinFormat.Floatx4:
                case PinFormat.Float2x2:
                case PinFormat.Float3x3:
                case PinFormat.Float4x4:
                    // We return all after it.
                    return AfterComponent(fmt, OrderedFloats);
                case PinFormat.Texture1D:
                    break;
                case PinFormat.BufferTexture:
                    break;
                case PinFormat.Texture1DArray:
                    break;
                case PinFormat.Texture2D:
                    break;
                case PinFormat.Texture2DArray:
                    break;
                case PinFormat.TextureCube:
                    break;
                case PinFormat.Texture3D:
                    break;
                case PinFormat.Sampler:
                    break;
                case PinFormat.Interface:
                    break;
                case PinFormat.Integer2x2:
                    break;
                case PinFormat.Integer3x3:
                    break;
                case PinFormat.Integer4x4:
                    break;
                case PinFormat.UInteger2x2:
                    break;
                case PinFormat.UInteger3x3:
                    break;
                case PinFormat.UInteger4x4:
                    break;
                case PinFormat.SNorm2x2:
                    break;
                case PinFormat.SNorm3x3:
                    break;
                case PinFormat.SNorm4x4:
                    break;
                case PinFormat.UNorm2x2:
                    break;
                case PinFormat.UNorm3x3:
                    break;
                case PinFormat.UNorm4x4:
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if it is vector and also gives information about it's length, if it exists.
        /// </summary>
        /// <param name="fmt">The format.</param>
        /// <param name="scalarType">Scalar type, same as IsVector.</param>
        /// <param name="size">The length/size of vector.</param>
        /// <returns>Is it a vector.</returns>
        public static bool IsVector(PinFormat fmt, out PinFormat scalarType, out uint size)
        {
            scalarType = PinFormat.Undefined;
            size = 0;

            switch (fmt)
            {
                case PinFormat.Undefined:
                case PinFormat.Integer:
                    break;
                case PinFormat.Integerx2:
                    scalarType = PinFormat.Integer;
                    size = 2;
                    break;
                case PinFormat.Integerx3:
                    scalarType = PinFormat.Integer;
                    size = 3;
                    break;
                case PinFormat.Integerx4:
                    scalarType = PinFormat.Integer;
                    size = 3;
                    break;
                case PinFormat.Float:
                    break;
                case PinFormat.Floatx2:
                    scalarType = PinFormat.Float;
                    size = 2;
                    break;
                case PinFormat.Floatx3:
                    scalarType = PinFormat.Float;
                    size = 3;
                    break;
                case PinFormat.Floatx4:
                    scalarType = PinFormat.Float;
                    size = 4;
                    break;
                default:
                    break;
            }

            return scalarType != PinFormat.Undefined;
        }

        /// <summary>
        /// Checks if format is a matrix.
        /// </summary>
        /// <param name="fmt">The format of pin.</param>
        /// <param name="scalarType">The scalar type.</param>
        /// <returns>Is it a matrix.</returns>
        public static bool IsMatrix(PinFormat fmt, out PinFormat scalarType)
        {
            uint x, y;
            return IsMatrix(fmt, out scalarType, out x, out y);
        }

        /// <summary>
        /// Checks if format is a matrix.
        /// </summary>
        /// <param name="fmt">The format of pin.</param>
        /// <param name="scalarType">The scalar type.</param>
        /// <returns>Is it a matrix.</returns>
        /// <param name="columns">Number of columns.</param>
        /// <param name="rows">Number of rows.</param>
        public static bool IsMatrix(PinFormat fmt, out PinFormat scalarType,
                                    out uint rows, out uint columns)
        {
            scalarType = PinFormat.Undefined;
            rows = columns = 0;

            switch (fmt)
            {
                case PinFormat.Float2x2:
                    scalarType = PinFormat.Float;
                    rows = columns = 2;
                    break;
                case PinFormat.Float3x3:
                    scalarType = PinFormat.Float;
                    rows = columns = 3;
                    break;
                case PinFormat.Float4x4:
                    scalarType = PinFormat.Float;
                    rows = columns = 4;
                    break;
                default:
                    break;
            }

            return scalarType != PinFormat.Undefined;
        }


        /// <summary>
        /// Checks if value type matches the format.
        /// </summary>
        /// <param name="fmt">The format.</param>
        /// <param name="value">The value type.</param>
        /// <returns>Are they compatible.</returns>
        public static bool IsCompatible(PinFormat fmt, [NotNull] object value)
        {
            switch (fmt)
            {
                case PinFormat.Undefined:
                    return false;
                case PinFormat.Sampler:
                case PinFormat.Texture1D:
                case PinFormat.BufferTexture:
                case PinFormat.Texture1DArray:
                case PinFormat.Texture2D:
                case PinFormat.Texture2DArray:
                case PinFormat.Texture3D:
                case PinFormat.TextureCube:
                    return value == null;
                case PinFormat.Integer:
                    return value is int;
                case PinFormat.Integerx2:
                    return value is Vector2i;
                case PinFormat.Integerx3:
                    return value is Vector3i;
                case PinFormat.Integerx4:
                    return value is Vector4i;
                case PinFormat.Float:
                    return value is float;
                case PinFormat.Floatx2:
                    return value is Vector2f;
                case PinFormat.Floatx3:
                    return value is Vector3f;
                case PinFormat.Floatx4:
                    return value is Vector4f;
                case PinFormat.Float2x2:
                    return value is Matrix2x2f;
                case PinFormat.Float3x3:
                    return value is Matrix3x3f;
                case PinFormat.Float4x4:
                    return value is Matrix4x4f;
                case PinFormat.Integer2x2:
                    throw new NotSupportedException();
                case PinFormat.Integer3x3:
                    throw new NotSupportedException();
                case PinFormat.Integer4x4:
                    throw new NotSupportedException();
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Checks if object value is compatible array of objects.
        /// </summary>
        /// <param name="fmt">The format</param>
        /// <param name="value">Object checked.</param>
        /// <returns></returns>
        public static bool IsCompatibleArray(PinFormat fmt, [NotNull] object value)
        {
            uint dummy;
            return IsCompatibleArray(fmt, value, out dummy);
        }

        /// <summary>
        /// Checks if object value is compatible array of objects.
        /// </summary>
        /// <param name="fmt">The format</param>
        /// <param name="value">Object checked.</param>
        /// <param name="arraySize">The size of array.</param>
        /// <returns></returns>
        public static bool IsCompatibleArray(PinFormat fmt, [NotNull] object value, out uint arraySize)
        {
            arraySize = 0;

            switch (fmt)
            {
                case PinFormat.Integer:
                    if (value is int[])
                    {
                        arraySize = (uint) ((int[])value).Length;
                    }
                    return false;
                case PinFormat.Integerx2:
                    break;
                case PinFormat.Integerx3:
                    break;
                case PinFormat.Integerx4:
                    break;
                case PinFormat.UInteger:
                    break;
                case PinFormat.UIntegerx2:
                    break;
                case PinFormat.UIntegerx3:
                    break;
                case PinFormat.UIntegerx4:
                    break;
                case PinFormat.Bool:
                    break;
                case PinFormat.Boolx2:
                    break;
                case PinFormat.Boolx3:
                    break;
                case PinFormat.Boolx4:
                    break;
                case PinFormat.SNorm:
                    break;
                case PinFormat.SNormx2:
                    break;
                case PinFormat.SNormx3:
                    break;
                case PinFormat.SNormx4:
                    break;
                case PinFormat.UNorm:
                    break;
                case PinFormat.UNormx2:
                    break;
                case PinFormat.UNormx3:
                    break;
                case PinFormat.UNormx4:
                    break;
                case PinFormat.Float:
                    break;
                case PinFormat.Floatx2:
                    break;
                case PinFormat.Floatx3:
                    break;
                case PinFormat.Floatx4:
                    break;
                case PinFormat.Texture1D:
                    break;
                case PinFormat.BufferTexture:
                    break;
                case PinFormat.Texture1DArray:
                    break;
                case PinFormat.Texture2D:
                    break;
                case PinFormat.Texture2DArray:
                    break;
                case PinFormat.TextureCube:
                    break;
                case PinFormat.Texture3D:
                    break;
                case PinFormat.Sampler:
                    break;
                case PinFormat.Interface:
                    break;
                case PinFormat.Float2x2:
                    break;
                case PinFormat.Float3x3:
                    break;
                case PinFormat.Float4x4:
                    break;
                case PinFormat.Integer2x2:
                    break;
                case PinFormat.Integer3x3:
                    break;
                case PinFormat.Integer4x4:
                    break;
                case PinFormat.UInteger2x2:
                    break;
                case PinFormat.UInteger3x3:
                    break;
                case PinFormat.UInteger4x4:
                    break;
                case PinFormat.SNorm2x2:
                    break;
                case PinFormat.SNorm3x3:
                    break;
                case PinFormat.SNorm4x4:
                    break;
                case PinFormat.UNorm2x2:
                    break;
                case PinFormat.UNorm3x3:
                    break;
                case PinFormat.UNorm4x4:
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Enumerates all formats that are expandable to specific format.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static PinFormat[] ExpandableTo(PinFormat format)
        {
            switch (format)
            {
                case PinFormat.Integer:
                    return new PinFormat[] { PinFormat.Integer };
                case PinFormat.Integerx2:
                    break;
                case PinFormat.Integerx3:
                    break;
                case PinFormat.Integerx4:
                    break;
                case PinFormat.UInteger:
                    break;
                case PinFormat.UIntegerx2:
                    break;
                case PinFormat.UIntegerx3:
                    break;
                case PinFormat.UIntegerx4:
                    break;
                case PinFormat.Bool:
                    break;
                case PinFormat.Boolx2:
                    break;
                case PinFormat.Boolx3:
                    break;
                case PinFormat.Boolx4:
                    break;
                case PinFormat.SNorm:
                    break;
                case PinFormat.SNormx2:
                    break;
                case PinFormat.SNormx3:
                    break;
                case PinFormat.SNormx4:
                    break;
                case PinFormat.UNorm:
                    break;
                case PinFormat.UNormx2:
                    break;
                case PinFormat.UNormx3:
                    break;
                case PinFormat.UNormx4:
                    break;
                case PinFormat.Float:
                    break;
                case PinFormat.Floatx2:
                    break;
                case PinFormat.Floatx3:
                    break;
                case PinFormat.Floatx4:
                    return new PinFormat[] { PinFormat.Float, PinFormat.Floatx2, PinFormat.Floatx3, PinFormat.Floatx4 };
                case PinFormat.Texture1D:
                    break;
                case PinFormat.BufferTexture:
                    break;
                case PinFormat.Texture1DArray:
                    break;
                case PinFormat.Texture2D:
                    break;
                case PinFormat.Texture2DArray:
                    break;
                case PinFormat.TextureCube:
                    break;
                case PinFormat.Texture3D:
                    break;
                case PinFormat.Sampler:
                    break;
                case PinFormat.Interface:
                    break;
                case PinFormat.Float2x2:
                    break;
                case PinFormat.Float3x3:
                    break;
                case PinFormat.Float4x4:
                    break;
                case PinFormat.Integer2x2:
                    break;
                case PinFormat.Integer3x3:
                    break;
                case PinFormat.Integer4x4:
                    break;
                case PinFormat.UInteger2x2:
                    break;
                case PinFormat.UInteger3x3:
                    break;
                case PinFormat.UInteger4x4:
                    break;
                case PinFormat.SNorm2x2:
                    break;
                case PinFormat.SNorm3x3:
                    break;
                case PinFormat.SNorm4x4:
                    break;
                case PinFormat.UNorm2x2:
                    break;
                case PinFormat.UNorm3x3:
                    break;
                case PinFormat.UNorm4x4:
                    break;
                default:
                    break;
            }

            throw new NotImplementedException();
        }

        

    }
}
