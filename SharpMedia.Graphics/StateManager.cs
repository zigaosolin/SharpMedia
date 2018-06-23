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
using System.Collections;
using System.Threading;
using SharpMedia.Graphics.Implementation;
using SharpMedia.Graphics.States;

namespace SharpMedia.Graphics
{
    /// <summary>
    /// The state manager class.
    /// </summary>
    public sealed class StateManager
    {
        #region Private Static Members
        const uint HashSize = GraphicsDevice.MaxStateObjectsPerType / 8;
        static object syncRoot = new object();
        static Hashtable<BlendState> blendStateHash = new Hashtable<BlendState>(HashSize);
        static Hashtable<RasterizationState> rasterizationStateHash = new Hashtable<RasterizationState>(HashSize);
        static Hashtable<SamplerState> samplerStateHash = new Hashtable<SamplerState>(HashSize);
        static Hashtable<DepthStencilState> depthStencilStateHash = new Hashtable<DepthStencilState>(HashSize);
        #endregion

        #region Static Members

        /// <summary>
        /// Performs a forced "state collection". This does not make states not
        /// interned, only their "device part" is cleared. It is automatically recreated
        /// by GraphicsDevice when necessary.
        /// </summary>
        public static void Collect()
        {
            lock(syncRoot)
            {
                // We do state cleanup.
                foreach (BlendState bState in blendStateHash)
                {
                    // Cleanup only not locked (locked are only those used by device).
                    if(Monitor.TryEnter(bState.SyncRoot))
                    {
                        if (bState.DeviceData != null)
                        {
                            bState.DeviceData.Dispose();
                            bState.DeviceData = null;
                        }
                        Monitor.Exit(bState.SyncRoot);
                    }
                }

                // We do state cleanup.
                foreach (RasterizationState rState in rasterizationStateHash)
                {
                    // Cleanup only not locked (locked are only those used by device).
                    if (Monitor.TryEnter(rState.SyncRoot))
                    {
                        if (rState.DeviceData != null)
                        {
                            rState.DeviceData.Dispose();
                            rState.DeviceData = null;
                        }
                        Monitor.Exit(rState.SyncRoot);
                    }
                }

                // We do state cleanup.
                foreach (DepthStencilState dState in depthStencilStateHash)
                {
                    // Cleanup only not locked (locked are only those used by device).
                    if (Monitor.TryEnter(dState.SyncRoot))
                    {
                        if (dState.DeviceData != null)
                        {
                            dState.DeviceData.Dispose();
                            dState.DeviceData = null;
                        }
                        Monitor.Exit(dState.SyncRoot);
                    }
                }

                // We do state cleanup.
                foreach (SamplerState sState in samplerStateHash)
                {
                    // Cleanup only not locked (locked are only those used by device).
                    if (Monitor.TryEnter(sState.SyncRoot))
                    {
                        if (sState.DeviceData != null)
                        {
                            sState.DeviceData.Dispose();
                            sState.DeviceData = null;
                        }
                        Monitor.Exit(sState.SyncRoot);
                    }
                }

            }
        }

        /// <summary>
        /// Interns the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>Interned state; this state is shared and cannot be disposed.</returns>
        public static BlendState Intern(BlendState state)
        {
            // No checks for interned state.
            if (state.IsInterned) return state;

            lock (syncRoot)
            {
                BlendState internedPart = blendStateHash.IsInterned(state);

                if (internedPart == null)
                {
                    state.Intern();
                    blendStateHash.Add(state);
                    return state;
                }

                return internedPart;
            }
        }

        /// <summary>
        /// Interns the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public static RasterizationState Intern(RasterizationState state)
        {
            // No checks for interned state.
            if (state.IsInterned) return state;

            lock (syncRoot)
            {
                RasterizationState internedPart = rasterizationStateHash.IsInterned(state);

                if (internedPart == null)
                {
                    state.Intern();
                    rasterizationStateHash.Add(state);
                    return state;
                }

                return internedPart;
            }
        }

        /// <summary>
        /// Interns the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public static DepthStencilState Intern(DepthStencilState state)
        {
            // No checks for interned state.
            if (state.IsInterned) return state;

            lock (syncRoot)
            {
                DepthStencilState internedPart = depthStencilStateHash.IsInterned(state);

                if (internedPart == null)
                {
                    state.Intern();
                    depthStencilStateHash.Add(state);
                    return state;
                }

                return internedPart;
            }
        }

        /// <summary>
        /// Interns the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public static SamplerState Intern(SamplerState state)
        {
            // No checks for interned state.
            if (state.IsInterned) return state;

            lock (syncRoot)
            {
                SamplerState internedPart = samplerStateHash.IsInterned(state);

                if (internedPart == null)
                {
                    state.Intern();
                    samplerStateHash.Add(state);
                    return state;
                }

                return internedPart;
            }
        }

        #endregion

        #region Private Members
        GraphicsDevice device;
        ulong frameCount = 1;
        StateChangeGroup blendState;
        StateChangeGroup depthStencilState;
        StateChangeGroup rasterizationState;
        StateChangeGroup viewportState;
        StateChangeGroup scissorRectState;
        StateChangeGroup vertexState;
        StateChangeGroup geometryState;
        StateChangeGroup pixelState;

        internal void BeginFrame()
        {
        }

        internal void BlendStateChanged()
        {
            blendState.Changed();
        }

        internal void DepthStencilStateChanged()
        {
            depthStencilState.Changed();
        }

        internal void RasterizationStateChanged()
        {
            rasterizationState.Changed();
        }

        internal void ViewportStateChanged()
        {
            viewportState.Changed();
        }

        internal void ScissorStateChanged()
        {
            scissorRectState.Changed();
        }

        internal void VertexStateChanged()
        {
            vertexState.Changed();
        }

        internal void GeometryStateChanged()
        {
            geometryState.Changed();
        }

        internal void PixelStateChanged()
        {
            pixelState.Changed();
        }

        internal void EndFrame()
        {
            frameCount++;

            blendState.Update();
            depthStencilState.Update();
            rasterizationState.Update();
            viewportState.Update();
            scissorRectState.Update();
            vertexState.Update();
            geometryState.Update();
            pixelState.Update();
        }

        // TODO: actual changes.

        internal StateManager(GraphicsDevice device)
        {
            this.device = device;
            Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the min blend state changes.
        /// </summary>
        /// <value>The min blend state changes.</value>
        public uint MinBlendStateChanges
        {
            get
            {
                return blendState.MinPerFrame;
            }
        }

        /// <summary>
        /// Gets the max blend state changes.
        /// </summary>
        /// <value>The max blend state changes.</value>
        public uint MaxBlendStateChanges
        {
            get
            {
                return blendState.MaxPerFrame;
            }
        }

        /// <summary>
        /// Gets all blend state changes.
        /// </summary>
        /// <value>All blend state changes.</value>
        public ulong AllBlendStateChanges
        {
            get
            {
                return blendState.All;
            }
        }

        /// <summary>
        /// Gets the average blend state changes.
        /// </summary>
        /// <value>The average blend state changes.</value>
        public uint AverageBlendStateChanges
        {
            get
            {
                return blendState.Average(frameCount);
            }
        }

        // TODO:

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets this state changes tracker.
        /// </summary>
        public void Reset()
        {
            frameCount = 1; // To avoid divide by 0, does not "affect" average too much.
            blendState = new StateChangeGroup();
            depthStencilState = new StateChangeGroup();
            rasterizationState = new StateChangeGroup();
            viewportState = new StateChangeGroup();
            scissorRectState = new StateChangeGroup();
            vertexState = new StateChangeGroup();
            geometryState = new StateChangeGroup();
            pixelState = new StateChangeGroup();

            blendState.Reset();
            depthStencilState.Reset();
            rasterizationState.Reset();
            viewportState.Reset();
            scissorRectState.Reset();
            vertexState.Reset();
            geometryState.Reset();
            pixelState.Reset();

        }

        #endregion
    }
}
