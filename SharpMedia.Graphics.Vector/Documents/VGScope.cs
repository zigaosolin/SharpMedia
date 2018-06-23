using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Graphics.Vector.Documents
{

    /// <summary>
    /// Scope is a base class for all scoped objects.
    /// </summary>
    public class VGScope : VGObject, IComponentDirectory
    {
        #region Private Members
        SortedList<string, VGObject> mapping = new SortedList<string, VGObject>();
        #endregion

        #region Overrides

        public override object UnderlayingObject
        {
            get { return this; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// A VG scope.
        /// </summary>
        /// <param name="name"></param>
        public VGScope(string name)
            : base(name)
        {
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Obtains a mapped object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public VGObject this[string name]
        {
            get { return mapping[name]; }
        }

        #endregion

        #region IComponentDirectory Members

        public object ConfigureInlineComponent(IComponentProvider cloneComponentDefinition)
        {
            return cloneComponentDefinition.GetInstance(this, null, string.Empty, string.Empty);
        }

        public IComponentProvider[] FindByName(string name)
        {
            // The names must be unique in this provider!
            VGObject provider;
            if (mapping.TryGetValue(name, out provider))
            {
                return new IComponentProvider[] { provider };
            }
            return new IComponentProvider[0];
        }

        public IComponentProvider[] FindByType(string type)
        {
            throw new ComponentSecurityException();
        }

        public T GetInstance<T>()
        {
            throw new ComponentSecurityException();
        }

        public object GetInstance(string name)
        {
            IComponentProvider[] components = FindByName(name);
            if (components.Length >= 1)
            {
                return components[0].GetInstance(this, null, string.Empty, string.Empty);
            }
            else
            {
                ComponentLookupException.NameNotFound(name);
            }
            return null;
        }

        public object GetInstanceByType(string typeName)
        {
            throw new ComponentSecurityException();
        }

        public void Register(IComponentProvider provider)
        {
            if (!(provider is VGObject))
            {
                throw new ComponentSecurityException("VG scope only supports VGObjects as component providers.");
            }

            // An object is created.
            VGObject obj = provider as VGObject;

            // May need to check for empty/null string and duplicates.
            mapping.Add(obj.Name, obj);
            
        }

        public IComponentProvider[] RegisteredProviders
        {
            get
            {
                IComponentProvider[] providers = new IComponentProvider[mapping.Values.Count];
                for (int i = 0; i < providers.Length; i++)
                {
                    providers[i] = mapping.Values[i];
                }

                return providers;
            }
        }

        public void UnRegister(IComponentProvider provider)
        {
            if (provider is VGObject)
            {
                int index = mapping.IndexOfValue(provider as VGObject);
                if(index >= 0)
                    mapping.RemoveAt(index);
            }

            // FIXME: ignore non VG objects?
        }

        #endregion

        #region ILibraryDirectory Members

        public IComponentProvider LoadFromLibrary(string libName, string componentName, bool throwOnFailure)
        {
            throw new NotImplementedException();
        }

        public SharpMedia.Components.Database.LibraryDesc LoadLibrary(string libName, bool throwOnFailure)
        {
            throw new NotImplementedException();
        }

        public SharpMedia.Components.Database.LibraryDesc[] LoadedLibraries
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
