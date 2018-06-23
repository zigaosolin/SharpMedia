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
using SharpMedia.Graphics.GUI.Metrics;
using System.Reflection;

namespace SharpMedia.Graphics.GUI.Animations
{
    /// <summary>
    /// A GuiScalar animation.
    /// </summary>
    public abstract class GuiScalarAnimation : IAnimation
    {
        #region Subclasses

        /// <summary>
        /// A linear interpolation.
        /// </summary>
        public class LinearInterpolation : GuiScalarAnimation
        {
            #region Private Members
            GuiScalar from;
            GuiScalar to;
            float time;

            protected override GuiScalar GetScalar(float animationTime)
            {
                if(animationTime > time) return to;

                float t = animationTime / time;


                return new GuiScalar((1.0f - t) * from.PixelCoordinate + t * to.PixelCoordinate,
                                  (1.0f - t) * from.CanvasCoordinate + t * to.CanvasCoordinate,
                                  (1.0f - t) * from.ParentCoordinate + t * to.ParentCoordinate);
            }
            #endregion

            #region Public Members

            public LinearInterpolation(PropertyInfo info, GuiScalar from, GuiScalar to, float time)
                : base(info)
            {
                this.from = from;
                this.to = to;
                this.time = time;
            }

            public override float AnimationTime
            {
                get { return time; }
            }

            #endregion

            
        }

        /// <summary>
        /// A looping wrapper.
        /// </summary>
        public class Looper : GuiScalarAnimation
        {
            #region Private Members
            uint loopCount = uint.MaxValue;
            GuiScalarAnimation internalAnimation;
            

            protected override GuiScalar GetScalar(float animationTime)
            {
                // Make sure we end at the correct place.
                if (animationTime > AnimationTime)
                {
                    return internalAnimation.GetScalar(internalAnimation.AnimationTime);
                }

                // We wrap the time.
                float remainder =
                    (float)global::System.Math.IEEERemainder(animationTime, internalAnimation.AnimationTime);

                return internalAnimation.GetScalar(remainder);
            }
            #endregion

            #region Public Members

            public override float AnimationTime
            {
                get { return internalAnimation.AnimationTime * (float)loopCount; }
            }

            /// <summary>
            /// The loop constructor.
            /// </summary>
            /// <param name="internalAnimation"></param>
            public Looper(GuiScalarAnimation internalAnimation)
                : base(internalAnimation.property)
            {
                this.internalAnimation = internalAnimation;
            }

            /// <summary>
            /// The loop constructor.
            /// </summary>
            /// <param name="internalAnimation"></param>
            /// <param name="loopCount"></param>
            public Looper(GuiScalarAnimation internalAnimation, uint loopCount)
                : base(internalAnimation.property)
            {
                this.internalAnimation = internalAnimation;
                this.loopCount = loopCount;
            }

            #endregion
        }

        /// <summary>
        /// A looper that goes to end and then starts at the end.
        /// </summary>
        public class LoopperInverse : GuiScalarAnimation
        {
            #region Private Members
            uint loopCount = uint.MaxValue;
            GuiScalarAnimation internalAnimation;
            

            protected override GuiScalar GetScalar(float animationTime)
            {
                // Make sure we end at the correct place.
                if (animationTime > AnimationTime)
                {
                    return internalAnimation.GetScalar(internalAnimation.AnimationTime);
                }

                // We wrap the time.
                float remainder =
                    (float)global::System.Math.IEEERemainder(animationTime, internalAnimation.AnimationTime);

                return internalAnimation.GetScalar(remainder);
            }
            #endregion

            #region Public Members

            public override float AnimationTime
            {
                get { return internalAnimation.AnimationTime * (float)loopCount; }
            }

            /// <summary>
            /// The loop constructor.
            /// </summary>
            /// <param name="internalAnimation"></param>
            public LoopperInverse(GuiScalarAnimation internalAnimation)
                : base(internalAnimation.property)
            {
                this.internalAnimation = internalAnimation;
            }

            /// <summary>
            /// The loop constructor.
            /// </summary>
            /// <param name="internalAnimation"></param>
            /// <param name="loopCount"></param>
            public LoopperInverse(GuiScalarAnimation internalAnimation, uint loopCount)
                : base(internalAnimation.property)
            {
                this.internalAnimation = internalAnimation;
                this.loopCount = loopCount;
            }

            #endregion
        }

        // TODO: add others, such as function animation, smooth step and more.

        #endregion

        #region Private Members
        PropertyInfo property;

        protected GuiScalarAnimation(PropertyInfo info)
        {
            this.property = info;
        }
        #endregion

        #region Abstract

        protected abstract GuiScalar GetScalar(float animationTime);

        #endregion

        #region IAnimation Members

        public void Update(float animationTime, IAnimatable animatable)
        {
            property.SetValue(animatable, GetScalar(animationTime), null);
        }

        public abstract float AnimationTime
        {
            get;
        }

        #endregion
    }

    
}
