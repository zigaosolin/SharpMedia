using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Scene.Query
{

    /// <summary>
    /// A scene query manager (manages query data).
    /// </summary>
    public class SceneQueryManager : IManager
    {
        #region IManager Members

        public void Invoke(TimeSpan deltaTime)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
