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
using SharpMedia.Graphics.States;
using SharpMedia.Graphics.Shaders.Operations;

namespace SharpMedia.Graphics.Shaders.Metadata
{
    /// <summary>
    /// A single "uuint".
    /// </summary>
    public sealed class UIntegerx1 : PinBinder
    {
        #region Constructors

        /// <summary>
        /// Constructs uint.
        /// </summary>
        /// <param name="p">The pin.</param>
        /// <param name="g">OutputDescriptor generator.</param>
        internal UIntegerx1([NotNull] Pin p, [NotNull] CodeGenerator g)
            : base(g, p)
        {
        }

        #endregion

        #region Overloads

        #region Add

        /// <summary>
        /// Adds pins.
        /// </summary>
        public static UIntegerx1 operator +([NotNull] UIntegerx1 f1, [NotNull] UIntegerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new UIntegerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static UIntegerx1 operator +([NotNull] UIntegerx1 f1, uint fixedVal)
        {
            AddOperation op = new AddOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new UIntegerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Adds pin and fixed.
        /// </summary>
        public static UIntegerx1 operator +(uint fixedVal, [NotNull] UIntegerx1 f1)
        {
            return f1 + fixedVal;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Muls pins.
        /// </summary>
        public static UIntegerx1 operator *([NotNull] UIntegerx1 f1, [NotNull] UIntegerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new UIntegerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static UIntegerx1 operator *([NotNull] UIntegerx1 f1, uint fixedVal)
        {
            MultiplyOperation op = new MultiplyOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new UIntegerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Muls pin and fixed.
        /// </summary>
        public static UIntegerx1 operator *(uint fixedVal, [NotNull] UIntegerx1 f1)
        {
            return f1 * fixedVal;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divs pins.
        /// </summary>
        public static UIntegerx1 operator /([NotNull] UIntegerx1 f1, [NotNull] UIntegerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new UIntegerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Divs pin and fixed.
        /// </summary>
        public static UIntegerx1 operator /([NotNull] UIntegerx1 f1, uint fixedVal)
        {
            DivideOperation op = new DivideOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new UIntegerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Dics pin and fixed.
        /// </summary>
        public static UIntegerx1 operator /(uint fixedVal, [NotNull] UIntegerx1 f1)
        {
            return f1 / fixedVal;
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subs pins.
        /// </summary>
        public static UIntegerx1 operator -([NotNull] UIntegerx1 f1, [NotNull] UIntegerx1 f2)
        {
            if (f1.Generator != f2.Generator) throw new ArgumentException("Cannot connect elements from different sources.");

            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f2.Pin);
            return new UIntegerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static UIntegerx1 operator -([NotNull] UIntegerx1 f1, uint fixedVal)
        {
            SubstractOperation op = new SubstractOperation();
            op.BindInputs(f1.Pin, f1.Generator.Fixed(fixedVal).Pin);
            return new UIntegerx1(op.Outputs[0], f1.Generator);
        }

        /// <summary>
        /// Subs pin and fixed.
        /// </summary>
        public static UIntegerx1 operator -(uint fixedVal, [NotNull] UIntegerx1 f1)
        {
            return f1 - fixedVal;
        }

        #endregion


        #endregion

        #region Comparison

        #region Private Members

        static Boolx1 Compare(CompareFunction func, UIntegerx1 f1, UIntegerx1 f2)
        {
            if (f1.Generator != f2.Generator)
            {
                throw new ArgumentException("Mixing values of two different generators.");
            }

            CompareOperation op = new CompareOperation(func);
            op.BindInputs(f1.Pin, f2.Pin);
            return new Boolx1(op.Outputs[0], f1.Generator);
        }

        static Boolx1 Compare(CompareFunction func, UIntegerx1 f1, uint f)
        {
            return Compare(func, f1, f1.Generator.Fixed(f));
        }

        static Boolx1 Compare(CompareFunction func, uint f1, UIntegerx1 f2)
        {
            return Compare(func, f2.Generator.Fixed(f1), f2);
        }

        #endregion

        #region Less

        public static Boolx1 operator <([NotNull] UIntegerx1 f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <([NotNull] UIntegerx1 f1, uint f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        public static Boolx1 operator <(uint f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.Less, f1, f2);
        }

        #endregion

        #region LEqual

        public static Boolx1 operator <=([NotNull] UIntegerx1 f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=([NotNull] UIntegerx1 f1, uint f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        public static Boolx1 operator <=(uint f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.LessEqual, f1, f2);
        }

        #endregion

        #region Equal

        public static Boolx1 operator ==([NotNull] UIntegerx1 f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==([NotNull] UIntegerx1 f1, uint f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        public static Boolx1 operator ==(uint f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.Equal, f1, f2);
        }

        #endregion

        #region NEqual

        public static Boolx1 operator !=([NotNull] UIntegerx1 f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=([NotNull] UIntegerx1 f1, uint f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        public static Boolx1 operator !=(uint f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.NotEqual, f1, f2);
        }

        #endregion

        #region GEqual

        public static Boolx1 operator >=([NotNull] UIntegerx1 f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=([NotNull] UIntegerx1 f1, uint f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        public static Boolx1 operator >=(uint f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.GreaterEqual, f1, f2);
        }

        #endregion

        #region Greater

        public static Boolx1 operator >([NotNull] UIntegerx1 f1, [NotNull] UIntegerx1 f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >([NotNull] UIntegerx1 f1, uint f2)
        {
            return Compare(CompareFunction.Greater, f1, f2);
        }

        public static Boolx1 operator >(uint f1, [NotNull] UIntegerx1 f2)
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
