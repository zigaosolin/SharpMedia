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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics
{
    /// <summary>
    /// Mipmap is a special class that allows direct manipulation of image. You MUST
    /// unlock it or the image's mipmap will be locked indefinatelly. You can use the
    /// dispose method or image.UnLock method.
    /// 
    /// After the mipmap is disposed, it's contents won't affect original image.
    /// </summary>
    public class Mipmap : IDisposable
    {
        #region Private Members
        private Image image;
        private uint mipmapIndex = 0;
        private byte[] data;
        private uint width = 1;
        private uint height = 1;
        private uint depth = 1;
        private uint face = 0;

        void AssertNotDisposed()
        {
            if (image == null)
            {
                throw new ObjectDisposedException("Mipmap is already disposed, cannot change it.");
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Mipmaps constructor.
        /// </summary>
        /// <param name="w">Width of image.</param>
        /// <param name="h">Height of image.</param>
        /// <param name="d">Depth of image.</param>
        /// <param name="f">Face of image.</param>
        /// <param name="i">Mipmap index.</param>
        /// <param name="imageData">Data of image.</param>
        /// <param name="par">Parent image.</param>
        public Mipmap(uint w, uint h, uint d, uint f, uint i,
                      [NotNull] byte[] imageData, [NotNull] Image par)
        {
            width = w;
            height = h;
            depth = d;
            face = f;
            mipmapIndex = i;
            data = imageData;
            image = par;
        }

        /// <summary>
        /// The size, in bytes, of the mipmap.
        /// </summary>
        public ulong Size
        {
            get { return width * height * depth * image.Format.Size; }
        }

        /// <summary>
        /// Gets the size of the pixel.
        /// </summary>
        /// <value>The size of the pixel.</value>
        public ulong PixelSize
        {
            get
            {
                return width * height * depth;
            }
        }

        /// <summary>
        /// The index of mipmap.
        /// </summary>
        public uint Index
        {
            get { return mipmapIndex; }
        }

        /// <summary>
        /// The format of the image.
        /// </summary>
        public PixelFormat Format
        {
            get { return image.Format; }
        }

        /// <summary>
        /// The mipmap face.
        /// </summary>
        public uint Face
        {
            get { return face; }
        }

        /// <summary>
        /// The width of mipmap.
        /// </summary>
        public uint Width
        {
            get { return width; }
        }

        /// <summary>
        /// Height of mipmap.
        /// </summary>
        public uint Height
        {
            get { return height; }
        }

        /// <summary>
        /// Depth of mipmap
        /// </summary>
        public uint Depth
        {
            get { return depth; }
        }

        /// <summary>
        /// Access to data. Data may be read-only, write-only or both, it
        /// depends on lock options.
        /// </summary>
        public byte[] Data
        {
            get
            {
                AssertNotDisposed();
                return data;
            }
            [param: NotNull]
            set
            {
                AssertNotDisposed();
                if (data.Length != value.Length) throw new ArgumentException("The sizes of buffers not the same.");
                value.CopyTo(data, 0);
            }
        }

        /// <summary>
        /// The parent image.
        /// </summary>
        public Image Parent
        {
            get { return image; }
        }

        #endregion

        #region Data Indexing

        /// <summary>
        /// Loads at specified p.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public Vector4f Load(Vector2i p)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Loads at specified p.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public Vector4f Load(Vector3i p)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Loads at specified p.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public Vector4f Load(Vector4i p)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (data)
            {
                if (image != null)
                {
                    image.UnMap(mipmapIndex);
                    image = null;
                }
            }
        }

        #endregion

    }
}
