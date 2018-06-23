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
using System.Runtime.Serialization;
using System.Threading;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A texture is hardware accelerated image that can be used
    /// as source of rendering when attached as state of shader and
    /// as a destination when used as render target. Some restrictions may apply
    /// fot texture and it's usage.
    /// </summary>
    public abstract class TypelessTexture : Image, IGraphicsLocality, 
        ISerializable, IGraphicsSignature
    {
        #region Private Members
        protected SharingContext sharingContext = new SharingContext();
        protected Usage usage = Usage.Static;
        protected TextureUsage textureUsage = TextureUsage.None;
        protected CPUAccess cpuAccess = CPUAccess.None;
        protected GraphicsLocality locality = GraphicsLocality.DeviceOrSystemMemory;
        protected bool disposeOnViewDispose = true;
        protected uint viewCount = 0;
        protected GraphicsDevice device;
        protected uint usedByDevice = 0;
        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds a view refence to texture.
        /// </summary>
        internal void AddRef()
        {
            lock (syncRoot)
            {
                viewCount++;
            }
        }

        /// <summary>
        /// Removes a view reference from texture.
        /// </summary>
        internal void Release()
        {
            lock (syncRoot)
            {
                viewCount--;
                if (viewCount == 0 && disposeOnViewDispose)
                {
                    Dispose();
                }
            }
        }

        /// <summary>
        /// Is the typeless texture used by device.
        /// </summary>
        internal void UsedByDevice()
        {
            usedByDevice++;

            if (usedByDevice == 1)
            {
                Monitor.Enter(syncRoot);
            }
        }

        internal void UnusedByDevice()
        {
            if (--usedByDevice == 0)
            {
                Monitor.Exit(syncRoot);
            }
        }

        internal void RegisterShared(Guid guid)
        {
            lock (syncRoot)
            {
                sharingContext.Guid = guid;
            }
        }

        internal void UnRegisterShared(Guid guid)
        {
            lock (syncRoot)
            {
                sharingContext.Guid = Guid.Empty;
            }
        }

        /// <summary>
        /// Signals the texture that it is changed.
        /// </summary>
        internal abstract void SignalChanged();

        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected TypelessTexture(SerializationInfo info, StreamingContext context)
        {
            this.usage = (Usage)info.GetValue("Usage", typeof(Usage));
            this.textureUsage = (TextureUsage)info.GetValue("TextureUsage", typeof(TextureUsage));
            this.locality = (GraphicsLocality)info.GetValue("Locality", typeof(GraphicsLocality));
        }

       

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <param name="texUsage">The texture usage.</param>
        public TypelessTexture(Usage usage, TextureUsage texUsage, CPUAccess cpuAccess, GraphicsLocality locality)
        {
            this.usage = usage;
            textureUsage = texUsage;
            this.locality = locality;
            this.cpuAccess = cpuAccess;
        }

        /// <summary>
        /// Can the texture be used as render target.
        /// </summary>
        public bool CanBeRenderTarget
        {
            get 
            { 
                return (TextureUsage & TextureUsage.RenderTarget) != 0; 
            }
        }

        /// <summary>
        /// Returns the texture's device. May be null if not bound.
        /// </summary>
        public GraphicsDevice Device
        {
            get
            {
                return device;
            }
        }

        /// <summary>
        /// Obtains read-only texture usage.
        /// </summary>
        public TextureUsage TextureUsage
        {
            get
            {
                return textureUsage;
            }
        }

        /// <summary>
        /// Gets the sharing context, if it exists.
        /// </summary>
        /// <value>The context.</value>
        public SharingContext SharingContext
        {
            get
            {
                return sharingContext;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is shared.
        /// </summary>
        /// <value><c>true</c> if this instance is shared; otherwise, <c>false</c>.</value>
        public bool IsShared
        {
            get
            {
                return sharingContext.IsShared;
            }
        }

        /// <summary>
        /// Is the texture disposed when all views are disposed.
        /// </summary>
        public bool DisposeOnViewDispose
        {
            get
            {
                return disposeOnViewDispose;
            }
            set
            {
                lock (syncRoot)
                {
                    disposeOnViewDispose = value;
                }
            }
        }

        #endregion

        #region IGraphicsLocality Members

        public abstract GraphicsLocality Locality
        {
            get;
            set;
        }

        public abstract bool IsBoundToDevice { get; }

        public abstract void BindToDevice(GraphicsDevice device);

        public abstract void UnBindFromDevice();

        #endregion

        #region ISerializable Members

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Usage", usage);
            info.AddValue("TextureUsage", textureUsage);
            info.AddValue("Locality", locality);
        }

        #endregion

        #region IGraphicsSignature Members


        public CPUAccess CPUAccess
        {
            get { return cpuAccess; }
        }

        public ulong ByteSize
        {
            get { return this.Format.Size * Width * Height * Depth * FaceCount; }
        }

        public Usage Usage
        {
            get
            {
                return usage;
            }
        }

        #endregion
    }
}
