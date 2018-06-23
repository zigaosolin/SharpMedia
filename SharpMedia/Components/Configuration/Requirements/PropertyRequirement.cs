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
using SharpMedia.Components.Configuration.ConfigurationValues;
using SharpMedia.Components.Configuration.ComponentProviders;

namespace SharpMedia.Components.Configuration.Requirements
{
    /// <summary>
    /// A Requirement that sets a property of an instance and is governed by attributes
    /// set on the property
    /// </summary>
    [Serializable]
    public class PropertyRequirement : IRequirement, IRequiresComponentDirectory
    {
        #region IRequirement Members

        bool valueTypesOnly;
        public bool ValueTypesOnly
        {
            get { return valueTypesOnly; }
        }

        bool hard;
        public bool Hard
        {
            get { return hard; }
        }

        string name;
        public string Name
        {
            get { return name; }
        }

        string type;
        public string Type
        {
            get { return type; }
        }

        bool isSet = false;
        public void SetValue(object value)
        {
            object instance = configuration.UnconfiguredInstance;
            if (instance == null)
            {
                throw new Exception("Cannot set value on a null parent instance");
            }

            try
            {
                propertyInfo.SetValue(instance, value, null);
            }
            catch (Exception)
            {
                throw new Exception(
                    String.Format(
                        "Property {0} of type {1} does not support assignment of type {2} (Declaring Class: {3})",
                        name, propertyInfo.PropertyType, value.GetType(), propertyInfo.DeclaringType));
            }
            isSet = true;
        }

        public bool Satisfied
        {
            get 
            { 
                // Console.Out.WriteLine("{0} -> IsSatisfied? {1}", propertyInfo.Name, isSet); 
                return isSet; 
            }
        }

        IConfigurationValue defaultValue;
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

        IRequiresConfiguration configuration;
        PropertyInfo propertyInfo;

        /// <param name="pinfo">info of property to work with</param>
        /// <param name="config">configuration containing the instance to set to</param>
        public PropertyRequirement(PropertyInfo pinfo, IRequiresConfiguration config)
        {
            this.propertyInfo   = pinfo;
            this.configuration  = config;
            this.type           = pinfo.PropertyType.FullName;
            this.name           = pinfo.Name;
            this.valueTypesOnly = pinfo.PropertyType.IsValueType;
            this.hard           = false;

            foreach (Attribute attr in pinfo.GetCustomAttributes(true))
            {
                if (attr is RequiredAttribute) 
                {
                    this.hard = true; 
                }
                if (attr is DefaultClassAttribute)
                {
                    this.defaultValue = new ProviderValue(
                        new ConfiguredComponent((attr as DefaultClassAttribute).ClassName));
                }
                if (attr is DefaultNameAttribute)
                {
                    this.defaultValue = new ComponentRef("${" + (attr as DefaultNameAttribute).ComponentName + "}");
                }
            }
        }

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
}
