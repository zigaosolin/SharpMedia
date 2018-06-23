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
    /// A configuration value based on a class name
    /// </summary>
    [Serializable]
    public class ClassName : IConfigurationValue
    {
        #region IConfigurationValue Members

        public object GetInstance(string destinationType)
        {
            try
            {
                return Type.GetType(className).GetConstructor(
                    BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance,
                    Type.DefaultBinder, Type.EmptyTypes, null)
                        .Invoke(null);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Failed to ClassName construct: {0}", className);
                throw e;
            }
        }

        #endregion

        string className;
        /// <param name="cname">Name of the class</param>
        public ClassName(string cname)
        {
            this.className = cname;
        }
    }
}
