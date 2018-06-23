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
using System.Text.RegularExpressions;
using SharpMedia.Components.Database;
using SharpMedia.Components.Configuration.ComponentProviders;
using System.Runtime.CompilerServices;
using SharpMedia.Database;

namespace SharpMedia.Components.Configuration
{
    /// <summary>
    /// A Component Directory implementation
    /// </summary>
    public class ComponentDirectory : 
        MarshalByRefObject, IComponentDirectory, IComponentAccessSecurity, ImportExportControl
    {
        #region Private
        IComponentDirectory parent;
        string namespaceName;

        Dictionary<string, List<IComponentProvider>> type2Component = new Dictionary<string, List<IComponentProvider>>();
        Dictionary<string, List<IComponentProvider>> name2Component = new Dictionary<string, List<IComponentProvider>>();

        // -- security --
        List<Regex> disallowNameReturn = new List<Regex>();
        List<Regex> disallowNameParentLookup = new List<Regex>();
        List<Regex> disallowTypeReturn = new List<Regex>();
        List<Regex> disallowTypeParentLookup = new List<Regex>();
        List<Regex> allowNameReturn = new List<Regex>();
        List<Regex> allowNameParentLookup = new List<Regex>();
        List<Regex> allowTypeReturn = new List<Regex>();
        List<Regex> allowTypeParentLookup = new List<Regex>();

        private bool AllowLookupByName(string name, out bool allowParent)
        {
            bool allowed = true;
            allowParent = true;
            foreach (Regex r in disallowNameReturn)
                if (r.IsMatch(name)) { allowed = false; break; }
            foreach (Regex r in disallowNameParentLookup)
                if (r.IsMatch(name)) { allowParent = false; break; }
            foreach (Regex r in allowNameReturn)
                if (r.IsMatch(name)) { allowed = true; break; }
            foreach (Regex r in allowNameParentLookup)
                if (r.IsMatch(name)) { allowParent = true; break; }

            return allowed;
        }


        private bool AllowLookupByType(string type, out bool allowParent)
        {
            bool allowed = true;
            allowParent = true;
            foreach (Regex r in disallowTypeReturn)
                if (r.IsMatch(type)) { allowed = false; break; }
            foreach (Regex r in disallowTypeParentLookup)
                if (r.IsMatch(type)) { allowParent = false; break; }
            foreach (Regex r in allowTypeReturn)
                if (r.IsMatch(type)) { allowed = true; break; }
            foreach (Regex r in allowTypeParentLookup)
                if (r.IsMatch(type)) { allowParent = true; break; }

            return allowed;
        }

        #endregion

        #region IComponentDirectory Members

        static readonly IComponentProvider[] nullComponents = new IComponentProvider[0];

        public IComponentProvider[] RegisteredProviders
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get 
            {
                List<IComponentProvider> providers = new List<IComponentProvider>();
                foreach (KeyValuePair<string, List<IComponentProvider>> xprov in name2Component)
                {
                    foreach (IComponentProvider prov in xprov.Value)
                    {
                        if (!providers.Contains(prov)) providers.Add(prov);
                    }
                }

                foreach (KeyValuePair<string, List<IComponentProvider>> xprov in type2Component)
                {
                    foreach (IComponentProvider prov in xprov.Value)
                    {
                        if (!providers.Contains(prov)) providers.Add(prov);
                    }
                }

                return providers.ToArray();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IComponentProvider[] FindByName(string name)
        {
            bool allowParent;
            if (!AllowLookupByName(name, out allowParent))
            {
                throw new ComponentSecurityException();
            }

            if (name2Component.ContainsKey(name))
            {
                if (name2Component[name].Count == 0)
                {
                    name2Component.Remove(name);
                }
                else return name2Component[name].ToArray();
            }

            /* check the libraries */
            if (name.Contains("."))
            {
                IComponentProvider provider = LoadFromLibrary(name.Substring(0, name.LastIndexOf('.')), name.Substring(name.LastIndexOf('.') + 1), false);

                if (provider == null) return nullComponents;
                else return new IComponentProvider[] { provider };
            }

            if (allowParent && parent != null)
            {
                return parent.FindByName(name);
            }

            /* NOT FOUND */
            return nullComponents;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IComponentProvider[] FindByType(string type)
        {
            bool allowParent;
            if (!AllowLookupByType(type, out allowParent))
            {
                throw new ComponentSecurityException();
            }

            if (type2Component.ContainsKey(type))
            {
                if (type2Component[type].Count == 0)
                {
                    type2Component.Remove(type);
                }
                else return type2Component[type].ToArray();
            }

            if (allowParent && parent != null)
            {
                return parent.FindByType(type);
            }

            /* NOT FOUND */
            return nullComponents;
        }

        private void Dump()
        {
            foreach (string key in type2Component.Keys)
            {
                Console.Out.WriteLine("{1}x {0}", key, type2Component[key].Count);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Register(IComponentProvider provider)
        {
            // Console.ForegroundColor = ConsoleColor.Green;
            // Console.Out.WriteLine("--- ICD.Register: {0}", provider.MatchedName);
            // Console.Out.WriteLine("    ByName? {0}, ByType? {1}", provider.MatchAgainstNameAllowed, provider.MatchAgainstTypeAllowed);
            // Console.Out.WriteLine("    Types:\n      {0}\n", String.Join("\n      ", provider.MatchedTypes));
            // Console.ResetColor();
            if (provider.MatchAgainstNameAllowed)
            {
                if (!name2Component.ContainsKey(provider.MatchedName))
                {
                    name2Component[provider.MatchedName] = new List<IComponentProvider>();
                }
                name2Component[provider.MatchedName].Add(provider);
            }
            if (provider.MatchAgainstTypeAllowed)
            {
                foreach (string type in provider.MatchedTypes)
                {
                    if (!type2Component.ContainsKey(type))
                    {
                        type2Component[type] = new List<IComponentProvider>();
                    }
                    type2Component[type].Add(provider);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UnRegister(IComponentProvider provider)
        {
            if (provider.MatchAgainstNameAllowed && name2Component.ContainsKey(provider.MatchedName))
            {
                name2Component[provider.MatchedName].Remove(provider);
            }
            if (provider.MatchAgainstTypeAllowed)
            {
                foreach (string type in provider.MatchedTypes)
                {
                    if (type2Component.ContainsKey(type))
                    {
                        type2Component[type].Remove(provider);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public object GetInstance(string name)
        {
            IComponentProvider[] providers = FindByName(name);
            if (providers.Length == 0)
                ComponentLookupException.NameNotFound(name);

            return providers[0].GetInstance(this, null, string.Empty, string.Empty);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public object GetInstanceByType(string typeName)
        {
            IComponentProvider[] providers = FindByType(typeName);
            if (providers.Length == 0)
                ComponentLookupException.NameNotFound(typeName);

            return providers[0].GetInstance(this, null, string.Empty, typeName);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public T GetInstance<T>()
        {
            return (T)GetInstanceByType(typeof(T).FullName);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public object ConfigureInlineComponent(IComponentProvider cloneComponentDefinition)
        {
            return cloneComponentDefinition.GetInstance(this, null, string.Empty, string.Empty);
        }

        #endregion

        #region Constructors

        public ComponentDirectory()
        {
            Register(
                new ComponentProviders.Instance(this, "ComponentDirectory"));
        }

        public ComponentDirectory(IComponentDirectory parent, string myName)
            : this()
        {
            this.namespaceName = myName;
            this.parent = parent;
        }

        #endregion

        #region IComponentAccessSecurity Members

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetNameAccess(Regex name, bool allow, bool allowParentLookup)
        {
            // FIXME: delete all entries that we nullify
            if (allow)
            {
                allowNameReturn.Add(name);
            }
            else
            {
                disallowNameReturn.Add(name);
            }

            if (allowParentLookup)
            {
                allowNameParentLookup.Add(name);
            }
            else
            {
                disallowNameParentLookup.Add(name);
            }
        }

        public void SetTypeAccess(Regex type, bool allow, bool allowParentLookup)
        {
            // FIXME: delete all entries that we nullify
            if (allow)
            {
                allowTypeReturn.Add(type);
            }
            else
            {
                disallowTypeReturn.Add(type);
            }

            if (allowParentLookup)
            {
                allowTypeParentLookup.Add(type);
            }
            else
            {
                disallowTypeParentLookup.Add(type);
            }
        }

        #endregion

        #region ILibraryDirectory Members

        Dictionary<string, LibraryDesc> partiallyLoadedLibraries = new Dictionary<string, LibraryDesc>();
        Dictionary<string, LibraryDesc> fullyLoadedLibraries = new Dictionary<string, LibraryDesc>();

        public LibraryDesc[] LoadedLibraries
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                List<LibraryDesc> libs = new List<LibraryDesc>();
                foreach (LibraryDesc lib in fullyLoadedLibraries.Values) { libs.Add(lib); }
                return libs.ToArray();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IComponentProvider LoadFromLibrary(string libName, string componentName, bool throwOnFailure)
        {
            // Console.Out.WriteLine("LoadFromLibrary: {0} -> {1}", libName, componentName);
            string fullName = libName + "." + componentName;

            if (partiallyLoadedLibraries.ContainsKey(libName))
            {
                /* check if it's loaded */
                if (this.FindByName(fullName) != null)
                {
                    return this.FindByName(fullName)[0];
                }
            }
            else if (fullyLoadedLibraries.ContainsKey(libName))
            {
                return this.FindByName(fullName)[0];
            }
            else
            {
                /* it's not even partially loaded, so do a partial load */
                LibraryDesc lib = PartialLoadLibrary(libName, throwOnFailure);

                /* if above doesn't throw, returning null is assumed */
                if (lib == null) return null;

                foreach (IComponentProvider provider in lib.Components)
                {
                    if (!provider.MatchAgainstNameAllowed) continue;

                    if (provider.MatchedName == componentName)
                    {
                        /* register lib */
                        partiallyLoadedLibraries[libName] = lib;

                        /* create name redirect */
                        IComponentProvider newProvider = new NameOverride(provider, fullName);
                        Register(newProvider);

                        return newProvider;
                    }
                }

                if (throwOnFailure)
                {
                    throw new Exception(
                        String.Format("Library {0} do not contain component {1}", libName, componentName));
                }
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public LibraryDesc LoadLibrary(string libName, bool throwOnFailure)
        {
            if (fullyLoadedLibraries.ContainsKey(libName))
            {
                return fullyLoadedLibraries[libName];
            }
            if (partiallyLoadedLibraries.ContainsKey(libName))
            {
                /* fully load partially loaded library */
                foreach (IComponentProvider provider in partiallyLoadedLibraries[libName].Components)
                {
                    if (!provider.MatchAgainstNameAllowed) continue;

                    string fullName = libName + "." + provider.MatchedName;
                    if (FindByName(fullName).Length == 0)
                    {
                        Register(new NameOverride(provider, fullName));
                    }
                }

                /* now fully loaded */
                LibraryDesc lib = partiallyLoadedLibraries[libName];
                partiallyLoadedLibraries.Remove(libName);
                fullyLoadedLibraries[libName] = lib;

                return lib;
            }

            LibraryDesc libFull = PartialLoadLibrary(libName, throwOnFailure);
            foreach (IComponentProvider provider in libFull.Components)
            {
                if (!provider.MatchAgainstNameAllowed) continue;
                string fullName = libName + "." + provider.MatchedName;
                Register(new NameOverride(provider, fullName));
            }

            fullyLoadedLibraries[libName] = libFull;

            return libFull;
        }

        static readonly string[] searchPaths = new string[]{
            "/System/Libraries/"
        };

        [MethodImpl(MethodImplOptions.Synchronized)]
        private LibraryDesc PartialLoadLibrary(string libName, bool throwOnFailure)
        {
            DatabaseManager dbmgr = GetInstance<DatabaseManager>();
            if (dbmgr == null)
            {
                if (throwOnFailure) throw new Exception("Database manager required to load libraries");
                return null;
            }

            foreach (string path in searchPaths)
            {
                Node<object> libNode = dbmgr.Find(path + libName.Replace('.','/'));
                if (libNode != null && libNode.TypedStreamExists<LibraryDesc>())
                {
                    return libNode.ObjectOfType<LibraryDesc>();
                }
            }

            if (throwOnFailure)
            {
                throw new Exception(
                    String.Format(
                        "Library {0} was not found and was not loaded", libName));
            }
            return null;
        }

        #endregion

        #region ImportExportControl Members

        ImportPolicy importPolicy = ImportPolicy.Full;
        public ImportPolicy ImportPolicy
        {
            get
            {
                return importPolicy;
            }
            set
            {
                importPolicy = value;
            }
        }

        IComponentDirectory machineDirectory = null;

        /// <summary>
        /// Used for cross-machine registration
        /// </summary>
        public IComponentDirectory MachineDirectory
        {
            get { return machineDirectory; }
            set { machineDirectory = value; }
        }

        public void Register(IComponentProvider provider, ExportScope scope, ProcessExitBehaviour onExit)
        {
            if (scope == ExportScope.Process)
            {
                /** register locally **/
                Register(provider);
            }
            else if (scope == ExportScope.Parent)
            {
                parent.Register(provider);
            }
            else if (scope == ExportScope.Machine)
            {
                if (machineDirectory != null)
                {
                    machineDirectory.Register(provider);
                }
                else
                {
                    throw new NotSupportedException("Component directory does not support machine scope registration");
                }
            }

            throw new NotSupportedException("Component directory does not support cluster scope registration");
        }

        #endregion
    }
}