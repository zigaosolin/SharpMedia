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

namespace SharpMedia.Graphics
{
    /// <summary>
    /// Shader manager tracks the shaders.
    /// </summary>
    public sealed class ShaderManager
    {
        #region Private Members
        GraphicsDevice device;
        ulong compiledShaderCount = 0;
        uint activeShaders = 0;
        TimeSpan compilationTime = new TimeSpan(0);
        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderManager"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal ShaderManager(GraphicsDevice device)
        {
            this.device = device;
        }

        /// <summary>
        /// Adds the shader compiled.
        /// </summary>
        internal void AddShaderCompiled(TimeSpan compileTime)
        {
            compiledShaderCount++;
            activeShaders++;
            compilationTime += compileTime;
        }

        /// <summary>
        /// Removes the shader compiled.
        /// </summary>
        internal void RemoveShader()
        {
            activeShaders--;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <value>The device.</value>
        public GraphicsDevice Device
        {
            get
            {
                return device;
            }
        }

        /// <summary>
        /// Returns number of shaders compiled.
        /// </summary>
        public ulong CompiledShadersCount
        {
            get
            {
                return compiledShaderCount;
            }
        }

        /// <summary>
        /// Gets the active shaders count.
        /// </summary>
        /// <value>The active shaders.</value>
        public uint ShaderCount
        {
            get
            {
                return activeShaders;
            }
        }

        /// <summary>
        /// Gets the total compilation time.
        /// </summary>
        /// <value>The total compilation time.</value>
        public TimeSpan TotalCompilationTime
        {
            get
            {
                return compilationTime;
            }
        }

        #endregion

    }
}
