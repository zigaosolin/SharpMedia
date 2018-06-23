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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.GPGPU
{
    /// <summary>
    /// An GPGPU pass.
    /// </summary>
    public interface IPass : IDisposable, ICloneable<IPass>
    {
        /// <summary>
        /// Executes the pass.
        /// </summary>
        /// <remarks>If outputs is null, they are automatically created by pass.</remarks>
        IData[] Execute([NotNull] GraphicsDevice device, IData[] outputs, [NotNull] params IData[] inputs); 
    }
}
