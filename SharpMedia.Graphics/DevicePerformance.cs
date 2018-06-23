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
    /// A device performance class tracks dynamic performance.
    /// </summary>
    [Serializable]
    public sealed class DevicePerformance
    {
        #region Private Members
        GraphicsDevice device;

        // Full data.
        ulong frameCount = 0;
        TimeSpan fullSpan = new TimeSpan(0);
        ulong trianglesRenderered = 0;
        ulong pointsRendered = 0;
        ulong linesRendered = 0;
        ulong drawCalls = 0;
        
        // Current data.
        DateTime lastFrame = DateTime.Now;
        TimeSpan lastFrameSpan;
        ulong lastTrianglesRendered = 0;
        ulong lastPointsRendered = 0;
        ulong lastLinesRendered = 0;
        uint lastDrawCalls = 0;

        // Maximum/minimum.
        ulong maxTrianglesRendered = 0;
        ulong maxPointsRendered = 0;
        ulong maxLinesRendered = 0;
        ulong minTrianglesRendered = ulong.MaxValue;
        ulong minPointsRendered = ulong.MaxValue;
        ulong minLinesRendered = ulong.MaxValue;
        uint maxRenderCalls = 0;
        uint minRenderCalls = uint.MaxValue;
        
        TimeSpan maxFrameLenght = new TimeSpan(0);
        TimeSpan minFrameLenght = TimeSpan.MaxValue;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the total triangles rendered.
        /// </summary>
        /// <value>The total triangles rendered.</value>
        public ulong TotalTrianglesRendered
        {
            get
            {
                return trianglesRenderered;
            }
        }

        /// <summary>
        /// Gets the total points rendered.
        /// </summary>
        /// <value>The total points rendered.</value>
        public ulong TotalPointsRendered
        {
            get
            {
                return pointsRendered;
            }
        }

        /// <summary>
        /// Gets the total lines rendered.
        /// </summary>
        /// <value>The total lines rendered.</value>
        public ulong TotalLinesRendered
        {
            get
            {
                return linesRendered;
            }
        }

        /// <summary>
        /// Gets the triangles per second.
        /// </summary>
        /// <value>The triangles per second.</value>
        public ulong TrianglesPerSecond
        {
            get
            {
                return (ulong)(trianglesRenderered / fullSpan.TotalSeconds);
            }
        }

        /// <summary>
        /// Gets the lines per second.
        /// </summary>
        /// <value>The lines per second.</value>
        public ulong LinesPerSecond
        {
            get
            {
                return (ulong)(linesRendered / fullSpan.TotalSeconds);
            }
        }

        /// <summary>
        /// Gets the points per second.
        /// </summary>
        /// <value>The points per second.</value>
        public ulong PointsPerSecond
        {
            get
            {
                return (ulong)(pointsRendered / fullSpan.TotalSeconds);
            }
        }

        /// <summary>
        /// Gets the max triangles per frame.
        /// </summary>
        /// <value>The max triangles per frame.</value>
        public ulong MaxTrianglesPerFrame
        {
            get
            {
                return maxTrianglesRendered;
            }
        }

        /// <summary>
        /// Gets the min triangles per frame.
        /// </summary>
        /// <value>The min triangles per frame.</value>
        public ulong MinTrianglesPerFrame
        {
            get
            {
                return minTrianglesRendered;
            }
        }

        /// <summary>
        /// Gets the max lines per frame.
        /// </summary>
        /// <value>The max lines per frame.</value>
        public ulong MaxLinesPerFrame
        {
            get
            {
                return maxLinesRendered;
            }
        }

        /// <summary>
        /// Gets the min lines per frame.
        /// </summary>
        /// <value>The min lines per frame.</value>
        public ulong MinLinesPerFrame
        {
            get
            {
                return minLinesRendered;
            }
        }

        /// <summary>
        /// Gets the max points per frame.
        /// </summary>
        /// <value>The max points per frame.</value>
        public ulong MaxPointsPerFrame
        {
            get
            {
                return maxPointsRendered;
            }
        }

        /// <summary>
        /// Gets the min points per frame.
        /// </summary>
        /// <value>The min points per frame.</value>
        public ulong MinPointsPerFrame
        {
            get
            {
                return minPointsRendered;
            }
        }

        /// <summary>
        /// Gets the total rendering span.
        /// </summary>
        /// <value>The total rendering span.</value>
        public TimeSpan TotalRenderingTime
        {
            get
            {
                return fullSpan;
            }
        }


        /// <summary>
        /// Gets the average frame render time.
        /// </summary>
        /// <value>The average frame render time.</value>
        public TimeSpan AverageFrameRenderTime
        {
            get
            {
                return new TimeSpan(fullSpan.Ticks / (long)frameCount);
            }
        }

        /// <summary>
        /// Gets the minimum frame render time.
        /// </summary>
        /// <value>The minimum frame render time.</value>
        public TimeSpan MinimumFrameRenderTime
        {
            get
            {
                return minFrameLenght;
            }
        }

        /// <summary>
        /// Gets the maximum frame render time.
        /// </summary>
        /// <value>The maximum frame render time.</value>
        public TimeSpan MaximumFrameRenderTime
        {
            get
            {
                return maxFrameLenght;
            }
        }

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
        /// Gets the current FPS.
        /// </summary>
        /// <value>The current FPS.</value>
        public float CurrentFPS
        {
            get
            {
                return 1.0f / (float)lastFrameSpan.TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the average FPS.
        /// </summary>
        /// <value>The average FPS.</value>
        public float AverageFPS
        {
            get
            {
                return (float)frameCount / (float)fullSpan.TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the maximum FPS.
        /// </summary>
        /// <value>The maximum FPS.</value>
        public float MaximumFPS
        {
            get
            {
                return 1.0f / (float)minFrameLenght.TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the minimum FPS.
        /// </summary>
        /// <value>The minimum FPS.</value>
        public float MinimumFPS
        {
            get
            {
                return 1.0f / (float)maxFrameLenght.TotalSeconds;
            }
        }

        /// <summary>
        /// Maximum render calls per frame.
        /// </summary>
        public uint MaxRenderCalls
        {
            get
            {
                return maxRenderCalls;
            }
        }

        /// <summary>
        /// Minimum render calls per frame.
        /// </summary>
        public uint MinRenderCalls
        {
            get
            {
                return minRenderCalls;
            }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            fullSpan = new TimeSpan(0);
            frameCount = 0;
            trianglesRenderered = 0;
            pointsRendered = 0;
            linesRendered = 0;
            drawCalls = 0;

            maxTrianglesRendered = 0;
            maxPointsRendered = 0;
            maxLinesRendered = 0;
            minTrianglesRendered = ulong.MaxValue;
            minPointsRendered = ulong.MaxValue;
            minLinesRendered = ulong.MaxValue;
            maxRenderCalls = 0;
            minRenderCalls = uint.MaxValue;

            maxFrameLenght = new TimeSpan(0);
            minFrameLenght = TimeSpan.MaxValue;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// A system performance.
        /// </summary>
        internal DevicePerformance(GraphicsDevice device)
        {
            this.device = device;
        }

        /// <summary>
        /// Begins the frame.
        /// </summary>
        internal void BeginFrame()
        {
            lastFrame = DateTime.Now;
            lastPointsRendered = lastTrianglesRendered = lastLinesRendered = 0;
            lastDrawCalls = 0;
        }

        /// <summary>
        /// Ends the frame.
        /// </summary>
        internal void EndFrame()
        {
            DateTime time = DateTime.Now;
            lastFrameSpan = time - lastFrame;

            // We update full data.
            frameCount++;
            fullSpan += lastFrameSpan;
            drawCalls += lastDrawCalls;
            trianglesRenderered += lastTrianglesRendered;
            pointsRendered += lastPointsRendered;
            linesRendered += lastPointsRendered;

            // We now the interval.
            if (lastFrameSpan < minFrameLenght)
            {
                minFrameLenght = lastFrameSpan;
            }
            if (lastFrameSpan > maxFrameLenght)
            {
                maxFrameLenght = lastFrameSpan;
            }

            if (lastDrawCalls > maxRenderCalls) maxRenderCalls = lastDrawCalls;
            if (lastDrawCalls < minRenderCalls) minRenderCalls = lastDrawCalls;

            // We update min-max
            if (lastPointsRendered > maxPointsRendered) maxPointsRendered = lastPointsRendered;
            if (lastLinesRendered > maxLinesRendered) maxLinesRendered = lastLinesRendered;
            if (lastTrianglesRendered > maxTrianglesRendered) maxTrianglesRendered = lastTrianglesRendered;
            if (lastTrianglesRendered < minTrianglesRendered) minTrianglesRendered = lastTrianglesRendered;
            if (lastLinesRendered < minLinesRendered) minLinesRendered = lastLinesRendered;
            if (lastPointsRendered < minPointsRendered) minPointsRendered = lastPointsRendered;

            lastDrawCalls = 0;
            lastPointsRendered = 0;
            lastLinesRendered = 0;
            lastTrianglesRendered = 0;
        }

        /// <summary>
        /// Renders the data.
        /// </summary>
        /// <param name="topology">The topology.</param>
        /// <param name="count">The count.</param>
        internal void RenderData(Topology topology, ulong count)
        {
            // Zero draw invalid.
            if (count == 0) return;

            lastDrawCalls++;

            switch (topology)
            {
                case Topology.Point:
                    lastPointsRendered += count;
                    break;
                case Topology.Line:
                    lastLinesRendered += count / 2;
                    break;
                case Topology.LineStrip:
                    lastLinesRendered += count - 1;
                    break;
                case Topology.Triangle:
                    lastTrianglesRendered += count / 3;
                    break;
                case Topology.TriangleStrip:
                    if (count <= 2) break;
                    lastTrianglesRendered += count - 2;
                    break;
            }
            
        }

        #endregion
    }
}
