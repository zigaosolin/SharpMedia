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
using SharpMedia.Resources;
using SharpMedia.Graphics.Shaders;
using System.Reflection;
using System.Threading;
using SharpMedia.Math.Matrix;
using SharpMedia.Math;

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A constant buffer view.
    /// </summary>
    [Serializable]
    public class ConstantBufferView : IResourceView, IGraphicsLocality, IMapable<byte[]>
    {
        #region Static Members

        public static uint MaxSize = 4096 * 4;

        #endregion

        #region Private Members
        ConstantBufferLayout layout;
        TypelessBuffer buffer;
        Driver.ICBufferView view;
        object syncRoot = new object();
        uint usedByDevice = 0;
        bool disposed = false;

        byte[] lockedData;
        #endregion

        #region Internal Methods

        void AssertNotDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ConstantBufferLayout buffer view already disposed");
            }
        }

        void Dispose(bool fin)
        {
            if (!disposed)
            {
                if (view != null)
                {
                    view.Dispose();
                    view = null;
                }
                buffer = null;

                if (!fin)
                {
                    GC.SuppressFinalize(this);
                }
            }
            
        }

        internal void UsedByDevice()
        {
            if (buffer != null)
            {
                buffer.UsedByDevice();
            }

            usedByDevice++;

            if (usedByDevice == 1)
            {
                Monitor.Enter(syncRoot);

            }
        }

        internal void UnusedByDevice()
        {
            usedByDevice--;

            if (usedByDevice == 0)
            {
                Monitor.Exit(syncRoot);
            }

            // May already be disposed.
            if (buffer != null)
            {
                buffer.UnusedByDevice();
            }
        }

        internal Driver.ICBufferView DeviceData
        {
            get
            {
                return view;
            }
        }

        internal ConstantBufferView(TypelessBuffer buffer, ConstantBufferLayout layout)
        {
            this.buffer = buffer;
            this.layout = layout;
        }

        ~ConstantBufferView()
        {
            Dispose(true);
        }

        #endregion

        #region IResourceView Members

        public object TypelessResource
        {
            get { return buffer; }
        }

        #endregion

        #region IGraphicsLocality Members

        public GraphicsLocality Locality
        {
            get
            {
                return buffer.Locality;
            }
            set
            {
                lock (syncRoot)
                {
                    AssertNotDisposed();
                    buffer.Locality = value;
                }
            }
        }

        public void BindToDevice(GraphicsDevice device)
        {
            AssertNotDisposed();
            lock (syncRoot)
            {
                buffer.BindToDevice(device);
                if (view == null)
                {
                    view = device.DriverDevice.CreateCBufferView(buffer.DeviceData);
                }
            }
        }

        public void UnBindFromDevice()
        {
            AssertNotDisposed();
            lock (syncRoot)
            {
                // we do not free shared buffer.

                if (view != null)
                {
                    view.Dispose();
                    view = null;
                }

            }
        }

        public bool IsBoundToDevice
        {
            get { return view != null; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (syncRoot)
            {
                Dispose(false);
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Layout bound to buffer view.
        /// </summary>
        public ConstantBufferLayout Layout
        {
            get
            {
                return layout;
            }
        }

        /// <summary>
        /// Sets the constant.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public unsafe void SetConstant(string name, object value)
        {
            

            AssertNotDisposed();
            if (lockedData == null)
            {
                throw new InvalidOperationException("The buffer is not mapped, cannot set constant.");
            }

            // We set constant (throws if not found).
            uint offset, arraySize;
            PinFormat format;
            if (!layout.TryGetData(name, out offset, out format, out arraySize))
            {
                throw new ArgumentException(string.Format("Parameter {0} does not exist in "+
                    "parameter layout bound to constant buffer view.", name));
            }

            // We validate format.
            if (arraySize == Pin.NotArray)
            {
                if (!PinFormatHelper.IsCompatible(format, value))
                {
                    throw new ArgumentException("Data is not compatible with format.");
                }
            }
            else
            {
                uint arSize;
                if (!PinFormatHelper.IsCompatibleArray(format, value, out arSize)
                    || (arraySize == Pin.DynamicArray && arSize != arraySize))
                {
                    throw new ArgumentException("Data is not compatible with format.");
                }
            }

            fixed (byte* pp = lockedData)
            {
                byte* ppp = pp + offset;

                // We get float pointer.
                float* p = (float*)ppp;

                // We now set data based on type.
                if (value is Matrix4x4f)
                {
                    // Not optimal setting ...
                    Matrix4x4f m = (Matrix4x4f)value;
                    p[0] = m[0, 0];
                    p[1] = m[0, 1];
                    p[2] = m[0, 2];
                    p[3] = m[0, 3];

                    p[4] = m[1, 0];
                    p[5] = m[1, 1];
                    p[6] = m[1, 2];
                    p[7] = m[1, 3];

                    p[8] = m[2, 0];
                    p[9] = m[2, 1];
                    p[10] = m[2, 2];
                    p[11] = m[2, 3];

                    p[12] = m[3, 0];
                    p[13] = m[3, 1];
                    p[14] = m[3, 2];
                    p[15] = m[3, 3];
                }
                else if (value is Vector4f)
                {
                    Vector4f v = (Vector4f)value;
                    p[0] = v.X;
                    p[1] = v.Y;
                    p[2] = v.Z;
                    p[3] = v.W;
                }
                else if (value is Vector3f)
                {
                    Vector3f v = (Vector3f)value;
                    p[0] = v.X;
                    p[1] = v.Y;
                    p[2] = v.Z;
                }
                else if (value is Vector2f)
                {
                    Vector2f v = (Vector2f)value;
                    p[0] = v.X;
                    p[1] = v.Y;
                }
                else if (value is float)
                {
                    float v = (float)value;
                    p[0] = v;
                }
                else if (value is int)
                {
                    int i = (int)value;
                    int* ip = (int*)p;
                    ip[0] = i;
                }
                else if (value is Vector2i)
                {
                    Vector2i i = (Vector2i)value;
                    int* ip = (int*)p;
                    ip[0] = i.X;
                    ip[1] = i.Y;
                }
                else if (value is Vector3i)
                {
                    Vector3i i = (Vector3i)value;
                    int* ip = (int*)p;
                    ip[0] = i.X;
                    ip[1] = i.Y;
                    ip[2] = i.Z;
                }
                else if (value is Vector4i)
                {
                    Vector4i i = (Vector4i)value;
                    int* ip = (int*)p;
                    ip[0] = i.X;
                    ip[1] = i.Y;
                    ip[2] = i.Z;
                    ip[3] = i.W;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            
        }

        /// <summary>
        /// Sets the data by using reflection.
        /// </summary>
        /// <remarks>If you define a struct/class with fields named as the constants, they will be
        /// read to the buffer.</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to set.</param>
        public void SetData<T>(T value)
        {
            SetData<T>(string.Empty, value);
        }

        /// <summary>
        /// Sets the data through reflection. All data is scoped under "scope".
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scope"></param>
        /// <param name="value"></param>
        public void SetData<T>(string scope, T value)
        {
            if (scope != "") scope += ".";

            Type t = value.GetType();
            foreach (FieldInfo info in t.GetFields())
            {
                // We read the name-value pair.
                string name = scope + info.Name;
                object v = info.GetValue(value);

                // We set the constant.
                SetConstant(name, v);
            }
        }

        #endregion

        #region IMapable<byte[]> Members

        public byte[] Map(MapOptions op)
        {
            AssertNotDisposed();

            try
            {
                Monitor.Enter(syncRoot);
                lockedData = buffer.Map(op);
                return lockedData;
            }
            catch (Exception)
            {
                lockedData = null;
                Monitor.Exit(syncRoot);
                throw;
            }


        }

        public void UnMap()
        {
            buffer.UnMap();
            lockedData = null;
            Monitor.Exit(syncRoot);
        }

        #endregion

        #region Static Constructors

        /// <summary>
        /// Creates a constant buffer layout.
        /// </summary>
        /// <returns></returns>
        public static ConstantBufferView Create(GraphicsDevice device, Usage usage, CPUAccess access, GraphicsLocality locality,
            ConstantBufferLayout layout)
        {
            TypelessBuffer buffer = new TypelessBuffer(device, usage, BufferUsage.ConstantBuffer, access, 
                locality, layout.MinimumBufferSizeInBytes);

            buffer.DisposeOnViewDispose = true;

            return buffer.CreateConstantBuffer(layout);
        }

        #endregion
    }
}
