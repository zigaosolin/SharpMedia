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
using SharpMedia.Math;
using SharpMedia.Graphics.States;

namespace SharpMedia.Graphics.Shaders.Metadata
{

    /// <summary>
    /// A single "int".
    /// </summary>
    public sealed class Integerx1 : PinBinder
    {
        #region Constructors

        /// <summary>
        /// Constructs int.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Integerx1([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }


        #endregion

        #region Overloads

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Integerx1 operator +([NotNull] Integerx1 f1, [NotNull] Integerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Integerx1 operator +([NotNull] Integerx1 f1, int fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Integerx1 operator +(int fixedVal, [NotNull] Integerx1 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Integerx1 operator *([NotNull] Integerx1 f1, [NotNull] Integerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx1 operator *([NotNull] Integerx1 f1, int fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx1 operator *(int fixedVal, [NotNull] Integerx1 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Integerx1 operator /([NotNull] Integerx1 f1, [NotNull] Integerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Integerx1 operator /([NotNull] Integerx1 f1, int fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Dics pin and fixed.
        /// </summary>
        public static Integerx1 operator /(int fixedVal, [NotNull] Integerx1 f1)
        {
            return f1 / fixedVal;
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Integerx1 operator -([NotNull] Integerx1 f1, [NotNull] Integerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Integerx1 operator -([NotNull] Integerx1 f1, int fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Integerx1 operator -(int fixedVal, [NotNull] Integerx1 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Multiply By Scalar

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx2 operator *([NotNull] Integerx1 f1, Vector2i fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx2 operator *(Vector2i fixedVal, [NotNull] Integerx1 f1)
        {
            return f1 * fixedVal;
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx3 operator *([NotNull] Integerx1 f1, Vector3i fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx3 operator *(Vector3i fixedVal, [NotNull] Integerx1 f1)
        {
            return f1 * fixedVal;
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx4 operator *([NotNull] Integerx1 f1, Vector4i fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx4 operator *(Vector4i fixedVal, [NotNull] Integerx1 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #endregion

        #region Comparison

        #region Private Members

        static Boolx1 Compare(CompareFunction func, Integerx1 f1, Integerx1 f2)
        {
            if (f1.Generator != f2.Generator)
            {
                throw new ArgumentException("Mixing values of two different generators.");
            }

            CompareOperation op = new CompareOperation(func);
            op.BindInputs(f1.Pin, f2.Pin);
            return new Boolx1(op.Outputs[0], f1.Generator);
        }

        static Boolx1 Compare(CompareFunction func, Integerx1 f1, int f)
        {
            return Compare(func, f1, f1.Generator.Fixed(f));
        }

        static Boolx1 Compare(CompareFunction func, int f1, Integerx1 f2)
        {
            return Compare(func, f2.Generator.Fixed(f1), f2);
        }

        #endregion

        #region Less

        public static Boolx1 operator <([NotNull] Integerx1 f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <([NotNull] Integerx1 f1, int f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <(int f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        #endregion

        #region LEqual

        public static Boolx1 operator <=([NotNull] Integerx1 f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=([NotNull] Integerx1 f1, int f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=(int f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        #endregion

        #region Equal

        public static Boolx1 operator ==([NotNull] Integerx1 f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==([NotNull] Integerx1 f1, int f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==(int f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        #endregion

        #region NEqual

        public static Boolx1 operator !=([NotNull] Integerx1 f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=([NotNull] Integerx1 f1, int f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=(int f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        #endregion

        #region GEqual

        public static Boolx1 operator >=([NotNull] Integerx1 f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=([NotNull] Integerx1 f1, int f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=(int f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        #endregion

        #region Greater

        public static Boolx1 operator >([NotNull] Integerx1 f1, [NotNull] Integerx1 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >([NotNull] Integerx1 f1, int f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >(int f1, [NotNull] Integerx1 f2)
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
    /// A "int2" vector.
    /// </summary>
    public sealed partial class Integerx2 : PinBinder
    {

        #region Constructors

        /// <summary>
        /// Constructs int.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Integerx2([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        
        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx2(Floatx1 x, Floatx1 y)
        {
            pin = CodeGenerator.Compound(out generator, x, y);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx2(Floatx1 x, int y)
        {
            pin = CodeGenerator.Compound(out generator, x, y);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx2(int x, Floatx1 y)
        {
            pin = CodeGenerator.Compound(out generator, x, y);
        }


        #endregion

        #region Overloads

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Integerx2 operator +([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Integerx2 operator +([NotNull] Integerx2 f1, Vector2i fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Integerx2 operator +(Vector2i fixedVal, [NotNull] Integerx2 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public Integerx2 Mul([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Integerx2 Mul([NotNull] Integerx2 f1, Vector2i fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Integerx2 Mul(Vector2i fixedVal, [NotNull] Integerx2 f1)
        {
            return Mul(f1, fixedVal);
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Integerx2 operator /([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Integerx2 operator /([NotNull] Integerx2 f1, int fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Integerx2 operator /([NotNull] Integerx2 f1, Vector2i fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Integerx2 operator -([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Integerx2 operator -([NotNull] Integerx2 f1, Vector2i fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Integerx2 operator -(Vector2i fixedVal, [NotNull] Integerx2 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Integerx1 operator *([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx1 operator *([NotNull] Integerx2 f1, Vector2i fixedVal)
        {
            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx1 operator *(Vector2i fixedVal, [NotNull] Integerx2 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Mul

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Integerx2 operator *([NotNull] Integerx2 f1, [NotNull] Integerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Integerx2 operator *([NotNull] Integerx1 f1, [NotNull] Integerx2 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx2 operator *([NotNull] Integerx2 f1, int fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx2(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx2 operator *(int fixedVal, [NotNull] Integerx2 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #endregion

        #region Comparison

        #region Private Members

        static Boolx1 Compare(CompareFunction func, Integerx2 f1, Integerx2 f2)
        {
            if (f1.Generator != f2.Generator)
            {
                throw new ArgumentException("Mixing values of two different generators.");
            }

            CompareOperation op = new CompareOperation(func);
            op.BindInputs(f1.Pin, f2.Pin);
            return new Boolx1(op.Outputs[0], f1.Generator);
        }

        static Boolx1 Compare(CompareFunction func, Integerx2 f1, Vector2i f)
        {
            return Compare(func, f1, f1.Generator.Fixed(f));
        }

        static Boolx1 Compare(CompareFunction func, Vector2i f1, Integerx2 f2)
        {
            return Compare(func, f2.Generator.Fixed(f1), f2);
        }

        #endregion

        #region Less

        public static Boolx1 operator <([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <([NotNull] Integerx2 f1, Vector2i f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <(Vector2i f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        #endregion

        #region LEqual

        public static Boolx1 operator <=([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=([NotNull] Integerx2 f1, Vector2i f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=(Vector2i f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        #endregion

        #region Equal

        public static Boolx1 operator ==([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==([NotNull] Integerx2 f1, Vector2i f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==(Vector2i f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        #endregion

        #region NEqual

        public static Boolx1 operator !=([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=([NotNull] Integerx2 f1, Vector2i f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=(Vector2i f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        #endregion

        #region GEqual

        public static Boolx1 operator >=([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=([NotNull] Integerx2 f1, Vector2i f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=(Vector2i f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        #endregion

        #region Greater

        public static Boolx1 operator >([NotNull] Integerx2 f1, [NotNull] Integerx2 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >([NotNull] Integerx2 f1, Vector2i f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >(Vector2i f1, [NotNull] Integerx2 f2)
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
    /// A "int3" vector.
    /// </summary>
    public sealed partial class Integerx3 : PinBinder
    {

        #region Constructors

        /// <summary>
        /// Constructs int.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Integerx3([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        
        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx3(Floatx1 x, Floatx1 y, Floatx1 z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx3(Floatx1 x, int y, Floatx1 z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx3(int x, Floatx1 y, Floatx1 z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx3(Floatx1 x, Floatx1 y, int z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx3(Floatx1 x, int y, int z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx3(int x, Floatx1 y, int z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx3(int x, int y, Floatx1 z)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx3(Integerx2 xy, int z)
        {
            pin = CodeGenerator.Compound(out generator, xy, z);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx3(Vector2i xy, Floatx1 z)
        {
            pin = CodeGenerator.Compound(out generator, xy, z);
        }



        #endregion

        #region Overloads

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Integerx3 operator +([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Integerx3 operator +([NotNull] Integerx3 f1, Vector3i fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Integerx3 operator +(Vector3i fixedVal, [NotNull] Integerx3 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public Integerx3 Mul([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Integerx3 Mul([NotNull] Integerx3 f1, Vector3i fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Integerx3 Mul(Vector3i fixedVal, [NotNull] Integerx3 f1)
        {
            return Mul(f1, fixedVal);
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Integerx3 operator /([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Integerx3 operator /([NotNull] Integerx3 f1, int fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Integerx3 operator /([NotNull] Integerx3 f1, Vector3i fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Integerx3 operator -([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Integerx3 operator -([NotNull] Integerx3 f1, Vector3i fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Integerx3 operator -(Vector3i fixedVal, [NotNull] Integerx3 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Integerx1 operator *([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx1 operator *([NotNull] Integerx3 f1, Vector3i fixedVal)
        {
            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx1 operator *(Vector2i fixedVal, [NotNull] Integerx3 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Mul

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Integerx3 operator *([NotNull] Integerx3 f1, [NotNull] Integerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Integerx3 operator *([NotNull] Integerx1 f1, [NotNull] Integerx3 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx3 operator *([NotNull] Integerx3 f1, int fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx3(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx3 operator *(int fixedVal, [NotNull] Integerx3 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #endregion

        #region Comparison

        #region Private Members

        static Boolx1 Compare(CompareFunction func, Integerx3 f1, Integerx3 f2)
        {
            if (f1.Generator != f2.Generator)
            {
                throw new ArgumentException("Mixing values of two different generators.");
            }

            CompareOperation op = new CompareOperation(func);
            op.BindInputs(f1.Pin, f2.Pin);
            return new Boolx1(op.Outputs[0], f1.Generator);
        }

        static Boolx1 Compare(CompareFunction func, Integerx3 f1, Vector3i f)
        {
            return Compare(func, f1, f1.Generator.Fixed(f));
        }

        static Boolx1 Compare(CompareFunction func, Vector3i f1, Integerx3 f2)
        {
            return Compare(func, f2.Generator.Fixed(f1), f2);
        }

        #endregion

        #region Less

        public static Boolx1 operator <([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <([NotNull] Integerx3 f1, Vector3i f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <(Vector3i f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        #endregion

        #region LEqual

        public static Boolx1 operator <=([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=([NotNull] Integerx3 f1, Vector3i f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=(Vector3i f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        #endregion

        #region Equal

        public static Boolx1 operator ==([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==([NotNull] Integerx3 f1, Vector3i f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==(Vector3i f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        #endregion

        #region NEqual

        public static Boolx1 operator !=([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=([NotNull] Integerx3 f1, Vector3i f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=(Vector3i f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        #endregion

        #region GEqual

        public static Boolx1 operator >=([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=([NotNull] Integerx3 f1, Vector3i f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=(Vector3i f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        #endregion

        #region Greater

        public static Boolx1 operator >([NotNull] Integerx3 f1, [NotNull] Integerx3 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >([NotNull] Integerx3 f1, Vector3i f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >(Vector3i f1, [NotNull] Integerx3 f2)
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
    /// A "int4" vector.
    /// </summary>
    public sealed partial class Integerx4 : PinBinder
    {

        #region Constructors

        /// <summary>
        /// Constructs int.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal Integerx4([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        
        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Floatx1 x, Floatx1 y, Floatx1 z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Floatx1 x, int y, Floatx1 z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(int x, Floatx1 y, Floatx1 z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Floatx1 x, Floatx1 y, int z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Floatx1 x, int y, int z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(int x, Floatx1 y, int z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(int x, int y, Floatx1 z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Integerx2 xy, int z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, xy, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Vector2i xy, Floatx1 z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, xy, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Vector3i xyz, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, xyz, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Integerx3 xyz, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, xyz, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Integerx3 xyz, float w)
        {
            pin = CodeGenerator.Compound(out generator, xyz, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Floatx1 x, Floatx1 y, Floatx1 z, int w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Floatx1 x, int y, Floatx1 z, int w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(int x, Floatx1 y, Floatx1 z, int w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Floatx1 x, Floatx1 y, int z, int w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Floatx1 x, int y, int z, int w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(int x, Floatx1 y, int z, int w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(int x, int y, Floatx1 z, int w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Integerx2 xy, int z, int w)
        {
            pin = CodeGenerator.Compound(out generator, xy, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(Vector2i xy, Floatx1 z, int w)
        {
            pin = CodeGenerator.Compound(out generator, xy, z, w);
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public Integerx4(int x, int y, int z, Floatx1 w)
        {
            pin = CodeGenerator.Compound(out generator, x, y, z, w);
        }


        #endregion

        #region Overloads

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static Integerx4 operator +([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Integerx4 operator +([NotNull] Integerx4 f1, Vector4i fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static Integerx4 operator +(Vector4i fixedVal, [NotNull] Integerx4 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public Integerx4 Mul([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Integerx4 Mul([NotNull] Integerx4 f1, Vector4i fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public Integerx4 Mul(Vector4i fixedVal, [NotNull] Integerx4 f1)
        {
            return Mul(f1, fixedVal);
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static Integerx4 operator /([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Integerx4 operator /([NotNull] Integerx4 f1, int fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static Integerx4 operator /([NotNull] Integerx4 f1, Vector4i fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static Integerx4 operator -([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Integerx4 operator -([NotNull] Integerx4 f1, Vector4i fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static Integerx4 operator -(Vector4i fixedVal, [NotNull] Integerx4 f1)
        {
            return f1 - fixedVal;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Integerx1 operator *([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx1 operator *([NotNull] Integerx4 f1, Vector4i fixedVal)
        {
            DotProductOperation op = new DotProductOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx1 operator *(Vector4i fixedVal, [NotNull] Integerx4 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Mul

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Integerx4 operator *([NotNull] Integerx4 f1, [NotNull] Integerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static Integerx4 operator *([NotNull] Integerx1 f1, [NotNull] Integerx4 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx4 operator *([NotNull] Integerx4 f1, int fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new Integerx4(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static Integerx4 operator *(int fixedVal, [NotNull] Integerx4 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #endregion

        #region Comparison

        #region Private Members

        static Boolx1 Compare(CompareFunction func, Integerx4 f1, Integerx4 f2)
        {
            if (f1.Generator != f2.Generator)
            {
                throw new ArgumentException("Mixing values of two different generators.");
            }

            CompareOperation op = new CompareOperation(func);
            op.BindInputs(f1.Pin, f2.Pin);
            return new Boolx1(op.Outputs[0], f1.Generator);
        }

        static Boolx1 Compare(CompareFunction func, Integerx4 f1, Vector4i f)
        {
            return Compare(func, f1, f1.Generator.Fixed(f));
        }

        static Boolx1 Compare(CompareFunction func, Vector4i f1, Integerx4 f2)
        {
            return Compare(func, f2.Generator.Fixed(f1), f2);
        }

        #endregion

        #region Less

        public static Boolx1 operator <([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <([NotNull] Integerx4 f1, Vector4i f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <(Vector4i f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        #endregion

        #region LEqual

        public static Boolx1 operator <=([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=([NotNull] Integerx4 f1, Vector4i f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=(Vector4i f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        #endregion

        #region Equal

        public static Boolx1 operator ==([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==([NotNull] Integerx4 f1, Vector4i f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==(Vector4i f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        #endregion

        #region NEqual

        public static Boolx1 operator !=([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=([NotNull] Integerx4 f1, Vector4i f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=(Vector4i f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        #endregion

        #region GEqual

        public static Boolx1 operator >=([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=([NotNull] Integerx4 f1, Vector4i f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=(Vector4i f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        #endregion

        #region Greater

        public static Boolx1 operator >([NotNull] Integerx4 f1, [NotNull] Integerx4 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >([NotNull] Integerx4 f1, Vector4i f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >(Vector4i f1, [NotNull] Integerx4 f2)
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
