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

namespace SharpMedia.Graphics.GUI
{
    /// <summary>
    /// An animatable element.
    /// </summary>
    public interface IAnimatable
    {
        /// <summary>
        /// Updates animation.
        /// </summary>
        /// <param name="deltaTime"></param>
        void Update(float deltaTime);

        /// <summary>
        /// Adds an animation to animatable.
        /// </summary>
        /// <remarks>Animation is automatically discarded when over.</remarks>
        /// <param name="animation"></param>
        void AddAnimation(Animations.IAnimation animation);

        /// <summary>
        /// Manually removes animation from animatable.
        /// </summary>
        /// <param name="animation"></param>
        void RemoveAnimation(Animations.IAnimation animation);
    }
}
