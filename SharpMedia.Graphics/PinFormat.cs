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

namespace SharpMedia.Graphics
{



    /// <summary>
    /// The format of certain element.
    /// </summary>
    public enum PinFormat : uint
    {

        /// <summary>
        /// Undefined format.
        /// </summary>
        Undefined,

        /// <summary>
        /// Integer constant. It is usually 32-bit precission (I).
        /// </summary>
        Integer,

        /// <summary>
        /// Integer vector of two elements (Ix2).
        /// </summary>
        Integerx2,

        /// <summary>
        /// Integer vector of three elements (Ix3).
        /// </summary>
        Integerx3,

        /// <summary>
        /// Integer vector of four elements (Ix4).
        /// </summary>
        Integerx4,

        /// <summary>
        /// UInteger constant. It is usually 32-bit precission (UI).
        /// </summary>
        UInteger,

        /// <summary>
        /// UInteger vector of two elements (UIx2).
        /// </summary>
        UIntegerx2,

        /// <summary>
        /// UInteger vector of three elements (UIx3).
        /// </summary>
        UIntegerx3,

        /// <summary>
        /// UInteger vector of four elements (UIx4).
        /// </summary>
        UIntegerx4,

        /// <summary>
        /// A boolean data type (B).
        /// </summary>
        Bool,

        /// <summary>
        /// A boolean vector of 2 elements (Bx2).
        /// </summary>
        Boolx2,

        /// <summary>
        /// A boolean vector of 3 element (Bx3).
        /// </summary>
        Boolx3,

        /// <summary>
        /// A boolean vector of 4 elements (Bx4).
        /// </summary>
        Boolx4,

        /// <summary>
        /// A normalized (signed) floating point value [-1,1] (SN).
        /// </summary>
        SNorm,

        /// <summary>
        /// A normalized (signed) floating point 2d vector value [-1,1] (SNx2).
        /// </summary>
        SNormx2,

        /// <summary>
        /// A normalized (signed) floating point 3d vector value [-1,1] (SNx3).
        /// </summary>
        SNormx3,

        /// <summary>
        /// A normalized (signed) floating point 4d vector value [-1,1] (SNx4).
        /// </summary>
        SNormx4,

        /// <summary>
        /// A unsigned normalized (signed) floating point value [0,1] (UN).
        /// </summary>
        UNorm,

        /// <summary>
        /// A unsigned normalized (signed) floating point 2d vector value [0,1] (N).
        /// </summary>
        UNormx2,

        /// <summary>
        /// A unsigned normalized (signed) floating point 3d vector value [0,1] (N).
        /// </summary>
        UNormx3,

        /// <summary>
        /// A unsigned normalized (signed) floating point 4d vector value [0,1] (N).
        /// </summary>
        UNormx4,

        /// <summary>
        /// A floating value (F).
        /// </summary>
        Float,

        /// <summary>
        /// A floating value 2D vector (Fx2).
        /// </summary>
        Floatx2,

        /// <summary>
        /// A floating value 3D vector (Fx3).
        /// </summary>
        Floatx3,

        /// <summary>
        /// A floating value 4D vector (Fx4).
        /// </summary>
        Floatx4,

        /// <summary>
        /// A buffer texture, similiar to 1D texture.
        /// </summary>
        BufferTexture,

        /// <summary>
        /// A reference to 1D texture or buffer.
        /// </summary>
        Texture1D,

        /// <summary>
        /// A reference to 1D texture array.
        /// </summary>
        Texture1DArray,

        /// <summary>
        /// A 2D texture reference.
        /// </summary>
        Texture2D,

        /// <summary>
        /// A 2D texture array reference.
        /// </summary>
        Texture2DArray,

        /// <summary>
        /// A cubemap texture reference.
        /// </summary>
        TextureCube,

        /// <summary>
        /// A 3D texture reference.
        /// </summary>
        Texture3D,

        /// <summary>
        /// A sampler.
        /// </summary>
        Sampler,

        /// <summary>
        /// An interface pin.
        /// </summary>
        Interface,

        /// <summary>
        /// A float 2x2 matrix.
        /// </summary>
        Float2x2,

        /// <summary>
        /// A float 3x3 matrix.
        /// </summary>
        Float3x3,

        /// <summary>
        /// A float 4x4 matrix.
        /// </summary>
        Float4x4,

        /// <summary>
        /// An integer 2x2 matrix.
        /// </summary>
        Integer2x2,

        /// <summary>
        /// An integer 3x3 matrix.
        /// </summary>
        Integer3x3,

        /// <summary>
        /// An integer 4x4 matrix.
        /// </summary>
        Integer4x4,

        /// <summary>
        /// An unsigned integer 2x2 matrix.
        /// </summary>
        UInteger2x2,

        /// <summary>
        /// An unsigned integer 3x3 matrix.
        /// </summary>
        UInteger3x3,

        /// <summary>
        /// An unsigned integer 4x4 matrix.
        /// </summary>
        UInteger4x4,

        /// <summary>
        /// A signed uniform normalized 2x2 matrix.
        /// </summary>
        SNorm2x2,

        /// <summary>
        /// A signed uniform normalized 3x3 matrix.
        /// </summary>
        SNorm3x3,

        /// <summary>
        /// A signed uniform normalized 4x4 matrix.
        /// </summary>
        SNorm4x4,

        /// <summary>
        /// A unsigned uniform normalized 2x2 matrix.
        /// </summary>
        UNorm2x2,

        /// <summary>
        /// A unsigned uniform normalized 3x3 matrix.
        /// </summary>
        UNorm3x3,

        /// <summary>
        /// A unsigned uniform normalized 4x4 matrix.
        /// </summary>
        UNorm4x4
    }


    /// <summary>
    /// Vertex component is a binding for shader. Some components are also special.
    /// </summary>
    [Flags]
    public enum PinComponent : long
    {

        /// <summary>
        /// Symbolic binding "none" (X), used to align data.
        /// </summary>
        None = 0,

        /// <summary>
        /// Vertex position (P).
        /// </summary>
        Position = 1L << 0,

        /// <summary>
        /// Normal (N).
        /// </summary>
        Normal = 1 << 1,

        /// <summary>
        /// Colour (C).
        /// </summary>
        Colour = 1L << 2,

        /// <summary>
        /// Secondary colour (SC).
        /// </summary>
        SecondaryColour = 1L << 3,

        /// <summary>
        /// Blend weights, up to 4 (BW).
        /// </summary>
        BlendWeights = 1L << 4,

        /// <summary>
        /// Blend indices, up to 4 (BI).
        /// </summary>
        BlendIndices = 1L << 5,

        /// <summary>
        /// BiNormal at this position (B).
        /// </summary>
        BiNormal = 1L << 6,

        /// <summary>
        /// Tangent (T).
        /// </summary>
        Tangent = 1L << 7,

        /// <summary>
        /// Texture coordinates (Tx).
        /// </summary>
        TexCoord0 = 1L << 8,
        TexCoord1 = 1L << 9,
        TexCoord2 = 1L << 10,
        TexCoord3 = 1L << 11,
        TexCoord4 = 1L << 12,
        TexCoord5 = 1L << 13,
        TexCoord6 = 1L << 14,
        TexCoord7 = 1L << 15,
        TexCoord8 = 1L << 16,
        TexCoord9 = 1L << 17,
        TexCoord10 = 1L << 18,
        TexCoord11 = 1L << 19,
        TexCoord12 = 1L << 20,
        TexCoord13 = 1L << 21,
        TexCoord14 = 1L << 22,
        TexCoord15 = 1L << 23,

        /// <summary>
        /// User defined (Ux).
        /// </summary>
        User0 = 1L << 24,
        User1 = 1L << 25,
        User2 = 1L << 26,
        User3 = 1L << 27,
        User4 = 1L << 28,
        User5 = 1L << 29,
        User6 = 1L << 30,
        User7 = 1L << 31,
        User8 = 1L << 32,
        User9 = 1L << 33,
        User10 = 1L << 34,
        User11 = 1L << 35,
        User12 = 1L << 36,
        User13 = 1L << 37,
        User14 = 1L << 38,
        User15 = 1L << 39,

        /// <summary>
        /// A depth output; only available by pixel shader as
        /// an output binding.
        /// </summary>
        Depth = 1L << 40,

        /// <summary>
        /// Render targets, only available by pixel shader as
        /// output binding (texture outputs not avaialable by pixel shader).
        /// </summary>
        RenderTarget0 = 1L << 41,
        RenderTarget1 = 1L << 42,
        RenderTarget2 = 1L << 43,
        RenderTarget3 = 1L << 44,
        RenderTarget4 = 1L << 45,
        RenderTarget5 = 1L << 46,
        RenderTarget6 = 1L << 47,
        RenderTarget7 = 1L << 48,

        /// <summary>
        /// System generated values (cannot bind to them).
        /// </summary>
        VertexID = 1L << 49,
        PrimitiveID = 1L << 50,
        InstanceID = 1L << 51,

        /// <summary>
        /// A render target array index for pixel stage to choose.
        /// </summary>
        RenderTargetArrayIndex = 1L << 52,

        /// <summary>
        /// A viewport array index for geometry shader output. 
        /// </summary>
		ViewportArrayIndex = 1L << 53


    }

}
