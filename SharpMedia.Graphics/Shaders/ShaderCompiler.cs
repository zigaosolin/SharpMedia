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
using System.Runtime.Serialization;
using SharpMedia.AspectOriented;
using SharpMedia.Graphics.States;

namespace SharpMedia.Graphics.Shaders
{

    /// <summary>
    /// Expand options.
    /// </summary>
    public enum ExpandType
    {
        /// <summary>
        /// Adds appropriate number of zeros for expansion.
        /// </summary>
        AddZeros,

        /// <summary>
        /// Add appropriate number of ones for expansion.
        /// </summary>
        AddOnes,

        /// <summary>
        /// Add zeros everywhere and ones on w channel (for matrices at the end of each row).
        /// </summary>
        AddOnesAtW

    }

    /// <summary>
    /// Incompatible operands exception is thrown when instruction with incompatible
    /// format is issued.
    /// </summary>
    public class IncompatibleOperandsException : Exception
    {
        /// <summary>
        /// IncompatibleOperandsException base constructor.
        /// </summary>
        public IncompatibleOperandsException()
            : base("Formats incompatible.")
        {
        }

        /// <summary>
        /// Incompatible operands exception with description.
        /// </summary>
        /// <param name="m">The message.</param>
        public IncompatibleOperandsException(string m)
            : base(m)
        {
        }

        /// <summary>
        /// Incompatible operands exception with description.
        /// </summary>
        /// <param name="m">The message.</param>
        public IncompatibleOperandsException(string m, Exception e)
            : base(m, e)
        {
        }

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="c"></param>
        public IncompatibleOperandsException(SerializationInfo info, StreamingContext c)
            : base(info, c)
        {
        }

    }

    /// <summary>
    /// Shader functions.
    /// </summary>
    public enum ShaderFunction
    {
        Abs,
        Floor,
        Ceil,
        Length,

        // Booleans functions.
        All,
        Any,
        None
    }


    /// <summary>
    /// Shader compiler is constructed using device and can compile shaders. It is
    /// thread-safe using Begin/End pair. You can reuse the same object for compiling more shaders
    /// and this is desirable. It is optimal to have one shader per thread.
    /// </summary>
    public sealed class ShaderCompiler : IDisposable
    {
        #region Operands

        /// <summary>
        /// An operand, it is read-write element. It is almost identical to pin except that:
        /// - all array sizes are fixed
        /// - fixed pins have asociated values
        /// - it can be written to (if not writable, that is if temporary)
        /// </summary>
        public class Operand : IEquatable<Operand>
        {
            #region Private Members
            private PinFormat format;
            private PinFormat textureFormat;
            private int name;
            private uint size = uint.MaxValue;
            private object value = null;
            private bool writable = false;
            #endregion

            #region Internal Methods

            /// <summary>
            /// Construction of operand.
            /// </summary>
            /// <param name="fmt">The format.</param>
            /// <param name="n">The name of operand.</param>
            internal Operand(PinFormat fmt, int n)
            {
                format = fmt;
                name = n;
            }

            /// <summary>
            /// Construction of operand.
            /// </summary>
            /// <param name="fmt">The format.</param>
            /// <param name="n">The name of operand.</param>
            /// <param name="s">The size of array.</param>
            internal Operand(PinFormat fmt, int n, uint s)
            {
                format = fmt;
                name = n;
                size = s;
            }

            /// <summary>
            /// Construction of operand.
            /// </summary>
            /// <param name="fmt">The format.</param>
            /// <param name="n">The name of operand.</param>
            internal Operand(PinFormat fmt, int n, bool w)
                : this(fmt, n)
            {
                writable = w;
            }

            /// <summary>
            /// Construction of operand.
            /// </summary>
            /// <param name="fmt">The format.</param>
            /// <param name="n">The name of operand.</param>
            /// <param name="s">The size of array.</param>
            internal Operand(PinFormat fmt, int n, uint s, bool w)
                : this(fmt, n, s)
            {
                writable = w;
            }

            /// <summary>
            /// Constant operand.
            /// </summary>
            /// <param name="fmt">The format.</param>
            /// <param name="n">The name.</param>
            /// <param name="v">Value of operand.</param>
            internal Operand(PinFormat fmt, int n, object v)
                : this(fmt, n)
            {
                value = v;
            }

            /// <summary>
            /// Constant operand.
            /// </summary>
            /// <param name="fmt">The format.</param>
            /// <param name="n">The name.</param>
            /// <param name="v">Value of operand.</param>
            /// <param name="s">The size of array.</param>
            internal Operand(PinFormat fmt, int n, uint s, object v)
                : this(fmt, n, s)
            {
                value = v;
            }

            /// <summary>
            /// Creates a texture operand.
            /// </summary>
            /// <param name="fmt"></param>
            /// <param name="n"></param>
            /// <param name="textureFmt"></param>
            internal Operand(PinFormat fmt, int n, PinFormat textureFmt)
                : this(fmt, n)
            {
                this.textureFormat = textureFmt;
            }

            #endregion

            #region Property

            /// <summary>
            /// The name of operand.
            /// </summary>
            public int Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            /// Format of operand.
            /// </summary>
            public PinFormat Format
            {
                get
                {
                    return format;
                }
            }

            /// <summary>
            /// Texture format, if format is Texture.
            /// </summary>
            public PinFormat TextureFormat
            {
                get
                {
                    return textureFormat;
                }
            }

            /// <summary>
            /// Returns arrays size.
            /// </summary>
            public uint ArraySize
            {
                get
                {
                    return size;
                }
            }

            /// <summary>
            /// Is the pin an array.
            /// </summary>
            public bool IsArray
            {
                get
                {
                    return size != uint.MaxValue;
                }
            }

            /// <summary>
            /// Are we dealing with fixed operand, known at compile time.
            /// </summary>
            public bool IsFixed
            {
                get
                {
                    return value != null;
                }
            }

            /// <summary>
            /// Only temporaries are writable.
            /// </summary>
            public bool IsWritable
            {
                get
                {
                    return writable;
                }
            }

            /// <summary>
            /// Value, valid only if constant.
            /// </summary>
            public object Value
            {
                get
                {
                    return value;
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Are the operands equal.
            /// </summary>
            /// <param name="other">The other operand.</param>
            /// <returns>Are their formats and sizes the same.</returns>
            public bool Equals(Operand other)
            {
                if (this.Format != other.Format) return false;
                if (this.ArraySize != other.ArraySize) return false;
                return true;
            }

            #endregion
        }


        #endregion

        #region Private Members
        Driver.IShaderCompiler compiler;
        BindingStage stage;
        int counter = 0;
        #endregion

        #region Private Methods

        /// <summary>
        /// Generates next unique name.
        /// </summary>
        /// <returns>The unique name.</returns>
        int GenerateNextUniqueName()
        {
            return counter++;
        }

        void AssertNotDisposed()
        {

        }

        ~ShaderCompiler()
        {
            if (compiler != null)
            {
                compiler.Dispose();
                compiler = null;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Construction of shader.
        /// </summary>
        /// <param name="c">The shader compiler.</param>
        internal ShaderCompiler(Driver.IShaderCompiler c)
        {
            compiler = c;
        }

        #endregion

        #region Constants, Variables

        /// <summary>
        /// Creates an uniform parameter.
        /// </summary>
        /// <param name="fmt">The format of parameter.</param>
        /// <param name="size">The size of array, 0 means no array.</param>
        /// <param name="bufferID">The constant buffer ID.</param>
        /// <param name="placementInBuffer">The placement inside constant buffer.</param>
        /// <returns>The actual input.</returns>
        public Operand CreateConstant(PinFormat fmt, uint size, 
                                uint bufferID, uint placementInBuffer)
        {
            int name = GenerateNextUniqueName();

            // We first "create" variable at global scope.
            compiler.RegisterConstant(name, fmt, size, bufferID, placementInBuffer);

            // Returns the input element that can be used in operations.
            return new Operand(fmt, name, size, false);
        }


        /// <summary>
        /// Creates temporary variable.
        /// </summary>
        /// <param name="fmt">Format of variable.</param>
        /// <returns>Variable that can be assigned.</returns>
        public Operand CreateTemporary(PinFormat fmt, uint size)
        {
            int name = GenerateNextUniqueName();
            compiler.RegisterTemp(name, fmt, size);
            return new Operand(fmt, name, size, true);
        }


        /// <summary>
        /// Creates a fixed value.
        /// </summary>
        /// <param name="fmt">The format of pin.</param>
        /// <param name="val">The pin value, as object.</param>
        /// <returns>Constant operand.</returns>
        public Operand CreateFixed(PinFormat fmt, uint size, object val)
        {
            int name = GenerateNextUniqueName();
            compiler.RegisterFixed(name, fmt, size, val);
            return new Operand(fmt, name, size, val);
        }

        /// <summary>
        /// Creates a texture.
        /// </summary>
        /// <param name="fmt">The texture type.</param>
        /// <param name="textureFmt">Texture data type, may not be matrix.</param>
        /// <param name="register">Texture register id.</param>
        /// <returns></returns>
        public Operand CreateTexture(PinFormat fmt, PinFormat textureFmt, uint register)
        {
            int name = GenerateNextUniqueName();
            compiler.RegisterTexture(name, fmt, textureFmt, register);

            return new Operand(fmt, name, textureFmt);
        }

        /// <summary>
        /// Creates a sampler.
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public Operand CreateSampler(uint register)
        {
            int name = GenerateNextUniqueName();
            compiler.RegisterSampler(name, register);

            return new Operand(PinFormat.Sampler, name);
        }

        /// <summary>
        /// Outputs the specified op as component.
        /// </summary>
        /// <remarks>Must always first output components with lower values, 
        /// to ensure correct input layers.</remarks>
        /// <param name="op">The op.</param>
        /// <param name="component">The component.</param>
        public void Output(Operand op, PinComponent component)
        {
            if (op.IsArray) throw new IncompatibleOperandsException("Output operands must be non-array.");
            compiler.Output(component, op.Format, op.Name);
        }

        /// <summary>
        /// Creates the input operand.
        /// </summary>
        /// <remarks>Must always first create components with lower values,
        /// to ensure correct input layout.</remarks>
        /// <param name="fmt">The format.</param>
        /// <param name="component">The component.</param>
        /// <returns>The operand.</returns>
        public Operand CreateInput(PinFormat fmt, PinComponent component)
        {
            Operand op = new Operand(fmt, GenerateNextUniqueName());
            compiler.RegisterInput(op.Name, fmt, component);
            return op;
        }

        #endregion

        #region Register Operations

        /// <summary>
        /// A simply move operation (e.g. a copy).
        /// </summary>
        /// <param name="inOp">An input operand.</param>
        /// <param name="outOp">Copy destination.</param>
        public void Mov(Operand inOp, Operand outOp)
        {
            if (!inOp.Equals(outOp) || !outOp.IsWritable)
            {
                throw new IncompatibleOperandsException("Mov operation requires both operands " + 
                    "to be the same and destination must be writable.");
            }

            // There is no constant (no op operation) on those (driver may create it a no-op
            // operation but since outOp is not constant, we must move it).
            compiler.Mov(inOp.Name, outOp.Name);
        }

        /// <summary>
        /// Performs a mov if inOp is writable only (otherwise not required).
        /// </summary>
        public Operand Mov(Operand inOp)
        {
            if (inOp.IsWritable)
            {
                Operand tmp = CreateTemporary(inOp.Format, inOp.ArraySize);
                compiler.Mov(inOp.Name, tmp.Name);
                return tmp;
            }

            // Const return.
            return inOp;
        }

        #endregion

        #region BinaryMathOperation Operations

        /// <summary>
        /// Calls intrinsic function.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="operand"></param>
        /// <returns></returns>
        public Operand Call(ShaderFunction function, [NotNull] Operand operand)
        {
            Operand tmp;
            if (function == ShaderFunction.Length)
            {
                tmp = CreateTemporary(PinFormat.Float, operand.ArraySize);
            }
            else
            {
                tmp = CreateTemporary(operand.Format, operand.ArraySize);
            }

            Call(function, operand, tmp);
            return tmp;
        }

        /// <summary>
        /// Calls intrinsic function.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public void Call(ShaderFunction function, [NotNull] Operand src, [NotNull] Operand dst)
        {
            compiler.Call(function, src.Name, dst.Name);
        }

        /// <summary>
        /// Compares operands.
        /// </summary>
        public void Compare(CompareFunction function, [NotNull] Operand src1, [NotNull] Operand src2, [NotNull] Operand dst)
        {
            if (!src2.Equals(src1) || src1.IsArray)
            {
                throw new ArgumentException("Compare requires operands to be the same and not array.");
            }

            if (dst.Format != PinFormat.Bool || dst.ArraySize != Pin.NotArray)
            {
                throw new ArgumentException("Destination must be boolean.");
            }

            compiler.Compare(function, src1.Name, src2.Name, dst.Name);
        }

        /// <summary>
        /// Compares src1 and src2.
        /// </summary>
        public Operand Compare(CompareFunction function, [NotNull] Operand src1, [NotNull] Operand src2)
        {
            Operand tmp = CreateTemporary(PinFormat.Bool, Pin.NotArray);
            Compare(function, src1, src2, tmp);
            return tmp;
        }

        /// <summary>
        /// Return minimum, per component.
        /// </summary>
        public Operand Min([NotNull] Operand src1, [NotNull] Operand src2)
        {
            Operand tmp = CreateTemporary(src1.Format, src1.ArraySize);
            Min(src1, src2, tmp);
            return tmp;
        }

        /// <summary>
        /// Returns minimum per component.
        /// </summary>
        public void Min([NotNull] Operand src1, [NotNull] Operand src2, [NotNull] Operand dst)
        {
            if (!src1.Equals(src2) || !src1.Equals(dst))
            {
                throw new ArgumentException("Arguments must be the same.");
            }

            // TODO: other checks.

            compiler.Min(src1.Name, src2.Name, dst.Name);
        }

        /// <summary>
        /// Returns maximum per component.
        /// </summary>
        public Operand Max([NotNull] Operand src1, [NotNull] Operand src2)
        {
            Operand tmp = CreateTemporary(src1.Format, src1.ArraySize);
            Max(src1, src2, tmp);
            return tmp;
        }

        /// <summary>
        /// Returns maximum per component.
        /// </summary>
        public void Max([NotNull] Operand src1, [NotNull] Operand src2, [NotNull] Operand dst)
        {
            if (!src1.Equals(src2) || !src1.Equals(dst))
            {
                throw new ArgumentException("Arguments must be the same.");
            }

            // TODO: other checks.

            compiler.Max(src1.Name, src2.Name, dst.Name);
        }

        /// <summary>
        /// Adds two operands together.
        /// </summary>
        /// <returns>Result of operation.</returns>
        public void Add(Operand in1, Operand in2, Operand outOp)
        {
            if (in1.IsArray || !in1.Equals(in2) || !in1.Equals(outOp) || !outOp.IsWritable)
            {
                throw new IncompatibleOperandsException("Addition operation requires both formats to be the same.");
            }

            // We check if we can precache it, this is copy only op.
            if (in1.IsFixed && in2.IsFixed)
            {
                Operand tmp = CreateFixed(in1.Format, in1.ArraySize, Math.MathHelper.Add(in1.Value, in2.Value));

                // We must create a mem copy.
                compiler.Mov(tmp.Name, outOp.Name);

                return;
                
            }
 
            compiler.Add(in1.Name, in2.Name, outOp.Name);
        }

        /// <summary>
        /// Adds two pins together, resulting in a genned output.
        /// </summary>
        public Operand Add(Operand in1, Operand in2)
        {
            if (in1.IsArray || !in1.Equals(in2))
            {
                throw new IncompatibleOperandsException("Addition operation requires both formats to be the same.");
            }

            // We check if we can precache it, this is copy only op.
            if (in1.IsFixed && in2.IsFixed)
            {
                return CreateFixed(in1.Format, in1.ArraySize, Math.MathHelper.Add(in1.Value, in2.Value));
            }

            // Else create temp and return.
            Operand tmp = CreateTemporary(in1.Format, in1.ArraySize);
            compiler.Add(in1.Name, in2.Name, tmp.Name);
            return tmp;
        }

        /// <summary>
        ///Substracts two operands together.
        /// </summary>
        /// <returns>Result of operation.</returns>
        public void Sub(Operand in1, Operand in2, Operand outOp)
        {
            if (in1.IsArray || !in1.Equals(in2) || !in1.Equals(outOp) || !outOp.IsWritable)
            {
                throw new IncompatibleOperandsException("Addition operation requires both formats to be the same.");
            }

            // We check if we can precache it, this is copy only op.
            if (in1.IsFixed && in2.IsFixed)
            {
                Operand tmp = CreateFixed(in1.Format, in1.ArraySize, Math.MathHelper.Sub(in1.Value, in2.Value));

                // We must create a mem copy.
                compiler.Mov(tmp.Name, outOp.Name);

                return;

            }

            compiler.Sub(in1.Name, in2.Name, outOp.Name);
        }

        /// <summary>
        /// Substracts two pins together, resulting in a genned output.
        /// </summary>
        public Operand Sub(Operand in1, Operand in2)
        {
            if (in1.IsArray || !in1.Equals(in2))
            {
                throw new IncompatibleOperandsException("Addition operation requires both formats to be the same.");
            }

            // We check if we can precache it, this is copy only op.
            if (in1.IsFixed && in2.IsFixed)
            {
                return CreateFixed(in1.Format, in1.ArraySize, Math.MathHelper.Sub(in1.Value, in2.Value));
            }

            // Else create temp and return.
            Operand tmp = CreateTemporary(in1.Format, in1.ArraySize);
            compiler.Sub(in1.Name, in2.Name, tmp.Name);
            return tmp;
        }

        /// <summary>
        /// Divides two operands together.
        /// </summary>
        /// <returns>Result of operation.</returns>
        public void Div(Operand in1, Operand in2, Operand outOp)
        {
            if (in1.IsArray || !in1.Equals(in2) || !in1.Equals(outOp) || !outOp.IsWritable)
            {
                throw new IncompatibleOperandsException("Addition operation requires both formats to be the same.");
            }

            // We check if we can precache it, this is copy only op.
            if (in1.IsFixed && in2.IsFixed)
            {
                Operand tmp = CreateFixed(in1.Format, in1.ArraySize, Math.MathHelper.Div(in1.Value, in2.Value));

                // We must create a mem copy.
                compiler.Mov(tmp.Name, outOp.Name);

                return;

            }

            compiler.Div(in1.Name, in2.Name, outOp.Name);
        }

        /// <summary>
        /// Divides two pins, resulting in a genned output.
        /// </summary>
        public Operand Div(Operand in1, Operand in2)
        {
            if (in1.IsArray || !in1.Equals(in2))
            {
                throw new IncompatibleOperandsException("Addition operation requires both formats to be the same.");
            }

            // We check if we can precache it, this is copy only op.
            if (in1.IsFixed && in2.IsFixed)
            {
                return CreateFixed(in1.Format,  in1.ArraySize, Math.MathHelper.Div(in1.Value, in2.Value));
            }

            // Else create temp and return.
            Operand tmp = CreateTemporary(in1.Format, in1.ArraySize);
            compiler.Div(in1.Name, in2.Name, tmp.Name);
            return tmp;
        }

        /// <summary>
        /// Multiplies two operands.
        /// </summary>
        /// <returns>Result of operation.</returns>
        public void Mul(Operand in1, Operand in2, Operand outOp)
        {
            if (in1.IsArray || !in1.Equals(in2) || !in1.Equals(outOp) || !outOp.IsWritable)
            {
                throw new IncompatibleOperandsException("Addition operation requires both formats to be the same.");
            }

            // We check if we can precache it, this is copy only op.
            if (in1.IsFixed && in2.IsFixed)
            {
                Operand tmp = CreateFixed(in1.Format, in1.ArraySize, Math.MathHelper.Mul(in1.Value, in2.Value));

                // We must create a mem copy.
                compiler.Mov(tmp.Name, outOp.Name);

                return;
            }

            PinFormat dummy;

            // Special matrix-* or *-matrix multiplication.
            if (PinFormatHelper.IsMatrix(in1.Format, out dummy) ||
               PinFormatHelper.IsMatrix(in2.Format, out dummy))
            {
                compiler.MulEx(in1.Name, in2.Name, outOp.Name);
            }
            else
            {
                compiler.Mul(in1.Name, in2.Name, outOp.Name);
            }
        }

        /// <summary>
        /// Multiplies two operands.
        /// </summary>
        public Operand Mul(Operand in1, Operand in2)
        {
            // TODO: validation.

            PinFormat outFmt = in1.Format;

            // Same format.
            if (in1.Format == in2.Format)
            {
                outFmt = in1.Format;
            }
            // Matrix x vector.
            else if (in1.Format == PinFormat.Float4x4 && in2.Format == PinFormat.Floatx4)
            {
                outFmt = PinFormat.Floatx4;
            }
            // By scalar.
            else if (in1.Format == PinFormat.Float)
            {
                outFmt = in2.Format;
            }
            else if (in2.Format == PinFormat.Float)
            {
                outFmt = in1.Format;
            }
            else
            {
                throw new NotSupportedException();
            }

            // We check if we can precache it, this is copy only op.
            if (in1.IsFixed && in2.IsFixed)
            {
                return CreateFixed(outFmt, in1.ArraySize, Math.MathHelper.Mul(in1.Value, in2.Value));
            }

            

            // Else create temp and return.
            Operand tmp = CreateTemporary(outFmt, in1.ArraySize);
            PinFormat dummy;

            // Special matrix-* or *-matrix multiplication.
            if (PinFormatHelper.IsMatrix(in1.Format, out dummy) ||
               PinFormatHelper.IsMatrix(in2.Format, out dummy))
            {
                compiler.MulEx(in1.Name, in2.Name, tmp.Name);
            }
            else
            {
                compiler.Mul(in1.Name, in2.Name, tmp.Name);
            }
            return tmp;
        }


        /// <summary>
        /// Computes dot product of two operands.
        /// </summary>
        /// <returns>Result of operation.</returns>
        public void Dot(Operand in1, Operand in2, Operand outOp)
        {
            
            if (in1.IsArray || !in1.Equals(in2) || !outOp.IsWritable)
            {
                throw new IncompatibleOperandsException("Addition operation requires both formats to be the same.");
            }

            PinFormat fmt;
            if (!PinFormatHelper.IsVector(in1.Format, out fmt) || outOp.Format != fmt)
            {
                throw new IncompatibleOperandsException("Only vector types can be used in dot product, scalars must match vector types.");
            }

            // We check if we can precache it, this is copy only op.
            if (in1.IsFixed && in2.IsFixed)
            {
                Operand tmp = CreateFixed(in1.Format, in1.ArraySize, Math.MathHelper.Dot(in1.Value, in2.Value));

                // We must create a mem copy.
                compiler.Mov(tmp.Name, outOp.Name);

                return;

            }

            compiler.Dot(in1.Name, in2.Name, outOp.Name);
        }

        /// <summary>
        /// Computes dot product of two operands.
        /// </summary>
        public Operand Dot(Operand in1, Operand in2)
        {
            if (in1.IsArray || !in1.Equals(in2))
            {
                throw new IncompatibleOperandsException("Dot operation requires both formats to be the same.");
            }

            PinFormat fmt;
            if(!PinFormatHelper.IsVector(in1.Format, out fmt))
            {
                throw new IncompatibleOperandsException("Dot operation requires both formats to be vectors.");
            }

            // We check if we can precache it, this is copy only op.
            if (in1.IsFixed && in2.IsFixed)
            {
                return CreateFixed(fmt, in1.ArraySize, Math.MathHelper.Dot(in1.Value, in2.Value));
            }

            // Else create temp and return.
            Operand tmp = CreateTemporary(fmt, in1.ArraySize);
            compiler.Mul(in1.Name, in2.Name, tmp.Name);
            return tmp;
        }

        #endregion

        #region Swizzle Operations


        public Operand Swizzle([NotNull] Operand source, SwizzleMask mask)
        {
            Operand tmp = CreateTemporary(mask.SwizzleOutFormat, source.ArraySize);
            Swizzle(source, tmp, mask);
            return tmp;
        }

        public void Swizzle([NotNull] Operand source, [NotNull] Operand dest, SwizzleMask mask)
        {
            if (dest.Format != mask.SwizzleOutFormat)
            {
                throw new ArgumentException("Destination does not match mask format.");
            }

            compiler.Swizzle(source.Name, mask, dest.Name);
        }

        /// <summary>
        /// Expands data.
        /// </summary>
        public Operand Expand([NotNull] Operand source, PinFormat outFormat, ExpandType type)
        {
            // TODO: constant expression

            Operand dest = CreateTemporary(outFormat, source.ArraySize);
            compiler.Expand(source.Name, dest.Name, source.Format, dest.Format, type);
            return dest;
        }

        /// <summary>
        /// Expands data.
        /// </summary>
        public void Expand([NotNull] Operand source, [NotNull] Operand dest, ExpandType type)
        {
            compiler.Expand(source.Name, dest.Name, source.Format, dest.Format, type);
        }

        #endregion

        #region Boolean Operations

        #endregion

        #region Array Operations

        /// <summary>
        /// We compute index in array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Operand IndexInArray([NotNull] Operand array, [NotNull] Operand index)
        {
            // We optimize if array is fixed.
            if (array.IsFixed)
            {
                // TODO: for now ignore.
            }

            // We can perform some validation if array size is known.
            if (index.IsFixed && array.ArraySize != Pin.DynamicArray)
            {
                // We allow UInt and Int indices.
                if (index.Format == PinFormat.Integer)
                {
                    if ((int)index.Value < 0 || (int)index.Value >= array.ArraySize)
                    {
                        throw new IndexOutOfRangeException("Access out of range is detected at compile time.");
                    }
                }
                else if (index.Format == PinFormat.Integerx2)
                {
                    if ((uint)index.Value >= array.ArraySize)
                    {
                        throw new IndexOutOfRangeException("Access out of range is detected at compile time.");
                    }
                }
            }

            // We emit operation.
            Operand ret = CreateTemporary(array.Format, Pin.NotArray);
            compiler.IndexInArray(array.Name, index.Name, ret.Name);
            return ret;
        }

        #endregion

        #region Flow Operations

        public void BeginSwitch(Operand operand)
        {
            compiler.BeginSwitch(operand.Name);
        }

        public void BeginCase(Operand operand)
        {
            if (!operand.IsFixed) throw new ArgumentException("The case label must be fixed.");
            compiler.BeginCase(operand.Name);
        }

        public void BeginDefault()
        {
            compiler.BeginDefault();
        }

        public void EndCase()
        {
            compiler.EndCase();
        }

        public void EndSwitch()
        {
            compiler.EndSwitch();
        }

        public void BeginWhile()
        {
            compiler.BeginWhile();
        }

        public void EndWhile()
        {
            compiler.EndWhile();
        }

        public void Break([NotNull] Operand op)
        {
            compiler.Break(op.Name);
        }

        public void BeginIf(Operand operand)
        {
            compiler.BeginIf(operand.Name);
        }

        public void Else()
        {
            compiler.Else();
        }

        public void EndIf()
        {
            compiler.EndIf();
        }

        #endregion

        #region Texture Operations

        /// <summary>
        /// Loads a value from texture.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Operand Load([NotNull] Operand texture, [NotNull] Operand position, Operand offset)
        {
            Operand tmp = CreateTemporary(texture.TextureFormat, Pin.NotArray);
            Load(texture, position, offset, tmp);
            return tmp;
        }

        /// <summary>
        /// Loads a value from texture.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="dst"></param>
        public void Load([NotNull] Operand texture, [NotNull] Operand position, Operand offset, [NotNull] Operand dst)
        {
            // We first validate.
            switch (texture.Format)
            {
                case PinFormat.Texture1D:
                    if (position.Format != PinFormat.Integerx2)
                    {
                        throw new ArgumentException("Position must be two-dimensional.");
                    }
                    break;
                case PinFormat.Texture1DArray:
                    if (position.Format != PinFormat.Integerx3)
                    {
                        throw new ArgumentException("Position must be three-dimensional.");
                    }
                    break;
                case PinFormat.Texture2D:
                    if (position.Format != PinFormat.Integerx3)
                    {
                        throw new ArgumentException("Position must be three-dimensional.");
                    }
                    break;
                case PinFormat.Texture2DArray:
                    if (position.Format != PinFormat.Integerx4)
                    {
                        throw new ArgumentException("Position must be four-dimensional.");
                    }
                    break;
                case PinFormat.Texture3D:
                    if (position.Format != PinFormat.Integerx4)
                    {
                        throw new ArgumentException("Position must be four-dimensional.");
                    }
                    break;
                case PinFormat.BufferTexture:
                    if (position.Format != PinFormat.Integer)
                    {
                        throw new ArgumentException("Position must be one dimensional.");
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            // We also validate offset.
            if (offset != null)
            {
                // TODO:
            }

            compiler.Load(texture.Name, position.Name,
                offset != null ? offset.Name : -1, dst.Name);
        }

        /// <summary>
        /// Samples at specified address of texture using sampler.
        /// </summary>
        /// <param name="sampler"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Operand Sample([NotNull] Operand sampler, [NotNull] Operand texture, Operand offset, [NotNull] Operand position)
        {
            Operand tmp = CreateTemporary(texture.TextureFormat, Pin.NotArray);
            Sample(texture, sampler, position, offset, tmp);
            return tmp;
        }

        /// <summary>
        /// Samples at specified address of texture using sampler.
        /// </summary>
        /// <param name="sampler"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="sample"></param>
        public void Sample([NotNull] Operand sampler, [NotNull] Operand texture, [NotNull] Operand position, Operand offset, [NotNull]Operand sample)
        {
            // We first validate.
            switch (texture.Format)
            {
                case PinFormat.Texture1D:
                    if (position.Format != PinFormat.Float)
                    {
                        throw new ArgumentException("Position must be one-dimensional.");
                    }
                    break;
                case PinFormat.Texture1DArray:
                    if (position.Format != PinFormat.Floatx2)
                    {
                        throw new ArgumentException("Position must be two-dimensional.");
                    }
                    break;
                case PinFormat.Texture2D:
                    if (position.Format != PinFormat.Floatx2)
                    {
                        throw new ArgumentException("Position must be two-dimensional.");
                    }
                    break;
                case PinFormat.Texture2DArray:
                    if (position.Format != PinFormat.Floatx3)
                    {
                        throw new ArgumentException("Position must be three-dimensional.");
                    }
                    break;
                case PinFormat.TextureCube:
                    if (position.Format != PinFormat.Floatx3)
                    {
                        throw new ArgumentException("Position must be three-dimensional.");
                    }
                    break;
                case PinFormat.Texture3D:
                    if (position.Format != PinFormat.Floatx3)
                    {
                        throw new ArgumentException("Position must be three-dimensional.");
                    }
                    break;
                case PinFormat.BufferTexture:
                    if (position.Format != PinFormat.Integer)
                    {
                        throw new ArgumentException("Position must be one dimensional.");
                    }
                    break;
                default:
                    break;
            }

            compiler.Sample(sampler.Name, texture.Name, position.Name, 
                offset != null ? offset.Name : -1, sample.Name);
        }

        #endregion

        #region Conversion Operations

        /// <summary>
        /// Converts input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outFormat"></param>
        /// <returns></returns>
        public Operand Convert([NotNull] Operand input, PinFormat outFormat)
        {
            Operand tmp = CreateTemporary(outFormat, Pin.NotArray);
            Convert(input, tmp);
            return tmp;
        }

        /// <summary>
        /// Converts input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public void Convert([NotNull] Operand input, [NotNull] Operand output)
        {
            compiler.Convert(input.Name, output.Format, output.Name);
        }

        #endregion

        #region Compilation

        /// <summary>
        /// Begins description.
        /// </summary>
        /// <param name="s">The stage.</param>
        public void Begin(BindingStage s)
        {
            counter = 0;
            stage = s;
            compiler.Begin(stage);
        }

        /// <summary>
        /// Compiles the actual code, we attach shaders 
        /// </summary>
        public IShader End(FixedShaderParameters parameters)
        {
            Driver.IShaderBase b = compiler.End();

            // We now terminate it.
            switch (stage)
            {
                case BindingStage.VertexShader:
                    return new VShader(parameters, (Driver.IVShader)b);
                case BindingStage.PixelShader:
                    return new PShader(parameters, (Driver.IPShader)b);
                case BindingStage.GeometryShader:
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Compiles the shader from file
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="filename"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Obsolete("This should be used for shader debugging only.")]
        public IShader CompileFromFile(BindingStage stage, String filename, FixedShaderParameters parameters)
        {
            Driver.IShaderBase b = compiler.Compile(stage, filename);

            // We now terminate it.
            switch (stage)
            {
                case BindingStage.VertexShader:
                    return new VShader(parameters, (Driver.IVShader)b);
                case BindingStage.PixelShader:
                    return new PShader(parameters, (Driver.IPShader)b);
                case BindingStage.GeometryShader:
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (compiler != null)
            {
                compiler.Dispose();
                compiler = null;
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }

}
