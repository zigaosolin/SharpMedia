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
using SharpMedia.Math.Shapes.Compounds;
using SharpMedia.Caching;
using SharpMedia.Math;
using SharpMedia.Math.Shapes.Storage;

namespace SharpMedia.Graphics.Vector.Fonts.SVG
{

    /// <summary>
    /// A glyph represents a character. It is closed path (e.g. outline).
    /// </summary>
    /// <remarks>You can treat glyph as a path (closed path) or as a shape (what needs to be filled).</remarks>
    [Serializable]
    internal sealed class SVGGlyph
    {
        #region Private Members
        OutlineCompound2f outline;
        string unicode;
        float advance;
        #endregion

        #region Public Members

        /// <summary>
        /// The glyph constructor.
        /// </summary>
        /// <param name="unicode"></param>
        /// <param name="outline"></param>
        public SVGGlyph(string unicode, OutlineCompound2f outline, float advance)
        {
            this.outline = outline;
            this.unicode = unicode;
            this.advance = advance;
        }

        /// <summary>
        /// The outline.
        /// </summary>
        public OutlineCompound2f Outline
        {
            get
            {
                return outline;
            }
        }

        /// <summary>
        /// The unicode character combination.
        /// </summary>
        public string Unicode
        {
            get
            {
                return unicode;
            }
        }

        /// <summary>
        /// The relative advancement of "cursor" after glyph is drawn.
        /// </summary>
        public float Advance
        {
            get
            {
                return advance;
            }
            set
            {
                advance = value;
            }
        }

        #endregion
    }
}
