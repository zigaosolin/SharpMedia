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

namespace SharpMedia.Components.Configuration
{

    /// <summary>
    /// A configuration attribute.
    /// </summary>
    public abstract class ConfigurationAttribute : Attribute
    {
        public ConfigurationAttribute()
        {
        }
    }


    /// <summary>
    /// Set on properties or constructor arguments that you wish to indicate are hard requirements
    /// </summary>
    /// <remarks>An object will not be used through the COS without its hard requirements met</remarks>
    [global::System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class RequiredAttribute : ConfigurationAttribute
    {
        // This is a positional argument.
        public RequiredAttribute()
        {
        }
    }

    /// <summary>
    /// Set on properties to ensure persistency on application exit. Must support get.
    /// </summary>
    /// <remarks>An object will not be used through the COS without its hard requirements met</remarks>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class PersistantAttribute : ConfigurationAttribute
    {
        // This is a positional argument.
        public PersistantAttribute()
        {
        }
    }

    /// <summary>
    /// Set on properties or constructor arguments that you wish to have ignored by the COS.
    /// </summary>
    /// <remarks>Value typed constructor arguments will be set to default values and instances to null</remarks>
    [global::System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Class | AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
    public sealed class IgnoreAttribute : ConfigurationAttribute
    {
        // This is a positional argument.
        public IgnoreAttribute()
        {
        }
    }

    /// <summary>
    /// Set on properties that are to be assigned to before another one
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class BeforeAttribute : ConfigurationAttribute
    {
        private string otherComponent;

	    public string OtherComponent
	    {
		    get { return otherComponent;}
		    set { otherComponent = value;}
	    }

        // This is a positional argument.
        public BeforeAttribute(string otherComponent)
        {
            this.otherComponent = otherComponent;
        }
    }

    /// <summary>
    /// Set on properties that are to be assigned to after another one
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class AfterAttribute : ConfigurationAttribute
    {
        private string otherComponent;

        public string OtherComponent
        {
            get { return otherComponent; }
            set { otherComponent = value; }
        }

        // This is a positional argument.
        public AfterAttribute(string otherComponent)
        {
            this.otherComponent = otherComponent;
        }
    }

    /// <summary>
    /// Properties and parameters with this attribute have their value defaulted to an instance of a class name
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class DefaultClassAttribute : ConfigurationAttribute
    {
        readonly string className;

        /// <param name="className">Name of the class that is instanced as a value for this property by default</param>
        public DefaultClassAttribute(string className)
        {
            this.className = className;
        }

        /// <param name="classType">Type of the class that is instanced as a value for this property by default</param>
        public DefaultClassAttribute(Type classType)
        {
            this.className = classType.FullName;
        }

        /// <summary>
        /// Name of the class that is instanced as a value for this property by default
        /// </summary>
        public string ClassName
        {
            get
            {
                return this.className;
            }
        }
    }

    /// <summary>
    /// Properties and parameters with this attribute have their value defaulted to a component name
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class DefaultNameAttribute : ConfigurationAttribute
    {
        readonly string componentName;

        /// <param name="componentName">Name of the component that is instanced as a value for this property by default</param>
        public DefaultNameAttribute(string componentName)
        {
            this.componentName = componentName;
        }

        /// <summary>
        /// Name of the component that is instanced as a value for this property by default
        /// </summary>
        public string ComponentName
        {
            get
            {
                return this.componentName;
            }
        }
    }
}
