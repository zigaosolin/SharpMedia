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

namespace SharpMedia.Graphics.GPGPU.Math
{
    /// <summary>
    /// A dense vector, represented as 1D texture.
    /// </summary>
    public sealed class DenseVector : IData
    {
        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IData Members

        public RenderTargetView AsOutput
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public TextureView AsInput
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
