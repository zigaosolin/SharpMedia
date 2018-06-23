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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Shaders.Metadata
{

    /// <summary>
    /// A pin binder is base class for all pin types and their overloaded operations.
    /// </summary>
    public abstract class PinBinder
    {
        #region Common Private Members
        protected CodeGenerator generator;
        protected Pin pin;
        #endregion

        #region Properties

        /// <summary>
        /// A generator property.
        /// </summary>
        public CodeGenerator Generator
        {
            get
            {
                return generator;
            }
        }

        /// <summary>
        /// Generator of pin.
        /// </summary>
        public Pin Pin
        {
            get
            {
                return pin;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construction with generator.
        /// </summary>
        internal PinBinder([NotNull] CodeGenerator g, [NotNull] Pin p)
        {
            generator = g;
            pin = p;
        }

        /// <summary>
        /// Default contructor.
        /// </summary>
        internal PinBinder()
        {
        }

        #endregion

    }
}
