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

namespace SharpMedia.Components.Configuration.ComponentProviders
{
    /// <summary>
    /// Serves already made instances
    /// </summary>
    public class Instance : MarshalByRefObject, IComponentProvider
    {
        #region Private
        object   instance;
        string[] types = null;
        string   name;
        bool matchWithName;
        bool matchWithTypes;
        #endregion

        #region IComponentProvider Members

        public object GetInstance(IComponentDirectory componentDirectory, object clientInstance, string requirementName, string requirementType)
        {
            return instance;
        }

        public string[] MatchedTypes
        {
            get { CacheTypes();  return types; }
        }

        private string ToString(Type t)
        {
            return t.FullName;
        }
        private void CacheTypes()
        {
            if (types != null) return;

            types = Array.ConvertAll<Type, string>(Common.SupportedCasts(instance.GetType(), false), ToString);
        }

        public string MatchedName
        {
            get { return name; }
        }

        public bool MatchAgainstNameAllowed
        {
            get { return matchWithName; }
        }

        public bool MatchAgainstTypeAllowed
        {
            get { return matchWithTypes; }
        }

        #endregion

        #region Constructors

        static string ToString(object obj)
        {
            return obj.ToString();
        }

        public Instance(object obj)
        {
            this.instance       = obj;
            this.types          = Array.ConvertAll<Type, string>(Common.SupportedCasts(obj.GetType(), false), ToString);
            this.matchWithTypes = true;
            this.matchWithName  = false;
        }

        public Instance(object obj, string[] types)
        {
            this.instance       = obj;
            this.types          = types.Clone() as string[];
            this.matchWithTypes = true;
            this.matchWithName  = false;
        }

        public Instance(object obj, string name)
        {
            this.instance = obj;
            this.name = name;
            this.matchWithTypes = true;
            this.matchWithName = true;
        }

        public Instance(object obj, string name, bool matchAgainstTypes, bool matchAgainstNames)
        {
            this.instance       = obj;
            this.name           = name;
            this.matchWithTypes = matchAgainstTypes;
            this.matchWithName  = matchAgainstNames;
        }

        public Instance(object obj, string name, string[] types)
        {
            this.instance = obj;
            this.name     = name;
            this.types    = types.Clone() as string[];
        }

        #endregion
    }
}
