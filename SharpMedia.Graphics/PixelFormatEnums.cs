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

namespace SharpMedia.Graphics
{

    /// <summary>
    /// Components of pixel format.
    /// </summary>
    public enum PixelComponent
    {
        /// <summary>
        /// Not known component (N).
        /// </summary>
        NotKnown = 1,
        /// <summary>
        /// Red component (R).
        /// </summary>
        Red = 1 << 1,
        /// <summary>
        /// Green component (G).
        /// </summary>
        Green = 1 << 2,
        /// <summary>
        /// Blue component (B).
        /// </summary>
        Blue = 1 << 3,
        /// <summary>
        /// Alpha component (A).
        /// </summary>
        Alpha = 1 << 6,
        /// <summary>
        /// Luminance, used by lightmaps (L).
        /// </summary>
        Luminance = 1 << 7,
        /// <summary>
        /// Bump, e.g. the height (Bump).
        /// </summary>
        Bump = 1 << 8,
        /// <summary>
        /// NormalX coordinate, used for normal maps (Nx).
        /// </summary>
        NormalX = 1 << 9,
        /// <summary>
        /// NormalY coordinate, used for normal maps (Ny).
        /// </summary>
        NormalY = 1 << 10,
        /// <summary>
        /// NormalZ coordinate, used for normal maps (Nz).
        /// </summary>
        NormalZ = 1 << 11,
        /// <summary>
        /// Reflection coordinate (Refl).
        /// </summary>
        Reflection = 1 << 12,
        /// <summary>
        /// Refraction coordinate (Refr).
        /// </summary>
        Refraction = 1 << 13,
        /// <summary>
        /// Depth format (D).
        /// </summary>
        Depth = 1 << 14,
        /// <summary>
        /// Stencil format (S).
        /// </summary>
        Stencil = 1 << 15,

        /// <summary>
        /// User defined usage, (Un).
        /// </summary>
        User0 = 1 << 16,
        User1 = 1 << 17,
        User2 = 1 << 18,
        User3 = 1 << 19,
        User4 = 1 << 20,
        User5 = 1 << 21,
        User6 = 1 << 22,
        User7 = 1 << 23,
        User8 = 1 << 24,
        User9 = 1 << 25
    }

    /// <summary>
    /// The pixel component format group.
    /// </summary>
    public enum PixelComponentFormatGroup
    {
        /// <summary>
        /// Typeless component format.
        /// </summary>
        Typeless,

        /// <summary>
        /// Unsigned int component format.
        /// </summary>
        UInt,

        /// <summary>
        /// Signed int component format.
        /// </summary>
        SInt,

        /// <summary>
        /// Unsigned int uniformly normalized group.
        /// </summary>
        UNorm,

        /// <summary>
        /// Signed int uniformly normalized group.
        /// </summary>
        SNorm,

        /// <summary>
        /// Floating point group.
        /// </summary>
        Float,

        /// <summary>
        /// Unknown group.
        /// </summary>
        Unknown
    }

    /// <summary>
    /// The pixel, per component format.
    /// </summary>
    public enum PixelComponentFormat
    {
        /// <summary>
        /// A typeless 8-bit format (T8).
        /// </summary>
        Typeless8,

        /// <summary>
        /// A typeless 16-bit format (T16).
        /// </summary>
        Typeless16,

        /// <summary>
        /// A typeless 24-bit format (T24).
        /// </summary>
        Typeless24,

        /// <summary>
        /// A typeless 32-bit format (T32).
        /// </summary>
        Typeless32,

        /// <summary>
        /// A typeless 64-bit format (T64).
        /// </summary>
        Typeless64,

        /// <summary>
        /// A unsigned int 8-bit format (UI8).
        /// </summary>
        UInt8,

        /// <summary>
        /// A signed int 8-bit format (SI8).
        /// </summary>
        SInt8,

        /// <summary>
        /// An unsigned int 16-bit format (UI16).
        /// </summary>
        UInt16,

        /// <summary>
        /// An unsigned int 16-bit format (UI16).
        /// </summary>
        SInt16,

        /// <summary>
        /// An unsigned 24-bit integer format (UI24).
        /// </summary>
        UInt24,

        /// <summary>
        /// A signed 24-bit integer format (SI24).
        /// </summary>
        SInt24,

        /// <summary>
        /// An unsigned int 32-bit format (UI32).
        /// </summary>
        UInt32,

        /// <summary>
        /// An unsigned int 32-bit format (UI32).
        /// </summary>
        SInt32,

        /// <summary>
        /// An unsigned int 64-bit format (UI64).
        /// </summary>
        UInt64,

        /// <summary>
        /// An unsigned int 64-bit format (UI64).
        /// </summary>
        SInt64,

        /// <summary>
        /// A float 16-bit format (F16).
        /// </summary>
        Float16,

        /// <summary>
        /// A float 24-bit format (F24).
        /// </summary>
        Float24,

        /// <summary>
        /// A float 32-bit format (F32).
        /// </summary>
        Float32,

        /// <summary>
        /// A float 64-bit format (F64).
        /// </summary>
        Float64,

        /// <summary>
        /// A unsigned integer 8-bit format viewed in [0,1] range (UN8).
        /// </summary>
        UNorm8,

        /// <summary>
        /// A signed integer 8-bit format viewed in [-1,1] range (SN8).
        /// </summary>
        SNorm8,

        /// <summary>
        /// A unsigned integer 16-bit format viewed in [0,1] range (UN16).
        /// </summary>
        UNorm16,

        /// <summary>
        /// A signed integer 16-bit format viewed in [-1,1] range (SN16).
        /// </summary>
        SNorm16
    }

    /// <summary>
    /// Image compression used, asociated with format. Compression formats
    /// may require certain layouts.
    /// </summary>
    public enum CompressionFormat
    {
        /// <summary>
        /// No compression.
        /// </summary>
        None,

        /// <summary>
        /// Shared exponent, only to X8Y8Z8.
        /// </summary>
        SharedExponent,

        /// <summary>
        /// A block compressed.
        /// </summary>
        BlockCompressed
    }

    /// <summary>
    /// The common pixel format layout allows to map pixel formats to actual hardware formats. It format is
    /// mappable to any of those, it can probably run on GPU hardware.
    /// </summary>
    /// <remarks>
    /// A "R.UI8 G.UI8 B.UI8" and "B.UI8 G.UI8 R.UI8" have the same common layout formats. In shader,
    /// texture will contain red in ".x" accessor for the first format and in ".z" in the second.
    /// </remarks>
    public enum CommonPixelFormatLayout
    {
        // Depth-Stencil Formats --------------------

        /// <summary>
        /// A 24-bit depth, 8-bit stencil.
        /// </summary>
        D24_UNORM_S8_UINT,

        /// <summary>
        /// A floating point 32-bit depth format.
        /// </summary>
        D32_FLOAT,

        /// <summary>
        /// A floatin point 32-bit depth and 24 bit stencil format.
        /// </summary>
        D32_FLOAT_S24_UINT,

        // ------------------------------------------

        /// <summary>
        /// 32-bit 4 component typeless format.
        /// </summary>
        X8Y8Z8W8_TYPELESS,

        /// <summary>
        /// 64-bit 4 component typeless format.
        /// </summary>
        X16Y16Z16W16_TYPELESS,

        /// <summary>
        /// 128-bit 4-bit component typeless format.
        /// </summary>
        X32Y32Z32W32_TYPELESS,

        /// <summary>
        /// 32-bit 4 component uint format.
        /// </summary>
        X8Y8Z8W8_UINT,

        /// <summary>
        /// 64-bit 4 component uint format.
        /// </summary>
        X16Y16Z16W16_UINT,

        /// <summary>
        /// 128-bit 4 component uint format.
        /// </summary>
        X32Y32Z32W32_UINT,

        /// <summary>
        /// 32-bit 4 component signed int format.
        /// </summary>
        X8Y8Z8W8_SINT,

        /// <summary>
        /// 64-bit 4 component signed int format.
        /// </summary>
        X16Y16Z16W16_SINT,

        /// <summary>
        /// 128-bit 4 component signed int format.
        /// </summary>
        X32Y32Z32W32_SINT,

        /// <summary>
        /// 32-bit 4 component unsigned int normalized format.
        /// </summary>
        X8Y8Z8W8_UNORM,

        /// <summary>
        /// 64-bit 4 component unsigned int normalized format.
        /// </summary>
        X16Y16Z16W16_UNORM,

        /// <summary>
        /// 32-bit 4 component signed int normalized format.
        /// </summary>
        X8Y8Z8W8_SNORM,

        /// <summary>
        /// 64-bit 4 component unsigned int normalized format.
        /// </summary>
        X16Y16Z16W16_SNORM,

        /// <summary>
        /// 64-bit 4 component floating point format.
        /// </summary>
        X16Y16Z16W16_FLOAT,

        /// <summary>
        /// 128-bit 4 component floating point format.
        /// </summary>
        X32Y32Z32W32_FLOAT,


        // ---------------------------------------------------------

        /// <summary>
        /// 24-bit 3 component typeless format.
        /// </summary>
        X8Y8Z8_TYPELESS,

        /// <summary>
        /// 48-bit 3 component typeless format.
        /// </summary>
        X16Y16Z16_TYPELESS,

        /// <summary>
        /// 96-bit 3 component typeless format.
        /// </summary>
        X32Y32Z32_TYPELESS,

        /// <summary>
        /// 24-bit 3 component uint format.
        /// </summary>
        X8Y8Z8_UINT,

        /// <summary>
        /// 48-bit 3 component uint format.
        /// </summary>
        X16Y16Z16_UINT,

        /// <summary>
        /// 96-bit 3 component uint format.
        /// </summary>
        X32Y32Z32_UINT,

        /// <summary>
        /// 32-bit 3 component signed int format.
        /// </summary>
        X8Y8Z8_SINT,

        /// <summary>
        /// 48-bit 3 component signed int format.
        /// </summary>
        X16Y16Z16_SINT,

        /// <summary>
        /// 96-bit 3 component signed int format.
        /// </summary>
        X32Y32Z32_SINT,

        /// <summary>
        /// 24-bit 3 component unsigned int normalized format.
        /// </summary>
        X8Y8Z8_UNORM,

        /// <summary>
        /// 48-bit 3 component unsigned int normalized format.
        /// </summary>
        X16Y16Z16_UNORM,

        /// <summary>
        /// 24-bit 3 component signed int normalized format.
        /// </summary>
        X8Y8Z8_SNORM,

        /// <summary>
        /// 48-bit 3 component unsigned int normalized format.
        /// </summary>
        X16Y16Z16_SNORM,

        /// <summary>
        /// 48-bit 3 component floating point format.
        /// </summary>
        X16Y16Z16_FLOAT,

        /// <summary>
        /// 96-bit 3 component floating point format.
        /// </summary>
        X32Y32Z32_FLOAT,


        // --------------------------------------------------------

        /// <summary>
        /// 16-bit 2 component typeless format.
        /// </summary>
        X8Y8_TYPELESS,

        /// <summary>
        /// 32-bit 2 component typeless format.
        /// </summary>
        X16Y16_TYPELESS,

        /// <summary>
        /// 64-bit 2 component typeless format.
        /// </summary>
        X32Y32_TYPELESS,

        /// <summary>
        /// 16-bit 2 component uint format.
        /// </summary>
        X8Y8_UINT,

        /// <summary>
        /// 32-bit 2 component uint format.
        /// </summary>
        X16Y16_UINT,

        /// <summary>
        /// 64-bit 2 component uint format.
        /// </summary>
        X32Y32_UINT,

        /// <summary>
        /// 16-bit 2 component signed int format.
        /// </summary>
        X8Y8_SINT,

        /// <summary>
        /// 32-bit 2 component signed int format.
        /// </summary>
        X16Y16_SINT,

        /// <summary>
        /// 64-bit 2 component signed int format.
        /// </summary>
        X32Y32_SINT,

        /// <summary>
        /// 16-bit 2 component unsigned int normalized format.
        /// </summary>
        X8Y8_UNORM,

        /// <summary>
        /// 32-bit 2 component unsigned int normalized format.
        /// </summary>
        X16Y16_UNORM,

        /// <summary>
        /// 16-bit 2 component signed int normalized format.
        /// </summary>
        X8Y8_SNORM,

        /// <summary>
        /// 32-bit 2 component unsigned int normalized format.
        /// </summary>
        X16Y16_SNORM,

        /// <summary>
        /// 32-bit 2 component floating point format.
        /// </summary>
        X16Y16_FLOAT,

        /// <summary>
        /// 64-bit 2 component floating point format.
        /// </summary>
        X32Y32_FLOAT,

        // ---------------------------------------------------

        /// <summary>
        /// 8-bit 1 component typeless format.
        /// </summary>
        X8_TYPELESS,

        /// <summary>
        /// 16-bit 1 component typeless format.
        /// </summary>
        X16_TYPELESS,

        /// <summary>
        /// 32-bit 1 component typeless format.
        /// </summary>
        X32_TYPELESS,

        /// <summary>
        /// 8-bit 1 component uint format.
        /// </summary>
        X8_UINT,

        /// <summary>
        /// 16-bit 1 component uint format.
        /// </summary>
        X16_UINT,

        /// <summary>
        /// 32-bit 1 component uint format.
        /// </summary>
        X32_UINT,

        /// <summary>
        /// 8-bit 1 component signed int format.
        /// </summary>
        X8_SINT,

        /// <summary>
        /// 16-bit 1 component signed int format.
        /// </summary>
        X16_SINT,

        /// <summary>
        /// 32-bit 1 component signed int format.
        /// </summary>
        X32_SINT,

        /// <summary>
        /// 8-bit 1 component unsigned int normalized format.
        /// </summary>
        X8_UNORM,

        /// <summary>
        /// 16-bit 1 component unsigned int normalized format.
        /// </summary>
        X16_UNORM,

        /// <summary>
        /// 8-bit 1 component signed int normalized format.
        /// </summary>
        X8_SNORM,

        /// <summary>
        /// 16-bit 1 component unsigned int normalized format.
        /// </summary>
        X16_SNORM,

        /// <summary>
        /// 16-bit 1 component floating point format.
        /// </summary>
        X16_FLOAT,

        /// <summary>
        /// 32-bit 1 component floating point format.
        /// </summary>
        X32_FLOAT,

        /// <summary>
        /// A not common layout.
        /// </summary>
        NotCommonLayout

    }

}
