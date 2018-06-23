using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Scene
{
    /// <summary>
    /// The visibility scope of scene component.
    /// </summary>
    public enum VisibilityScope
    {
        User,
        Runtime
    }

    /// <summary>
    /// A scene component attribute, used to costumize scene component binding
    /// and verification.
    /// </summary>
    /// <remarks>This is placed in core library since libraries that do not depend
    /// on Scene library may indeed implement components.</remarks>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface|AttributeTargets.Struct,
        AllowMultiple=false, Inherited=true)]
    public sealed class SceneComponentAttribute : Attribute
    {
        #region Private Members
        Type queryType = null;
        uint maxInstanceCount = 1;
        VisibilityScope scope = VisibilityScope.User;
        #endregion

        #region Constructors

        /// <summary>
        /// The default contructor; component is a singleton.
        /// </summary>
        public SceneComponentAttribute()
        {
            this.maxInstanceCount = 1;
        }

        /// <summary>
        /// The constructor with instance count.
        /// </summary>
        /// <param name="maxInstanceCount"></param>
        public SceneComponentAttribute(uint maxInstanceCount)
        {
            this.maxInstanceCount = maxInstanceCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Maximum number of instances.
        /// </summary>
        public uint MaxInstanceCount
        {
            get { return maxInstanceCount; }
        }

        /// <summary>
        /// The query type of component - if null, it targets to component type.
        /// </summary>
        public Type QueryType
        {
            set 
            {
                queryType = value;
            }
            get
            {
                return queryType;
            }
        }



        /// <summary>
        /// The visibility scope of scene component.
        /// </summary>
        public VisibilityScope VisibilityScope
        {
            get { return scope; }
            set { scope = value; }
        }

        #endregion

    }
}
