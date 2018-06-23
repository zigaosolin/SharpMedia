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
using SharpMedia.Math.Matrix;

namespace SharpMedia.Graphics.Shaders.Metadata
{

    /// <summary>
    /// A ShaderCode generator is an entry element for all metadata programming. It
    /// can create inputs, you can bind pins as outputs.
    /// </summary>
    /// <remarks>Flow control is a bit limited.</remarks>
    public class CodeGenerator
    {
        #region Private Members
        ShaderCode dag;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a code generator.
        /// </summary>
        public CodeGenerator(BindingStage stage)
        {
            dag = new ShaderCode(stage);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Accesses a ShaderCode generated using metadata programming.
        /// </summary>
        public ShaderCode ShaderCode
        {
            get
            {
                return dag;
            }
        }

        #endregion

        #region Fixed Construction

        /// <summary>
        /// Creates fixed float.
        /// </summary>
        public Floatx1 Fixed(float f)
        {
            ConstantOperation c = dag.FromScalar(f);
            return new Floatx1(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed float.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Floatx2 Fixed(Vector2f v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new Floatx2(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed float.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Floatx3 Fixed(Vector3f v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new Floatx3(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed float.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Floatx4 Fixed(Vector4f v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new Floatx4(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates fixed int.
        /// </summary>
        public Integerx1 Fixed(int f)
        {
            ConstantOperation c = dag.FromScalar(f);
            return new Integerx1(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates fixed int.
        /// </summary>
        public UIntegerx1 Fixed(uint f)
        {
            ConstantOperation c = dag.FromScalar(f);
            return new UIntegerx1(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed int2.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Integerx2 Fixed(Vector2i v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new Integerx2(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed int3.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Integerx3 Fixed(Vector3i v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new Integerx3(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed float.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Integerx4 Fixed(Vector4i v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new Integerx4(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates matrix fixed.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Float4x4 Fixed(Matrix4x4f m)
        {
            ConstantOperation c = dag.CreateFixed(m);
            return new Float4x4(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates matrix fixed.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Float3x3 Fixed(Matrix3x3f m)
        {
            ConstantOperation c = dag.CreateFixed(m);
            return new Float3x3(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates matrix fixed.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Float2x2 Fixed(Matrix2x2f m)
        {
            ConstantOperation c = dag.CreateFixed(m);
            return new Float2x2(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed boolean.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Boolx1 Fixed(bool v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new Boolx1(c.Outputs[0], this);
        }

        #region Arrays

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Floatx1> Fixed(float[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Floatx1>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Floatx2> Fixed(Vector2f[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Floatx2>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Floatx3> Fixed(Vector3f[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Floatx3>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Floatx4> Fixed(Vector4f[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Floatx4>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Float2x2> Fixed(Matrix2x2f[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Float2x2>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Float3x3> Fixed(Matrix3x3f[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Float3x3>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Float4x4> Fixed(Matrix4x4f[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Float4x4>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Integerx1> Fixed(int[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Integerx1>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Integerx2> Fixed(Vector2i[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Integerx2>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Integerx3> Fixed(Vector3i[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Integerx3>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<Integerx4> Fixed(Vector4i[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<Integerx4>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a fixed array.
        /// </summary>
        public PinArray<UIntegerx1> Fixed(uint[] v)
        {
            ConstantOperation c = dag.CreateFixed(v);
            return new PinArray<UIntegerx1>(c.Outputs[0], this);
        }

        #endregion

        /// <summary>
        /// Creates a constant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <returns></returns>
        public Floatx1 ConstantFloatx1([NotEmpty] string name)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Float, Pin.NotArray, dag);
            return new Floatx1(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a contant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <returns></returns>
        public Floatx2 CreateFloatx2([NotEmpty] string name)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Floatx2, Pin.NotArray, dag);
            return new Floatx2(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a constant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <returns></returns>
        public Floatx2 CreateFloatx3([NotEmpty] string name)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Floatx3, Pin.NotArray, dag);
            return new Floatx2(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a constant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <returns></returns>
        public Floatx2 CreateFloatx4([NotEmpty] string name)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Floatx4, Pin.NotArray, dag);
            return new Floatx2(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a constant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <returns></returns>
        public Float2x2 CreateFloat2x2([NotEmpty] string name)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Float2x2, Pin.NotArray, dag);
            return new Float2x2(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a constant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <returns></returns>
        public Float3x3 CreateFloat3x3([NotEmpty] string name)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Float3x3, Pin.NotArray, dag);
            return new Float3x3(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a constant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <returns></returns>
        public Float4x4 CreateFloat4x4([NotEmpty] string name)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Float4x4, Pin.NotArray, dag);
            return new Float4x4(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates true-false.
        /// </summary>
        public Boolx1 CreateBoolx1([NotEmpty] string name)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Bool, Pin.NotArray, dag);
            return new Boolx1(c.Outputs[0], this);
        }

        #region Arrays

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Floatx1> CreateFloatx1Array([NotEmpty] string name)
        {
            return CreateFloatx1Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Floatx1> CreateFloatx1Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Float, size, dag);
            return new PinArray<Floatx1>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Floatx2> CreateFloatx2Array([NotEmpty] string name)
        {
            return CreateFloatx2Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Floatx2> CreateFloatx2Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Floatx2, size, dag);
            return new PinArray<Floatx2>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Floatx3> CreateFloatx3Array([NotEmpty] string name)
        {
            return CreateFloatx3Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Floatx3> CreateFloatx3Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Floatx3, size, dag);
            return new PinArray<Floatx3>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Floatx4> CreateFloatx4Array([NotEmpty] string name)
        {
            return CreateFloatx4Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Floatx4> CreateFloatx4Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Floatx4, size, dag);
            return new PinArray<Floatx4>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Integerx1> CreateIntegerx1Array([NotEmpty] string name)
        {
            return CreateIntegerx1Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Integerx1> CreateIntegerx1Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Integer, size, dag);
            return new PinArray<Integerx1>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Integerx2> CreateIntegerx2Array([NotEmpty] string name)
        {
            return CreateIntegerx2Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Integerx2> CreateIntegerx2Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Integerx2, size, dag);
            return new PinArray<Integerx2>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Integerx3> CreateIntegerx3Array([NotEmpty] string name)
        {
            return CreateIntegerx3Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Integerx3> CreateIntegerx3Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Integerx3, size, dag);
            return new PinArray<Integerx3>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Integerx4> CreateIntegerx4Array([NotEmpty] string name)
        {
            return CreateIntegerx4Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Integerx4> CreateIntegerx4Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Integerx4, size, dag);
            return new PinArray<Integerx4>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<UIntegerx1> CreateUIntegerx1Array([NotEmpty] string name)
        {
            return CreateUIntegerx1Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<UIntegerx1> CreateUIntegerx1Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.UInteger, size, dag);
            return new PinArray<UIntegerx1>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Float4x4> CreateFloat4x4Array([NotEmpty] string name)
        {
            return CreateFloat4x4Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Float4x4> CreateFloat4x4Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Float4x4, size, dag);
            return new PinArray<Float4x4>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Float3x3> CreateFloat3x3Array([NotEmpty] string name)
        {
            return CreateFloat3x3Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Float3x3> CreateFloat3x3Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Float3x3, size, dag);
            return new PinArray<Float3x3>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic array.
        /// </summary>
        public PinArray<Float2x2> CreateFloat2x2Array([NotEmpty] string name)
        {
            return CreateFloat2x2Array(name, Pin.DynamicArray);
        }

        /// <summary>
        /// Creates a static array.
        /// </summary>
        public PinArray<Float2x2> CreateFloat2x2Array([NotEmpty] string name, uint size)
        {
            ConstantOperation c = new ConstantOperation(name, PinFormat.Float2x2, size, dag);
            return new PinArray<Float2x2>(c.Outputs[0], this);
        }



        #endregion

        #endregion

        #region Create Input

        /// <summary>
        /// Input floatx1.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Floatx1 InputFloatx1(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Float);
            return new Floatx1(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input floatx2.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Floatx2 InputFloatx2(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Floatx2);
            return new Floatx2(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input floatx3.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Floatx3 InputFloatx3(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Floatx3);
            return new Floatx3(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input floatx4.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Floatx4 InputFloatx4(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Floatx4);
            return new Floatx4(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input uint.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public UIntegerx1 InputUInteger1(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.UInteger);
            return new UIntegerx1(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input int1.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Integerx1 InputInteger1(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Integer);
            return new Integerx1(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input int2.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Integerx2 InputInteger2(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Integerx2);
            return new Integerx2(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input int3.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Integerx3 InputInteger3(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Integerx3);
            return new Integerx3(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input int4.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Integerx4 InputInteger4(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Integerx4);
            return new Integerx4(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input float4x4.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Float4x4 InputFloat4x4(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Float4x4);
            return new Float4x4(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input float3x3.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Float3x3 InputFloat3x3(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Float3x3);
            return new Float3x3(dag.InputOperation.PinAsOutput(component), this);
        }

        /// <summary>
        /// Input float2x2.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public Float2x2 InputFloat2x2(PinComponent component)
        {
            dag.InputOperation.AddInput(component, PinFormat.Float2x2);
            return new Float2x2(dag.InputOperation.PinAsOutput(component), this);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Creates binder from pin.
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        internal PinBinder CreateFrom(Pin pin)
        {
            switch (pin.Format)
            {
                case PinFormat.Float:
                    return new Floatx1(pin, this);
                case PinFormat.Floatx2:
                    return new Floatx2(pin, this);
                case PinFormat.Floatx3:
                    return new Floatx3(pin, this);
                case PinFormat.Floatx4:
                    return new Floatx4(pin, this);
                case PinFormat.Float2x2:
                    return new Float2x2(pin, this);
                case PinFormat.Float3x3:
                    return new Float3x3(pin, this);
                case PinFormat.Float4x4:
                    return new Float4x4(pin, this);
                case PinFormat.Integer:
                    return new Integerx1(pin, this);
                case PinFormat.UInteger:
                    return new UIntegerx1(pin, this);
                case PinFormat.Integerx2:
                    return new Integerx1(pin, this);
                case PinFormat.Integerx3:
                    return new Integerx1(pin, this);
                case PinFormat.Integerx4:
                    return new Integerx1(pin, this);
                default:
                    throw new NotSupportedException();
            }
        }

        internal PinFormat FromType(Type t)
        {
            if (t == typeof(Floatx1)) return PinFormat.Float;
            if (t == typeof(Floatx2)) return PinFormat.Floatx2;
            if (t == typeof(Floatx3)) return PinFormat.Floatx3;
            if (t == typeof(Floatx4)) return PinFormat.Floatx4;
            if (t == typeof(Float2x2)) return PinFormat.Float2x2;
            if (t == typeof(Float3x3)) return PinFormat.Float3x3;
            if (t == typeof(Float4x4)) return PinFormat.Float4x4;
            if (t == typeof(Integerx1)) return PinFormat.Integer;
            if (t == typeof(Integerx2)) return PinFormat.Integerx2;
            if (t == typeof(Integerx3)) return PinFormat.Integerx3;
            if (t == typeof(Integerx4)) return PinFormat.Integerx4;
            if (t == typeof(UIntegerx1)) return PinFormat.UInteger;
            return PinFormat.Undefined;
        }

        internal static Pin Compound(out CodeGenerator generator, params object[] objs)
        {
            generator = null;
            Pin[] pins = new Pin[objs.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                object obj = objs[i];
                if (obj is PinBinder)
                {
                    pins[i] = ((PinBinder)obj).Pin;
                    CodeGenerator g = ((PinBinder)obj).Generator;
                    if (generator != null)
                    {
                        if (generator != g)
                        {
                            throw new ArgumentException("Mixing generators.");
                        }
                    }
                    else
                    {
                        generator = g;
                    }
                }
                else
                {
                    pins[i] = generator.dag.CreateFixed(obj).Outputs[0];
                }
            }

            // Create compounds operation and return.
            CompoundOperation op = new CompoundOperation();
            op.BindInputs(pins);

            return op.Outputs[0];
        }

        #endregion

        #region Flow Control

        /// <summary>
        /// We use a branch instruction.
        /// </summary>
        public T Branch<T>([NotNull] Boolx1 value, [NotNull] T isTrue, [NotNull] T isFalse) where T : PinBinder
        {
            if (isTrue.Generator != isFalse.Generator || 
                isTrue.Generator != value.Generator || isTrue.Generator != this)
            {
                throw new ArgumentException("All values must have the same generator.");
            }

            BranchOperation op = new BranchOperation();
            op.BindInputs(value.Pin, isTrue.Pin, isFalse.Pin);

            return (T)CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Begins loop.
        /// </summary>
        public Loop<T> BeginLoop<T>(UIntegerx1 count, T value)
            where T : PinBinder
        {
            return new Loop<T>(count, value);
        }

        /// <summary>
        /// Begins loop.
        /// </summary>
        public Loop<T, T2> BeginLoop<T, T2>(UIntegerx1 count, T value, T2 value2)
            where T : PinBinder
            where T2 : PinBinder
        {
            return new Loop<T, T2>(count, value, value2);
        }

        /// <summary>
        /// Begins loop.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public Loop BeginLoop(UIntegerx1 count, params PinBinder[] values)
        {
            return new Loop(count, values);
        }

        /// <summary>
        /// Ends any loops.
        /// </summary>
        /// <param name="loop"></param>
        public void EndLoop(Loop loop)
        {
            loop.EndLoop();
        }


        #endregion

        #region Resources

        /// <summary>
        /// Executes external operation.
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public PinBinder[] Execute<T>(params PinBinder[] inputs) where T : IOperation
        {
            T operation = Activator.CreateInstance<T>();
            Pin[] inp = new Pin[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                inp[i] = inputs[i].Pin;
            }
            operation.BindInputs(inp);

            Pin[] outputs = operation.Outputs;
            PinBinder[] binders = new PinBinder[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                binders[i] = CreateFrom(outputs[i]);
            }

            // We return executed binders.
            return binders;
        }

        /// <summary>
        /// Creates an interface binder.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public InterfaceBinder CreateInterface([NotEmpty] string name)
        {
            ConstantOperation c = dag.CreateConstant(name, PinFormat.Interface, Pin.NotArray, null);
            return new InterfaceBinder(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a dynamic interface array binder.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PinArray<InterfaceBinder> CreateInterfaceArray([NotEmpty] string name)
        {
            ConstantOperation c = dag.CreateConstant(name, PinFormat.Interface, Pin.DynamicArray, null);
            return new PinArray<InterfaceBinder>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a texture 1D.
        /// </summary>
        public Texture1DBinder<T> CreateTexture1D<T>([NotEmpty] string name) where T : PinBinder
        {
            ConstantOperation c = dag.CreateConstant(name, PinFormat.Texture1D, this.FromType(typeof(T)));
            return new Texture1DBinder<T>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a texture 1D.
        /// </summary>
        public BufferTextureBinder<T> CreateBufferTexture<T>([NotEmpty] string name) where T : PinBinder
        {
            ConstantOperation c = dag.CreateConstant(name, PinFormat.BufferTexture, this.FromType(typeof(T)));
            return new BufferTextureBinder<T>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a texture 2D.
        /// </summary>
        public Texture2DBinder<T> CreateTexture2D<T>([NotEmpty] string name) where T : PinBinder
        {
            ConstantOperation c = dag.CreateConstant(name, PinFormat.Texture2D, this.FromType(typeof(T)));
            return new Texture2DBinder<T>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a texture 2D array.
        /// </summary>
        public Texture2DArrayBinder<T> CreateTexture2DArray<T>([NotEmpty] string name) where T : PinBinder
        {
            ConstantOperation c = dag.CreateConstant(name, PinFormat.Texture2DArray, this.FromType(typeof(T)));
            return new Texture2DArrayBinder<T>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a texture 1D array.
        /// </summary>
        public Texture1DArrayBinder<T> CreateTexture1DArray<T>([NotEmpty] string name) where T : PinBinder
        {
            ConstantOperation c = dag.CreateConstant(name, PinFormat.Texture1DArray, this.FromType(typeof(T)));
            return new Texture1DArrayBinder<T>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a texture cube.
        /// </summary>
        public TextureCubeBinder<T> CreateTextureCube<T>([NotEmpty] string name) where T : PinBinder
        {
            ConstantOperation c = dag.CreateConstant(name, PinFormat.TextureCube, this.FromType(typeof(T)));
            return new TextureCubeBinder<T>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a texture 3D.
        /// </summary>
        public Texture3DBinder<T> CreateTexture3D<T>([NotEmpty] string name) where T : PinBinder
        {
            ConstantOperation c = dag.CreateConstant(name, PinFormat.Texture3D, this.FromType(typeof(T)));
            return new Texture3DBinder<T>(c.Outputs[0], this);
        }

        /// <summary>
        /// Creates a sampler binder.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SamplerBinder CreateSampler([NotEmpty] string name)
        {
            ConstantOperation c = dag.CreateConstant(name, PinFormat.Sampler, null);
            return new SamplerBinder(c.Outputs[0], this);
        }

        #endregion

        #region Additional Operators

        /// <summary>
        /// Expands data to other.
        /// </summary>
        public T Expand<T>(PinBinder binder, ExpandType expandType) where T : PinBinder
        {
            PinFormat expandTo = FromType(typeof(T));
            if (expandTo == PinFormat.Undefined)
            {
                throw new InvalidOperationException("Invalid format to expand to.");
            }
            
            // May also throw.
            ExpandOperation op = new ExpandOperation(expandTo, expandType);
            op.BindInputs(binder.Pin);
            return (T)CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Compounds data to create new operand.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binders"></param>
        /// <returns></returns>
        public T Compound<T>([NotEmptyArray] params PinBinder[] binders)
            where T : PinBinder
        {
            Pin[] inputs = new Pin[binders.Length];
            for (int i = 0; i < binders.Length; i++)
            {
                if (binders[i] == null || 
                    binders[i].Generator != this) throw new ArgumentException("Mixing generators.");
                inputs[i] = binders[i].Pin;
            }

            CompoundOperation op = new CompoundOperation();
            op.BindInputs(inputs);

            // We create output, it may not match (cast exception thrown).
            return (T)CreateFrom(op.Outputs[0]);
        }

        #endregion

        #region Output

        /// <summary>
        /// Outputs data.
        /// </summary>
        public void Output(PinComponent component, PinBinder binder)
        {
            dag.OutputOperation.AddComponentAndLink(component, binder.Pin);
        }

        #endregion
    }
}
