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
using SharpMedia.Graphics.Shaders;
using SharpMedia.Graphics.States;

namespace SharpMedia.Graphics.Driver
{

    

    /// <summary>
    /// A shader compiler interface; can compile shaders.
    /// </summary>
    [Linkable(LinkMask.Drivers)]
    public interface IShaderCompiler : IDisposable
    {
        /// <summary>
        /// Begins shader compilation.
        /// </summary>
        /// <param name="stage"></param>
        void Begin(BindingStage t);

        /// <summary>
        /// This is a compatibility precompiled shader compilation (driver dependant).
        /// </summary>
        /// <param name="t">The binding type.</param>
        /// <param name="filename">The shader code as file.</param>
        /// <returns>Compiled shader.</returns>
        /// <remarks>Should not be used, only for debugging purpuses.</remarks>
        IShaderBase Compile(BindingStage t, String filename);

        /// <summary>
        /// Ends compilation, resulting in a shader.
        /// </summary>
        IShaderBase End();

        /// <summary>
        /// Registers an input.
        /// </summary>
        /// <param name="n">Name of input.</param>
        /// <param name="fmt">It's format.</param>
        /// <param name="component">The component.</param>
        void RegisterInput(int n, PinFormat fmt, PinComponent component);

        /// <summary>
        /// Registers an uniform at global scope.
        /// </summary>
        /// <param name="n">Name of uniform.</param>
        /// <param name="fmt">The format of pin.</param>
        /// <param name="arraySize">The actual array size, 0 implies not an array.</param>
        /// <param name="buffer">The constant buffer (0-15).</param>
        /// <param name="position">The constant position (4D vectors must be 4-address aligned ...s).</param>
        void RegisterConstant(int n, PinFormat fmt, uint arraySize, uint buffer, uint position);


        /// <summary>
        /// Registers a texture with specific format, internal data format and register id.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="fmt"></param>
        /// <param name="textureFmt"></param>
        /// <param name="register"></param>
        void RegisterTexture(int n, PinFormat fmt, PinFormat textureFmt, uint register);

        /// <summary>
        /// Registers a sampler unsed specific register.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="register"></param>
        void RegisterSampler(int n, uint register);

        /// <summary>
        /// Registers a fixed value.
        /// </summary>
        /// <param name="n">The name.</param>
        /// <param name="fmt">Format of constant.</param>
        /// <param name="arraySize">Array size of constant.</param>
        /// <param name="data">The data of constant.</param>
        void RegisterFixed(int n, PinFormat fmt, uint arraySize, object data);

        /// <summary>
        /// Register a temporary variable.
        /// </summary>
        /// <param name="n">The name of variable.</param>
        /// <param name="fmt">The format of variable.</param>
        /// <param name="arraySize">Array size of variable.</param>
        void RegisterTemp(int n, PinFormat fmt,  uint arraySize);

        /// <summary>
        /// Convert operation.
        /// </summary>
        void Convert(int n, PinFormat outFormat, int result);

        /// <summary>
        /// Adds two pins together.
        /// </summary>
        /// <param name="n1">The first pin.</param>
        /// <param name="n2">The second pin.</param>
        void Add(int n1, int n2, int name);

        /// <summary>
        /// Substracts two pins.
        /// </summary>
        /// <param name="n1">The first pin.</param>
        /// <param name="n2">The second pin.</param>
        void Sub(int n1, int n2, int name);

        /// <summary>
        /// Divides two pins.
        /// </summary>
        /// <param name="n1">The first pin.</param>
        /// <param name="n2">The second pin.</param>
        void Div(int n1, int n2, int name);

        /// <summary>
        /// Multiplies two pins (vector-vector).
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="name"></param>
        void Mul(int n1, int n2, int name);

        /// <summary>
        /// Computes minimum, per component.
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="dst"></param>
        void Min(int n1, int n2, int dst);

        /// <summary>
        /// Computes maximum, per component.
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="dst"></param>
        void Max(int n1, int n2, int dst);

        /// <summary>
        /// Multiplies two pins (matrix-vector, vector-matrix or matrix-matrix).
        /// </summary>
        /// <param name="n1">The first pin.</param>
        /// <param name="n2">The second pin.</param>
        void MulEx(int n1, int n2, int name);

        /// <summary>
        /// Performs a dot product. The operation is defined for vectors only.
        /// </summary>
        /// <param name="n1">The first pin.</param>
        /// <param name="n2">The second pin.</param>
        /// <param name="name">The output pin.</param>
        void Dot(int n1, int n2, int name);

        /// <summary>
        /// A swizzle operation.
        /// </summary>
        /// <param name="n">The name of pin.</param>
        /// <param name="mask">A swizzle mask - 4 bits define one position.</param>
        /// <param name="name"></param>
        void Swizzle(int n, SwizzleMask mask, int name);

        /// <summary>
        /// Moves information from n to o (a copy operation).
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="o"></param>
        void Mov(int n, int o);

        /// <summary>
        /// Expands data.
        /// </summary>
        void Expand(int n1, int n2, PinFormat from, PinFormat to, ExpandType type);

        /// <summary>
        /// Calls function.
        /// </summary>
        void Call(ShaderFunction function, int n1, int n2);

        /// <summary>
        /// Compares two values based on function, returns 1/0.
        /// </summary>
        void Compare(CompareFunction func, int n1, int n2, int r);

        /// <summary>
        /// Performs a sampling.
        /// </summary>
        /// <remarks>Offset can be -1.</remarks>
        /// <param name="sampler"></param>
        /// <param name="texture"></param>
        /// <param name="pos"></param>
        void Sample(int sampler, int texture, int pos, int offset, int result);

        /// <summary>
        /// Loads at specific position.
        /// </summary>
        /// <remarks>Offset can be set to -1.</remarks>
        /// <param name="texture"></param>
        /// <param name="pos"></param>
        void Load(int texture, int pos,  int offset, int result);

        #region Flow Control

        void BeginIf(int n);
        void Else();
        void EndIf();

        void BeginWhile();
        void Break(int n);
        void EndWhile();
        void BeginSwitch(int n);
        void BeginCase(int n);
        void BeginDefault();
        void EndCase();
        void EndSwitch();

        #endregion

        /// <summary>
        /// Index in array operation.
        /// </summary>
        void IndexInArray(int array, int index, int outName);

        /// <summary>
        /// Outputs pin component. It must be copied since variable can still be written to.
        /// </summary>
        /// <param name="component">The component</param>
        /// <param name="n">The variable name to bind to component.</param>
        void Output(PinComponent component, PinFormat fmt, int n);

    }
}
