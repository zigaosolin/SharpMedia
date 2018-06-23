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
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;

namespace SharpMedia.Tools.Parameters
{
    /// <summary>
    /// Basic value type.
    /// </summary>
    /// <typeparam name="BoxedType">A type to cache/parse</typeparam>
    public class Basic : IToolParameter
    {
        #region Private Members
        UIAttribute attribute;
        Type parseType;
        string name;
        object value;
        #endregion

        #region Public Members

        public Basic(UIAttribute attribute, Type parseType, string name)
        {
            this.attribute = attribute;
            this.parseType = parseType;
            this.name = name;
        }

        public override string ToString()
        {
            if (value != null)
            {
                return name + " = " + value.ToString();
            }
            return string.Empty;
        }

        #endregion

        #region IToolParameter Members

        public string Name
        {
            get { return name; }
        }

        public UIAttribute Attribute
        {
            get { return attribute; }
        }

        public void Parse(string sourceString)
        {
            MethodInfo info = parseType.GetMethod("Parse", BindingFlags.Static);
            if (info == null)
            {
                if (parseType == typeof(string))
                {
                    value = sourceString;
                    return;
                }
                else
                {
                    throw new Exception("The 'basic' type must have a parse method.");
                }
            }

            value = info.Invoke(null, new object[] { sourceString });
        }

        public void Apply(IComponentDirectory toolDirectory)
        {
            toolDirectory.Register(new Instance(value, name)); 
        }

        public string[] PossibleValuesHint
        {
            get { return new string[0]; }
        }

        public bool AcceptsOnlyHintedValues
        {
            get { return false; }
        }

        public bool IsSet
        {
            get { return value != null; }
        }

        #endregion
    }
}
