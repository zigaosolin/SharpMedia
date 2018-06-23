using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;

namespace SharpMedia.Tools.Parameters
{

    /// <summary>
    /// An enumerator tool parameter.
    /// </summary>
    public class Enumerator : IToolParameter
    {
        #region Private Members
        UIAttribute attribute;
        Type enumType;
        string name;
        object value;
        #endregion

        #region Public Members

        public Enumerator(UIAttribute attribute, Type enumType, string name)
        {
            this.attribute = attribute;
            this.enumType = enumType;
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
            string[] names = Enum.GetNames(enumType);

            value = Enum.Parse(enumType, sourceString);
        }

        public void Apply(IComponentDirectory toolDirectory)
        {
            // We apply as enum.
            toolDirectory.Register(new Instance(value, name));
        }

        public string[] PossibleValuesHint
        {
            get { return Enum.GetNames(enumType); }
        }

        public bool AcceptsOnlyHintedValues
        {
            get { return true; }
        }

        public bool IsSet
        {
            get { return value != null; }
        }


        #endregion
    }
}
