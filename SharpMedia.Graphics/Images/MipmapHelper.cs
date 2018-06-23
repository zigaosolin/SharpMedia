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

namespace SharpMedia.Graphics.Images
{

    /// <summary>
    /// Class can generate mipmaps in software or hardware.
    /// </summary>
    public static class MipmapHelper
    {

        /// <summary>
        /// Generates all mipmaps from the base image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="filter">The filter.</param>
        public static void GenerateMipmaps(Image image, BuildImageFilter filter)
        {

        }

        /// <summary>
        /// Computs the count of mipmaps needed until 1x1 mipmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>Number of mipmaps.</returns>
        public static uint MipmapCount(uint width, uint height)
        {
            for (uint i = 2; ; i++)
            {
                width = width != 1 ? width >> 1 : 1;
                height = height != 1 ? height >> 1 : 1;

                if (width == 1 && height == 1) return i;
            }
        }

        /// <summary>
        /// Computes the dimensions.
        /// </summary>
        /// <param name="width">The weight.</param>
        /// <param name="height">The height.</param>
        /// <param name="index">The index.</param>
        /// <param name="newWidth">The new width.</param>
        /// <param name="newHeight">The new height.</param>
        public static void ComputeDimensions(uint width, uint height,
                uint index, out uint newWidth, out uint newHeight)
        {
            newWidth = width;
            newHeight = height;
            for (uint i = 0; i < index; i++)
            {
                newWidth = newWidth != 1 ? newWidth >> 1 : 1;
                newHeight = newHeight != 1 ? newHeight >> 1 : 1;
            }
        }

    }
}
