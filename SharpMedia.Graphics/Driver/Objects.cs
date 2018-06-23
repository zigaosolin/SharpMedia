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

namespace SharpMedia.Graphics.Driver
{

    /// <summary>
    /// The implementation of buffer.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IBuffer : IDisposable
    {
        /// <summary>
        /// Reads at the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        byte[] Read(ulong offset, ulong count);

        /// <summary>
        /// Writes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        void Update(byte[] data, ulong offset, ulong count);
    }

    /// <summary>
    /// A texture implementation is required by driver.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface ITexture : IDisposable
    {
        /// <summary>
        /// Reads at the specified offset.
        /// </summary>
        /// <param name="mipmap">The mipmap</param>
        /// <returns></returns>
        byte[] Read(uint mipmap, uint face);

        /// <summary>
        /// Writes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        void Update(byte[] data, uint mipmap, uint face);
    }

    /// <summary>
    /// A one dimensional texture.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface ITexture1D : ITexture
    {
    }

    /// <summary>
    /// A one dimensional texture array.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface ITexture1DArray : ITexture
    {
    }

    /// <summary>
    /// A two dimensional texture.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface ITexture2D : ITexture
    {
    }

    /// <summary>
    /// A two dimensional array of textures.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface ITexture2DArray : ITexture
    {
    }

    /// <summary>
    /// A 3D texture.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface ITexture3D : ITexture
    {
    }

    /// <summary>
    /// Base class for driver shaders.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IShaderBase : IDisposable
    {
    }

    /// <summary>
    /// A vertex shader interface.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IVShader : IShaderBase
    {
    }

    /// <summary>
    /// A pixel shader interface.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IPShader : IShaderBase
    {
    }

    /// <summary>
    /// A geometry shader, not supported by all.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IGShader : IShaderBase
    {
    }


    /// <summary>
    /// The swap chain; chain of buffers where rendering to window occurs.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface ISwapChain : IRenderTargetView
    {

        /// <summary>
        /// Resets the swap chain.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        void Resize(uint width, uint height);

        /// <summary>
        /// Is it fullscreen or not.
        /// </summary>
        void Reset(uint width, uint height, CommonPixelFormatLayout layout, bool fs);

        /// <summary>
        /// Presents this instance.
        /// </summary>
        void Present();

        /// <summary>
        /// Waits until presented.
        /// </summary>
        void Finish();
    }

    /// <summary>
    /// A window backend.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IWindowBackend
    {
        /// <summary>
        /// Sets the listener object.
        /// </summary>
        /// <param name="backend">The beckend.</param>
        void SetListener(IWindow backend);

        /// <summary>
        /// Processes events.
        /// </summary>
        void DoEvents();

        /// <summary>
        /// A window handle.
        /// </summary>
        IntPtr Handle { get; }
    }

    /// <summary>
    /// Window listener.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IWindow
    {
        void Resized(uint width, uint height);
        void Closed();
        void Minimized(bool minimize);
        void Focused(bool focus);
    }



    /// <summary>
    /// Cached interface that can be supported for geometry.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IVerticesBindingLayout : IDisposable
    {
    }

    /// <summary>
    /// Cached interface that can be supports geometry output stage.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IVerticesOutBindingLayout : IDisposable
    {
    }
}
