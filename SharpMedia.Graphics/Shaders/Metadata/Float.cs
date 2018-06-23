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
using SharpMedia.Graphics.Shaders.Operations;
using SharpMedia.AspectOriented;
using SharpMedia.Math;
using SharpMedia.Graphics.States;
using SharpMedia.Math.Matrix;

namespace SharpMedia.Graphics.Shaders.Metadata
{

    /// <summary>
    /// A single "float".
    /// </summary>
    public sealed class Floatx1 : PinBinder
    {
        #region Constructors

        /// <summary>
        /// Constructs float.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Floatx1([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        #endregion

        #region Overloads

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Floatx1 operator +([NotNull] Floatx1 f1, [NotNull] Floatx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Floatx1 operator +([NotNull] Floatx1 f1, float fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Floatx1 operator +(float fixedVal, [NotNull] Floatx1 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx1 operator *([NotNull] Floatx1 f1, [NotNull] Floatx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx1 operator *([NotNull] Floatx1 f1, float fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx1 operator *(float fixedVal, [NotNull] Floatx1 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Floatx1 operator /([NotNull] Floatx1 f1, [NotNull] Floatx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Floatx1 operator /([NotNull] Floatx1 f1, float fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Dics pin and fixed.
        /// </summary>
        public static Floatx1 operator /(float fixedVal, [NotNull] Floatx1 f1)
        {
            return f1 / fixedVal;
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Floatx1 operator -([NotNull] Floatx1 f1, [NotNull] Floatx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Floatx1 operator -([NotNull] Floatx1 f1, float fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Floatx1 operator -(float fixedVal, [NotNull] Floatx1 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Multiply By Scalar

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float2x2 operator *([NotNull] Floatx1 f1, Matrix2x2f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float2x2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float2x2 operator *(Matrix2x2f fixedVal, [NotNull] Floatx1 f1)
        {
            return f1 * fixedVal;
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float3x3 operator *([NotNull] Floatx1 f1, Matrix3x3f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float3x3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float3x3 operator *(Matrix3x3f fixedVal, [NotNull] Floatx1 f1)
        {
            return f1 * fixedVal;
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float4x4 operator *([NotNull] Floatx1 f1, Matrix4x4f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Float4x4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Float4x4 operator *(Matrix4x4f fixedVal, [NotNull] Floatx1 f1)
        {
            return f1 * fixedVal;
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx2 operator *([NotNull] Floatx1 f1, Vector2f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx2 operator *(Vector2f fixedVal, [NotNull] Floatx1 f1)
        {
            return f1 * fixedVal;
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx3 operator *([NotNull] Floatx1 f1, Vector3f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx3 operator *(Vector3f fixedVal, [NotNull] Floatx1 f1)
        {
            return f1 * fixedVal;
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx4 operator *([NotNull] Floatx1 f1, Vector4f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx4 operator *(Vector4f fixedVal, [NotNull] Floatx1 f1)
        {
            return f1 * fixedVal;
        }    

        #endregion

        #endregion

        #region Comparison

        #region Private Members

        static Boolx1 Compare(CompareFunction func, Floatx1 f1, Floatx1 f2)
        {
            if (f1.Generator != f2.Generator)
            {
                throw new ArgumentException("Mixing values of two different generators.");
            }

            CompareOperation op = new CompareOperation(func);
            op.BindInputs(f1.Pin, f2.Pin);
            return new Boolx1(op.Outputs[0], f1.Generator);
        }

        static Boolx1 Compare(CompareFunction func, Floatx1 f1, float f)
        {
            return Compare(func, f1, f1.Generator.Fixed(f));
        }

        static Boolx1 Compare(CompareFunction func, float f1, Floatx1 f2)
        {
            return Compare(func, f2.Generator.Fixed(f1), f2);
        }

        #endregion

        #region Less

        public static Boolx1 operator <([NotNull] Floatx1 f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <([NotNull] Floatx1 f1, float f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <(float f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        #endregion

        #region LEqual

        public static Boolx1 operator <=([NotNull] Floatx1 f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=([NotNull] Floatx1 f1, float f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=(float f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        #endregion

        #region Equal

        public static Boolx1 operator ==([NotNull] Floatx1 f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==([NotNull] Floatx1 f1, float f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==(float f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        #endregion

        #region NEqual

        public static Boolx1 operator !=([NotNull] Floatx1 f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=([NotNull] Floatx1 f1, float f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=(float f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        #endregion

        #region GEqual

        public static Boolx1 operator >=([NotNull] Floatx1 f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=([NotNull] Floatx1 f1, float f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=(float f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        #endregion

        #region Greater

        public static Boolx1 operator >([NotNull] Floatx1 f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >([NotNull] Floatx1 f1, float f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >(float f1, [NotNull] Floatx1 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
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
    /// A "float2" vector.
    /// </summary>
    public sealed partial class Floatx2 : PinBinder
    {

        #region Constructors

        /// <summary>
        /// Constructs float.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Floatx2([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx2(Floatx1 x, Floatx1 y)
        {
            pin = CodeGenerator.Compound(out generator, x, y);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx2(Floatx1 x, float y)
        {
            pin = CodeGenerator.Compound(out generator, x, y);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx2(float x, Floatx1 y)
        {
            pin = CodeGenerator.Compound(out generator, x, y);
        }


        #endregion

        #region Overloads

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Floatx2 operator +([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Floatx2 operator +([NotNull] Floatx2 f1, Vector2f fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Floatx2 operator +(Vector2f fixedVal, [NotNull] Floatx2 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public Floatx2 Mul([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Floatx2 Mul([NotNull] Floatx2 f1, Vector2f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Floatx2 Mul(Vector2f fixedVal, [NotNull] Floatx2 f1)
        {
            return Mul(f1, fixedVal);
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Floatx2 operator /([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Floatx2 operator /([NotNull] Floatx2 f1, float fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Floatx2 operator /([NotNull] Floatx2 f1, Vector2f fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Floatx2 operator -([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Floatx2 operator -([NotNull] Floatx2 f1, Vector2f fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Floatx2 operator -(Vector2f fixedVal, [NotNull] Floatx2 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx1 operator *([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx1 operator *([NotNull] Floatx2 f1, Vector2f fixedVal)
        {
            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx1 operator *(Vector2f fixedVal, [NotNull] Floatx2 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Mul

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx2 operator *([NotNull] Floatx2 f1, [NotNull] Floatx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx2 operator *([NotNull] Floatx1 f1, [NotNull] Floatx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx2 operator *([NotNull] Floatx2 f1, float fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx2 operator *(float fixedVal, [NotNull] Floatx2 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Matrix

        /// <summary>
        /// A matrix multiply.
        /// </summary>
        public static Floatx2 operator *(Matrix2x2f matrix, [NotNull] Floatx2 f)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f.Generator.Fixed(matrix).Pin, f.Pin);
            return new Floatx2(op.Outputs[0], f.Generator);
        }

        /// <summary>
        /// A matrix multiply.
        /// </summary>
        public static Floatx2 operator *([NotNull] Floatx2 f, Matrix2x2f matrix)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f.Pin, f.Generator.Fixed(matrix).Pin);
            return new Floatx2(op.Outputs[0], f.Generator);
        }

        #endregion

        #endregion

        #region Comparison

        #region Private Members

        static Boolx1 Compare(CompareFunction func, Floatx2 f1, Floatx2 f2)
        {
            if (f1.Generator != f2.Generator)
            {
                throw new ArgumentException("Mixing values of two different generators.");
            }

            CompareOperation op = new CompareOperation(func);
            op.BindInputs(f1.Pin, f2.Pin);
            return new Boolx1(op.Outputs[0], f1.Generator);
        }

        static Boolx1 Compare(CompareFunction func, Floatx2 f1, Vector2f f)
        {
            return Compare(func, f1, f1.Generator.Fixed(f));
        }

        static Boolx1 Compare(CompareFunction func, Vector2f f1, Floatx2 f2)
        {
            return Compare(func, f2.Generator.Fixed(f1), f2);
        }

        #endregion

        #region Less

        public static Boolx1 operator <([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <([NotNull] Floatx2 f1, Vector2f f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <(Vector2f f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        #endregion

        #region LEqual

        public static Boolx1 operator <=([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=([NotNull] Floatx2 f1, Vector2f f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=(Vector2f f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        #endregion

        #region Equal

        public static Boolx1 operator ==([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==([NotNull] Floatx2 f1, Vector2f f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==(Vector2f f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        #endregion

        #region NEqual

        public static Boolx1 operator !=([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=([NotNull] Floatx2 f1, Vector2f f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=(Vector2f f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        #endregion

        #region GEqual

        public static Boolx1 operator >=([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=([NotNull] Floatx2 f1, Vector2f f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=(Vector2f f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        #endregion

        #region Greater

        public static Boolx1 operator >([NotNull] Floatx2 f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >([NotNull] Floatx2 f1, Vector2f f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >(Vector2f f1, [NotNull] Floatx2 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
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
    /// A "float3" vector.
    /// </summary>
    public sealed partial class Floatx3 : PinBinder
    {

        #region Constructors

        /// <summary>
        /// Constructs float.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Floatx3([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx3(Floatx1 x, Floatx1 y, Floatx1 z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx3(Floatx1 x, float y, Floatx1 z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx3(float x, Floatx1 y, Floatx1 z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx3(Floatx1 x, Floatx1 y, float z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx3(Floatx1 x, float y, float z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx3(float x, Floatx1 y, float z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx3(float x, float y, Floatx1 z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx3(Floatx2 xy, float z)
        {
            pin = CodeGenerator.Compound(out generator, xy, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx3(Vector2f xy, Floatx1 z)
        {
            pin = CodeGenerator.Compound(out generator, xy, z);
        }

        #endregion

        #region Overloads

        #region Matrix

        /// <summary>
        /// A matrix multiply.
        /// </summary>
        public static Floatx3 operator *(Matrix3x3f matrix, [NotNull] Floatx3 f)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f.Generator.Fixed(matrix).Pin, f.Pin);
            return new Floatx3(op.Outputs[0], f.Generator);
        }

        /// <summary>
        /// A matrix multiply.
        /// </summary>
        public static Floatx3 operator *([NotNull] Floatx3 f, Matrix3x3f matrix)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f.Pin, f.Generator.Fixed(matrix).Pin);
            return new Floatx3(op.Outputs[0], f.Generator);
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Floatx3 operator +([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Floatx3 operator +([NotNull] Floatx3 f1, Vector3f fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Floatx3 operator +(Vector3f fixedVal, [NotNull] Floatx3 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public Floatx3 Mul([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Floatx3 Mul([NotNull] Floatx3 f1, Vector3f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Floatx3 Mul(Vector3f fixedVal, [NotNull] Floatx3 f1)
        {
            return Mul(f1, fixedVal);
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Floatx3 operator /([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Floatx3 operator /([NotNull] Floatx3 f1, float fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Floatx3 operator /([NotNull] Floatx3 f1, Vector3f fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Floatx3 operator -([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Floatx3 operator -([NotNull] Floatx3 f1, Vector3f fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Floatx3 operator -(Vector3f fixedVal, [NotNull] Floatx3 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx1 operator *([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx1 operator *([NotNull] Floatx3 f1, Vector3f fixedVal)
        {
            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx1 operator *(Vector2f fixedVal, [NotNull] Floatx3 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Mul

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx3 operator *([NotNull] Floatx3 f1, [NotNull] Floatx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx3 operator *([NotNull] Floatx1 f1, [NotNull] Floatx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx3 operator *([NotNull] Floatx3 f1, float fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx3 operator *(float fixedVal, [NotNull] Floatx3 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #endregion

        #region Comparison

        #region Private Members

        static Boolx1 Compare(CompareFunction func, Floatx3 f1, Floatx3 f2)
        {
            if (f1.Generator != f2.Generator)
            {
                throw new ArgumentException("Mixing values of two different generators.");
            }

            CompareOperation op = new CompareOperation(func);
            op.BindInputs(f1.Pin, f2.Pin);
            return new Boolx1(op.Outputs[0], f1.Generator);
        }

        static Boolx1 Compare(CompareFunction func, Floatx3 f1, Vector3f f)
        {
            return Compare(func, f1, f1.Generator.Fixed(f));
        }

        static Boolx1 Compare(CompareFunction func, Vector3f f1, Floatx3 f2)
        {
            return Compare(func, f2.Generator.Fixed(f1), f2);
        }

        #endregion

        #region Less

        public static Boolx1 operator <([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <([NotNull] Floatx3 f1, Vector3f f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <(Vector3f f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        #endregion

        #region LEqual

        public static Boolx1 operator <=([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=([NotNull] Floatx3 f1, Vector3f f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=(Vector3f f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        #endregion

        #region Equal

        public static Boolx1 operator ==([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==([NotNull] Floatx3 f1, Vector3f f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==(Vector3f f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        #endregion

        #region NEqual

        public static Boolx1 operator !=([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=([NotNull] Floatx3 f1, Vector3f f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=(Vector3f f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        #endregion

        #region GEqual

        public static Boolx1 operator >=([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=([NotNull] Floatx3 f1, Vector3f f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=(Vector3f f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        #endregion

        #region Greater

        public static Boolx1 operator >([NotNull] Floatx3 f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >([NotNull] Floatx3 f1, Vector3f f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >(Vector3f f1, [NotNull] Floatx3 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
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
    /// A "float4" vector.
    /// </summary>
    public sealed partial class Floatx4 : PinBinder
    {

        #region Constructors

        /// <summary>
        /// Constructs float.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Floatx4([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx1 x, Floatx1 y, Floatx1 z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx1 x, float y, Floatx1 z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(float x, Floatx1 y, Floatx1 z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx1 x, Floatx1 y, float z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx1 x, float y, float z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(float x, Floatx1 y, float z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(float x, float y, Floatx1 z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx2 xy, float z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, xy, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Vector2f xy, Floatx1 z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, xy, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Vector3f xyz, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, xyz, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx3 xyz, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, xyz, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx1 x, Floatx1 y, Floatx1 z, float w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx1 x, float y, Floatx1 z, float w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(float x, Floatx1 y, Floatx1 z, float w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx1 x, Floatx1 y, float z, float w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx1 x, float y, float z, float w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(float x, Floatx1 y, float z, float w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(float x, float y, Floatx1 z, float w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx2 xy, float z, float w)
        {
            pin = CodeGenerator.Compound(out generator, xy, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Vector2f xy, Floatx1 z, float w)
        {
            pin = CodeGenerator.Compound(out generator, xy, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(float x, float y, float z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Floatx4(Floatx3 xyz, float w)
        {
            pin = CodeGenerator.Compound(out generator, xyz, w);
        }


        #endregion

        #region Overloads

        #region Matrix

        /// <summary>
        /// A matrix multiply.
        /// </summary>
        public static Floatx4 operator *(Matrix4x4f matrix, [NotNull] Floatx4 f)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f.Generator.Fixed(matrix).Pin, f.Pin);
            return new Floatx4(op.Outputs[0], f.Generator);
        }

        /// <summary>
        /// A matrix multiply.
        /// </summary>
        public static Floatx4 operator *([NotNull] Floatx4 f, Matrix4x4f matrix)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f.Pin, f.Generator.Fixed(matrix).Pin);
            return new Floatx4(op.Outputs[0], f.Generator);
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Floatx4 operator +([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Floatx4 operator +([NotNull] Floatx4 f1, Vector4f fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Floatx4 operator +(Vector4f fixedVal, [NotNull] Floatx4 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public Floatx4 Mul([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Floatx4 Mul([NotNull] Floatx4 f1, Vector4f fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Floatx4 Mul(Vector4f fixedVal, [NotNull] Floatx4 f1)
        {
            return Mul(f1, fixedVal);
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Floatx4 operator /([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Floatx4 operator /([NotNull] Floatx4 f1, float fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Floatx4 operator /([NotNull] Floatx4 f1, Vector4f fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Floatx4 operator -([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Floatx4 operator -([NotNull] Floatx4 f1, Vector4f fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Floatx4 operator -(Vector4f fixedVal, [NotNull] Floatx4 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx1 operator *([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx1 operator *([NotNull] Floatx4 f1, Vector4f fixedVal)
        {
            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx1 operator *(Vector4f fixedVal, [NotNull] Floatx4 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Mul

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx4 operator *([NotNull] Floatx4 f1, [NotNull] Floatx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Floatx4 operator *([NotNull] Floatx1 f1, [NotNull] Floatx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx4 operator *([NotNull] Floatx4 f1, float fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Floatx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Floatx4 operator *(float fixedVal, [NotNull] Floatx4 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #endregion

        #region Comparison

        #region Private Members

        static Boolx1 Compare(CompareFunction func, Floatx4 f1, Floatx4 f2)
        {
            if (f1.Generator != f2.Generator)
            {
                throw new ArgumentException("Mixing values of two different generators.");
            }

            CompareOperation op = new CompareOperation(func);
            op.BindInputs(f1.Pin, f2.Pin);
            return new Boolx1(op.Outputs[0], f1.Generator);
        }

        static Boolx1 Compare(CompareFunction func, Floatx4 f1, Vector4f f)
        {
            return Compare(func, f1, f1.Generator.Fixed(f));
        }

        static Boolx1 Compare(CompareFunction func, Vector4f f1, Floatx4 f2)
        {
            return Compare(func, f2.Generator.Fixed(f1), f2);
        }

        #endregion

        #region Less

        public static Boolx1 operator <([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <([NotNull] Floatx4 f1, Vector4f f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <(Vector4f f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        #endregion

        #region LEqual

        public static Boolx1 operator <=([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=([NotNull] Floatx4 f1, Vector4f f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=(Vector4f f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        #endregion

        #region Equal

        public static Boolx1 operator ==([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==([NotNull] Floatx4 f1, Vector4f f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==(Vector4f f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        #endregion

        #region NEqual

        public static Boolx1 operator !=([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=([NotNull] Floatx4 f1, Vector4f f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=(Vector4f f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        #endregion

        #region GEqual

        public static Boolx1 operator >=([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=([NotNull] Floatx4 f1, Vector4f f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=(Vector4f f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        #endregion

        #region Greater

        public static Boolx1 operator >([NotNull] Floatx4 f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >([NotNull] Floatx4 f1, Vector4f f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >(Vector4f f1, [NotNull] Floatx4 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
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
