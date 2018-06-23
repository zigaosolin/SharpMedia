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
using SharpMedia.Caching;
using SharpMedia.Resources;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// An image class. Image is anything that has PixelFormat and buffer asociated. 
    /// </summary>
    public abstract class Image : Resources.IResource
    {
        #region Protected Members

        /// <summary>
        /// A synhronisation data.
        /// </summary>
        protected object syncRoot = new object();

        /// <summary>
        /// The state of resource.
        /// </summary>
        protected CacheableState cacheableState = CacheableState.Evicted;

        /// <summary>
        /// An on-touch event.
        /// </summary>
        protected Action<ICacheable> onTouch;

        /// <summary>
        /// On written event.
        /// </summary>
        protected Action<IResource> onWritten;

        /// <summary>
        /// Fired when resource is disposed.
        /// </summary>
        protected Action<IResource> onDisposed;

        /// <summary>
        /// Gets the sync root.
        /// </summary>
        /// <value>The sync root.</value>
        protected object SyncRoot
        {
            get { return syncRoot; }
        }

        protected void AssertNotDisposed()
        {
            if (cacheableState == SharpMedia.Caching.CacheableState.Disposed)
            {
                throw new ObjectDisposedException("Image already disposed, cannot use it.");
            }
        }

        protected void FireWritten()
        {
            Action<IResource> w = onWritten;
            if (w != null)
            {
                w(this);
            }
        }

        #endregion

        #region Abstract/Virtual Methods

        /// <summary>
        /// The pixel format of the image.
        /// </summary>
        public abstract PixelFormat Format
        {
            get;
        }

        /// <summary>
        /// Number of mipmaps.
        /// </summary>
        public abstract uint MipmapCount
        {
            get;
        }

        /// <summary>
        /// Generates all mipmaps of this image.
        /// </summary>
        /// <param name="filter">The filter used to generate mipmaps.</param>
        public abstract void GenerateMipmaps(BuildImageFilter filter);

        /// <summary>
        /// Width of image.
        /// </summary>
        public abstract uint Width
        {
            get;
        }

        /// <summary>
        /// Height of image.
        /// </summary>
        public abstract uint Height
        {
            get;
        }

        /// <summary>
        /// Depth of image.
        /// </summary>
        public virtual uint Depth
        {
            get { return 1; }
        }

        /// <summary>
        /// Support for multiface images, such as cubemap.
        /// </summary>
        public virtual uint FaceCount
        {
            get { return 1; }
        }

        /// <summary>
        /// Locks the image.
        /// </summary>
        /// <param name="mipmap">The mipmap 0 is garantied to succeed, while other
        /// mipmap indices may return null.</param>
        /// <param name="face">The face to lock, usually 0.</param>
        /// <param name="uncompress">Should we uncompress the image. This also implies
        /// that if you write to image, it will be recompressed.</param>
        /// <returns>The mipmap image.</returns>
        public abstract Mipmap Map(MapOptions op, uint mipmap, uint face, bool uncompress);


        /// <summary>
        /// Unlocks a previously locked mipmap.
        /// </summary>
        /// <param name="image">The image to unlock.</param>
        public abstract void UnMap(uint mipmap);

        /// <summary>
        /// Creates object of the same type, this is used when cloning. Must not lock.
        /// </summary>
        /// <param name="fmt">The new format of "type-cloned" image.</param>
        /// <param name="width">The width of image.</param>
        /// <param name="height">The height of image, may limit it to 1 (if 1D texture).</param>
        /// <param name="depth">The depth of image, may limit it to 1 (if 1D or 2D texture).</param>
        /// <param name="faces">The number of faces of image, can be 1 if non-face image (not a cube map).</param>
        /// <param name="mipmaps">Number of mipmaps, can be 1 if no mipmaps supported.</param>
        /// <returns>New instance of image.</returns>
        public abstract Image CloneSameType([NotNull] PixelFormat fmt,
                        uint width, uint height, uint depth,
                        uint faces, uint mipmaps);

        #endregion

        #region Quick Access

        /// <summary>
        /// Is the format compressed. The same as Format.IsCompressed.
        /// </summary>
        public bool IsCompressed
        {
            get
            {
                return Format.IsCompressed;
            }
        }

        /// <summary>
        /// The size of the image.
        /// </summary>
        public ulong Size
        {
            get { return Width * Depth * Height * Format.Size; }
        }

        /// <summary>
        /// Where does the image reside. Most images reside in RAM memory,
        /// but textures can reside in either RAM or AGP memory.
        /// </summary>
        public Placement Location
        {
            get { return this.Address.Location; }
        }

        
        /// <summary>
        /// Locks the image's mipmap.
        /// </summary>
        /// <param name="op">Lock operation.</param>
        /// <param name="mipmap">Mipmap index.</param>
        /// <param name="uncompress">Should the image be uncompressed.</param>
        /// <returns>Locked mipmap.</returns>
        public Mipmap Map(MapOptions op, uint mipmap, bool uncompress)
        {
            return Map(op, mipmap, 0, uncompress);
        }

        /// <summary>
        /// Locks the image's mipmap.
        /// </summary>
        /// <param name="op">Lock operation.</param>
        /// <param name="mipmap">Mipmap index.</param>
        /// <returns>Locked mipmap.</returns>
        public Mipmap Map(MapOptions op, uint mipmap)
        {
            return Map(op, mipmap, 0, true);
        }

        /// <summary>
        /// Maps all mipmaps.
        /// </summary>
        /// <param name="op">The options.</param>
        /// <returns>All mipmas locked.</returns>
        public Mipmap[] MapAll(MapOptions op)
        {
            Mipmap[] mipmaps = new Mipmap[MipmapCount];
            for (uint i = 0; i < MipmapCount; i++)
            {
                mipmaps[i] = Map(op, i);
            }
            return mipmaps;
        }

        /// <summary>
        /// Unmaps all mipmaps.
        /// </summary>
        public void UnMapAll()
        {
            for (uint i = 0; i < MipmapCount; i++)
            {
                UnMap(i);
            }
        }


        #endregion

        #region IResource Members

        public abstract ResourceAddress Address
        {
            get;
        }

        public event Action<IResource> Disposed
        {
            add
            {
                lock (syncRoot)
                {
                    onDisposed += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onDisposed -= value;
                }
            }
        }

        public bool IsDisposed
        {
            get { return cacheableState == CacheableState.Disposed; }
        }

        public event Action<IResource> OnWritten
        {
            add
            {
                lock (syncRoot)
                {
                    onWritten += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onWritten -= value;
                }
            }
        }

        #endregion

        #region ICacheable Members

        public virtual void Cached()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                cacheableState = CacheableState.Normal;
            }
        }

        public virtual void Evict()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                cacheableState = CacheableState.Evicted;
            }
        }

        public event Action<ICacheable> OnTouch
        {
            add
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    onTouch += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    onTouch -= value;
                }
            }
        }

        public SharpMedia.Caching.CacheableState State
        {
            get
            {
                return cacheableState;
            }
        }

        public virtual void Touch()
        {
            Action<ICacheable> action = onTouch;
            if (action != null)
            {
                action(this);
            }
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            lock (syncRoot)
            {
                if (cacheableState == CacheableState.Disposed) return;
                cacheableState = CacheableState.Disposed;

            }

            Action<IResource> r = onDisposed;
            if (r != null)
            {
                r(this);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Image histogram calculation.
        /// </summary>
        public Images.ColourHistogram Histogram
        {
            get
            {
                return Images.ColourHistogram.FromImage(this);
            }
        }

        #region SubImage Section

        /// <summary>
        /// Creates the sub image. Sub-image is just a wrapper and affects the image.
        /// </summary>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="width">The width.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <param name="height">The height.</param>
        /// <param name="offsetZ">The offset Z.</param>
        /// <param name="depth">The depth.</param>
        /// <returns>SubImage that is can be manipulated as image.</returns>
        public Images.SubImage CreateSubImage(uint offsetX, uint width,
                                              uint offsetY, uint height,
                                              uint offsetZ, uint depth)
        {
            return CreateSubImage(offsetX, width, offsetY, height, offsetZ, depth,
                                  0, MipmapCount, 0, FaceCount);
        }

        /// <summary>
        /// Creates the sub image. Sub-image is just a wrapper and affects the image.
        /// </summary>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="width">The width.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <param name="height">The height.</param>
        /// <param name="offsetZ">The offset Z.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="offsetMipmap">The offset mipmap.</param>
        /// <param name="mipmapCount">The mipmap count.</param>
        /// <returns>SubImage that is can be manipulated as image.</returns>
        public Images.SubImage CreateSubImage(uint offsetX, uint width,
                                              uint offsetY, uint height,
                                              uint offsetZ, uint depth,
                                              uint offsetMipmap, uint mipmapCount)
        {
            return CreateSubImage(offsetX, width, offsetY, height, offsetZ,
                depth, offsetMipmap, mipmapCount, 0, FaceCount);
        }


        /// <summary>
        /// Creates the sub image. Sub-image is just a wrapper and affects the image.
        /// </summary>
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
        /// <returns>SubImage that is can be manipulated as image.</returns>
        public Images.SubImage CreateSubImage(uint offsetX, uint width,
                                              uint offsetY, uint height,
                                              uint offsetZ, uint depth,
                                              uint offsetMipmap, uint mipmapCount,
                                              uint offsetFace, uint faceCount)
        {
            return new Images.SubImage(this, offsetX, width, offsetY, height, offsetZ,
                depth, offsetMipmap, mipmapCount, offsetFace, faceCount);
        }

        /// <summary>
        /// Creates the sub image.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns>SubImage that is can be manipulated as image.</returns>
        public Images.SubImage CreateSubImage(Math.Region3i region)
        {
            Math.Vector3i b = region.LeftFrontBottom;
            Math.Vector3i w = region.RightBackTop - region.LeftFrontBottom;
            return CreateSubImage((uint)b.X, (uint)w.X, (uint)b.Y, (uint)w.Y, (uint)b.Z, (uint)w.Z);
        }

        /// <summary>
        /// Creates the sub image.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns>SubImage that is can be manipulated as image.</returns>
        public Images.SubImage CreateSubImage(Math.Region2i region)
        {
            Math.Vector2i b = region.LeftBottom;
            Math.Vector2i w = region.RightTop - region.LeftBottom;
            return CreateSubImage((uint)b.X, (uint)w.X, (uint)b.Y, (uint)w.Y, 0, 1);
        }

        #endregion

        /// <summary>
        /// Converts image to format. Generated image is of the same type as this image.
        /// </summary>
        /// <param name="format">The format of new image.</param>
        /// <returns>Converted image.</returns>
        public Image ConvertTo([NotNull] PixelFormat format)
        {
            Image newImage = null;
            lock (syncRoot)
            {
                // Recreate image.
                newImage = this.CloneSameType(format, Width, Height, Depth, FaceCount, MipmapCount);
            }

            // Convert it.
            Images.ImageHelper.ConvertImages(this, newImage);
            return newImage;
        }

        /// <summary>
        /// Resizes image, resulting in new image instance (in-place resizing not generically supported).
        /// </summary>
        /// <param name="filter">Filter used when resizing.</param>
        /// <param name="width">The width of new image.</param>
        /// <param name="height">The height of new image.</param>
        /// <param name="depth">The depth of new image.</param>
        /// <param name="mipmaps">The number of mipmaps, use 0 for full mipmap set.</param>
        /// <returns>Returned image.</returns>
        public Image Resize(BuildImageFilter filter, uint width, uint height, uint depth, uint mipmaps)
        {
            Image newImage = null;
            lock (syncRoot)
            {
                newImage = this.CloneSameType(Format, width, height, depth, FaceCount, mipmaps);
            }

            // Resize it.
            Images.ImageHelper.ResizeImage(filter, this, newImage);
            return newImage;
        }


        /// <summary>
        /// Initializes all face's mipmaps 0 with the image default (zero) data.
        /// </summary>
        public void Initialize()
        {
            for (uint i = 0; i < FaceCount; i++)
            {
                Initialize(i);
            }
        }

        /// <summary>
        /// Initializes the specified face witd default data.
        /// </summary>
        /// <param name="face">The face.</param>
        public void Initialize(uint face)
        {
            Mipmap mip = this.Map(MapOptions.Write, 0, face, true);
            mip.Data.Initialize();
            this.UnMap(0);
        }

        /// <summary>
        /// Initializes the image with specified colour. Other (non RGBA components)
        /// stay the same.
        /// </summary>
        /// <param name="colour">The colour.</param>
        public void Initialize(Colour colour)
        {
            for (uint i = 0; i < FaceCount; i++)
            {
                Initialize(i, colour);
            }
        }

        /// <summary>
        /// Initializes the image's face with specific colour. Other (non RGBA components)
        /// stay the same.
        /// </summary>
        /// <param name="face">The face index.</param>
        /// <param name="colour">The colour.</param>
        public void Initialize(uint face, Colour colour)
        {
            Mipmap mip = this.Map(MapOptions.Write, 0, face, true);
            try
            {
                byte[] data = mip.Data;

                // We go through red first.
                PixelFormat format = this.Format;
                PixelFormat.Element red = format.Find(PixelComponent.Red);
                PixelFormat.Element green = format.Find(PixelComponent.Green);
                PixelFormat.Element blue = format.Find(PixelComponent.Blue);
                PixelFormat.Element alpha = format.Find(PixelComponent.Alpha);

                // We go through each component, red first.
                if (red != null)
                {
                    Images.ImageHelper.WriteComponentToImage(red.Format, data, colour.R, 
                        red.Offset, mip.PixelSize, format.Size);
                }

                // Green ...
                if (green != null)
                {
                    Images.ImageHelper.WriteComponentToImage(green.Format, data, colour.G, 
                        green.Offset, mip.PixelSize, format.Size);
                }

                // Blue.
                if (blue != null)
                {
                    Images.ImageHelper.WriteComponentToImage(blue.Format, data, colour.B, 
                        blue.Offset, mip.PixelSize, format.Size);
                }

                // And alpha channel.
                if (alpha != null)
                {
                    Images.ImageHelper.WriteComponentToImage(alpha.Format, data, colour.A,
                        alpha.Offset, mip.PixelSize, format.Size);
                }
                
            }
            finally
            {
                this.UnMap(0);
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Image ");
            builder.Append(GetType());
            builder.Append("( Width = ");
            builder.Append(Width);
            builder.Append(", Height = ");
            builder.Append(Height);
            builder.Append(", Depth = ");
            builder.Append(Depth);
            builder.Append(", Faces = ");
            builder.Append(FaceCount);
            builder.Append(", Format = ");
            builder.Append(Format);

            return builder.ToString();
        }

        #endregion

    }


}
