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
using SharpMedia.Math.Shapes;
using SharpMedia.Math;
using SharpMedia.AspectOriented;
using SharpMedia.Caching;
using SharpMedia.Math.Shapes.Storage;
using SharpMedia.Math.Shapes.Compounds;
using SharpMedia.Graphics.Vector.Fills;

namespace SharpMedia.Graphics.Vector.Fonts
{

    /// <summary>
    /// The text options.
    /// </summary>
    /// <remarks>
    /// If Right and Left are not used, central alignement is used. The same applies
    /// to Top and Bottom.
    /// </remarks>
    [Flags]
    public enum TextOptions
    {
        /// <summary>
        /// This means central alignment.
        /// </summary>
        None = 0,

        /// <summary>
        /// Aligns text to right, exclusive with Left.
        /// </summary>
        Right = 1,

        /// <summary>
        /// Aligns text to left, exclusive with Right.
        /// </summary>
        Left = 2,

        /// <summary>
        /// Aligns text top, exclusive with Bottom.
        /// </summary>
        Top = 4,

        /// <summary>
        /// Aligns text bottom, exclusivew with Top.
        /// </summary>
        Bottom = 8,

        /// <summary>
        /// Groups words when going to new line.
        /// </summary>
        NotGroupWords = 16,

        /// <summary>
        /// Ignores newlines (not threated as special characters, new line breaks).
        /// </summary>
        NewLineOrdinary = 32,

        /// <summary>
        /// Uses canvas units for rendering, not pixels.
        /// </summary>
        UseCanvasUnits = 64,

        /// <summary>
        /// Are multi-characters allowed.
        /// </summary>
        /// <remarks>Multicharacters are special glyphs, that represent more than
        /// one unicode character together and are specifically fit for combination.</remarks>
        AllowMulticharacters = 128
    }

    /// <summary>
    /// A font class.
    /// </summary>
    public sealed class Font
    {
        #region Private Members
        IFontSupplier fontSupplier;

        // Caching data.
        GlyphCacheOptions defaultCacheOptions = GlyphCacheOptions.CacheShapes;
        Glyph defaultGlyph = null;
        SortedList<string, Glyph> glyphs = new SortedList<string, Glyph>();

        #endregion

        #region Helper Methods

        bool HasFlag(TextOptions options, TextOptions flag)
        {
            return (options & flag) != 0;
        }

        Glyph SelectGlyph(string text, int index, out int advanceIndex, bool allowMulti)
        {
            int length = text.Length;
            OutlineCompound2f obj;
            Glyph glyph;
            float adv;

            // We find longest, best matching glyph.
            for (int i = allowMulti ? (int)fontSupplier.MaxGlyphCharacters : 1; i >= 1; i--)
            {
                if (index + i > length) continue;
                advanceIndex = i;

                // We first check our cache.
                string searchStr = text.Substring(index, i);
                if(glyphs.TryGetValue(searchStr, out glyph)) return glyph;

                // We try to get.
                obj = fontSupplier.GetGlyph(searchStr, out adv);
                if (obj != null)
                {
                    glyphs.Add(searchStr, new Glyph(searchStr, obj, adv, defaultCacheOptions));
                }
            }

            advanceIndex = 1;

            if (defaultGlyph != null) return defaultGlyph;

            // No found, return default.
            obj = fontSupplier.GetDefaultGlyph(out adv);
            defaultGlyph = new Glyph(string.Empty, obj, adv, defaultCacheOptions);
            return defaultGlyph;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// This is to be called when the underlaying font supplier is changed or
        /// when there are resolution changes, discarding most of the current cached
        /// shapes.
        /// </summary>
        /// <remarks>Manual glyph must be readded.</remarks>
        public void ResetGlyphCache()
        {
            glyphs.Clear();
        }

        /// <summary>
        /// Adds a manual glyph.
        /// </summary>
        /// <param name="unicode">The unicode character.</param>
        /// <param name="outline">The outline.</param>
        /// <param name="advance">The advancement.</param>
        /// <param name="options">Caching options.</param>
        public void AddManual(string unicode, OutlineCompound2f outline, float advance,
            GlyphCacheOptions options)
        {
            glyphs.Add(unicode, new Glyph(unicode, outline, advance, options));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of font.
        /// </summary>
        public string FontName
        {
            get{ return fontSupplier.FontName; }
        }

        /// <summary>
        /// Sets or gets a font-wide options.
        /// </summary>
        /// <remarks>Setting options will also apply it to all glyph, even if they
        /// were handled manually.</remarks>
        public GlyphCacheOptions CacheOptions
        {
            get { return defaultCacheOptions; }
            set
            {
                defaultCacheOptions = value;
                foreach (KeyValuePair<string, Glyph> pair in this.glyphs)
                {
                    pair.Value.CachingOptions = value;
                }

                defaultGlyph.CachingOptions = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// A font constructor.
        /// </summary>
        /// <param name="supplier">The supplier.</param>
        public Font([NotNull] IFontSupplier supplier)
        {
            this.fontSupplier = supplier;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Prepares text for rendering, with costumized line starts and lengths.
        /// </summary>
        /// <remarks>Top/Bottom flags are ignored here.</remarks>
        public TextRenderInfo Prepare(ICanvas canvas, string text, float fontSize,
            TextOptions optons, Vector2f[] lineStarts, float[] lineLengths)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepares text for rendering, with rectange where data should be rendered.
        /// </summary>
        public TextRenderInfo Prepare(ICanvas canvas, string text, float fontSize,
            TextOptions options, float lineSpacing, Vector2f leftBottom, Vector2f size)
        {
            // We extract information and convert them.
            bool newLineOrdinary = HasFlag(options, TextOptions.NewLineOrdinary);
            bool groupWords = HasFlag(options, TextOptions.NotGroupWords);
            bool allowMulti = HasFlag(options, TextOptions.AllowMulticharacters);
            if (!HasFlag(options, TextOptions.UseCanvasUnits))
            {
                // We use Y dimension, since it is primary axis for text (we want it properly aligned).
                fontSize *= canvas.CanvasInfo.CanvasUnitSize.Y / canvas.CanvasInfo.CanvasPixelSize.Y;
            }
            lineSpacing *= fontSize;

            // We go for each character.
            float lineY = leftBottom.Y + size.Y - fontSize;
            float lineX = leftBottom.X;

            // Return data.
            List<TextRenderLineInfo> lines = new List<TextRenderLineInfo>();
            TextRenderLineInfo currentLine = new TextRenderLineInfo(
                new Vector2f(leftBottom.X, leftBottom.X + size.X));

            for (int i = 0; i < text.Length; i++)
            {
                // We first check for out of range.
                if (lineY < leftBottom.Y) break;

                // We select next character.
                int advIndex = 1;
                Glyph obj = null;

                // We find next glyph.
                if (text[i] != '\n' || newLineOrdinary)
                {
                    obj = SelectGlyph(text, i, out advIndex, allowMulti);
                }

                // We have it multiplied.
                float adv = obj.Advance;
                adv *= fontSize;

                // We check for line fit.
                if ((!newLineOrdinary && text[i] == '\n') || (lineX + adv >= leftBottom.X + size.X))
                {
                    // We go to new line.
                    lines.Add(currentLine);
                    lineY -= lineSpacing;
                    lineX = leftBottom.X;
                    if (lineY < leftBottom.Y)
                    {
                        currentLine = null;
                        break;
                    }
                    else
                    {
                        i--;
                        currentLine = new TextRenderLineInfo(
                            new Vector2f(leftBottom.X, leftBottom.X + size.X));
                        continue;
                    }
                    
                }

                // We advance and add it. We must carefully handle multi-char glyphs.
                for (int j = 0; j < advIndex; j++)
                {
                    // We create information.
                    RenderGlyphInfo info2 = new RenderGlyphInfo(
                        text[i + j], j == 0 ? text.Substring(i, advIndex) : string.Empty,
                        new Vector2f(lineX + j * (adv / advIndex), lineY), new Vector2f(adv / advIndex, fontSize),
                        j == 0 ? new Vector2f(adv, fontSize) : Vector2f.Zero, j == 0 ? obj : null, i + j);

                    currentLine.Append(info2);
                }

                // We advance.
                lineX += adv;
            }

            if (currentLine != null)
            {
                lines.Add(currentLine);
            }

            TextRenderInfo info = new TextRenderInfo(canvas, this, fontSize, lines.ToArray());

            // We now post-transform using flags.
            if (HasFlag(options, TextOptions.Top))
            {
                // Already positioned.
            }
            else if (HasFlag(options, TextOptions.Bottom))
            {
                float disp = lines[lines.Count - 1].Bottom - leftBottom.Y;

                info.Translate(new Vector2f(0, -disp));
            }
            else
            {
                // Center positioning.
                float disp = (lines[lines.Count - 1].Bottom - leftBottom.Y) / 2.0f;

                info.Translate(new Vector2f(0, -disp));
            }

            // We also align.
            if (HasFlag(options, TextOptions.Left))
            {
                // Already aligned.
                //info.LeftAlignLines();
            }
            else if (HasFlag(options, TextOptions.Right))
            {
                info.RightAlignLines();
            }
            else
            {
                info.CenterAlignLines();
            }

            return info;

        }


        /// <summary>
        /// Prepares and renders, with rectange range.
        /// </summary>
        public TextRenderInfo Render(Pen pen, ICanvas canvas, string text, float fontSize,
            TextOptions options, float lineSpacing, Vector2f leftBottom, Vector2f size)
        {
            TextRenderInfo info = Prepare(canvas, text, fontSize, options, lineSpacing,
                leftBottom, size);
            info.Render(pen);
            return info;
        }

        // TODO: other preparations

        /// <summary>
        /// Prepares and renders, with rectange range.
        /// </summary>
        public TextRenderInfo Render(IFill fill, ICanvas canvas, string text, float fontSize,
            TextOptions options, float lineSpacing, Vector2f leftBottom, Vector2f size)
        {
            TextRenderInfo info = Prepare(canvas, text, fontSize, options, lineSpacing,
                leftBottom, size);
            info.Render(fill);
            return info;
        }

        /// <summary>
        /// Prepares and renders, with customized line starts and lengths.
        /// </summary>
        public TextRenderInfo Render(Pen pen, ICanvas canvas, string text, float fontSize,
            TextOptions optons, Vector2f[] lineStarts, float[] lineLengths)
        {
            TextRenderInfo info = Prepare(canvas, text, fontSize, optons,
                lineStarts, lineLengths);
            info.Render(pen);
            return info;
        }

        /// <summary>
        /// Prepares and renders, with customized line starts and lengths.
        /// </summary>
        public TextRenderInfo Render(IFill fill, ICanvas canvas, string text, float fontSize,
            TextOptions optons, Vector2f[] lineStarts, float[] lineLengths)
        {
            TextRenderInfo info = Prepare(canvas, text, fontSize, optons,
                lineStarts, lineLengths);
            info.Render(fill);
            return info;
        }


        #endregion
    }
}
