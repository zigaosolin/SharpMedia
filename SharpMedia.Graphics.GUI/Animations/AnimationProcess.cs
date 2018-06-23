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

namespace SharpMedia.Graphics.GUI.Animations
{
    /// <summary>
    /// Animation process is time-animation pair that together defines animation and its state.
    /// </summary>
    public sealed class AnimationProcess
    {
        #region Private Members
        float time = 0.0f;
        IAnimation animation;
        #endregion

        #region Public Members

        /// <summary>
        /// Creates an inital time animation.
        /// </summary>
        /// <param name="animation"></param>
        public AnimationProcess(IAnimation animation)
        {
            this.animation = animation;
        }

        /// <summary>
        /// Creates animation with startup time.
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="startTime"></param>
        public AnimationProcess(IAnimation animation, float startTime)
        {
            this.animation = animation;
            this.time = startTime;
        }

        public bool IsOver
        {
            get
            {
                return time > animation.AnimationTime;
            }
        }

        /// <summary>
        /// We update animation.
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="animatable"></param>
        public void Update(float deltaTime, IAnimatable animatable)
        {
            time += deltaTime;

            animation.Update(time, animatable);
        }

        /// <summary>
        /// The animation.
        /// </summary>
        public IAnimation Animation
        {
            get
            {
                return animation;
            }
        }

        /// <summary>
        /// Time of animation.
        /// </summary>
        public float Time
        {
            get
            {
                return time;
            }
        }

        #endregion

    }
}
