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
    /// An animation specifier.
    /// </summary>
    /// <remarks>Theme usually "interpolates" between states correctly.</remarks>
    public sealed class AnimationState
    {
        #region Public Members

        /// <summary>
        /// The current state.
        /// </summary>
        public string CurrentState = AnimationState.Normal;

        /// <summary>
        /// Next state, may be null if no animation.
        /// </summary>
        public string NextState = null;

        /// <summary>
        /// Weight, 0.0f means current state, 1.0f means next state.
        /// </summary>
        public float Weight = 0.0f;

        #endregion

        #region Methods

        public void TransistTo(string next)
        {
            if (NextState == null)
            {
                // Simple transition
                Weight = 0.0f;
                NextState = next;
            }
            else
            {
                // We try to do cool mechanism if back-transition is done.
                if (next == CurrentState)
                {
                    CurrentState = NextState;
                    NextState = next;
                    Weight = 1.0f - Weight;
                }
                else
                {
                    // We "skip" the animation to next,
                    CurrentState = NextState;
                    NextState = next;
                    Weight = 0.0f;
                }
            }
        }

        /// <summary>
        /// Animates the state.
        /// </summary>
        /// <param name="deltaWeight"></param>
        public void Animate(float deltaWeight)
        {
            if (NextState == null) return;

            // We do the interpolation.
            Weight += deltaWeight;
            if (Weight >= 1.0f)
            {
                CurrentState = NextState;
                NextState = null;
                Weight = 0.0f;
            }
        }

        #endregion

        #region Static Members

        // Common state styles.
        public const string Normal = "Normal";
        public const string MouseOver = "MouseOver";
        public const string MouseDown = "MouseDown";

        // Common animation states.
        public static AnimationState NormalState
        {
            get
            {
                AnimationState state = new AnimationState();
                state.CurrentState = Normal;
                state.Weight = 0.0f;
                return state;
            }
        }

        #endregion

    }
}
