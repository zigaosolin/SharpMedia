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
using SharpMedia.Graphics;
using SharpMedia.Math.Shapes;
using SharpMedia.Math.Volumes;
using System.Reflection;

namespace SharpMedia.Scene
{

    /// <summary>
    /// A manager component.
    /// </summary>
    public interface IManager : IDisposable
    {

        /// <summary>
        /// Invokes a scene manager, as specified by rules.
        /// </summary>
        /// <param name="deltaTime">The delta time from previous invoke.</param>
        void Invoke(TimeSpan deltaTime);

    }

    

    /// <summary>
    /// The Scene Manager.
    /// </summary>
    /// <remarks>Scene manager manages all other scene managers.</remarks>
    public sealed class SceneManager
    {
        #region Rules

        public abstract class Rule
        {

        }

        #endregion


        #region Private Members
        List<IManager> managers = new List<IManager>();
        #endregion

        #region Public Members

        public void AddManager(IManager manager, Rule rule)
        {

        }

        #endregion

    }

}