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

namespace SharpMedia.Graphics.Shaders
{
    /// <summary>
    /// A parameter description class. It only holds name-pin pair and has no
    /// information about locality.
    /// </summary>
    public class ParameterDescription
    {
        #region Private Members
        string name;
        Pin pin;
        #endregion

        #region Constructor

        /// <summary>
        /// A parameter description constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pin"></param>
        public ParameterDescription([NotEmpty] string name, [NotNull] Pin pin)
        {
            this.name = name;
            this.pin = pin;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Creates a description based on previous that has deeper scope.
        /// </summary>
        public static ParameterDescription ScopeParameter([NotEmpty] string scope, [NotNull] ParameterDescription desc)
        {
            return new ParameterDescription(scope + "." + desc.Name, desc.Pin);
        }

        /// <summary>
        /// Creates a description based on previous that has deeper scope.
        /// </summary>
        public static ParameterDescription ScopeParameter([NotEmpty] string scope, int arrayIndex, [NotNull] ParameterDescription desc)
        {
            return new ParameterDescription(string.Format("{0}[{1}].{2}", scope, arrayIndex, desc.Name), desc.Pin);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Unique name of parameter.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Checks if parameter must be fixed.
        /// </summary>
        public bool IsFixed
        {
            get
            {
                // Fixed is required, if format is any of following.
                PinFormat format = pin.Format;
                switch (format)
                {

                    case PinFormat.Texture1D:
                    case PinFormat.Texture1DArray:
                    case PinFormat.Texture2D:
                    case PinFormat.Texture2DArray:
                    case PinFormat.TextureCube:
                    case PinFormat.Texture3D:
                    case PinFormat.Sampler:
                    case PinFormat.BufferTexture:
                    case PinFormat.Interface:
                        return true;
                }

                // Otherwise, it is "fixed" if it is dynamic array because size must be specified.
                return pin.IsDynamicArray;
            }
        }

        /// <summary>
        /// The pin element.
        /// </summary>
        public Pin Pin
        {
            get
            {
                return pin;
            }
        }

        /// <summary>
        /// The size of parameter in bytes.
        /// </summary>
        public uint SizeInBytes
        {
            get
            {
                return 0;
            }
        }

        #endregion
    }
}
