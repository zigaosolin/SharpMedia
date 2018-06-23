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
using System.Xml;
using SharpMedia.AspectOriented;
using SharpMedia.Math;
using SharpMedia.Math.Matrix;
using SharpMedia.Math.Shapes.Compounds;
using SharpMedia.Graphics.Vector.Fills;
using SharpMedia.Math.Shapes.Storage;

namespace SharpMedia.Graphics.Vector.Fonts.SVG
{

    /// <summary>
    /// This is a native vector font class.
    /// </summary>
    [Serializable]
    public class SVGFontProvider : IFontSupplier
    {
        #region Private Members
        string name;

        // A set of chars to glyph mapper.
        SortedList<string, SVGGlyph> glyphs;
        SVGGlyph defaultGlyph;

        // Data for kernelling.
        SortedList<ComparablePair<char, char>, float> kernelling =
            new SortedList<ComparablePair<char, char>, float>();
        #endregion

        #region Private Methods


        internal SVGFontProvider(string name, SortedList<string, SVGGlyph> glyph,
            SortedList<ComparablePair<char, char>, float> kernelling, SVGGlyph defaultGlyph)
        {
            this.name = name;
            this.glyphs = glyph;
            this.kernelling = kernelling;
            this.defaultGlyph = defaultGlyph;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Imports a SVG font.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static SVGFontProvider ImportSVG([NotNull] XmlDocument document)
        {
            SVGImport import = new SVGImport();
            return import.ImportSVG(document);
        }

        /// <summary>
        /// Imports a SVG font.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static SVGFontProvider ImportSVG(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            return ImportSVG(doc);
        }

        #endregion

        #region IFontSupplier Members

        public string FontName
        {
            get { return name; }
        }

        public uint MaxGlyphCharacters
        {
            get { return 3; }
        }

        public OutlineCompound2f GetGlyph(string glyphString, out float advance)
        {
            SVGGlyph glyph;
            if (glyphs.TryGetValue(glyphString, out glyph))
            {
                advance = glyph.Advance;
                return glyph.Outline;
            }

            advance = 0;
            return null;
        }

        public OutlineCompound2f GetDefaultGlyph(out float advance)
        {
            advance = defaultGlyph.Advance;
            return defaultGlyph.Outline;
        }

        public float Kernelling(char c1, char c2)
        {
            float value;
            if (kernelling.TryGetValue(new ComparablePair<char, char>(c1, c2), out value))
            {
                return value;
            }

            // Otherwise, no special kernelling.
            return 0.0f;
        }

        #endregion
    }
}
