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
using System.Runtime.Serialization;
using SharpMedia.Testing;
using SharpMedia.AspectOriented;
using SharpMedia.Math.Shapes.Storage;

namespace SharpMedia.Graphics
{


    /// <summary>
    /// Invalid vertex format exception, thrown when wrong combinations are used for format.
    /// </summary>
    public class InvalidVertexFormatException : Exception
    {
        public InvalidVertexFormatException(string message)
            : base(message)
        {
        }

        public InvalidVertexFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public InvalidVertexFormatException()
            : base("Invalid vertex format")
        {
        }

        public InvalidVertexFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// Provides format for <see cref="Vertices"/>. You can define any type of format you
    /// want but it must be "full" (e.g. no blank spaces). Vertex format is immutable.
    /// 
    /// The hardware can usually support almost any format you define. However, there may
    /// be a limit of number of components.
    /// </summary>
    [Serializable]
    public class VertexFormat : IEquatable<VertexFormat>, IEnumerable<VertexFormat.Element>
    {
        #region Vertex Element

        /// <summary>
        /// An element of vertex format.
        /// </summary>
        public class Element
        {
            PinComponent component;
            PinFormat format;
            uint offset;

            /// <summary>
            /// Helper, obtains element's size.
            /// </summary>
            /// <param name="fmt">The format.</param>
            /// <returns></returns>
            public static uint FormatSize(PinFormat fmt)
            {
                uint size;
                switch (fmt)
                {
                    case PinFormat.UInteger:
                        size = 4;
                        break;
                    case PinFormat.UIntegerx2:
                        size = 4 * 2;
                        break;
                    case PinFormat.UIntegerx3:
                        size = 4 * 3;
                        break;
                    case PinFormat.UIntegerx4:
                        size = 4 * 4;
                        break;
                    case PinFormat.Integer:
                        size = 4;
                        break;
                    case PinFormat.Integerx2:
                        size = 4 * 2;
                        break;
                    case PinFormat.Integerx3:
                        size = 4 * 3;
                        break;
                    case PinFormat.Integerx4:
                        size = 4 * 4;
                        break;
                    case PinFormat.Float:
                        size = 4;
                        break;
                    case PinFormat.Floatx2:
                        size = 4 * 2;
                        break;
                    case PinFormat.Floatx3:
                        size = 4 * 3;
                        break;
                    case PinFormat.Floatx4:
                        size = 4 * 4;
                        break;
                    default:
                        throw new InvalidVertexFormatException("The format is unknown, size undetirmined.");
                }

                // Apply the attribute.
                return size;
            }

            /// <summary>
            /// Component of the element.
            /// </summary>
            public PinComponent Component
            {
                get { return component; }
            }

            /// <summary>
            /// Format of the component.
            /// </summary>
            public PinFormat Format
            {
                get { return format; }
            }

            /// <summary>
            /// Offset of the element.
            /// </summary>
            public uint Offset
            {
                get { return offset; }
            }

            /// <summary>
            /// Resolves pixel format.
            /// </summary>
            /// <param name="offset">The offset.</param>
            /// <returns>New offset.</returns>
            internal uint Resolve(uint off)
            {
                offset = off;
                return offset + FormatSize(format);
            }

            /// <summary>
            /// Construction of element.
            /// </summary>
            /// <param name="c">The component.</param>
            /// <param name="fmt">The format.</param>
            internal Element(PinComponent c, PinFormat fmt)
            {
                component = c;
                format = fmt;
                offset = 0;
            }

            /// <summary>
            /// Obtains short name of component.
            /// </summary>
            /// <param name="component">The component.</param>
            /// <returns>Components name.</returns>
            public static string ToString(PinComponent component)
            {
                string c = null;
                switch (component)
                {
                    case PinComponent.Position:
                        c = "P";
                        break;
                    case PinComponent.Normal:
                        c = "N";
                        break;
                    case PinComponent.Colour:
                        c = "C";
                        break;
                    case PinComponent.SecondaryColour:
                        c = "SC";
                        break;
                    case PinComponent.BlendWeights:
                        c = "BW";
                        break;
                    case PinComponent.BlendIndices:
                        c = "BI";
                        break;
                    case PinComponent.BiNormal:
                        c = "B";
                        break;
                    case PinComponent.Tangent:
                        c = "T";
                        break;
                    case PinComponent.TexCoord0:
                        c = "T0";
                        break;
                    case PinComponent.TexCoord1:
                        c = "T1";
                        break;
                    case PinComponent.TexCoord2:
                        c = "T2";
                        break;
                    case PinComponent.TexCoord3:
                        c = "T3";
                        break;
                    case PinComponent.TexCoord4:
                        c = "T4";
                        break;
                    case PinComponent.TexCoord5:
                        c = "T5";
                        break;
                    case PinComponent.TexCoord6:
                        c = "T6";
                        break;
                    case PinComponent.TexCoord7:
                        c = "T7";
                        break;
                    case PinComponent.TexCoord8:
                        c = "T8";
                        break;
                    case PinComponent.TexCoord9:
                        c = "T9";
                        break;
                    case PinComponent.TexCoord10:
                        c = "T10";
                        break;
                    case PinComponent.TexCoord11:
                        c = "T11";
                        break;
                    case PinComponent.TexCoord12:
                        c = "T12";
                        break;
                    case PinComponent.TexCoord13:
                        c = "T13";
                        break;
                    case PinComponent.TexCoord14:
                        c = "T14";
                        break;
                    case PinComponent.TexCoord15:
                        c = "T15";
                        break;
                    case PinComponent.User0:
                        c = "U0";
                        break;
                    case PinComponent.User1:
                        c = "U1";
                        break;
                    case PinComponent.User2:
                        c = "U2";
                        break;
                    case PinComponent.User3:
                        c = "U3";
                        break;
                    case PinComponent.User4:
                        c = "U4";
                        break;
                    case PinComponent.User5:
                        c = "U5";
                        break;
                    default:
                        break;
                }
                return c;
            }

            /// <summary>
            /// Obtains short name of the format.
            /// </summary>
            /// <param name="format">The format.</param>
            /// <returns>Format name.</returns>
            public static string ToString(PinFormat format)
            {
                switch (format)
                {
                    case PinFormat.Integer:
                        return "I";
                    case PinFormat.Integerx2:
                        return "Ix2";
                    case PinFormat.Integerx3:
                        return "Ix3";
                    case PinFormat.Integerx4:
                        return "Ix4";
                    case PinFormat.UInteger:
                        return "UI";
                    case PinFormat.UIntegerx2:
                        return "UIx2";
                    case PinFormat.UIntegerx3:
                        return "UIx3";
                    case PinFormat.UIntegerx4:
                        return "UIx4";
                    case PinFormat.SNorm:
                        return "SN";
                    case PinFormat.SNormx2:
                        return "SNx2";
                    case PinFormat.SNormx3:
                        return "SNx3";
                    case PinFormat.SNormx4:
                        return "SNx4";
                    case PinFormat.UNorm:
                        return "UN";
                    case PinFormat.UNormx2:
                        return "UNx2";
                    case PinFormat.UNormx3:
                        return "UNx3";
                    case PinFormat.UNormx4:
                        return "UNx4";
                    case PinFormat.Float:
                        return "F";
                    case PinFormat.Floatx2:
                        return "Fx2";
                    case PinFormat.Floatx3:
                        return "Fx3";
                    case PinFormat.Floatx4:
                        return "Fx4";
                    case PinFormat.Float2x2:
                        return "F2x2";
                    case PinFormat.Float3x3:
                        return "F3x3";
                    case PinFormat.Float4x4:
                        return "F4x4";
                    case PinFormat.Integer2x2:
                        return "I2x2";
                    case PinFormat.Integer3x3:
                        return "I3x3";
                    case PinFormat.Integer4x4:
                        return "I4x4";
                    case PinFormat.UInteger2x2:
                        return "UI2x2";
                    case PinFormat.UInteger3x3:
                        return "UI3x3";
                    case PinFormat.UInteger4x4:
                        return "UI4x4";
                    case PinFormat.SNorm2x2:
                        return "SN2x2";
                    case PinFormat.SNorm3x3:
                        return "SN3x3";
                    case PinFormat.SNorm4x4:
                        return "SN4x4";
                    case PinFormat.UNorm2x2:
                        return "UN2x2";
                    case PinFormat.UNorm3x3:
                        return "UN3x3";
                    case PinFormat.UNorm4x4:
                        return "UN4x4";
                    default:
                        throw new Exception();
                }
            }

            /// <summary>
            /// Converts element to string.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return ToString(component) + "." + ToString(format);
            }

            /// <summary>
            /// Constructor with string symbolicName.
            /// </summary>
            /// <param name="desc">The symbolicName.</param>
            internal Element(string desc)
            {
                string[] split = desc.Split('.');
                if (split.Length != 2) throw new InvalidVertexFormatException("The vertex format is malformed.");
                offset = 0;

                // We check component first.
                switch (split[0])
                {
                    case "B":
                        component = PinComponent.BiNormal;
                        break;
                    case "P":
                        component = PinComponent.Position;
                        break;
                    case "N":
                        component = PinComponent.Normal;
                        break;
                    case "C":
                        component = PinComponent.Colour;
                        break;
                    case "SC":
                        component = PinComponent.SecondaryColour;
                        break;
                    case "T":
                        component = PinComponent.Tangent;
                        break;
                    case "BW":
                        component = PinComponent.BlendWeights;
                        break;
                    case "BI":
                        component = PinComponent.BlendIndices;
                        break;
                    case "T0":
                        component = PinComponent.TexCoord0;
                        break;
                    case "T1":
                        component = PinComponent.TexCoord1;
                        break;
                    case "T2":
                        component = PinComponent.TexCoord2;
                        break;
                    case "T3":
                        component = PinComponent.TexCoord3;
                        break;
                    case "T4":
                        component = PinComponent.TexCoord4;
                        break;
                    case "T5":
                        component = PinComponent.TexCoord5;
                        break;
                    case "T6":
                        component = PinComponent.TexCoord6;
                        break;
                    case "T7":
                        component = PinComponent.TexCoord7;
                        break;
                    case "T8":
                        component = PinComponent.TexCoord8;
                        break;
                    case "T9":
                        component = PinComponent.TexCoord9;
                        break;
                    case "T10":
                        component = PinComponent.TexCoord10;
                        break;
                    case "T11":
                        component = PinComponent.TexCoord11;
                        break;
                    case "T12":
                        component = PinComponent.TexCoord12;
                        break;
                    case "T13":
                        component = PinComponent.TexCoord13;
                        break;
                    case "T14":
                        component = PinComponent.TexCoord14;
                        break;
                    case "T15":
                        component = PinComponent.TexCoord15;
                        break;
                    case "U0":
                        component = PinComponent.User0;
                        break;
                    case "U1":
                        component = PinComponent.User1;
                        break;
                    case "U2":
                        component = PinComponent.User2;
                        break;
                    case "U3":
                        component = PinComponent.User3;
                        break;
                    case "U4":
                        component = PinComponent.User4;
                        break;
                    case "U5":
                        component = PinComponent.User5;
                        break;
                    default:
                        throw new InvalidVertexFormatException("Unknown component.");
                }

                // We first extract attributes.
                string f = split[1];


                // We extact format.
                switch (f)
                {
                    case "I":
                        format = PinFormat.Integer;
                        break;
                    case "Ix2":
                        format = PinFormat.Integerx2;
                        break;
                    case "Ix3":
                        format = PinFormat.Integerx3;
                        break;
                    case "Ix4":
                        format = PinFormat.Integerx4;
                        break;
                    case "UI":
                        format = PinFormat.UInteger;
                        break;
                    case "UIx2":
                        format = PinFormat.UIntegerx2;
                        break;
                    case "UIx3":
                        format = PinFormat.UIntegerx3;
                        break;
                    case "UIx4":
                        format = PinFormat.UIntegerx4;
                        break;
                    case "F":
                        format = PinFormat.Float;
                        break;
                    case "Fx2":
                        format = PinFormat.Floatx2;
                        break;
                    case "Fx3":
                        format = PinFormat.Floatx3;
                        break;
                    case "Fx4":
                        format = PinFormat.Floatx4;
                        break;
                    // TODO: add others
                    default:
                        throw new InvalidVertexFormatException("The vertex format not recognised.");
                }
            }


        }

        #endregion

        #region Private Members
        private Element[] elements;
        private uint size;
        private int components = 0;
        #endregion

        #region Private Methods
        private VertexFormat(Element[] els)
        {
            size = 0;
            elements = els;

            // Resolve them.
            foreach (Element el in els)
            {
                size = el.Resolve(size);
            }

            // We need to calc components.
            CalcComponents();
        }

        private void CalcComponents()
        {
            foreach (Element el in elements)
            {
                components |= (int)el.Component;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts vertex format to control point format.
        /// </summary>
        /// <returns></returns>
        public ControlPointFormat ToShapeFormat()
        {
            return ControlPointFormat.Parse(this.ToString());
        }

        /// <summary>
        /// Returns element at index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Element at index.</returns>
        public Element this[uint index]
        {
            get
            {
                return elements[index];
            }
        }

        /// <summary>
        /// Creates format from string.
        /// </summary>
        /// <param name="desc">String symbolicName of format.</param>
        /// <returns></returns>
        public static VertexFormat Parse([NotEmpty] string desc)
        {
            // We split on ' ' and create element array.
            string[] split = desc.Split(' ');
            Element[] els = new Element[split.Length];

            // We initialize elements.
            for(int i = 0; i < split.Length; i++)
            {
                els[i] = new Element(split[i]);
            }

            // Create array.
            return new VertexFormat(els);
        }

        /// <summary>
        /// The size of format.
        /// </summary>
        public uint ByteSize
        {
            get { return size; }
        }

        /// <summary>
        /// Calculates the offset of component.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public uint GetOffset(PinComponent component)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Component == component) return elements[i].Offset;
            }
            throw new ArgumentException("The component " + component.ToString() + " does not exist in vertex " +
                " format " + ToString());
        }

        /// <summary>
        /// Obtains format of component.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <returns>Format of component.</returns>
        public PinFormat GetFormat(PinComponent component)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Component == component) return elements[i].Format;
            }
            throw new ArgumentException("The component " + component.ToString() + " does not exist in vertex " +
                "  format " + ToString());
        }

        /// <summary>
        /// Obtains elelemt with component.
        /// </summary>
        /// <param name="component">The component.</param>
        public Element GetElement(PinComponent component)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Component == component) return elements[i];
            }
            throw new ArgumentException("The component " + component.ToString() + " does not exist in vertex " +
                "  format " + ToString());
        }

        /// <summary>
        /// Does this format contain the component.
        /// </summary>
        /// <param name="c">Component to check.</param>
        /// <returns>Indication if it is in format.</returns>
        public bool HasComponent(PinComponent c)
        {
            if ((Components & (int)c) != 0) return true;
            return false;
        }

        /// <summary>
        /// The components of vertex format.
        /// </summary>
        public int Components
        {
            get
            {
                return components;
            }
        }

        /// <summary>
        /// The number of elements in vertex format.
        /// </summary>
        public uint ElementCount
        {
            get { return (uint)elements.Length; }
        }

        #endregion

        #region IEquatable<VertexFormat> Members

        public bool Equals(VertexFormat other)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<Element> Members

        public IEnumerator<VertexFormat.Element> GetEnumerator()
        {
            foreach (Element el in elements)
            {
                yield return el;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (Element el in elements)
            {
                yield return el;
            }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class VertexFormatTest
    {
        [CorrectnessTest]
        public void ParseAndTest()
        {
            VertexFormat fmt = VertexFormat.Parse("P.Fx3 N.[D]Fx3 T0.[H]Fx2");
            
            Assert.AreEqual(12 + 24 + 4, fmt.ByteSize);
            Assert.AreEqual((int)PinComponent.Position | (int)PinComponent.Normal
                | (int)PinComponent.TexCoord0, fmt.Components);
            Assert.AreEqual(3, fmt.ElementCount);
        }

        [CorrectnessTest]
        public void ParseAndTest2()
        {
            VertexFormat fmt = VertexFormat.Parse("P.Fx3 N.[H,SU]Fx3");

            Assert.AreEqual(12 + 6, fmt.ByteSize);
            Assert.AreEqual(2, fmt.ElementCount);
            Assert.AreEqual(0, fmt.GetFormat(PinComponent.Position));
        }
    }
#endif
}
