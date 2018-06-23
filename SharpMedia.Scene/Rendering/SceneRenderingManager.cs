using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Scene.Rendering
{

    /// <summary>
    /// A scene rendering manager.
    /// </summary>
    public class SceneRenderingManager : IManager
    {
        #region IManager Members

        public void Invoke(TimeSpan deltaTime)
        {
            // We first check for cameras in scene graph 
            // query.GetSceneNodes(typeof(Camera), delegate(SceneNode obj) 
            // { return obj.GetComponent(typeof(Camera)).IsEnabled == true; }
            // Then we render scene from each camera

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
