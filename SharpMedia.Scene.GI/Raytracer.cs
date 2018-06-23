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

namespace SharpMedia.Scene.GI
{
    /// <summary>
    /// A raytracer is a class that can trace rays in scene. Raytracing relies on
    /// scene to provide fast transversal (using any spatial algorithm) and on element
    /// provider to provide raytracer with elements.
    /// </summary>
    public class Raytracer
    {
        #region Private Members
        SceneManager sceneManager;
        #endregion


        #region Public Members

        public Element RayTrace(Vector3f origin, Vector3f direction, float maxDistance)
        {
            return null;
        }

        #endregion

    }
}
