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
using SharpMedia.Graphics.States;
using SharpMedia.Math;
using SharpMedia.Graphics.Implementation;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics
{


    /// <summary>
    /// A graphics device.
    /// </summary>
    public class GraphicsDevice : IDisposable
    {
        #region Non-Accessible
        private Driver.IDevice device;
        private object rootSync = new object();
        private uint locks = 0;
        private bool isDisposed = false;

        // State objects references.
        Geometry inputGeometry;
        Geometry outputGeometry;
        Region2i[]  viewports = new Region2i[0];
        Region2i[]  scissorRects = new Region2i[0];
        BlendState blendState;
        Colour blendColour;
        uint blendMask;
        DepthStencilState depthStencilState;
        uint stencilRef;
        RasterizationState rasterizationState;
        SamplerState[] vertexSamplerStates = new SamplerState[0];
        SamplerState[] pixelSamplerStates = new SamplerState[0];
        SamplerState[] geometrySamplerStates = new SamplerState[0];
        TextureView[] vertexTextures = new TextureView[0];
        TextureView[] pixelTextures = new TextureView[0];
        TextureView[] geometryTextures = new TextureView[0];
        ConstantBufferView[] vertexCBuffers = new ConstantBufferView[0];
        ConstantBufferView[] pixelCBuffers = new ConstantBufferView[0];
        ConstantBufferView[] geometryCBuffers = new ConstantBufferView[0];
        RenderTargetView[] pixelRenderTargets = new RenderTargetView[0];
        DepthStencilTargetView pixelDepthStencilTarget;
        Shaders.VShader vshader;
        Shaders.PShader pshader;
        Shaders.GShader gshader;

        // Events.
        DeviceListener listener;
        Action<GraphicsDevice> deviceLost;
        Action<GraphicsDevice> deviceReset;
        Action<GraphicsDevice> deviceDisposing;

        // Default render targets.
        RenderTargetView defaultRenderTarget;
        DepthStencilTargetView defaultDepthStencilTarget;
        
        // Managers.
        DevicePerformance devicePerformance;
        TextureManager textureManager;
        ShaderManager shaderManager;
        BufferManager bufferManager;
        StateManager stateManager;

        #endregion

        #region Constants

        /// <summary>
        /// Maximum number of render targets bound at the same time.
        /// </summary>
        public const uint MaxRenderTargets = 8;

        /// <summary>
        /// Maximum number of viewports.
        /// </summary>
        public const uint MaxViewportCount = 8;

        /// <summary>
        /// Maximum number of scissor rectangles.
        /// </summary>
        public const uint MaxScissorRectCount = 8;

        /// <summary>
        /// Maximum number of samplers.
        /// </summary>
        public const uint MaxSamplers = 8;

        /// <summary>
        /// Maximum number of textures.
        /// </summary>
        public const uint MaxTexture = 128;

        /// <summary>
        /// Maximum number of state object per type.
        /// </summary>
        public const uint MaxStateObjectsPerType = 4096;

        /// <summary>
        /// Maximum number of geometry output streams.
        /// </summary>
        public const uint MaxGeometryOutputStream = 4;

        #endregion

        #region Private Methods

        internal Driver.IDevice DriverDevice
        {
            get
            {
                return device;
            }
        }        

        internal void RaiseDispose()
        {
            Dispose();
        }

        internal void RaiseReset()
        {
            throw new NotImplementedException();
        }

        private void AssertNotDiposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("Device was already disposed, cannot use it.");
            }
        }

        private void AssertLocked()
        {
            if (locks == 0)
            {
                throw new InvalidOperationException("The Device is not locked.");
            }
        }

        private void AssertNotLocked()
        {
            if (locks != 0)
            {
                throw new InvalidOperationException("The device is locked.");
            }
        }

        private void DrawValidate()
        {

        }

        private void ClearStatesInternal()
        {
            if (inputGeometry != null)
            {
                inputGeometry.UnusedByDevice();
                inputGeometry = null;
            }


            if (outputGeometry != null)
            {
                outputGeometry.UnusedByDevice();
                outputGeometry = null;
            }

            // Can simply reset viewport and scissor rects.
            viewports = new Region2i[0];
            scissorRects = new Region2i[0];

            // Blend state.
            if (blendState != null)
            {
                Monitor.Exit(blendState.SyncRoot);
            }
            blendState = StateManager.Intern(new BlendState());
            blendColour = Colour.Black;
            blendMask = 0;
            blendState.Prepare(this);

            // Depth-stencil state.
            if (depthStencilState != null)
            {
                Monitor.Exit(depthStencilState.SyncRoot);
            }
            depthStencilState = StateManager.Intern(new DepthStencilState());
            stencilRef = 0;
            depthStencilState.Prepare(this);

            // Rasterization state.
            if (rasterizationState != null)
            {
                Monitor.Exit(rasterizationState.SyncRoot);
            }
            rasterizationState = StateManager.Intern(new RasterizationState());
            rasterizationState.Prepare(this);

            // Samplers.
            int i;
            for (i = 0; i < vertexSamplerStates.Length; i++)
            {
                Monitor.Exit(vertexSamplerStates[i].SyncRoot);
            }

            for (i = 0; i < pixelSamplerStates.Length; i++)
            {
                Monitor.Exit(pixelSamplerStates[i].SyncRoot);
            }

            for (i = 0; i < geometrySamplerStates.Length; i++)
            {
                Monitor.Exit(geometrySamplerStates[i].SyncRoot);
            }

            // Textures.
            for (i = 0; i < vertexTextures.Length; i++)
            {
                vertexTextures[i].UnusedByDevice();
            }

            for (i = 0; i < pixelTextures.Length; i++)
            {
                pixelTextures[i].UnusedByDevice();
            }

            for (i = 0; i < geometryTextures.Length; i++)
            {
                geometryTextures[i].UnusedByDevice();
            }

            // Buffers.
            for (i = 0; i < vertexCBuffers.Length; i++)
            {
                vertexCBuffers[i].UnusedByDevice();
            }

            for (i = 0; i < pixelCBuffers.Length; i++)
            {
                pixelCBuffers[i].UnusedByDevice();
            }

            for (i = 0; i < geometryCBuffers.Length; i++)
            {
                geometryCBuffers[i].UnusedByDevice();
            }

            // Render targets.
            for (i = 0; i < pixelRenderTargets.Length; i++)
            {
                pixelRenderTargets[i].UnusedByDevice();
            }
            pixelRenderTargets = new RenderTargetView[0];

            // Depth-stencil target.
            if (pixelDepthStencilTarget != null)
            {
                pixelDepthStencilTarget.UnusedByDevice();
            }
            pixelDepthStencilTarget = null;

            // Shaders have no managment.
            vshader = null;
            pshader = null;
            gshader = null;

            device.ClearStates();
        }

        private void ReclaimLocks()
        {
            if(inputGeometry != null) inputGeometry.UsedByDevice();
            if (outputGeometry != null) outputGeometry.UsedByDevice();
            Monitor.Enter(blendState.SyncRoot);
            Monitor.Enter(depthStencilState.SyncRoot);
            Monitor.Enter(rasterizationState.SyncRoot);

            // Shader locks are intentionally not reclaimed, since they were not released.

            // Samplers.
            int i;
            for (i = 0; i < vertexSamplerStates.Length; i++) Monitor.Enter(vertexSamplerStates[i].SyncRoot);
            for (i = 0; i < pixelSamplerStates.Length; i++) Monitor.Enter(pixelSamplerStates[i].SyncRoot);
            for (i = 0; i < geometrySamplerStates.Length; i++) Monitor.Enter(geometrySamplerStates[i].SyncRoot);

            // Textures.
            for (i = 0; i < vertexTextures.Length; i++) vertexTextures[i].UsedByDevice();
            for (i = 0; i < pixelTextures.Length; i++) pixelTextures[i].UsedByDevice();
            for (i = 0; i < geometryTextures.Length; i++) geometryTextures[i].UsedByDevice();
            
            // Buffers.
            for (i = 0; i < vertexCBuffers.Length; i++) vertexCBuffers[i].UsedByDevice();
            for (i = 0; i < pixelCBuffers.Length; i++) pixelCBuffers[i].UsedByDevice();
            for (i = 0; i < geometryCBuffers.Length; i++) geometryCBuffers[i].UsedByDevice();
            
            // RTs
            for (i = 0; i < pixelRenderTargets.Length; i++) pixelRenderTargets[i].UsedByDevice();

            if (pixelDepthStencilTarget != null) pixelDepthStencilTarget.UsedByDevice();

            // We actually aquire lockes.
            SetBlendState(blendState, blendColour, blendMask);
            SetDepthStencilState(depthStencilState, stencilRef);
            SetRasterizationState(rasterizationState);
            SetViewports(viewports);
            SetScissorRects(scissorRects);
            SetPixelShader(pshader, pixelSamplerStates, pixelTextures, pixelCBuffers,
                pixelRenderTargets, pixelDepthStencilTarget);
            SetGeometryShader(gshader, outputGeometry, geometrySamplerStates, 
                geometryTextures, geometryCBuffers);
            SetVertexShader(vshader, inputGeometry, vertexSamplerStates, 
                            vertexTextures, vertexCBuffers);
            


        }

        private void ReleaseLocks()
        {
            if (inputGeometry != null) inputGeometry.UnusedByDevice();
            if (outputGeometry != null) outputGeometry.UnusedByDevice();
            Monitor.Exit(blendState.SyncRoot);
            Monitor.Exit(depthStencilState.SyncRoot);
            Monitor.Exit(rasterizationState.SyncRoot);

            // Shader locks are intentionally not released, since shaders could be released.

            // Samplers.
            int i;
            for (i = 0; i < vertexSamplerStates.Length; i++) Monitor.Exit(vertexSamplerStates[i].SyncRoot);
            for (i = 0; i < pixelSamplerStates.Length; i++) Monitor.Exit(pixelSamplerStates[i].SyncRoot);
            for (i = 0; i < geometrySamplerStates.Length; i++) Monitor.Exit(geometrySamplerStates[i].SyncRoot);

            // Textures.
            for (i = 0; i < vertexTextures.Length; i++) vertexTextures[i].UnusedByDevice();
            for (i = 0; i < pixelTextures.Length; i++) pixelTextures[i].UnusedByDevice();
            for (i = 0; i < geometryTextures.Length; i++) geometryTextures[i].UnusedByDevice();

            // Buffers.
            for (i = 0; i < vertexCBuffers.Length; i++) vertexCBuffers[i].UnusedByDevice();
            for (i = 0; i < pixelCBuffers.Length; i++) pixelCBuffers[i].UnusedByDevice();
            for (i = 0; i < geometryCBuffers.Length; i++) geometryCBuffers[i].UnusedByDevice();

            // RTs
            for (i = 0; i < pixelRenderTargets.Length; i++) pixelRenderTargets[i].UnusedByDevice();

            if (pixelDepthStencilTarget != null) pixelDepthStencilTarget.UnusedByDevice();

            device.ClearStates();
        }
        

        /// <summary>
        /// Can be only created by AdapterFactory.
        /// </summary>
        /// <param name="d">The device.</param>
        internal GraphicsDevice(Driver.IDevice d)
        {
            // Init all data.
            device = d;

            devicePerformance = new DevicePerformance(this);
            textureManager = new TextureManager(this);
            shaderManager = new ShaderManager(this);
            bufferManager = new BufferManager(this);
            stateManager = new StateManager(this);

            ClearStatesInternal();
        }

        /// <summary>
        /// Initializes the specified device.
        /// </summary>
        /// <param name="colour">The colour.</param>
        /// <param name="depthStencil">The depth stencil.</param>
        internal void Initialize(RenderTargetView colour, DepthStencilTargetView depthStencil)
        {
            defaultRenderTarget = colour;
            defaultDepthStencilTarget = depthStencil;

            // We need to add event.
            listener = new DeviceListener(this);
            device.RegisterListener(listener);

           
        }

        ~GraphicsDevice()
        {
            Dispose(true);
        }

        /// <summary>
        /// Triggers the lost event.
        /// </summary>
        internal void RaiseLostEvent()
        {
            Action<GraphicsDevice> lost = deviceLost;
            if (lost != null)
            {
                lost(this);
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the default device. If device already created, it binds to it.
        /// </summary>
        public static GraphicsDevice GetDefaultDevice()
        {
            GraphicsService init = null;// new GraphicsService();
            if (init.DeviceExists)
            {
                return init.ObtainDevice(null);
            }
            else
            {
                throw new InvalidOperationException("Cannot create default device because no-one initialized it yet.");
            } 
        }

        #endregion

        #region Events

        /// <summary>
        /// A device lost event. Must not be called within Enter() and Exit() states.
        /// </summary>
        public event Action<GraphicsDevice> DeviceLost
        {
            add
            {
                lock (rootSync)
                {
                    deviceLost += value;
                }
            }
            remove
            {
                lock (rootSync)
                {
                    deviceLost -= value;
                }
            }
        }

        /// <summary>
        /// A device reset event. Must not be called within Enter() and Exit() states.
        /// </summary>
        public event Action<GraphicsDevice> DeviceReset
        {
            add
            {
                lock (rootSync)
                {
                    deviceReset += value;
                }
            }
            remove
            {
                lock (rootSync)
                {
                    deviceReset -= value;
                }
            }
        }

        /// <summary>
        /// A device disposing event. Here, resources must be deleted before the device is
        /// disposed, e.g. must be al least unbound from hardware.
        /// </summary>
        public event Action<GraphicsDevice> DeviceDisposing
        {
            add
            {
                lock (rootSync)
                {
                    deviceDisposing += value;
                }
            }
            remove
            {
                lock (rootSync)
                {
                    deviceDisposing -= value;
                }
            }
        }

        #endregion

        #region Render Target Sharing

        /// <summary>
        /// Registers a shared render target texture.
        /// </summary>
        /// <param name="target">The texture to be registered.</param>
        /// <returns>A guid that can be used to access it from other process.</returns>
        public Guid RegisterShared(TypelessTexture target, TextureUsage allowUsage)
        {
            if (target.IsShared)
            {
                throw new ArgumentException("Texture is already shared.");
            }

            // We must lock.
            target.UsedByDevice();

            try
            {
                // Ensure bound.
                target.BindToDevice(this);

                // We create sharing context.
                Driver.SharedTextureInfo info = new Driver.SharedTextureInfo();
                info.Width = target.Width;
                info.Height = target.Height;
                info.Depth = target.Depth;
                info.TextureUsage = allowUsage;
                info.Usage = target.Usage;
                info.Format = target.Format;
                info.Mipmaps = target.MipmapCount;

                if (target is TypelessTexture2D)
                {
                    info.Texture = (target as TypelessTexture2D).DeviceData;
                }
                else
                {
                    throw new NotImplementedException();
                }


                Guid guid = Guid.NewGuid();
                target.SharingContext.Guid = guid;

                this.device.RegisterShared(guid, info);

                return guid;
            }
            finally
            {
                target.UnusedByDevice();
            }
            
        }

        /// <summary>
        /// Unregisters the shared. This is automatically done on texture disposal.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public void UnRegisterShared(TypelessTexture texture)
        {
            texture.UsedByDevice();

            try
            {
                Guid guid = texture.SharingContext.Guid;
                texture.SharingContext.Guid = Guid.Empty;

                this.device.UnregisterShared(guid);

            }
            finally
            {
                texture.UnusedByDevice();
            }
        }

        /// <summary>
        /// Obtains a read-only render target registered by other process.
        /// </summary>
        /// <param name="guid">The global unique identifier.</param>
        /// <returns>Read-only render target.</returns>
        public TypelessTexture GetShared(Guid guid)
        {
            Driver.SharedTextureInfo info = device.GetShared(guid);
            if (info == null) return null;

            if (info.Texture is Driver.ITexture2D)
            {
                return new TypelessTexture2D(this, info, guid);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Creates a new instance of shader compiler.
        /// </summary>
        /// <value>The shader compiler.</value>
        public Shaders.ShaderCompiler CreateShaderCompiler()
        {
            lock (rootSync)
            {
                AssertNotDiposed();
                return new SharpMedia.Graphics.Shaders.ShaderCompiler(device.CreateShaderCompiler());
            }

        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns associated swap chain, if it exists.
        /// </summary>
        public SwapChain SwapChain
        {
            get
            {
                if (defaultRenderTarget is SwapChain)
                {
                    return defaultRenderTarget as SwapChain;
                }
                return null;
            }
        }

        /// <summary>
        /// Tells whether device is locked.
        /// </summary>
        public bool IsLocked
        {
            get
            {
                return locks > 0;
            }
        }

        /// <summary>
        /// Symbolic name of device.
        /// </summary>
        public string Name
        {
            get { return device.Name; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed
        {
            get
            {
                return isDisposed;
            }
        }



        /// <summary>
        /// Gets the state manager.
        /// </summary>
        /// <value>The state manager.</value>
        public StateManager StateManager
        {
            get
            {
                return stateManager;
            }
        }

        /// <summary>
        /// Enables/disabled GPU emulator. Use for debugging only.
        /// </summary>
        public bool EnableGPUEmulator
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the performance.
        /// </summary>
        /// <value>The performance.</value>
        public DevicePerformance DevicePerformance
        {
            get
            {
                return devicePerformance;
            }
        }

        /// <summary>
        /// Gets the buffer manager.
        /// </summary>
        /// <value>The buffer manager.</value>
        public BufferManager BufferManager
        {
            get
            {
                return bufferManager;
            }
        }

        /// <summary>
        /// Gets the texture manager.
        /// </summary>
        /// <value>The texture manager.</value>
        public TextureManager TextureManager
        {
            get
            {
                return textureManager;
            }
        }

        /// <summary>
        /// Gets the shader manager.
        /// </summary>
        public ShaderManager ShaderManager
        {
            get
            {
                return shaderManager;
            }
        }

        #endregion

        #region Capabilities

        /// <summary>
        /// Checks if device supports pixel format.
        /// </summary>
        /// <param name="format">The format to support.</param>
        /// <returns>Does device support the format.</returns>
        public FormatUsage FormatSupport(PixelFormat format)
        {
            if (format.CommonFormatLayout == CommonPixelFormatLayout.NotCommonLayout) return FormatUsage.None;
            return device.FormatSupport(format.CommonFormatLayout);
        }

        /// <summary>
        /// Checks if device supports format; if not, it returns false but gives the
        /// hint of the nearest format. Sometimes, only rearangement of components
        /// is required or lower/higher precission.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="nearest">The nearest format, if available. If false is returned,
        /// the nearest format may be non-null, but not necessary.</param>
        /// <returns>True if format support, false otherwise with nearest format set
        /// to valid value or not.</returns>
        public bool FormatSupport(PixelFormat format, FormatUsage usage, out PixelFormat near)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The multi-sampling quality that can be specified.
        /// </summary>
        /// <param name="format">The pixel format.</param>
        /// <param name="sampleCount">Number of samplings taken.</param>
        /// <returns>The number of quality levels.</returns>
        public uint MultiSamplingQuality(PixelFormat format, uint sampleCount)
        {
            return device.MultiSamplingQuality(format.CommonFormatLayout, sampleCount);
        }

        #endregion

        #region Thread-Safety

        /// <summary>
        /// Enters thread-safe section; this must be done before any states are bound
        /// to device.
        /// </summary>
        public void Enter()
        {
            AssertNotDiposed();

            Monitor.Enter(rootSync);
            if (locks == 0)
            {
                device.Enter();
                locks = 1;
                devicePerformance.BeginFrame();
                stateManager.BeginFrame();

                ReclaimLocks();
            }
            else
            {
                locks++;
            }
            
        }

        /// <summary>
        /// Enters thread-safe sector, returning a lock object that can be disposed in order
        /// to unlock.
        /// </summary>
        /// <returns>The DeviceLock object.</returns>
        public DeviceLock Lock()
        {
            Enter();
            return new DeviceLock(this);
        }

        /// <summary>
        /// Exits thread-safe section; after this call, any state bounds can be rewritten.
        /// </summary>
        public void Exit()
        {
            AssertLocked();
            if (locks == 1)
            {
                ReleaseLocks();

                stateManager.EndFrame();
                devicePerformance.EndFrame();
                device.Exit();
                locks = 0;
                Monitor.Exit(rootSync);
            }
            else
            {
                locks--;
            }
            
        }

        #endregion

        #region States

        /// <summary>
        /// Sets viewports. Only one viewport can be active for all render targets.
        /// </summary>
        /// <param name="views"></param>
        public void SetViewports([NotNull] params Region2i[] views)
        {
            AssertLocked();
            stateManager.ViewportStateChanged();
            device.SetViewports(views);
            this.viewports = views;
        }

        /// <summary>
        /// Obtains viewports.
        /// </summary>
        public Region2i[] Viewports
        {
            [param: NotNull]
            set
            {
                SetViewports(value);
            }
            get
            {
                return viewports.Clone() as Region2i[];
            }
        }

        /// <summary>
        /// Obtains the first (possibly only) viewport.
        /// </summary>
        public Region2i Viewport
        {
            set
            {
                SetViewports(value);
            }
            get
            {
                if (viewports.Length > 1)
                {
                    return viewports[0];
                }
                throw new InvalidOperationException("Viewport not set.");
            }
        }

        /// <summary>
        /// Sets scissor rectanges.
        /// </summary>
        /// <param name="regions"></param>
        public void SetScissorRects(params Region2i[] regions)
        {
            AssertLocked();
            stateManager.ScissorStateChanged();
            scissorRects = regions;
        }

        /// <summary>
        /// Gets or sets the scissor rects.
        /// </summary>
        /// <value>The scissor rects.</value>
        public Region2i[] ScissorRects
        {
            get
            {
                return scissorRects;
            }
            [param: NotNull]
            set
            {
                SetScissorRects(value);
            }
        }

        /// <summary>
        /// Gets or sets the scissor rect.
        /// </summary>
        /// <value>The scissor rect.</value>
        public Region2i ScissorRect
        {
            get
            {
                return ScissorRects[0];
            }
            set
            {
                SetScissorRects(value);
            }
        }

        /// <summary>
        /// Sets the state of the blend.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="blendColour">The blend colour.</param>
        public void SetBlendState(BlendState state, Colour blendColour, uint mask)
        {
            AssertLocked();

            // We want the default state.
            if (state == null)
            {
                state = StateManager.Intern(new BlendState());
            }

            if (!state.IsInterned)
            {
                throw new InvalidOperationException("State must be interned before set.");
            }

            // We enter state.
            if (state != blendState)
            {
                Monitor.Exit(blendState.SyncRoot);
                Monitor.Enter(state.SyncRoot);

                // Force prepared.
                state.Prepare(this);

                blendState = state;

            }

            StateManager.BlendStateChanged();

            this.blendColour = blendColour;
            this.blendMask = mask;
            device.SetBlendState(state.DeviceData, blendColour, mask);
        }

        /// <summary>
        /// Gets the state of the blend.
        /// </summary>
        /// <value>The state of the blend.</value>
        public BlendState BlendState
        {
            get
            {
                return blendState;
            }
        }

        /// <summary>
        /// Gets the blend colour.
        /// </summary>
        /// <value>The blend colour.</value>
        public Colour BlendColour
        {
            get
            {
                return blendColour;
            }
        }

        /// <summary>
        /// Gets the blend mask.
        /// </summary>
        /// <value>The blend mask.</value>
        public uint BlendMask
        {
            get
            {
                return blendMask;
            }
        }

        /// <summary>
        /// Sets the state of the depth stencil.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="stencilRef">The stencil ref.</param>
        public void SetDepthStencilState(DepthStencilState state, uint stencilRef)
        {
            AssertLocked();

            // We want the default state.
            if (state == null)
            {
                state = StateManager.Intern(new DepthStencilState());
            }

            if (!state.IsInterned)
            {
                throw new InvalidOperationException("The state must be interned before set.");
            }

            if (state != depthStencilState)
            {
                Monitor.Exit(depthStencilState.SyncRoot);
                Monitor.Enter(state.SyncRoot);

                // We force binding to device.
                state.Prepare(this);

                depthStencilState = state;
            }

            StateManager.DepthStencilStateChanged();

            this.stencilRef = stencilRef;
            device.SetDepthStencilState(depthStencilState.DeviceData, stencilRef);
        }

        /// <summary>
        /// Gets the state of the depth stencil.
        /// </summary>
        /// <value>The state of the depth stencil.</value>
        public DepthStencilState DepthStencilState
        {
            get
            {
                return depthStencilState;
            }
        }

        /// <summary>
        /// Gets the stencil reference.
        /// </summary>
        /// <value>The stencil reference.</value>
        public uint StencilReference
        {
            get
            {
                return stencilRef;
            }
        }

        /// <summary>
        /// Sets the state of the rasterization.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <remarks>This method is provided for compilance with other states.</remarks>
        public void SetRasterizationState(RasterizationState state)
        {
            AssertLocked();
            
            // We want the default state.
            if (state == null)
            {
                state = StateManager.Intern(new RasterizationState());
            }

            if (!state.IsInterned)
            {
                throw new InvalidOperationException("The state must be interned before set.");
            }

            if (state != rasterizationState)
            {
                Monitor.Exit(rasterizationState.SyncRoot);
                Monitor.Enter(state.SyncRoot);

                // We force binding to device.
                state.Prepare(this);

                rasterizationState = state;
            }

            StateManager.RasterizationStateChanged();

            device.SetRasterizationState(rasterizationState.DeviceData);
        }

        /// <summary>
        /// Gets or sets the state of the rasterization.
        /// </summary>
        /// <value>The state of the rasterization.</value>
        public RasterizationState RasterizationState
        {
            get
            {
                return rasterizationState;
            }
            set
            {
                SetRasterizationState(value);
            }
        }

        /// <summary>
        /// Sets the vertex shader and other constants.
        /// </summary>
        /// <param name="shader">The shader.</param>
        /// <param name="input">The input, can be null.</param>
        /// <param name="samplers">The samplers, can be null.</param>
        /// <param name="textures">The textures, can be null.</param>
        /// <param name="constantBuffers">The constant buffers, can be null.</param>
        public void SetVertexShader(Shaders.VShader shader, Geometry input, 
            SamplerState[] samplers, TextureView[] textures, ConstantBufferView[] constantBuffers)
        {
            AssertLocked();

            // We first decorate arguments.
            if (samplers == null) samplers = new SamplerState[0];
            if (textures == null) textures = new TextureView[0];
            if (constantBuffers == null) constantBuffers = new ConstantBufferView[0];

            // Now we check compatibility.
            int i;
            for (i = 0; i < samplers.Length; i++)
            {
                if (samplers[i] == null || !samplers[i].IsInterned)
                {
                    throw new InvalidOperationException("The sampler state " + i.ToString() + " must be interned before set.");
                }
            }
            for (i = 0; i < textures.Length; i++)
            {
                if (textures[i] == null)
                {
                    throw new ArgumentException("Supplying null texture.");
                }
            }
            for (i = 0; i < constantBuffers.Length; i++)
            {
                if (constantBuffers[i] == null)
                {
                    throw new ArgumentException("Supplying null texture.");
                }
            }

            if(vshader != null) vshader.UnusedByDevice();
            if (shader != null) shader.UsedByDevice();

            // TODO: shader - all compatibility.

            // We unlock previously used states and lock current.

            // Sampler states.
            {
                for (i = 0; i < samplers.Length; i++)
                {
                    // We check if replacing (we do not want ulonk-relock scenarios).
                    if (vertexSamplerStates.Length > i)
                    {
                        if (vertexSamplerStates[i] != samplers[i])
                        {
                            Monitor.Exit(vertexSamplerStates[i].SyncRoot);
                            Monitor.Enter(samplers[i].SyncRoot);

                            samplers[i].Prepare(this);
                        }
                    }
                    else
                    {
                        Monitor.Enter(samplers[i].SyncRoot);

                        samplers[i].Prepare(this);
                    }
                }

                // Release all following
                for (; i < vertexSamplerStates.Length; i++)
                {
                    Monitor.Exit(samplers[i].SyncRoot);
                }
            }

            // Texture states.
            {
                for (i = 0; i < textures.Length; i++)
                {
                    if (vertexTextures.Length > i)
                    {
                        if (vertexTextures[i] != textures[i])
                        {
                            vertexTextures[i].UnusedByDevice();
                            textures[i].UsedByDevice();

                            // Now we create device part if necessary.
                            textures[i].BindToDevice(this);
                        }
                    }
                    else
                    {
                        textures[i].UsedByDevice();

                        // Now we create device part if necessary.
                        textures[i].BindToDevice(this);
                    }
                }
                for (; i < vertexTextures.Length; i++)
                {
                    vertexTextures[i].UnusedByDevice();
                }
            }

            // Constant buffer states.
            {
                for (i = 0; i < constantBuffers.Length; i++)
                {
                    if (vertexCBuffers.Length > i)
                    {
                        if (vertexCBuffers[i] != constantBuffers[i])
                        {
                            vertexCBuffers[i].UnusedByDevice();
                            constantBuffers[i].UsedByDevice();

                            // We make sure it is bound to device.
                            constantBuffers[i].BindToDevice(this);
                        }
                    }
                    else
                    {
                        constantBuffers[i].UsedByDevice();

                        // We make sure it is bound to device.
                        constantBuffers[i].BindToDevice(this);
                    }
                }
                for (; i < vertexCBuffers.Length; i++)
                {
                    vertexCBuffers[i].UsedByDevice();
                }
            }

            // Geometry.
            if (input != inputGeometry)
            {
                if (inputGeometry != null)
                {
                    inputGeometry.UnusedByDevice();
                }

                if (input != null)
                {
                    input.UsedByDevice();
                }
            }

            // Must ensure binding (may ontain internal changed)
            if (input != null)
            {
                input.BindToDevice(this);
                input.BindInputLayout(this);
            }

            // We copy data.
            vertexSamplerStates = (SamplerState[])samplers.Clone();
            vertexTextures = (TextureView[])textures.Clone();
            vertexCBuffers = (ConstantBufferView[])constantBuffers.Clone();
            inputGeometry = input;
            vshader = shader;

            // We send states.
            Driver.ISamplerState[] inSamplers = new Driver.ISamplerState[vertexSamplerStates.Length];
            Driver.ITextureView[] inTextures = new Driver.ITextureView[vertexTextures.Length];
            Driver.ICBufferView[] inConstants = new Driver.ICBufferView[vertexCBuffers.Length];
            
            // We pack some values.
            
            for (i = 0; i < vertexSamplerStates.Length; i++)
            {
                inSamplers[i] = vertexSamplerStates[i].DeviceData;
            }

            for (i = 0; i < vertexTextures.Length; i++)
            {
                inTextures[i] = vertexTextures[i].DeviceData;
            }

            for (i = 0; i < vertexCBuffers.Length; i++)
            {
                inConstants[i] = vertexCBuffers[i].DeviceData;
            }

            if (inputGeometry != null)
            {
                // We pack vertices.
                Driver.IVBufferView[] inVertices = new Driver.IVBufferView[inputGeometry.VertexBufferCount];
                for(uint j = 0; j < inputGeometry.VertexBufferCount; j++)
                {
                    inVertices[j] = inputGeometry[j].DeviceData;

                }

                device.BindVStage(inputGeometry.Topology, inputGeometry.Layout, inVertices, inputGeometry.IndexBuffer != null ? inputGeometry.IndexBuffer.DeviceData : null,
                    vshader != null ? vshader.DriverPart : null, inSamplers, inTextures, inConstants);
            }
            else
            {
                device.BindVStage(Topology.Triangle, null, new Driver.IVBufferView[0], null, vshader != null ? vshader.DriverPart : null,
                    inSamplers, inTextures, inConstants); 
            }

            // We signal state manager
            StateManager.VertexStateChanged();


        }


        /// <summary>
        /// Sets the geometry shader.
        /// </summary>
        /// <param name="shader">The shader, can be null.</param>
        /// <param name="output">The output, can be null.</param>
        /// <param name="samplers">The samplers, can be null.</param>
        /// <param name="textures">The textures, can be null.</param>
        /// <param name="constantBuffers">The constant buffers, can be null.</param>
        public void SetGeometryShader(Shaders.GShader shader, Geometry output,
            SamplerState[] samplers, TextureView[] textures, ConstantBufferView[] constantBuffers)
        {
            AssertLocked();

            // We first decorate arguments.
            if (samplers == null) samplers = new SamplerState[0];
            if (textures == null) textures = new TextureView[0];
            if (constantBuffers == null) constantBuffers = new ConstantBufferView[0];

            // Now we check compatibility.
            int i;
            for (i = 0; i < samplers.Length; i++)
            {
                if (samplers[i] == null || !samplers[i].IsInterned)
                {
                    throw new InvalidOperationException("The sampler state " + i.ToString() + " must be interned before set.");
                }
            }
            for (i = 0; i < textures.Length; i++)
            {
                if (textures[i] == null)
                {
                    throw new ArgumentException("Supplying null texture.");
                }
            }
            for (i = 0; i < constantBuffers.Length; i++)
            {
                if (constantBuffers[i] == null)
                {
                    throw new ArgumentException("Supplying null buffer.");
                }
            }

            if (output != null && !output.IsOutputCompatible)
            {
                throw new ArgumentException("The geometry is not output compatible.");
            }

            if (gshader != null) this.gshader.UnusedByDevice();
            if (shader != null) shader.UsedByDevice();
            // TODO: shader - all compatibility.

            // We unlock previously used states and lock current.

            // Sampler states.
            {
                for (i = 0; i < samplers.Length; i++)
                {
                    // We check if replacing (we do not want ulonk-relock scenarios).
                    if (geometrySamplerStates.Length > i)
                    {
                        if (geometrySamplerStates[i] != samplers[i])
                        {
                            Monitor.Exit(geometrySamplerStates[i].SyncRoot);
                            Monitor.Enter(samplers[i].SyncRoot);

                            samplers[i].Prepare(this);
                        }
                    }
                    else
                    {
                        Monitor.Enter(samplers[i].SyncRoot);

                        samplers[i].Prepare(this);
                    }
                }

                // Release all following
                for (; i < geometrySamplerStates.Length; i++)
                {
                    Monitor.Exit(samplers[i].SyncRoot);
                }
            }

            // Texture states.
            {
                for (i = 0; i < textures.Length; i++)
                {
                    if (geometryTextures.Length > i)
                    {
                        if (geometryTextures[i] != textures[i])
                        {
                            geometryTextures[i].UnusedByDevice();
                            textures[i].UsedByDevice();

                            // Now we create device part if necessary.
                            textures[i].BindToDevice(this);
                        }
                    }
                    else
                    {
                        textures[i].UsedByDevice();

                        // Now we create device part if necessary.
                        textures[i].BindToDevice(this);
                    }
                }
                for (; i < geometryTextures.Length; i++)
                {
                    geometryTextures[i].UnusedByDevice();
                }
            }

            // Constant buffer states.
            {
                for (i = 0; i < constantBuffers.Length; i++)
                {
                    if (geometryCBuffers.Length > i)
                    {
                        if (geometryCBuffers[i] != constantBuffers[i])
                        {
                            geometryCBuffers[i].UnusedByDevice();
                            constantBuffers[i].UsedByDevice();

                            // We make sure it is bound to device.
                            constantBuffers[i].BindToDevice(this);
                        }
                    }
                    else
                    {
                        constantBuffers[i].UsedByDevice();

                        // We make sure it is bound to device.
                        constantBuffers[i].BindToDevice(this);
                    }
                }
                for (; i < geometryCBuffers.Length; i++)
                {
                    geometryCBuffers[i].UsedByDevice();
                }
            }

            // Geometry.
            if (outputGeometry != output)
            {
                if (outputGeometry != null)
                {
                    outputGeometry.UnusedByDevice();
                }

                if (output != null)
                {
                    output.UsedByDevice();

                    output.BindToDevice(this);
                }
            }

            // We copy data.
            geometrySamplerStates = (SamplerState[])samplers.Clone();
            geometryTextures = (TextureView[])textures.Clone();
            geometryCBuffers = (ConstantBufferView[])constantBuffers.Clone();
            outputGeometry = output;
            gshader = shader;

            // We send states.
            Driver.ISamplerState[] inSamplers = new Driver.ISamplerState[geometrySamplerStates.Length];
            Driver.ITextureView[] inTextures = new Driver.ITextureView[geometryTextures.Length];
            Driver.ICBufferView[] inConstants = new Driver.ICBufferView[geometryCBuffers.Length];

            // We pack some values.

            for (i = 0; i < geometrySamplerStates.Length; i++)
            {
                inSamplers[i] = geometrySamplerStates[i].DeviceData;
            }

            for (i = 0; i < geometryTextures.Length; i++)
            {
                inTextures[i] = geometryTextures[i].DeviceData;
            }

            for (i = 0; i < geometryCBuffers.Length; i++)
            {
                inConstants[i] = geometryCBuffers[i].DeviceData;
            }

            if (outputGeometry != null)
            {
                Driver.IVBufferView[] inBuffers = new Driver.IVBufferView[outputGeometry.VertexBufferCount];
                for (i = 0; i < outputGeometry.VertexBufferCount; i++)
                {
                    inBuffers[i] = outputGeometry[(uint)i].DeviceData;
                }

                device.BindGStage(gshader != null ? gshader.DriverPart : null, inSamplers, inTextures, inConstants, outputGeometry.OutLayout, inBuffers);
            }
            else
            {
                device.BindGStage(gshader != null ? gshader.DriverPart : null, inSamplers, inTextures, inConstants,
                    null, new SharpMedia.Graphics.Driver.IVBufferView[0]);

            }

            // We signal state manager
            StateManager.GeometryStateChanged();
        }


        /// <summary>
        /// Sets the pixel shader.
        /// </summary>
        /// <param name="shader">The shader.</param>
        /// <param name="samplers">The samplers, can be null.</param>
        /// <param name="textures">The textures, can be null.</param>
        /// <param name="constantBuffers">The constant buffers, can be null.</param>
        /// <param name="renderTargets">The render targets.</param>
        /// <param name="depthStencilTarget">The depth stencil target, can be null.</param>
        public void SetPixelShader(Shaders.PShader shader,
            SamplerState[] samplers, TextureView[] textures,
            ConstantBufferView[] constantBuffers, RenderTargetView[] renderTargets,
            DepthStencilTargetView depthStencilTarget)
        {
            AssertLocked();

            // We first decorate arguments.
            if (samplers == null) samplers = new SamplerState[0];
            if (textures == null) textures = new TextureView[0];
            if (renderTargets == null) renderTargets = new RenderTargetView[0];
            if (constantBuffers == null) constantBuffers = new ConstantBufferView[0];

            // Now we check compatibility.
            int i;
            for (i = 0; i < samplers.Length; i++)
            {
                if (samplers[i] == null || !samplers[i].IsInterned)
                {
                    throw new InvalidOperationException("The sampler state " + i.ToString() + " must be interned before set.");
                }
            }
            for (i = 0; i < textures.Length; i++)
            {
                if (textures[i] == null)
                {
                    throw new ArgumentException("Supplying null texture.");
                }
            }
            for (i = 0; i < constantBuffers.Length; i++)
            {
                if (constantBuffers[i] == null)
                {
                    throw new ArgumentException("Supplying null texture.");
                }
            }
            for (i = 0; i < renderTargets.Length; i++)
            {
                if (renderTargets[i] == null)
                {
                    throw new ArgumentException("Supplying null render target.");
                }

                if (i > 0)
                {
                    if(renderTargets[i].Width != renderTargets[i-1].Width ||
                       renderTargets[i].Height != renderTargets[i-1].Height ||
                       !renderTargets[i].Format.IsCompatible(renderTargets[i-1].Format))
                    {
                        throw new ArgumentException("Render targets do not match in width, height or format layout.");
                    }
                }
            }

            if (pshader != null) pshader.UnusedByDevice();
            if (shader != null) shader.UsedByDevice();

            // TODO: shader - all compatibility.

            // We unlock previously used states and lock current.

            // Sampler states.
            {
                for (i = 0; i < samplers.Length; i++)
                {
                    // We check if replacing (we do not want ulonk-relock scenarios).
                    if (pixelSamplerStates.Length > i)
                    {
                        if (pixelSamplerStates[i] != samplers[i])
                        {
                            Monitor.Exit(pixelSamplerStates[i].SyncRoot);
                            Monitor.Enter(samplers[i].SyncRoot);

                            samplers[i].Prepare(this);
                        }
                    }
                    else
                    {
                        Monitor.Enter(samplers[i].SyncRoot);

                        samplers[i].Prepare(this);
                    }
                }

                // Release all following
                for (; i < pixelSamplerStates.Length; i++)
                {
                    Monitor.Exit(pixelSamplerStates[i].SyncRoot);
                }
            }

            // Texture states.
            {
                for (i = 0; i < textures.Length; i++)
                {
                    if (pixelTextures.Length > i)
                    {
                        if (pixelTextures[i] != textures[i])
                        {
                            pixelTextures[i].UnusedByDevice();
                            textures[i].UsedByDevice();

                            // Now we create device part if necessary.
                            textures[i].BindToDevice(this);
                        }
                    }
                    else
                    {
                        textures[i].UsedByDevice();

                        // Now we create device part if necessary.
                        textures[i].BindToDevice(this);
                    }
                }
                for (; i < pixelTextures.Length; i++)
                {
                    pixelTextures[i].UnusedByDevice();
                }
            }

            // Constant buffer states.
            {
                for (i = 0; i < constantBuffers.Length; i++)
                {
                    if (pixelCBuffers.Length > i)
                    {
                        if (pixelCBuffers[i] != constantBuffers[i])
                        {
                            pixelCBuffers[i].UnusedByDevice();
                            constantBuffers[i].UsedByDevice();

                            // We make sure it is bound to device.
                            constantBuffers[i].BindToDevice(this);
                        }
                    }
                    else
                    {
                        constantBuffers[i].UsedByDevice();

                        // We make sure it is bound to device.
                        constantBuffers[i].BindToDevice(this);
                    }
                }
                for (; i < pixelCBuffers.Length; i++)
                {
                    pixelCBuffers[i].UsedByDevice();
                }
            }

            // Render targets.
            {
                for (i = 0; i < renderTargets.Length; i++)
                {
                    if(pixelRenderTargets.Length > i)
                    {
                        if(pixelRenderTargets[i] != renderTargets[i])
                        {
                            pixelRenderTargets[i].UnusedByDevice();
                            renderTargets[i].UsedByDevice();

                            // Make sure it is bound.
                            renderTargets[i].BindToDevice(this);
                        }
                    } else {
                        renderTargets[i].UsedByDevice();
                    }
                }
                for(; i<  pixelRenderTargets.Length; i++)
                {
                    pixelRenderTargets[i].UnusedByDevice();
                }
            }

            // Depth stencil.
            if(depthStencilTarget != pixelDepthStencilTarget)
            {
                if(pixelDepthStencilTarget != null)
                {
                    pixelDepthStencilTarget.UnusedByDevice();
                }

                if(depthStencilTarget != null)
                {
                    depthStencilTarget.UsedByDevice();
                }
            }

            // We copy data.
            pixelSamplerStates = (SamplerState[])samplers.Clone();
            pixelTextures = (TextureView[])textures.Clone();
            pixelCBuffers = (ConstantBufferView[])constantBuffers.Clone();
            pixelRenderTargets = (RenderTargetView[])renderTargets.Clone();
            pixelDepthStencilTarget = depthStencilTarget;
            pshader = shader;

            // We send states.
            Driver.ISamplerState[] inSamplers = new Driver.ISamplerState[pixelSamplerStates.Length];
            Driver.ITextureView[] inTextures = new Driver.ITextureView[pixelTextures.Length];
            Driver.ICBufferView[] inConstants = new Driver.ICBufferView[pixelCBuffers.Length];
            Driver.IRenderTargetView[] inRTs = new Driver.IRenderTargetView[pixelRenderTargets.Length];

            // We pack some values.

            for (i = 0; i < pixelSamplerStates.Length; i++)
            {
                inSamplers[i] = pixelSamplerStates[i].DeviceData;
            }

            for (i = 0; i < pixelTextures.Length; i++)
            {
                inTextures[i] = pixelTextures[i].DeviceData;
            }

            for (i = 0; i < pixelCBuffers.Length; i++)
            {
                inConstants[i] = pixelCBuffers[i].DeviceData;
            }

            for(i = 0; i < pixelRenderTargets.Length; i++)
            {
                inRTs[i] = pixelRenderTargets[i].DeviceData;
            }


            device.BindPStage(pshader != null ? pshader.DriverPart : null, inSamplers, inTextures, inConstants, inRTs,
                depthStencilTarget != null ? depthStencilTarget.DeviceData : null);


            // We signal state manager
            StateManager.PixelStateChanged();
        }

        /// <summary>
        /// Clears all states, unbinding all resources.
        /// </summary>
        public void ClearStates()
        {
            AssertLocked();

            ClearStatesInternal();
        }


        #endregion

        #region Rendering

        /// <summary>
        /// Clears the render target.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="colour">The colour.</param>
        public void Clear([NotNull] RenderTargetView view, Colour colour)
        {
            AssertLocked();

            // We quickly bind.
            try
            {
                view.UsedByDevice();

                // Make sure it is bound.
                view.BindToDevice(this);

                device.Clear(view.DeviceData, colour);
            }
            finally
            {
                view.UnusedByDevice();
            }
        }

        /// <summary>
        /// Clears depth/stencil target.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="options">The options.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="stencil">The stencil.</param>
        public void Clear([NotNull] DepthStencilTargetView view, ClearOptions options, float depth, uint stencil)
        {
            AssertLocked();

            // We quickly bind.
            try
            {
                view.UsedByDevice();

                // Make sure it is bound.
                view.BindToDevice(this);

                device.Clear(view.DeviceData, options, depth, stencil);
            }
            finally
            {
                view.UnusedByDevice();
            }
        }

        /// <summary>
        /// Draws entire buffer.
        /// </summary>
        public void DrawAuto()
        {
            AssertLocked();
            DrawValidate();

            // We draw.
            device.DrawAuto();

            // Unknown ...
        }

        /// <summary>
        /// Draws from offset to offset+length.
        /// </summary>
        /// <param name="off">The off.</param>
        /// <param name="lenght">The lenght.</param>
        public void Draw(ulong off, ulong lenght)
        {
            AssertLocked();
            DrawValidate();

            // We also validate for out of range.
            if (!inputGeometry.IsInRange(off, lenght))
            {
                throw new ArgumentException("Trying to render out of range.");
            }

            // We draw.
            device.Draw(off, lenght);

            DevicePerformance.RenderData(inputGeometry.Topology, lenght);
        }

        /// <summary>
        /// Draws indexed.
        /// </summary>
        /// <param name="off">The off.</param>
        /// <param name="length">The length.</param>
        /// <param name="baseIndex">Index of the base.</param>
        public void DrawIndexed(ulong off, ulong length, long baseIndex)
        {
            AssertLocked();
            DrawValidate();

            // We also validate for out of range.
            if (!inputGeometry.IsInRange(off, length, baseIndex))
            {
                throw new ArgumentException("Trying to render out of range.");
            }

            // We draw.
            device.DrawIndexed(off, length, baseIndex);

            DevicePerformance.RenderData(inputGeometry.Topology, length);
        }

        /// <summary>
        /// Draw instanced.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <param name="instanceOffset">The instance offset.</param>
        /// <param name="instanceCount">The instance count.</param>
        public void Draw(ulong offset, ulong count,
                  uint instanceOffset, uint instanceCount)
        {
            AssertLocked();
            DrawValidate();

            // We also validate for out of range.
            if (!inputGeometry.IsInRange(offset, count, instanceOffset, instanceCount))
            {
                throw new ArgumentException("Trying to render out of range.");
            }

            // We draw.
            device.Draw(offset, count, instanceOffset, instanceCount);

            DevicePerformance.RenderData(inputGeometry.Topology, count * instanceCount);
        }

        /// <summary>
        /// Draw instanced and indexed.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <param name="baseIndex">Index of the base.</param>
        /// <param name="instanceOffset">The instance offset.</param>
        /// <param name="instanceCount">The instance count.</param>
        public void DrawIndexed(ulong offset, ulong count, long baseIndex,
                  uint instanceOffset, uint instanceCount)
        {
            AssertLocked();
            DrawValidate();

            // We also validate for out of range.
            if (!inputGeometry.IsInRange(offset, count, instanceOffset, instanceCount))
            {
                throw new ArgumentException("Trying to render out of range.");
            }

            // We draw.
            device.DrawIndexed(offset, count, baseIndex);

            DevicePerformance.RenderData(inputGeometry.Topology, count * instanceCount);
        }

        #endregion

        #region IDisposable Members

        private void Dispose(bool fin)
        {
            AssertNotLocked();

            bool wasDisposed = true;
            lock (rootSync)
            {
                if (!isDisposed)
                {
                    isDisposed = true;
                    wasDisposed = false;
                    device.UnRegisterListener();

                    device.Dispose();
                    device = null;

                    if (defaultRenderTarget != null)
                    {
                        defaultRenderTarget.Dispose();
                    }

                    if (defaultDepthStencilTarget != null)
                    {
                        defaultDepthStencilTarget.Dispose();
                    }

                    if (!fin) GC.SuppressFinalize(this);
                }
            }

            // Only fire on first disposing.
            if (!wasDisposed)
            {
                Action<GraphicsDevice> e = deviceDisposing;
                if (e != null)
                {
                    // Only pass event argument if *not* in finalizer.
                    e(this);
                }
            }
        }

        public void Dispose()
        {
            Dispose(false);
        }

        #endregion
    }
}
