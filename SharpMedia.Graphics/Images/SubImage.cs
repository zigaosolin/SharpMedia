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
using System.Threading;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Images
{
    /// <summary>
    /// A sub-image, sometimes useful if you want to apply operations only to
    /// the portion of the image. Subimage effects original image.
    /// </summary>
    public class SubImage : Image
    {
        #region Private Members
        Image internalImage;
        uint offsetX;
        uint width;
        uint offsetY;
        uint height;
        uint offsetZ;
        uint depth;
        uint offsetFace;
        uint faceCount;
        uint offsetMipmap;
        uint mipmapCount;

        // Locking data.
        Mipmap internalMipmapLocked;
        Mipmap lockedMipmap;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubImage"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="width">The width.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <param name="height">The height.</param>
        /// <param name="offsetZ">The offset Z.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="offsetMipmap">The offset mipmap.</param>
        /// <param name="mipmapCount">The mipmap count.</param>
        /// <param name="offsetFace">The offset face.</param>
        /// <param name="faceCount">The face count.</param>
        internal SubImage([NotNull] Image parent,uint offsetX, uint width,
                          uint offsetY, uint height,
                          uint offsetZ, uint depth,
                          uint offsetMipmap, uint mipmapCount,
                          uint offsetFace, uint faceCount)
        {
            // Make sure we set the zerod values to correct ones.
            if (width == 0) width = parent.Width;
            if (height == 0) height = parent.Height;
            if (depth == 0) depth = parent.Depth;
            if (mipmapCount == 0) mipmapCount = parent.MipmapCount;
            if (faceCount == 0) faceCount = parent.FaceCount;

            // Image check.
            if ((offsetX + width > parent.Width) ||
                (offsetY + height > parent.Height) ||
                (offsetZ + depth > parent.Depth) ||
                (offsetMipmap + mipmapCount > parent.MipmapCount) ||
                (offsetFace + faceCount > parent.FaceCount))
            {
                throw new ArgumentException("Sub-image is created with access out of parent image bounds.");
            }

            this.internalImage = parent;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.offsetZ = offsetZ;
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.offsetMipmap = offsetMipmap;
            this.mipmapCount = mipmapCount;
            this.offsetFace = offsetFace;
            this.faceCount = faceCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent image.
        /// </summary>
        /// <value>The parent image.</value>
        public Image ParentImage
        {
            get
            {
                return internalImage;
            }
        }

        #endregion

        #region Public Members

        public override PixelFormat Format
        {
            get { return internalImage.Format; }
        }

        public override uint MipmapCount
        {
            get { return mipmapCount; }
        }

        public override void GenerateMipmaps(BuildImageFilter filter)
        {
            // This will create mipmaps for entrire image.
            internalImage.GenerateMipmaps(filter);
        }

        public override uint Width
        {
            get { return width; }
        }

        public override uint Height
        {
            get { return height; }
        }

        public override Mipmap Map(MapOptions op, uint mipmap, uint face, bool uncompress)
        {
            if(face + offsetFace >= faceCount) throw new IndexOutOfRangeException("Face index is out of image's range.");
            Monitor.Enter(syncRoot);
            try
            {
                // We lock and extract subregion.
                internalMipmapLocked = internalImage.Map(op, mipmap, face, uncompress);

                // We check if reading is required.
                if (op != MapOptions.Write)
                {
                    lockedMipmap = new Mipmap(width, height, depth, face, mipmap, null, this);
                }
                else
                {
                    
                }
                throw new NotImplementedException();
            }
            catch (Exception)
            {
                Monitor.Exit(syncRoot);

                // Rethrow.
                throw;
            }
        }

        public override void UnMap(uint mipmap)
        {
            // Copy data to internal image.

            // Unlock the internal image.
            internalImage.UnMap(mipmap);
            internalMipmapLocked.Dispose();
            Monitor.Exit(syncRoot);
        }

        public override Image CloneSameType(PixelFormat fmt, uint width, uint height, uint depth, uint faces, uint mipmaps)
        {
            return internalImage.CloneSameType(fmt, width, height, depth, faces, mipmaps);
        }

        public override SharpMedia.Resources.ResourceAddress Address
        {
            get { return internalImage.Address; }
        }

        #endregion
    }
}
