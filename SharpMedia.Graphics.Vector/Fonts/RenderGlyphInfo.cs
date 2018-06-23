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
using SharpMedia.Graphics.Vector.Mappers;

namespace SharpMedia.Graphics.Vector.Fonts
{
    /// <summary>
    /// The include type.
    /// </summary>
    public enum IncludeType
    {
        Fully,
        CenterIncluded,
        PartyIncluded
    }

    /// <summary>
    /// A glyph rendering information.
    /// </summary>
    /// <remarks>A render glyph information handles multi-character glyphs as seperate by placing render
    /// data only in first glyph in the row. Real size holds the multi-character glyph size while normal
    /// size is the size of single glyph.</remarks>
    public sealed class RenderGlyphInfo
    {
        #region Private Members
        Glyph renderingData;
        string realRepresentation;
        char representation;

        Vector2f leftBottom;
        Vector2f size;
        Vector2f realSize;
        int indexInArray;

        IMapper mapper;
        #endregion

        #region Internal Members

        internal void Adjust(Vector2f move)
        {
            leftBottom += move;
        }

        static bool IsIncluded(Vector2f min, Vector2f max, Vector2f center)
        {
            if (center.X > min.X && center.X < max.X &&
                           center.Y > min.Y && center.Y < max.Y) return true;
            return false;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public RenderGlyphInfo(char representation, string realRepresentation,
            Vector2f leftBottom, Vector2f size,
            Vector2f realSize, Glyph renderingData, int index)
        {
            this.realRepresentation = realRepresentation;
            this.representation = representation;
            this.leftBottom = leftBottom;
            this.size = size;
            this.realSize = realSize;
            this.renderingData = renderingData;
            this.indexInArray = index;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The mapper used for rendering this glyph, can be asigned at any timebefore rendering.
        /// </summary>
        public IMapper AttachedMapper
        {
            get { return mapper; }
            set { mapper = value; }
        }

        /// <summary>
        /// Renderign data of glyph.
        /// </summary>
        public Glyph RenderingData
        {
            get { return renderingData; }
        }

        /// <summary>
        /// Representation of glyph.
        /// </summary>
        public char Representation
        {
            get { return representation; }
        }

        /// <summary>
        /// Multirepresentation of glyph (if multi-character glyph, it will be followed by
        /// normal glyphs).
        /// </summary>
        public string MultiRepresentation
        {
            get { return realRepresentation; }
        }

        /// <summary>
        /// The index in rendered array, sequential.
        /// </summary>
        public uint IndexInArray
        {
            get { return (uint)indexInArray; }
        }

        /// <summary>
        /// Left bottom corner.
        /// </summary>
        public Vector2f LeftBottom
        {
            get { return leftBottom; }
        }

        /// <summary>
        /// Size of character
        /// </summary>
        public Vector2f Size
        {
            get { return size; }
        }

        /// <summary>
        /// Multi-character size.
        /// </summary>
        public Vector2f MultiSize
        {
            get { return realSize; }
        }

        /// <summary>
        /// Right top corner of glyph.
        /// </summary>
        public Vector2f RightTop
        {
            get { return leftBottom + size; }
        }

        #endregion

        #region Methods

        public bool IsInside(Vector2f p)
        {
            return false;
        }

        public bool IsIncluded(Vector2f min, Vector2f max, IncludeType includeType)
        {
            switch (includeType)
            {
                case IncludeType.CenterIncluded:
                    {
                        Vector2f center = this.leftBottom + size * 0.5f;
                        return IsIncluded(min, max, center);
                        
                    }
                case IncludeType.PartyIncluded:
                    {
                        return IsIncluded(min, max, this.leftBottom + size * 0.25f) ||
                               IsIncluded(min, max, this.leftBottom + size * 0.75f) ||
                               IsIncluded(min, max, this.leftBottom +
                                    new Vector2f(size.X * 0.25f, size.Y * 0.75f)) ||
                               IsIncluded(min, max, this.leftBottom +
                                    new Vector2f(size.X * 0.75f, size.Y * 0.25f));
                    }
                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }

}
