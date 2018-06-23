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

namespace SharpMedia.Components.Configuration.ConfigurationValues
{
    /// <summary>
    /// A simple verbatim configuration value - mainly for value types
    /// </summary>
    [Serializable]
    public class Simple : IConfigurationValue
    {
        object value;

        static Type[] stringTypes = new Type[] { typeof(string) };

        /// <summary>
        /// Converts a string to an object instance
        /// </summary>
        /// <param name="t">Destiantion Type</param>
        /// <param name="sourceString">String value</param>
        /// <returns>Specified object instance or the original string if the conversion failed</returns>
        /// <remarks>if there is a cast after a call to this method, this will invoke the standard 
        /// casting work, and fail with an <see cref="IllegalCastException"/></remarks>
        public static object StringToType(Type t, string sourceString)
        {
            if (Common.IsTypeSameOrDerived(typeof(Enum), t))
            {
                return Enum.Parse(t, sourceString);
            }

            ConstructorInfo constructor = t.GetConstructor(stringTypes);
            if (constructor != null)
            {
                return constructor.Invoke(new object[] { sourceString });
            }

            MethodInfo method =
                t.GetMethod(
                    "Parse",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder,
                    stringTypes, null);

            if (method != null)
            {
                return method.Invoke(null, new object[] { sourceString });
            }

            return sourceString;
        }

        /// <summary>
        /// Converts a string to an object instance
        /// </summary>
        /// <param name="destinationType">Full name of the type</param>
        /// <param name="value">String value</param>
        /// <returns>Specified object instance or the original string if the conversion failed</returns>
        public static object StringToType(string destinationType, string value)
        {
            Type t = Type.GetType(destinationType);

            return StringToType(t, value);
        }


        #region IConfigurationValue Members

        public object GetInstance(string destinationType)
        {
            if (value.GetType() == typeof(string))
            {
                return StringToType(destinationType, (string)value);
            }

            return value;
        }

        #endregion

        public Simple(object value)
        {
            this.value = value;
        }
    }
}
