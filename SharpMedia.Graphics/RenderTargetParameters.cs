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
using System.Reflection;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics
{


    /// <summary>
    /// A default render target parameters.
    /// </summary>
    [Serializable]
    public class RenderTargetParameters
    {
        #region Private Parameters
        CommonPixelFormatLayout depthStencilFormat = CommonPixelFormatLayout.D24_UNORM_S8_UINT;
        CommonPixelFormatLayout bufferFormat = CommonPixelFormatLayout.X8Y8Z8W8_UNORM;
        uint backBufferCount = 1;
        uint width = 0;
        uint height = 0;
        uint multiSampleType = 1;
        uint multiSampleQuality = 0;
        bool windowed = true;
        uint refreshRate = 75;
        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SwapChainParameters"/> is windowed.
        /// </summary>
        /// <value><c>true</c> if windowed; otherwise, <c>false</c>.</value>
        public bool Windowed
        {
            get
            {
                return windowed;
            }
            set
            {
                windowed = value;
            }
        }

        /// <summary>
        /// Gets or sets the depth stencil format.
        /// </summary>
        /// <value>The depth stencil format.</value>
        public PixelFormat DepthStencilFormat
        {
            get
            {
                return PixelFormat.FromCommonLayout(depthStencilFormat);
            }
            [param: NotNull]
            set
            {
                depthStencilFormat = value.CommonFormatLayout;
                if (depthStencilFormat == CommonPixelFormatLayout.NotCommonLayout)
                {
                    throw new InvalidPixelFormatException("The pixel format is not common and can thus " +
                        " not be used with hardware acceleration.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the depth stencil common format.
        /// </summary>
        /// <value>The depth stencil common format.</value>
        public CommonPixelFormatLayout DepthStencilCommonFormat
        {
            get
            {
                return depthStencilFormat;
            }
            set
            {
                depthStencilFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public PixelFormat Format
        {
            get
            {
                return PixelFormat.FromCommonLayout(bufferFormat);
            }
            [param: NotNull]
            set
            {
                bufferFormat = value.CommonFormatLayout;
                if (bufferFormat == CommonPixelFormatLayout.NotCommonLayout)
                {
                    throw new InvalidPixelFormatException("The pixel format is not common and can thus " +
                        " not be used with hardware acceleration.");
                }

            }
        }

        /// <summary>
        /// Gets or sets the format common.
        /// </summary>
        /// <value>The format common.</value>
        public CommonPixelFormatLayout FormatCommon
        {
            get
            {
                return bufferFormat;
            }
            set
            {
                bufferFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the back buffer count.
        /// </summary>
        /// <value>The back buffer count.</value>
        public uint BackBufferCount
        {
            get
            {
                return backBufferCount;
            }
            set
            {
                backBufferCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the back buffer.
        /// </summary>
        /// <value>The width of the back buffer.</value>
        public uint BackBufferWidth
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the back buffer.
        /// </summary>
        /// <value>The height of the back buffer.</value>
        public uint BackBufferHeight
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the multi sample, ranging from 1 (no multisample) to 15.
        /// </summary>
        /// <value>The type of the multi sample.</value>
        public uint MultiSampleType
        {
            get
            {
                return multiSampleType;
            }
            [param: MinUInt(1)]
            set
            {
                multiSampleType = value;
            }
        }

        /// <summary>
        /// Gets or sets the multi sample quality.
        /// </summary>
        /// <value>The multi sample quality.</value>
        public uint MultiSampleQuality
        {
            get
            {
                return multiSampleQuality;
            }
            set
            {
                multiSampleQuality = value;
            }
        }

        /// <summary>
        /// Gets or sets the refresh rate.
        /// </summary>
        /// <value>The refresh rate.</value>
        public uint RefreshRate
        {
            get
            {
                return refreshRate;
            }
            set
            {
                refreshRate = value;
            }
        }

        #endregion
    }
}
