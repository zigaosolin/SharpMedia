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
    /// Represents required configuration from a type
    /// </summary>
    [Serializable]
    public class TypeRequiredConfiguration : IRequiresConfiguration, IRequiresComponentDirectory
    {
        #region Private
        string typeName;

        [NonSerialized]
        ConstructorConfiguration[] preinitializers = null;

        [NonSerialized]
        IRequirement[] requirements = null;

        [NonSerialized]
        object instance = null;

        private void CachePreinitializers()
        {
            if (preinitializers != null) return;

            Type type = Type.GetType(typeName);
            CachePreinitializers(type);
        }

        private void CachePreinitializers(Type type)
        {
            List<ConstructorConfiguration> preList = new List<ConstructorConfiguration>();

            foreach (ConstructorInfo info in type.GetConstructors(
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                bool ignore = false;
                foreach (Attribute attr in info.GetCustomAttributes(true))
                {
                    if (attr is IgnoreAttribute) { ignore = true; break; }
                }

                if (ignore) continue;

                preList.Add(new ConstructorConfiguration(info));
            }

            // TODO: Add static methods returning the type as well

            preinitializers = preList.ToArray();
        }

        private void CacheRequirements()
        {
            if (requirements != null) return;

            Type type = Type.GetType(typeName);
            CacheRequirements(type);
        }

        private void CacheRequirements(Type type)
        {
            List<IRequirement> reqList = new List<IRequirement>();

            foreach (PropertyInfo info in
                type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                bool ignore = !info.CanWrite;
                foreach (Attribute attr in info.GetCustomAttributes(true))
                {
                    if (attr is IgnoreAttribute) { ignore = true; break; }
                }
                if (ignore) continue;

                reqList.Add(new PropertyRequirement(info, this));
            }

            requirements = reqList.ToArray();
        }

        #endregion

        #region IRequiresConfiguration Members

        public IRequiresConfiguration[] PreInitializers
        {
            get
            {
                CachePreinitializers();
                return preinitializers;
            }
        }

        public IRequirement[] Requirements
        {
            get 
            {
                CacheRequirements();
                return requirements;
            }
        }

        public object Instance
        {
            get 
            {
                if (instance == null)
                {
                    return null;
                }

                foreach (IRequirement req in requirements)
                {
                    if (req.Hard && !req.Satisfied)
                    {
                        throw new ComponentLookupException(
                            String.Format("Cannot instantiate instance, hard requirement {0} not met", req.Name));
                    }
                }

                return instance;
            }
        }

        public object UnconfiguredInstance
        {
            get 
            {
                if (instance == null)
                {
                    throw new ComponentLookupException("This required configuration must be preinitialized before yielding an unconfigured instance");
                }

                return instance;
            }
        }

        public IRequiresConfiguration Clone()
        {
            return new TypeRequiredConfiguration(typeName);
        }

        public void Preinitialize(IRequiresConfiguration config)
        {
            // call the constructor, get the instance
            bool found = false;
            foreach (IRequiresConfiguration cfg in preinitializers)
            {
                if (cfg == config) { found = true; break; }
            }

            if (!found || !(config is ConstructorConfiguration))
            {
                throw new Exception("Preinitializer does not belong to this instance");
            }

            instance = config.Instance;
        }

        #endregion

        public TypeRequiredConfiguration(string typeName)
        {
            this.typeName = typeName;
        }

        public TypeRequiredConfiguration(Type type)
        {
            this.typeName = type.FullName;
            this.CachePreinitializers(type);
            this.CacheRequirements(type);
        }

        public TypeRequiredConfiguration(Type type, object instance)
        {
            this.typeName = type.FullName;
            this.instance = instance;
            this.preinitializers = new ConstructorConfiguration[0];
            this.CacheRequirements();
        }

        #region IRequiresComponentDirectory Members

        public IComponentDirectory ComponentDirectory
        {
            set 
            {
                CachePreinitializers();
                foreach (object obj in preinitializers)
                {
                    if (obj is IRequiresComponentDirectory) 
                        (obj as IRequiresComponentDirectory).ComponentDirectory = value;
                }

                CacheRequirements();
                foreach (object obj in requirements)
                {
                    if (obj is IRequiresComponentDirectory)
                        (obj as IRequiresComponentDirectory).ComponentDirectory = value;
                }
            }
        }

        #endregion
    }
}
