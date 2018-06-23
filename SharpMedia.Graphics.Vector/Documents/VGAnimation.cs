using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SharpMedia.Graphics.Vector.Documents
{
    /// <summary>
    /// Animation repeat mode.
    /// </summary>
    public enum VGRepeatMode
    {
        /// <summary>
        /// When it comes to end, it goes from end to beginning. 
        /// </summary>
        Wrap,

        /// <summary>
        /// The animation is repeated repeat count times.
        /// </summary>
        Repeat
    }

    /// <summary>
    /// An animation object.
    /// </summary>
    public abstract class VGAnimation : VGObject
    {
        #region Private Members
        // Non-resolved references.
        string referenceName;
        string referenceProperty;

        // Resolved references.
        object referenceNameResolved;
        PropertyInfo referencePropertyResolved;

        // Number of repeats.
        float duration = 1.0f;
        uint repeatCount = 1;
        VGRepeatMode repeatMode = VGRepeatMode.Repeat;

        // State data.
        float timeState = 0.0f;
        uint repeatState = 0;

        #endregion

        #region Constructors


        public VGAnimation(string referenceName, string referenceProperty,
            float duration, VGRepeatMode repeatMode, uint repeatCount)
        {

        }

        #endregion

    }
}
