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
using SharpMedia.Math.Interpolation;
using SharpMedia.AspectOriented;
using SharpMedia.Math;

namespace SharpMedia.Graphics.GUI.Styles
{
    /// <summary>
    /// An style animation controller. Gives you full controll over widget's style animation,
    /// </summary>
    /// <remarks>Theme usually "interpolates" between states correctly.</remarks>
    public sealed class StyleAnimationController
    {
        #region Internal Classes

        class StyleAnimationBucket
        {
            public StyleState TransistFrom;
            public StyleState TransistTo;
            public IUniformStepper Stepper;
            public float TransitionTime;
        }

        #endregion

        #region Private Members
        
        // State data.
        StyleState currentState = CommonStyleStates.Normal;
        StyleState nextState = null;
        float time = 0.0f;
        float timeToTransist = 1.0f;
        IUniformStepper stepper = smoothStepper;

        // Style animation buckets.
        SortedList<StyleState, List<StyleAnimationBucket>> defaultAnimations =
            new SortedList<StyleState, List<StyleAnimationBucket>>();

        // Static members
        const float globalDefaultTime = 0.5f;
        static IUniformStepper smoothStepper = new SmoothStep();
        #endregion

        #region Public Members

        /// <summary>
        /// Creates a style in normal state.
        /// </summary>
        public StyleAnimationController()
        {
        }

        /// <summary>
        /// The current state.
        /// </summary>
        public StyleState CurrentState
        {
            get
            {
                return currentState;
            }
        }

        /// <summary>
        /// Next state, may be CommonStyleState.Null.
        /// </summary>
        public StyleState NextState
        {
            get
            {
                return nextState;
            }
        }

        /// <summary>
        /// Weight, 0.0f means current state, 1.0f means next state.
        /// </summary>
        public float Weight
        {
            get
            {
                return stepper.Interpolate(time);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is next state to transist.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool IsOrTransistingTo(StyleState state)
        {
            if (nextState == null)
            {
                return currentState == state;
            }
            else
            {
                return nextState == state;
            }
        }

        /// <summary>
        /// Sets or replaces default transition.
        /// </summary>
        /// <param name="from">May be null to specify all</param>
        /// <param name="to">May be null to specify all.</param>
        /// <param name="defaultTime">The default time.</param>
        /// <param name="stepper">Stepper used.</param>
        public void SetDefaultTransition(StyleState from, StyleState to, 
            float transitionTime, IUniformStepper stepper)
        {
            if (from == null) from = CommonStyleStates.All;
            if (to == null) to = CommonStyleStates.All;

            lock (defaultAnimations)
            {
                // We first retrieve it.
                bool wasFound = true;
                List<StyleAnimationBucket> bucketList;
                if (!defaultAnimations.TryGetValue(from, out bucketList))
                {
                    wasFound = false;
                    bucketList = new List<StyleAnimationBucket>();
                }

                // We may need to overwrite.
                StyleAnimationBucket anim = bucketList.Find(delegate(StyleAnimationBucket b)
                {
                    if (b.TransistTo == to) return true;
                    return false;
                });

                if (anim == null)
                {
                    anim = new StyleAnimationBucket();
                    anim.Stepper = stepper;
                    anim.TransistFrom = from;
                    anim.TransistTo = to;
                    anim.TransitionTime = transitionTime;

                    bucketList.Add(anim);
                }
                else
                {
                    anim = new StyleAnimationBucket();
                    anim.Stepper = stepper;
                    anim.TransistFrom = from;
                    anim.TransistTo = to;
                    anim.TransitionTime = transitionTime;
                }

                // We finally add it.
                if (!wasFound) defaultAnimations.Add(from, bucketList);

            }
        }

        /// <summary>
        /// Obtains the default animation time and stepper for some transition.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="defaultTime"></param>
        /// <param name="stepper"></param>
        public void GetDefaultTransition(StyleState from, StyleState to,
            out float defaultTime, out IUniformStepper stepper)
        {
            if (from == null) from = CommonStyleStates.Null;
            if (to == null) to = CommonStyleStates.Null;

            lock (defaultAnimations)
            {
                List<StyleAnimationBucket> bucketList;
                if (!defaultAnimations.TryGetValue(from, out bucketList))
                {
                    // We need a all-* mapper.
                    if (!defaultAnimations.TryGetValue(CommonStyleStates.All, out bucketList))
                    {
                        defaultTime = globalDefaultTime;
                        stepper = smoothStepper;
                        return;
                    }
                }

                // We now search for direct match first.
                for (int i = 0; i < bucketList.Count; i++)
                {
                    if (bucketList[i].TransistTo == to)
                    {
                        defaultTime = bucketList[i].TransitionTime;
                        stepper = bucketList[i].Stepper;
                        return;
                    }
                }

                // We try for -all match.
                for (int i = 0; i < bucketList.Count; i++)
                {
                    if (bucketList[i].TransistTo == CommonStyleStates.All)
                    {
                        defaultTime = bucketList[i].TransitionTime;
                        stepper = bucketList[i].Stepper;
                        return;
                    }
                }

                // Otherwise, use default.
                defaultTime = globalDefaultTime;
                stepper = smoothStepper;
            }
        }

        /// <summary>
        /// Transist that uses default bindings.
        /// </summary>
        /// <param name="next"></param>
        public void TransistTo(StyleState next)
        {
            float timeToTransist;
            IUniformStepper stepper;

            GetDefaultTransition(currentState, next, out timeToTransist, out stepper);

            TransistTo(next, timeToTransist, stepper);
        }

        /// <summary>
        /// Transists to next state, with time to transist and stepper specified.
        /// </summary>
        public void TransistTo(StyleState next, [MinFloat(0.0f)] float timeToTransist, IUniformStepper stepper)
        {
            // We default.
            if (stepper == null)
            {
                stepper = smoothStepper;
            }

            // Immediate transition.
            if (MathHelper.NearEqual(timeToTransist, 0.0f))
            {
                currentState = nextState;
                nextState = null;
                return;
            }

            // We set current.
            this.timeToTransist = timeToTransist;
            this.stepper = stepper;

            if (nextState == null)
            {
                // Simple transition
                time = 0.0f;
                nextState = next;
            }
            else
            {
                // Already transisting.
                if (next == nextState) return;

                // We try to do cool mechanism if back-transition is done.
                if (next == CurrentState)
                {
                    currentState = nextState;
                    nextState = next;
                    time = 1.0f - time;
                }
                else
                {
                    // We "skip" the animation to next,
                    currentState = nextState;
                    nextState = next;
                    time = 0.0f;
                }
            }
        }

        /// <summary>
        /// Animates the state.
        /// </summary>
        /// <remarks>Automatically called by widget, do not call this manually.</remarks>
        /// <param name="deltaWeight"></param>
        public void Animate(float deltaTime)
        {
            if (NextState == null) return;

            // We do the interpolation.
            this.time += deltaTime / this.timeToTransist;
            if (time >= 1.0f)
            {
                currentState = nextState;
                nextState = null;
                time = 0.0f;
            }
        }

        #endregion
    }


}
