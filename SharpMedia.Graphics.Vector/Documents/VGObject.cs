using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Graphics.Vector.Documents
{

    /// <summary>
    /// A Vector Graphics's object.
    /// </summary>
    public abstract class VGObject : IComponentProvider
    {
        #region Private Members
        string name;
        #endregion

        #region Constructors

        /// <summary>
        /// Unnamed object contructor.
        /// </summary>
        public VGObject()
        {
        }

        /// <summary>
        /// Constructor with name.
        /// </summary>
        /// <param name="name"></param>
        public VGObject(string name)
        {
            this.name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtains the name of object.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The underlaying object, where properties can be bound to.
        /// </summary>
        public abstract object UnderlayingObject
        {
            get;
        }

        #endregion

        #region IComponentProvider Members

        object IComponentProvider.GetInstance(IComponentDirectory componentDirectory, 
            object clientInstance, string requirementName, string requirementType)
        {
            // For now just return this, configuration may come later.
            return this;
        }

        bool IComponentProvider.MatchAgainstNameAllowed
        {
            get { return true; }
        }

        bool IComponentProvider.MatchAgainstTypeAllowed
        {
            get { return false; }
        }

        string IComponentProvider.MatchedName
        {
            get { return name; }
        }

        string[] IComponentProvider.MatchedTypes
        {
            get { return new string[0]; }
        }

        #endregion
    }
}
