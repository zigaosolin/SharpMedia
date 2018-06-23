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

namespace SharpMedia.Graphics.GPGPU.Math.Implementation
{
    /// <summary>
    /// A vector product algorithm.
    /// </summary>
    internal sealed class VectorDotProduct : IAlgorithm
    {
        #region IPass Members

        public IData[] Execute(GraphicsDevice device, IData[] outputs, params IData[] inputs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ICloneable<IPass> Members

        public IPass Clone()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
