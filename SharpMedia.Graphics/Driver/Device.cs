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

namespace SharpMedia.Graphics.Driver
{

    /// <summary>
    /// A device listener interface.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IDeviceListener
    {
        /// <summary>
        /// Called when master device is disposed.
        /// </summary>
        void Disposed();

        /// <summary>
        /// Device reset is issued.
        /// </summary>
        void Reset();

        /// <summary>
        /// Device lost occured.
        /// </summary>
        void DeviceLost();
    }

    /// <summary>
    /// Resource usage types.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public enum UsageDimensionType
    {
        Buffer,
        Texture1D,
        Texture2D,
        Texture2DMS,
        Texture3D,
        TextureCube,
        Texture1DArray,
        Texture2DArray,
        Texture2DMSArray
    }

    [Linkable(LinkMask.Drivers)]
    public sealed class SharedTextureInfo
    {
        public Driver.ITexture Texture;
        public uint Width;
        public uint Height;
        public uint Depth;
        public uint Mipmaps;
        public Usage Usage;
        public PixelFormat Format;
        public TextureUsage TextureUsage;
    }

    /// <summary>
    /// A vertex binding element.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public struct VertexBindingElement
    {
        public VertexFormat Format;
        public UpdateFrequency UpdateFrequency;
        public uint UpdateFrequencyCount;
    }

    /// <summary>
    /// A device interface. Device is actual interface that can render.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IDevice : IDisposable
    {
        /// <summary>
        /// Symbolic name of the device.
        /// </summary>
        string Name { get; }

        #region Messaging

        /// <summary>
        /// Registers device listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        void RegisterListener(IDeviceListener listener);

        /// <summary>
        /// Unregisters device listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        void UnRegisterListener();

        #endregion

        #region Capabilities

        /// <summary>
        /// Do we support format.
        /// </summary>
        /// <param name="fmt">The format.</param>
        FormatUsage FormatSupport(CommonPixelFormatLayout fmt);

        /// <summary>
        /// The multisampling quality.
        /// </summary>
        uint MultiSamplingQuality(CommonPixelFormatLayout format, uint sampleCount);

        /// <summary>
        /// The device memory available.
        /// </summary>
        ulong DeviceMemory { get; }

        #endregion

        #region State Creation

        /// <summary>
        /// Clears all states.
        /// </summary>
        void ClearStates();

        /// <summary>
        /// Creates immutable blend state object based on description.
        /// </summary>
        /// <param name="desc">The descriptor object.</param>
        IBlendState CreateState(States.BlendState desc);

        /// <summary>
        /// Creates immutable blend state object based on description.
        /// </summary>
        /// <param name="desc">The descriptor object.</param>
        IRasterizationState CreateState(States.RasterizationState desc);

        /// <summary>
        /// Creates immutable blend state object based on description.
        /// </summary>
        /// <param name="desc">The descriptor object.</param>
        IDepthStencilState CreateState(States.DepthStencilState desc);

        /// <summary>
        /// Creates immutable blend state object based on description.
        /// </summary>
        /// <param name="desc">The descriptor object.</param>
        ISamplerState CreateState(States.SamplerState desc);

        #endregion

        #region Resource Creation

        /// <summary>
        /// Binds vertex format defined by vertices.
        /// </summary>
        /// <param name="fmt">The format.</param>
        /// <returns>Resulting biding.</returns>    ASFG
        IVerticesBindingLayout CreateVertexBinding(VertexBindingElement[] elements);


        /// <summary>
        /// Creates the buffer.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <param name="access">The access by CPU.</param>
        /// <param name="length">The length.</param>
        /// <param name="initialData">Initial data, can be null (but not for all usages).</param>
        /// <returns></returns>
        IBuffer CreateBuffer(BufferUsage bufferUsage, Usage usage, CPUAccess access, ulong length, byte[] initialData);


        /// <summary>
        /// Creates the texture1 D.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <param name="fmt">The FMT.</param>
        /// <param name="width">The width.</param>
        /// <param name="mipmapLevels">The mipmap levels.</param>
        /// <param name="textureUsage">The texture usage.</param>
        /// <returns></returns>
        ITexture1D CreateTexture1D(Usage usage, CommonPixelFormatLayout fmt, CPUAccess access, uint width,
                                         uint mipmapLevels, TextureUsage textureUsage, byte[][] data);
        /// <summary>
        /// Creates the texture2 D.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <param name="fmt">The FMT.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipmapLevels">The mipmap levels.</param>
        /// <param name="textureUsage">The texture usage.</param>
        /// <param name="sampleCount">The sample count.</param>
        /// <param name="sampleQuality">The sample quality.</param>
        /// <param name="data">Data, can be null if not initialized at construction.</param>
        /// <returns></returns>
        ITexture2D CreateTexture2D(Usage usage, CommonPixelFormatLayout fmt, CPUAccess access, uint width, uint height,
                                         uint mipmapLevels, TextureUsage textureUsage,
                                         uint sampleCount, uint sampleQuality, byte[][] data);


        /// <summary>
        /// Creates the texture 3D.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <param name="fmt">The FMT.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="mipmapLevels">The mipmap levels.</param>
        /// <param name="textureUsage">The texture usage.</param>
        /// <returns></returns>
        ITexture3D CreateTexture3D(Usage usage, CommonPixelFormatLayout fmt, CPUAccess access, uint width, uint height, uint depth,
                                             uint mipmapLevels, TextureUsage textureUsage, byte[][] data);

        /// <summary>
        /// Creates a shader compiler.
        /// </summary>
        /// <returns>The compiler.</returns>
        IShaderCompiler CreateShaderCompiler();

        #endregion

        #region View Creation

        /// <summary>
        /// Creates a constants buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>The constant buffer view of buffer.</returns>
        ICBufferView CreateCBufferView(IBuffer buffer);

        /// <summary>
        /// Create a vertex buffer view.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="stride">Element stride of buffer.</param>
        /// <param name="offset">Offset from the beginning og buffer in bytes.</param>
        /// <returns></returns>
        IVBufferView CreateVBufferView(IBuffer buffer, uint stride, ulong offset);

        /// <summary>
        /// Creates an index buffer view.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="wide">Is the buffer format wide.</param>
        /// <param name="offset">Offset from the beginning of buffer in bytes.</param>
        /// <returns></returns>
        IIBufferView CreateIBufferView(IBuffer buffer, bool wide, ulong offset);

        /// <summary>
        /// Creates a render target view.
        /// </summary>
        /// <param name="resource">The resource, either IBuffer or ITexture.</param>
        /// <param name="usageType">The usage type.</param>
        /// <param name="param1">First parameter - depends on usage.</param>
        /// <param name="param2">Second parameter - depends on usage.</param>
        /// <param name="param3">Third parameter - depends on usage.</param>
        /// <returns>The render target view.</returns>
        IRenderTargetView CreateRenderTargetView(object resource, UsageDimensionType usageType,
                    CommonPixelFormatLayout layout, ulong param1, ulong param2, ulong param3);

        /// <summary>
        /// Creates a depth-stencil target view.
        /// </summary>
        /// <param name="resource">The texture.</param>
        /// <param name="usageType">The usage type.</param>
        /// <param name="param1">First parameter - depends on usage.</param>
        /// <param name="param2">Second parameter - depends on usage.</param>
        /// <param name="param3">Third parameter - depends on usage.</param>
        /// <returns>The render target view.</returns>
        IDepthStencilTargetView CreateDepthStencilTargetView(ITexture texture, UsageDimensionType usageType,
                    CommonPixelFormatLayout layout, ulong param1, ulong param2, ulong param3);

        /// <summary>
        /// Creates a texture view.
        /// </summary>
        /// <param name="resource">The resource, either IBuffer or ITexture.</param>
        /// <param name="usageType">The usage type.</param>
        /// <param name="param1">First parameter - depends on usage.</param>
        /// <param name="param2">Second parameter - depends on usage.</param>
        /// <param name="param3">Third parameter - depends on usage.</param>
        /// <returns>The texture view.</returns>
        ITextureView CreateTextureView(object resource, UsageDimensionType usageType,
                     CommonPixelFormatLayout layout, ulong param1, ulong param2, ulong param3);

        #endregion

        #region Thread Safety

        /// <summary>
        /// Enters this instance.
        /// </summary>
        void Enter();

        /// <summary>
        /// Exits this instance.
        /// </summary>
        void Exit();

        #endregion

        #region Bindings

        /// <summary>
        /// Binds a vertex stage variables.
        /// </summary>
        void BindVStage(Topology topology, IVerticesBindingLayout layout, IVBufferView[] vbuffers, 
                        IIBufferView ibuffer, IVShader vshader, ISamplerState[] samplers,
                        ITextureView[] textures, ICBufferView[] constants);

        /// <summary>
        /// Binds a geometry stage variables.
        /// </summary>
        void BindGStage(IGShader pshader, ISamplerState[] samplers, ITextureView[] textures,
                        ICBufferView[] constants, IVerticesOutBindingLayout layout, IVBufferView[] vbuffers);

        /// <summary>
        /// Binds a pixel stage variables.
        /// </summary>
        void BindPStage(IPShader pshader, ISamplerState[] samplers, ITextureView[] textures,
                        ICBufferView[] constants, IRenderTargetView[] renderTargets, 
                        IDepthStencilTargetView depthTarget);


        /// <summary>
        /// Sets the viewports.
        /// </summary>
        /// <param name="viewports">The viewports.</param>
        void SetViewports(Region2i[] viewports);

        /// <summary>
        /// Sets the scissor rects.
        /// </summary>
        /// <param name="rects">The rects.</param>
        void SetScissorRects(Region2i[] rects);

        /// <summary>
        /// Sets blend state.
        /// </summary>
        void SetBlendState(IBlendState state, Colour colour, uint mask);

        /// <summary>
        /// Sets depth stencil state.
        /// </summary>
        void SetDepthStencilState(IDepthStencilState state, uint stencilRef);

        /// <summary>
        /// Sets asterization state.
        /// </summary>
        void SetRasterizationState(IRasterizationState state);
        #endregion

        #region Sharing Context

        /// <summary>
        /// Obtains shared texture.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        SharedTextureInfo GetShared(Guid guid);

        /// <summary>
        /// Registers shared texture.
        /// </summary>
        void RegisterShared(Guid guid, SharedTextureInfo info);

        /// <summary>
        /// Unregisters shared texture.
        /// </summary>
        void UnregisterShared(Guid guid);

        #endregion

        #region Rendering

        /// <summary>
        /// Clears the render target.
        /// </summary>
        void Clear(IRenderTargetView view, Colour colour);

        /// <summary>
        /// Clears depth/stencil target.
        /// </summary>
        void Clear(IDepthStencilTargetView view, ClearOptions options, float depth, uint stencil); 

        /// <summary>
        /// Draws entire buffer.
        /// </summary>
        void DrawAuto();

        /// <summary>
        /// Draws from offset to offset+length.
        /// </summary>
        void Draw(ulong off, ulong lenght);

        /// <summary>
        /// Draws indexed.
        /// </summary>
        void DrawIndexed(ulong off, ulong length, long baseIndex);

        /// <summary>
        /// Draw instanced.
        /// </summary>
        void Draw(ulong offset, ulong count,
                  uint instanceOffset, uint instanceCount);

        /// <summary>
        /// Draw instanced and indexed.
        /// </summary>
        void DrawIndexed(ulong offset, ulong count, long baseIndex,
                  uint instanceOffset, uint instanceCount);

        #endregion

    }
}
