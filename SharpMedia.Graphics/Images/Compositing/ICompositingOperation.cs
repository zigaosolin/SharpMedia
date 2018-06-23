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

namespace SharpMedia.Graphics.Images.Compositing
{

    public enum OperationType
    {
        NoSource,
        OneSource,
        TwoSources
    }

    /// <summary>
    /// An operation.
    /// </summary>
    public interface ICompositingOperation
    {
        /// <summary>
        /// The operation type.
        /// </summary>
        OperationType OperationType { get; }

        /// <summary>
        /// The source 1 of operation.
        /// </summary>
        ICompositingOperation Source1 { get; set; }

        /// <summary>
        /// The source 2 of operation.
        /// </summary>
        ICompositingOperation Source2 { get; set; }

        /// <summary>
        /// The shader operation interface.
        /// </summary>
        ICompositeInterface Interface { get; }

        /// <summary>
        /// Size of target.
        /// </summary>
        Vector2i Size { get; }
    }

    /// <summary>
    /// A terminal operation.
    /// </summary>
    public interface ITerminalCompositingOperation : ICompositingOperation
    {
        /// <summary>
        /// Composites to certain view.
        /// </summary>
        void CompositeTo(CompositingResources resources, RenderTargetView view);

    }
}
