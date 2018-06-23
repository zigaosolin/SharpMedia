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
    /// Serves new instances from simple types
    /// </summary>
    [Serializable]
    public class SimpleClass : IComponentProvider
    {
        #region Private
        string className;

        string[] supportedTypes = new string[0];

        [NonSerialized]
        ConstructorInfo constructor = null;

        bool matchAgainstName;
        bool matchAgainstType;

        #endregion

        #region IComponentProvider Members

        public object GetInstance(IComponentDirectory componentDirectory, object clientInstance, string requirementName, string requirementType)
        {
            if (constructor == null)
            {
                constructor = Type.GetType(className).GetConstructor(Type.EmptyTypes);
                if (constructor == null)
                {
                    throw new Exception(
                        String.Format(
                            "SimpleClass provider: Type {0} does not provide a default constructor", className));
                }
            }

            return constructor.Invoke(null);
        }

        public string[] MatchedTypes
        {
            get { return supportedTypes; }
        }

        string name;
        public string MatchedName
        {
            get { return name; }
        }

        public bool MatchAgainstNameAllowed
        {
            get { return matchAgainstName; }
        }

        public bool MatchAgainstTypeAllowed
        {
            get { return matchAgainstType; }
        }

        #endregion

        #region Constructors

        public SimpleClass(string className)
        {
            this.className = className;
            this.supportedTypes = new string[] { className };
            this.matchAgainstType = true;
        }

        public SimpleClass(Type classType)
        {
            this.className = classType.FullName;
            this.supportedTypes = new string[] { classType.FullName };
            this.matchAgainstType = true;
        }

        public SimpleClass(string className, string[] supportedTypes)
        {
            this.className = className;
            this.supportedTypes = supportedTypes.Clone() as string[];
            this.matchAgainstType = true;
        }

        public SimpleClass(string className, string[] supportedTypes, string name)
        {
            this.className = className;
            this.supportedTypes = supportedTypes.Clone() as string[];
            this.name = name;
            this.matchAgainstName = true;
            this.matchAgainstType = true;
        }

        public SimpleClass(Type classType, string[] supportedTypes)
        {
            this.className = classType.FullName;
            this.supportedTypes = supportedTypes.Clone() as string[];
            this.matchAgainstType = true;
        }

        public SimpleClass(Type classType, string[] supportedTypes, string name)
        {
            this.className = classType.FullName;
            this.supportedTypes = supportedTypes.Clone() as string[];
            this.name = name;
            this.matchAgainstType = true;
            this.matchAgainstName = true;
        }

        #endregion
    }
}
