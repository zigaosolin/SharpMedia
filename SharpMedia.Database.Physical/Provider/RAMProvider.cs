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

namespace SharpMedia.Database.Physical.Provider
{

    /// <summary>
    /// Provides blocks that reside in RAM. It is only used as testing provider (only
    /// simple tests). It has features like:
    /// - testing that write happens before read
    /// </summary>
    internal class RAMProvider : IProvider
    {
        #region Private Members
        private uint blockSize;
        private ulong blockCount;
        private byte[][] buffer;
        #endregion

        #region IProvider Members

        public uint BlockSize
        {
            get
            {
                return blockSize;
            }
            set
            {
                throw new NotImplementedException("Resizing not supported.");
            }
        }

        public bool Enlarge(ulong size)
        {
            if (size == (ulong)buffer.LongLength) return true;

            throw new InvalidOperationException("RAM database cannot be resized.");
        }

        public void Write(ulong addr, byte[] block)
        {
            if (addr >= blockCount)
            {
                throw new IndexOutOfRangeException("addr");
            }

            buffer[addr] = (byte[])block.Clone();
        }

        public byte[] Read(ulong addr)
        {
            if (addr >= blockCount)
            {
                throw new IndexOutOfRangeException("addr");
            }

            // Must not allow reads on invalid location.
            byte[] b = buffer[addr];
            if(b == null) 
            {
                throw new InvalidOperationException("Cannot read before write.");
            }


            //return b;

            // May need to clone but since we are careful not to overwrite blocks, this
            // is not needed.
            return (byte[])b.Clone();
        }

        public unsafe string GetPhysicalLocation(ulong address)
        {
            byte[] b = buffer[address];
            fixed(byte* p = b)
            {
                IntPtr i = (IntPtr)p;
                return "RAM:@" + i.ToString();
            }
        }

        #endregion

        #region Constructors

        public RAMProvider(ulong blockCount, uint blockSize)
        {
            if (!BlockHelper.IsValidSize(blockSize))
            {
                throw new ArgumentException("blockSize");
            }

            this.blockCount = blockCount;
            this.blockSize = blockSize;
            this.buffer = new byte[this.blockCount][];
        }

        #endregion

    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class RAMProviderTest 
    {

        [CorrectnessTest]
        public void ReadWrite()
        {
            RAMProvider p = new RAMProvider(1024, 512);
            Assert.AreEqual(512, p.BlockSize);

            byte[] b = new byte[512];
            p.Write(1, b);
            Assert.AreEqual(b, p.Read(1));
        }

    }

#if NOT_DEFINED
    [TestSuite]
    internal class RAMNodeTest : PhysicalNodeTest
    {

        public override IProvider CreateProvider(ulong blockCount, uint blockSize)
        {
            return new RAMProvider(blockCount, blockSize);
        }

    }
#endif
#endif
}
