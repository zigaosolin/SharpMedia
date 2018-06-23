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
using SharpMedia.Components.Database;
using System.Reflection;
using SharpMedia.Database;

namespace SharpMedia.Components.Installation.Sources
{
    /// <summary>
    /// An installation source that casts to an Assembly
    /// </summary>
    [Serializable]
    public class AssemblySource : InstallSource
    {
        [NonSerialized]
        InstallEnvironment environment = null;

        private InstallSource child;
        /// <summary>
        /// Child source
        /// </summary>
        public InstallSource Child
        {
            get { return child; }
            set { child = value; }
        }

        bool unmanaged = false;
        /// <summary>
        /// Whether the assembly is unmanaged
        /// </summary>
        public string Unmanaged
        {
            get { return unmanaged.ToString(); }
            set { unmanaged = Boolean.Parse(value); }
        }

        [NonSerialized]
        SharpMedia.Components.Database.AssemblyDesc cachedAssembly = null;

        Type[] asmTypes = new Type[] { typeof(SharpMedia.Components.Database.AssemblyDesc) };

        #region InstallSource Members

        public Type[] Types
        {
            get { return asmTypes; }
        }

        public TypedStream<T> OpenForReading<T>()
        {
            if (typeof(T) != typeof(SharpMedia.Components.Database.AssemblyDesc))
            {
                throw new Exception("AssemblySource does not support type: " + typeof(T));
            }

            if (cachedAssembly != null)
            {
                return SingleValueTypedStream.Create(cachedAssembly).As<T>();
            }

            foreach (Type t in Child.Types)
            {
                if (t == typeof(byte[]))
                {
                    cachedAssembly = new SharpMedia.Components.Database.AssemblyDesc(
                        Child.OpenForReading<byte[]>().Object);
                    cachedAssembly.Unmanaged = unmanaged;

                    return OpenForReading<T>();
                }
            }

            throw new Exception("Cannot create Assembly from child types");
        }

        public TypedStream<object> OpenForReading(Type t)
        {
            if (t != typeof(SharpMedia.Components.Database.AssemblyDesc))
            {
                throw new Exception("AssemblySource does not support type: " + t);
            }

            if (cachedAssembly != null)
            {
                return SingleValueTypedStream.Create(cachedAssembly).As<object>();
            }

            foreach (Type tx in Child.Types)
            {
                if (tx == typeof(byte[]))
                {
                    cachedAssembly = new SharpMedia.Components.Database.AssemblyDesc(Child.OpenForReading<byte[]>().Object);
                    cachedAssembly.Unmanaged = unmanaged;
                    return OpenForReading(t);
                }
                else if (tx == typeof(SharpMedia.Components.Database.AssemblyDesc))
                {
                    cachedAssembly = new SharpMedia.Components.Database.AssemblyDesc(Child.OpenForReading<Assembly>().Object);
                    cachedAssembly.Unmanaged = unmanaged;
                    return OpenForReading(t);
                }
            }

            throw new Exception("Cannot create Assembly from available child types");
        }

        #endregion

        #region UsesInstallEnvironment Members

        public InstallEnvironment InstallEnvironment
        {
            set { environment = value; Child.InstallEnvironment = value; }
        }

        #endregion
    }
}
