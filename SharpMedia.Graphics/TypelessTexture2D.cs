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
using System.Threading;
using SharpMedia.Math;
using System.Runtime.Serialization;
using SharpMedia.Caching;
using SharpMedia.Resources;
using SharpMedia.AspectOriented;
using SharpMedia.Graphics.Implementation;
using System.Runtime.InteropServices;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A 2D texture class, capable of holding texture data arranged in pixel format. It
    /// is automatically bound to device when first rendered to. The class is capable of
    /// holding 2D image in software/hardware mode and provide read/write access to
    /// it.
    /// </summary>
    [Serializable]
    public sealed class TypelessTexture2D : TypelessTexture, ISerializable
    {
        #region Private Members
        // Immutable state.
        PixelFormat format;
        uint width;
        uint height = 1;
        uint mipmapCount = 1;
        byte[][] swData;

        List<LockState> locks = new List<LockState>();

        // Driver part.
        bool isDevicePartChanged = false;
        Driver.ITexture2D driverPart;
        #endregion

        #region Private Methods

        new void AssertNotDisposed()
        {
            if (cacheableState == CacheableState.Disposed)
            {
                throw new ObjectDisposedException("TypelessTexture2D was already disposed.");
            }
        }

        void AssertNotLocked()
        {
            if (locks.Count > 0)
            {
                throw new InvalidOperationException("TypelessTexture2D is locked, operation invalid.");
            }
        }

        void AssertLocked(uint mipmap)
        {
            int i;
            for (i = 0; i < locks.Count; i++)
            {
                if (locks[i].Mipmap == mipmap) break;
            }

            if (i == locks.Count)
            {
                throw new InvalidOperationException("The mipmap " + mipmap + " is not locked.");
            }
        }

        void AssertNotLocked(uint mipmap)
        {
            if (usedByDevice != 0)
            {
                throw new InvalidOperationException("TypelessTexture2D is used by device.");
            }

            int i;
            for (i = 0; i < locks.Count; i++)
            {
                if (locks[i].Mipmap == mipmap)
                {
                    throw new InvalidOperationException("The mipmap " + mipmap + " is locked.");
                }
            }
        }

        void AssertLocked()
        {
            if (locks.Count == 0)
            {
                throw new InvalidOperationException("Texture2D is not locked, operation invalid.");
            }
        }

        void Validate(uint mipCount, uint width, uint height)
        {
            uint maxMipmaps = Images.MipmapHelper.MipmapCount(width, height);
            if (mipCount > maxMipmaps) throw new ArgumentException("Not so many mipmaps exist in texture.");

        }

        internal Driver.ITexture2D DeviceData
        {
            get
            {
                return driverPart;
            }
            set
            {
                driverPart = value;
            }
        }

        void ReleaseDriverPart()
        {
            device = null;
            if (driverPart != null)
            {
                if (sharingContext.IsShared)
                {
                    throw new InvalidOperationException("Unbinding device that is shared and owned.");    
                }
    
                driverPart.Dispose();
                driverPart = null;
            }
        }

        void FillBuffer()
        {
            if (swData != null && !isDevicePartChanged) return;

            swData = GetInternalData();

        }

        byte[][] GetInternalData()
        {
            if (swData != null) return swData;

            // Else, we have to obtain them.
            byte[][] data = new byte[mipmapCount][];
            for (uint i = 0; i < mipmapCount; i++)
            {
                data[i] = driverPart.Read(i, 0);
            }

            return data;
        }

        private void Dispose(bool fromFinalizer)
        {
            if (cacheableState == CacheableState.Disposed) return;
            cacheableState = CacheableState.Disposed;

            swData = null;
            if (driverPart != null)
            {
                if (sharingContext.IsOwned)
                {
                    driverPart.Dispose();
                }
                driverPart = null;
            }

            if (!fromFinalizer)
            {
                GC.SuppressFinalize(this);
            }
            

            // We fire events.
            FireWritten();
        }

        private void FillDriverPart()
        {
            for (uint i = 0; i < mipmapCount; i++)
            {
                driverPart.Update(swData[i], i, 0);
            }
        }

        internal override void SignalChanged()
        {
            // Must sync driver and RAM part
            isDevicePartChanged = true;

            Action<IResource> t = onWritten;
            if (t != null)
            {
                t(this);
            }
        }

        ~TypelessTexture2D()
        {
            Dispose(true);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        private TypelessTexture2D(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.format = info.GetValue("Format", typeof(PixelFormat)) as PixelFormat;
            this.width = info.GetUInt32("Width");
            this.height = info.GetUInt32("Height");
            this.mipmapCount = info.GetUInt32("MipmapCount");
            this.swData = info.GetValue("Data", typeof(byte[][])) as byte[][];
        }

        /// <summary>
        /// Creates a shared typeless texture.
        /// </summary>
        internal TypelessTexture2D(GraphicsDevice device, Driver.SharedTextureInfo info, Guid guid)
            : base(info.Usage, info.TextureUsage, CPUAccess.None, GraphicsLocality.DeviceMemoryOnly)
        {
            this.width = info.Width;
            this.height = info.Height;
            this.format = info.Format;
            this.mipmapCount = info.Mipmaps;

            this.sharingContext = new SharingContext(guid);

            // We get shared.
            this.driverPart = info.Texture as Driver.ITexture2D;
        }
            

        /// <summary>
        /// Initializes a new instance of the <see cref="TypelessTexture2D"/> class.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <param name="textureUsage">The texture usage.</param>
        /// <param name="fmt">The FMT.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dp">The dp.</param>
        public TypelessTexture2D(GraphicsDevice device, Usage usage, TextureUsage textureUsage, 
                            CPUAccess cpuAccess, PixelFormat fmt, uint width, uint height, uint mipmaps, 
                            uint sampleCount, uint sampleQuality, GraphicsLocality locality, byte[][] data)
            : base(usage, textureUsage, cpuAccess, locality)
        {
            Validate(mipmaps, width, height);
            this.width = width;
            this.height = height;
            this.format = fmt;
            this.mipmapCount = mipmaps == 0 ? Images.MipmapHelper.MipmapCount(width, height) : mipmaps;

            // We create driver part.
            if (locality != GraphicsLocality.SystemMemoryOnly)
            {
                this.device = device;
                this.driverPart = device.DriverDevice.CreateTexture2D(usage, fmt.CommonFormatLayout,
                                CPUAccess, width, height, mipmaps, textureUsage, sampleCount, sampleQuality, data);
            }

            // We may need to create sw part.
            if (locality == GraphicsLocality.DeviceAndSystemMemory
                || locality == GraphicsLocality.SystemMemoryOnly)
            {
                this.swData = data;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <param name="textureUsage">The texture usage.</param>
        /// <param name="fmt">The FMT.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipmaps">The number of mipmaps.</param>
        /// <param name="locality">The locality.</param>
        /// <param name="data">The data.</param>
        public TypelessTexture2D(Usage usage, TextureUsage textureUsage, CPUAccess cpuAccess, PixelFormat fmt,
                            uint width, uint height, uint mipmaps, GraphicsLocality locality, byte[][] data)
            : base(usage, textureUsage, cpuAccess, locality)
        {
            // Mipmaps first.
            Validate(mipmaps, width, height);
            this.mipmapCount = mipmaps == 0 ? Images.MipmapHelper.MipmapCount(width, height) : mipmaps;

            // We copy data if it exists, otherwise we dont do it.
            if (data != null)
            {
                if (data.Length != mipmapCount)
                {
                    throw new ArgumentException("Data is not provided for all mipmaps.");
                }

                swData = new byte[mipmapCount][];

                for (uint i = 0; i < mipmapCount; i++)
                {
                    if (data[i].Length != fmt.Size * width * height)
                    {
                        throw new ArgumentOutOfRangeException(
                            "Byte size is not the same as texture size for mipmap " + i.ToString());
                    }

                    // We copy the data now.
                    swData[i] = data[i];
                }
            }
            else
            {
                swData = new byte[mipmapCount][];
                for (uint i = 0; i < mipmapCount; i++)
                {
                    uint w, h;
                    Images.MipmapHelper.ComputeDimensions(width, height, i, out w, out h);
                    swData[i] = new byte[w * h * fmt.Size];
                }
            }

            // And we copy all data.
            this.width = width;
            this.height = height;
            this.format = fmt;
        }

        #endregion

        #region Public Methods

        public override void BindToDevice(GraphicsDevice device)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                AssertNotLocked();

                this.device = device;
                if (driverPart == null)
                {
                    if (sharingContext.IsOwned)
                    {
                        driverPart = device.DriverDevice.CreateTexture2D(usage, Format.CommonFormatLayout,
                            this.CPUAccess, width, height, mipmapCount, textureUsage, 1, 0, swData);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public override void UnBindFromDevice()
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                AssertNotLocked();
                ReleaseDriverPart();
            }
        }

        public override void Dispose()
        {
            lock (syncRoot)
            {
                Dispose(false);
            }
        }

        public override PixelFormat Format
        {
            get { return format; }
        }

        public override uint MipmapCount
        {
            get { return mipmapCount; }
        }

        public override void GenerateMipmaps(BuildImageFilter filter)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                AssertNotLocked();
                throw new NotImplementedException();
            }
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
            try
            {
                Monitor.Enter(syncRoot);
                AssertNotLocked(mipmap);
                AssertNotDisposed();

                // Check if all is right.
                if (face != 0) throw new IndexOutOfRangeException("Face " + face + " not supported.");
                if (mipmap >= mipmapCount) 
                    throw new IndexOutOfRangeException("Mipmap " + mipmap + " out of range.");

                // We snyc it if necessary.
                if (swData != null && isDevicePartChanged)
                {
                    swData = GetInternalData();
                    isDevicePartChanged = false;
                }

                // Copy options.
                LockState state = new LockState();
                state.Mipmap = mipmap;
                state.MapOptions = op;

                // Precompute sizes.
                uint w, h;
                Images.MipmapHelper.ComputeDimensions(width, height, mipmap, out w, out h);


                // We now perform actual lock.
                if (swData != null)
                {
                    state.LockedData = swData[mipmap];
                }
                else
                {
                    if (op == MapOptions.Write)
                    {
                        state.LockedData = new byte[w * h * format.Size];
                    }
                    else
                    {
                        state.LockedData = driverPart.Read(mipmap, 0);
                    }
                }
                
                // We add the lock state.
                locks.Add(state);

                return new Mipmap(w, h, 1, 0, mipmap, state.LockedData, this);

            }
            catch (Exception)
            {
                Monitor.Exit(syncRoot);
                throw;
            }
        }

        public override void UnMap(uint mipmap)
        {

            // Must initialize the read because it may be unassigned.
            LockState state;
            state.MapOptions = MapOptions.Read;
            try
            {
                AssertNotDisposed();
                AssertLocked(mipmap);

                // We find the mipmap.
                int index = 0;
                for (; index < locks.Count; index++)
                {
                    if (locks[index].Mipmap == mipmap) break;
                }

                // Obtain and remove index.
                state = locks[index];
                locks.RemoveAt(index);

                if (swData != null)
                {
                    if (state.MapOptions == MapOptions.Read)
                    {
                        return;
                    }
                    
                    // We have to update driver part.
                    if (driverPart != null)
                    {
                        driverPart.Update(state.LockedData, state.Mipmap, 0);
                    }
                }
                else
                {
                    if (state.MapOptions == MapOptions.Read)
                    {
                        return;
                    }

                    driverPart.Update(state.LockedData, state.Mipmap, 0);
                }

            }
            finally
            {
                Monitor.Exit(syncRoot);
            }

            // We fire written event.
            FireWritten();
            

            
        }

        public override Image CloneSameType(PixelFormat fmt, uint width, uint height, 
                                                uint depth, uint faces, uint mipmaps)
        {
            if (depth != 1 || faces != 1) throw new ArgumentException("Depth and faces must be 1 for this image type.");
            return new TypelessTexture2D(usage, textureUsage, cpuAccess, fmt, width, height, mipmapCount, locality, null);  
        }

        public override ResourceAddress Address
        {
            get 
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();

                    Placement placement = Placement.NoPlacement;
                    if (driverPart != null) placement |= Placement.DeviceMemory;
                    if (swData != null) placement |= Placement.Memory;
                    return new ResourceAddress(placement);
                }
            }
        }

        public override GraphicsLocality Locality
        {
            get
            {
                return locality;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    AssertNotLocked();

                    // Early exit.
                    if (locality == value) return;

                    if (driverPart == null 
                        && value == GraphicsLocality.DeviceMemoryOnly)
                    {
                        throw new ArgumentException("Cannot create device memory only texture, driver part does "
                            + "not exist (prepare it first).");
                    }

                    // We check what we need to do.
                    locality = value;

                    switch (locality)
                    {
                        case GraphicsLocality.SystemMemoryOnly:
                            FillBuffer();
                            ReleaseDriverPart();
                            break;
                        case GraphicsLocality.DeviceMemoryOnly:
                            swData = null;
                            break;
                        case GraphicsLocality.DeviceAndSystemMemory:
                            FillBuffer();
                            break;
                        case GraphicsLocality.DeviceOrSystemMemory:
                            if (driverPart != null)
                            {
                                swData = null;
                            }
                            break;
                    }
                }
            }
        }

        public override bool IsBoundToDevice
        {
            get { return device != null; }
        }

        #endregion

        #region ISerializable Members

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();
                AssertNotLocked();

                if (locality == GraphicsLocality.DeviceMemoryOnly)
                {
                    throw new InvalidOperationException("Cannot serialize device memory only texture.");
                }

                base.GetObjectData(info, context);

                info.AddValue("Format", format);
                info.AddValue("Width", width);
                info.AddValue("Height", height);
                info.AddValue("MipmapCount", mipmapCount);
                info.AddValue("Data", GetInternalData());
            }
        }

        #endregion

        #region Views

        /// <summary>
        /// Creates the shader resource view.
        /// </summary>
        /// <returns></returns>
        public TextureView CreateTexture()
        {
            return CreateTexture(this.format, 0, mipmapCount);
        }

        /// <summary>
        /// Creates the shader resource view.
        /// </summary>
        /// <param name="mostDetailed">The most detailed.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public TextureView CreateTexture(uint mostDetailed, uint count)
        {
            return CreateTexture(this.format, mostDetailed, count);
        }

        /// <summary>
        /// Creates the shader resource view.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public TextureView CreateTexture(PixelFormat format)
        {
            return CreateTexture(format, 0, mipmapCount);
        }

        /// <summary>
        /// Creates the shader resource view.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="mostDetailed">The most detailed.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public TextureView CreateTexture([NotNull] PixelFormat format, uint mostDetailed, uint count)
        {
            if (format.IsTypeless || !this.format.IsCompatible(format))
            {
                throw new ArgumentException("Format for type resource is either typeless or not compatible.");
            }

            // We compute actual count.
            if(count == 0) count = mipmapCount - mostDetailed;

            if (mostDetailed + count > mipmapCount)
            {
                throw new IndexOutOfRangeException("Trying to access mipmap(s) out of range.");
            }

            return new Implementation.TypelessTexture2DAsTexture2D(format, mostDetailed, count, this);
        }

        /// <summary>
        /// Creates the render target view.
        /// </summary>
        /// <returns></returns>
        public RenderTargetView CreateRenderTarget()
        {
            return CreateRenderTarget(this.format, 0);
        }

        /// <summary>
        /// Creates the render target view.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public RenderTargetView CreateRenderTarget(PixelFormat format)
        {
            return CreateRenderTarget(format, 0);
        }

        /// <summary>
        /// Creates the render target view.
        /// </summary>
        /// <param name="mipmap">The mipmap.</param>
        /// <returns></returns>
        public RenderTargetView CreateRenderTarget(uint mipmap)
        {
            return CreateRenderTarget(format, mipmap);
        }

        /// <summary>
        /// Creates the render target view.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="mipmap">The mipmap.</param>
        /// <returns></returns>
        public RenderTargetView CreateRenderTarget(PixelFormat format, uint mipmap)
        {
            return new Implementation.TypelessTexture2DAsRenderTarget(this, format, mipmap);
        }

        /// <summary>
        /// Crates a render target view with multisapling.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public RenderTargetView CreateRenderTargetMS(PixelFormat format)
        {
            return new Implementation.TypelessTexture2DAsRenderTarget(this, format);
        }

        /// <summary>
        /// Creates a depth stencil target.
        /// </summary>
        /// <param name="mipmap">The mipmap to use.</param>
        /// <returns></returns>
        public DepthStencilTargetView CreateDepthStencil()
        {
            return null;
        }

        #endregion

        #region Data Manipulation

        /// <summary>
        /// Sets data of typeless texture.
        /// </summary>
        /// <typeparam name="T">The size of type must match bytesize of format.</typeparam>
        /// <param name="mipmap"></param>
        /// <param name="array"></param>
        public unsafe void SetData<T>(uint mipmap, T[] data)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                // Validate T.
                ulong structSize = (ulong)Marshal.SizeOf(typeof(T));

                if (structSize != format.Size)
                {
                    throw new ArgumentException("The T is not compatible with pixel format.");
                }

                // We first map it, it may throw (if it throws, it is automatically unlocked).
                byte[] dstBuffer = Map(MapOptions.Write, mipmap).Data;

                if ((ulong)dstBuffer.LongLength != (ulong)data.LongLength * structSize)
                {
                    throw new InvalidOperationException("Trying to set data that is not compatible in size.");
                }

                // We pin the address.
                try
                {
                    fixed (byte* dst = dstBuffer)
                    {
                        Common.Memcpy(data, dst, (ulong)dstBuffer.LongLength);
                    }
                }
                finally
                {
                    // Must unmap.
                    UnMap(mipmap);
                }

            }
        }

        /// <summary>
        /// Obtains data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mipmap"></param>
        /// <returns></returns>
        public unsafe T[] GetData<T>(uint mipmap)
        {
            lock (syncRoot)
            {
                AssertNotDisposed();

                // Validate T.
                ulong structSize = (ulong)Marshal.SizeOf(typeof(T));

                if (structSize != format.Size)
                {
                    throw new ArgumentException("The T is not compatible with pixel format.");
                }

                // We first map it, it may throw (if it throws, it is automatically unlocked).
                byte[] srcBuffer = Map(MapOptions.Read, mipmap).Data;


                T[] data = new T[srcBuffer.Length / (int)structSize];

                try
                {
                    fixed (byte* src = srcBuffer)
                    {
                        Common.Memcpy(src, data, (ulong)srcBuffer.Length);
                    }

                }
                finally
                {

                    // Must unmap.
                    UnMap(mipmap);
                }

                return data;
            }
        }

        #endregion
    }
}
