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
    /// Ignore an interface when registering this configured component
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class IgnoreInterfaceAttribute : ConfigurationAttribute
    {
        readonly string interfaceName;

        /// <summary>
        /// Ignore an interface when registering this configured component
        /// </summary>
        /// <param name="interfaceName">Which interface type name to ignore</param>
        public IgnoreInterfaceAttribute(string interfaceName)
        {
            this.interfaceName = interfaceName;
        }

        /// <summary>
        /// Ignore an interface when registering this configured component
        /// </summary>
        /// <param name="type">Which interface type to ignore</param>
        public IgnoreInterfaceAttribute(Type type)
        {
            this.interfaceName = type.FullName;
        }

        public string InterfaceName
        {
            get
            {
                return this.interfaceName;
            }
        }
    }

    /// <summary>
    /// Ignore some base classes when registering this configured component
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class IgnoreBaseClassAttribute : ConfigurationAttribute
    {
        readonly string[] baseClasses;

        /// <summary>
        /// Ignore some base classes when registering this configured component
        /// </summary>
        /// <param name="baseClasses">Which base class names to ignore</param>
        public IgnoreBaseClassAttribute(string[] baseClasses)
        {
            this.baseClasses = baseClasses;
        }

        /// <summary>
        /// Ignore some base classes when registering this configured component
        /// </summary>
        /// <param name="baseClasses">Which base classes to ignore</param>
        public IgnoreBaseClassAttribute(Type[] baseClasses)
        {
            this.baseClasses = Array.ConvertAll<Type, String>(
                baseClasses,
                delegate(Type t) { return t.FullName; });
        }

        /// <summary>
        /// Ignore a base class when registering this configured component
        /// </summary>
        /// <param name="baseClasses">Which base classes to ignore</param>
        public IgnoreBaseClassAttribute(string baseClass)
        {
            this.baseClasses = new string[] { baseClass };
        }

        /// <summary>
        /// Ignore a base class when registering this configured component
        /// </summary>
        /// <param name="baseClasses">Which base classes to ignore</param>
        public IgnoreBaseClassAttribute(Type baseClass)
        {
            this.baseClasses = new string[] { baseClass.FullName };
        }

        /// <summary>
        /// Ignore all base classes when registering this configured component
        /// </summary>
        public IgnoreBaseClassAttribute()
        {
            this.baseClasses = IgnoreBaseClasses;
        }

        public string[] BaseClasses
        {
            get
            {
                return baseClasses;
            }
        }

        public static readonly string[] IgnoreBaseClasses = new string[] { "<IGNORE#ALL>" };
    }
}
