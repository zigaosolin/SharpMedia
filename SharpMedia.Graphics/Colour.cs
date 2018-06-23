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

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A RGBA colour, needed by many device imputs. It usually maps to range [0,1] for
    /// each component.
    /// </summary>
    [Serializable]
    public struct Colour
    {
        #region Members
        public Vector4f c;
        #endregion

        #region Properties

        /// <summary>
        /// The red value.
        /// </summary>
        public float R
        {
            get { return c.X; }
            set { c.X = MathHelper.Max(value, 0.0f); }
        }

        /// <summary>
        /// A green value.
        /// </summary>
        public float G
        {
            get { return c.Y; }
            set { c.Y = MathHelper.Max(value, 0.0f); }
        }

        /// <summary>
        /// Blue value.
        /// </summary>
        public float B
        {
            get { return c.Z; }
            set { c.Z = MathHelper.Max(value, 0.0f); }
        }

        /// <summary>
        /// Alpha value.
        /// </summary>
        public float A
        {
            get { return c.W; }
            set { c.W = MathHelper.Max(value, 0.0f); }
        }

        /// <summary>
        /// Gets or sets the RGB.
        /// </summary>
        /// <value>The RGB.</value>
        public Vector3f RGB
        {
            get { return c.Vec3; }
            set
            {
                Vector3f v = new Vector3f(MathHelper.Max(value.X, 0.0f),
                    MathHelper.Max(value.Y, 0.0f), MathHelper.Max(value.Z, 0.0f));
                c.Vec3 = v;
            }
        }

        /// <summary>
        /// Gets or sets the RGBA as vector.
        /// </summary>
        /// <value>The RGBA.</value>
        public Vector4f RGBA
        {
            get
            {
                return c;
            }
            set
            {
                R = value.X;
                G = value.Y;
                B = value.Z;
                A = value.W;
            }
        }

        /// <summary>
        /// Gets the clamp colour (non HDR colour).
        /// </summary>
        /// <value>The clamp.</value>
        public Colour Clamp
        {
            get
            {
                Vector4f v = c;
                v.X = MathHelper.Clamp(v.X);
                v.Y = MathHelper.Clamp(v.Y);
                v.Z = MathHelper.Clamp(v.Z);
                v.W = MathHelper.Clamp(v.W);
                return new Colour(v);
            }
        }

        /// <summary>
        /// Obtains packed RGBA. Precission can be lost, no HDR supported.
        /// </summary>
        public uint PackedRGBA
        {
            get
            {

                // We now pack it.
                uint byteColour = 0;
                byteColour |= MathHelper.Min((uint)(c.W * 255.0f), 255);
                byteColour |= MathHelper.Min((uint)(c.Z * 255.0f), 255) << 8;
                byteColour |= MathHelper.Min((uint)(c.Y * 255.0f), 255) << 16;
                byteColour |= MathHelper.Min((uint)(c.X * 255.0f), 255) << 24;
                return byteColour;
            }
        }

        /// <summary>
        /// Obtains packed ARGB. Precission can be lost, no HDR support.
        /// </summary>
        public uint PackedARGB
        {
            get
            {
                // We now pack it.
                uint byteColour = 0;
                byteColour |= MathHelper.Min((uint)(c.Z * 255.0f), 255);
                byteColour |= MathHelper.Min((uint)(c.Y * 255.0f), 255) << 8;
                byteColour |= MathHelper.Min((uint)(c.X * 255.0f), 255) << 16;
                byteColour |= MathHelper.Min((uint)(c.W * 255.0f), 255) << 24;
                return byteColour;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Operator ==.
        /// </summary>
        public static bool operator ==(Colour c1, Colour c2)
        {
            return c1.c == c2.c;
        }

        /// <summary>
        /// Operator ==.
        /// </summary>
        public static bool operator !=(Colour c1, Colour c2)
        {
            return c1.c != c2.c;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is Colour)
            {
                return c.Equals(((Colour)obj).c);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return c.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("(R:{0}, G:{1}, B:{2}, A:{3})", c.X, c.Y, c.Z, c.W);
        }

        #endregion

        #region Static constructs

        ///<summary>
        ///Returns Black.
        ///</summary>
        public static Colour Black
        {
            get
            {
                return new Colour(0x000000FF);
            }
        }

        ///<summary>
        ///Returns Navy.
        ///</summary>
        public static Colour Navy
        {
            get
            {
                return new Colour(0x000080FF);
            }
        }

        ///<summary>
        ///Returns DarkBlue.
        ///</summary>
        public static Colour DarkBlue
        {
            get
            {
                return new Colour(0x00008BFF);
            }
        }

        ///<summary>
        ///Returns MediumBlue.
        ///</summary>
        public static Colour MediumBlue
        {
            get
            {
                return new Colour(0x0000CDFF);
            }
        }

        ///<summary>
        ///Returns Blue.
        ///</summary>
        public static Colour Blue
        {
            get
            {
                return new Colour(0x0000FFFF);
            }
        }

        ///<summary>
        ///Returns DarkGreen.
        ///</summary>
        public static Colour DarkGreen
        {
            get
            {
                return new Colour(0x006400FF);
            }
        }

        ///<summary>
        ///Returns Green.
        ///</summary>
        public static Colour Green
        {
            get
            {
                return new Colour(0x008000FF);
            }
        }

        ///<summary>
        ///Returns Teal.
        ///</summary>
        public static Colour Teal
        {
            get
            {
                return new Colour(0x008080FF);
            }
        }

        ///<summary>
        ///Returns DarkCyan.
        ///</summary>
        public static Colour DarkCyan
        {
            get
            {
                return new Colour(0x008B8BFF);
            }
        }

        ///<summary>
        ///Returns DeepSkyBlue.
        ///</summary>
        public static Colour DeepSkyBlue
        {
            get
            {
                return new Colour(0x00BFFFFF);
            }
        }

        ///<summary>
        ///Returns DarkTurquoise.
        ///</summary>
        public static Colour DarkTurquoise
        {
            get
            {
                return new Colour(0x00DED1FF);
            }
        }

        ///<summary>
        ///Returns MediumSpringGreen.
        ///</summary>
        public static Colour MediumSpringGreen
        {
            get
            {
                return new Colour(0x00FA9AFF);
            }
        }

        ///<summary>
        ///Returns Lime.
        ///</summary>
        public static Colour Lime
        {
            get
            {
                return new Colour(0x00FF00FF);
            }
        }

        ///<summary>
        ///Returns SpringGreen.
        ///</summary>
        public static Colour SpringGreen
        {
            get
            {
                return new Colour(0x00FF7FFF);
            }
        }

        ///<summary>
        ///Returns Cyan.
        ///</summary>
        public static Colour Cyan
        {
            get
            {
                return new Colour(0x00FFFFFF);
            }
        }

        ///<summary>
        ///Returns MidnightBlue.
        ///</summary>
        public static Colour MidnightBlue
        {
            get
            {
                return new Colour(0x191970FF);
            }
        }

        ///<summary>
        ///Returns DodgerBlue.
        ///</summary>
        public static Colour DodgerBlue
        {
            get
            {
                return new Colour(0x1E90FFFF);
            }
        }

        ///<summary>
        ///Returns LightSeagreen.
        ///</summary>
        public static Colour LightSeagreen
        {
            get
            {
                return new Colour(0x20B2AAFF);
            }
        }

        ///<summary>
        ///Returns ForestGreen.
        ///</summary>
        public static Colour ForestGreen
        {
            get
            {
                return new Colour(0x228B22FF);
            }
        }

        ///<summary>
        ///Returns SeaGreen.
        ///</summary>
        public static Colour SeaGreen
        {
            get
            {
                return new Colour(0x2E8B57FF);
            }
        }

        ///<summary>
        ///Returns DarkSlateGray.
        ///</summary>
        public static Colour DarkSlateGray
        {
            get
            {
                return new Colour(0x2F4F4FFF);
            }
        }

        ///<summary>
        ///Returns LimeGreen.
        ///</summary>
        public static Colour LimeGreen
        {
            get
            {
                return new Colour(0x32CD32FF);
            }
        }

        ///<summary>
        ///Returns MediumSeaGreen.
        ///</summary>
        public static Colour MediumSeaGreen
        {
            get
            {
                return new Colour(0x3CB371FF);
            }
        }

        ///<summary>
        ///Returns Turquoise.
        ///</summary>
        public static Colour Turquoise
        {
            get
            {
                return new Colour(0x40E0D0FF);
            }
        }

        ///<summary>
        ///Returns RoyalBlue.
        ///</summary>
        public static Colour RoyalBlue
        {
            get
            {
                return new Colour(0x4169E1FF);
            }
        }

        ///<summary>
        ///Returns Steelblue.
        ///</summary>
        public static Colour Steelblue
        {
            get
            {
                return new Colour(0x4682B4FF);
            }
        }

        ///<summary>
        ///Returns DarkSlateBlue.
        ///</summary>
        public static Colour DarkSlateBlue
        {
            get
            {
                return new Colour(0x483D8BFF);
            }
        }

        ///<summary>
        ///Returns MediumTurquoise.
        ///</summary>
        public static Colour MediumTurquoise
        {
            get
            {
                return new Colour(0x48D1CCFF);
            }
        }

        ///<summary>
        ///Returns Indigo.
        ///</summary>
        public static Colour Indigo
        {
            get
            {
                return new Colour(0x4B0082FF);
            }
        }

        ///<summary>
        ///Returns DarkOliveGreen.
        ///</summary>
        public static Colour DarkOliveGreen
        {
            get
            {
                return new Colour(0x556B2FFF);
            }
        }

        ///<summary>
        ///Returns CadetBlue.
        ///</summary>
        public static Colour CadetBlue
        {
            get
            {
                return new Colour(0x5F9EA0FF);
            }
        }

        ///<summary>
        ///Returns CornflowerBlue.
        ///</summary>
        public static Colour CornflowerBlue
        {
            get
            {
                return new Colour(0x6495EDFF);
            }
        }

        ///<summary>
        ///Returns MediumAquamarine.
        ///</summary>
        public static Colour MediumAquamarine
        {
            get
            {
                return new Colour(0x66CDAAFF);
            }
        }

        ///<summary>
        ///Returns DimGray.
        ///</summary>
        public static Colour DimGray
        {
            get
            {
                return new Colour(0x696969FF);
            }
        }

        ///<summary>
        ///Returns SlateBlue.
        ///</summary>
        public static Colour SlateBlue
        {
            get
            {
                return new Colour(0x6A5ACDFF);
            }
        }

        ///<summary>
        ///Returns OliveDrab.
        ///</summary>
        public static Colour OliveDrab
        {
            get
            {
                return new Colour(0x6B8E23FF);
            }
        }

        ///<summary>
        ///Returns LightSlateGray.
        ///</summary>
        public static Colour LightSlateGray
        {
            get
            {
                return new Colour(0x778899FF);
            }
        }

        ///<summary>
        ///Returns MediumSlateBlue.
        ///</summary>
        public static Colour MediumSlateBlue
        {
            get
            {
                return new Colour(0x7B68EEFF);
            }
        }

        ///<summary>
        ///Returns Lawngreen.
        ///</summary>
        public static Colour Lawngreen
        {
            get
            {
                return new Colour(0x7CFC00FF);
            }
        }

        ///<summary>
        ///Returns Chartreuse.
        ///</summary>
        public static Colour Chartreuse
        {
            get
            {
                return new Colour(0x7FFF00FF);
            }
        }

        ///<summary>
        ///Returns Aquamarine.
        ///</summary>
        public static Colour Aquamarine
        {
            get
            {
                return new Colour(0x7FFFD4FF);
            }
        }

        ///<summary>
        ///Returns Maroon.
        ///</summary>
        public static Colour Maroon
        {
            get
            {
                return new Colour(0x800000FF);
            }
        }

        ///<summary>
        ///Returns Purple.
        ///</summary>
        public static Colour Purple
        {
            get
            {
                return new Colour(0x800080FF);
            }
        }

        ///<summary>
        ///Returns Gray.
        ///</summary>
        public static Colour Gray
        {
            get
            {
                return new Colour(0x808080FF);
            }
        }

        ///<summary>
        ///Returns SkyBlue.
        ///</summary>
        public static Colour SkyBlue
        {
            get
            {
                return new Colour(0x87CEEBFF);
            }
        }

        ///<summary>
        ///Returns LightSkyBlue.
        ///</summary>
        public static Colour LightSkyBlue
        {
            get
            {
                return new Colour(0x87CEFAFF);
            }
        }

        ///<summary>
        ///Returns BlueViolet.
        ///</summary>
        public static Colour BlueViolet
        {
            get
            {
                return new Colour(0x8A2BE2FF);
            }
        }

        ///<summary>
        ///Returns DarkRed.
        ///</summary>
        public static Colour DarkRed
        {
            get
            {
                return new Colour(0x8B0000FF);
            }
        }

        ///<summary>
        ///Returns DarkMagenta.
        ///</summary>
        public static Colour DarkMagenta
        {
            get
            {
                return new Colour(0x8B008BFF);
            }
        }

        ///<summary>
        ///Returns SaddleBrown.
        ///</summary>
        public static Colour SaddleBrown
        {
            get
            {
                return new Colour(0x8B4513FF);
            }
        }

        ///<summary>
        ///Returns DarkSeagreen.
        ///</summary>
        public static Colour DarkSeagreen
        {
            get
            {
                return new Colour(0x8DBC8FFF);
            }
        }

        ///<summary>
        ///Returns LightGreen.
        ///</summary>
        public static Colour LightGreen
        {
            get
            {
                return new Colour(0x90EE90FF);
            }
        }

        ///<summary>
        ///Returns MediumPurple.
        ///</summary>
        public static Colour MediumPurple
        {
            get
            {
                return new Colour(0x9370DBFF);
            }
        }

        ///<summary>
        ///Returns DarkViolet.
        ///</summary>
        public static Colour DarkViolet
        {
            get
            {
                return new Colour(0x9400D3FF);
            }
        }

        ///<summary>
        ///Returns PaleGreen.
        ///</summary>
        public static Colour PaleGreen
        {
            get
            {
                return new Colour(0x98FB98FF);
            }
        }

        ///<summary>
        ///Returns DarkOrchid.
        ///</summary>
        public static Colour DarkOrchid
        {
            get
            {
                return new Colour(0x9932CCFF);
            }
        }

        ///<summary>
        ///Returns YellowGreen.
        ///</summary>
        public static Colour YellowGreen
        {
            get
            {
                return new Colour(0x9ACD32FF);
            }
        }

        ///<summary>
        ///Returns Sienna.
        ///</summary>
        public static Colour Sienna
        {
            get
            {
                return new Colour(0xA0522DFF);
            }
        }

        ///<summary>
        ///Returns Brown.
        ///</summary>
        public static Colour Brown
        {
            get
            {
                return new Colour(0xA52A2AFF);
            }
        }

        ///<summary>
        ///Returns DarkGray.
        ///</summary>
        public static Colour DarkGray
        {
            get
            {
                return new Colour(0xA9A9A9FF);
            }
        }

        ///<summary>
        ///Returns LightBlue.
        ///</summary>
        public static Colour LightBlue
        {
            get
            {
                return new Colour(0xADD8E6FF);
            }
        }

        ///<summary>
        ///Returns GreenYellow.
        ///</summary>
        public static Colour GreenYellow
        {
            get
            {
                return new Colour(0xADFF2FFF);
            }
        }

        ///<summary>
        ///Returns PaleTurquoise.
        ///</summary>
        public static Colour PaleTurquoise
        {
            get
            {
                return new Colour(0xAFEEEEFF);
            }
        }

        ///<summary>
        ///Returns LightSteelBlue.
        ///</summary>
        public static Colour LightSteelBlue
        {
            get
            {
                return new Colour(0xB0C4DEFF);
            }
        }

        ///<summary>
        ///Returns PowderBlue.
        ///</summary>
        public static Colour PowderBlue
        {
            get
            {
                return new Colour(0xB0E0E6FF);
            }
        }

        ///<summary>
        ///Returns Firebrick.
        ///</summary>
        public static Colour Firebrick
        {
            get
            {
                return new Colour(0xB22222FF);
            }
        }

        ///<summary>
        ///Returns DarkGoldenrod.
        ///</summary>
        public static Colour DarkGoldenrod
        {
            get
            {
                return new Colour(0xB8860BFF);
            }
        }

        ///<summary>
        ///Returns MediumOrchid.
        ///</summary>
        public static Colour MediumOrchid
        {
            get
            {
                return new Colour(0xBA55D3FF);
            }
        }

        ///<summary>
        ///Returns RosyBrown.
        ///</summary>
        public static Colour RosyBrown
        {
            get
            {
                return new Colour(0xBC8F8FFF);
            }
        }

        ///<summary>
        ///Returns DarkKhaki.
        ///</summary>
        public static Colour DarkKhaki
        {
            get
            {
                return new Colour(0xBDB76BFF);
            }
        }

        ///<summary>
        ///Returns Silver.
        ///</summary>
        public static Colour Silver
        {
            get
            {
                return new Colour(0xC0C0C0FF);
            }
        }

        ///<summary>
        ///Returns MediumVioletRed.
        ///</summary>
        public static Colour MediumVioletRed
        {
            get
            {
                return new Colour(0xC71585FF);
            }
        }

        ///<summary>
        ///Returns IndianRed.
        ///</summary>
        public static Colour IndianRed
        {
            get
            {
                return new Colour(0xCD5C5CFF);
            }
        }

        ///<summary>
        ///Returns Peru.
        ///</summary>
        public static Colour Peru
        {
            get
            {
                return new Colour(0xCD853FFF);
            }
        }

        ///<summary>
        ///Returns Chocolate.
        ///</summary>
        public static Colour Chocolate
        {
            get
            {
                return new Colour(0xD2691EFF);
            }
        }

        ///<summary>
        ///Returns Tan.
        ///</summary>
        public static Colour Tan
        {
            get
            {
                return new Colour(0xD2B48CFF);
            }
        }

        ///<summary>
        ///Returns LightGrey.
        ///</summary>
        public static Colour LightGrey
        {
            get
            {
                return new Colour(0xD3D3D3FF);
            }
        }

        ///<summary>
        ///Returns Thistle.
        ///</summary>
        public static Colour Thistle
        {
            get
            {
                return new Colour(0xD8BFD8FF);
            }
        }

        ///<summary>
        ///Returns Orchid.
        ///</summary>
        public static Colour Orchid
        {
            get
            {
                return new Colour(0xDA70D6FF);
            }
        }

        ///<summary>
        ///Returns Goldenrod.
        ///</summary>
        public static Colour Goldenrod
        {
            get
            {
                return new Colour(0xDAA520FF);
            }
        }

        ///<summary>
        ///Returns PaleVioletRed.
        ///</summary>
        public static Colour PaleVioletRed
        {
            get
            {
                return new Colour(0xDB7093FF);
            }
        }

        ///<summary>
        ///Returns Crimson.
        ///</summary>
        public static Colour Crimson
        {
            get
            {
                return new Colour(0xDC143CFF);
            }
        }

        ///<summary>
        ///Returns Gainsboro.
        ///</summary>
        public static Colour Gainsboro
        {
            get
            {
                return new Colour(0xDCDCDCFF);
            }
        }

        ///<summary>
        ///Returns Plum.
        ///</summary>
        public static Colour Plum
        {
            get
            {
                return new Colour(0xDDA0DDFF);
            }
        }

        ///<summary>
        ///Returns Burlywood.
        ///</summary>
        public static Colour Burlywood
        {
            get
            {
                return new Colour(0xDEB887FF);
            }
        }

        ///<summary>
        ///Returns Mauve.
        ///</summary>
        public static Colour Mauve
        {
            get
            {
                return new Colour(0xE0B0FFFF);
            }
        }

        ///<summary>
        ///Returns LightCyan.
        ///</summary>
        public static Colour LightCyan
        {
            get
            {
                return new Colour(0xE0FFFFFF);
            }
        }

        ///<summary>
        ///Returns Lavender.
        ///</summary>
        public static Colour Lavender
        {
            get
            {
                return new Colour(0xE6E6FAFF);
            }
        }

        ///<summary>
        ///Returns DarkSalmon.
        ///</summary>
        public static Colour DarkSalmon
        {
            get
            {
                return new Colour(0xE9967AFF);
            }
        }

        ///<summary>
        ///Returns Violet.
        ///</summary>
        public static Colour Violet
        {
            get
            {
                return new Colour(0xEE82EEFF);
            }
        }

        ///<summary>
        ///Returns PaleGoldenrod.
        ///</summary>
        public static Colour PaleGoldenrod
        {
            get
            {
                return new Colour(0xEEE8AAFF);
            }
        }

        ///<summary>
        ///Returns LightCoral.
        ///</summary>
        public static Colour LightCoral
        {
            get
            {
                return new Colour(0xF08080FF);
            }
        }

        ///<summary>
        ///Returns Khaki.
        ///</summary>
        public static Colour Khaki
        {
            get
            {
                return new Colour(0xF0E68CFF);
            }
        }

        ///<summary>
        ///Returns AliceBlue.
        ///</summary>
        public static Colour AliceBlue
        {
            get
            {
                return new Colour(0xF0F8FFFF);
            }
        }

        ///<summary>
        ///Returns Honeydew.
        ///</summary>
        public static Colour Honeydew
        {
            get
            {
                return new Colour(0xF0FFF0FF);
            }
        }

        ///<summary>
        ///Returns Azure.
        ///</summary>
        public static Colour Azure
        {
            get
            {
                return new Colour(0xF0FFFFFF);
            }
        }

        ///<summary>
        ///Returns SandyBrown.
        ///</summary>
        public static Colour SandyBrown
        {
            get
            {
                return new Colour(0xF4A460FF);
            }
        }

        ///<summary>
        ///Returns Wheat.
        ///</summary>
        public static Colour Wheat
        {
            get
            {
                return new Colour(0xF5DEB3FF);
            }
        }

        ///<summary>
        ///Returns Beige.
        ///</summary>
        public static Colour Beige
        {
            get
            {
                return new Colour(0xF5F5DCFF);
            }
        }

        ///<summary>
        ///Returns Whitesmoke.
        ///</summary>
        public static Colour Whitesmoke
        {
            get
            {
                return new Colour(0xF5F5F5FF);
            }
        }

        ///<summary>
        ///Returns MintCream.
        ///</summary>
        public static Colour MintCream
        {
            get
            {
                return new Colour(0xF5FFFAFF);
            }
        }

        ///<summary>
        ///Returns GhostWhite.
        ///</summary>
        public static Colour GhostWhite
        {
            get
            {
                return new Colour(0xF8F8FFFF);
            }
        }

        ///<summary>
        ///Returns Salmon.
        ///</summary>
        public static Colour Salmon
        {
            get
            {
                return new Colour(0xFA8072FF);
            }
        }

        ///<summary>
        ///Returns AntiqueWhite.
        ///</summary>
        public static Colour AntiqueWhite
        {
            get
            {
                return new Colour(0xFAEBD7FF);
            }
        }

        ///<summary>
        ///Returns Linen.
        ///</summary>
        public static Colour Linen
        {
            get
            {
                return new Colour(0xFAF0E6FF);
            }
        }

        ///<summary>
        ///Returns LightGoldenrodYellow.
        ///</summary>
        public static Colour LightGoldenrodYellow
        {
            get
            {
                return new Colour(0xFAFAD2FF);
            }
        }

        ///<summary>
        ///Returns OldLace.
        ///</summary>
        public static Colour OldLace
        {
            get
            {
                return new Colour(0xFDF5E6FF);
            }
        }

        ///<summary>
        ///Returns Red.
        ///</summary>
        public static Colour Red
        {
            get
            {
                return new Colour(0xFF0000FF);
            }
        }

        ///<summary>
        ///Returns Magenta.
        ///</summary>
        public static Colour Magenta
        {
            get
            {
                return new Colour(0xFF00FFFF);
            }
        }

        ///<summary>
        ///Returns DeepPink.
        ///</summary>
        public static Colour DeepPink
        {
            get
            {
                return new Colour(0xFF1493FF);
            }
        }

        ///<summary>
        ///Returns OrangeRed.
        ///</summary>
        public static Colour OrangeRed
        {
            get
            {
                return new Colour(0xFF4500FF);
            }
        }

        ///<summary>
        ///Returns Tomato.
        ///</summary>
        public static Colour Tomato
        {
            get
            {
                return new Colour(0xFF6347FF);
            }
        }

        ///<summary>
        ///Returns HotPink.
        ///</summary>
        public static Colour HotPink
        {
            get
            {
                return new Colour(0xFF69B4FF);
            }
        }

        ///<summary>
        ///Returns Coral.
        ///</summary>
        public static Colour Coral
        {
            get
            {
                return new Colour(0xFF7F50FF);
            }
        }

        ///<summary>
        ///Returns DarkOrange.
        ///</summary>
        public static Colour DarkOrange
        {
            get
            {
                return new Colour(0xFF8C00FF);
            }
        }

        ///<summary>
        ///Returns LightSalmon.
        ///</summary>
        public static Colour LightSalmon
        {
            get
            {
                return new Colour(0xFFA07AFF);
            }
        }

        ///<summary>
        ///Returns Orange.
        ///</summary>
        public static Colour Orange
        {
            get
            {
                return new Colour(0xFFA500FF);
            }
        }

        ///<summary>
        ///Returns LightPink.
        ///</summary>
        public static Colour LightPink
        {
            get
            {
                return new Colour(0xFFB6C1FF);
            }
        }

        ///<summary>
        ///Returns Pink.
        ///</summary>
        public static Colour Pink
        {
            get
            {
                return new Colour(0xFFC8CBFF);
            }
        }

        ///<summary>
        ///Returns Gold.
        ///</summary>
        public static Colour Gold
        {
            get
            {
                return new Colour(0xFFD700FF);
            }
        }

        ///<summary>
        ///Returns PeachPuff.
        ///</summary>
        public static Colour PeachPuff
        {
            get
            {
                return new Colour(0xFFDAB9FF);
            }
        }

        ///<summary>
        ///Returns NavajoWhite.
        ///</summary>
        public static Colour NavajoWhite
        {
            get
            {
                return new Colour(0xFFDEADFF);
            }
        }

        ///<summary>
        ///Returns Moccasin.
        ///</summary>
        public static Colour Moccasin
        {
            get
            {
                return new Colour(0xFFE4B5FF);
            }
        }

        ///<summary>
        ///Returns Bisque.
        ///</summary>
        public static Colour Bisque
        {
            get
            {
                return new Colour(0xFFE4C4FF);
            }
        }

        ///<summary>
        ///Returns MistyRose.
        ///</summary>
        public static Colour MistyRose
        {
            get
            {
                return new Colour(0xFFE4E1FF);
            }
        }

        ///<summary>
        ///Returns BlanchedAlmond.
        ///</summary>
        public static Colour BlanchedAlmond
        {
            get
            {
                return new Colour(0xFFEBCDFF);
            }
        }

        ///<summary>
        ///Returns PapayaWhip.
        ///</summary>
        public static Colour PapayaWhip
        {
            get
            {
                return new Colour(0xFFEFD5FF);
            }
        }

        ///<summary>
        ///Returns LavenderBlush.
        ///</summary>
        public static Colour LavenderBlush
        {
            get
            {
                return new Colour(0xFFF0F5FF);
            }
        }

        ///<summary>
        ///Returns SeaShell.
        ///</summary>
        public static Colour SeaShell
        {
            get
            {
                return new Colour(0xFFF5EEFF);
            }
        }

        ///<summary>
        ///Returns Cornsilk.
        ///</summary>
        public static Colour Cornsilk
        {
            get
            {
                return new Colour(0xFFF8DCFF);
            }
        }

        ///<summary>
        ///Returns LemonChiffon.
        ///</summary>
        public static Colour LemonChiffon
        {
            get
            {
                return new Colour(0xFFFACDFF);
            }
        }

        ///<summary>
        ///Returns FloralWhite.
        ///</summary>
        public static Colour FloralWhite
        {
            get
            {
                return new Colour(0xFFFAF0FF);
            }
        }

        ///<summary>
        ///Returns Snow.
        ///</summary>
        public static Colour Snow
        {
            get
            {
                return new Colour(0xFFFAFAFF);
            }
        }

        ///<summary>
        ///Returns Yellow.
        ///</summary>
        public static Colour Yellow
        {
            get
            {
                return new Colour(0xFFFF00FF);
            }
        }

        ///<summary>
        ///Returns LightYellow.
        ///</summary>
        public static Colour LightYellow
        {
            get
            {
                return new Colour(0xFFFFE0FF);
            }
        }

        ///<summary>
        ///Returns Ivory.
        ///</summary>
        public static Colour Ivory
        {
            get
            {
                return new Colour(0xFFFFF0FF);
            }
        }

        ///<summary>
        ///Returns White.
        ///</summary>
        public static Colour White
        {
            get
            {
                return new Colour(0xFFFFFFFF);
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Constructor using all components.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component.</param>
        public Colour(float r, float g, float b, float a)
        {
            c.X = MathHelper.Max(0.0f, r);
            c.Y = MathHelper.Max(0.0f, g);
            c.Z = MathHelper.Max(0.0f, b);
            c.W = MathHelper.Max(0.0f, a);
        }

        /// <summary>
        /// Constructs from RGBA colours.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component.</param>
        public Colour(byte r, byte g, byte b, byte a)
        {
            c.X = (float)r / 255.0f;
            c.Y = (float)g / 255.0f;
            c.Z = (float)b / 255.0f;
            c.W = (float)a / 255.0f;
        }

        /// <summary>
        /// Creation from 32-bit RGBA packed array.
        /// </summary>
        /// <param name="rgba"></param>
        public Colour(uint rgba)
        {
            c.X = (float)((rgba >> 24) & 0xFF) / 255.0f;
            c.Y = (float)((rgba >> 16) & 0xFF) / 255.0f;
            c.Z = (float)((rgba >> 8) & 0xFF) / 255.0f;
            c.W = (float)((rgba) & 0xFF) / 255.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> class.
        /// </summary>
        /// <param name="v">The vector.</param>
        public Colour(Vector4f v)
        {
            c = v;
        }
        
        #endregion

        #region Conversions

        /// <summary>
        /// Froms the grayscale.
        /// </summary>
        /// <param name="gray">The gray.</param>
        /// <returns></returns>
        public static Colour FromGrayscale(float gray)
        {
            return new Colour(gray, gray, gray, 1.0f);
        }

        public static Colour FromCMYK(float cyan, float magenta, float yellow, float black)
        {
            throw new NotImplementedException();
        }

        public static Colour FromRGBA(uint rgba)
        {
            return new Colour((float)((rgba >> 24) & 0xFF) / 255.0f, (float)((rgba >> 16) & 0xFF) / 255.0f,
                (float)((rgba >> 8) & 0xFF) / 255.0f, (float)((rgba) & 0xFF) / 255.0f);
        }

        /// <summary>
        /// Converts RGB part to CMYK.
        /// </summary>
        public Vector4f CMYK
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Operations

        /// <summary>
        /// Clamps the value to range [0,1].
        /// </summary>
        public void ClampSelf()
        {
            if (c.X > 1.0f) c.X = 1.0f;
            if (c.Y > 1.0f) c.Y = 1.0f;
            if (c.Z > 1.0f) c.Z = 1.0f;
            if (c.W > 1.0f) c.W = 1.0f;
        }

        #endregion

    }

}
