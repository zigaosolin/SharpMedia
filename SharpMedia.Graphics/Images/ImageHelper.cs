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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Images
{



    /// <summary>
    /// Internal image helper, to help with some image constructions.
    /// </summary>
    internal static class ImageHelper
    {
        /// <summary>
        /// Minimum number that will still imply conversion code.
        /// </summary>
        const ulong EmitConversionCodeThreeshold = 64 * 64;

        /// <summary>
        /// Is the dimension power of two.
        /// </summary>
        /// <param name="size">The size of image.</param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(uint size)
        {
            return size > 0 && (size & (size - 1)) == 0;
        }


        /// <summary>
        /// Converts the images. This is one of the most powerful functions that
        /// also works preaty fast because it will generate per-element conversion
        /// code if a lot of pixels need to be converted.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void ConvertImages([NotNull] Image source, [NotNull] Image destination)
        {

        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void ResizeImage(BuildImageFilter filter, [NotNull] Image source, [NotNull] Image destination)
        {

        }

        /// <summary>
        /// Writes specific component to image.
        /// </summary>
        public unsafe static void WriteComponentToImage(PixelComponentFormat fmt, [NotNull] byte[] data, float toWrite,
                                                 uint offset, ulong count, uint stride)
        {
            fixed (byte* x = data)
            {
                // We are at index.
                byte* p = x + offset;

                switch (fmt)
                {
                    case PixelComponentFormat.UInt8:
                        for (ulong i = 0; i < count; i++)
                        {    
                            *p = (byte)(Math.MathHelper.Clamp(toWrite * 255.0f, 0.0f, 255.0f));
                            p += stride;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
                
            }
        }

    }
}
