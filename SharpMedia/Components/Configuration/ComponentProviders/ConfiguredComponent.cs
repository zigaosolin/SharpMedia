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
using System.Xml;
using SharpMedia.Components.Configuration.Requirements;

namespace SharpMedia.Components.Configuration.ComponentProviders
{
    /// <summary>
    /// A fully configurable component provider
    /// </summary>
    [Serializable]
    public class ConfiguredComponent : IComponentProvider, IRequiresComponentDirectory
    {
        #region Private Members
        string typeName = string.Empty;

        private static object ConfigureComponentWithRequirements(
            IRequiresConfiguration requiredConfiguration, 
            IComponentConfiguration configuration, 
            IComponentDirectory componentDirectory)
        {
            // 3. fill in the other parameters from the component directory
            foreach (IRequirement req in requiredConfiguration.Requirements)
            {
                // Console.Out.WriteLine(">> Requirement: {0}", req.Name);
                if (configuration.Values.ContainsKey(req.Name))
                {
                    // Console.Out.WriteLine(" ... Contains Key");
                    req.SetValue(configuration.Values[req.Name].GetInstance(req.Type));
                }
                else
                {
                    if (req.Default != null)
                    {
                        // Console.Out.WriteLine(" ... Has Default");
                        // NEW CONFIGURED COMPONENT THAT IS 
                        req.SetValue(
                            componentDirectory.ConfigureInlineComponent(
                                new Instance(
                                    req.Default.GetInstance(req.Type))));
                    }
                    else if (HasComponent(componentDirectory, req.Type))
                    {
                        // Console.Out.WriteLine(" ... Setting By ICD");
                        req.SetValue(componentDirectory.GetInstanceByType(req.Type));
                    }
                    else if (req.Hard)
                    {
                        throw new ComponentConfigurationException(
                            null,
                            String.Format("ComponentOS KERNEL PANIC! I have no value for: {0}", req.Name));
                    }
                }
            }

            if (requiredConfiguration.Instance is IComponentPostInitialization)
            {
                (requiredConfiguration.Instance as IComponentPostInitialization).PostComponentInit(componentDirectory);
            }

            return requiredConfiguration.Instance;
        }
        #endregion

        #region IComponentProvider Members

        public virtual object GetInstance(
            IComponentDirectory componentDirectory, 
            object clientInstance, 
            string requirementName, 
            string requirementType)
        {
            ComponentDirectory = componentDirectory;

            {
                object instance = requiredConfiguration.Instance;
                if (instance != null) return instance;
            }


            // 1. compute the best pre-initializer to use

            int currentScore = 0;
            int maxScore = Int32.MinValue;
            IRequiresConfiguration currentInitializer = null;

            foreach (IRequiresConfiguration initializer in requiredConfiguration.PreInitializers)
            {
                // Console.Out.WriteLine("Initializer...");
                currentScore = 0;
                bool canUse = true;
                foreach (IRequirement req in initializer.Requirements)
                {
                    if (req.Default == null)
                    {
                        /* find the value */
                        if (configuration.Values.ContainsKey(req.Name))
                        {
                            currentScore++;
                        }
                        else if (HasComponent(componentDirectory, req.Type))
                        {
                            currentScore++;
                        }
                        else if (req.Hard)
                        {
                            /* DEBUG: Console.Out.WriteLine("Missing Parameter: {0} {1}", req.Type, req.Name); */
                            if (req.Hard) { canUse = false; break; }
                        }
                        else currentScore--;
                    }
                }

                if (canUse && (currentScore > maxScore))
                {
                    currentInitializer = initializer;
                    maxScore = currentScore;
                } 
            }
            // 1.1 give up if no pre-initializer can be used (because of unfulfillable hard requirements)
            if (currentInitializer == null)
            {
                throw new ComponentConfigurationException(this, "The component cannot be instantiated; No compatible pre-initializer exists");
            }

            // 1.2 give up if after pre-initialization, the object cannot be used (because of unfulfillable hard requirements)
            foreach (IRequirement req in requiredConfiguration.Requirements)
            {
                if (req.Default == null && req.Hard)
                {
                    /* if it's been solved through a pre-initializer */
                    bool found = false;
                    foreach (IRequirement reqpre in currentInitializer.Requirements)
                    {
                        if (reqpre.Name == req.Name)
                        {
                            found = true; break;
                        }
                    }

                    if (!found)
                    {
                        if (!req.ValueTypesOnly)
                        {
                            // query the ICD by type
                            if (HasComponent(componentDirectory, req.Type)) continue;
                        }

                        throw new ComponentConfigurationException(
                            this,
                            String.Format(
                                "The component cannot be instantiated; After pre-initialization, the {0} requirement is not satisfied",
                                req.Name));
                    }
                }
            }
            // 2. fill in the pre-initializer
            foreach (IRequirement req in currentInitializer.Requirements)
            {
                // Console.Out.WriteLine(">> PreInitializer Requirement: {0}", req.Name);
                if (configuration.Values.ContainsKey(req.Name))
                {
                    // Console.Out.WriteLine(" ... Contains Key");
                    req.SetValue(configuration.Values[req.Name].GetInstance(req.Type));
                }
                else
                {
                    if (req.Default != null)
                    {
                        // Console.Out.WriteLine(" ... Has Default");
                        req.SetValue(
                            componentDirectory.ConfigureInlineComponent(
                                new Instance(
                                    req.Default.GetInstance(req.Type))));
                    }
                    else if (HasComponent(componentDirectory, req.Type))
                    {
                        // Console.Out.WriteLine(" ... Setting By ICD (type={0})", req.Type);
                        req.SetValue(componentDirectory.GetInstanceByType(req.Type));
                    }
                    else if (req.Hard)
                    {
                        throw new ComponentConfigurationException(
                            this,
                            String.Format("ComponentOS KERNEL PANIC! I have no value for: {0}", req.Name));
                    }
                }
            }
            requiredConfiguration.Preinitialize(currentInitializer);

            return ConfigureComponentWithRequirements(requiredConfiguration, configuration, componentDirectory);
        }

        private static bool HasComponent(IComponentDirectory componentDirectory, string typeName)
        {
            try
            {
                // Console.Out.WriteLine(".. HasComponent {0}", typeName);
                return componentDirectory.FindByType(typeName).Length > 0;
            }
            catch (Exception) { return false; }
        }

        private static string ToString(Type obj)
        {
            return obj.FullName;
        }

        private void CacheTypes()
        {
            if (matchedTypes == null)
            {
                Type type = Type.GetType(typeName);

                CacheTypes(type);
            }
        }

        private void CacheTypes(Type type)
        {
            if (matchedTypes == null)
            {
                List<string> strTypes = new List<string>();
                strTypes.Add(type.FullName);

                foreach (Type iface in type.GetInterfaces())
                {
                    strTypes.Add(iface.FullName);
                }

                matchedTypes = strTypes.ToArray();
            }
        }

        string[] matchedTypes;
        public string[] MatchedTypes
        {
            get { CacheTypes();  return matchedTypes; }
            set { matchedTypes = value; }
        }

        string matchedName;
        public string MatchedName
        {
            get { return matchedName; }
            set { matchedName = value; }
        }

        bool matchAgainstName = true;
        public bool MatchAgainstNameAllowed
        {
            get { return matchAgainstName; }
        }

        bool matchAgainstType = true;
        public bool MatchAgainstTypeAllowed
        {
            get { return matchAgainstType; }
        }

        #endregion

        #region Properties
        protected IComponentConfiguration configuration;
        /// <summary>
        /// Provided configuration
        /// </summary>
        public IComponentConfiguration Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        protected IRequiresConfiguration requiredConfiguration;
        /// <summary>
        /// Required configuration
        /// </summary>
        public IRequiresConfiguration RequiredConfiguration
        {
            get { return requiredConfiguration; }
            set { requiredConfiguration = value; }
        }

        /// <summary>
        /// Returns the component's type.
        /// </summary>
        public Type ComponentType
        {
            get { return Type.GetType(typeName); }
        }
        #endregion

        #region IRequiresComponentDirectory Members

        IComponentDirectory componentDirectory;
        public IComponentDirectory ComponentDirectory
        {
            set
            {
                componentDirectory = value;
                if (configuration is IRequiresComponentDirectory)
                {
                    (configuration as IRequiresComponentDirectory).ComponentDirectory = value;
                }

                if (requiredConfiguration is IRequiresComponentDirectory)
                {
                    (requiredConfiguration as IRequiresComponentDirectory).ComponentDirectory = value;
                }
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Constructs from a type name
        /// </summary>
        public ConfiguredComponent(string typeName)
        {
            this.typeName              = typeName;
            this.requiredConfiguration = new TypeRequiredConfiguration(typeName);
            this.configuration         = new StandardConfiguration();
        }

        /// <summary>
        /// Constructs from a type name and component name
        /// </summary>
        public ConfiguredComponent(Type type, string componentName)
        {
            this.typeName = type.FullName;
            this.CacheTypes(type);
            this.requiredConfiguration = new TypeRequiredConfiguration(type);
            this.configuration = new StandardConfiguration();
            this.matchedName = componentName;
        }

        /// <summary>
        /// Constructs from a type name and component name
        /// </summary>
        public ConfiguredComponent(string typeName, string componentName)
        {
            this.typeName              = typeName;
            this.requiredConfiguration = new TypeRequiredConfiguration(typeName);
            this.configuration         = new StandardConfiguration();
            this.matchedName           = componentName;
        }

        /// <summary>
        /// Constructs from XML
        /// </summary>
        /// <param name="typeName"></param>
        public ConfiguredComponent(XmlNode node)
        {
            string typeName            = (node as XmlElement).GetAttribute("Type");
            this.typeName              = typeName;
            this.matchedName           = (node as XmlElement).GetAttribute("Id");
            this.configuration         = new StandardConfiguration(node["Parameters"]);
            this.requiredConfiguration = new TypeRequiredConfiguration(typeName);

            XmlElement element = node as XmlElement;
            if (element.HasAttribute("MatchAgainstType"))
            {
                this.matchAgainstType = Boolean.Parse(element.GetAttribute("MatchAgainstType"));
            }
            if (element.HasAttribute("MatchAgainstName"))
            {
                this.matchAgainstName = Boolean.Parse(element.GetAttribute("MatchAgainstName"));
            }
        }

        /// <summary>
        /// Constructs from a type name and a provided configuration
        /// </summary>
        public ConfiguredComponent(string typeName, IComponentConfiguration configProvided)
        {
            this.typeName              = typeName;
            this.configuration         = configProvided;
            this.requiredConfiguration = new TypeRequiredConfiguration(typeName);
        }

        /// <summary>
        /// Constructs from required configuration
        /// </summary>
        public ConfiguredComponent(IRequiresConfiguration configRequired)
        {
            this.matchedTypes          = new string[0];
            this.requiredConfiguration = configRequired;
            this.configuration         = new StandardConfiguration();
        }

        /// <summary>
        /// Creates a new configured component from required configuration and provided configuration
        /// </summary>
        public ConfiguredComponent(IRequiresConfiguration configRequired, IComponentConfiguration configProvided)
        {
            this.matchedTypes          = new string[0];
            this.requiredConfiguration = configRequired;
            this.configuration         = configProvided;
        }
        #endregion

        #region Helpers
        public static T ReconfigureInstance<T>(T instance, IComponentDirectory directory)
        {
            IRequiresConfiguration required = new TypeRequiredConfiguration(typeof(T), instance);
            IComponentConfiguration config  = new StandardConfiguration();

            return (T) ConfigureComponentWithRequirements(required, config, directory);
        }

        public static T ReconfigureInstance<T>(T instance, Type type, IComponentDirectory directory)
        {
            IRequiresConfiguration required = new TypeRequiredConfiguration(type, instance);
            IComponentConfiguration config  = new StandardConfiguration();

            return (T) ConfigureComponentWithRequirements(required, config, directory);
        }

        public static T ReconfigureInstance<T>(T instance, IComponentDirectory directory, IComponentConfiguration config)
        {
            IRequiresConfiguration required = new TypeRequiredConfiguration(typeof(T), instance);

            return (T)ConfigureComponentWithRequirements(required, config, directory);
        }

        public static T ReconfigureInstance<T>(T instance, Type type, IComponentDirectory directory, IComponentConfiguration config)
        {
            IRequiresConfiguration required = new TypeRequiredConfiguration(type, instance);

            return (T)ConfigureComponentWithRequirements(required, config, directory);
        }
        #endregion
    }
}