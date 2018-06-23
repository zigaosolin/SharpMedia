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
using System.IO;
using System.Runtime.InteropServices;

using SharpMedia.Testing;
using Microsoft.Win32.SafeHandles;

namespace SharpMedia.Database.Physical.Windows
{
    /// <summary>
    /// Implementation of windows consistent (write-through) file device.
    /// </summary>
    public class ConsistentFileDevice : Provider.IProvider
    {
        #region Private Imports

        protected const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
        protected const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;
        protected const uint OPEN_EXISTING = 3;
        protected const uint FILE_BEGIN = 0;
        protected const uint FILE_CURRENT = 1;
        protected const uint FILE_END = 2;
        protected const uint GENERIC_READ = 0x80000000;
        protected const uint GENERIC_WRITE = 0x40000000;

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe SafeFileHandle CreateFile
        (
            string FileName,          // file name
            uint DesiredAccess,       // access mode
            uint ShareMode,           // share mode
            uint SecurityAttributes,  // Security Attributes
            uint CreationDisposition, // how to create
            uint FlagsAndAttributes,  // file attributes
            int hTemplateFile         // handle to template file
        );

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool ReadFile
        (
            SafeFileHandle hFile,         // handle to file
            byte* pBuffer,            // data buffer
            int NumberOfBytesToRead,  // number of bytes to read
            out int pNumberOfBytesRead,  // number of bytes read
            IntPtr Overlapped            // overlapped buffer
        );

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool WriteFile
        (
            SafeFileHandle hFile,
            byte* lpBuffer,
            int nNumberOfBytesToWrite,
            out int lpNumberOfBytesWritten,
            IntPtr lpOverlapped
        );

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool GetFileSizeEx
        (
            SafeFileHandle hFile,
            long* lenght
        );

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool CloseHandle
        (
            SafeFileHandle hObject            // handle to object
        );

        [DllImport("kernel32", SetLastError= true)]
        static extern unsafe bool SetFilePointerEx
        (
            SafeFileHandle hObject,
            long DistanceToMove,
            long* NewFilePointer,
            uint MoveMethod
        );

        #endregion

        #region Private Members

        /// <summary>
        /// The filename.
        /// </summary>
        string filename;

        /// <summary>
        /// File handle, used by Windows OS.
        /// </summary>
        SafeFileHandle handle;

        /// <summary>
        /// Size of one block.
        /// </summary>
        uint blockSize;

        /// <summary>
        /// Number of blocks.
        /// </summary>
        ulong blockCount;


        #endregion

        /// <summary>
        /// Creates an immediate writing device (no software or hardware (write) caching).
        /// </summary>
        /// <param name="blockSize">The size of basic block, must be n*512.</param>
        /// <param name="filename">The name of stream to open.</param>
        public unsafe ConsistentFileDevice(uint blockSize, string filename)
        {
            this.filename = filename;
            this.blockSize = blockSize;
            handle = CreateFile(filename, GENERIC_READ|GENERIC_WRITE, 0, 0, 
                OPEN_EXISTING, FILE_FLAG_NO_BUFFERING, 0);
            if(handle.IsInvalid)
            {
                throw new Exception("Could not open file " + filename + " as consistent device.");
            }

            // We compute block count.
            long count;
            GetFileSizeEx(handle, &count);
            this.blockCount = (ulong)count / (ulong)blockSize;
        }

        #region IDevice Members

        public uint BlockSize
        {
            get
            {
                return blockSize;
            }
            set
            {
                blockSize = value;
            }
        }

        public bool Enlarge(ulong size)
        {
            if (size > blockCount)
            {
                throw new NotImplementedException();
            }

            return true;
        }

        public unsafe void Write(ulong address, byte[] data)
        {
            if (!SetFilePointerEx(handle, (long)(address * blockSize), null, FILE_BEGIN))
            {
                throw new Exception();
            }

            fixed (byte* pData = data)
            {
                int written;
                WriteFile(handle, pData, (int)(blockSize), out written, IntPtr.Zero);
            }
            
        }

        public unsafe byte[] Read(ulong address)
        {
            if (!SetFilePointerEx(handle, (long)(address * blockSize), null, FILE_BEGIN))
            {
                throw new Exception();
            }
            byte[] data = new byte[blockSize];
            fixed (byte* pData = data)
            {
                int read;
                ReadFile(handle, pData, (int)(blockSize), out read, IntPtr.Zero);
            }
            
            return data;
        }

        public string GetPhysicalLocation(ulong address)
        {
            return filename + "@" + address.ToString();
        }

        #endregion

        /// <summary>
        /// A helper, creates a file with size and closes it.
        /// </summary>
        /// <param name="str">The name of stream.</param>
        /// <param name="size">The size of stream.</param>
        /// <param name="bSize">The size of basic block.</param>
        public static void CreateFile(string name, ulong size, uint bSize)
        {
            FileStream str = File.Create(name);
            str.SetLength((long)(size * (ulong)bSize));
            str.Dispose();
        }

    }


}
