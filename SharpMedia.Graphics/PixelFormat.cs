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
using System.Runtime.Serialization;
using SharpMedia.Testing;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics
{

 

    /// <summary>
    /// Invalid pixel format exception, thrown when wrong combinations are used for format.
    /// </summary>
    public class InvalidPixelFormatException : Exception
    {
        public InvalidPixelFormatException(string message)
            : base(message)
        {
        }

        public InvalidPixelFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public InvalidPixelFormatException()
            : base("Invalid pixel format")
        {
        }

        public InvalidPixelFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// Defines pixel format that can be used by any image. Pixel format is
    /// immutable and can be used to access certain elements of the image.
    /// </summary>
    /// <remarks>
    /// By default, images should support any kind of mixed format. All formats
    /// must be aligned to one byte at least (and this is garantied because components are
    /// one byte at least).
    /// Hardware, on the other hand, may not support very complex formats.
    /// 
    /// Also note that specific compressions require specific pixel formats and component
    /// orderings (usually common formats).
    /// 
    /// A special layouts are introduced in order to convert format to hardware representation.
    /// </remarks>
    [Serializable]
    public class PixelFormat : IEquatable<PixelFormat>, IEnumerable<PixelFormat.Element>
    {
        #region Pixel Element Implementations

        /// <summary>
        /// A single pixel element.
        /// </summary>
        [Serializable]
        public class Element : IEquatable<Element>
        {
            private uint offset;
            private PixelComponentFormat format;
            private PixelComponent component;

            /// <summary>
            /// Offset of element, calculated.
            /// </summary>
            public uint Offset
            {
                get { return offset; }
            }

            /// <summary>
            /// Component in format.
            /// </summary>
            public PixelComponent Component
            {
                get { return component; }
            }

            /// <summary>
            /// The format of component.
            /// </summary>
            public PixelComponentFormat Format
            {
                get { return format; }
            }

            /// <summary>
            /// ByteSize of format.
            /// </summary>
            public static uint FormatSize(PixelComponentFormat fmt)
            {
                switch (fmt)
                {
                    case PixelComponentFormat.Typeless8:
                    case PixelComponentFormat.UInt8:
                    case PixelComponentFormat.SInt8:
                    case PixelComponentFormat.SNorm8:
                    case PixelComponentFormat.UNorm8:
                        return 1;
                    case PixelComponentFormat.Typeless16:
                    case PixelComponentFormat.SNorm16:
                    case PixelComponentFormat.UNorm16:
                    case PixelComponentFormat.UInt16:
                    case PixelComponentFormat.SInt16:
                    case PixelComponentFormat.Float16:
                        return 2;
                    case PixelComponentFormat.Typeless24:
                    case PixelComponentFormat.Float24:
                    case PixelComponentFormat.SInt24:
                    case PixelComponentFormat.UInt24:
                        return 3;
                    case PixelComponentFormat.Float32:
                    case PixelComponentFormat.Typeless32:
                    case PixelComponentFormat.UInt32:
                    case PixelComponentFormat.SInt32:
                        return 4;
                    case PixelComponentFormat.Typeless64:
                    case PixelComponentFormat.UInt64:
                    case PixelComponentFormat.SInt64:
                    case PixelComponentFormat.Float64:
                        return 8;
                    default:
                        throw new NotImplementedException("The pixel format is not supported.");
                }
            }


            /// <summary>
            /// Resolve format; e.g. validates it and sets offset.
            /// </summary>
            /// <param name="offset"></param>
            /// <returns></returns>
            internal uint Resolve(uint off)
            {
                offset = off;

                // Now return new offset.
                return off + FormatSize(Format);
            }

            /// <summary>
            /// Constructor for pixel format.
            /// </summary>
            /// <param name="c">The component.</param>
            /// <param name="f">Component's format.</param>
            internal Element(PixelComponent c, PixelComponentFormat f)
            {
                component = c;
                format = f;
                offset = 0;
            }

            /// <summary>
            /// Obtains short name of component.
            /// </summary>
            /// <param name="component">The component.</param>
            /// <returns>String name.</returns>
            public static string ToString(PixelComponent component)
            {
                string componentName = null;
                switch (component)
                {
                    case PixelComponent.NotKnown:
                        componentName = "N";
                        break;
                    case PixelComponent.Red:
                        componentName = "R";
                        break;
                    case PixelComponent.Green:
                        componentName = "G";
                        break;
                    case PixelComponent.Blue:
                        componentName = "B";
                        break;
                    case PixelComponent.Alpha:
                        componentName = "A";
                        break;
                    case PixelComponent.Luminance:
                        componentName = "L";
                        break;
                    case PixelComponent.Bump:
                        componentName = "Bump";
                        break;
                    case PixelComponent.NormalX:
                        componentName = "Nx";
                        break;
                    case PixelComponent.NormalY:
                        componentName = "Ny";
                        break;
                    case PixelComponent.NormalZ:
                        componentName = "Nz";
                        break;
                    case PixelComponent.Reflection:
                        componentName = "Refl";
                        break;
                    case PixelComponent.Refraction:
                        componentName = "Refr";
                        break;
                    case PixelComponent.Depth:
                        componentName = "D";
                        break;
                    case PixelComponent.Stencil:
                        componentName = "S";
                        break;
                    case PixelComponent.User0:
                        componentName = "U0";
                        break;
                    case PixelComponent.User1:
                        componentName = "U1";
                        break;
                    case PixelComponent.User2:
                        componentName = "U2";
                        break;
                    case PixelComponent.User3:
                        componentName = "U3";
                        break;
                    case PixelComponent.User4:
                        componentName = "U4";
                        break;
                    case PixelComponent.User5:
                        componentName = "U5";
                        break;
                    case PixelComponent.User6:
                        componentName = "U6";
                        break;
                    case PixelComponent.User7:
                        componentName = "U7";
                        break;
                    case PixelComponent.User8:
                        componentName = "U8";
                        break;
                    case PixelComponent.User9:
                        componentName = "U9";
                        break;
                    default:
                        throw new Exception("Unknown component type.");
                }
                return componentName;
            }

            /// <summary>
            /// Check if elements are compatible.
            /// </summary>
            /// <param name="element">The other element.</param>
            /// <returns></returns>
            public bool IsCompatible(Element element)
            {
                if (format == element.format) return true;

                switch (format)
                {
                    case PixelComponentFormat.Typeless8:
                        switch (element.format)
                        {
                            case PixelComponentFormat.UInt8:
                            case PixelComponentFormat.SInt8:
                            case PixelComponentFormat.UNorm8:
                            case PixelComponentFormat.SNorm8:
                                return true;
                        }
                        break;
                    case PixelComponentFormat.Typeless16:
                        switch (element.format)
                        {
                            case PixelComponentFormat.UInt16:
                            case PixelComponentFormat.SInt16:
                            case PixelComponentFormat.UNorm16:
                            case PixelComponentFormat.SNorm16:
                            case PixelComponentFormat.Float16:
                                return true;
                        }
                        break;
                    case PixelComponentFormat.Typeless24:
                        switch (element.format)
                        {
                            case PixelComponentFormat.Float24:
                            case PixelComponentFormat.UInt24:
                                return true;
                        }
                        break;
                    case PixelComponentFormat.Typeless32:
                        switch (element.format)
                        {
                            case PixelComponentFormat.UInt32:
                            case PixelComponentFormat.SInt32:
                            case PixelComponentFormat.Float32:
                                return true;
                        }
                        break;
                    case PixelComponentFormat.Typeless64:
                        switch (element.format)
                        {
                            case PixelComponentFormat.Float64:
                            case PixelComponentFormat.SInt64:
                            case PixelComponentFormat.UInt64:
                                return true;
                        }
                        break;
                }

                return false;
            }

            /// <summary>
            /// Obtains short name of format.
            /// </summary>
            /// <param name="format">The format.</param>
            /// <returns>String name.</returns>
            public static string ToString(PixelComponentFormat format)
            {
                string formatName = null;
                switch (format)
                {
                    case PixelComponentFormat.Typeless8:
                        formatName = "T8";
                        break;
                    case PixelComponentFormat.Typeless16:
                        formatName = "T16";
                        break;
                    case PixelComponentFormat.Typeless32:
                        formatName = "T32";
                        break;
                    case PixelComponentFormat.Typeless64:
                        formatName = "T64";
                        break;
                    case PixelComponentFormat.UInt8:
                        formatName = "UI8";
                        break;
                    case PixelComponentFormat.SInt8:
                        formatName = "SI8";
                        break;
                    case PixelComponentFormat.UInt16:
                        formatName = "UI16";
                        break;
                    case PixelComponentFormat.SInt16:
                        formatName = "SI16";
                        break;
                    case PixelComponentFormat.UInt24:
                        formatName = "UI24";
                        break;
                    case PixelComponentFormat.SInt24:
                        formatName = "SI24";
                        break;
                    case PixelComponentFormat.Typeless24:
                        formatName = "T24";
                        break;
                    case PixelComponentFormat.UInt32:
                        formatName = "UI32";
                        break;
                    case PixelComponentFormat.SInt32:
                        formatName = "SI32";
                        break;
                    case PixelComponentFormat.UInt64:
                        formatName = "UI64";
                        break;
                    case PixelComponentFormat.SInt64:
                        formatName = "SI64";
                        break;
                    case PixelComponentFormat.Float16:
                        formatName = "F16";
                        break;
                    case PixelComponentFormat.Float24:
                        formatName = "F24";
                        break;
                    case PixelComponentFormat.Float32:
                        formatName = "F32";
                        break;
                    case PixelComponentFormat.Float64:
                        formatName = "F64";
                        break;
                    case PixelComponentFormat.UNorm8:
                        formatName = "UN8";
                        break;
                    case PixelComponentFormat.SNorm8:
                        formatName = "SN8";
                        break;
                    case PixelComponentFormat.UNorm16:
                        formatName = "UN16";
                        break;
                    case PixelComponentFormat.SNorm16:
                        formatName = "SN16";
                        break;
                    default:
                        break;
                }
                return formatName;
            }

            public override string ToString()
            {
                return ToString(component) + "." + ToString(format);
            }

            /// <summary>
            /// Description from string.
            /// </summary>
            /// <param name="desc"></param>
            internal Element(string desc)
            {
                offset = 0;
                string[] split = desc.Split('.');
                if (split.Length != 2)
                {
                    throw new InvalidPixelFormatException("The element of pixel format not split by '.' or there " +
                                                          "were too many splits.");
                }

                // First split is component
                switch (split[0])
                {
                    case "N":
                        component = PixelComponent.NotKnown;
                        break;
                    case "R":
                        component = PixelComponent.Red;
                        break;
                    case "G":
                        component = PixelComponent.Green;
                        break;
                    case "B":
                        component = PixelComponent.Blue;
                        break;
                    case "A":
                        component = PixelComponent.Alpha;
                        break;
                    case "Refl":
                        component = PixelComponent.Reflection;
                        break;
                    case "Refr":
                        component = PixelComponent.Refraction;
                        break;
                    case "D":
                        component = PixelComponent.Depth;
                        break;
                    case "S":
                        component = PixelComponent.Stencil;
                        break;
                    case "L":
                        component = PixelComponent.Luminance;
                        break;
                    case "Bump":
                        component = PixelComponent.Bump;
                        break;
                    case "Nx":
                        component = PixelComponent.NormalX;
                        break;
                    case "Ny":
                        component = PixelComponent.NormalY;
                        break;
                    case "Nz":
                        component = PixelComponent.NormalZ;
                        break;
                    case "U0":
                        component = PixelComponent.User0;
                        break;
                    case "U1":
                        component = PixelComponent.User1;
                        break;
                    case "U2":
                        component = PixelComponent.User2;
                        break;
                    case "U3":
                        component = PixelComponent.User3;
                        break;
                    case "U4":
                        component = PixelComponent.User4;
                        break;
                    case "U5":
                        component = PixelComponent.User5;
                        break;
                    case "U6":
                        component = PixelComponent.User6;
                        break;
                    case "U7":
                        component = PixelComponent.User7;
                        break;
                    case "U8":
                        component = PixelComponent.User8;
                        break;
                    case "U9":
                        component = PixelComponent.User9;
                        break;
                    default:
                        throw new InvalidPixelFormatException("Unsupported component " + split[0]);
                }


                // Second split is format.
                switch (split[1])
                {
                    case "SI8":
                        format = PixelComponentFormat.SInt8;
                        break;
                    case "UI8":
                        format = PixelComponentFormat.UInt8;
                        break;
                    case "SI16":
                        format = PixelComponentFormat.SInt16;
                        break;
                    case "UI16":
                        format = PixelComponentFormat.UInt16;
                        break;
                    case "UI24":
                        format = PixelComponentFormat.UInt24;
                        break;
                    case "SI24":
                        format = PixelComponentFormat.SInt24;
                        break;
                    case "SI32":
                        format = PixelComponentFormat.SInt32;
                        break;
                    case "UI32":
                        format = PixelComponentFormat.UInt32;
                        break;
                    case "SI64":
                        format = PixelComponentFormat.SInt64;
                        break;
                    case "UI64":
                        format = PixelComponentFormat.UInt64;
                        break;
                    case "F16":
                        format = PixelComponentFormat.Float16;
                        break;
                    case "T24":
                        format = PixelComponentFormat.Typeless24;
                        break;
                    case "F24":
                        format = PixelComponentFormat.Float24;
                        break;
                    case "F32":
                        format = PixelComponentFormat.Float32;
                        break;
                    case "F64":
                        format = PixelComponentFormat.Float64;
                        break;
                    case "SN8":
                        format = PixelComponentFormat.SNorm8;
                        break;
                    case "SN16":
                        format = PixelComponentFormat.SNorm16;
                        break;
                    case "UN8":
                        format = PixelComponentFormat.UNorm8;
                        break;
                    case "UN16":
                        format = PixelComponentFormat.UNorm16;
                        break;
                    case "T8":
                        format = PixelComponentFormat.Typeless8;
                        break;
                    case "T16":
                        format = PixelComponentFormat.Typeless16;
                        break;
                    case "T32":
                        format = PixelComponentFormat.Typeless32;
                        break;
                    case "T64":
                        format = PixelComponentFormat.Typeless64;
                        break;
                    default:
                        throw new InvalidPixelFormatException("Unsupported format " + split[1]);
                }
            }

            #region IEquatable<Element> Members

            public bool Equals(Element other)
            {
                if (Format == other.Format && Component == other.Component && Offset == other.Offset) return true;
                return false;
            }

            #endregion
        }

        #endregion

        #region Private Members
        private Element[] elements;
        private uint size;
        private bool isTypeless;
        private CommonPixelFormatLayout commonFormat;
        private CompressionFormat compression;
        #endregion

        #region Private Methods

        private void CheckForCommonFormatLayout()
        {
            switch (elements.Length)
            {
                case 1:
                    switch (elements[0].Format)
                    {
                        case PixelComponentFormat.Typeless8:
                            commonFormat = CommonPixelFormatLayout.X8_TYPELESS;
                            return;
                        case PixelComponentFormat.Typeless16:
                            commonFormat = CommonPixelFormatLayout.X16_TYPELESS;
                            return;
                        case PixelComponentFormat.Typeless32:
                            commonFormat = CommonPixelFormatLayout.X32_TYPELESS;
                            return;
                        case PixelComponentFormat.UInt8:
                            commonFormat = CommonPixelFormatLayout.X8_UINT;
                            return;
                        case PixelComponentFormat.SInt8:
                            commonFormat = CommonPixelFormatLayout.X8_SINT;
                            return;
                        case PixelComponentFormat.UInt16:
                            commonFormat = CommonPixelFormatLayout.X16_UINT;
                            return;
                        case PixelComponentFormat.SInt16:
                            commonFormat = CommonPixelFormatLayout.X16_SINT;
                            return;
                        case PixelComponentFormat.UInt32:
                            commonFormat = CommonPixelFormatLayout.X32_UINT;
                            return;
                        case PixelComponentFormat.SInt32:
                            commonFormat = CommonPixelFormatLayout.X32_SINT;
                            return;
                        case PixelComponentFormat.Float16:
                            commonFormat = CommonPixelFormatLayout.X16_FLOAT;
                            return;
                        case PixelComponentFormat.Float32:
                            if (elements[0].Component == PixelComponent.Depth)
                            {
                                commonFormat = CommonPixelFormatLayout.D32_FLOAT;
                            }
                            else
                            {
                                commonFormat = CommonPixelFormatLayout.X32_FLOAT;
                            }
                            return;
                        case PixelComponentFormat.UNorm8:
                            commonFormat = CommonPixelFormatLayout.X8_UNORM;
                            return;
                        case PixelComponentFormat.SNorm8:
                            commonFormat = CommonPixelFormatLayout.X8_SNORM;
                            return;
                        case PixelComponentFormat.UNorm16:
                            commonFormat = CommonPixelFormatLayout.X16_UNORM;
                            return;
                        case PixelComponentFormat.SNorm16:
                            commonFormat = CommonPixelFormatLayout.X8_SNORM;
                            return;
                    }
                    break;
                case 2:
                    // Elements must be equal (or depth stencil).
                    if (elements[0].Format != elements[1].Format)
                    {
                        // Depth stencil formats are possible.
                        if (elements[0].Component == PixelComponent.Depth && 
                            elements[1].Component == PixelComponent.Stencil)
                        {
                            if (elements[0].Format == PixelComponentFormat.Float24 &&
                                elements[1].Format == PixelComponentFormat.UInt8)
                            {
                                commonFormat = CommonPixelFormatLayout.D24_UNORM_S8_UINT;
                            }
                            else if (elements[0].Format == PixelComponentFormat.Float32 &&
                                     elements[1].Format == PixelComponentFormat.UInt24)
                            {
                                commonFormat = CommonPixelFormatLayout.D32_FLOAT_S24_UINT;
                            }
                        }
                        break;
                    }


                    switch (elements[0].Format)
                    {
                        case PixelComponentFormat.Typeless8:
                            commonFormat = CommonPixelFormatLayout.X8Y8_TYPELESS;
                            return;
                        case PixelComponentFormat.Typeless16:
                            commonFormat = CommonPixelFormatLayout.X16Y16_TYPELESS;
                            return;
                        case PixelComponentFormat.Typeless32:
                            commonFormat = CommonPixelFormatLayout.X32Y32_TYPELESS;
                            return;
                        case PixelComponentFormat.UInt8:
                            commonFormat = CommonPixelFormatLayout.X8Y8_UINT;
                            return;
                        case PixelComponentFormat.SInt8:
                            commonFormat = CommonPixelFormatLayout.X8Y8_SINT;
                            return;
                        case PixelComponentFormat.UInt16:
                            commonFormat = CommonPixelFormatLayout.X16Y16_UINT;
                            return;
                        case PixelComponentFormat.SInt16:
                            commonFormat = CommonPixelFormatLayout.X16Y16_SINT;
                            return;
                        case PixelComponentFormat.UInt32:
                            commonFormat = CommonPixelFormatLayout.X32Y32_UINT;
                            return;
                        case PixelComponentFormat.SInt32:
                            commonFormat = CommonPixelFormatLayout.X32Y32_SINT;
                            return;
                        case PixelComponentFormat.Float16:
                            commonFormat = CommonPixelFormatLayout.X16Y16_FLOAT;
                            return;
                        case PixelComponentFormat.Float32:
                            commonFormat = CommonPixelFormatLayout.X32Y32_FLOAT;
                            return;
                        case PixelComponentFormat.UNorm8:
                            commonFormat = CommonPixelFormatLayout.X8Y8_UNORM;
                            return;
                        case PixelComponentFormat.SNorm8:
                            commonFormat = CommonPixelFormatLayout.X8Y8_SNORM;
                            return;
                        case PixelComponentFormat.UNorm16:
                            commonFormat = CommonPixelFormatLayout.X16Y16_UNORM;
                            return;
                        case PixelComponentFormat.SNorm16:
                            commonFormat = CommonPixelFormatLayout.X8Y8_SNORM;
                            return;
                    }

                    break;
                case 3:
                    // All components must be equal.
                    if (elements[0].Format != elements[1].Format ||
                        elements[1].Format != elements[2].Format) break;

                    switch (elements[0].Format)
                    {
                        case PixelComponentFormat.Typeless8:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8_TYPELESS;
                            return;
                        case PixelComponentFormat.Typeless16:
                            commonFormat = CommonPixelFormatLayout.X16Y16Z16_TYPELESS;
                            return;
                        case PixelComponentFormat.Typeless32:
                            commonFormat = CommonPixelFormatLayout.X32Y32Z32_TYPELESS;
                            return;
                        case PixelComponentFormat.UInt8:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8_UINT;
                            return;
                        case PixelComponentFormat.SInt8:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8_SINT;
                            return;
                        case PixelComponentFormat.UInt16:
                            commonFormat = CommonPixelFormatLayout.X16Y16Z16_UINT;
                            return;
                        case PixelComponentFormat.SInt16:
                            commonFormat = CommonPixelFormatLayout.X16Y16Z16_SINT;
                            return;
                        case PixelComponentFormat.UInt32:
                            commonFormat = CommonPixelFormatLayout.X32Y32Z32_UINT;
                            return;
                        case PixelComponentFormat.SInt32:
                            commonFormat = CommonPixelFormatLayout.X32Y32Z32_SINT;
                            return;
                        case PixelComponentFormat.Float16:
                            commonFormat = CommonPixelFormatLayout.X16Y16Z16_FLOAT;
                            return;
                        case PixelComponentFormat.Float32:
                            commonFormat = CommonPixelFormatLayout.X32Y32Z32_FLOAT;
                            return;
                        case PixelComponentFormat.UNorm8:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8_UNORM;
                            return;
                        case PixelComponentFormat.SNorm8:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8_SNORM;
                            return;
                        case PixelComponentFormat.UNorm16:
                            commonFormat = CommonPixelFormatLayout.X16Y16Z16_UNORM;
                            return;
                        case PixelComponentFormat.SNorm16:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8_SNORM;
                            return;
                    }
                    break;
                case 4:
                    // All components must be equal.
                    if (elements[0].Format != elements[1].Format ||
                        elements[1].Format != elements[2].Format ||
                        elements[2].Format != elements[3].Format) break;

                    switch (elements[0].Format)
                    {
                        case PixelComponentFormat.Typeless8:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8W8_TYPELESS;
                            return;
                        case PixelComponentFormat.Typeless16:
                            commonFormat = CommonPixelFormatLayout.X16Y16Z16W16_TYPELESS;
                            return;
                        case PixelComponentFormat.Typeless32:
                            commonFormat = CommonPixelFormatLayout.X32Y32Z32W32_TYPELESS;
                            return;
                        case PixelComponentFormat.UInt8:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8W8_UINT;
                            return;
                        case PixelComponentFormat.SInt8:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8W8_SINT;
                            return;
                        case PixelComponentFormat.UInt16:
                            commonFormat = CommonPixelFormatLayout.X16Y16Z16W16_UINT;
                            return;
                        case PixelComponentFormat.SInt16:
                            commonFormat = CommonPixelFormatLayout.X16Y16Z16W16_SINT;
                            return;
                        case PixelComponentFormat.UInt32:
                            commonFormat = CommonPixelFormatLayout.X32Y32Z32W32_UINT;
                            return;
                        case PixelComponentFormat.SInt32:
                            commonFormat = CommonPixelFormatLayout.X32Y32Z32W32_SINT;
                            return;
                        case PixelComponentFormat.Float16:
                            commonFormat = CommonPixelFormatLayout.X16Y16Z16W16_FLOAT;
                            return;
                        case PixelComponentFormat.Float32:
                            commonFormat = CommonPixelFormatLayout.X32Y32Z32W32_FLOAT;
                            return;
                        case PixelComponentFormat.UNorm8:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8W8_UNORM;
                            return;
                        case PixelComponentFormat.SNorm8:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8W8_SNORM;
                            return;
                        case PixelComponentFormat.UNorm16:
                            commonFormat = CommonPixelFormatLayout.X16Y16Z16W16_UNORM;
                            return;
                        case PixelComponentFormat.SNorm16:
                            commonFormat = CommonPixelFormatLayout.X8Y8Z8W8_SNORM;
                            return;
                    }
                    break;

            }
            commonFormat = CommonPixelFormatLayout.NotCommonLayout;
        }

        private void ValidateFormat()
        {
            // TODO: Compression format validation comes here.
        }

        private void CalculateSize()
        {
            ValidateFormat();

            // We first resolve all members
            size = 0;
            foreach (Element el in elements)
            {
                size = el.Resolve(size);
            }
        }

        #endregion

        #region Static Members


        /// <summary>
        /// Returns the size of format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The size in bytes.</returns>
        public static uint FormatSize(PixelComponentFormat format)
        {
            switch (format)
            {
                case PixelComponentFormat.Typeless8:
                    return 1;
                case PixelComponentFormat.Typeless16:
                    return 2;
                case PixelComponentFormat.Typeless32:
                    return 4;
                case PixelComponentFormat.Typeless64:
                    return 8;
                case PixelComponentFormat.UInt8:
                    return 1;
                case PixelComponentFormat.SInt8:
                    return 1;
                case PixelComponentFormat.UInt16:
                    return 2;
                case PixelComponentFormat.SInt16:
                    return 2;
                case PixelComponentFormat.UInt32:
                    return 4;
                case PixelComponentFormat.SInt32:
                    return 4;
                case PixelComponentFormat.UInt64:
                    return 8;
                case PixelComponentFormat.SInt64:
                    return 8;
                case PixelComponentFormat.Float16:
                    return 2;
                case PixelComponentFormat.Float24:
                    return 3;
                case PixelComponentFormat.Float32:
                    return 4;
                case PixelComponentFormat.Float64:
                    return 8;
                case PixelComponentFormat.UNorm8:
                    return 1;
                case PixelComponentFormat.SNorm8:
                    return 1;
                case PixelComponentFormat.UNorm16:
                    return 2;
                case PixelComponentFormat.SNorm16:
                    return 2;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Assigns a format group to format.
        /// </summary>
        /// <param name="format">The format group.</param>
        /// <returns></returns>
        public static PixelComponentFormatGroup FormatTypeGroup(PixelComponentFormat format)
        {
            switch (format)
            {
                case PixelComponentFormat.Typeless8:
                case PixelComponentFormat.Typeless16:
                case PixelComponentFormat.Typeless32:
                case PixelComponentFormat.Typeless64:
                    return PixelComponentFormatGroup.Typeless;
                case PixelComponentFormat.UInt8:
                case PixelComponentFormat.UInt16:
                case PixelComponentFormat.UInt32:
                case PixelComponentFormat.UInt64:
                    return PixelComponentFormatGroup.UInt;
                case PixelComponentFormat.SInt8:
                case PixelComponentFormat.SInt16:
                case PixelComponentFormat.SInt32:
                case PixelComponentFormat.SInt64:
                    return PixelComponentFormatGroup.SInt;
                case PixelComponentFormat.Float16:
                case PixelComponentFormat.Float24:
                case PixelComponentFormat.Float32:
                case PixelComponentFormat.Float64:
                    return PixelComponentFormatGroup.Float;
                case PixelComponentFormat.UNorm8:
                case PixelComponentFormat.UNorm16:
                    return PixelComponentFormatGroup.UNorm;
                case PixelComponentFormat.SNorm8:
                case PixelComponentFormat.SNorm16:
                    return PixelComponentFormatGroup.SNorm;
            }

            return PixelComponentFormatGroup.Unknown;
        }

        /// <summary>
        /// Checks if pixel component format is typeless.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>Is the format typeless.</returns>
        public static bool IsFormatTypeless(PixelComponentFormat format)
        {
            switch (format)
            {
                case PixelComponentFormat.Typeless8:
                case PixelComponentFormat.Typeless16:
                case PixelComponentFormat.Typeless32:
                case PixelComponentFormat.Typeless64:
                    return true;
                default: 
                    return false;
            }
        }

        /// <summary>
        /// Creates pixel format from string.
        /// </summary>
        /// <param name="str">String symbolicName of format.</param>
        /// <returns>Resulting format.</returns>
        public static PixelFormat Parse([NotEmpty] string str)
        {
            return Parse(str, CompressionFormat.None);
        }


        /// <summary>
        /// Froms the common layout.
        /// </summary>
        /// <param name="layout">The layout, must be typed. RGBA is used for values.</param>
        /// <returns></returns>
        public static PixelFormat FromCommonLayout(CommonPixelFormatLayout layout)
        {
            switch (layout)
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
                    return PixelFormat.Parse("R.UI8 G.UI8 B.UI8 A.UI8");
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
                    return PixelFormat.Parse("R.UN8 G.UN8 B.UN8 A.UN8");
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
                    return null;
                default:
                    break;
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Creates format from string and applies compression.
        /// </summary>
        /// <param name="str">The string symbolicName.</param>
        /// <param name="compression">Compression of format.</param>
        /// <returns>Pixel format instance.</returns>
        public static PixelFormat Parse([NotEmpty] string str, CompressionFormat compression)
        {

            // We now get components and parse each.
            string[] substr = str.Split(' ');
            Element[] elements = new Element[substr.Length];

            for (int i = 0; i < substr.Length; i++)
            {
                elements[i] = new Element(substr[i]);
            }

            // We now just create format.

            return new PixelFormat(elements, compression);
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Is the format typeless.
        /// </summary>
        public bool IsTypeless
        {
            get
            {
                return isTypeless;
            }
        }

        /// <summary>
        /// Common format, if available.
        /// </summary>
        public CommonPixelFormatLayout CommonFormatLayout
        {
            get 
            { 
                return commonFormat;
            }
        }

        /// <summary>
        /// Element access property.
        /// </summary>
        /// <param name="index">The index to access.</param>
        /// <returns>Element at position.</returns>
        public Element this[uint index]
        {
            get
            {
                return elements[index];
            }
        }

        /// <summary>
        /// The components in pixel format.
        /// </summary>
        public int Components
        {
            get
            {
                int r = 0;
                for (int i = 0; i < elements.Length; i++ )
                {
                    r |= (int)elements[i].Component;
                }
                return r;
            }
        }

        /// <summary>
        /// Does format contain component.
        /// </summary>
        /// <param name="c">The component type.</param>
        /// <returns>Does it contain component.</returns>
        public bool HasComponent(PixelComponent c)
        {
            return (Components & (int)c) != 0;
        }

        /// <summary>
        /// Finds the specified component and returns description element.
        /// </summary>
        /// <param name="c">The component.</param>
        /// <returns></returns>
        public Element Find(PixelComponent c)
        {
            foreach (Element el in this.elements)
            {
                if (el.Component == c) return el;
            }
            return null;
        }

        /// <summary>
        /// Number of elements in pixel format.
        /// </summary>
        public uint ElementCount
        {
            get { return (uint)elements.Length; }
        }

        /// <summary>
        /// Is this format compressed.
        /// </summary>
        public bool IsCompressed
        {
            get 
            { 
                return compression != CompressionFormat.None;
            }
        }

        /// <summary>
        /// Determines whether the specified format is compatible with this one.
        /// </summary>
        public bool IsCompatible([NotNull] PixelFormat fmt)
        {
            // If only one is not common layout quit.
            if (this.CommonFormatLayout == fmt.CommonFormatLayout) return true;

            // Make component match.
            if (ElementCount != fmt.ElementCount) return false;

            for (uint i = 0; i < ElementCount; i++)
            {
                if (!this[i].IsCompatible(fmt[i])) return false;
                
            }

            return true;
            
        }

        /// <summary>
        /// The compression pixel width. If format is not compressed, it is always 1. This
        /// property returns how many pixels in width are packed in one compressed element.
        /// </summary>
        public uint CompressionPixelWidth
        {
            get
            {
                switch (compression)
                {
                    case CompressionFormat.SharedExponent:
                        return 1;
                    case CompressionFormat.BlockCompressed:
                        return 4;
                    default:
                        return 1;
                }
            }
        }

        /// <summary>
        /// The compression pixel width. If format is not compressed, it is always 1. This
        /// property returns how many pixels in height are packed in one compressed element.
        /// </summary>
        public uint CompressionPixelHeight
        {
            get
            {
                switch (compression)
                {
                    case CompressionFormat.SharedExponent:
                        return 1;
                    case CompressionFormat.BlockCompressed:
                        return 4;
                    default:
                        return 1;
                }
            }
        }

        /// <summary>
        /// The compression pixel depth. If format is not compressed, it is always 1. This
        /// property returns how many pixels in depth are packed in one compressed element.
        /// 
        /// Note that not all compression formats support 3D compression; those return 0 for
        /// this component.
        /// </summary>
        public uint CompressionPixelDepth
        {
            get
            {
                switch (compression)
                {
                    case CompressionFormat.SharedExponent:
                        return 1;
                    case CompressionFormat.BlockCompressed:
                        return 4;
                    default:
                        return 1;
                }
            }
        }

        /// <summary>
        /// Compression size; it is the same of the basic element in format. ByteSize of
        /// one element is returned if format is not compressed.
        /// </summary>
        public uint CompressionSize
        {
            get
            {
                switch (compression)
                {
                    case CompressionFormat.BlockCompressed:
                    case CompressionFormat.SharedExponent:
                        // TODO: not implemented.
                        return Size;
                    default:
                        return Size;
                }
            }
        }

        /// <summary>
        /// Compression format used.
        /// </summary>
        public CompressionFormat Compression
        {
            get { return compression; }
        }

        /// <summary>
        /// The size of format in bytes, not taking compression into account.
        /// </summary>
        public uint Size
        {
            get 
            { 
                return size;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(elements.Length * 4);
            foreach (Element el in elements)
            {
                builder.Append(el.ToString());
                builder.Append(' ');
            }
            return builder.ToString();
        }

        private PixelFormat(Element[] els, CompressionFormat comp)
        {
            isTypeless = false;
            elements = els;
            // We must ensure two elements do not contain the same component.
            for (int j, i = 0; i < elements.Length; i++)
            {
                if (IsFormatTypeless(elements[i].Format)) isTypeless = true;

                for (j = 0; j < i; j++)
                {
                    if (elements[i].Component == elements[j].Component)
                    {
                        throw new InvalidPixelFormatException("Component "
                            + elements[i].Component.ToString() + " was repeated twice.");
                    }
                        
                }

                for (j = i + 1; j < elements.Length; j++)
                {
                    if (elements[i].Component == elements[j].Component)
                    {
                        throw new InvalidPixelFormatException("Component " 
                            + elements[i].Component.ToString() + " was repeated twice.");
                    }
                }
            }

            compression = comp;

            // We must calculate size.
            CalculateSize();

            // Check for common format layout.
            CheckForCommonFormatLayout();
            
        }

        #endregion

        #region IEquatable<PixelFormat> Members

        public bool Equals(PixelFormat other)
        {
            if (Object.ReferenceEquals(this, other)) return true;
            if(this.elements.Length != other.elements.Length) return false;
            for (int i = 0; i < elements.Length; i++)
            {
                if (!elements[i].Equals(other.elements[i])) return false;
            }
            return true;
        }

        #endregion

        #region IEnumerable<Element> Members

        public IEnumerator<PixelFormat.Element> GetEnumerator()
        {
            foreach (Element el in elements)
            {
                yield return el;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (Element el in elements)
            {
                yield return el;
            }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class FormatTest
    {
        public void Test()
        {
            PixelFormat fmt = PixelFormat.Parse("R.UB G.UB B.UB A.UB");
        }
    }
#endif
}
