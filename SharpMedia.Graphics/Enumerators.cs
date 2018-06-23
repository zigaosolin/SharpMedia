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
    /// The format usage flags.
    /// </summary>
    [Flags]
    public enum FormatUsage
    {
        /// <summary>
        /// Cannot use the format.
        /// </summary>
        None = 0,

        /// <summary>
        /// Can use it in shaders as textures 
        /// </summary>
        ShaderTexture = 1,

        /// <summary>
        /// Can use it in render target.
        /// </summary>
        RenderTarget = 2,

        /// <summary>
        /// Can use if for blending.
        /// </summary>
        BlendRenderTarget = 4,

        /// <summary>
        /// Can use it for multisapling.
        /// </summary>
        MultisampleRenderTarget = 8

    }

    /// <summary>
    /// A texture usage hint, use the most restrictive combination to avoid potencial
    /// bugs and/or to give a hint to underlaying renderer.
    /// </summary>
    [Flags]
    public enum TextureUsage : int
    {
        /// <summary>
        /// No texture usage.
        /// </summary>
        None = 0,

        /// <summary>
        /// A texture usage means read usage from this target; e.g. can be bound to shaders
        /// as input source.
        /// </summary>
        Texture = 1,

        /// <summary>
        /// A render target usage; means that it can be bound to shaders as output (through
        /// MRT). Note that the same texture cannot be bound to shader as input and output at the
        /// same time.
        /// </summary>
        RenderTarget = 2,

        /// <summary>
        /// The depth-stencil texture target, cannot be combined with render target if normal
        /// texture, can be both if swap chain.
        /// </summary>
        DepthStencilTarget = 8,

        /// <summary>
        /// Can be bound as cubemap (only valid for array of 6 2D textures).
        /// </summary>
        CubeMap = 16
    }


    /// <summary>
    /// The interpolation mode.
    /// </summary>
    public enum InterpolationMode
    {
        /// <summary>
        /// Linear interpolation mode.
        /// </summary>
        Linear,

        /// <summary>
        /// Smoothstep mode.
        /// </summary>
        Smoothstep
    }


    /// <summary>
    /// Possible buffer usages.
    /// </summary>
    [Flags]
    public enum BufferUsage : int
    {
        /// <summary>
        /// Buffer can be bounds as vertices.
        /// </summary>
        VertexBuffer = 1,

        /// <summary>
        /// Buffer can be bound as indices.
        /// </summary>
        IndexBuffer = 2,

        /// <summary>
        /// Buffer can be bound as constants.
        /// </summary>
        ConstantBuffer = 4,

        /// <summary>
        /// Buffer can be bound to geometry output stage.
        /// </summary>
        GeometryOutput = 8,

        /// <summary>
        /// Buffer can be bounds as render target.
        /// </summary>
        RenderTarget = 16
    }

    /// <summary>
    /// A CPU access flags.
    /// </summary>
    [Flags]
    public enum CPUAccess
    {
        /// <summary>
        /// No CPU access.
        /// </summary>
        None = 0,
 
        /// <summary>
        /// Read access.
        /// </summary>
        Read = 1,

        /// <summary>
        /// Write access.
        /// </summary>
        Write = 2,

        /// <summary>
        /// Both access.
        /// </summary>
        ReadWrite = Read|Write
    }

    /// <summary>
    /// A mipmap build filter.
    /// </summary>
    public enum BuildImageFilter
    {
        /// <summary>
        /// The nearest filter.
        /// </summary>
        Nearest,

        /// <summary>
        /// Quad filter.
        /// </summary>
        Quad,

        /// <summary>
        /// Gaussian filter
        /// </summary>
        Gaussian
    }

    /// <summary>
    /// A binding stage of some element.
    /// </summary>
    [Flags]
    public enum BindingStage
    {
        /// <summary>
        /// To be used by vertex shader.
        /// </summary>
        VertexShader = 1,

        /// <summary>
        /// To be used by pixel shader.
        /// </summary>
        PixelShader = 2,

        /// <summary>
        /// To be used by geometry shader.
        /// </summary>
        GeometryShader = 4,

        /// <summary>
        /// All bindig stages combiner.
        /// </summary>
        All = VertexShader|PixelShader|GeometryShader

    }


    /// <summary>
    /// The tolpology that can be used for graphics. Some topologies require
    /// specific enumerators.
    /// </summary>
    public enum Topology
    {
        /// <summary>
        /// A collection of points; each vertex is independant.
        /// </summary>
        Point,

        /// <summary>
        /// Tow vertices are paired to form a line.
        /// </summary>
        Line,

        /// <summary>
        /// Multiple vertices are paired to form a shape with lines.
        /// </summary>
        LineStrip,

        /// <summary>
        /// Three vertices form a triangle.
        /// </summary>
        Triangle,

        /// <summary>
        /// Three vertices form a triangle and any aditional vertex forms another triangle, defined
        /// by last three vertices.
        /// </summary>
        TriangleStrip

        // TODO: GS shape support
    }

    /// <summary>
    /// Clear options.
    /// </summary>
    [Flags]
    public enum ClearOptions
    {
        /// <summary>
        /// Clears depth render target.
        /// </summary>
        Depth = 1,

        /// <summary>
        /// Clears stencil render target.
        /// </summary>
        Stencil = 2
    }

    /// <summary>
    /// The creation mode.
    /// </summary>
    public enum DeviceMode
    {
        /// <summary>
        /// Explicit mode of device, no-one else can create device.
        /// </summary>
        Explicit,

        /// <summary>
        /// Shared device mode, must be already owned.
        /// </summary>
        Shared,

        /// <summary>
        /// Device is in shared mode, owned by this process.
        /// </summary>
        SharedOwned
    }
}
