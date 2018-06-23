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
    /// A constants buffer view.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface ICBufferView : IDisposable
    {
    }

    /// <summary>
    /// An index buffer view.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IIBufferView : IDisposable
    {
    }

    /// <summary>
    /// A vertex buffer view.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IVBufferView : IDisposable
    {

    }

    /// <summary>
    /// A render target category.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IRenderTargetView : IDisposable
    {
    }

    /// <summary>
    /// A shader resource view.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface ITextureView : IDisposable
    {
    }

    /// <summary>
    /// A depth stencil resource view.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IDepthStencilTargetView : IDisposable
    {
    }
}
