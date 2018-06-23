using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Scene.Rendering
{

    /// <summary>
    /// Shader supplied variable.
    /// </summary>
    [SceneComponent]
    public sealed class ShaderSuppliedVariable : ISCNamed
    {
        #region Private Members
        string name = string.Empty;
        #endregion

        #region ISCNamed Members

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
