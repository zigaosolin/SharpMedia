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

namespace SharpMedia.Components.Database
{
    /// <summary>
    /// Parameter description
    /// </summary>
    [Serializable]
    public class ComponentParameter
    {
        private string parameterName;
        /// <summary>
        /// Name of the parameter to bind to
        /// </summary>
        public string ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }
    }

    /// <summary>
    /// Parameter defined with a string value
    /// </summary>
    [Serializable]
    public class TextParameter : ComponentParameter
    {
        private string value;
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public TextParameter() { }
        public TextParameter(string value) { this.value = value; }
    }

    /// <summary>
    /// Parameter defined with a reference to a component instance (by name)
    /// </summary>
    [Serializable]
    public class InstanceNameParameter : ComponentParameter
    {
        private string componentInstanceId;
        /// <summary>
        /// Id of the component value of the parameter
        /// </summary>
        /// <remarks>
        /// The component id may be optionally namespace qualified with [LibName].id, for example
        /// Company.Product.loggerFactory
        /// </remarks>
        public string ComponentInstanceId
        {
            get { return componentInstanceId; }
            set { componentInstanceId = value; }
        }

        public InstanceNameParameter() { }
        public InstanceNameParameter(string instanceName) { this.componentInstanceId = instanceName; }
    }

    /// <summary>
    /// Parameter defined with a reference to a component class (by name)
    /// </summary>
    [Serializable]
    public class ClassParameter : ComponentParameter
    {
        private string componentClassName;
        /// <summary>
        /// Name of the component class to use as the value of the parameter
        /// </summary>
        public string ComponentClassName
        {
            get { return componentClassName; }
            set { componentClassName = value; }
        }

        public ClassParameter() { }
        public ClassParameter(string className) { this.componentClassName = className; }
    }
}