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

namespace SharpMedia.Components.Configuration.ComponentProviders
{
    /// <summary>
    /// An instance with view support.
    /// </summary>
    /// <remarks>View must have a constructor with one argument, the instance.</remarks>
    public sealed class InstanceViewSupport : MarshalByRefObject, IComponentProvider
    {
        #region Private Members
        Instance root;
        Type viewType;
        string name;
        Type exposedType;
        #endregion

        #region IComponentProvider Members

        public object GetInstance(IComponentDirectory componentDirectory, object clientInstance, 
            string requirementName, string requirementType)
        {
            // We obtain root object 
            // FIXME: should we use those parameters
            object rootObject = root.GetInstance(componentDirectory, clientInstance, 
                requirementName, requirementType);

            // We create a view proxy.
            ConstructorInfo[] info = viewType.GetConstructors(
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (ConstructorInfo constructor in info)
            {
                if (constructor.GetParameters().Length == 1)
                {
                    // We call first contructor with such parameters.
                    return constructor.Invoke(new object[] { rootObject });
                }
            }

            throw new Exception("Could not create view.");
        }

        public string[] MatchedTypes
        {
            get { return new string[] { exposedType.FullName }; }
        }

        public string MatchedName
        {
            get { return name; }
        }

        public bool MatchAgainstNameAllowed
        {
            get { return name != null; }
        }

        public bool MatchAgainstTypeAllowed
        {
            get { return true; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with singleton instance and view type.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="type"></param>
        /// <param name="viewType"></param>
        public InstanceViewSupport(Instance instance, Type type, Type expodsedType)
        {
            this.root = instance;
            this.viewType = type;
            this.exposedType = expodsedType;
        }

        /// <summary>
        /// Constructor with singleton instance and view type.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="exposedType"></param>
        public InstanceViewSupport(Instance instance, Type type, Type exposedType, string name)
        {
            this.root = instance;
            this.viewType = type;
            this.exposedType = viewType;
            this.name = name;
        }

        #endregion
    }
}
