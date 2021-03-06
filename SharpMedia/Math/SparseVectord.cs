// This file was generated by TemplateEngine from template source 'SparseVector'
// using template 'SparseVectord. Do not modify this file directly, modify it from template source.

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
using SharpMedia.AspectOriented;

namespace SharpMedia.Math
{

    /// <summary>
    /// Helper for SparseVector construction.
    /// </summary>
    [Serializable]
    public struct SparseElementd : IComparable<SparseElementd>
    {
        /// <summary>
        /// Offset of the element.
        /// </summary>
        public uint Offset;

        /// <summary>
        /// Value of sparse element.
        /// </summary>
        public double Value;

        public SparseElementd(uint off, double val)
        {
            Offset = off;
            Value = val;
        }

        #region IComparable<SparseElementd> Members

        public int CompareTo(SparseElementd other)
        {
            return Offset.CompareTo(other.Offset);
        }

        #endregion
    }

    /// <summary>
    /// Sparse vector is an implementation of vector that has many zeros. Use
    /// Vectord if there are a lot of elements with zeros.
    /// 
    /// Class is not thread safe.
    /// </summary>
    [Serializable]
    public sealed class SparseVectord : IEquatable<SparseVectord>, IEnumerable<SparseElementd>, ICloneable<SparseVectord>
    {
        #region Private Members
        uint size;
        Element root = null;
        uint nonZero = 0;
        #endregion

        #region Private Members
        /// <summary>
        /// An element of vector.
        /// </summary>
        [Serializable]
        private sealed class Element : ICloneable<Element>
        {
            /// <summary>
            /// The offset of element.
            /// </summary>
            public uint Offset;

            /// <summary>
            /// Value of element.
            /// </summary>
            public double Value;

            /// <summary>
            /// Next element, or null.
            /// </summary>
            public Element Next;

            public Element(uint off, double v, Element n)
            {
                Offset = off;
                Value = v;
                Next = n;
            }

            public Element(uint off, double v)
                : this(off, v, null)
            {
            }

            #region ICloneable<Element> Members

            public Element Clone()
            {
                return new Element(Offset, Value);
            }

            #endregion
        }

        #endregion

        #region Properties

        //#ifdef Sqrt


        /// <summary>
        /// Length of vector.
        /// </summary>
        public double Length
        {
            get
            {
                return MathHelper.Sqrt(this * this);
            }
        }

        //#endif

        /// <summary>
        /// Squared length of vector.
        /// </summary>
        public double Length2
        {
            get
            {
                return this * this;
            }
        }

        /// <summary>
        /// Number of dimensions of this sparse vector.
        /// </summary>
        public uint DimensionsCount
        {
            get { return size; }
        }

        /// <summary>
        /// Number of nonzero elements.
        /// </summary>
        public uint NonZeroCount
        {
            get { return nonZero; }
        }

        /// <summary>
        /// Element accessor. If you go in accending order, the indexing may be optimized.
        /// </summary>
        /// <param name="index">The index, must be in range.</param>
        public double this[uint index]
        {
            get
            {
                if (root == null) return 0.0;
                if (index >= size) throw new ArgumentException("Cannot access elements out of bound.");


                for (Element el = root, prev; ; )
                {
                    // We check for element.
                    if (el.Offset > index)
                    {
                        return 0.0;
                    }
                    if (el.Offset == index)
                    {
                        return el.Value;
                    }

                    if (el.Next != null)
                    {
                        prev = el;
                        el = el.Next;
                    }
                    else
                    {
                        break;
                    }
                }
                return 0.0;
            }
            set
            {
                if (index >= size) throw new ArgumentException("Cannot access elements out of bound.");


                // Transverse vector.
                if (root == null)
                {
                    if (value != 0.0)
                    {
                        root = new Element(index, value);
                    }
                }
                else
                {
                    Element prev = null;
                    for (Element el = root; ; )
                    {
                        // Check if this is the element.
                        if (el.Offset == index)
                        {
                            if (value != 0.0)
                            {
                                el.Value = value;
                            }
                            else
                            {
                                // We need to delete the element.
                                if (prev == null)
                                {
                                    // Just relink root.
                                    root = el.Next;
                                    nonZero--;
                                }
                                else
                                {
                                    // Delete it.
                                    prev.Next = el.Next;
                                    nonZero--;
                                }
                            }
                            return;
                        }

                        // We check if we are through.
                        if (el.Offset > index)
                        {
                            // We append it.
                            if (value != 0.0)
                            {
                                if (prev != null)
                                {
                                    prev.Next = new Element(index, value);
                                    prev.Next.Next = el;
                                    nonZero++;
                                }
                                else
                                {
                                    root = new Element(index, value);
                                    root.Next = el;
                                    nonZero++;
                                }
                            }
                            return;
                        }

                        if (el.Next != null)
                        {
                            // We need previous element.
                            prev = el;
                            el = el.Next;
                        }
                        else
                        {
                            break;
                        }

                    }
                }
            }
        }

        #endregion

        #region Operations on Vector

        /// <summary>
        /// Adds two sparse vectors.
        /// </summary>
        /// <param name="s1">The first vector.</param>
        /// <param name="s2">The second vector.</param>
        /// <returns>Result.</returns>
        public static SparseVectord operator +([NotNull] SparseVectord s1, [NotNull] SparseVectord s2)
        {
            if (s1.size != s2.size) throw new ArgumentException("Cannot add vectors with different sizes.");

            // We hint number of nodes.
            List<SparseElementd> els = new List<SparseElementd>((int)(s1.nonZero + s2.nonZero));

            Element e1 = s1.root;
            Element e2 = s2.root;

            // We first add the common part, until one is null.
            if (e1 != null && e2 != null)
            {
                while (true)
                {
                    int offdir = (int)e2.Offset - (int)e1.Offset;
                    if (offdir > 0)
                    {
                        // We add only smaller.
                        els.Add(new SparseElementd(e1.Offset, e1.Value));
                        e1 = e1.Next;
                        if (e1 == null) break;
                    }
                    else if (offdir < 0)
                    {
                        // We add only smaller.
                        els.Add(new SparseElementd(e2.Offset, e2.Value));
                        e2 = e2.Next;
                        if (e2 == null) break;
                    }
                    else
                    {
                        els.Add(new SparseElementd(e2.Offset, e1.Value + e2.Value));
                        e1 = e1.Next;
                        e2 = e2.Next;
                        if (e1 == null) break;
                        if (e2 == null) break;
                    }

                }
            }

            // We now only have to finalize addition, only one is valid.
            while (e1 != null)
            {
                els.Add(new SparseElementd(e1.Offset, e1.Value));
                e1 = e1.Next;
            }

            while (e2 != null)
            {
                els.Add(new SparseElementd(e2.Offset, e2.Value));
                e2 = e2.Next;
            }

            // Finaly return vector.
            return new SparseVectord(els, s1.size);
        }

        /// <summary>
        /// Substracts two sparse vectors.
        /// </summary>
        /// <param name="s1">The first vector.</param>
        /// <param name="s2">The second vector.</param>
        /// <returns>Result.</returns>
        public static SparseVectord operator -([NotNull] SparseVectord s1, [NotNull] SparseVectord s2)
        {
            if (s1.size != s2.size) throw new ArgumentException("Cannot add vectors with different sizes.");

            // We hint number of nodes.
            List<SparseElementd> els = new List<SparseElementd>((int)(s1.nonZero + s2.nonZero));

            Element e1 = s1.root;
            Element e2 = s2.root;

            // We first add the common part, until one is null.
            if (e1 != null && e2 != null)
            {
                while (true)
                {
                    int offdir = (int)e2.Offset - (int)e1.Offset;
                    if (offdir > 0)
                    {
                        // We add only smaller.
                        els.Add(new SparseElementd(e1.Offset, e1.Value));
                        e1 = e1.Next;
                        if (e1 == null) break;
                    }
                    else if (offdir < 0)
                    {
                        // We add only smaller.
                        els.Add(new SparseElementd(e2.Offset, -e2.Value));
                        e2 = e2.Next;
                        if (e2 == null) break;
                    }
                    else
                    {
                        els.Add(new SparseElementd(e2.Offset, e1.Value - e2.Value));
                        e1 = e1.Next;
                        e2 = e2.Next;
                        if (e1 == null) break;
                        if (e2 == null) break;
                    }

                }
            }

            // We now only have to finalize addition, only one is valid.
            while (e1 != null)
            {
                els.Add(new SparseElementd(e1.Offset, e1.Value));
                e1 = e1.Next;
            }

            while (e2 != null)
            {
                els.Add(new SparseElementd(e2.Offset, -e2.Value));
                e2 = e2.Next;
            }

            // Finaly return vector.
            return new SparseVectord(els, s1.size);
        }

        /// <summary>
        /// Dot product on sparse vectors.
        /// </summary>
        /// <param name="s1">The first vector.</param>
        /// <param name="s2">The second vector.</param>
        /// <returns>Result.</returns>
        public static double operator *([NotNull] SparseVectord s1, [NotNull] SparseVectord s2)
        {
            if (s1.size != s2.size) throw new ArgumentException("Cannot add vectors with different sizes.");


            double result = 0.0;
            Element e1 = s1.root;
            Element e2 = s2.root;

            // We first add the common part, until one is null.
            if (e1 != null && e2 != null)
            {
                while (true)
                {
                    // Roll until offset not bigger or equal to other vector element.
                    while (e1.Offset < e2.Offset)
                    {
                        e1 = e1.Next;
                        if (e1 == null) return result;
                    }

                    // Roll until offset not bigger or equal to other vector element.
                    while (e2.Offset < e1.Offset)
                    {
                        e2 = e2.Next;
                        if (e2 == null) return result;
                    }

                    if (e1.Offset == e2.Offset)
                    {
                        result += e1.Value * e2.Value;
                        e1 = e1.Next;
                        if (e1 == null) return result;
                        e2 = e2.Next;
                        if (e2 == null) return result;
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// Scales this sparse vector by value.
        /// </summary>
        /// <param name="f">The factor.</param>
        public void Scale(double f)
        {
            for (Element el = root; el != null; el = el.Next)
            {
                el.Value *= f;
            }
        }

        /// <summary>
        /// Multiplies vector by scalar.
        /// </summary>
        public static SparseVectord operator *([NotNull] SparseVectord v, double f)
        {
            SparseVectord r = (SparseVectord)v.Clone();
            r.Scale(f);
            return r;
        }

        /// <summary>
        /// Multiplies vector by scalar.
        /// </summary>
        public static SparseVectord operator *(double f, [NotNull] SparseVectord v)
        {
            return v * f;
        }

        /// <summary>
        /// Sparse-dense vector multiplication.
        /// </summary>
        /// <param name="v1">The sparse vector.</param>
        /// <param name="v2">The dense vector.</param>
        /// <returns>Dot product.</returns>
        public static double operator *([NotNull] SparseVectord v1, [NotNull] Vectord v2)
        {
            if (v1.DimensionsCount != v2.DimensionCount)
                throw new ArgumentException("Vectors not compatible in sizes.");

            double r = 0.0;
            for (Element el = v1.root; el != null; el = el.Next)
            {
                r += el.Value * v2[el.Offset];
            }

            return r;
        }

        /// <summary>
        /// Divides by scalar.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="f">Division factor; must not be null.</param>
        /// <returns>Sparse vector after division.</returns>
        public static SparseVectord operator /([NotNull] SparseVectord v, double f)
        {
            return v * (1.0 / f);
        }

        /// <summary>
        /// Compares two sparse vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns></returns>
        public static bool operator ==([NotNull] SparseVectord v1, [NotNull] SparseVectord v2)
        {
            if (v1.size != v2.size) return false;
            if (v1.nonZero != v2.nonZero) return false;

            for (Element el1 = v1.root, el2 = v2.root; el1 != null; el1 = el1.Next, el2 = el2.Next)
            {
                if (el1.Offset != el2.Offset) return false;
                if (el1.Value != el2.Value) return false;
            }

            return true;
        }

        /// <summary>
        /// Compares two sparse vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Are vectors different.</returns>
        public static bool operator !=([NotNull] SparseVectord v1, [NotNull] SparseVectord v2)
        {
            return !(v1 == v2);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Sparse vector with elements.
        /// </summary>
        /// <param name="els">The elements.</param>
        public SparseVectord([NotNull] double[] els)
            : this(new Vectord(els))
        {
        }

        /// <summary>
        /// Constructor with sorted array.
        /// </summary>
        /// <param name="sorted">Sorted array list.</param>
        /// <param name="siz">Size of sparse vector.</param>
        public SparseVectord([NotNull] SparseElementd[] els, uint siz)
        {
            size = siz;
            if (els.Length == 0) return;

            // We now construct vector using elements. We first
            // need to write to root.
            root = new Element(els[0].Offset, els[0].Value);
            nonZero = (uint)els.Length;
            Element current = root;

            // We add elements.
            for (int i = 1; i < els.Length; i++)
            {
                current.Next = new Element(els[i].Offset, els[i].Value);
                current = current.Next;
            }

            // We extract size.
            if (size < current.Offset) throw new ArgumentException("Sparse array goes beyond size.");
        }

        /// <summary>
        /// Constructor using sparse elements.
        /// </summary>
        /// <param name="els">The element list.</param>
        /// <param name="size">The size of sparse vector.</param>
        /// <param name="sorted">Are elements sorted.</param>
        public SparseVectord([NotNull] List<SparseElementd> els, uint size, bool sorted)
        {
            this.size = size;
            if (!sorted) els.Sort();
            if (els.Count == 0) { return; }

            // We now construct vector using elements. We first
            // need to write to root.
            root = new Element(els[0].Offset, els[0].Value);
            nonZero = (uint)els.Count;
            Element current = root;

            // We add elements.
            for (int i = 1; i < els.Count; i++)
            {
                current.Next = new Element(els[i].Offset, els[i].Value);
                current = current.Next;
            }

            // We extract size.
            if (size < current.Offset) throw new ArgumentException("Sparse array goes beyond size.");
        }

        /// <summary>
        /// Constructor using sparse elements. They are not sorted.
        /// </summary>
        /// <param name="els">The element list.</param>
        /// <param name="siz">The size of sparse vector.</param>
        public SparseVectord([NotNull] List<SparseElementd> els, uint size)
            : this(els, size, false)
        {
        }

        /// <summary>
        /// Constructs from ordinary vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public SparseVectord([NotNull] Vectord vector)
        {
            size = vector.DimensionCount;

            Element current = null;
            for (uint i = 0; i < size; i++)
            {
                if (vector[i] != 0.0)
                {
                    if (root != null)
                    {
                        root = current = new Element(i, vector[i]);
                    }
                    else
                    {
                        current.Next = new Element(i, vector[i]);
                        current = current.Next;
                    }
                    nonZero++;
                }
            }
        }

        /// <summary>
        /// Constructor with only size.
        /// </summary>
        /// <param name="size">The size of vector.</param>
        public SparseVectord(uint size)
        {
            this.size = size;
            nonZero = 0;
        }

        #endregion

        #region Overrides

        public override bool Equals([NotNull] object obj)
        {
            if (obj is SparseVectord)
            {
                return (SparseVectord)obj == this;
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder((int)DimensionsCount * 3);
            Element el = root;
            builder.Append("( ");
            for (uint i = 0; ; )
            {
                if (el != null && el.Offset == i)
                {
                    builder.Append(el.Value);
                    el = el.Next;
                }
                else
                {
                    builder.Append(0.0);
                }
                i++;
                if (i < DimensionsCount)
                {
                    builder.Append(" ,");
                }
                else
                {
                    break;
                }
            }
            builder.Append(" )");
            return builder.ToString();

        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (nonZero + DimensionsCount).GetHashCode();
            }
        }

        #endregion

        #region IEquatable<SparseVectord> Members

        public bool Equals(SparseVectord other)
        {
            return this == other;
        }

        #endregion

        #region IEnumerable<SparseElementd> Members

        public IEnumerator<SparseElementd> GetEnumerator()
        {
            Element el = root;
            while (el != null)
            {
                yield return new SparseElementd(el.Offset, el.Value);
                el = el.Next;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            Element el = root;
            while (el != null)
            {
                yield return new SparseElementd(el.Offset, el.Value);
                el = el.Next;
            }
        }

        #endregion

        #region ICloneable<SparseVectord> Members

        public SparseVectord Clone()
        {
            // Create new object.
            SparseVectord v = new SparseVectord(this.size);
            v.nonZero = this.nonZero;

            if (this.root != null)
            {
                v.root = (Element)this.root.Clone();

                Element el = this.root.Next;
                Element prev = v.root;
                while (el != null)
                {
                    // We clone it first.
                    prev.Next = (Element)el.Clone();
                    prev = prev.Next;
                    el = el.Next;
                }
            }

            return v;
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class Test_SparseVectord
    {

        SparseVectord v1 = new SparseVectord(
            new SparseElementd[]{
                new SparseElementd(1,(double)2.0),
                new SparseElementd(5,(double)7.0),
                new SparseElementd(12,(double)5.0)}, 15);

        SparseVectord v2 = new SparseVectord(
            new SparseElementd[]{
                new SparseElementd(2,(double)2.0),
                new SparseElementd(5,(double)7.0),
                new SparseElementd(14,(double)5.0)}, 15);

        [CorrectnessTest]
        public void ElementConstructionAndAccess()
        {
            // We construct giant sparse vector.
            SparseVectord v = new SparseVectord(
                new SparseElementd[]{ 
                    new SparseElementd(1, (double)2.0),
                    new SparseElementd(5, (double)3.0),
                    new SparseElementd(12, (double)4.0),
                    new SparseElementd(115, (double)70.0)}, 150);

            // Accessor test.
            Assert.AreEqual(0.0, v[7]);
            Assert.AreEqual((double)3.0, v[5]);
            Assert.AreEqual((double)70.0, v[115]);
            Assert.AreEqual(0.0, v[145]);

            // Setting test.
            v[15] = (double)6.0;
            Assert.AreEqual((double)6.0, v[15]);
            v[5] = (double)3.0;
            Assert.AreEqual((double)3.0, v[5]);
        }

        [CorrectnessTest]
        public void Add()
        {
            SparseVectord add = v1 + v2;

            Assert.AreEqual((double)2.0, add[1]);
            Assert.AreEqual((double)2.0, add[2]);
            Assert.AreEqual(0.0, add[3]);
            Assert.AreEqual((double)14.0, add[5]);
            Assert.AreEqual((double)5.0, add[12]);
            Assert.AreEqual((double)5.0, add[14]);
        }

        [CorrectnessTest]
        public void Sub()
        {
            SparseVectord sub = v1 - v2;

            Assert.AreEqual((double)2.0, sub[1]);
            Assert.AreEqual((double)-2.0, sub[2]);
        }

        [CorrectnessTest]
        public void Dot()
        {
            double r = v1 * v2;
            Assert.AreEqual((double)49.0, r);
        }
    }
#endif
}
