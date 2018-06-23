using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;
using System.Reflection;

namespace SharpMedia.Components.Applications
{

    /// <summary>
    /// Auto parametrization facilities.
    /// </summary>
    public static class AutoParametrization
    {

        #region Auto Parametrization


        /// <summary>
        /// Extracts a value.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="value"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        private static IComponentProvider ExtractValue(Type targetType, string name, string value, IComponentDirectory directory)
        {
            // We first check if it is a link.
            if (value.Length == 0) return null;

            // We try to transform to specific type.
            return new Instance(Configuration.ConfigurationValues.Simple.StringToType(targetType, value), name, false, true);
        }

        /// <summary>
        /// Performs auto-parametrization on arbitary object.
        /// </summary>
        /// <param name="arguments">The arguments that will be matched with properties.</param>
        /// <param name="directory">The component directory, to resolve links. May be null.</param>
        public static string[] PerformAutoParametrization(Type targetType, IComponentDirectory directory, string[] arguments)
        {
            List<string> unprocessed = new List<string>();

            // We go for each.
            foreach (string argument in arguments)
            {
                // We check for '='.
                if (!argument.Contains("="))
                {
                    unprocessed.Add(argument);
                    continue;
                }

                // We split to two parts on this index.
                int index = argument.IndexOf('=');
                string first = argument.Substring(0, index).Trim();
                string last = argument.Substring(index + 1).Trim();

                // We try to find target's type property with that name to find the correct mask.
                Type maskType = typeof(string);

                // We extract correct type.
                // FIXME: also consider aliases or "case invariant" matching!
                PropertyInfo info = targetType.GetProperty(first);
                if (info != null && info.CanWrite) maskType = info.PropertyType;


                IComponentProvider provider = ExtractValue(maskType, first, last, directory);
                if (provider != null)
                {
                    directory.Register(provider);
                }
                else
                {
                    unprocessed.Add(argument);
                }



            }

            return unprocessed.ToArray(); ;
        }

        #endregion

    }
}
