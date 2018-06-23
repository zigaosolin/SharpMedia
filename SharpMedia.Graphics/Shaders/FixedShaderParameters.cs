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

namespace SharpMedia.Graphics.Shaders
{

    /// <summary>
    /// A fixed parameters describe all those parameters that are fixed during the shader
    /// compiling. Compiler can take advantage od them (loop unrollings) and some parameters
    /// are even required to be fixed (such as interfaces, dynamic arrays and interface arrays).
    /// </summary>
    public class FixedShaderParameters : IEquatable<FixedShaderParameters>, IComparable<FixedShaderParameters>
    {
        #region Private Members
        // DAG part.
        ShaderCode shaderCode;
        ParameterDescription[] parameters;

        // Layouts define how/where constants are stored.
        List<ConstantBufferLayout> layouts = new List<ConstantBufferLayout>();
        SortedList<string, object> fixedParameters = new SortedList<string, object>();
        #endregion

        #region Internal Methods

        internal FixedShaderParameters(ShaderCode code, ParameterDescription[] parameters)
        {
            this.shaderCode = code;
            this.parameters = parameters;
        }


        /// <summary>
        /// We check the parameter and do it recursevelly in case of interfaces.
        /// </summary>
        /// <param name="parameter"></param>
        private bool CheckParam(ParameterDescription parameter, out string errorDesc)
        {
            errorDesc = null;

            // We first find it.
            object value;
            if (!fixedParameters.TryGetValue(parameter.Name, out value))
            {
                if (parameter.IsFixed)
                {
                    errorDesc = string.Format("Parameter {0} is not defined in fixed shader parameters and it should be.", parameter.Name);
                    return false;
                }

                // We must locate parameter in one of constant layouts.
                foreach (ConstantBufferLayout layout in layouts)
                {
                    if (layout.IsDefined(parameter.Name)) return true;
                }

                errorDesc = string.Format("Parameter {0} is not defined anywhere", parameter.Name);
                return false;
            }

            // We first check if format is compatible and "sizes" are compatible.
            if (parameter.Pin.IsDynamicArray)
            {
                switch (parameter.Pin.Format)
                {
                    case PinFormat.Texture1D:
                    case PinFormat.Texture1DArray:
                    case PinFormat.Texture2D:
                    case PinFormat.Texture2DArray:
                    case PinFormat.TextureCube:
                    case PinFormat.Texture3D:
                    case PinFormat.BufferTexture:
                        if (value is uint[])
                        {
                            uint[] array = (uint[])value;


                            for (int i = 0; i < array.Length; i++)
                            {
                                if (array[i] >= GraphicsDevice.MaxTexture)
                                {
                                    errorDesc = string.Format("Index of texture must be in range [0,{0}) for all parameter {1}, it is not at least for index {2}",
                                        GraphicsDevice.MaxTexture, parameter.Name, i);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            errorDesc = string.Format("Value of format for parameter {0} must be uint[] that represents position of textures", parameter.Name);
                            return false;
                        }
                        break;
                    case PinFormat.Sampler:
                        if (value is uint[])
                        {
                            uint[] array = (uint[])value;

                            for (int i = 0; i < array.Length; i++)
                            {
                                if ((uint)array[i] >= GraphicsDevice.MaxStateObjectsPerType)
                                {
                                    errorDesc = string.Format("Index of sampler state must be in range [0,{0}) for parameter {1}, index {2}",
                                        GraphicsDevice.MaxTexture, parameter.Name, i);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            errorDesc = string.Format("Value of format for parameter {0} must be uint that represents positions of samplers", parameter.Name);
                            return false;
                        }
                        break;
                    case PinFormat.Interface:
                        if (value is IInterface[])
                        {
                            IInterface[] array = value as IInterface[];


                            for (int i = 0; i < array.Length; i++)
                            {
                                // We check consistency of interface when compiled (more efficient).
                                foreach (ParameterDescription d in array[i].AdditionalParameters)
                                {
                                    // We first "scope" the parameter.
                                    ParameterDescription d2 = ParameterDescription.ScopeParameter(parameter.Name, i, d);

                                    // Now we search for it (validate it).
                                    if (!CheckParam(d2, out errorDesc)) return false;
                                }
                            }
                        }
                        else
                        {
                            errorDesc = string.Format("The parameter {0} is not an interface array. All parameters must be filled in IInterface[] (and not other base type).", parameter.Name);
                            return false;
                        }

                        break;
                    default:
                        uint arraySize;
                        // For all others, we check type consistency.
                        if (!PinFormatHelper.IsCompatibleArray(parameter.Pin.Format, value, out arraySize))
                        {
                            errorDesc = string.Format("Array parameter {0} with format {1} is not compatible with value {2}", parameter.Name, parameter.Pin.Format, value);
                            return false;
                        }

                        // We check if array length is ok.
                        object va;
                        if (fixedParameters.TryGetValue(parameter.Name + ".length", out va) && (va is uint))
                        {
                            if ((uint)va != arraySize)
                            {
                                errorDesc = string.Format("Array size {0} for parameter {1} needn't be specified, but it is specified and it is {2} - not consistant.",
                                    arraySize, parameter.Name, va);
                                return false;
                            }
                        }
                        break;
                }
            }
            else if (parameter.Pin.IsStaticArray)
            {
                switch (parameter.Pin.Format)
                {
                    case PinFormat.Texture1D:
                    case PinFormat.BufferTexture:
                    case PinFormat.Texture1DArray:
                    case PinFormat.Texture2D:
                    case PinFormat.Texture2DArray:
                    case PinFormat.TextureCube:
                    case PinFormat.Texture3D:
                        if (value is uint[])
                        {
                            uint[] array = (uint[])value;

                            if (array.Length != parameter.Pin.Size)
                            {
                                errorDesc = string.Format("The length of array of parameter {0} is {1} but should be {2}", parameter.Name, array.Length, parameter.Pin.Size);
                                return false;
                            }

                            for(int i = 0; i < array.Length; i++)
                            {
                                if (array[i] >= GraphicsDevice.MaxTexture)
                                {
                                    errorDesc = string.Format("Index of texture must be in range [0,{0}) for all parameter {1}, it is not at least for index {2}", 
                                        GraphicsDevice.MaxTexture, parameter.Name, i);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            errorDesc = string.Format("Value of format for parameter {0} must be uint[] that represents position of textures", parameter.Name);
                            return false;
                        }
                        break;
                    case PinFormat.Sampler:
                        if (value is uint[])
                        {
                            uint[] array = (uint[])value;

                            if (array.Length != parameter.Pin.Size)
                            {
                                errorDesc = string.Format("The length of array of parameter {0} is {1} but should be {2}", parameter.Name, array.Length, parameter.Pin.Size);
                                return false;
                            }

                            for (int i = 0; i < array.Length; i++)
                            {
                                if ((uint)array[i] >= GraphicsDevice.MaxStateObjectsPerType)
                                {
                                    errorDesc = string.Format("Index of sampler state must be in range [0,{0}) for parameter {1}, index {2}", 
                                        GraphicsDevice.MaxTexture, parameter.Name, i);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            errorDesc = string.Format("Value of format for parameter {0} must be uint that represents positions of samplers", parameter.Name);
                            return false;
                        }
                        break;
                    case PinFormat.Interface:
                        if (value is IInterface[])
                        {
                            IInterface[] array = value as IInterface[];

                            if (array.Length != parameter.Pin.Size)
                            {
                                errorDesc = string.Format("The length of array of parameter {0} is {1} but should be {2}", parameter.Name, array.Length, parameter.Pin.Size);
                                return false;
                            }

                            for (int i = 0; i < array.Length; i++)
                            {
                                // We check consistency of interface when compiled (more efficient).
                                foreach (ParameterDescription d in array[i].AdditionalParameters)
                                {
                                    // We first "scope" the parameter.
                                    ParameterDescription d2 = ParameterDescription.ScopeParameter(parameter.Name, d);

                                    // Now we search for it (validate it).
                                    if (!CheckParam(d2, out errorDesc)) return false;
                                }
                            }
                        }
                        else
                        {
                            errorDesc = string.Format("The parameter {0} is not an interface array. All parameters must be filled in IInterface[] (and not other base type).", parameter.Name);
                            return false;
                        }

                        break;
                    default:

                        uint arraySize;
                        // For all others, we check type consistency.
                        if (!PinFormatHelper.IsCompatibleArray(parameter.Pin.Format, value, out arraySize))
                        {
                            errorDesc = string.Format("Array parameter {0} with format {1} is not compatible with value {2}", parameter.Name, parameter.Pin.Format, value);
                            return false;
                        }

                        // We check if array length is ok.
                        if (arraySize != parameter.Pin.Size)
                        {
                            errorDesc = string.Format("Parameter {0} with array size {1} is expected to be of size {2}", parameter.Name, arraySize, parameter.Pin.Size);
                            return false;
                        }
                        break;
                }
            }
            else
            {
                switch (parameter.Pin.Format)
                {
                    case PinFormat.Texture1D:
                    case PinFormat.BufferTexture:
                    case PinFormat.Texture1DArray:
                    case PinFormat.Texture2D:
                    case PinFormat.Texture2DArray:
                    case PinFormat.TextureCube:
                    case PinFormat.Texture3D:
                        if (value is uint)
                        {
                            if ((uint)value >= GraphicsDevice.MaxTexture)
                            {
                                errorDesc = string.Format("Index of texture must be in range [0,{0}) for parameter {1}", GraphicsDevice.MaxTexture, parameter.Name);
                                return false;
                            }
                        }
                        else
                        {
                            errorDesc = string.Format("Value of format for parameter {0} must be uint that represents position of texture", parameter.Name);
                            return false;
                        }
                        break;
                    case PinFormat.Sampler:
                        if (value is uint)
                        {
                            if ((uint)value >= GraphicsDevice.MaxStateObjectsPerType)
                            {
                                errorDesc = string.Format("Index of sampler state must be in range [0,{0}) for parameter {1}", GraphicsDevice.MaxTexture, parameter.Name);
                                return false;
                            }
                        }
                        else
                        {
                            errorDesc = string.Format("Value of format for parameter {0} must be uint that represents position of sampler", parameter.Name);
                            return false;
                        }
                        break;
                    case PinFormat.Interface:
                        if (value is IInterface)
                        {
                            IInterface ivalue = value as IInterface;
                            
                            // We check consistency of interface when compiled (more efficient).
                            foreach (ParameterDescription d in ivalue.AdditionalParameters)
                            {
                                // We first "scope" the parameter.
                                ParameterDescription d2 = ParameterDescription.ScopeParameter(parameter.Name, d);

                                // Now we search for it (validate it).
                                if (!CheckParam(d2, out errorDesc)) return false;
                            }
                        }
                        else
                        {
                            errorDesc = string.Format("The parameter {0} is not an interface (does not extend IInterface)", parameter.Name);
                            return false;
                        }

                        break;
                    default:
                        // For all others, we check type consistency.
                        if (!PinFormatHelper.IsCompatible(parameter.Pin.Format, value))
                        {
                            errorDesc = string.Format("Parameter {0} with format {1} is not compatible with value {2}", parameter.Name, parameter.Pin.Format, value);
                            return false;
                        }
                        break;
                }
            }


            return true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Shader code of fixed parameters.
        /// </summary>
        public ShaderCode ShaderCode
        {
            get
            {
                return shaderCode;
            }
        }

        /// <summary>
        /// Adds a layout (or replaces it) at specified index. May not add out of range.
        /// </summary>
        /// <param name="index">The index of layout.</param>
        /// <param name="layout">Constant buffer layout.</param>
        public void AddLayout(uint index, [NotNull] ConstantBufferLayout layout)
        {
            if(index > layouts.Count) throw new IndexOutOfRangeException("Cannot add at index out of range.");

            if (layouts.Count == index)
            {
                layouts.Add(layout);
            }
            else
            {
                layouts[(int)index] = layout;
            }
        }

        /// <summary>
        /// Appends a layout. Index of buffer is returned.
        /// </summary>
        /// <param name="layout">The layout.</param>
        public uint AppendLayout([NotNull] ConstantBufferLayout layout)
        {
            layouts.Add(layout);
            return (uint)(layouts.Count - 1);
        }

        /// <summary>
        /// Sets interface array (actually a helper that performs casts).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="array"></param>
        public void SetInterfaceArray<T>([NotEmpty] string name, [NotNull] IEnumerable<T> array) where T : IInterface
        {
            List<IInterface> result = new List<IInterface>();
            foreach (T el in array)
            {
                result.Add(el);
            }

            SetParameter(name, result.ToArray());
        }

        /// <summary>
        /// Sets a parameter.
        /// </summary>
        /// <param name="name">The name of parameter.</param>
        /// <param name="value">The value.</param>
        public void SetParameter([NotEmpty] string name, [NotNull] object value)
        {
            // We simply add the parameter.
            fixedParameters[name] = value;
        }

        /// <summary>
        /// We set array size for dynamic arrays. This is necessary only for arrays that will
        /// be defined in constant buffers.
        /// </summary>
        /// <param name="name">The name of dynamic array.</param>
        /// <param name="arraySize"></param>
        public void SetArraySize([NotEmpty] string name, uint arraySize)
        {
            fixedParameters[name + ".length"] = arraySize;
        }

        /// <summary>
        /// Indexed for parameters.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                return fixedParameters[name];
            }
            set
            {
                SetParameter(name, value);
            }
            
        }

        /// <summary>
        /// Obtains the interface bound under this name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetInterface([NotEmpty] string name)
        {
            return fixedParameters[name];
        }

        /// <summary>
        /// Obtains the interface array bound under this name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object[] GetInterfaceArray([NotEmpty] string name)
        {
            return fixedParameters[name] as object[];
        }

        /// <summary>
        /// Constants buffer collection.
        /// </summary>
        public ConstantBufferLayout[] ConstantBuffers
        {
            get
            {
                return layouts.ToArray();
            }
        }

        /// <summary>
        /// We check if parameters are defined.
        /// </summary>
        public bool IsDefined
        {
            get
            {
                string ignored;
                foreach (ParameterDescription desc in parameters)
                {
                    if (!CheckParam(desc, out ignored))
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// The same as IsDefined property, except that it contains the error string if not defined.
        /// </summary>
        /// <param name="errorString"></param>
        /// <returns></returns>
        public bool IsDefinedWithError(out string errorString)
        {
            errorString = null;
            foreach (ParameterDescription desc in parameters)
            {
                if (!CheckParam(desc, out errorString))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if parameter is defined and throws if it is not.
        /// </summary>
        public void IsDefinedThrow()
        {
            string error;
            if (!IsDefinedWithError(out error))
            {
                throw new InvalidRenderingStateConfiguration("Validation failed: " + error);
            }

        }
        
        /// <summary>
        /// We check if parameter is fixed.
        /// </summary>
        /// <param name="name">The name of parameter.</param>
        /// <returns></returns>
        public bool IsParameterFixed([NotEmpty] string name)
        {
            return fixedParameters.ContainsKey(name);
        }

        /// <summary>
        /// Obtains offset of element.
        /// </summary>
        public bool TryGetOffset(string name, out uint layoutID, out uint offset)
        {
            for (int i = 0; i < layouts.Count; i++)
            {
                if (layouts[i].TryGetOffset(name, out offset))
                {
                    layoutID = (uint)i;
                    return true;
                }
            }
            offset = 0;
            layoutID = uint.MaxValue;
            return false;
        }

        /// <summary>
        /// Obtains offset of element, throws if not found.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="layoutID"></param>
        /// <returns></returns>
        public uint GetOffset(string name, out uint layoutID)
        {
            uint r;
            if (!TryGetOffset(name, out layoutID, out r))
            {
                throw new ArgumentException("Name is not contained in parameter layouts.");
            }
            return r;
        }

        #endregion

        #region IEquatable<FixedShaderParameters> Members

        public bool Equals(FixedShaderParameters other)
        {
            return this.CompareTo(other) == 0;
        }

        #endregion

        #region IComparable<FixedShaderParameters> Members

        public int CompareTo([NotNull] FixedShaderParameters other)
        {
            // Compare layout sizes.
            int cmp = layouts.Count.CompareTo(other.layouts.Count);
            if (cmp != 0) return cmp;

            // Compare fixed parameter sizes.
            cmp = fixedParameters.Count.CompareTo(other.fixedParameters.Count);
            if (cmp != 0) return cmp;
            
            // We now compare layouts.
            for (int i = 0; i < layouts.Count; i++)
            {
                cmp = layouts[i].CompareTo(other.layouts[i]);
                if (cmp != 0) return cmp;
            }

            // We compare parameters (they must be ordered).
            for (int i = 0; i < fixedParameters.Count; i++)
            {
                object obj1 = fixedParameters.Values[i];
                object obj2 = fixedParameters.Values[i];

                // We first check if they are the same type.
                if (obj1.GetType() != obj2.GetType())
                {
                    return obj1.GetType().FullName.CompareTo(obj2.GetType().FullName);
                }
                else if (obj1 is IComparable && obj2 is IComparable)
                {
                    cmp = (obj1 as IComparable).CompareTo(obj2);
                    if (cmp != 0) return cmp;
                }
                else if (obj1 is IInterface[])
                {
                    // In case we have an interface array, we check for same type and size.
                    IInterface[] arr1 = obj1 as IInterface[];
                    IInterface[] arr2 = obj2 as IInterface[];

                    // First compare lengths.
                    cmp = arr1.Length.CompareTo(arr2.Length);
                    if (cmp != 0) return cmp;

                    // We now compare each type.
                    for (int j = 0; j < arr1.Length; j++)
                    {
                        cmp = arr1[j].GetType().FullName.CompareTo(arr2[j].GetType().FullName);
                        if (cmp != 0) return cmp;
                    }
                } else if (obj1 is IInterface)
                {
                    // If interfaces of the same type, we continue iteration.
                    continue;
                } else
                // TODO: handle arrays.
                {
                    // We do hash code compare.
                    cmp = obj2.GetHashCode() - obj1.GetHashCode();
                    if (cmp != 0) return cmp;

                    // If hash codes match, we have a problem.
                    throw new NotImplementedException();
                }
            }
            return 0;
        }

        #endregion
    }
}
