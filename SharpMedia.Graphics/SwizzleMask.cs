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
using SharpMedia.Testing;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// This is immutable swizzle mask class. It can swizzle vectors and matrices, up to 4x4 matrices.
    /// </summary>
    /// <remarks>
    /// Class packs the swizzle mask in one ulong variable.
    /// </remarks>
    [Serializable]
    public class SwizzleMask : IEquatable<SwizzleMask>
    {
        /// <summary>
        /// A submask 
        /// </summary>
        public enum ComponentSelector : uint
        {
            /// <summary>
            /// Vector accessors.
            /// </summary>
            X = 0,
            Y,
            Z,
            W,

            /// <summary>
            /// Colour accessors.
            /// </summary>
            R = 0,
            G,
            B,
            A,

            /// <summary>
            /// Matrix accessors.
            /// </summary>
            M00 = 0,
            M01,
            M02,
            M03,
            M10,
            M11,
            M12,
            M13,
            M20,
            M21,
            M22,
            M23,
            M30,
            M31,
            M32,
            M33
        }


        #region Private Members
        uint rows = 0;
        uint rowLength = 0;
        ulong mask = 0;

        // Statics
        static SwizzleMask xyzw = SwizzleMask.Parse("xyzw");
        static SwizzleMask xyz = SwizzleMask.Parse("xyz");
        static SwizzleMask xy = SwizzleMask.Parse("xy");
        static SwizzleMask x = SwizzleMask.Parse("x");
        static SwizzleMask y = SwizzleMask.Parse("y");
        static SwizzleMask z = SwizzleMask.Parse("z");
        static SwizzleMask w = SwizzleMask.Parse("w");

        static SwizzleMask row2_4 = SwizzleMask.Parse("M10M11M12M13");
        static SwizzleMask row2_3 = SwizzleMask.Parse("M10M11M12");
        static SwizzleMask row2_2 = SwizzleMask.Parse("M10M11");

        static SwizzleMask row3_4 = SwizzleMask.Parse("M20M21M22M23");
        static SwizzleMask row3_3 = SwizzleMask.Parse("M20M21M22");
        static SwizzleMask row3_2 = SwizzleMask.Parse("M20M21");

        static SwizzleMask row4_4 = SwizzleMask.Parse("M30M31M32M33");
        static SwizzleMask row4_3 = SwizzleMask.Parse("M30M31M32");
        static SwizzleMask row4_2 = SwizzleMask.Parse("M30M31");

        static SwizzleMask m2x2 = SwizzleMask.Parse("M00M01.M10M11");
        static SwizzleMask m3x3 = SwizzleMask.Parse("M00M01M02.M10M11M12.M20M21M22");
        static SwizzleMask m4x4 = SwizzleMask.Parse("M00M01M02M03.M10M11M12M13.M20M21M22M23.M30M31M32M33");
        #endregion

        #region Static Swizzle Masks

        /// <summary>
        /// A RGBA swizzle mask.
        /// </summary>
        public static SwizzleMask RGBA
        {
            get
            {
                return xyzw;
            }
        }

        /// <summary>
        /// Gets the RGB.
        /// </summary>
        /// <value>The RGB.</value>
        public static SwizzleMask RGB
        {
            get
            {
                return xyz;
            }
        }

        /// <summary>
        /// Gets the row3 as vec2.
        /// </summary>
        /// <value>The row3 as vec2.</value>
        public static SwizzleMask Row3AsVec2
        {
            get
            {
                return row3_2;
            }
        }

        /// <summary>
        /// Gets the row3 as vec3.
        /// </summary>
        /// <value>The row3 as vec3.</value>
        public static SwizzleMask Row3AsVec3
        {
            get
            {
                return row3_3;
            }
        }

        /// <summary>
        /// Gets the row3 as vec4.
        /// </summary>
        /// <value>The row3 as vec4.</value>
        public static SwizzleMask Row3AsVec4
        {
            get
            {
                return row3_4;
            }
        }


        /// <summary>
        /// Gets the row4 as vec2.
        /// </summary>
        /// <value>The row4 as vec2.</value>
        public static SwizzleMask Row4AsVec2
        {
            get
            {
                return row4_2;
            }
        }

        /// <summary>
        /// Gets the row4 as vec3.
        /// </summary>
        /// <value>The row4 as vec3.</value>
        public static SwizzleMask Row4AsVec3
        {
            get
            {
                return row4_3;
            }
        }

        /// <summary>
        /// Gets the row4 as vec4.
        /// </summary>
        /// <value>The row4 as vec4.</value>
        public static SwizzleMask Row4AsVec4
        {
            get
            {
                return row4_4;
            }
        }

        /// <summary>
        /// Gets the row2 as vec2.
        /// </summary>
        /// <value>The row2 as vec2.</value>
        public static SwizzleMask Row2AsVec2
        {
            get
            {
                return row2_2;
            }
        }

        /// <summary>
        /// Gets the row2 as vec3.
        /// </summary>
        /// <value>The row2 as vec3.</value>
        public static SwizzleMask Row2AsVec3
        {
            get
            {
                return row2_3;
            }
        }

        /// <summary>
        /// Gets the row2 as vec4.
        /// </summary>
        /// <value>The row2 as vec4.</value>
        public static SwizzleMask Row2AsVec4
        {
            get
            {
                return row2_4;
            }
        }

        /// <summary>
        /// Gets the row1 as vec3.
        /// </summary>
        /// <value>The row1 as vec3.</value>
        public static SwizzleMask Row1AsVec2
        {
            get
            {
                return xyz;
            }
        }

        /// <summary>
        /// Gets the row1 as vec3.
        /// </summary>
        /// <value>The row1 as vec3.</value>
        public static SwizzleMask Row1AsVec3
        {
            get
            {
                return xyz;
            }
        }

        /// <summary>
        /// Gets the row1 as vec3.
        /// </summary>
        /// <value>The row1 as vec3.</value>
        public static SwizzleMask Row1AsVec4
        {
            get
            {
                return xyzw;
            }
        }

        /// <summary>
        /// Gets the matrix2x2 default.
        /// </summary>
        /// <value>The matrix2x2 default.</value>
        public static SwizzleMask Matrix2x2Default
        {
            get
            {
                return m2x2;
            }
        }



        /// <summary>
        /// Gets the matrix3x3 default.
        /// </summary>
        /// <value>The matrix3x3 default.</value>
        public static SwizzleMask Matrix3x3Default
        {
            get
            {
                return m3x3;
            }
        }


        /// <summary>
        /// Gets the matrix4x4 default.
        /// </summary>
        public static SwizzleMask Matrix4x4Default
        {
            get
            {
                return m4x4;
            }
        }

        /// <summary>
        /// Gets the X swizzle mask.
        /// </summary>
        public static SwizzleMask X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// Gets the X swizzle mask.
        /// </summary>
        public static SwizzleMask Y
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// Gets the X swizzle mask.
        /// </summary>
        public static SwizzleMask Z
        {
            get
            {
                return z;
            }
        }

        /// <summary>
        /// Gets the X swizzle mask.
        /// </summary>
        public static SwizzleMask W
        {
            get
            {
                return w;
            }
        }

        /// <summary>
        /// Gets the XY swizzle mask.
        /// </summary>
        public static SwizzleMask XY
        {
            get
            {
                return xy;
            }
        }

        /// <summary>
        /// Gets the XYZ swizzle mask.
        /// </summary>
        public static SwizzleMask XYZ
        {
            get
            {
                return xyz;
            }
        }

        /// <summary>
        /// Gets the XYZW swizzle mask.
        /// </summary>
        public static SwizzleMask XYZW
        {
            get
            {
                return xyzw;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public uint RowCount
        {
            get
            {
                return rows;
            }
        }

        /// <summary>
        /// Gets the column count.
        /// </summary>
        /// <value>The column count.</value>
        public uint ColumnCount
        {
            get
            {
                return rowLength;
            }
        }

        /// <summary>
        /// Gets the swizzle out format, as float.
        /// </summary>
        /// <value>The swizzle out format, as float. Can be convert to other types..</value>
        public PinFormat SwizzleOutFormat
        {
            get
            {
                switch (rows)
                {
                    case 1:
                        switch (rowLength)
                        {
                            case 1: return PinFormat.Float;
                            case 2: return PinFormat.Floatx2;
                            case 3: return PinFormat.Floatx3;
                            case 4: return PinFormat.Floatx4;
                        }
                        break;
                    case 2:
                        switch (rowLength)
                        {
                            case 1: return PinFormat.Undefined;
                            case 2: return PinFormat.Float2x2;
                            case 3: return PinFormat.Undefined;
                            case 4: return PinFormat.Undefined;
                        }
                        break;
                    case 3:
                        switch (rowLength)
                        {
                            case 1: return PinFormat.Undefined;
                            case 2: return PinFormat.Undefined;
                            case 3: return PinFormat.Float3x3;
                            case 4: return PinFormat.Undefined;
                        }
                        break;
                    case 4:
                        switch (rowLength)
                        {
                            case 1: return PinFormat.Undefined;
                            case 2: return PinFormat.Undefined;
                            case 3: return PinFormat.Undefined;
                            case 4: return PinFormat.Float4x4;
                        }
                        break;
                }
                return PinFormat.Undefined;
            }
        }

        /// <summary>
        /// The mask.
        /// </summary>
        public ulong Mask
        {
            get
            {
                return mask;
            }
        }

        /// <summary>
        /// Gets the minimum swizzle in format. This means that all components are included in that format.
        /// </summary>
        /// <value>The minimum swizzle in format.</value>
        public PinFormat MinimumSwizzleInFormat
        {
            get
            {
                // We find maximum component in x and y.
                uint mx = 0, my = 0;
                for (uint i = 0; i < rows; i++)
                {
                    for (uint j = 0; j < rowLength; j++)
                    {
                        uint c = (uint)this[i, j];

                        // We update x and y.
                        if (c % 4 > mx) mx = c % 4;
                        if (c / 4 > my) my = c / 4;
                    }
                }

                // We have special case my = 0.
                if (my == 0)
                {
                    switch (mx)
                    {
                        case 0:
                            return PinFormat.Float;
                        case 1:
                            return PinFormat.Floatx2;
                        case 2:
                            return PinFormat.Floatx3;
                        default:
                        case 3:
                            return PinFormat.Floatx4;
                    }
                }
                // Else we have a matrix.
                else
                {
                    uint max = mx > my ? mx : my;
                    switch (max)
                    {
                        case 0:
                            return PinFormat.Float;
                        case 1:
                            return PinFormat.Float2x2;
                        case 2:
                            return PinFormat.Float3x3;
                        default:
                        case 3:
                            return PinFormat.Float4x4;
                    }
                }
            }
        }

            /// <summary>
            /// Gets the <see cref="SharpMedia.Graphics.SwizzleMask.ComponentSelector"/> with the specified column.
            /// </summary>
            /// <value></value>
            public ComponentSelector this[uint column]
        {
            get
            {
                if (column >= rowLength) throw new IndexOutOfRangeException("Trying to access component out of range.");

                return (ComponentSelector)((mask >> (int)(column) * 4) & 0xF);
            }
        }

        /// <summary>
        /// Gets the <see cref="SharpMedia.Graphics.SwizzleMask.ComponentSelector"/> with the specified row.
        /// </summary>
        /// <value></value>
        public ComponentSelector this[uint row, uint column]
        {
            get
            {
                if(row >= rows || column >= rowLength)
                {
                    throw new IndexOutOfRangeException("Trying to access component out of range.");
                }
                return (ComponentSelector)((mask >> (int)(row * 4 + column) * 4) & 0xF);
            }
        }


        #endregion

        #region Overloads

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();

            // We have a vector
            if (rows == 1)
            {
                for (uint column = 0; column < rowLength; column++)
                {
                    ComponentSelector selector = (ComponentSelector)((mask >> (int)(column * 4)) & 0xF);

                    switch (selector)
                    {
                        case ComponentSelector.X:
                            b.Append("X");
                            break;
                        case ComponentSelector.Y:
                            b.Append("Y");
                            break;
                        case ComponentSelector.Z:
                            b.Append("Z");
                            break;
                        case ComponentSelector.W:
                            b.Append("W");
                            break;
                    }
                }
            }
            else
            {
                for (uint row = 0; row < rows; row++)
                {
                    // Split rows.
                    if (row != 0) b.Append(".");

                    for (uint column = 0; column < rowLength; column++)
                    {
                        ComponentSelector selector = (ComponentSelector)((mask >> (int)((column + row * 4) * 4)) & 0xF);
                        switch (selector)
                        {
                            case ComponentSelector.M00:
                                b.Append("M00");
                                break;
                            case ComponentSelector.M01:
                                b.Append("M01");
                                break;
                            case ComponentSelector.M02:
                                b.Append("M02");
                                break;
                            case ComponentSelector.M03:
                                b.Append("M03");
                                break;
                            case ComponentSelector.M10:
                                b.Append("M10");
                                break;
                            case ComponentSelector.M11:
                                b.Append("M11");
                                break;
                            case ComponentSelector.M12:
                                b.Append("M12");
                                break;
                            case ComponentSelector.M13:
                                b.Append("M13");
                                break;
                            case ComponentSelector.M20:
                                b.Append("M20");
                                break;
                            case ComponentSelector.M21:
                                b.Append("M21");
                                break;
                            case ComponentSelector.M22:
                                b.Append("M22");
                                break;
                            case ComponentSelector.M23:
                                b.Append("M23");
                                break;
                            case ComponentSelector.M30:
                                b.Append("M30");
                                break;
                            case ComponentSelector.M31:
                                b.Append("M31");
                                break;
                            case ComponentSelector.M32:
                                b.Append("M32");
                                break;
                            case ComponentSelector.M33:
                                b.Append("M33");
                                break;
                            default:
                                break;
                        }
                    }
                    
                }
            }


            return b.ToString();
        }

        /// <summary>
        /// Operator ==.
        /// </summary>
        public static bool operator ==(SwizzleMask mask1, SwizzleMask mask2)
        {
            return mask1.Equals(mask2);
        }

        /// <summary>
        /// Operator !=.
        /// </summary>
        public static bool operator !=(SwizzleMask mask1, SwizzleMask mask2)
        {
            return !(mask1 != mask2);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return this == (SwizzleMask)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)mask ^ (int)(mask >> 32);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Converts character to uint.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static uint CharToUInt(char c)
        {
            switch (c)
            {
                case '0': return 0;
                case '1': return 1;
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;
                default: throw new FormatException("Invalid swizzle format.");
            }
        }

        /// <summary>
        /// Constructs swizzle mask.
        /// </summary>
        /// <param name="m">The mask.</param>
        internal SwizzleMask(ulong m, uint rs, uint rowSize)
        {
            mask = m;
            rows = rs;
            rowLength = rowSize;
        }

        /// <summary>
        /// Constructs swizzle mask from string.
        /// </summary>
        /// <param name="s">String specifying swizzle mask. You can use rgba or xyzw components. If you need
        /// matrix access, you can use m00m11m22 ... syntax (dot means new row).</param>
        /// <example>
        /// <code>
        /// SwizzleMask m1 = SwizzleMask.Parse("zwwy");
        /// SwizzleMask m2 = SwizzleMask.Parse("m11m00m20.m00m11m20.m21m11m01");
        /// </code>
        /// </example>
        /// <returns>Swizzle mask object.</returns>
        public static SwizzleMask Parse(string s)
        {
            ulong mask = 0;
            uint rows = 0;
            uint rowSize = 0;

            uint prevRowSize = uint.MaxValue;

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                // We begin filling.
                switch (c)
                {
                    case 'x':
                    case 'X':
                    case 'r':
                    case 'R':
                        mask |= (ulong)ComponentSelector.X << (int)(rows * 4 + rowSize++) * 4;
                        break;
                    case 'y':
                    case 'Y':
                    case 'G':
                    case 'g':
                        mask |= (ulong)ComponentSelector.Y << (int)(rows * 4 + rowSize++) * 4;
                        break;
                    case 'z':
                    case 'Z':
                    case 'b':
                    case 'B':
                        mask |= (ulong)ComponentSelector.Z << (int)(rows * 4 + rowSize++) * 4;
                        break;
                    case 'W':
                    case 'w':
                    case 'A':
                    case 'a':
                        mask |= (ulong)ComponentSelector.W << (int)(rows * 4 + rowSize++) * 4;
                        break;
                    case 'm':
                    case 'M':
                        // We need to read two mode.
                        if (i + 2 >= s.Length) throw new FormatException("Invalid swizzle format.");

                        uint r = CharToUInt(s[++i]), col = CharToUInt(s[++i]);

                        if(r >= 4 || col >= 4)
                        {
                            throw new FormatException("Only matriced up to 4x4 dimensions are supported.");
                        }

                        switch (r * 4 + col)
                        {
                            case 0:
                                mask |= (ulong)ComponentSelector.M00 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 1:
                                mask |= (ulong)ComponentSelector.M01 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 2:
                                mask |= (ulong)ComponentSelector.M02 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 3:
                                mask |= (ulong)ComponentSelector.M03 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 4:
                                mask |= (ulong)ComponentSelector.M10 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 5:
                                mask |= (ulong)ComponentSelector.M11 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 6:
                                mask |= (ulong)ComponentSelector.M12 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 7:
                                mask |= (ulong)ComponentSelector.M13 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 8:
                                mask |= (ulong)ComponentSelector.M20 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 9:
                                mask |= (ulong)ComponentSelector.M21 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 10:
                                mask |= (ulong)ComponentSelector.M22 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 11:
                                mask |= (ulong)ComponentSelector.M23 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 12:
                                mask |= (ulong)ComponentSelector.M30 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 13:
                                mask |= (ulong)ComponentSelector.M31 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 14:
                                mask |= (ulong)ComponentSelector.M32 << (int)(rows * 4 + rowSize) * 4;
                                break;
                            case 15:
                                mask |= (ulong)ComponentSelector.M33 << (int)(rows * 4 + rowSize) * 4;
                                break;
                        }

                        rowSize++;
                        break;
                    case '.':
                        if (prevRowSize != uint.MaxValue && prevRowSize != rowSize)
                        {
                            throw new FormatException("Invalid format, row lenghts are not the same.");
                        }

                        // Go to next.
                        prevRowSize = rowSize;
                        rowSize = 0;
                        if (++rows >= 4)
                        {
                            throw new FormatException("Too many rows.");
                        }
                        break;
                    default:
                        throw new FormatException("Invalid swizzle format.");
                }

            }

            // A final checkup.
            if (prevRowSize != uint.MaxValue && rowSize != prevRowSize)
            {
                throw new FormatException("Invalid format, row lenghts are not the same.");
            }
            rows++;

            return new SwizzleMask(mask, rows, rowSize);
        }



        #endregion

        #region IEquatable<SwizzleMask> Members

        public bool Equals(SwizzleMask other)
        {
            if (this.rows != other.rows || this.rowLength != other.rowLength || mask != other.mask) return false;
            return true;
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class SwizzleMaskTest
    {
        [CorrectnessTest]
        public void Parse1()
        {
            SwizzleMask mask = SwizzleMask.Parse("RGBA");
            SwizzleMask mask2 = SwizzleMask.Parse("XYZW");
            SwizzleMask mask3 = SwizzleMask.Parse("M00M01M02M03");

            Assert.AreEqual(mask.ToString(), mask2.ToString());
            Assert.AreEqual(mask3.ToString(), mask2.ToString());

            mask = SwizzleMask.Parse("M00M01M02.M10M00M20.M20M10M20");
            Assert.AreEqual(mask.ToString(), "M00M01M02.M10M00M20.M20M10M20");
        }
    }
#endif
}
