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
using System.Reflection;

namespace SharpMedia.Components.Configuration.Requirements
{
    /// <summary>
    /// Constructor configuration
    /// </summary>
    [Serializable]
    public class ConstructorConfiguration : IRequiresConfiguration, IRequiresComponentDirectory
    {

        internal class ParameterRequirement : IRequirement, IRequiresComponentDirectory
        {
            ParameterInfo parameter;
            ConstructorConfiguration parent;
            object value;
            bool isSet = false;
            bool valueTypesOnly = false;
            bool hard = false;

            internal object Value { get { return value; } }

            internal ParameterRequirement(ConstructorConfiguration parent, ParameterInfo parameter)
            {
                this.parent    = parent;
                this.parameter = parameter;

                this.valueTypesOnly = parameter.ParameterType.IsValueType;
                foreach (Attribute attrib in parameter.GetCustomAttributes(true))
                {
                    if (attrib is RequiredAttribute) hard = true;
                }
            }

            #region IRequirement Members

            public bool ValueTypesOnly
            {
                get { return valueTypesOnly; }
            }

            public bool Hard
            {
                get { return hard; }
            }

            public string Name
            {
                get { return Common.StringCapitalize(parameter.Name); }
            }

            public string Type
            {
                get { return parameter.ParameterType.FullName; }
            }

            public void SetValue(object value)
            {
                this.value = value;
                this.isSet = true;
            }

            public bool Satisfied
            {
                get { return isSet; }
            }

            IConfigurationValue defaultValue = null;
            public IConfigurationValue Default
            {
                get
                {
                    return defaultValue;
                }
                set
                {
                    defaultValue = value;
                }
            }
            #endregion

            #region IRequiresComponentDirectory Members

            public IComponentDirectory ComponentDirectory
            {
                set
                {
                    if (defaultValue != null && defaultValue is IRequiresComponentDirectory)
                        (defaultValue as IRequiresComponentDirectory).ComponentDirectory = value;
                }
            }

            #endregion
        }

        ConstructorInfo constructor;

        [NonSerialized]
        object instance;

        [NonSerialized]
        Dictionary<ParameterInfo, ParameterRequirement> parameters = new Dictionary<ParameterInfo, ParameterRequirement>();

        #region IRequiresConfiguration Members

        static readonly IRequiresConfiguration[] nullPreInitializers = new IRequiresConfiguration[0];

        public IRequiresConfiguration[] PreInitializers
        {
            get { return nullPreInitializers; }
        }

        public IRequirement[] Requirements
        {
            get
            {
                ParameterRequirement[] arr = new ParameterRequirement[parameters.Values.Count];
                parameters.Values.CopyTo(arr, 0);
                return Array.ConvertAll<ParameterRequirement, IRequirement>(arr,
                    delegate(ParameterRequirement p) { return (IRequirement)p; });
            }
        }

        public object Instance
        {
            get 
            {
                if (instance != null)
                    return instance;

                return CacheInstance();
            }
        }

        public object UnconfiguredInstance
        {
            get { return Instance; }
        }

        public void Preinitialize(IRequiresConfiguration config)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ICloneable<IRequiresConfiguration> Members

        public IRequiresConfiguration Clone()
        {
            return new ConstructorConfiguration(constructor);
        }

        #endregion

        public ConstructorConfiguration(ConstructorInfo constructor)
        {
            this.constructor = constructor;
            CacheParameters();
        }

        private object CacheInstance()
        {
            if (parameters == null) CacheParameters();

            foreach (ParameterRequirement req in parameters.Values)
            {
                if (!req.Satisfied && req.Hard)
                    throw new Exception(
                        String.Format("Cannot instantiate instance, hard requirement {0} not met", req.Name));
            }

            /* create a parameter store */
            object[] parameterStore = new object[constructor.GetParameters().Length];
            int i = 0;
            foreach (ParameterInfo param in constructor.GetParameters())
            {
                parameterStore[i] = parameters.ContainsKey(param) 
                    ? (parameters[param].Satisfied 
                        ? parameters[param].Value 
                        : DefaultValue(param)) 
                    : DefaultValue(param);

                ++i;
            }

            instance = constructor.Invoke(parameterStore);
            return instance;
        }

        private object DefaultValue(ParameterInfo param)
        {
            return null;
            /*
            return param.ParameterType.IsValueType
                ? null
                : param.ParameterType.GetConstructor(Type.EmptyTypes).Invoke(null);
             */
        }

        private void CacheParameters()
        {
            List<IRequirement> reqList = new List<IRequirement>();

            parameters.Clear();
            foreach (ParameterInfo param in constructor.GetParameters())
            {
                parameters[param] = new ParameterRequirement(this, param);
            }
        }

        #region IRequiresComponentDirectory Members

        public IComponentDirectory ComponentDirectory
        {
            set 
            {
                foreach (object obj in parameters.Values)
                {
                    if (obj is IRequiresComponentDirectory)
                        (obj as IRequiresComponentDirectory).ComponentDirectory = value;
                }
            }
        }

        #endregion
    }
}
