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

namespace SharpMedia.Database.Physical
{

    /// <summary>
    /// A boolean array is array of booleans in packed form; e.g. 32 booleans in one uint.
    /// It does keep data in uint[] which can be shared so there is no need to copy when
    /// reading directly from block.
    /// </summary>
    [Serializable]
    internal unsafe class BoolArray : ICloneable
    {
        #region Static Members

        private readonly static ulong[] bitMask = new ulong[]{
                1,
                2,
                4,
                8,
                16,
                32,
                64,
                128,
                256,
                512,
                1024,
                2048,
                4096,
                8192,
                16384,
                32768,
                65536,
                131072,
                262144,
                524288,
                1048576,
                2097152,
                4194304,
                8388608,
                16777216,
                33554432,
                67108864,
                134217728,
                268435456,
                536870912,
                1073741824,
                2147483648,
                4294967296,
                8589934592,
                17179869184,
                34359738368,
                68719476736,
                137438953472,
                274877906944,
                549755813888,
                1099511627776,
                2199023255552,
                4398046511104,
                8796093022208,
                17592186044416,
                35184372088832,
                70368744177664,
                140737488355328,
                281474976710656,
                562949953421312,
                1125899906842624,
                2251799813685248,
                4503599627370496,
                9007199254740992,
                18014398509481984,
                36028797018963968,
                72057594037927936,
                144115188075855872,
                288230376151711744,
                576460752303423488,
                1152921504606846976,
                2305843009213693952,
                4611686018427387904,
                9223372036854775808
            
        };

        #endregion

        #region Private Members
        int count = 0;
        byte[] data = null;
        private const int IntSizeInBits = 64;
        private const int IntMask = 63;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolArray"/> class.
        /// </summary>
        /// <param name="contents">The contents.</param>
        public BoolArray(byte[] contents)
            : this(contents, contents.Length*IntSizeInBits)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolArray"/> class.
        /// </summary>
        /// <param name="contents">The contents.</param>
        /// <param name="length">The length.</param>
        public BoolArray(byte[] contents, int length)
        {
            if (length != contents.Length*IntSizeInBits ||
                contents.Length % 8 != 0)
            {
                throw new ArgumentException("Contents not big enough for specified length or not 8-byte aligned");
            }
            data = contents;
            count = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolArray"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        public BoolArray(int length)
        {

            count = length;
            data = new byte[((count + 63) / 64)*8];
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets or sets the <see cref="System.Boolean"/> at the specified index.
        /// </summary>
        /// <value>The value.</value>
        public bool this[uint index]
        {
            get
            {
                return this[(int)index];
            }
            set
            {
                this[(int)index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Boolean"/> at the specified index.
        /// </summary>
        /// <value></value>
        public bool this[int index]
        {
            get
            {
                if (index < 0 || index >= count) throw new IndexOutOfRangeException("Cannot access of range.");
                fixed (byte* p = data)
                {
                    ulong* d = (ulong*)p;
                    return (d[index / IntSizeInBits] & bitMask[index & IntMask]) != 0;
                }
            }
            set
            {
                if (index < 0 || index >= count) throw new IndexOutOfRangeException("Cannot access of range.");
                fixed (byte* p = data)
                {
                    ulong* d = (ulong*)p;
                    if (value)
                    {
                        d[index / IntSizeInBits] |= bitMask[index & IntMask];
                    }
                    else
                    {
                        d[index / IntSizeInBits] &= ~bitMask[index & IntMask];
                    }
                }
            }
        }

        /// <summary>
        /// Changes the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void Change(uint index)
        {
            if (index < 0 || index >= count) throw new IndexOutOfRangeException("Cannot access of range.");
            fixed (byte* p = data)
            {
                ulong* d = (ulong*)p;
                if ((d[index / IntSizeInBits] & bitMask[index & IntMask]) != 0)
                {

                    d[index / IntSizeInBits] |= bitMask[index & IntMask];
                }
                else
                {
                    d[index / IntSizeInBits] &= ~bitMask[index & IntMask];
                }
            }
        }

        /// <summary>
        /// Is 8-byte full (all ones).
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public bool IsULongFull(uint index)
        {
            if (index*8 >= count) throw new IndexOutOfRangeException("Cannot access out of range.");
            fixed (byte* b = data)
            {
                ulong* p = (ulong*)b;
                return p[index] == ulong.MaxValue;
            }
        }

        /// <summary>
        /// Is 8-byte free (all zeros).
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public bool IsULongFree(uint index)
        {
            if (index * 8 >= count) throw new IndexOutOfRangeException("Cannot access out of range.");
            fixed (byte* b = data)
            {
                ulong* p = (ulong*)b;
                return p[index] == 0;
            }
        }

        /// <summary>
        /// Sets all bits to false.
        /// </summary>
        public void Initialize()
        {
            fixed (byte* p = data)
            {
                ulong* d = (ulong*)p;
                for (int i = 0; i < data.Length / sizeof(ulong); i++)
                {
                    d[i] = 0;
                }
            }
        }

        /// <summary>
        /// Initializes array to specified value.
        /// </summary>
        /// <param name="allValue">The value to initialize array.</param>
        public void Initialize(bool allValue)
        {
            if (allValue)
            {
                fixed (byte* p = data)
                {
                    ulong* d = (ulong*)p;
                    for (int i = 0; i < data.Length / sizeof(ulong); i++)
                    {
                        d[i] = ulong.MaxValue;
                    }
                }
            }
            else
            {
                Initialize();
            }
        }

        /// <summary>
        /// Checks if all are false. Works correctly only if lenght multiplier
        /// of number of bits in integer (that is 32).
        /// </summary>
        public bool IsAllFalse
        {
            get
            {
                fixed (byte* p = data)
                {
                    ulong* d = (ulong*)p;
                    for (int i = 0; i < data.Length / sizeof(ulong); i++)
                    {
                        if (d[i] != 0) return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Checks if all are true. Works correctly only if lenght multiplier
        /// of number of bits in integer (that is 32).
        /// </summary>
        public bool IsAllTrue
        {
            get
            {
                fixed (byte* p = data)
                {
                    ulong* d = (ulong*)p;
                    for (int i = 0; i < data.Length / sizeof(ulong); i++)
                    {
                        if (d[i] != ulong.MaxValue) return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data
        {
            get
            {
                return data;
            }
        }

        #endregion

        #region ICollection<bool> Members

        public void Clear()
        {
            data.Initialize();
        }

        public int Count
        {
            get { return count; }
        }

        #endregion

        #region IEnumerable<bool> Members

        public IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return this[i];
            }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            byte[] newData = new byte[data.Length];
            data.CopyTo(newData, 0);
            return new BoolArray(newData, count);
        }

        #endregion
    }


#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class BoolArrayTest
    {
        [CorrectnessTest]
        public void Properties1()
        {
            BoolArray array = new BoolArray(10);
            array.Initialize();

            // We now try accesses.
            array[0] = true;
            array[1] = false;
            Assert.AreEqual(true, array[0]);
            Assert.AreEqual(false, array[1]);
            Assert.AreEqual(false, array[2]);

            array.Initialize(true);
            Assert.AreEqual(true, array[2]);
        }

        [CorrectnessTest]
        public void Properties2()
        {
            BoolArray array = new BoolArray(new byte[8]);
            array.Initialize(true);
            Assert.AreEqual(true, array.IsAllTrue);
            array.Initialize(false);
            Assert.AreEqual(true, array.IsAllFalse);
            array[40] = true;

            // We also check accessors over one byte.
            Assert.AreEqual(true, array[40]);
        }
    }
#endif
}
