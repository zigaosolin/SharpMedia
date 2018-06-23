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
using SharpMedia.Math.Shapes.Compounds;
using SharpMedia.Caching;
using SharpMedia.Math.Shapes.Storage;
using SharpMedia.Math.Shapes.Algorithms;

namespace SharpMedia.Graphics.Vector.Fonts
{

    /// <summary>
    /// Glyph caching options.
    /// </summary>
    [Flags]
    public enum GlyphCacheOptions
    {
        /// <summary>
        /// No cache is used.
        /// </summary>
        NoCache = 0,

        /// <summary>
        /// Shapes are cached (this flag is always recommended).
        /// </summary>
        CacheShapes = 1,

        /// <summary>
        /// Outlines are cached (recommended only if using outline extensivelly).
        /// </summary>
        CacheOutlines = 2,

        /// <summary>
        /// Caching has resolution tolerance; this means that required resolution
        /// is worse in quality that already cached one and that they do not differ
        /// too much (less than 50%), than that resolution will be used.
        /// </summary>
        /// <remarks>We do not recommend this for font scaling themes but otherwise it
        /// works ok.</remarks>
        ResolutionTolerance = 4
    }

    /// <summary>
    /// A glyph, containing information about glyph and cached data.
    /// </summary>
    public sealed class Glyph
    {
        #region Private Classes

        /// <summary>
        /// A glyph per-resolution caching.
        /// </summary>
        private class GlyphShapeCache : ICacheable
        {
            #region Private Members
            TriangleSoup2f soup;
            Action<ICacheable> onTouch;
            CacheableState state;
            #endregion

            #region Constructors

            /// <summary>
            /// A glyph shape cache.
            /// </summary>
            /// <param name="soup"></param>
            public GlyphShapeCache(TriangleSoup2f soup)
            {
                this.soup = soup;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Obtains the data.
            /// </summary>
            public TriangleSoup2f Data
            {
                get { return soup; }
            }

            #endregion

            #region ICacheable Members

            public void Cached()
            {
                if (state == CacheableState.Normal) throw new Exception("The shape already cached.");
                state = CacheableState.Normal;
            }

            public void Evict()
            {
                onTouch = null;
                state = CacheableState.Evicted;
            }

            public event Action<ICacheable> OnTouch
            {
                add
                {
                    onTouch += value;
                }
                remove
                {
                    onTouch -= value;
                }
            }

            public CacheableState State
            {
                get { return state; }
            }

            public void Touch()
            {
                Action<ICacheable> t = onTouch;
                if (t != null)
                {
                    t(this);
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                state = CacheableState.Disposed;
            }

            #endregion
        }

        #endregion

        #region Private Members
        string unicode;
        float advance;
        OutlineCompound2f outline;
        GlyphCacheOptions cachingOptions;

        // Caching, configured later on (with hierarhical).
        Cache<float> filledCache;
        #endregion

        #region Private Methods

        void ConfigureCaching(GlyphCacheOptions options)
        {
            // TODO: ignore for now.

            // For now, manually configure, later on, we may need configuration to do that.
            filledCache = new Cache<float>(1.0f, 3.0f,
                new LRU<float>(0.1f, 1.0f));
        }


        #endregion

        #region Constructors

        /// <summary>
        /// A glyph constructor.
        /// </summary>
        /// <param name="unicode">The unicode character.</param>
        /// <param name="outline">Outline.</param>
        /// <param name="advance">The advance after glyph.</param>
        internal Glyph(string unicode, OutlineCompound2f outline,
                       float advance, GlyphCacheOptions options)
        {
            this.unicode = unicode;
            this.outline = outline;
            this.advance = advance;

            ConfigureCaching(options);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Caching options on per-character basis.
        /// </summary>
        public GlyphCacheOptions CachingOptions
        {
            get { return cachingOptions; }
            set
            {
                cachingOptions = value;
            }
        }

        /// <summary>
        /// The outline of glyph.
        /// </summary>
        public OutlineCompound2f Outline
        {
            get { return outline; }
        }

        /// <summary>
        /// The advance of glyph.
        /// </summary>
        public float Advance
        {
            get { return advance; }
        }

        /// <summary>
        /// The unicode signature.
        /// </summary>
        public string Unicode
        {
            get { return unicode; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Acquires a mesh for given resolution.
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public TriangleSoup2f AcquireFilled(float resolution)
        {
            // We first check the cache.
            if (filledCache != null)
            {
                ICacheable cacheable = filledCache.FindAndTouch(resolution);
                if (cacheable != null)
                {
                    GlyphShapeCache data = cacheable as GlyphShapeCache;
                    return data.Data;
                }
            }

            // Otherwise not found, we create it.
            TriangleSoup2f soup = new TriangleSoup2f();
            outline.Tesselate(resolution, soup);

            if (filledCache != null)
            {
                // We ignore if already there.
                filledCache.Add(resolution, new GlyphShapeCache(soup));
            }
            return soup;
        }

        /// <summary>
        /// Acquires a tesselated outline.
        /// </summary>
        /// <param name="width">The width of outline.</param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public TriangleSoup2f AcquireOutline(float resolution, OutlineTesselation.TesselationOptionsf options)
        {
            // For now caching is not supported.

            TriangleSoup2f soup = new TriangleSoup2f();
            outline.TesselateOutline(resolution, options, soup);
            return soup;
        }

        #endregion

    }
}
