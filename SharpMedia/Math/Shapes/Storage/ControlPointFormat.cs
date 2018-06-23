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

namespace SharpMedia.Math.Shapes.Storage
{


    /// <summary>
    /// Provides format for control points layout in buffers. You can define any type of format you
    /// want but it must be "full" (e.g. no blank spaces). Format is immutable.
    /// </summary>
    [Serializable]
    public class ControlPointFormat : IEquatable<ControlPointFormat>, IEnumerable<ControlPointFormat.Element>
    {
        #region ControlPoint Element

        /// <summary>
        /// An element of vertex format.
        /// </summary>
        public class Element
        {
            string component;
            Type format;
            uint offset;

            /// <summary>
            /// Helper, obtains element's size.
            /// </summary>
            /// <param name="fmt">The format.</param>
            /// <returns></returns>
            public static uint FormatSize(Type fmt)
            {
                uint size;
                switch (fmt.FullName)
                {
                    case "System.Single":
                    case "System.Int32":
                    case "System.UInt32":
                        return 4;
                    case "System.Int16":
                    case "SharpMedia.Math.Half":
                        return 2;
                    default:
                        throw new Exception("The format is not known, cannot be used.");
                }
            }

            /// <summary>
            /// Component of the element.
            /// </summary>
            public string Component
            {
                get { return component; }
            }

            /// <summary>
            /// Format of the component.
            /// </summary>
            public Type Format
            {
                get { return format; }
            }

            /// <summary>
            /// Alias for format.
            /// </summary>
            public Type Type
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
            internal Element(string c, Type fmt)
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
            public static string ToString(string component)
            {
                return component;
            }

            /// <summary>
            /// Obtains short name of the format.
            /// </summary>
            /// <param name="format">The format.</param>
            /// <returns>Format name.</returns>
            public static string ToString(Type format)
            {
                switch (format.FullName)
                {
                    case "System.Int32":
                        return "I";
                    case "SharpMedia.Math.Vector2i":
                        return "Ix2";
                    case "SharpMedia.Math.Vector3i":
                        return "Ix3";
                    /*
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
                        return "UN4x4";*/
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
                if (split.Length != 2) throw new ArgumentException("The vertex format is malformed.");
                offset = 0;

                // We check component first.
                component = split[0];

                // We first extract attributes.
                string f = split[1];


                // We extact format.
                switch (f)
                {
                    case "I":
                        format = typeof(int);
                        break;
                    case "Ix2":
                        format = typeof(Vector2i);
                        break;
                    case "Ix3":
                        format = typeof(Vector3i);
                        break;
                    case "Ix4":
                        format = typeof(Vector4i);
                        break;
                    /*
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
                        break;*/
                    // TODO: add others
                    default:
                        throw new Exception("The control point format not recognised.");
                }
            }


        }

        #endregion

        #region Private Members
        private Element[] elements;
        private uint size;
        #endregion

        #region Private Methods
        private ControlPointFormat(Element[] els)
        {
            size = 0;
            elements = els;

            // Resolve them.
            foreach (Element el in els)
            {
                size = el.Resolve(size);
            }

        }

        #endregion

        #region Public Methods

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
        public static ControlPointFormat Parse([NotEmpty] string desc)
        {
            // We split on ' ' and create element array.
            string[] split = desc.Split(' ');
            Element[] els = new Element[split.Length];

            // We initialize elements.
            for (int i = 0; i < split.Length; i++)
            {
                els[i] = new Element(split[i]);
            }

            // Create array.
            return new ControlPointFormat(els);
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
        public uint GetOffset(string component)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Component == component) return elements[i].Offset;
            }
            throw new ArgumentException("The component " + component.ToString() + " does not exist in control point " +
                " format " + ToString());
        }

        /// <summary>
        /// Obtains format of component.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <returns>Format of component.</returns>
        public Type GetFormat(string component)
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
        public Element GetElement(string component)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Component == component) return elements[i];
            }
            throw new ArgumentException("The component " + component.ToString() + " does not exist in control point " +
                "  format " + ToString());
        }

        /// <summary>
        /// Checks if component exists in format.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool HasComponent(string component)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Component == component) return true;
            }
            return false;
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

        public bool Equals(ControlPointFormat other)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<Element> Members

        public IEnumerator<ControlPointFormat.Element> GetEnumerator()
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
    internal class ControlPointFormatTest
    {

    }
#endif
}
