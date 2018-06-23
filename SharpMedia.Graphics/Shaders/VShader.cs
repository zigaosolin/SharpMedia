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
using System.Threading;

namespace SharpMedia.Graphics.Shaders
{
    /// <summary>
    /// A vertex shader.
    /// </summary>
    public sealed class VShader : IShader
    {
        #region Private Members
        Driver.IVShader driverPart;
        FixedShaderParameters fixedParams;

        object syncRoot = new object();
        int usedByDevice = 0;
        CacheableState state;
        Action<ICacheable> onTouch;
        #endregion

        #region Internal Methods


        void Dispose(bool fin)
        {
            
                state = CacheableState.Disposed;
                if (driverPart != null)
                {
                    driverPart.Dispose();
                    driverPart = null;
                }

                if (!fin)
                {
                    GC.SuppressFinalize(this);
                }
                else
                {
                    Common.NotDisposed(this, "If shader is created manually, you must dispose it, otherwise dispose the ShaderCode.");
                }
            
        }

        ~VShader()
        {
            Dispose(true);
        }

        /// <summary>
        /// Constructor from device.
        /// </summary>
        /// <param name="d">The ShaderCode.</param>
        /// <param name="s">The shader.</param>
        internal VShader(FixedShaderParameters f, Driver.IVShader s)
        {
            driverPart = s;
            fixedParams = f;
        }

        /// <summary>
        /// Driver part of the shader, used by device when binding.
        /// </summary>
        internal Driver.IVShader DriverPart 
        {
            get { return driverPart; }
        }

        /// <summary>
        /// Useds the by device.
        /// </summary>
        internal void UsedByDevice()
        {
            if (state == CacheableState.Disposed) throw new ObjectDisposedException("Shader disposed, cannot be bound.");

            if (usedByDevice == 0)
            {
                Monitor.Enter(syncRoot);
            }

            usedByDevice++;
        }

        /// <summary>
        /// Unuseds the by device.
        /// </summary>
        internal void UnusedByDevice()
        {
            if (--usedByDevice == 0)
            {
                if (state == CacheableState.Evicted)
                {
                    Dispose();
                }

                Monitor.Exit(syncRoot);
            }
        }

        #endregion

        #region IShader Members

        ShaderCode IShader.Source
        {
            get { return fixedParams.ShaderCode; }
        }

        public FixedShaderParameters FixedParameters
        {
            get { return fixedParams; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (syncRoot)
            {
                Dispose(false);
            }
        }

        #endregion

        #region ICacheable Members

        void SharpMedia.Caching.ICacheable.Cached()
        {
            lock (syncRoot)
            {
                if (state == CacheableState.Disposed) throw new ObjectDisposedException("Cannot re-claim a vertex shader.");
                state = CacheableState.Normal;
            }
        }

        void SharpMedia.Caching.ICacheable.Evict()
        {
            // We treat evict as dispose.
            lock (syncRoot)
            {
                if (usedByDevice == 0) Dispose();
                else
                {
                    state = CacheableState.Evicted;
                }
            }
        }

        event Action<SharpMedia.Caching.ICacheable> SharpMedia.Caching.ICacheable.OnTouch
        {
            add 
            {
                lock (syncRoot)
                {
                    onTouch += value;
                }
            }
            remove 
            {
                lock (syncRoot)
                {
                    onTouch -= value;
                }
            }
        }

        CacheableState ICacheable.State
        {
            get 
            {
                return state;
            }
        }

        void SharpMedia.Caching.ICacheable.Touch()
        {
            Action<ICacheable> t = onTouch;
            if (t != null)
            {
                t(this);
            }
        }

        #endregion

    }
}
