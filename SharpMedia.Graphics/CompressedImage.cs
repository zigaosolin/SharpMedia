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

namespace SharpMedia.Graphics
{
    /// <summary>
    /// Compressed image exposes it's components as uncompressed image. This
    /// class can be used to check if image uses lossy methods to store image.
    /// Such formats are not prefered for high quality effects.
    /// </summary>
    public abstract class CompressedImage : Image
    {
        /// <summary>
        /// Is the texture compression lossy.
        /// </summary>
        public abstract bool IsLossy
        {
            get;
        }

        /// <summary>
        /// The compression ratio of this image compared to
        /// raw image.
        /// </summary>
        public abstract double CompressionRatio
        {
            get;
        }

        /// <summary>
        /// The amount of precision loss, in percantage, of this format
        /// related to this image.
        /// </summary>
        public abstract double PrecissionLoss
        {
            get;
        }
    }
}
