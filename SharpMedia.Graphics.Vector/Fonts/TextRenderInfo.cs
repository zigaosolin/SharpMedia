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
using SharpMedia.Math;
using SharpMedia.Math.Matrix;
using SharpMedia.Graphics.Vector.Fills;
using SharpMedia.Graphics.Vector.Transforms;
using SharpMedia.Math.Shapes.Storage;
using SharpMedia.Graphics.Vector.Mappers;

namespace SharpMedia.Graphics.Vector.Fonts
{

    /// <summary>
    /// Contains all information for text rendering.
    /// </summary>
    public class TextRenderInfo
    {
        #region Private Members
        Font font;
        ICanvas canvas;
        TextRenderLineInfo[] lines;
        RenderGlyphInfo[] glyphs;
        Vector2i fullRange;
        float fontSize;
        #endregion

        #region Helper Methods

        void ValidateRange(Vector2i range)
        {
            if (range.X < 0 || range.Y > fullRange.Y || range.Y < range.X)
            {
                throw new ArgumentException("Range invalid.");
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Obtains bounding rectangle.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void GetBoundingRect(Vector2i range, out Vector2f min, out Vector2f max)
        {
            ValidateRange(range);

            min = new Vector2f(float.PositiveInfinity, float.PositiveInfinity);
            max = new Vector2f(float.NegativeInfinity, float.NegativeInfinity);

            for (int i = range.X; i <= range.Y; i++)
            {
                RenderGlyphInfo info = glyphs[i];
                Vector2f lb = info.LeftBottom, rt = lb + info.MultiSize;

                if (lb.X < min.X) min.X = lb.X;
                if (lb.Y < min.Y) min.Y = lb.Y;
                if (rt.X > max.X) max.X = rt.X;
                if (rt.Y > max.Y) max.Y = rt.Y;
            }
            
        }

        /// <summary>
        /// Obtains index range of selected text.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Vector2i GetRange(Vector2f min, Vector2f max, IncludeType includeType)
        {
            // FIXME: more optimal
            Vector2i range = new Vector2i(int.MaxValue, int.MinValue);

            for (int i = 0; i < glyphs.Length; i++)
            {
                if (glyphs[i].IsIncluded(min, max, includeType))
                {
                    int idx = (int)glyphs[i].IndexInArray;
                    if (range.X > idx) range.X = idx;
                    if (range.Y < idx) range.Y = idx;
                }
            }

            if (range.X == int.MaxValue) return new Vector2i(0, -1);
            return range;
        }

        /// <summary>
        /// Renders all text using pen (or fill).
        /// </summary>
        /// <param name="pen"></param>
        public void Render(Pen pen)
        {
            Render(pen, fullRange);
        }

        /// <summary>
        /// Renders filled characters.
        /// </summary>
        /// <param name="fill"></param>
        public void Render(IFill fill)
        {
            Render(fill, fullRange);
        }

        /// <summary>
        /// Render outlined characters.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="range"></param>
        public void Render(Pen pen, Vector2i range)
        {
            if (range.Y < range.X) return;
            ValidateRange(range);

            canvas.Begin(CanvasRenderFlags.SoftwarePositionTransform);
            try
            {

                for (int i = range.X; i <= range.Y; i++)
                {
                    RenderGlyphInfo info = glyphs[i];
                    if(glyphs[i].RenderingData == null) continue;

                    // We set transform.
                    canvas.Transform = 
                       new LinearTransform(
                         Matrix4x4f.CreateTranslate(info.LeftBottom.Vec3) *
                         Matrix4x4f.CreateScale(new Vector3f(fontSize, fontSize, fontSize))
                    );


                    // We finally render.
                    TriangleSoup2f soup =
                        info.RenderingData.AcquireOutline(canvas.CanvasInfo.TesselationResolution,
                        pen.ToOutlineTesselationOptions(canvas.CanvasInfo));
                    canvas.FillShape(pen.Fill, soup, info.AttachedMapper);
                }

            }
            finally
            {
                canvas.End();
            }
        }

        /// <summary>
        /// Renders filled characters.
        /// </summary>
        /// <param name="fill"></param>
        /// <param name="range"></param>
        public void Render(IFill fill, Vector2i range)
        {
            if (range.Y < range.X) return;
            ValidateRange(range);

            canvas.Begin(CanvasRenderFlags.SoftwarePositionTransform);
            try
            {

                for (int i = range.X; i <= range.Y; i++)
                {
                    RenderGlyphInfo info = glyphs[i];
                    if (glyphs[i].RenderingData == null) continue;

                    // We set transform.
                    canvas.Transform =
                       new LinearTransform(
                         Matrix4x4f.CreateTranslate(info.LeftBottom.Vec3) *
                         Matrix4x4f.CreateScale(new Vector3f(fontSize, fontSize, fontSize))
                    );


                    // We finally render.
                    TriangleSoup2f soup = 
                        info.RenderingData.AcquireFilled(canvas.CanvasInfo.TesselationResolution);
                    canvas.FillShape(fill, soup, info.AttachedMapper);
                }

            }
            finally
            {
                canvas.End();
            }
        }

        /// <summary>
        /// Gets specific line.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TextRenderLineInfo GetLine(uint index)
        {
            return lines[index];
        }

        /// <summary>
        /// Gets number of lines.
        /// </summary>
        public uint LineCount
        {
            get
            {
                return (uint)lines.Length;
            }
        }

        /// <summary>
        /// Obtains glyph count.
        /// </summary>
        public uint GlyphCount
        {
            get
            {
                return (uint)glyphs.Length;
            }
        }

        /// <summary>
        /// Obtains render glyph information.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public RenderGlyphInfo this[uint index]
        {
            get
            {
                return glyphs[index];
            }
        }

        /// <summary>
        /// Constructs render info from lines.
        /// </summary>
        /// <param name="lines"></param>
        public TextRenderInfo(ICanvas canvas, Font font, 
            float fontSize, params TextRenderLineInfo[] lines)
        {
            this.lines = lines;

            // We extract glyphs.
            List<RenderGlyphInfo> t = new List<RenderGlyphInfo>();
            for (int i = 0; i < lines.Length; i++)
            {
                TextRenderLineInfo info = lines[i];
                for (uint j = 0; j < info.GlyphCount; j++)
                {
                    RenderGlyphInfo g = info[j];
                    t.Add(info[j]);
                }
            }

            this.glyphs = t.ToArray();

            // May adjust.
            this.fullRange = new Vector2i(0, glyphs.Length - 1);
            this.canvas = canvas;
            this.font = font;
            this.fontSize = fontSize;
        }


        #endregion

        #region Transformations

        /// <summary>
        /// Translates text render information by vector.
        /// </summary>
        /// <param name="translateVector"></param>
        public void Translate(Vector2f translateVector)
        {
            for (int i = 0; i < glyphs.Length; i++)
            {
                RenderGlyphInfo info = glyphs[i];
                info.Adjust(translateVector);
            }


            // Must adjust line spans.
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].AdjustSpan(translateVector.X);
            }
        }

        /// <summary>
        /// Left aligns all lines.
        /// </summary>
        public void LeftAlignLines()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].LeftAlign();
            }
        }

        /// <summary>
        /// Right aligns lines.
        /// </summary>
        public void RightAlignLines()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].RightAlign();
            }
        }

        /// <summary>
        /// Center aligns lines.
        /// </summary>
        public void CenterAlignLines()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].CenterAlign();
            }
        }

        #endregion
    }

    /// <summary>
    /// A line information.
    /// </summary>
    public sealed class TextRenderLineInfo
    {
        #region Private Members
        Vector2f lineSpan;
        List<RenderGlyphInfo> glyphs;
        #endregion

        #region Internal Members

        internal void AdjustSpan(float delta)
        {
            lineSpan.X += delta;
            lineSpan.Y += delta;
        }

        #endregion

        #region Construtors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="info"></param>
        public TextRenderLineInfo(Vector2f span, params RenderGlyphInfo[] info)
        {
            this.lineSpan = span;
            this.glyphs = new List<RenderGlyphInfo>(info);
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        public TextRenderLineInfo(Vector2f span)
        {
            this.lineSpan = span;
            this.glyphs = new List<RenderGlyphInfo>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Number of glyphs in line.
        /// </summary>
        public uint GlyphCount
        {
            get
            {
                return (uint)glyphs.Count;
            }
        }

        /// <summary>
        /// Obtains glyph.
        /// </summary>
        public RenderGlyphInfo this[uint idx]
        {
            get { return glyphs[(int)idx]; }
        }

        /// <summary>
        /// The index range of line.
        /// </summary>
        public Vector2i IndexRange
        {
            get
            {
                if (glyphs.Count == 0) return new Vector2i(int.MinValue, int.MinValue);
                return new Vector2i((int)glyphs[0].IndexInArray, 
                                    (int)glyphs[glyphs.Count - 1].IndexInArray);
            }
        }

        /// <summary>
        /// The bottom of line.
        /// </summary>
        public float Bottom
        {
            get
            {
                if (glyphs.Count == 0) return 0;
                return glyphs[0].LeftBottom.Y;
            }
        }

        /// <summary>
        /// The top of line.
        /// </summary>
        public float Top
        {
            get
            {
                if (glyphs.Count == 0) return 0;
                return glyphs[0].RightTop.Y;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Appends a glyph.
        /// </summary>
        /// <param name="info"></param>
        public void Append(RenderGlyphInfo info)
        {
            glyphs.Add(info);
        }

        #endregion

        #region Transformations

        /// <summary>
        /// Right aligns line.
        /// </summary>
        public void RightAlign()
        {
            if (glyphs.Count == 0) return;

            // We get the displacement.
            float disp = glyphs[0].LeftBottom.X - lineSpan.X;

            for (int i = 0; i < glyphs.Count; i++)
            {
                glyphs[i].Adjust(new Vector2f(-disp, 0));
            }
        }

        /// <summary>
        /// Left aligns line.
        /// </summary>
        public void LeftAlign()
        {
            if (glyphs.Count == 0) return;

            // We get the displacement.
            float disp = glyphs[glyphs.Count-1].RightTop.X - lineSpan.X;

            for (int i = 0; i < glyphs.Count; i++)
            {
                glyphs[i].Adjust(new Vector2f(disp, 0));
            }
        }

        /// <summary>
        /// Center aligns line.
        /// </summary>
        public void CenterAlign()
        {
            if (glyphs.Count == 0) return;
            
            // We get lengths.
            float length = glyphs[glyphs.Count - 1].RightTop.X - glyphs[0].LeftBottom.X;
            float dispFromRight = ((lineSpan.Y - lineSpan.X) - length) / 2.0f;

            float disp = (glyphs[0].LeftBottom.X - lineSpan.X) - dispFromRight;

            for (int i = 0; i < glyphs.Count; i++)
            {
                glyphs[i].Adjust(new Vector2f(-disp, 0));
            }
        }

        #endregion
    }
}
