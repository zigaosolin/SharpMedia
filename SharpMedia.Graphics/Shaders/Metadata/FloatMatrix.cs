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
using SharpMedia.AspectOriented;
using SharpMedia.Graphics.Shaders.Operations;
using SharpMedia.Math.Matrix;
using SharpMedia.Math;

namespace SharpMedia.Graphics.Shaders.Metadata
{
    /// <summary>
    /// A float4x4 matrix.
    /// </summary>
    public sealed class Float4x4 : PinBinder
    {
        #region Properties

        /// <summary>
        /// The Row1 swizzle.
        /// </summary>
        public Floatx4 Row1
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Row1AsVec4);
                op.BindInputs(pin);
                return new Floatx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Row1AsVec4);
                op.BindInputs(pin, value.Pin);
                this.pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Row2 swizzle.
        /// </summary>
        public Floatx4 Row2
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Row2AsVec4);
                op.BindInputs(pin);
                return new Floatx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Row2AsVec4);
                op.BindInputs(pin, value.Pin);
                this.pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Row3 swizzle.
        /// </summary>
        public Floatx4 Row3
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Row3AsVec4);
                op.BindInputs(pin);
                return new Floatx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Row3AsVec4);
                op.BindInputs(pin, value.Pin);
                this.pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Row4 swizzle.
        /// </summary>
        public Floatx4 Row4
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Row4AsVec4);
                op.BindInputs(pin);
                return new Floatx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Row4AsVec4);
                op.BindInputs(pin, value.Pin);
                this.pin = op.Outputs[0];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs float.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Float4x4([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Floatx4 row1, Floatx4 row2, Floatx4 row3, Floatx4 row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Vector4f row1, Floatx4 row2, Floatx4 row3, Floatx4 row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Floatx4 row1, Vector4f row2, Floatx4 row3, Floatx4 row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Floatx4 row1, Floatx4 row2, Vector4f row3, Floatx4 row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Floatx4 row1, Floatx4 row2, Floatx4 row3, Vector4f row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Vector4f row1, Vector4f row2, Floatx4 row3, Floatx4 row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Vector4f row1, Floatx4 row2, Vector4f row3, Floatx4 row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Vector4f row1, Floatx4 row2, Floatx4 row3, Vector4f row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Floatx4 row1, Vector4f row2, Vector4f row3, Floatx4 row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Floatx4 row1, Vector4f row2, Floatx4 row3, Vector4f row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Floatx4 row1, Floatx4 row2, Vector4f row3, Vector4f row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Floatx4 row1, Vector4f row2, Vector4f row3, Vector4f row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Vector4f row1, Floatx4 row2, Vector4f row3, Vector4f row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Vector4f row1, Vector4f row2, Floatx4 row3, Vector4f row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float4x4(Vector4f row1, Vector4f row2, Vector4f row3, Floatx4 row4)
        {
            pin = CodeGenerator.Compound(out generator, row1, row2, row3, row4);
        }

        #endregion

        #region Overloads

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Float4x4 operator +([NotNull] Float4x4 f1, [NotNull] Float4x4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Float4x4 operator +([NotNull] Float4x4 f1, Matrix4x4f fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Float4x4 operator +(Matrix4x4f fixedVal, [NotNull] Float4x4 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx4 operator*([NotNull] Float4x4 f1, [NotNull] Floatx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx4 operator*([NotNull] Float4x4 f1, Vector4f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Float4x4 operator /([NotNull] Float4x4 f1, [NotNull] Float4x4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Float4x4 operator /([NotNull] Float4x4 f1, float fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Float4x4 operator /([NotNull] Float4x4 f1, Matrix4x4f fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Float4x4 operator -([NotNull] Float4x4 f1, [NotNull] Float4x4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Float4x4 operator -([NotNull] Float4x4 f1, Matrix4x4f fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Float4x4 operator -(Matrix4x4f fixedVal, [NotNull] Float4x4 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Dot


        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx4 operator *([NotNull] Floatx4 f1, [NotNull] Float4x4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx4 operator *(Vector4f f1, [NotNull] Float4x4 f2)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f2.Generator.Fixed(f1).Pin, f2.Pin);
            return new Floatx4(op.Outputs[0], f2.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Float4x4 operator *([NotNull] Float4x4 f1, [NotNull] Float4x4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float4x4 operator *([NotNull] Float4x4 f1, Matrix4x4f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float4x4 operator *(Matrix4x4f fixedVal, [NotNull] Float4x4 f1)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Generator.Fixed(fixedVal).Pin, f1.Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Mul

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Float4x4 operator *([NotNull] Float4x4 f1, [NotNull] Floatx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Float4x4 operator *([NotNull] Floatx1 f1, [NotNull] Float4x4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float4x4 operator *([NotNull] Float4x4 f1, float fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float4x4 operator *(float fixedVal, [NotNull] Float4x4 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }

    /// <summary>
    /// A float3x3 matrix.
    /// </summary>
    public sealed class Float3x3 : PinBinder
    {
        #region Properties

        /// <summary>
        /// The Row1 swizzle.
        /// </summary>
        public Floatx3 Row1
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Row1AsVec3);
                op.BindInputs(pin);
                return new Floatx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Row1AsVec3);
                op.BindInputs(pin, value.Pin);
                this.pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Row2 swizzle.
        /// </summary>
        public Floatx3 Row2
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Row2AsVec3);
                op.BindInputs(pin);
                return new Floatx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Row2AsVec3);
                op.BindInputs(pin, value.Pin);
                this.pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Row3 swizzle.
        /// </summary>
        public Floatx3 Row3
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Row3AsVec3);
                op.BindInputs(pin);
                return new Floatx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Row3AsVec3);
                op.BindInputs(pin, value.Pin);
                this.pin = op.Outputs[0];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs float.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Float3x3([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float3x3(Floatx3 row1, Floatx3 row2, Floatx3 row3)
        {
            this.pin = CodeGenerator.Compound(out generator, row1, row2, row3);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float3x3(Vector3f row1, Floatx3 row2, Floatx3 row3)
        {
            this.pin = CodeGenerator.Compound(out generator, row1, row2, row3);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float3x3(Floatx3 row1, Vector3f row2, Floatx3 row3)
        {
            this.pin = CodeGenerator.Compound(out generator, row1, row2, row3);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float3x3(Floatx3 row1, Floatx3 row2, Vector3f row3)
        {
            this.pin = CodeGenerator.Compound(out generator, row1, row2, row3);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float3x3(Vector3f row1, Floatx3 row2, Vector3f row3)
        {
            this.pin = CodeGenerator.Compound(out generator, row1, row2, row3);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float3x3(Vector3f row1, Vector3f row2, Floatx3 row3)
        {
            this.pin = CodeGenerator.Compound(out generator, row1, row2, row3);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float3x3(Floatx3 row1, Vector3f row2, Vector3f row3)
        {
            this.pin = CodeGenerator.Compound(out generator, row1, row2, row3);
        }


        #endregion

        #region Overloads

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Float3x3 operator +([NotNull] Float3x3 f1, [NotNull] Float3x3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Float3x3 operator +([NotNull] Float3x3 f1, Matrix3x3f fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Float3x3 operator +(Matrix3x3f fixedVal, [NotNull] Float3x3 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Float3x3 operator*([NotNull] Float3x3 f1, [NotNull] Float3x3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float3x3 operator*([NotNull] Float3x3 f1, Matrix3x3f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Float3x3 operator /([NotNull] Float3x3 f1, [NotNull] Float3x3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Float3x3 operator /([NotNull] Float3x3 f1, float fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Float3x3 operator /([NotNull] Float3x3 f1, Matrix3x3f fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Float3x3 operator -([NotNull] Float3x3 f1, [NotNull] Float3x3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Float3x3 operator -([NotNull] Float3x3 f1, Matrix3x3f fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Float3x3 operator -(Matrix3x3f fixedVal, [NotNull] Float3x3 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Dot

        
        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx3 operator *([NotNull] Floatx3 f1, [NotNull] Float3x3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx3 operator *(Vector3f f1, [NotNull] Float3x3 f2)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f2.Generator.Fixed(f1).Pin, f2.Pin);
            return new Floatx3(op.Outputs[0], f2.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx3 operator *([NotNull] Float3x3 f1, [NotNull] Floatx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx3 operator *([NotNull] Float3x3 f1, Vector3f f2)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(f2).Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Mul

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Float3x3 operator *([NotNull] Float3x3 f1, [NotNull] Floatx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Float3x3 operator *([NotNull] Floatx1 f1, [NotNull] Float3x3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float3x3 operator *([NotNull] Float3x3 f1, float fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float3x3 operator *(float fixedVal, [NotNull] Float3x3 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }

    /// <summary>
    /// A float2x2 matrix.
    /// </summary>
    public sealed class Float2x2 : PinBinder
    {
        #region Properties

        /// <summary>
        /// The Row1 swizzle.
        /// </summary>
        public Floatx3 Row1
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Row1AsVec3);
                op.BindInputs(pin);
                return new Floatx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Row1AsVec2);
                op.BindInputs(pin, value.Pin);
                this.pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Row2 swizzle.
        /// </summary>
        public Floatx3 Row2
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Row2AsVec3);
                op.BindInputs(pin);
                return new Floatx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Row1AsVec2);
                op.BindInputs(pin, value.Pin);
                this.pin = op.Outputs[0];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs float.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Float2x2([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float2x2(Floatx2 row1, Floatx2 row2)
        {
            this.pin = CodeGenerator.Compound(out generator, row1, row2);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float2x2(Vector2f row1, Floatx2 row2)
        {
            this.pin = CodeGenerator.Compound(out generator, row1, row2);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Float2x2(Floatx2 row1, Vector2f row2)
        {
            this.pin = CodeGenerator.Compound(out generator, row1, row2);
        }


        #endregion

        #region Overloads

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Float2x2 operator +([NotNull] Float2x2 f1, [NotNull] Float2x2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Float2x2 operator +([NotNull] Float2x2 f1, Matrix2x2f fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Float2x2 operator +(Matrix2x2f fixedVal, [NotNull] Float2x2 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Float2x2 operator*([NotNull] Float2x2 f1, [NotNull] Float2x2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float2x2 operator*([NotNull] Float2x2 f1, Matrix2x2f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float2x2 operator*(Matrix2x2f fixedVal, [NotNull] Float2x2 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Float2x2 operator /([NotNull] Float2x2 f1, [NotNull] Float2x2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Float2x2 operator /([NotNull] Float2x2 f1, float fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Float2x2 operator /([NotNull] Float2x2 f1, Matrix2x2f fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Float2x2 operator -([NotNull] Float2x2 f1, [NotNull] Float2x2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Float2x2 operator -([NotNull] Float2x2 f1, Matrix2x2f fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Float2x2 operator -(Matrix2x2f fixedVal, [NotNull] Float2x2 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx2 operator *([NotNull] Floatx2 f1, [NotNull] Float2x2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx2 operator *(Vector2f f1, [NotNull] Float2x2 f2)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f2.Generator.Fixed(f1).Pin, f2.Pin);
            return new Floatx2(op.Outputs[0], f2.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx2 operator *([NotNull] Float2x2 f1, [NotNull] Floatx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx2 operator *([NotNull] Float2x2 f1, Vector2f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Mul

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Float2x2 operator *([NotNull] Float2x2 f1, [NotNull] Floatx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Float2x2 operator *([NotNull] Floatx1 f1, [NotNull] Float2x2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float2x2 operator *([NotNull] Float2x2 f1, float fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float2x2 operator *(float fixedVal, [NotNull] Float2x2 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }

   
}
