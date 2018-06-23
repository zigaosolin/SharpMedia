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
using System.IO;
using SharpMedia.Math.Graphs;
using SharpMedia.Math;
using SharpMedia.Caching;
using SharpMedia.AspectOriented;
using System.Runtime.Serialization;

namespace SharpMedia.Graphics.Shaders
{

    /// <summary>
    /// A directed acyclic graph that represents one type of shader (hardware shader group). Each
    /// ShaderCode is composed of several operations that are connected using pins.
    /// </summary>
    /// <remarks>
    /// ShaderCode needs recompilation for changed fixed parameters or if any operation is changed. Operation
    /// changes are tracked by ShaderCode. At some stage, ShaderCode is made immutable and no changes are allowed.
    /// </remarks>
    [Serializable]
    public partial class ShaderCode : ICommentable, IScope, IDisposable
    {
        #region Private Members

        // The data part.
        InputOperation input;
        OutputOperation output;
        BindingStage stage;
        string comment = string.Empty;

        SortedDictionary<string, ConstantOperation> constants = new SortedDictionary<string, ConstantOperation>();
        ParameterDescription[] paramDesc;
        
        // Not serializable part.
        [NonSerialized]
        List<IOperation> cachedOps;
        [NonSerialized]
        bool immutable = true;
        

        // Caching part.
        [NonSerialized]
        object cacheSyncRoot = new object();
        [NonSerialized]
        Cache<FixedShaderParameters> cache = new Cache<FixedShaderParameters>(1.0f, 1.0f,
            new LRU<FixedShaderParameters>(0.2f, 1.0f));
        #endregion

        #region Private Methods

        ~ShaderCode()
        {
            Dispose(true);
        }

        void Dispose(bool fin)
        {
            lock (cacheSyncRoot)
            {
                cache.EvictAll();
            }

            if (!fin)
            {
                GC.SuppressFinalize(this);
            }
            else
            {
                Common.NotDisposed(this, "ShaderCode must be disposed.");
            }
        }

        private ConstantOperation Add(ConstantOperation c)
        {
            constants.Add(c.Name, c);
            return c;
        }


        /// <summary>
        /// Validates and throws if something is wrong.
        /// </summary>
        void ValidateThrow()
        {
            
        }

        [OnSerializing]
        void ValidateSerialization(StreamingContext ignored)
        {
            if (!immutable)
            {
                throw new InvalidOperationException("ShaderCode must be immutable in order to be serializable.");
            }
        }

        #endregion

        #region Constant Creation

        /// <summary>
        /// Creates a fixed (unreferencable) constant based on some value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Constant that represents this value.</returns>
        public ConstantOperation CreateFixed(object value)
        {
            if (value is float)
            {
                return new ConstantOperation(null, PinFormat.Float, Pin.NotArray, value, this);
            }
            else if (value is Vector2f)
            {
                return new ConstantOperation(null, PinFormat.Floatx2, Pin.NotArray, value, this);
            }
            else if (value is Vector3f)
            {
                return new ConstantOperation(null, PinFormat.Floatx3, Pin.NotArray, value, this);
            }
            else if (value is Vector4f)
            {
                return new ConstantOperation(null, PinFormat.Floatx4, Pin.NotArray, value, this);
            }
            else if (value is uint)
            {
                return new ConstantOperation(null, PinFormat.UInteger, Pin.NotArray, value, this);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Creates constant from scalar.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <returns></returns>
        public ConstantOperation FromScalar(float scalar)
        {
            return CreateFixed(scalar);
        }

        /// <summary>
        /// Creates a constant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <param name="fmt">The format of constant.</param>
        /// <returns>The constant providing operation.</returns>
        public ConstantOperation CreateConstant(string name, PinFormat fmt)
        {
            return Add(new ConstantOperation(name, fmt, Pin.NotArray, this));
        }

        /// <summary>
        /// Creates a texture constant.
        /// </summary>
        public ConstantOperation CreateConstant(string name, PinFormat fmt, PinFormat texFormat)
        {
            return Add(new ConstantOperation(name, fmt, texFormat, this));
        }

        /// <summary>
        /// Creates a constant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <param name="fmt">The format of constant.</param>
        /// <param name="size">The default value of constant.</param>
        /// <returns>The constant providing operation.</returns>
        public ConstantOperation CreateConstant(string name, PinFormat fmt, uint size)
        {
            return Add(new ConstantOperation(name, fmt, size, null, this));
        }

        /// <summary>
        /// Creates a constant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <param name="fmt">The format of constant.</param>
        /// <param name="value">The default value of constant.</param>
        /// <returns>The constant providing operation.</returns>
        public ConstantOperation CreateConstant(string name, PinFormat fmt, object value)
        {
            return Add(new ConstantOperation(name, fmt, Pin.NotArray, value, this));
        }

        /// <summary>
        /// Creates an array constant.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <param name="fmt">The format of constant.</param>
        /// <param name="size">Size of array. Can be special (Pin.NotArray or Pin.DynamicArray).</param>
        /// <param name="value">The default value of constant.</param>
        /// <returns>The constant providing operation.</returns>
        public ConstantOperation CreateConstant(string name, PinFormat fmt, uint size, object value)
        {
            return Add(new ConstantOperation(name, fmt, size, value, this));
        }


        /// <summary>
        /// Obtains constant with specific name.
        /// </summary>
        /// <param name="name">The name of constant.</param>
        /// <returns>The constant.</returns>
        public ConstantOperation GetConstant(string name)
        {
            return constants[name];
        }

        /// <summary>
        /// Removes the constant.
        /// </summary>
        /// <param name="name">Name of constant to remove.</param>
        public bool Remove(string name)
        {
            ConstantOperation c;
            if(constants.TryGetValue(name, out c))
            {

                // Discard constant.
                c.Discard();

                return constants.Remove(name);
            }
            return false;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Sets if ShaderCode can be changed. We usually code ShaderCode only once (by editor), when it is first
        /// serialized, it is made immutable and noone can change it.
        /// </summary>
        public bool Immutable
        {
            get
            {
                return immutable;
            }
            set
            {
                if (value == false && immutable == true)
                {
                    throw new InvalidOperationException("Immutable ShaderCode cannot be made mutable (thread-safety reasons) " +
                        " if you need editing, use scope.Edittable property");
                }
                ValidateThrow();
                immutable = value;

                // We create description.
                paramDesc = new ParameterDescription[constants.Count];

                // We prepare parameters.
                int i = 0;
                foreach (ConstantOperation c in constants.Values)
                {
                    paramDesc[i++] = c.ParameterDescription;
                }
            }
        }

        /// <summary>
        /// Obtains a copy of ShaderCode that is in immutable state. It is always a copy.
        /// </summary>
        public ShaderCode Edittable
        {
            get
            {
                ShaderCode d = Clone();
                d.immutable = false;
                return d;
            }
        }

        /// <summary>
        /// The stage where this ShaderCode is to be applied to.
        /// </summary>
        public BindingStage Stage
        {
            get
            {
                return stage;
            }
        }

        /// <summary>
        /// The input operation.
        /// </summary>
        public InputOperation InputOperation
        {
            get
            {
                return input;
            }
        }

        /// <summary>
        /// The output operation.
        /// </summary>
        public OutputOperation OutputOperation
        {
            get
            {
                return output;
            }
        }

        /// <summary>
        /// Parameters on global scope (interface parameters are added later and are based on actual interface type).
        /// </summary>
        public ParameterDescription[] ParameterDescription
        {
            get
            {
                if (immutable == false)
                {
                    throw new InvalidOperationException("Parameter description requires the ShaderCode to be immutable.");
                }
                else
                {
                    return paramDesc;
                }
            }
        }

        /// <summary>
        /// Obtains fixed parameters template, needed to fill necessary data and compile shader.
        /// </summary>
        public FixedShaderParameters FixedParameters
        {
            get
            {
                if (immutable == false)
                {
                    throw new InvalidOperationException("Fixed parameters require the ShaderCode to be immutable.");
                }


                return new FixedShaderParameters(this, paramDesc);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates ShaderCode using stage.
        /// </summary>
        /// <param name="stage">The stage.</param>
        public ShaderCode(BindingStage dagStage)
        {
            stage = dagStage;
            immutable = false;

            input = new InputOperation(this);
            if (dagStage == BindingStage.PixelShader)
            {
                output = new PixelOutputOperation(this);
            }
            else
            {
                output = new OutputOperation(this);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clones ShaderCode and all suboperations. Not thread safe (it is thread safe if in immutable state).
        /// </summary>
        /// <returns>Clones ShaderCode.</returns>
        public ShaderCode Clone()
        {
            MemoryStream s = new MemoryStream();
            try
            {
                // TODO: optimize later
                Common.SerializeToStream(s, this);
                s.Seek(0, SeekOrigin.Begin);
                return (ShaderCode) Common.DeserializeFromStream(s);
            }
            finally
            {
                s.Dispose();
            }
        }

        /// <summary>
        /// Forces validation.
        /// </summary>
        public void Validate()
        {
            // Validate only if not yet validated.
            ValidateThrow();
        }

        /// <summary>
        /// Compiles using a device and fixed parameters.
        /// </summary>
        /// <param name="device">The device.</param>
        public IShader Compile(GraphicsDevice device, FixedShaderParameters parameters)
        {
            using (ShaderCompiler compiler = device.CreateShaderCompiler())
            {
                return Compile(compiler, parameters);
            }
        }

        /// <summary>
        /// Compiles using a given shader compiler.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        /// <param name="parameters">Shader parameters.</param>
        public IShader Compile(ShaderCompiler compiler, FixedShaderParameters parameters)
        {
            if (immutable == false || parameters == null || parameters.ShaderCode != this)
            {
                throw new ArgumentNullException("Argument is invalid (null) or is not part of this ShaderCode.");
            }

            lock (cacheSyncRoot)
            {
                ICacheable shader = cache.FindAndTouch(parameters);
                if (shader != null)
                {
                    return shader as IShader;
                }
            }

            // We make validation of parameters and layouts.
            parameters.IsDefinedThrow();

            // ShaderCode must be immutable here if parameters were sucessfully created, operations must be cached.
            List<KeyValuePair<Pin, ShaderCompiler.Operand>> operands = new List<KeyValuePair<Pin, ShaderCompiler.Operand>>();
            cachedOps = (cachedOps == null) ? GraphHelper.GetSortedOperations(output) : cachedOps;
            
            List<DualShareContext> shareData = new List<DualShareContext>();

            // Begin compilation process.
            compiler.Begin(stage);
            
            // We go through all operations in right order
            for (int i = cachedOps.Count - 1; i >= 0; i--)
            {
                IOperation operation = cachedOps[i];

                // First prepare all inputs.
                Pin[] pins = operation.Inputs;
                ShaderCompiler.Operand[] inputs = new ShaderCompiler.Operand[pins.Length];
                for (uint j = 0; j < pins.Length; j++)
                {
                    // We extract it.
                    for (int z = 0; z < operands.Count; z++)
                    {
                        if (object.ReferenceEquals(operands[z].Key, pins[j]))
                        {
                            inputs[j] = operands[z].Value;
                            continue;
                        }
                    }
                }

                // Find if sharing context exists.
                int idx = shareData.FindIndex(delegate(DualShareContext c) { return c.DestinationOperation == operation; });
                DualShareContext shareContext = null;
                if(idx >= 0) 
                {
                    shareContext = shareData[idx];
                    shareData.RemoveAt(idx);
                }

                // Compile the operation.
                ShaderCompiler.Operand[] outputs = operation.Compile(compiler, inputs, 
                                                parameters, ref shareContext);

                // We add it if it is not the same as "this" and notn-null.
                if (shareContext != null && shareContext.DestinationOperation != operation)
                {
                    shareData.Add(shareContext);
                }

                // Add all outputs.
                for (uint z = 0; z < outputs.Length; z++)
                {
                    operands.Add(new KeyValuePair<Pin, ShaderCompiler.Operand>(operation.Outputs[z], outputs[z]));
                }
            }

            // We do the compile process.
            IShader shader1 = compiler.End(parameters);

            // We add it to cache.
            lock (cacheSyncRoot)
            {
                try
                {
                    cache.Add(parameters, shader1);
                } catch(Exception)
                {
                    // This is the case when cache probably re-inserted shader.
                    IShader shader2 = cache.FindAndTouch(parameters) as IShader;
                    if (shader2 != null)
                    {
                        Common.Warning(typeof(ShaderCode), "Recompiled the same shader two times in a row.");

                        shader1.Dispose();
                        return shader2;
                    }

                    // If not, we rethrow.
                    throw;
                }
            }

            return shader1;

        }

        /// <summary>
        /// This is signaled when ShaderCode is changed and we must release the ops cache. This
        /// *must* be called before the change is made because it may be rejected (exception
        /// is thrown if rejected).
        /// </summary>
        public void SignalChanged()
        {
            if (immutable)
            {
                throw new InvalidOperationException("Cannot change ShaderCode, it is in immutable state.");
            }
            cachedOps = null;
        }

        #endregion

        #region ICommentable Members

        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                if (immutable == false)
                {
                    throw new InvalidOperationException("Cannot set comment for immutable ShaderCode.");
                }
                comment = value;
            }
        }

        #endregion

        #region IScope Members

        public bool IsInScope(IScope other)
        {
            IScope scope = other.ParentScope;
            while (scope != null)
            {
                if (scope == this) return true;
                scope = scope.ParentScope;
            }

            return false;
        }

        public IScope ParentScope
        {
            get { return null; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(false);
        }

        #endregion

    }
}
